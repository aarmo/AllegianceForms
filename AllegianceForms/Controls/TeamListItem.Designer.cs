namespace AllegianceForms.Controls
{
    partial class TeamListItem
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
            this.TeamColour = new System.Windows.Forms.Button();
            this.TeamFaction = new System.Windows.Forms.Button();
            this.TeamNumber = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // TeamColour
            // 
            this.TeamColour.BackColor = System.Drawing.Color.Lime;
            this.TeamColour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TeamColour.ForeColor = System.Drawing.Color.Black;
            this.TeamColour.Location = new System.Drawing.Point(55, 3);
            this.TeamColour.Name = "TeamColour";
            this.TeamColour.Size = new System.Drawing.Size(69, 29);
            this.TeamColour.TabIndex = 49;
            this.TeamColour.Text = "Change";
            this.TeamColour.UseVisualStyleBackColor = false;
            this.TeamColour.Click += new System.EventHandler(this.TeamColour_Click);
            this.TeamColour.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.TeamColour.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // TeamFaction
            // 
            this.TeamFaction.BackColor = System.Drawing.Color.Black;
            this.TeamFaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TeamFaction.Font = new System.Drawing.Font("Palatino Linotype", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TeamFaction.Location = new System.Drawing.Point(130, 3);
            this.TeamFaction.Name = "TeamFaction";
            this.TeamFaction.Size = new System.Drawing.Size(292, 29);
            this.TeamFaction.TabIndex = 50;
            this.TeamFaction.Text = "...";
            this.TeamFaction.UseVisualStyleBackColor = false;
            this.TeamFaction.Click += new System.EventHandler(this.TeamFaction_Click);
            this.TeamFaction.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.TeamFaction.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // TeamNumber
            // 
            this.TeamNumber.AutoSize = true;
            this.TeamNumber.Location = new System.Drawing.Point(3, 11);
            this.TeamNumber.Name = "TeamNumber";
            this.TeamNumber.Size = new System.Drawing.Size(46, 13);
            this.TeamNumber.TabIndex = 48;
            this.TeamNumber.Text = "Team 1:";
            // 
            // TeamListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.TeamColour);
            this.Controls.Add(this.TeamFaction);
            this.Controls.Add(this.TeamNumber);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TeamListItem";
            this.Size = new System.Drawing.Size(427, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TeamColour;
        private System.Windows.Forms.Button TeamFaction;
        private System.Windows.Forms.Label TeamNumber;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
