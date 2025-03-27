using System.Diagnostics;
using System.Text;
using System.Windows.Controls;

namespace test_gemBox_ui;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        GemBox.Document.ComponentInfo.FreeLimitReached += (sender, e) =>
            e.FreeLimitReachedAction = GemBox.Document.FreeLimitReachedAction.ContinueAsTrial;
        GemBox.Presentation.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        GemBox.Presentation.ComponentInfo.FreeLimitReached += (sender, e) =>
            e.FreeLimitReachedAction = GemBox.Presentation.FreeLimitReachedAction.ContinueAsTrial;
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void copyRaw_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        var result = fbd.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
            if (Directory.Exists(fbd.SelectedPath))
                rootPath.Text = fbd.SelectedPath;
            else
                MessageBox.Show("Invalid Root Directory");
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        var result = fbd.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
            if (Directory.Exists(fbd.SelectedPath))
                outPath.Text = fbd.SelectedPath;
            else
                MessageBox.Show("Invalid Out Directory");
        }
    }

    private async void button3_Click(object sender, EventArgs e)
    {
        // check path
        if (string.IsNullOrWhiteSpace(rootPath.Text) || string.IsNullOrWhiteSpace(outPath.Text))
        {
            MessageBox.Show("rootPath and outPath must selected");
            return;
        }

        // check if rootPath exists 
        if (!Directory.Exists(rootPath.Text))
        {
            MessageBox.Show("Invalid Root Directory");
            return;
        }

        // check if outPath exists
        if (!Directory.Exists(outPath.Text))
        {
            MessageBox.Show("Invalid Out Directory");
            return;
        }

        button3.Enabled = false;

        string[] exts = { ".ppt", ".pptx", ".pdf", ".doc", ".docx" };

        status.Text = "Scanning...";
        var successItems = new List<string>();
        var failedItems = new List<string>();
        try
        {
            var outPathText = outPath.Text;
            var rootPathText = rootPath.Text;
            await Task.Run(async () =>
            {
                List<string> results = new();
                await ScanDirectory(rootPathText, exts, results);
                Invoke((MethodInvoker)delegate { status.Text = $"Found {results.Count} files, processing..."; });
                foreach (var item in results)
                    try
                    {
                        Invoke((MethodInvoker)delegate { status.Text = $"Processing {item}"; });
                        var stripped_item_path = item.Replace(rootPathText, "");
                        var fileName = Path.GetFileName(item);
                        var directoryName = Path.GetDirectoryName(outPathText + stripped_item_path);
                        if (directoryName is null || fileName is null)
                        {
                            MessageBox.Show("Invalid directory name, skipped.");
                            continue;
                        }

                        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

                        // if file path is hidden or tmp office file, skipped
                        if (fileName.StartsWith(".") || fileName.StartsWith("~")) continue;

                        if (copyRaw.Checked)
                        {
                            // copy the file
                            var out_file = outPathText + stripped_item_path;
                            await FileExtensions.CopyAsync(item, out_file, true);
                        }

                        if (item.EndsWith(".ppt") || item.EndsWith(".pptx"))
                        {
                            var presentation = GemBox.Presentation.PresentationDocument.Load(item);
                            if (outTxt.Checked)
                            {
                                var sb = new StringBuilder();

                                var slide = presentation.Slides[0];

                                foreach (var shape in slide.Content.Drawings.OfType<GemBox.Presentation.Shape>())
                                {
                                    sb.AppendFormat("Shape ShapeType={0}:", shape.ShapeType);
                                    sb.AppendLine();

                                    foreach (var paragraph in shape.Text.Paragraphs)
                                    {
                                        foreach (var run in paragraph.Elements.OfType<GemBox.Presentation.TextRun>())
                                        {
                                            var isBold = run.Format.Bold;
                                            var text = run.Text;

                                            sb.AppendFormat("{0}{1}{2}", isBold ? "<b>" : "", text,
                                                isBold ? "</b>" : "");
                                        }

                                        sb.AppendLine();
                                    }

                                    sb.AppendLine("----------");
                                }

                                var out_file = outPathText + stripped_item_path + ".txt";
                                await File.WriteAllTextAsync(out_file, sb.ToString());
                            }

                            if (outPdf.Checked)
                                await Task.Run(() => presentation.Save(outPathText + stripped_item_path + ".pdf"));
                        }

                        if (item.EndsWith(".pdf"))
                        {
                            var document = GemBox.Document.DocumentModel.Load(item);
                            if (outTxt.Checked)
                            {
                                var text = document.Content.ToString();
                                var out_file = outPathText + stripped_item_path + ".txt";
                                await File.WriteAllTextAsync(out_file, text);
                            }
                        }

                        if (item.EndsWith(".doc") || item.EndsWith(".docx"))
                        {
                            var document = GemBox.Document.DocumentModel.Load(item);
                            if (outTxt.Checked)
                            {
                                var text = document.Content.ToString();
                                var out_file = outPathText + stripped_item_path + ".txt";
                                File.WriteAllTextAsync(out_file, text);
                            }

                            if (outPdf.Checked)
                                await Task.Run(() => document.Save(outPathText + stripped_item_path + ".pdf"));
                        }

                        Invoke((MethodInvoker)delegate { status.Text = $"Processed {item}"; });
                        successItems.Add(item);
                    }
                    catch (GemBox.Presentation.FreeLimitReachedException)
                    {
                        failedItems.Add(item);
                    }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.ToString()}");
        }
        finally
        {
            status.Text = "Done!";
            button3.Enabled = true;
        }

        var tempDir = Path.GetTempPath();
        var tempFileName = $"tempfile_{Guid.NewGuid()}.txt";
        var tempFilePath = Path.Combine(tempDir, tempFileName);
        var sb = new StringBuilder();
        sb.AppendLine("Success Items:");
        foreach (var item in successItems) sb.AppendLine($"  {item}");

        sb.AppendLine("");
        sb.AppendLine("Failed Items:");
        foreach (var item in failedItems) sb.AppendLine($"  {item}");

        await File.WriteAllTextAsync(tempFilePath, sb.ToString());
        Process.Start(new ProcessStartInfo
        {
            FileName = tempFilePath,
            UseShellExecute = true
        });
    }


    private async Task ScanDirectory(string directory, string[] extensions, List<string> results)
    {
        try
        {
            var files = await Task.Run(() => Directory.GetFiles(directory));
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file).ToLower();

                if (Array.Exists(extensions, ext => ext.ToLower() == extension))
                {
                    Invoke((MethodInvoker)delegate { status.Text = $"Found {file}, continue scanning..."; });
                    results.Add(file);
                }
            }

            var dirs = await Task.Run(() => Directory.GetDirectories(directory));
            foreach (var subDir in dirs) await ScanDirectory(subDir, extensions, results);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error accessing {directory}: {ex.Message}");
        }
    }
}

public static class FileExtensions
{
    // 异步文件复制
    public static async Task CopyAsync(this string sourceFile, string destinationFile, bool overwrite)
    {
        using (var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                   FileOptions.Asynchronous))
        using (var destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write,
                   FileShare.None, 4096, FileOptions.Asynchronous))
        {
            await sourceStream.CopyToAsync(destinationStream);
        }
    }
}