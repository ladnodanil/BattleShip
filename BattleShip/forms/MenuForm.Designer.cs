namespace BattleShip
{
    partial class MenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPlayWithPC = new System.Windows.Forms.Button();
            this.buttonPlayOnline = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // buttonPlayWithPC
            // 
            this.buttonPlayWithPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPlayWithPC.Location = new System.Drawing.Point(224, 139);
            this.buttonPlayWithPC.Name = "buttonPlayWithPC";
            this.buttonPlayWithPC.Size = new System.Drawing.Size(364, 89);
            this.buttonPlayWithPC.TabIndex = 1;
            this.buttonPlayWithPC.Text = "Игра с ПК";
            this.buttonPlayWithPC.UseVisualStyleBackColor = true;
            this.buttonPlayWithPC.Click += new System.EventHandler(this.buttonPlayWithPC_Click);
            // 
            // buttonPlayOnline
            // 
            this.buttonPlayOnline.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPlayOnline.Location = new System.Drawing.Point(224, 289);
            this.buttonPlayOnline.Name = "buttonPlayOnline";
            this.buttonPlayOnline.Size = new System.Drawing.Size(364, 89);
            this.buttonPlayOnline.TabIndex = 2;
            this.buttonPlayOnline.Text = "Игра по сети";
            this.buttonPlayOnline.UseVisualStyleBackColor = true;
            this.buttonPlayOnline.Click += new System.EventHandler(this.buttonPlayOnline_Click);
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonPlayOnline);
            this.Controls.Add(this.buttonPlayWithPC);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MenuForm";
            this.Text = "PlaceForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonPlayWithPC;
        private System.Windows.Forms.Button buttonPlayOnline;
    }
}