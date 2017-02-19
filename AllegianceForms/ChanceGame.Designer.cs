namespace AllegianceForms
{
    partial class ChanceGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChanceGame));
            this.GameRules = new System.Windows.Forms.Button();
            this.StartGame = new System.Windows.Forms.Button();
            this.WinLose = new System.Windows.Forms.Label();
            this.GameInfo = new System.Windows.Forms.Label();
            this.Score = new System.Windows.Forms.Label();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GameRules
            // 
            this.GameRules.BackColor = System.Drawing.Color.Black;
            this.GameRules.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GameRules.ForeColor = System.Drawing.Color.White;
            this.GameRules.Location = new System.Drawing.Point(5, 12);
            this.GameRules.Name = "GameRules";
            this.GameRules.Size = new System.Drawing.Size(156, 23);
            this.GameRules.TabIndex = 0;
            this.GameRules.Text = "Rules";
            this.GameRules.UseVisualStyleBackColor = false;
            this.GameRules.Click += new System.EventHandler(this.GameRules_Click);
            this.GameRules.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.GameRules.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // StartGame
            // 
            this.StartGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartGame.ForeColor = System.Drawing.Color.White;
            this.StartGame.Location = new System.Drawing.Point(5, 41);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(156, 23);
            this.StartGame.TabIndex = 0;
            this.StartGame.Text = "Start Game";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            this.StartGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.StartGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // WinLose
            // 
            this.WinLose.BackColor = System.Drawing.Color.Transparent;
            this.WinLose.Font = new System.Drawing.Font("Segoe Print", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinLose.ForeColor = System.Drawing.Color.White;
            this.WinLose.Location = new System.Drawing.Point(5, 67);
            this.WinLose.Name = "WinLose";
            this.WinLose.Size = new System.Drawing.Size(349, 63);
            this.WinLose.TabIndex = 1;
            this.WinLose.Text = "Dogfight";
            this.WinLose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameInfo
            // 
            this.GameInfo.BackColor = System.Drawing.Color.Transparent;
            this.GameInfo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameInfo.Image = ((System.Drawing.Image)(resources.GetObject("GameInfo.Image")));
            this.GameInfo.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.GameInfo.Location = new System.Drawing.Point(2, 130);
            this.GameInfo.Name = "GameInfo";
            this.GameInfo.Size = new System.Drawing.Size(352, 359);
            this.GameInfo.TabIndex = 1;
            this.GameInfo.Text = "Good luck!";
            this.GameInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Score
            // 
            this.Score.BackColor = System.Drawing.Color.DarkGreen;
            this.Score.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Score.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Score.ForeColor = System.Drawing.Color.White;
            this.Score.Location = new System.Drawing.Point(198, 12);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(156, 52);
            this.Score.TabIndex = 1;
            this.Score.Text = "0 : 0";
            this.Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 1000;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // ChanceGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(358, 493);
            this.Controls.Add(this.Score);
            this.Controls.Add(this.WinLose);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.GameRules);
            this.Controls.Add(this.GameInfo);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Lime;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ChanceGame";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allegiance Forms - Dogfight";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChanceGame_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChanceGame_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GameRules;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Label WinLose;
        private System.Windows.Forms.Label GameInfo;
        private System.Windows.Forms.Label Score;
        private System.Windows.Forms.Timer gameTimer;
    }
}