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
        GemBox.Document.ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = GemBox.Document.FreeLimitReachedAction.ContinueAsTrial;
        GemBox.Presentation.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        GemBox.Presentation.ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = GemBox.Presentation.FreeLimitReachedAction.ContinueAsTrial;
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

    private void button3_Click(object sender, EventArgs e)
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
        List<string> results = new();
        ScanDirectory(rootPath.Text, exts, results);
        status.Text = $"Scan complete; Found {results.Count} docs.";
        var outpath = outPath.Text;
        foreach (var item in results)
        {
            try
            {
                status.Text = $"Processing {item}";
                var stripped_item_path = item.Replace(rootPath.Text, "");
                var dirname = Path.GetDirectoryName(outpath + stripped_item_path);
                if (!Directory.Exists(dirname))
                {
                    Directory.CreateDirectory(dirname);
                }
                if (copyRaw.Checked)
                {
                    // copy the file
                    var out_file = outpath + stripped_item_path;
                    File.Copy(item, out_file, true);
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

                                    sb.AppendFormat("{0}{1}{2}", isBold ? "<b>" : "", text, isBold ? "</b>" : "");
                                }

                                sb.AppendLine();
                            }

                            sb.AppendLine("----------");
                        }

                        var out_file = outpath + stripped_item_path + ".txt";
                        File.WriteAllText(out_file, sb.ToString());
                    }

                    if (outPdf.Checked)
                    {
                        presentation.Save(outpath + stripped_item_path + ".pdf");
                    }
                }

                if (item.EndsWith(".pdf"))
                {
                    var document = GemBox.Document.DocumentModel.Load(item);
                    if (outTxt.Checked)
                    {
                        string text = document.Content.ToString();
                        var out_file = outpath + stripped_item_path + ".txt";
                        File.WriteAllText(out_file, text);
                    }
                }

                if (item.EndsWith(".doc") || item.EndsWith(".docx"))
                {
                    var document = GemBox.Document.DocumentModel.Load(item);
                    if (outTxt.Checked)
                    {
                        string text = document.Content.ToString();
                        var out_file = outpath + stripped_item_path + ".txt";
                        File.WriteAllText(out_file, text);
                    }

                    if (outPdf.Checked)
                    {
                        document.Save(outpath + stripped_item_path + ".pdf");
                    }
                }

                status.Text = $"Processed {item}";
            }
            catch (GemBox.Presentation.FreeLimitReachedException)
            {
                status.Text = "reach free limit, skipped.";
            }
            catch (GemBox.Document.FreeLimitReachedException)
            {
                status.Text = "reach free limit, skipped.";
            }
        }

        status.Text = "Done!";
        button3.Enabled = true;
    }


    private void ScanDirectory(string directory, string[] extensions, List<string> results)
    {
        try
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                var extension = Path.GetExtension(file).ToLower();

                if (Array.Exists(extensions, ext => ext.ToLower() == extension))
                {
                    status.Text = $"Found {file}, continue scanning...";
                    results.Add(file);
                }
            }

            foreach (var subDir in Directory.GetDirectories(directory)) ScanDirectory(subDir, extensions, results);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing {directory}: {ex.Message}");
        }
    }
}