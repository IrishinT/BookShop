namespace BookShop
{
    partial class BookStoreForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookStoreForm));
            bookFormTabControl = new TabControl();
            shopPage = new TabPage();
            newBookPage = new TabPage();
            bookFormTabControl.SuspendLayout();
            SuspendLayout();
            // 
            // bookFormTabControl
            // 
            bookFormTabControl.Controls.Add(shopPage);
            bookFormTabControl.Controls.Add(newBookPage);
            bookFormTabControl.Dock = DockStyle.Fill;
            bookFormTabControl.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bookFormTabControl.Location = new Point(0, 0);
            bookFormTabControl.Name = "bookFormTabControl";
            bookFormTabControl.SelectedIndex = 0;
            bookFormTabControl.Size = new Size(800, 450);
            bookFormTabControl.TabIndex = 0;
            // 
            // shopPage
            // 
            shopPage.Location = new Point(4, 29);
            shopPage.Name = "shopPage";
            shopPage.Padding = new Padding(3);
            shopPage.Size = new Size(792, 417);
            shopPage.TabIndex = 0;
            shopPage.Text = "Магазин";
            shopPage.UseVisualStyleBackColor = true;
            // 
            // newBookPage
            // 
            newBookPage.Location = new Point(4, 29);
            newBookPage.Name = "newBookPage";
            newBookPage.Padding = new Padding(3);
            newBookPage.Size = new Size(792, 417);
            newBookPage.TabIndex = 1;
            newBookPage.Text = "Новая книга";
            newBookPage.UseVisualStyleBackColor = true;
            // 
            // BookStoreForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(bookFormTabControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BookStoreForm";
            Text = "Книжный магазин";
            bookFormTabControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl bookFormTabControl;
        private TabPage shopPage;
        private TabPage newBookPage;
    }
}
