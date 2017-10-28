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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignEnd));
            this.label3 = new System.Windows.Forms.Label();
            this.Points = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.StartGame = new System.Windows.Forms.Button();
            this.ChangeFaction = new System.Windows.Forms.Button();
            this.UnlockTech = new System.Windows.Forms.Button();
            this.FactionProgress = new System.Windows.Forms.ProgressBar();
            this.TechProgress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.FactionText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TechText = new System.Windows.Forms.Label();
            this.MapPreview = new System.Windows.Forms.PictureBox();
            this.MapsRemaining = new System.Windows.Forms.ProgressBar();
            this.MapText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CommanderName = new System.Windows.Forms.Label();
            this.FactionName = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MapPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "Available Points:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Points
            // 
            this.Points.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Points.ForeColor = System.Drawing.Color.Lime;
            this.Points.Location = new System.Drawing.Point(160, 88);
            this.Points.Name = "Points";
            this.Points.Size = new System.Drawing.Size(125, 39);
            this.Points.TabIndex = 1;
            this.Points.Text = "[Points]";
            this.Points.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Black;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Location = new System.Drawing.Point(160, 337);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(125, 45);
            this.Cancel.TabIndex = 16;
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
            this.StartGame.Location = new System.Drawing.Point(12, 337);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(142, 45);
            this.StartGame.TabIndex = 15;
            this.StartGame.Text = "Next Game";
            this.StartGame.UseVisualStyleBackColor = false;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            this.StartGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.StartGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ChangeFaction
            // 
            this.ChangeFaction.BackColor = System.Drawing.Color.Black;
            this.ChangeFaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeFaction.Location = new System.Drawing.Point(42, 138);
            this.ChangeFaction.Name = "ChangeFaction";
            this.ChangeFaction.Size = new System.Drawing.Size(112, 32);
            this.ChangeFaction.TabIndex = 3;
            this.ChangeFaction.Text = "Upgrade Faction";
            this.ChangeFaction.UseVisualStyleBackColor = false;
            this.ChangeFaction.Click += new System.EventHandler(this.ChangeFaction_Click);
            this.ChangeFaction.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ChangeFaction.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // UnlockTech
            // 
            this.UnlockTech.BackColor = System.Drawing.Color.Black;
            this.UnlockTech.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnlockTech.Location = new System.Drawing.Point(42, 182);
            this.UnlockTech.Name = "UnlockTech";
            this.UnlockTech.Size = new System.Drawing.Size(112, 32);
            this.UnlockTech.TabIndex = 8;
            this.UnlockTech.Text = "Unlock Tech";
            this.UnlockTech.UseVisualStyleBackColor = false;
            this.UnlockTech.Click += new System.EventHandler(this.UnlockTech_Click);
            this.UnlockTech.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.UnlockTech.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // FactionProgress
            // 
            this.FactionProgress.ForeColor = System.Drawing.Color.Lime;
            this.FactionProgress.Location = new System.Drawing.Point(160, 159);
            this.FactionProgress.Name = "FactionProgress";
            this.FactionProgress.Size = new System.Drawing.Size(125, 11);
            this.FactionProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.FactionProgress.TabIndex = 6;
            this.FactionProgress.Value = 50;
            // 
            // TechProgress
            // 
            this.TechProgress.ForeColor = System.Drawing.Color.Lime;
            this.TechProgress.Location = new System.Drawing.Point(160, 203);
            this.TechProgress.Name = "TechProgress";
            this.TechProgress.Size = new System.Drawing.Size(125, 11);
            this.TechProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.TechProgress.TabIndex = 11;
            this.TechProgress.Value = 50;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(157, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Progress:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FactionText
            // 
            this.FactionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FactionText.ForeColor = System.Drawing.Color.Lime;
            this.FactionText.Location = new System.Drawing.Point(225, 138);
            this.FactionText.Name = "FactionText";
            this.FactionText.Size = new System.Drawing.Size(57, 18);
            this.FactionText.TabIndex = 5;
            this.FactionText.Text = "50 / 100";
            this.FactionText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(157, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "Unlocked:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TechText
            // 
            this.TechText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TechText.ForeColor = System.Drawing.Color.Lime;
            this.TechText.Location = new System.Drawing.Point(228, 182);
            this.TechText.Name = "TechText";
            this.TechText.Size = new System.Drawing.Size(57, 18);
            this.TechText.TabIndex = 10;
            this.TechText.Text = "50 / 100";
            this.TechText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MapPreview
            // 
            this.MapPreview.Location = new System.Drawing.Point(12, 220);
            this.MapPreview.Name = "MapPreview";
            this.MapPreview.Size = new System.Drawing.Size(142, 111);
            this.MapPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MapPreview.TabIndex = 62;
            this.MapPreview.TabStop = false;
            // 
            // MapsRemaining
            // 
            this.MapsRemaining.ForeColor = System.Drawing.Color.Lime;
            this.MapsRemaining.Location = new System.Drawing.Point(160, 287);
            this.MapsRemaining.Name = "MapsRemaining";
            this.MapsRemaining.Size = new System.Drawing.Size(125, 11);
            this.MapsRemaining.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.MapsRemaining.TabIndex = 14;
            this.MapsRemaining.Value = 50;
            // 
            // MapText
            // 
            this.MapText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MapText.ForeColor = System.Drawing.Color.Lime;
            this.MapText.Location = new System.Drawing.Point(160, 266);
            this.MapText.Name = "MapText";
            this.MapText.Size = new System.Drawing.Size(125, 18);
            this.MapText.TabIndex = 13;
            this.MapText.Text = "50 / 100";
            this.MapText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Lime;
            this.label4.Location = new System.Drawing.Point(1, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 32);
            this.label4.TabIndex = 2;
            this.label4.Text = "1.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(1, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 32);
            this.label5.TabIndex = 7;
            this.label5.Text = "2.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(160, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Available Maps:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CommanderName
            // 
            this.CommanderName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommanderName.ForeColor = System.Drawing.Color.White;
            this.CommanderName.Location = new System.Drawing.Point(2, 9);
            this.CommanderName.Name = "CommanderName";
            this.CommanderName.Size = new System.Drawing.Size(296, 16);
            this.CommanderName.TabIndex = 0;
            this.CommanderName.Text = "[Commander Name]";
            this.CommanderName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FactionName
            // 
            this.FactionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FactionName.ForeColor = System.Drawing.Color.Lime;
            this.FactionName.Location = new System.Drawing.Point(0, 25);
            this.FactionName.Name = "FactionName";
            this.FactionName.Size = new System.Drawing.Size(298, 63);
            this.FactionName.TabIndex = 0;
            this.FactionName.Text = "[Faction Name]";
            this.FactionName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CampaignEnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(297, 395);
            this.Controls.Add(this.MapPreview);
            this.Controls.Add(this.MapsRemaining);
            this.Controls.Add(this.TechProgress);
            this.Controls.Add(this.FactionProgress);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.UnlockTech);
            this.Controls.Add(this.ChangeFaction);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.MapText);
            this.Controls.Add(this.TechText);
            this.Controls.Add(this.FactionText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Points);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FactionName);
            this.Controls.Add(this.CommanderName);
            this.Controls.Add(this.label3);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CampaignEnd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Campaign Game";
            this.Load += new System.EventHandler(this.Ladder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MapPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Points;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Button ChangeFaction;
        private System.Windows.Forms.Button UnlockTech;
        private System.Windows.Forms.ProgressBar FactionProgress;
        private System.Windows.Forms.ProgressBar TechProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label FactionText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TechText;
        private System.Windows.Forms.PictureBox MapPreview;
        private System.Windows.Forms.ProgressBar MapsRemaining;
        private System.Windows.Forms.Label MapText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label CommanderName;
        private System.Windows.Forms.Label FactionName;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}