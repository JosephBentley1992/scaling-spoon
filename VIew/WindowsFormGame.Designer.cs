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
            this.btnNewGame = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.txtSolutionPath = new System.Windows.Forms.RichTextBox();
            this.boardLocation = new System.Windows.Forms.FlowLayoutPanel();
            this.txtNumberOfMoves = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(528, 24);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(75, 23);
            this.btnNewGame.TabIndex = 1;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(674, 24);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(75, 23);
            this.btnSolve.TabIndex = 2;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // txtSolutionPath
            // 
            this.txtSolutionPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSolutionPath.Location = new System.Drawing.Point(528, 53);
            this.txtSolutionPath.Name = "txtSolutionPath";
            this.txtSolutionPath.Size = new System.Drawing.Size(221, 457);
            this.txtSolutionPath.TabIndex = 4;
            this.txtSolutionPath.Text = "";
            // 
            // boardLocation
            // 
            this.boardLocation.Location = new System.Drawing.Point(2, 1);
            this.boardLocation.Name = "boardLocation";
            this.boardLocation.Size = new System.Drawing.Size(520, 509);
            this.boardLocation.TabIndex = 5;
            this.boardLocation.Visible = false;
            // 
            // txtNumberOfMoves
            // 
            this.txtNumberOfMoves.Location = new System.Drawing.Point(609, 27);
            this.txtNumberOfMoves.Name = "txtNumberOfMoves";
            this.txtNumberOfMoves.Size = new System.Drawing.Size(58, 20);
            this.txtNumberOfMoves.TabIndex = 6;
            // 
            // WindowsFormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 515);
            this.Controls.Add(this.txtNumberOfMoves);
            this.Controls.Add(this.txtSolutionPath);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.boardLocation);
            this.Name = "WindowsFormGame";
            this.Text = "WindowsFormGame";
            this.Load += new System.EventHandler(this.WindowsFormGame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.RichTextBox txtSolutionPath;
        private System.Windows.Forms.FlowLayoutPanel boardLocation;
        private System.Windows.Forms.TextBox txtNumberOfMoves;
    }
}