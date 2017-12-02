namespace AllegianceForms.Controls
{
    partial class TechTreeItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TimeProgress = new System.Windows.Forms.ProgressBar();
            this.TechName = new System.Windows.Forms.Label();
            this.InvestmentProgress = new System.Windows.Forms.ProgressBar();
            this.TechAmount = new System.Windows.Forms.Label();
            this.TechDuration = new System.Windows.Forms.Label();
            this.TechIcon = new System.Windows.Forms.PictureBox();
            this.Shortcut = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TechIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TimeProgress
            // 
            this.TimeProgress.ForeColor = System.Drawing.Color.Lime;
            this.TimeProgress.Location = new System.Drawing.Point(3, 65);
            this.TimeProgress.Name = "TimeProgress";
            this.TimeProgress.Size = new System.Drawing.Size(224, 11);
            this.TimeProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.TimeProgress.TabIndex = 6;
            this.TimeProgress.Value = 50;
            // 
            // TechName
            // 
            this.TechName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TechName.Location = new System.Drawing.Point(21, 2);
            this.TechName.Name = "TechName";
            this.TechName.Size = new System.Drawing.Size(180, 27);
            this.TechName.TabIndex = 5;
            this.TechName.Text = "[Description]";
            this.TechName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TechName.Click += new System.EventHandler(this.Name_Click);
            this.TechName.DoubleClick += new System.EventHandler(this.Name_DoubleClick);
            this.TechName.MouseEnter += new System.EventHandler(this.Name_MouseEnter);
            this.TechName.MouseLeave += new System.EventHandler(this.Name_MouseLeave);
            // 
            // InvestmentProgress
            // 
            this.InvestmentProgress.ForeColor = System.Drawing.Color.Gold;
            this.InvestmentProgress.Location = new System.Drawing.Point(3, 55);
            this.InvestmentProgress.Name = "InvestmentProgress";
            this.InvestmentProgress.Size = new System.Drawing.Size(224, 22);
            this.InvestmentProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.InvestmentProgress.TabIndex = 6;
            this.InvestmentProgress.Value = 50;
            // 
            // TechAmount
            // 
            this.TechAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TechAmount.Location = new System.Drawing.Point(2, 29);
            this.TechAmount.Name = "TechAmount";
            this.TechAmount.Size = new System.Drawing.Size(116, 25);
            this.TechAmount.TabIndex = 5;
            this.TechAmount.Text = "$[Amount]";
            this.TechAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TechAmount.Click += new System.EventHandler(this.Name_Click);
            this.TechAmount.DoubleClick += new System.EventHandler(this.Name_DoubleClick);
            this.TechAmount.MouseEnter += new System.EventHandler(this.Name_MouseEnter);
            this.TechAmount.MouseLeave += new System.EventHandler(this.Name_MouseLeave);
            // 
            // TechDuration
            // 
            this.TechDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TechDuration.Location = new System.Drawing.Point(116, 29);
            this.TechDuration.Name = "TechDuration";
            this.TechDuration.Size = new System.Drawing.Size(111, 25);
            this.TechDuration.TabIndex = 5;
            this.TechDuration.Text = "([Seconds])";
            this.TechDuration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TechDuration.Click += new System.EventHandler(this.Name_Click);
            this.TechDuration.DoubleClick += new System.EventHandler(this.Name_DoubleClick);
            this.TechDuration.MouseEnter += new System.EventHandler(this.Name_MouseEnter);
            this.TechDuration.MouseLeave += new System.EventHandler(this.Name_MouseLeave);
            // 
            // TechIcon
            // 
            this.TechIcon.Location = new System.Drawing.Point(203, 3);
            this.TechIcon.Name = "TechIcon";
            this.TechIcon.Size = new System.Drawing.Size(24, 24);
            this.TechIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TechIcon.TabIndex = 7;
            this.TechIcon.TabStop = false;
            // 
            // Shortcut
            // 
            this.Shortcut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Shortcut.Location = new System.Drawing.Point(2, 2);
            this.Shortcut.Name = "Shortcut";
            this.Shortcut.Size = new System.Drawing.Size(24, 27);
            this.Shortcut.TabIndex = 5;
            this.Shortcut.Text = "X";
            this.Shortcut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Shortcut.Click += new System.EventHandler(this.Name_Click);
            this.Shortcut.DoubleClick += new System.EventHandler(this.Name_DoubleClick);
            this.Shortcut.MouseEnter += new System.EventHandler(this.Name_MouseEnter);
            this.Shortcut.MouseLeave += new System.EventHandler(this.Name_MouseLeave);
            // 
            // TechTreeItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.TechIcon);
            this.Controls.Add(this.TimeProgress);
            this.Controls.Add(this.TechDuration);
            this.Controls.Add(this.TechAmount);
            this.Controls.Add(this.Shortcut);
            this.Controls.Add(this.TechName);
            this.Controls.Add(this.InvestmentProgress);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TechTreeItem";
            this.Size = new System.Drawing.Size(230, 80);
            ((System.ComponentModel.ISupportInitialize)(this.TechIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar TimeProgress;
        private System.Windows.Forms.Label TechName;
        private System.Windows.Forms.ProgressBar InvestmentProgress;
        private System.Windows.Forms.Label TechAmount;
        private System.Windows.Forms.Label TechDuration;
        private System.Windows.Forms.PictureBox TechIcon;
        private System.Windows.Forms.Label Shortcut;
    }
}
