namespace ListInsight.Windows
{
    partial class MainForm
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.scanProgressBar = new System.Windows.Forms.ProgressBar();
            this.openButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.White;
            this.infoLabel.Location = new System.Drawing.Point(9, 13);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(296, 17);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "Open a CSV file of your subscribers to begin";
            this.infoLabel.Click += new System.EventHandler(this.openButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            // 
            // scanProgressBar
            // 
            this.scanProgressBar.Location = new System.Drawing.Point(12, 38);
            this.scanProgressBar.Name = "scanProgressBar";
            this.scanProgressBar.Size = new System.Drawing.Size(302, 23);
            this.scanProgressBar.TabIndex = 2;
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 38);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 3;
            this.openButton.Text = "Open CSV...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 38);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(117, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save segment to...";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "gmail-users.csv";
            this.saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.ClientSize = new System.Drawing.Size(326, 75);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.scanProgressBar);
            this.Controls.Add(this.infoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Litmus";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ProgressBar scanProgressBar;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

