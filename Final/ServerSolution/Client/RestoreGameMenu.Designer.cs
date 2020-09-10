namespace Client
{
    partial class RestoreGameMenu
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
            this.TblGamesDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TblGamesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // TblGamesDataGridView
            // 
            this.TblGamesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TblGamesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TblGamesDataGridView.Location = new System.Drawing.Point(19, 73);
            this.TblGamesDataGridView.Name = "TblGamesDataGridView";
            this.TblGamesDataGridView.RowHeadersWidth = 62;
            this.TblGamesDataGridView.RowTemplate.Height = 28;
            this.TblGamesDataGridView.Size = new System.Drawing.Size(792, 390);
            this.TblGamesDataGridView.TabIndex = 0;
            this.TblGamesDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(437, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose Game To Restore";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(19, 495);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(256, 102);
            this.button1.TabIndex = 2;
            this.button1.Text = "Restore Game";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // RestoreGameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 641);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TblGamesDataGridView);
            this.Name = "RestoreGameMenu";
            this.Text = "RestoreGameMenu";
            this.Load += new System.EventHandler(this.RestoreGameMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TblGamesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView TblGamesDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}