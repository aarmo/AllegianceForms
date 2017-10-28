namespace AllegianceForms.Forms
{
    partial class CampaignStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignStart));
            this.Cancel = new System.Windows.Forms.Button();
            this.StartGame = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PlayerName = new System.Windows.Forms.TextBox();
            this.RandomCommanderName = new System.Windows.Forms.Button();
            this.RandomName = new System.Windows.Forms.Button();
            this.FactionName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Black;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Location = new System.Drawing.Point(183, 148);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(125, 45);
            this.Cancel.TabIndex = 7;
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
            this.StartGame.Location = new System.Drawing.Point(12, 148);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(165, 45);
            this.StartGame.TabIndex = 6;
            this.StartGame.Text = "Start Game";
            this.StartGame.UseVisualStyleBackColor = false;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            this.StartGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.StartGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Commander Name:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(9, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Faction Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PlayerName
            // 
            this.PlayerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.PlayerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PlayerName.Location = new System.Drawing.Point(12, 29);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(232, 23);
            this.PlayerName.TabIndex = 1;
            this.PlayerName.Text = "Player 1";
            this.PlayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PlayerName.Enter += new System.EventHandler(this.PlayerName_Enter);
            this.PlayerName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlayerName_MouseDown);
            // 
            // RandomCommanderName
            // 
            this.RandomCommanderName.BackColor = System.Drawing.Color.Black;
            this.RandomCommanderName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RandomCommanderName.ForeColor = System.Drawing.Color.White;
            this.RandomCommanderName.Location = new System.Drawing.Point(250, 30);
            this.RandomCommanderName.Name = "RandomCommanderName";
            this.RandomCommanderName.Size = new System.Drawing.Size(58, 22);
            this.RandomCommanderName.TabIndex = 2;
            this.RandomCommanderName.Text = "Random";
            this.RandomCommanderName.UseVisualStyleBackColor = false;
            this.RandomCommanderName.Click += new System.EventHandler(this.RandomCommanderName_Click);
            // 
            // RandomName
            // 
            this.RandomName.BackColor = System.Drawing.Color.Black;
            this.RandomName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RandomName.ForeColor = System.Drawing.Color.White;
            this.RandomName.Location = new System.Drawing.Point(250, 98);
            this.RandomName.Name = "RandomName";
            this.RandomName.Size = new System.Drawing.Size(58, 22);
            this.RandomName.TabIndex = 5;
            this.RandomName.Text = "Random";
            this.RandomName.UseVisualStyleBackColor = false;
            this.RandomName.Click += new System.EventHandler(this.RandomName_Click);
            // 
            // FactionName
            // 
            this.FactionName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.FactionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FactionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FactionName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FactionName.Location = new System.Drawing.Point(12, 99);
            this.FactionName.Name = "FactionName";
            this.FactionName.Size = new System.Drawing.Size(232, 23);
            this.FactionName.TabIndex = 4;
            this.FactionName.Text = "New Blood";
            this.FactionName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FactionName.Enter += new System.EventHandler(this.FactionName_Enter);
            this.FactionName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FactionName_MouseDown);
            // 
            // CampaignStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(322, 213);
            this.Controls.Add(this.FactionName);
            this.Controls.Add(this.PlayerName);
            this.Controls.Add(this.RandomCommanderName);
            this.Controls.Add(this.RandomName);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CampaignStart";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Campaign";
            this.Load += new System.EventHandler(this.Ladder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RandomCommanderName;
        private System.Windows.Forms.Button RandomName;
        public System.Windows.Forms.TextBox PlayerName;
        public System.Windows.Forms.TextBox FactionName;
    }
}