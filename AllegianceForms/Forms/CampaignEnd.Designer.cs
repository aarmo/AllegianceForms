namespace AllegianceForms.Forms
{
    partial class CampaignEnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignEnd));
            this.label3 = new System.Windows.Forms.Label();
            this.Points = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.StartGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Available Points:";
            // 
            // Points
            // 
            this.Points.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Points.ForeColor = System.Drawing.Color.White;
            this.Points.Location = new System.Drawing.Point(12, 25);
            this.Points.Name = "Points";
            this.Points.Size = new System.Drawing.Size(777, 39);
            this.Points.TabIndex = 3;
            this.Points.Text = "[Points]";
            this.Points.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Black;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Location = new System.Drawing.Point(401, 111);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(125, 45);
            this.Cancel.TabIndex = 60;
            this.Cancel.Text = "Close";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Cancel.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // StartGame
            // 
            this.StartGame.BackColor = System.Drawing.Color.Black;
            this.StartGame.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.StartGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartGame.Location = new System.Drawing.Point(263, 111);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(125, 45);
            this.StartGame.TabIndex = 59;
            this.StartGame.Text = "Next Game";
            this.StartGame.UseVisualStyleBackColor = false;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            this.StartGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.StartGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // CampaignEnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(801, 174);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.Points);
            this.Controls.Add(this.label3);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CampaignEnd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allegiance Forms - Campaign";
            this.Load += new System.EventHandler(this.Ladder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Points;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button StartGame;
    }
}