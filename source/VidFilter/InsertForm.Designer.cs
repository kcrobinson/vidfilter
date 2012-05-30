namespace VidFilter
{
    partial class InsertForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.FileLoadButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.FrameRateTextBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ResolutionHeightTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ResolutionWidthTextBox = new System.Windows.Forms.TextBox();
            this.InsertMovieButton = new System.Windows.Forms.Button();
            this.CloseFormButton = new System.Windows.Forms.Button();
            this.InsertStatusTextBox = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.InsertControl = new System.Windows.Forms.TabControl();
            this.InsertMovieTabPage = new System.Windows.Forms.TabPage();
            this.InsertColorspaceTabPage = new System.Windows.Forms.TabPage();
            this.InsertColorspaceButton = new System.Windows.Forms.Button();
            this.ColorsapceBitsPerPixelTextBox = new System.Windows.Forms.TextBox();
            this.ColorspaceNameTextBox = new System.Windows.Forms.TextBox();
            this.ColorspaceIdTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ColorspaceChannelCountTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.MovieColorspaceComboBox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.InsertControl.SuspendLayout();
            this.InsertMovieTabPage.SuspendLayout();
            this.InsertColorspaceTabPage.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.Location = new System.Drawing.Point(85, 9);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.Size = new System.Drawing.Size(344, 22);
            this.FilePathTextBox.TabIndex = 0;
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Location = new System.Drawing.Point(6, 12);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(73, 17);
            this.FilePathLabel.TabIndex = 1;
            this.FilePathLabel.Text = "Select File";
            // 
            // FileLoadButton
            // 
            this.FileLoadButton.Enabled = false;
            this.FileLoadButton.Location = new System.Drawing.Point(435, 9);
            this.FileLoadButton.Name = "FileLoadButton";
            this.FileLoadButton.Size = new System.Drawing.Size(75, 23);
            this.FileLoadButton.TabIndex = 2;
            this.FileLoadButton.Text = "Load";
            this.FileLoadButton.UseVisualStyleBackColor = true;
            this.FileLoadButton.Click += new System.EventHandler(this.FileLoadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Frame Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Resolution (pixels)";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.FrameRateTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 79);
            this.panel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "fps";
            // 
            // FrameRateTextBox
            // 
            this.FrameRateTextBox.Location = new System.Drawing.Point(6, 30);
            this.FrameRateTextBox.Name = "FrameRateTextBox";
            this.FrameRateTextBox.Size = new System.Drawing.Size(37, 22);
            this.FrameRateTextBox.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ResolutionHeightTextBox);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.ResolutionWidthTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(108, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(145, 79);
            this.panel2.TabIndex = 6;
            // 
            // ResolutionHeightTextBox
            // 
            this.ResolutionHeightTextBox.Location = new System.Drawing.Point(80, 29);
            this.ResolutionHeightTextBox.Name = "ResolutionHeightTextBox";
            this.ResolutionHeightTextBox.Size = new System.Drawing.Size(47, 22);
            this.ResolutionHeightTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "x";
            // 
            // ResolutionWidthTextBox
            // 
            this.ResolutionWidthTextBox.Location = new System.Drawing.Point(6, 30);
            this.ResolutionWidthTextBox.Name = "ResolutionWidthTextBox";
            this.ResolutionWidthTextBox.Size = new System.Drawing.Size(47, 22);
            this.ResolutionWidthTextBox.TabIndex = 5;
            // 
            // InsertMovieButton
            // 
            this.InsertMovieButton.Location = new System.Drawing.Point(439, 93);
            this.InsertMovieButton.Name = "InsertMovieButton";
            this.InsertMovieButton.Size = new System.Drawing.Size(75, 23);
            this.InsertMovieButton.TabIndex = 7;
            this.InsertMovieButton.Text = "Insert";
            this.InsertMovieButton.UseVisualStyleBackColor = true;
            this.InsertMovieButton.Click += new System.EventHandler(this.InsertMovieButton_Click);
            // 
            // CloseFormButton
            // 
            this.CloseFormButton.Location = new System.Drawing.Point(455, 282);
            this.CloseFormButton.Name = "CloseFormButton";
            this.CloseFormButton.Size = new System.Drawing.Size(75, 23);
            this.CloseFormButton.TabIndex = 8;
            this.CloseFormButton.Text = "Close";
            this.CloseFormButton.UseVisualStyleBackColor = true;
            this.CloseFormButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // InsertStatusTextBox
            // 
            this.InsertStatusTextBox.Location = new System.Drawing.Point(3, 24);
            this.InsertStatusTextBox.Multiline = true;
            this.InsertStatusTextBox.Name = "InsertStatusTextBox";
            this.InsertStatusTextBox.ReadOnly = true;
            this.InsertStatusTextBox.Size = new System.Drawing.Size(517, 73);
            this.InsertStatusTextBox.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.InsertStatusTextBox);
            this.panel3.Location = new System.Drawing.Point(12, 175);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(525, 101);
            this.panel3.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Insert Status";
            // 
            // InsertControl
            // 
            this.InsertControl.Controls.Add(this.InsertMovieTabPage);
            this.InsertControl.Controls.Add(this.InsertColorspaceTabPage);
            this.InsertControl.Location = new System.Drawing.Point(12, 12);
            this.InsertControl.Name = "InsertControl";
            this.InsertControl.SelectedIndex = 0;
            this.InsertControl.Size = new System.Drawing.Size(525, 157);
            this.InsertControl.TabIndex = 11;
            // 
            // InsertMovieTabPage
            // 
            this.InsertMovieTabPage.Controls.Add(this.panel4);
            this.InsertMovieTabPage.Controls.Add(this.FilePathLabel);
            this.InsertMovieTabPage.Controls.Add(this.FilePathTextBox);
            this.InsertMovieTabPage.Controls.Add(this.FileLoadButton);
            this.InsertMovieTabPage.Controls.Add(this.InsertMovieButton);
            this.InsertMovieTabPage.Controls.Add(this.panel1);
            this.InsertMovieTabPage.Controls.Add(this.panel2);
            this.InsertMovieTabPage.Location = new System.Drawing.Point(4, 25);
            this.InsertMovieTabPage.Name = "InsertMovieTabPage";
            this.InsertMovieTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.InsertMovieTabPage.Size = new System.Drawing.Size(517, 128);
            this.InsertMovieTabPage.TabIndex = 0;
            this.InsertMovieTabPage.Text = "Insert Movie";
            this.InsertMovieTabPage.UseVisualStyleBackColor = true;
            // 
            // InsertColorspaceTabPage
            // 
            this.InsertColorspaceTabPage.Controls.Add(this.InsertColorspaceButton);
            this.InsertColorspaceTabPage.Controls.Add(this.ColorsapceBitsPerPixelTextBox);
            this.InsertColorspaceTabPage.Controls.Add(this.ColorspaceNameTextBox);
            this.InsertColorspaceTabPage.Controls.Add(this.ColorspaceIdTextBox);
            this.InsertColorspaceTabPage.Controls.Add(this.label9);
            this.InsertColorspaceTabPage.Controls.Add(this.label8);
            this.InsertColorspaceTabPage.Controls.Add(this.label7);
            this.InsertColorspaceTabPage.Controls.Add(this.ColorspaceChannelCountTextBox);
            this.InsertColorspaceTabPage.Controls.Add(this.label6);
            this.InsertColorspaceTabPage.Location = new System.Drawing.Point(4, 25);
            this.InsertColorspaceTabPage.Name = "InsertColorspaceTabPage";
            this.InsertColorspaceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.InsertColorspaceTabPage.Size = new System.Drawing.Size(517, 128);
            this.InsertColorspaceTabPage.TabIndex = 1;
            this.InsertColorspaceTabPage.Text = "Insert Colorspace";
            this.InsertColorspaceTabPage.UseVisualStyleBackColor = true;
            // 
            // InsertColorspaceButton
            // 
            this.InsertColorspaceButton.Location = new System.Drawing.Point(439, 92);
            this.InsertColorspaceButton.Name = "InsertColorspaceButton";
            this.InsertColorspaceButton.Size = new System.Drawing.Size(75, 23);
            this.InsertColorspaceButton.TabIndex = 8;
            this.InsertColorspaceButton.Text = "Insert";
            this.InsertColorspaceButton.UseVisualStyleBackColor = true;
            this.InsertColorspaceButton.Click += new System.EventHandler(this.InsertColorpsaceButton_Click);
            // 
            // ColorsapceBitsPerPixelTextBox
            // 
            this.ColorsapceBitsPerPixelTextBox.Location = new System.Drawing.Point(108, 93);
            this.ColorsapceBitsPerPixelTextBox.Name = "ColorsapceBitsPerPixelTextBox";
            this.ColorsapceBitsPerPixelTextBox.Size = new System.Drawing.Size(29, 22);
            this.ColorsapceBitsPerPixelTextBox.TabIndex = 7;
            // 
            // ColorspaceNameTextBox
            // 
            this.ColorspaceNameTextBox.Location = new System.Drawing.Point(108, 4);
            this.ColorspaceNameTextBox.Name = "ColorspaceNameTextBox";
            this.ColorspaceNameTextBox.Size = new System.Drawing.Size(142, 22);
            this.ColorspaceNameTextBox.TabIndex = 6;
            // 
            // ColorspaceIdTextBox
            // 
            this.ColorspaceIdTextBox.Location = new System.Drawing.Point(108, 32);
            this.ColorspaceIdTextBox.Name = "ColorspaceIdTextBox";
            this.ColorspaceIdTextBox.Size = new System.Drawing.Size(142, 22);
            this.ColorspaceIdTextBox.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "Bits per Pixel";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "# of Channels";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Id Code";
            // 
            // ColorspaceChannelCountTextBox
            // 
            this.ColorspaceChannelCountTextBox.Location = new System.Drawing.Point(108, 62);
            this.ColorspaceChannelCountTextBox.Name = "ColorspaceChannelCountTextBox";
            this.ColorspaceChannelCountTextBox.Size = new System.Drawing.Size(29, 22);
            this.ColorspaceChannelCountTextBox.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Name";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.MovieColorspaceComboBox);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Location = new System.Drawing.Point(259, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(170, 79);
            this.panel4.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 17);
            this.label11.TabIndex = 3;
            this.label11.Text = "Colorspace";
            // 
            // MovieColorspaceComboBox
            // 
            this.MovieColorspaceComboBox.FormattingEnabled = true;
            this.MovieColorspaceComboBox.Location = new System.Drawing.Point(6, 30);
            this.MovieColorspaceComboBox.Name = "MovieColorspaceComboBox";
            this.MovieColorspaceComboBox.Size = new System.Drawing.Size(121, 24);
            this.MovieColorspaceComboBox.TabIndex = 4;
            // 
            // InsertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 312);
            this.Controls.Add(this.InsertControl);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.CloseFormButton);
            this.Name = "InsertForm";
            this.Text = "Insert a Record";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.InsertControl.ResumeLayout(false);
            this.InsertMovieTabPage.ResumeLayout(false);
            this.InsertMovieTabPage.PerformLayout();
            this.InsertColorspaceTabPage.ResumeLayout(false);
            this.InsertColorspaceTabPage.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox FilePathTextBox;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.Button FileLoadButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FrameRateTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox ResolutionHeightTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ResolutionWidthTextBox;
        private System.Windows.Forms.Button InsertMovieButton;
        private System.Windows.Forms.Button CloseFormButton;
        private System.Windows.Forms.TextBox InsertStatusTextBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl InsertControl;
        private System.Windows.Forms.TabPage InsertMovieTabPage;
        private System.Windows.Forms.TabPage InsertColorspaceTabPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ColorspaceChannelCountTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button InsertColorspaceButton;
        private System.Windows.Forms.TextBox ColorsapceBitsPerPixelTextBox;
        private System.Windows.Forms.TextBox ColorspaceNameTextBox;
        private System.Windows.Forms.TextBox ColorspaceIdTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox MovieColorspaceComboBox;
        private System.Windows.Forms.Label label11;
    }
}