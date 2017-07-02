namespace AllegianceForms.Forms
{
    partial class Ladder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ladder));
            this.Difficulty = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LadderType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CommanderItems = new System.Windows.Forms.FlowLayoutPanel();
            this.label55 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.MapPool = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CurrentTier = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.StartGame = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PlayerCommander = new System.Windows.Forms.FlowLayoutPanel();
            this.Abandon = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Difficulty
            // 
            this.Difficulty.BackColor = System.Drawing.Color.Black;
            this.Difficulty.Enabled = false;
            this.Difficulty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Difficulty.ForeColor = System.Drawing.Color.White;
            this.Difficulty.FormattingEnabled = true;
            this.Difficulty.Items.AddRange(new object[] {
            "Inactive",
            "Very Easy",
            "Easy",
            "Normal",
            "Hard",
            "Very Hard",
            "Insane"});
            this.Difficulty.Location = new System.Drawing.Point(89, 8);
            this.Difficulty.Name = "Difficulty";
            this.Difficulty.Size = new System.Drawing.Size(120, 21);
            this.Difficulty.TabIndex = 4;
            this.Difficulty.Text = "Normal";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.Lime;
            this.label41.Location = new System.Drawing.Point(12, 9);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(71, 16);
            this.label41.TabIndex = 3;
            this.label41.Text = "Difficulty:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(227, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Type:";
            // 
            // LadderType
            // 
            this.LadderType.BackColor = System.Drawing.Color.Black;
            this.LadderType.Enabled = false;
            this.LadderType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LadderType.ForeColor = System.Drawing.Color.White;
            this.LadderType.FormattingEnabled = true;
            this.LadderType.Items.AddRange(new object[] {
            "1v1",
            "2v2"});
            this.LadderType.Location = new System.Drawing.Point(281, 8);
            this.LadderType.Name = "LadderType";
            this.LadderType.Size = new System.Drawing.Size(76, 21);
            this.LadderType.TabIndex = 4;
            this.LadderType.Text = "1v1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(709, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Map Pool:";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CommanderItems);
            this.panel1.Location = new System.Drawing.Point(15, 111);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 251);
            this.panel1.TabIndex = 56;
            // 
            // CommanderItems
            // 
            this.CommanderItems.AutoSize = true;
            this.CommanderItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.CommanderItems.Location = new System.Drawing.Point(5, 0);
            this.CommanderItems.Name = "CommanderItems";
            this.CommanderItems.Size = new System.Drawing.Size(742, 65);
            this.CommanderItems.TabIndex = 3;
            this.CommanderItems.WrapContents = false;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.ForeColor = System.Drawing.Color.Lime;
            this.label55.Location = new System.Drawing.Point(278, 96);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(105, 13);
            this.label55.TabIndex = 54;
            this.label55.Text = "Games (Won / Loss)";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.ForeColor = System.Drawing.Color.Lime;
            this.label50.Location = new System.Drawing.Point(18, 95);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(95, 13);
            this.label50.TabIndex = 55;
            this.label50.Text = "Commander Rank:";
            // 
            // MapPool
            // 
            this.MapPool.BackColor = System.Drawing.Color.Black;
            this.MapPool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapPool.ForeColor = System.Drawing.Color.White;
            this.MapPool.FormattingEnabled = true;
            this.MapPool.Location = new System.Drawing.Point(802, 8);
            this.MapPool.Name = "MapPool";
            this.MapPool.Size = new System.Drawing.Size(120, 106);
            this.MapPool.TabIndex = 57;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(12, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Current Tier:";
            // 
            // CurrentTier
            // 
            this.CurrentTier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CurrentTier.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentTier.ForeColor = System.Drawing.Color.Lime;
            this.CurrentTier.Location = new System.Drawing.Point(15, 48);
            this.CurrentTier.Name = "CurrentTier";
            this.CurrentTier.Size = new System.Drawing.Size(772, 39);
            this.CurrentTier.TabIndex = 3;
            this.CurrentTier.Text = "Unranked";
            this.CurrentTier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(18, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 55;
            this.label5.Text = "Player:";
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Black;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Location = new System.Drawing.Point(405, 457);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(125, 45);
            this.Cancel.TabIndex = 60;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Cancel.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // StartGame
            // 
            this.StartGame.BackColor = System.Drawing.Color.Black;
            this.StartGame.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.StartGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartGame.Location = new System.Drawing.Point(267, 457);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(125, 45);
            this.StartGame.TabIndex = 59;
            this.StartGame.Text = "Next Game";
            this.StartGame.UseVisualStyleBackColor = false;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            this.StartGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.StartGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.PlayerCommander);
            this.panel2.Location = new System.Drawing.Point(15, 381);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(772, 70);
            this.panel2.TabIndex = 56;
            // 
            // PlayerCommander
            // 
            this.PlayerCommander.AutoSize = true;
            this.PlayerCommander.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.PlayerCommander.Location = new System.Drawing.Point(5, 0);
            this.PlayerCommander.Name = "PlayerCommander";
            this.PlayerCommander.Size = new System.Drawing.Size(742, 65);
            this.PlayerCommander.TabIndex = 3;
            this.PlayerCommander.WrapContents = false;
            // 
            // Abandon
            // 
            this.Abandon.BackColor = System.Drawing.Color.Black;
            this.Abandon.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Abandon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Abandon.Location = new System.Drawing.Point(797, 457);
            this.Abandon.Name = "Abandon";
            this.Abandon.Size = new System.Drawing.Size(125, 45);
            this.Abandon.TabIndex = 60;
            this.Abandon.Text = "Abandon Ladder";
            this.Abandon.UseVisualStyleBackColor = false;
            this.Abandon.Click += new System.EventHandler(this.Abandon_Click);
            this.Abandon.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Abandon.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Ladder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(925, 508);
            this.Controls.Add(this.Abandon);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.MapPool);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label55);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.LadderType);
            this.Controls.Add(this.Difficulty);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentTier);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label41);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Ladder";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allegiance Forms - Ladder Game";
            this.Load += new System.EventHandler(this.Ladder_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Difficulty;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox LadderType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel CommanderItems;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.ListBox MapPool;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label CurrentTier;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel PlayerCommander;
        private System.Windows.Forms.Button Abandon;
    }
}