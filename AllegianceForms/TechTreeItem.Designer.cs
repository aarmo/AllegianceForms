namespace AllegianceForms
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
            // Name
            // 
            this.TechName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TechName.Location = new System.Drawing.Point(3, 3);
            this.TechName.Name = "Name";
            this.TechName.Size = new System.Drawing.Size(224, 45);
            this.TechName.TabIndex = 5;
            this.TechName.Text = "[Description]";
            this.TechName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            // TechTreeItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.TimeProgress);
            this.Controls.Add(this.TechName);
            this.Controls.Add(this.InvestmentProgress);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TechTreeItem";
            this.Size = new System.Drawing.Size(230, 80);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar TimeProgress;
        private System.Windows.Forms.Label TechName;
        private System.Windows.Forms.ProgressBar InvestmentProgress;
    }
}
