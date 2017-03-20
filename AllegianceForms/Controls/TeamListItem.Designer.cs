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
            this.AllianceGroup = new System.Windows.Forms.ComboBox();
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
            // AllianceGroup
            // 
            this.AllianceGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.AllianceGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AllianceGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AllianceGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllianceGroup.FormattingEnabled = true;
            this.AllianceGroup.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.AllianceGroup.Location = new System.Drawing.Point(428, 3);
            this.AllianceGroup.Name = "AllianceGroup";
            this.AllianceGroup.Size = new System.Drawing.Size(38, 28);
            this.AllianceGroup.TabIndex = 51;
            this.AllianceGroup.SelectedIndexChanged += new System.EventHandler(this.AllianceGroup_SelectedIndexChanged);
            // 
            // TeamListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.AllianceGroup);
            this.Controls.Add(this.TeamColour);
            this.Controls.Add(this.TeamFaction);
            this.Controls.Add(this.TeamNumber);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TeamListItem";
            this.Size = new System.Drawing.Size(469, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TeamColour;
        private System.Windows.Forms.Button TeamFaction;
        private System.Windows.Forms.Label TeamNumber;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ComboBox AllianceGroup;
    }
}
