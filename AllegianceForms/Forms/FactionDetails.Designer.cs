namespace AllegianceForms.Forms
{
    partial class FactionDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FactionDetails));
            this.Cancel = new System.Windows.Forms.Button();
            this.Random = new System.Windows.Forms.Button();
            this.LoadPreset = new System.Windows.Forms.Button();
            this.SavePreset = new System.Windows.Forms.Button();
            this.Done = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.CustomPresets = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FactionName = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ResearchTimeUp = new System.Windows.Forms.Button();
            this.ResearchTimeDown = new System.Windows.Forms.Button();
            this.ResearchTime = new System.Windows.Forms.Label();
            this.RandomName = new System.Windows.Forms.Button();
            this.ResearchCost = new System.Windows.Forms.Label();
            this.ResearchCostUp = new System.Windows.Forms.Button();
            this.ResearchCostDown = new System.Windows.Forms.Button();
            this.Speed = new System.Windows.Forms.Label();
            this.SpeedUp = new System.Windows.Forms.Button();
            this.SpeedDown = new System.Windows.Forms.Button();
            this.Health = new System.Windows.Forms.Label();
            this.HealthUp = new System.Windows.Forms.Button();
            this.HealthDown = new System.Windows.Forms.Button();
            this.ScanRange = new System.Windows.Forms.Label();
            this.ScanRangeUp = new System.Windows.Forms.Button();
            this.ScanRangeDown = new System.Windows.Forms.Button();
            this.Signature = new System.Windows.Forms.Label();
            this.SignatureUp = new System.Windows.Forms.Button();
            this.SignatureDown = new System.Windows.Forms.Button();
            this.FireRate = new System.Windows.Forms.Label();
            this.FireRateUp = new System.Windows.Forms.Button();
            this.FireRateDown = new System.Windows.Forms.Button();
            this.MissileSpeed = new System.Windows.Forms.Label();
            this.MissileSpeedUp = new System.Windows.Forms.Button();
            this.MissileSpeedDown = new System.Windows.Forms.Button();
            this.MiningEfficiency = new System.Windows.Forms.Label();
            this.MiningSpeedUp = new System.Windows.Forms.Button();
            this.MiningSpeedDown = new System.Windows.Forms.Button();
            this.MiningCapacity = new System.Windows.Forms.Label();
            this.MiningCapacityUp = new System.Windows.Forms.Button();
            this.MiningCapacityDown = new System.Windows.Forms.Button();
            this.BalancedLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.MissileTracking = new System.Windows.Forms.Label();
            this.MissileTrackingUp = new System.Windows.Forms.Button();
            this.MissileTrackingDown = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.FactionPicture = new System.Windows.Forms.PictureBox();
            this.RandomImage = new System.Windows.Forms.Button();
            this.PlayerName = new System.Windows.Forms.TextBox();
            this.RandomCommanderName = new System.Windows.Forms.Button();
            this.Default = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.FactionRace = new System.Windows.Forms.ComboBox();
            this.RacePicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.FactionPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RacePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Black;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.ForeColor = System.Drawing.Color.White;
            this.Cancel.Location = new System.Drawing.Point(230, 430);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(125, 45);
            this.Cancel.TabIndex = 58;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Cancel.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Random
            // 
            this.Random.BackColor = System.Drawing.Color.Black;
            this.Random.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Random.ForeColor = System.Drawing.Color.White;
            this.Random.Location = new System.Drawing.Point(321, 6);
            this.Random.Name = "Random";
            this.Random.Size = new System.Drawing.Size(58, 22);
            this.Random.TabIndex = 56;
            this.Random.Text = "Random";
            this.Random.UseVisualStyleBackColor = false;
            this.Random.Click += new System.EventHandler(this.Random_Click);
            this.Random.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Random.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // LoadPreset
            // 
            this.LoadPreset.BackColor = System.Drawing.Color.Black;
            this.LoadPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadPreset.ForeColor = System.Drawing.Color.White;
            this.LoadPreset.Location = new System.Drawing.Point(199, 6);
            this.LoadPreset.Name = "LoadPreset";
            this.LoadPreset.Size = new System.Drawing.Size(55, 22);
            this.LoadPreset.TabIndex = 54;
            this.LoadPreset.Text = "Load";
            this.LoadPreset.UseVisualStyleBackColor = false;
            this.LoadPreset.Click += new System.EventHandler(this.Load_Click);
            this.LoadPreset.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.LoadPreset.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // SavePreset
            // 
            this.SavePreset.BackColor = System.Drawing.Color.Black;
            this.SavePreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SavePreset.ForeColor = System.Drawing.Color.White;
            this.SavePreset.Location = new System.Drawing.Point(260, 6);
            this.SavePreset.Name = "SavePreset";
            this.SavePreset.Size = new System.Drawing.Size(55, 22);
            this.SavePreset.TabIndex = 55;
            this.SavePreset.Text = "Save";
            this.SavePreset.UseVisualStyleBackColor = false;
            this.SavePreset.Click += new System.EventHandler(this.Save_Click);
            this.SavePreset.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.SavePreset.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Done
            // 
            this.Done.BackColor = System.Drawing.Color.Black;
            this.Done.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Done.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Done.ForeColor = System.Drawing.Color.White;
            this.Done.Location = new System.Drawing.Point(68, 430);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(125, 45);
            this.Done.TabIndex = 57;
            this.Done.Text = "OK";
            this.Done.UseVisualStyleBackColor = false;
            this.Done.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Done.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.ForeColor = System.Drawing.Color.Lime;
            this.label42.Location = new System.Drawing.Point(12, 9);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(65, 16);
            this.label42.TabIndex = 52;
            this.label42.Text = "Presets:";
            // 
            // CustomPresets
            // 
            this.CustomPresets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CustomPresets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomPresets.FormattingEnabled = true;
            this.CustomPresets.Location = new System.Drawing.Point(83, 7);
            this.CustomPresets.Name = "CustomPresets";
            this.CustomPresets.Size = new System.Drawing.Size(110, 21);
            this.CustomPresets.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 52;
            this.label1.Text = "Name:";
            // 
            // FactionName
            // 
            this.FactionName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.FactionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FactionName.Location = new System.Drawing.Point(83, 39);
            this.FactionName.Name = "FactionName";
            this.FactionName.Size = new System.Drawing.Size(232, 20);
            this.FactionName.TabIndex = 59;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.ForeColor = System.Drawing.Color.White;
            this.label51.Location = new System.Drawing.Point(22, 100);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(82, 13);
            this.label51.TabIndex = 60;
            this.label51.Text = "Research Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(22, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "Research Cost:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(27, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 60;
            this.label3.Text = "Speed:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(27, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Health:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(22, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Scan Range:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(22, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 60;
            this.label6.Text = "Signature:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(22, 268);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 60;
            this.label7.Text = "Fire Rate:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(22, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 60;
            this.label8.Text = "Missile Speed:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(22, 352);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 60;
            this.label9.Text = "Mining Efficiency:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(22, 380);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Mining Capacity:";
            // 
            // ResearchTimeUp
            // 
            this.ResearchTimeUp.BackColor = System.Drawing.Color.Black;
            this.ResearchTimeUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResearchTimeUp.ForeColor = System.Drawing.Color.White;
            this.ResearchTimeUp.Location = new System.Drawing.Point(139, 95);
            this.ResearchTimeUp.Name = "ResearchTimeUp";
            this.ResearchTimeUp.Size = new System.Drawing.Size(58, 22);
            this.ResearchTimeUp.TabIndex = 54;
            this.ResearchTimeUp.Text = "+10%";
            this.ResearchTimeUp.UseVisualStyleBackColor = false;
            this.ResearchTimeUp.Click += new System.EventHandler(this.ResearchTimeUp_Click);
            this.ResearchTimeUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ResearchTimeUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ResearchTimeDown
            // 
            this.ResearchTimeDown.BackColor = System.Drawing.Color.Black;
            this.ResearchTimeDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResearchTimeDown.ForeColor = System.Drawing.Color.White;
            this.ResearchTimeDown.Location = new System.Drawing.Point(325, 95);
            this.ResearchTimeDown.Name = "ResearchTimeDown";
            this.ResearchTimeDown.Size = new System.Drawing.Size(58, 22);
            this.ResearchTimeDown.TabIndex = 54;
            this.ResearchTimeDown.Text = "-10%";
            this.ResearchTimeDown.UseVisualStyleBackColor = false;
            this.ResearchTimeDown.Click += new System.EventHandler(this.ResearchTimeDown_Click);
            this.ResearchTimeDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ResearchTimeDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ResearchTime
            // 
            this.ResearchTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResearchTime.ForeColor = System.Drawing.Color.Lime;
            this.ResearchTime.Location = new System.Drawing.Point(203, 97);
            this.ResearchTime.Name = "ResearchTime";
            this.ResearchTime.Size = new System.Drawing.Size(116, 16);
            this.ResearchTime.TabIndex = 52;
            this.ResearchTime.Text = "-130%";
            this.ResearchTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RandomName
            // 
            this.RandomName.BackColor = System.Drawing.Color.Black;
            this.RandomName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RandomName.ForeColor = System.Drawing.Color.White;
            this.RandomName.Location = new System.Drawing.Point(321, 38);
            this.RandomName.Name = "RandomName";
            this.RandomName.Size = new System.Drawing.Size(58, 22);
            this.RandomName.TabIndex = 56;
            this.RandomName.Text = "Random";
            this.RandomName.UseVisualStyleBackColor = false;
            this.RandomName.Click += new System.EventHandler(this.RandomName_Click);
            this.RandomName.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.RandomName.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ResearchCost
            // 
            this.ResearchCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResearchCost.ForeColor = System.Drawing.Color.Lime;
            this.ResearchCost.Location = new System.Drawing.Point(203, 126);
            this.ResearchCost.Name = "ResearchCost";
            this.ResearchCost.Size = new System.Drawing.Size(116, 16);
            this.ResearchCost.TabIndex = 52;
            this.ResearchCost.Text = "-130%";
            this.ResearchCost.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ResearchCostUp
            // 
            this.ResearchCostUp.BackColor = System.Drawing.Color.Black;
            this.ResearchCostUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResearchCostUp.ForeColor = System.Drawing.Color.White;
            this.ResearchCostUp.Location = new System.Drawing.Point(139, 123);
            this.ResearchCostUp.Name = "ResearchCostUp";
            this.ResearchCostUp.Size = new System.Drawing.Size(58, 22);
            this.ResearchCostUp.TabIndex = 54;
            this.ResearchCostUp.Text = "+10%";
            this.ResearchCostUp.UseVisualStyleBackColor = false;
            this.ResearchCostUp.Click += new System.EventHandler(this.ResearchCostUp_Click);
            this.ResearchCostUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ResearchCostUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ResearchCostDown
            // 
            this.ResearchCostDown.BackColor = System.Drawing.Color.Black;
            this.ResearchCostDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResearchCostDown.ForeColor = System.Drawing.Color.White;
            this.ResearchCostDown.Location = new System.Drawing.Point(325, 123);
            this.ResearchCostDown.Name = "ResearchCostDown";
            this.ResearchCostDown.Size = new System.Drawing.Size(58, 22);
            this.ResearchCostDown.TabIndex = 54;
            this.ResearchCostDown.Text = "-10%";
            this.ResearchCostDown.UseVisualStyleBackColor = false;
            this.ResearchCostDown.Click += new System.EventHandler(this.ResearchCostDown_Click);
            this.ResearchCostDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ResearchCostDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Speed
            // 
            this.Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Speed.ForeColor = System.Drawing.Color.Lime;
            this.Speed.Location = new System.Drawing.Point(203, 154);
            this.Speed.Name = "Speed";
            this.Speed.Size = new System.Drawing.Size(116, 16);
            this.Speed.TabIndex = 52;
            this.Speed.Text = "-130%";
            this.Speed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SpeedUp
            // 
            this.SpeedUp.BackColor = System.Drawing.Color.Black;
            this.SpeedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpeedUp.ForeColor = System.Drawing.Color.White;
            this.SpeedUp.Location = new System.Drawing.Point(139, 151);
            this.SpeedUp.Name = "SpeedUp";
            this.SpeedUp.Size = new System.Drawing.Size(58, 22);
            this.SpeedUp.TabIndex = 54;
            this.SpeedUp.Text = "+10%";
            this.SpeedUp.UseVisualStyleBackColor = false;
            this.SpeedUp.Click += new System.EventHandler(this.SpeedUp_Click);
            this.SpeedUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.SpeedUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // SpeedDown
            // 
            this.SpeedDown.BackColor = System.Drawing.Color.Black;
            this.SpeedDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpeedDown.ForeColor = System.Drawing.Color.White;
            this.SpeedDown.Location = new System.Drawing.Point(325, 151);
            this.SpeedDown.Name = "SpeedDown";
            this.SpeedDown.Size = new System.Drawing.Size(58, 22);
            this.SpeedDown.TabIndex = 54;
            this.SpeedDown.Text = "-10%";
            this.SpeedDown.UseVisualStyleBackColor = false;
            this.SpeedDown.Click += new System.EventHandler(this.SpeedDown_Click);
            this.SpeedDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.SpeedDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Health
            // 
            this.Health.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Health.ForeColor = System.Drawing.Color.Lime;
            this.Health.Location = new System.Drawing.Point(203, 182);
            this.Health.Name = "Health";
            this.Health.Size = new System.Drawing.Size(116, 16);
            this.Health.TabIndex = 52;
            this.Health.Text = "-130%";
            this.Health.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // HealthUp
            // 
            this.HealthUp.BackColor = System.Drawing.Color.Black;
            this.HealthUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HealthUp.ForeColor = System.Drawing.Color.White;
            this.HealthUp.Location = new System.Drawing.Point(139, 179);
            this.HealthUp.Name = "HealthUp";
            this.HealthUp.Size = new System.Drawing.Size(58, 22);
            this.HealthUp.TabIndex = 54;
            this.HealthUp.Text = "+10%";
            this.HealthUp.UseVisualStyleBackColor = false;
            this.HealthUp.Click += new System.EventHandler(this.HealthUp_Click);
            this.HealthUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.HealthUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // HealthDown
            // 
            this.HealthDown.BackColor = System.Drawing.Color.Black;
            this.HealthDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HealthDown.ForeColor = System.Drawing.Color.White;
            this.HealthDown.Location = new System.Drawing.Point(325, 179);
            this.HealthDown.Name = "HealthDown";
            this.HealthDown.Size = new System.Drawing.Size(58, 22);
            this.HealthDown.TabIndex = 54;
            this.HealthDown.Text = "-10%";
            this.HealthDown.UseVisualStyleBackColor = false;
            this.HealthDown.Click += new System.EventHandler(this.HealthDown_Click);
            this.HealthDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.HealthDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ScanRange
            // 
            this.ScanRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScanRange.ForeColor = System.Drawing.Color.Lime;
            this.ScanRange.Location = new System.Drawing.Point(203, 210);
            this.ScanRange.Name = "ScanRange";
            this.ScanRange.Size = new System.Drawing.Size(116, 16);
            this.ScanRange.TabIndex = 52;
            this.ScanRange.Text = "-130%";
            this.ScanRange.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ScanRangeUp
            // 
            this.ScanRangeUp.BackColor = System.Drawing.Color.Black;
            this.ScanRangeUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanRangeUp.ForeColor = System.Drawing.Color.White;
            this.ScanRangeUp.Location = new System.Drawing.Point(139, 207);
            this.ScanRangeUp.Name = "ScanRangeUp";
            this.ScanRangeUp.Size = new System.Drawing.Size(58, 22);
            this.ScanRangeUp.TabIndex = 54;
            this.ScanRangeUp.Text = "+10%";
            this.ScanRangeUp.UseVisualStyleBackColor = false;
            this.ScanRangeUp.Click += new System.EventHandler(this.ScanRangeUp_Click);
            this.ScanRangeUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ScanRangeUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ScanRangeDown
            // 
            this.ScanRangeDown.BackColor = System.Drawing.Color.Black;
            this.ScanRangeDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanRangeDown.ForeColor = System.Drawing.Color.White;
            this.ScanRangeDown.Location = new System.Drawing.Point(325, 207);
            this.ScanRangeDown.Name = "ScanRangeDown";
            this.ScanRangeDown.Size = new System.Drawing.Size(58, 22);
            this.ScanRangeDown.TabIndex = 54;
            this.ScanRangeDown.Text = "-10%";
            this.ScanRangeDown.UseVisualStyleBackColor = false;
            this.ScanRangeDown.Click += new System.EventHandler(this.ScanRangeDown_Click);
            this.ScanRangeDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.ScanRangeDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Signature
            // 
            this.Signature.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Signature.ForeColor = System.Drawing.Color.Lime;
            this.Signature.Location = new System.Drawing.Point(203, 238);
            this.Signature.Name = "Signature";
            this.Signature.Size = new System.Drawing.Size(116, 16);
            this.Signature.TabIndex = 52;
            this.Signature.Text = "-130%";
            this.Signature.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SignatureUp
            // 
            this.SignatureUp.BackColor = System.Drawing.Color.Black;
            this.SignatureUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignatureUp.ForeColor = System.Drawing.Color.White;
            this.SignatureUp.Location = new System.Drawing.Point(139, 235);
            this.SignatureUp.Name = "SignatureUp";
            this.SignatureUp.Size = new System.Drawing.Size(58, 22);
            this.SignatureUp.TabIndex = 54;
            this.SignatureUp.Text = "+10%";
            this.SignatureUp.UseVisualStyleBackColor = false;
            this.SignatureUp.Click += new System.EventHandler(this.SignatureUp_Click);
            this.SignatureUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.SignatureUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // SignatureDown
            // 
            this.SignatureDown.BackColor = System.Drawing.Color.Black;
            this.SignatureDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignatureDown.ForeColor = System.Drawing.Color.White;
            this.SignatureDown.Location = new System.Drawing.Point(325, 235);
            this.SignatureDown.Name = "SignatureDown";
            this.SignatureDown.Size = new System.Drawing.Size(58, 22);
            this.SignatureDown.TabIndex = 54;
            this.SignatureDown.Text = "-10%";
            this.SignatureDown.UseVisualStyleBackColor = false;
            this.SignatureDown.Click += new System.EventHandler(this.SignatureDown_Click);
            this.SignatureDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.SignatureDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // FireRate
            // 
            this.FireRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FireRate.ForeColor = System.Drawing.Color.Lime;
            this.FireRate.Location = new System.Drawing.Point(203, 266);
            this.FireRate.Name = "FireRate";
            this.FireRate.Size = new System.Drawing.Size(116, 16);
            this.FireRate.TabIndex = 52;
            this.FireRate.Text = "-130%";
            this.FireRate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FireRateUp
            // 
            this.FireRateUp.BackColor = System.Drawing.Color.Black;
            this.FireRateUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FireRateUp.ForeColor = System.Drawing.Color.White;
            this.FireRateUp.Location = new System.Drawing.Point(139, 263);
            this.FireRateUp.Name = "FireRateUp";
            this.FireRateUp.Size = new System.Drawing.Size(58, 22);
            this.FireRateUp.TabIndex = 54;
            this.FireRateUp.Text = "+10%";
            this.FireRateUp.UseVisualStyleBackColor = false;
            this.FireRateUp.Click += new System.EventHandler(this.FireRateUp_Click);
            this.FireRateUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.FireRateUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // FireRateDown
            // 
            this.FireRateDown.BackColor = System.Drawing.Color.Black;
            this.FireRateDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FireRateDown.ForeColor = System.Drawing.Color.White;
            this.FireRateDown.Location = new System.Drawing.Point(325, 263);
            this.FireRateDown.Name = "FireRateDown";
            this.FireRateDown.Size = new System.Drawing.Size(58, 22);
            this.FireRateDown.TabIndex = 54;
            this.FireRateDown.Text = "-10%";
            this.FireRateDown.UseVisualStyleBackColor = false;
            this.FireRateDown.Click += new System.EventHandler(this.FireRateDown_Click);
            this.FireRateDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.FireRateDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MissileSpeed
            // 
            this.MissileSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MissileSpeed.ForeColor = System.Drawing.Color.Lime;
            this.MissileSpeed.Location = new System.Drawing.Point(203, 294);
            this.MissileSpeed.Name = "MissileSpeed";
            this.MissileSpeed.Size = new System.Drawing.Size(116, 16);
            this.MissileSpeed.TabIndex = 52;
            this.MissileSpeed.Text = "-130%";
            this.MissileSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MissileSpeedUp
            // 
            this.MissileSpeedUp.BackColor = System.Drawing.Color.Black;
            this.MissileSpeedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MissileSpeedUp.ForeColor = System.Drawing.Color.White;
            this.MissileSpeedUp.Location = new System.Drawing.Point(139, 291);
            this.MissileSpeedUp.Name = "MissileSpeedUp";
            this.MissileSpeedUp.Size = new System.Drawing.Size(58, 22);
            this.MissileSpeedUp.TabIndex = 54;
            this.MissileSpeedUp.Text = "+10%";
            this.MissileSpeedUp.UseVisualStyleBackColor = false;
            this.MissileSpeedUp.Click += new System.EventHandler(this.MissileSpeedUp_Click);
            this.MissileSpeedUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MissileSpeedUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MissileSpeedDown
            // 
            this.MissileSpeedDown.BackColor = System.Drawing.Color.Black;
            this.MissileSpeedDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MissileSpeedDown.ForeColor = System.Drawing.Color.White;
            this.MissileSpeedDown.Location = new System.Drawing.Point(325, 291);
            this.MissileSpeedDown.Name = "MissileSpeedDown";
            this.MissileSpeedDown.Size = new System.Drawing.Size(58, 22);
            this.MissileSpeedDown.TabIndex = 54;
            this.MissileSpeedDown.Text = "-10%";
            this.MissileSpeedDown.UseVisualStyleBackColor = false;
            this.MissileSpeedDown.Click += new System.EventHandler(this.MissileSpeedDown_Click);
            this.MissileSpeedDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MissileSpeedDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MiningEfficiency
            // 
            this.MiningEfficiency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MiningEfficiency.ForeColor = System.Drawing.Color.Lime;
            this.MiningEfficiency.Location = new System.Drawing.Point(203, 350);
            this.MiningEfficiency.Name = "MiningEfficiency";
            this.MiningEfficiency.Size = new System.Drawing.Size(116, 16);
            this.MiningEfficiency.TabIndex = 52;
            this.MiningEfficiency.Text = "-130%";
            this.MiningEfficiency.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MiningSpeedUp
            // 
            this.MiningSpeedUp.BackColor = System.Drawing.Color.Black;
            this.MiningSpeedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MiningSpeedUp.ForeColor = System.Drawing.Color.White;
            this.MiningSpeedUp.Location = new System.Drawing.Point(139, 347);
            this.MiningSpeedUp.Name = "MiningSpeedUp";
            this.MiningSpeedUp.Size = new System.Drawing.Size(58, 22);
            this.MiningSpeedUp.TabIndex = 54;
            this.MiningSpeedUp.Text = "+10%";
            this.MiningSpeedUp.UseVisualStyleBackColor = false;
            this.MiningSpeedUp.Click += new System.EventHandler(this.MiningSpeedUp_Click);
            this.MiningSpeedUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MiningSpeedUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MiningSpeedDown
            // 
            this.MiningSpeedDown.BackColor = System.Drawing.Color.Black;
            this.MiningSpeedDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MiningSpeedDown.ForeColor = System.Drawing.Color.White;
            this.MiningSpeedDown.Location = new System.Drawing.Point(325, 347);
            this.MiningSpeedDown.Name = "MiningSpeedDown";
            this.MiningSpeedDown.Size = new System.Drawing.Size(58, 22);
            this.MiningSpeedDown.TabIndex = 54;
            this.MiningSpeedDown.Text = "-10%";
            this.MiningSpeedDown.UseVisualStyleBackColor = false;
            this.MiningSpeedDown.Click += new System.EventHandler(this.MiningSpeedDown_Click);
            this.MiningSpeedDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MiningSpeedDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MiningCapacity
            // 
            this.MiningCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MiningCapacity.ForeColor = System.Drawing.Color.Lime;
            this.MiningCapacity.Location = new System.Drawing.Point(203, 378);
            this.MiningCapacity.Name = "MiningCapacity";
            this.MiningCapacity.Size = new System.Drawing.Size(116, 16);
            this.MiningCapacity.TabIndex = 52;
            this.MiningCapacity.Text = "-130%";
            this.MiningCapacity.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MiningCapacityUp
            // 
            this.MiningCapacityUp.BackColor = System.Drawing.Color.Black;
            this.MiningCapacityUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MiningCapacityUp.ForeColor = System.Drawing.Color.White;
            this.MiningCapacityUp.Location = new System.Drawing.Point(139, 375);
            this.MiningCapacityUp.Name = "MiningCapacityUp";
            this.MiningCapacityUp.Size = new System.Drawing.Size(58, 22);
            this.MiningCapacityUp.TabIndex = 54;
            this.MiningCapacityUp.Text = "+10%";
            this.MiningCapacityUp.UseVisualStyleBackColor = false;
            this.MiningCapacityUp.Click += new System.EventHandler(this.MiningCapacityUp_Click);
            this.MiningCapacityUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MiningCapacityUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MiningCapacityDown
            // 
            this.MiningCapacityDown.BackColor = System.Drawing.Color.Black;
            this.MiningCapacityDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MiningCapacityDown.ForeColor = System.Drawing.Color.White;
            this.MiningCapacityDown.Location = new System.Drawing.Point(325, 375);
            this.MiningCapacityDown.Name = "MiningCapacityDown";
            this.MiningCapacityDown.Size = new System.Drawing.Size(58, 22);
            this.MiningCapacityDown.TabIndex = 54;
            this.MiningCapacityDown.Text = "-10%";
            this.MiningCapacityDown.UseVisualStyleBackColor = false;
            this.MiningCapacityDown.Click += new System.EventHandler(this.MiningCapacityDown_Click);
            this.MiningCapacityDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MiningCapacityDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // BalancedLabel
            // 
            this.BalancedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BalancedLabel.ForeColor = System.Drawing.Color.Red;
            this.BalancedLabel.Location = new System.Drawing.Point(40, 400);
            this.BalancedLabel.Name = "BalancedLabel";
            this.BalancedLabel.Size = new System.Drawing.Size(343, 18);
            this.BalancedLabel.TabIndex = 60;
            this.BalancedLabel.Text = "...";
            this.BalancedLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Lime;
            this.label11.Location = new System.Drawing.Point(10, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(244, 16);
            this.label11.TabIndex = 52;
            this.label11.Text = "Faction Bonuses:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MissileTracking
            // 
            this.MissileTracking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MissileTracking.ForeColor = System.Drawing.Color.Lime;
            this.MissileTracking.Location = new System.Drawing.Point(203, 322);
            this.MissileTracking.Name = "MissileTracking";
            this.MissileTracking.Size = new System.Drawing.Size(116, 16);
            this.MissileTracking.TabIndex = 52;
            this.MissileTracking.Text = "-130%";
            this.MissileTracking.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MissileTrackingUp
            // 
            this.MissileTrackingUp.BackColor = System.Drawing.Color.Black;
            this.MissileTrackingUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MissileTrackingUp.ForeColor = System.Drawing.Color.White;
            this.MissileTrackingUp.Location = new System.Drawing.Point(139, 319);
            this.MissileTrackingUp.Name = "MissileTrackingUp";
            this.MissileTrackingUp.Size = new System.Drawing.Size(58, 22);
            this.MissileTrackingUp.TabIndex = 54;
            this.MissileTrackingUp.Text = "+10%";
            this.MissileTrackingUp.UseVisualStyleBackColor = false;
            this.MissileTrackingUp.Click += new System.EventHandler(this.MissileTrackingUp_Click);
            this.MissileTrackingUp.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MissileTrackingUp.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MissileTrackingDown
            // 
            this.MissileTrackingDown.BackColor = System.Drawing.Color.Black;
            this.MissileTrackingDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MissileTrackingDown.ForeColor = System.Drawing.Color.White;
            this.MissileTrackingDown.Location = new System.Drawing.Point(325, 319);
            this.MissileTrackingDown.Name = "MissileTrackingDown";
            this.MissileTrackingDown.Size = new System.Drawing.Size(58, 22);
            this.MissileTrackingDown.TabIndex = 54;
            this.MissileTrackingDown.Text = "-10%";
            this.MissileTrackingDown.UseVisualStyleBackColor = false;
            this.MissileTrackingDown.Click += new System.EventHandler(this.MissileTrackingDown_Click);
            this.MissileTrackingDown.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MissileTrackingDown.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(22, 324);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 60;
            this.label13.Text = "Missile Tracking:";
            // 
            // FactionPicture
            // 
            this.FactionPicture.Location = new System.Drawing.Point(389, 66);
            this.FactionPicture.Name = "FactionPicture";
            this.FactionPicture.Size = new System.Drawing.Size(200, 200);
            this.FactionPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FactionPicture.TabIndex = 61;
            this.FactionPicture.TabStop = false;
            // 
            // RandomImage
            // 
            this.RandomImage.BackColor = System.Drawing.Color.Black;
            this.RandomImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RandomImage.ForeColor = System.Drawing.Color.White;
            this.RandomImage.Location = new System.Drawing.Point(492, 272);
            this.RandomImage.Name = "RandomImage";
            this.RandomImage.Size = new System.Drawing.Size(97, 27);
            this.RandomImage.TabIndex = 56;
            this.RandomImage.Text = "Random Image";
            this.RandomImage.UseVisualStyleBackColor = false;
            this.RandomImage.Click += new System.EventHandler(this.RandomImage_Click);
            this.RandomImage.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.RandomImage.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // PlayerName
            // 
            this.PlayerName.BackColor = System.Drawing.Color.Black;
            this.PlayerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PlayerName.Location = new System.Drawing.Point(389, 37);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(137, 23);
            this.PlayerName.TabIndex = 62;
            this.PlayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RandomCommanderName
            // 
            this.RandomCommanderName.BackColor = System.Drawing.Color.Black;
            this.RandomCommanderName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RandomCommanderName.ForeColor = System.Drawing.Color.White;
            this.RandomCommanderName.Location = new System.Drawing.Point(531, 37);
            this.RandomCommanderName.Name = "RandomCommanderName";
            this.RandomCommanderName.Size = new System.Drawing.Size(58, 22);
            this.RandomCommanderName.TabIndex = 56;
            this.RandomCommanderName.Text = "Random";
            this.RandomCommanderName.UseVisualStyleBackColor = false;
            this.RandomCommanderName.Click += new System.EventHandler(this.RandomCommanderName_Click);
            this.RandomCommanderName.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.RandomCommanderName.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Default
            // 
            this.Default.BackColor = System.Drawing.Color.Black;
            this.Default.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Default.ForeColor = System.Drawing.Color.White;
            this.Default.Location = new System.Drawing.Point(389, 5);
            this.Default.Name = "Default";
            this.Default.Size = new System.Drawing.Size(58, 22);
            this.Default.TabIndex = 56;
            this.Default.Text = "Default";
            this.Default.UseVisualStyleBackColor = false;
            this.Default.Click += new System.EventHandler(this.Default_Click);
            this.Default.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Default.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Lime;
            this.label12.Location = new System.Drawing.Point(389, 315);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 20);
            this.label12.TabIndex = 52;
            this.label12.Text = "Race:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FactionRace
            // 
            this.FactionRace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.FactionRace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FactionRace.FormattingEnabled = true;
            this.FactionRace.Location = new System.Drawing.Point(453, 316);
            this.FactionRace.Name = "FactionRace";
            this.FactionRace.Size = new System.Drawing.Size(136, 21);
            this.FactionRace.TabIndex = 53;
            this.FactionRace.SelectedValueChanged += new System.EventHandler(this.FactionRace_SelectedValueChanged);
            // 
            // RacePicture
            // 
            this.RacePicture.Location = new System.Drawing.Point(389, 343);
            this.RacePicture.Name = "RacePicture";
            this.RacePicture.Size = new System.Drawing.Size(200, 75);
            this.RacePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RacePicture.TabIndex = 63;
            this.RacePicture.TabStop = false;
            // 
            // FactionDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(604, 479);
            this.Controls.Add(this.RacePicture);
            this.Controls.Add(this.PlayerName);
            this.Controls.Add(this.BalancedLabel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label51);
            this.Controls.Add(this.FactionName);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.RandomImage);
            this.Controls.Add(this.RandomCommanderName);
            this.Controls.Add(this.RandomName);
            this.Controls.Add(this.Default);
            this.Controls.Add(this.Random);
            this.Controls.Add(this.MiningCapacityDown);
            this.Controls.Add(this.MissileTrackingDown);
            this.Controls.Add(this.MiningSpeedDown);
            this.Controls.Add(this.MissileSpeedDown);
            this.Controls.Add(this.FireRateDown);
            this.Controls.Add(this.SignatureDown);
            this.Controls.Add(this.ScanRangeDown);
            this.Controls.Add(this.HealthDown);
            this.Controls.Add(this.SpeedDown);
            this.Controls.Add(this.ResearchCostDown);
            this.Controls.Add(this.ResearchTimeDown);
            this.Controls.Add(this.MiningCapacityUp);
            this.Controls.Add(this.MissileTrackingUp);
            this.Controls.Add(this.MiningSpeedUp);
            this.Controls.Add(this.MissileSpeedUp);
            this.Controls.Add(this.FireRateUp);
            this.Controls.Add(this.SignatureUp);
            this.Controls.Add(this.ScanRangeUp);
            this.Controls.Add(this.HealthUp);
            this.Controls.Add(this.SpeedUp);
            this.Controls.Add(this.ResearchCostUp);
            this.Controls.Add(this.ResearchTimeUp);
            this.Controls.Add(this.LoadPreset);
            this.Controls.Add(this.SavePreset);
            this.Controls.Add(this.Done);
            this.Controls.Add(this.MiningCapacity);
            this.Controls.Add(this.MissileTracking);
            this.Controls.Add(this.MiningEfficiency);
            this.Controls.Add(this.MissileSpeed);
            this.Controls.Add(this.FireRate);
            this.Controls.Add(this.Signature);
            this.Controls.Add(this.ScanRange);
            this.Controls.Add(this.Health);
            this.Controls.Add(this.Speed);
            this.Controls.Add(this.ResearchCost);
            this.Controls.Add(this.ResearchTime);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.FactionRace);
            this.Controls.Add(this.CustomPresets);
            this.Controls.Add(this.FactionPicture);
            this.ForeColor = System.Drawing.Color.Lime;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FactionDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allegiance Forms - Faction Details";
            this.Load += new System.EventHandler(this.FactionDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FactionPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RacePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Random;
        private System.Windows.Forms.Button LoadPreset;
        private System.Windows.Forms.Button SavePreset;
        private System.Windows.Forms.Button Done;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox CustomPresets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FactionName;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button ResearchTimeUp;
        private System.Windows.Forms.Button ResearchTimeDown;
        private System.Windows.Forms.Label ResearchTime;
        private System.Windows.Forms.Button RandomName;
        private System.Windows.Forms.Label ResearchCost;
        private System.Windows.Forms.Button ResearchCostUp;
        private System.Windows.Forms.Button ResearchCostDown;
        private System.Windows.Forms.Label Speed;
        private System.Windows.Forms.Button SpeedUp;
        private System.Windows.Forms.Button SpeedDown;
        private System.Windows.Forms.Label Health;
        private System.Windows.Forms.Button HealthUp;
        private System.Windows.Forms.Button HealthDown;
        private System.Windows.Forms.Label ScanRange;
        private System.Windows.Forms.Button ScanRangeUp;
        private System.Windows.Forms.Button ScanRangeDown;
        private System.Windows.Forms.Label Signature;
        private System.Windows.Forms.Button SignatureUp;
        private System.Windows.Forms.Button SignatureDown;
        private System.Windows.Forms.Label FireRate;
        private System.Windows.Forms.Button FireRateUp;
        private System.Windows.Forms.Button FireRateDown;
        private System.Windows.Forms.Label MissileSpeed;
        private System.Windows.Forms.Button MissileSpeedUp;
        private System.Windows.Forms.Button MissileSpeedDown;
        private System.Windows.Forms.Label MiningEfficiency;
        private System.Windows.Forms.Button MiningSpeedUp;
        private System.Windows.Forms.Button MiningSpeedDown;
        private System.Windows.Forms.Label MiningCapacity;
        private System.Windows.Forms.Button MiningCapacityUp;
        private System.Windows.Forms.Button MiningCapacityDown;
        private System.Windows.Forms.Label BalancedLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label MissileTracking;
        private System.Windows.Forms.Button MissileTrackingUp;
        private System.Windows.Forms.Button MissileTrackingDown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox FactionPicture;
        private System.Windows.Forms.Button RandomImage;
        private System.Windows.Forms.TextBox PlayerName;
        private System.Windows.Forms.Button RandomCommanderName;
        private System.Windows.Forms.Button Default;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox FactionRace;
        private System.Windows.Forms.PictureBox RacePicture;
    }
}