namespace BookShop
{
    partial class TitleForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelOverlay;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnEasy;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnHard;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Label lblTeam;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TitleForm));
            panelOverlay = new Panel();
            lblTitle = new Label();
            btnEasy = new Button();
            btnNormal = new Button();
            btnHard = new Button();
            btnAbout = new Button();
            lblTeam = new Label();
            pictureBox1 = new PictureBox();
            panelOverlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelOverlay
            // 
            panelOverlay.BackColor = Color.FromArgb(0, 0, 0, 0);
            panelOverlay.Controls.Add(pictureBox1);
            panelOverlay.Controls.Add(lblTitle);
            panelOverlay.Controls.Add(btnEasy);
            panelOverlay.Controls.Add(btnNormal);
            panelOverlay.Controls.Add(btnHard);
            panelOverlay.Controls.Add(btnAbout);
            panelOverlay.Controls.Add(lblTeam);
            panelOverlay.Dock = DockStyle.Fill;
            panelOverlay.Location = new Point(0, 0);
            panelOverlay.Margin = new Padding(3, 2, 3, 2);
            panelOverlay.Name = "panelOverlay";
            panelOverlay.Size = new Size(525, 338);
            panelOverlay.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Location = new Point(44, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(438, 52);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "КНИЖНЫЙ МАГНАТ";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnEasy
            // 
            btnEasy.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnEasy.Location = new Point(44, 251);
            btnEasy.Margin = new Padding(3, 2, 3, 2);
            btnEasy.Name = "btnEasy";
            btnEasy.Size = new Size(118, 34);
            btnEasy.TabIndex = 1;
            btnEasy.Text = "Лёгкий";
            btnEasy.UseVisualStyleBackColor = true;
            btnEasy.Click += btnEasy_Click;
            // 
            // btnNormal
            // 
            btnNormal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnNormal.Location = new Point(203, 251);
            btnNormal.Margin = new Padding(3, 2, 3, 2);
            btnNormal.Name = "btnNormal";
            btnNormal.Size = new Size(125, 34);
            btnNormal.TabIndex = 2;
            btnNormal.Text = "Нормальный";
            btnNormal.UseVisualStyleBackColor = true;
            btnNormal.Click += btnNormal_Click;
            // 
            // btnHard
            // 
            btnHard.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnHard.Location = new Point(360, 251);
            btnHard.Margin = new Padding(3, 2, 3, 2);
            btnHard.Name = "btnHard";
            btnHard.Size = new Size(122, 34);
            btnHard.TabIndex = 3;
            btnHard.Text = "Сложный";
            btnHard.UseVisualStyleBackColor = true;
            btnHard.Click += btnHard_Click;
            // 
            // btnAbout
            // 
            btnAbout.Font = new Font("Segoe UI", 11F);
            btnAbout.Location = new Point(301, 302);
            btnAbout.Margin = new Padding(3, 2, 3, 2);
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new Size(105, 27);
            btnAbout.TabIndex = 4;
            btnAbout.Text = "Об игре";
            btnAbout.UseVisualStyleBackColor = true;
            btnAbout.Click += btnAbout_Click;
            // 
            // lblTeam
            // 
            lblTeam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTeam.AutoSize = true;
            lblTeam.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblTeam.ForeColor = Color.Black;
            lblTeam.Location = new Point(413, 314);
            lblTeam.Name = "lblTeam";
            lblTeam.Size = new Size(109, 15);
            lblTeam.TabIndex = 5;
            lblTeam.Text = "Команда DevTeam";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(114, 54);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(292, 192);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // TitleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(525, 338);
            Controls.Add(panelOverlay);
            Margin = new Padding(3, 2, 3, 2);
            Name = "TitleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Книжный Магазин";
            panelOverlay.ResumeLayout(false);
            panelOverlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }
        private PictureBox pictureBox1;
    }
}