namespace AllegianceForms.Controls
{
    partial class CommandBarButton
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
            this.ButtonText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonText
            // 
            this.ButtonText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ButtonText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonText.Location = new System.Drawing.Point(4, 1);
            this.ButtonText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ButtonText.Name = "ButtonText";
            this.ButtonText.Size = new System.Drawing.Size(118, 65);
            this.ButtonText.TabIndex = 6;
            this.ButtonText.Text = "LongButton Name \r\n(C)\r\n";
            this.ButtonText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonText_MouseDown);
            this.ButtonText.MouseEnter += new System.EventHandler(this.Name_MouseEnter);
            this.ButtonText.MouseLeave += new System.EventHandler(this.Name_MouseLeave);
            this.ButtonText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonText_MouseUp);
            // 
            // CommandBarButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.ButtonText);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CommandBarButton";
            this.Size = new System.Drawing.Size(125, 68);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ButtonText;
    }
}
