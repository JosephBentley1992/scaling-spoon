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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.txtSolutionPath.Size = new System.Drawing.Size(221, 425);
            this.txtSolutionPath.TabIndex = 4;
            this.txtSolutionPath.Text = "";
            // 
            // boardLocation
            // 
            this.boardLocation.Location = new System.Drawing.Point(0, 27);
            this.boardLocation.Name = "boardLocation";
            this.boardLocation.Size = new System.Drawing.Size(520, 509);
            this.boardLocation.TabIndex = 5;
            this.boardLocation.Visible = false;
            // 
            // txtNumberOfMoves
            // 
            this.txtNumberOfMoves.Location = new System.Drawing.Point(528, 27);
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
            this.btnRobot0.FlatAppearance.BorderSize = 0;
            this.btnRobot0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.btnRobot1.FlatAppearance.BorderSize = 0;
            this.btnRobot1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.btnRobot2.FlatAppearance.BorderSize = 0;
            this.btnRobot2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.btnRobot3.FlatAppearance.BorderSize = 0;
            this.btnRobot3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.txtSolvedNumberOfMoves.Location = new System.Drawing.Point(592, 27);
            this.txtSolvedNumberOfMoves.Name = "txtSolvedNumberOfMoves";
            this.txtSolvedNumberOfMoves.Size = new System.Drawing.Size(58, 20);
            this.txtSolvedNumberOfMoves.TabIndex = 14;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(761, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.newGameToolStripMenuItem.Text = "New Game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exportToolStripMenuItem.Text = "Export (TODO)";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // WindowsFormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 543);
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
            this.Controls.Add(this.boardLocation);
            this.Controls.Add(this.menuStrip1);
            this.Name = "WindowsFormGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scaling Spoon";
            this.Load += new System.EventHandler(this.WindowsFormGame_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    }
}