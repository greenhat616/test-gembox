namespace test_gemBox_ui
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rootPath = new TextBox();
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            label3 = new Label();
            outPath = new TextBox();
            button3 = new Button();
            copyRaw = new CheckBox();
            outPdf = new CheckBox();
            outTxt = new CheckBox();
            status = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // rootPath
            // 
            rootPath.Location = new Point(146, 46);
            rootPath.Name = "rootPath";
            rootPath.ReadOnly = true;
            rootPath.Size = new Size(565, 38);
            rootPath.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 49);
            label1.Name = "label1";
            label1.Size = new Size(128, 31);
            label1.TabIndex = 1;
            label1.Text = "Root Path";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(719, 41);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 2;
            button1.Text = "Select";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(719, 102);
            button2.Name = "button2";
            button2.Size = new Size(150, 46);
            button2.TabIndex = 6;
            button2.Text = "Select";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 110);
            label3.Name = "label3";
            label3.Size = new Size(117, 31);
            label3.TabIndex = 5;
            label3.Text = "Out Path";
            // 
            // outPath
            // 
            outPath.Location = new Point(146, 107);
            outPath.Name = "outPath";
            outPath.ReadOnly = true;
            outPath.Size = new Size(565, 38);
            outPath.TabIndex = 4;
            // 
            // button3
            // 
            button3.Location = new Point(12, 309);
            button3.Name = "button3";
            button3.Size = new Size(857, 111);
            button3.TabIndex = 7;
            button3.Text = "Process";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // copyRaw
            // 
            copyRaw.AutoSize = true;
            copyRaw.Checked = true;
            copyRaw.CheckState = CheckState.Checked;
            copyRaw.Location = new Point(12, 162);
            copyRaw.Name = "copyRaw";
            copyRaw.Size = new Size(194, 35);
            copyRaw.TabIndex = 8;
            copyRaw.Text = "Copy raw file";
            copyRaw.UseVisualStyleBackColor = true;
            copyRaw.CheckedChanged += copyRaw_CheckedChanged;
            // 
            // outPdf
            // 
            outPdf.AutoSize = true;
            outPdf.Checked = true;
            outPdf.CheckState = CheckState.Checked;
            outPdf.Location = new Point(246, 162);
            outPdf.Name = "outPdf";
            outPdf.Size = new Size(143, 35);
            outPdf.TabIndex = 9;
            outPdf.Text = "Out PDF";
            outPdf.UseVisualStyleBackColor = true;
            // 
            // outTxt
            // 
            outTxt.AutoSize = true;
            outTxt.Checked = true;
            outTxt.CheckState = CheckState.Checked;
            outTxt.Location = new Point(476, 162);
            outTxt.Name = "outTxt";
            outTxt.Size = new Size(132, 35);
            outTxt.TabIndex = 10;
            outTxt.Text = "Out Txt";
            outTxt.UseVisualStyleBackColor = true;
            // 
            // status
            // 
            status.AutoSize = true;
            status.Location = new Point(109, 259);
            status.Name = "status";
            status.Size = new Size(62, 31);
            status.TabIndex = 11;
            status.Text = "Idle.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 259);
            label2.Name = "label2";
            label2.Size = new Size(91, 31);
            label2.TabIndex = 12;
            label2.Text = "Status:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 430);
            Controls.Add(label2);
            Controls.Add(status);
            Controls.Add(outTxt);
            Controls.Add(outPdf);
            Controls.Add(copyRaw);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(outPath);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(rootPath);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox rootPath;
        private Label label1;
        private Button button1;
        private Button button2;
        private Label label3;
        private TextBox outPath;
        private Button button3;
        private CheckBox copyRaw;
        private CheckBox outPdf;
        private CheckBox outTxt;
        private Label status;
        private Label label2;
    }
}
