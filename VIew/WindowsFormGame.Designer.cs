namespace ScalingSpoon.View
{
    partial class WindowsFormGame
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
            this.txtGameDescription = new System.Windows.Forms.TextBox();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtGameDescription
            // 
            this.txtGameDescription.Location = new System.Drawing.Point(528, 53);
            this.txtGameDescription.MinimumSize = new System.Drawing.Size(100, 450);
            this.txtGameDescription.Multiline = true;
            this.txtGameDescription.Name = "txtGameDescription";
            this.txtGameDescription.Size = new System.Drawing.Size(196, 457);
            this.txtGameDescription.TabIndex = 0;
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(592, 24);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(75, 23);
            this.btnNewGame.TabIndex = 1;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // WindowsFormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 515);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.txtGameDescription);
            this.Name = "WindowsFormGame";
            this.Text = "WindowsFormGame";
            this.Load += new System.EventHandler(this.WindowsFormGame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGameDescription;
        private System.Windows.Forms.Button btnNewGame;
    }
}