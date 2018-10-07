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
            this.btnNextPosition = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnUndoMove = new System.Windows.Forms.Button();
            this.btnRobot0 = new System.Windows.Forms.Button();
            this.btnRobot1 = new System.Windows.Forms.Button();
            this.btnRobot2 = new System.Windows.Forms.Button();
            this.btnRobot3 = new System.Windows.Forms.Button();
            this.txtSolvedNumberOfMoves = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(528, 12);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(75, 23);
            this.btnNewGame.TabIndex = 1;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(642, 82);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(51, 23);
            this.btnSolve.TabIndex = 2;
            this.btnSolve.Text = "??";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // txtSolutionPath
            // 
            this.txtSolutionPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSolutionPath.Location = new System.Drawing.Point(528, 111);
            this.txtSolutionPath.Name = "txtSolutionPath";
            this.txtSolutionPath.Size = new System.Drawing.Size(221, 399);
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
            this.txtNumberOfMoves.Location = new System.Drawing.Point(609, 15);
            this.txtNumberOfMoves.Name = "txtNumberOfMoves";
            this.txtNumberOfMoves.Size = new System.Drawing.Size(58, 20);
            this.txtNumberOfMoves.TabIndex = 6;
            // 
            // btnNextPosition
            // 
            this.btnNextPosition.Location = new System.Drawing.Point(698, 82);
            this.btnNextPosition.Name = "btnNextPosition";
            this.btnNextPosition.Size = new System.Drawing.Size(51, 23);
            this.btnNextPosition.TabIndex = 7;
            this.btnNextPosition.Text = ">";
            this.btnNextPosition.UseVisualStyleBackColor = true;
            this.btnNextPosition.Visible = false;
            this.btnNextPosition.Click += new System.EventHandler(this.btnNextPosition_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(528, 82);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(51, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "<<";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnUndoMove
            // 
            this.btnUndoMove.Location = new System.Drawing.Point(585, 82);
            this.btnUndoMove.Name = "btnUndoMove";
            this.btnUndoMove.Size = new System.Drawing.Size(51, 23);
            this.btnUndoMove.TabIndex = 9;
            this.btnUndoMove.Text = "<";
            this.btnUndoMove.UseVisualStyleBackColor = true;
            this.btnUndoMove.Click += new System.EventHandler(this.btnUndoMove_Click);
            // 
            // btnRobot0
            // 
            this.btnRobot0.Location = new System.Drawing.Point(528, 53);
            this.btnRobot0.Name = "btnRobot0";
            this.btnRobot0.Size = new System.Drawing.Size(51, 23);
            this.btnRobot0.TabIndex = 10;
            this.btnRobot0.UseVisualStyleBackColor = true;
            this.btnRobot0.Click += new System.EventHandler(this.btnRobot0_Click);
            this.btnRobot0.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtonCell_KeyDown);
            // 
            // btnRobot1
            // 
            this.btnRobot1.Location = new System.Drawing.Point(585, 53);
            this.btnRobot1.Name = "btnRobot1";
            this.btnRobot1.Size = new System.Drawing.Size(51, 23);
            this.btnRobot1.TabIndex = 11;
            this.btnRobot1.UseVisualStyleBackColor = true;
            this.btnRobot1.Click += new System.EventHandler(this.btnRobot1_Click);
            this.btnRobot1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtonCell_KeyDown);
            // 
            // btnRobot2
            // 
            this.btnRobot2.Location = new System.Drawing.Point(642, 53);
            this.btnRobot2.Name = "btnRobot2";
            this.btnRobot2.Size = new System.Drawing.Size(51, 23);
            this.btnRobot2.TabIndex = 12;
            this.btnRobot2.UseVisualStyleBackColor = true;
            this.btnRobot2.Click += new System.EventHandler(this.btnRobot2_Click);
            this.btnRobot2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtonCell_KeyDown);
            // 
            // btnRobot3
            // 
            this.btnRobot3.Location = new System.Drawing.Point(698, 53);
            this.btnRobot3.Name = "btnRobot3";
            this.btnRobot3.Size = new System.Drawing.Size(51, 23);
            this.btnRobot3.TabIndex = 13;
            this.btnRobot3.UseVisualStyleBackColor = true;
            this.btnRobot3.Click += new System.EventHandler(this.btnRobot3_Click);
            this.btnRobot3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ButtonCell_KeyDown);
            // 
            // txtSolvedNumberOfMoves
            // 
            this.txtSolvedNumberOfMoves.Location = new System.Drawing.Point(673, 15);
            this.txtSolvedNumberOfMoves.Name = "txtSolvedNumberOfMoves";
            this.txtSolvedNumberOfMoves.Size = new System.Drawing.Size(58, 20);
            this.txtSolvedNumberOfMoves.TabIndex = 14;
            // 
            // WindowsFormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 515);
            this.Controls.Add(this.txtSolvedNumberOfMoves);
            this.Controls.Add(this.btnRobot3);
            this.Controls.Add(this.btnRobot2);
            this.Controls.Add(this.btnRobot1);
            this.Controls.Add(this.btnRobot0);
            this.Controls.Add(this.btnUndoMove);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnNextPosition);
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
        private System.Windows.Forms.Button btnNextPosition;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnUndoMove;
        private System.Windows.Forms.Button btnRobot0;
        private System.Windows.Forms.Button btnRobot1;
        private System.Windows.Forms.Button btnRobot2;
        private System.Windows.Forms.Button btnRobot3;
        private System.Windows.Forms.TextBox txtSolvedNumberOfMoves;
    }
}