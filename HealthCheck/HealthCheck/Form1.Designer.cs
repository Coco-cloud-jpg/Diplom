namespace HealthCheck
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.activityToggle = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Label();
            this.buttonWrapper = new System.Windows.Forms.Panel();
            this.alertField = new System.Windows.Forms.Label();
            this.buttonWrapper.SuspendLayout();
            this.SuspendLayout();
            // 
            // activityToggle
            // 
            this.activityToggle.BackColor = System.Drawing.Color.Green;
            this.activityToggle.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.activityToggle.ForeColor = System.Drawing.SystemColors.Control;
            this.activityToggle.Location = new System.Drawing.Point(3, 3);
            this.activityToggle.Name = "activityToggle";
            this.activityToggle.Size = new System.Drawing.Size(84, 79);
            this.activityToggle.TabIndex = 0;
            this.activityToggle.Text = "START";
            this.activityToggle.UseVisualStyleBackColor = false;
            this.activityToggle.Click += new System.EventHandler(this.activityToggle_Click);
            // 
            // timer
            // 
            this.timer.AutoSize = true;
            this.timer.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.timer.Location = new System.Drawing.Point(41, 43);
            this.timer.Name = "timer";
            this.timer.Size = new System.Drawing.Size(173, 54);
            this.timer.TabIndex = 1;
            this.timer.Text = "00:00:00";
            // 
            // buttonWrapper
            // 
            this.buttonWrapper.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonWrapper.Controls.Add(this.activityToggle);
            this.buttonWrapper.Location = new System.Drawing.Point(79, 114);
            this.buttonWrapper.Name = "buttonWrapper";
            this.buttonWrapper.Size = new System.Drawing.Size(90, 85);
            this.buttonWrapper.TabIndex = 2;
            // 
            // alertField
            // 
            this.alertField.AutoSize = true;
            this.alertField.BackColor = System.Drawing.Color.IndianRed;
            this.alertField.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.alertField.Location = new System.Drawing.Point(16, 96);
            this.alertField.Name = "alertField";
            this.alertField.Size = new System.Drawing.Size(0, 15);
            this.alertField.TabIndex = 3;
            this.alertField.TextAlign = ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 235);
            this.Controls.Add(this.alertField);
            this.Controls.Add(this.timer);
            this.Controls.Add(this.buttonWrapper);
            this.Name = "Form1";
            this.Text = "Health Checker";
            this.buttonWrapper.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            this.StartPosition = FormStartPosition.Manual;
            foreach (var scrn in Screen.AllScreens)
            {
                if (scrn.Bounds.Contains(this.Location))
                {
                    this.Location = new Point(scrn.Bounds.Right - this.Width, scrn.Bounds.Bottom - this.Height - 100);
                    return;
                }
            }
        }

        #endregion

        private Button activityToggle;
        private Label timer;
        private Panel buttonWrapper;
        private Label alertField;
    }
}