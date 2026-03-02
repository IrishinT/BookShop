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
            tableLayoutPanel1 = new TableLayoutPanel();
            idField = new TextBox();
            idLabel = new Label();
            priceNumbericUpDown = new NumericUpDown();
            priceLabel = new Label();
            pagesCountLabel = new Label();
            ganreLabel = new Label();
            authorField = new TextBox();
            bookNameField = new TextBox();
            bookNameLabel = new Label();
            authorLabel = new Label();
            pagesCountNumbericUpDown = new NumericUpDown();
            flowLayoutPanel1 = new FlowLayoutPanel();
            createBookBtn = new Button();
            generateBookBtn = new Button();
            ganreComboBox = new ComboBox();
            bookFormTabControl.SuspendLayout();
            newBookPage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)priceNumbericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pagesCountNumbericUpDown).BeginInit();
            flowLayoutPanel1.SuspendLayout();
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
            shopPage.BackgroundImage = (Image)resources.GetObject("shopPage.BackgroundImage");
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
            newBookPage.BackColor = Color.White;
            newBookPage.BackgroundImage = (Image)resources.GetObject("newBookPage.BackgroundImage");
            newBookPage.BackgroundImageLayout = ImageLayout.Center;
            newBookPage.Controls.Add(tableLayoutPanel1);
            newBookPage.Location = new Point(4, 29);
            newBookPage.Name = "newBookPage";
            newBookPage.Padding = new Padding(3);
            newBookPage.Size = new Size(792, 417);
            newBookPage.TabIndex = 1;
            newBookPage.Text = "Новая книга";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(idField, 1, 5);
            tableLayoutPanel1.Controls.Add(idLabel, 0, 5);
            tableLayoutPanel1.Controls.Add(priceNumbericUpDown, 1, 4);
            tableLayoutPanel1.Controls.Add(priceLabel, 0, 4);
            tableLayoutPanel1.Controls.Add(pagesCountLabel, 0, 3);
            tableLayoutPanel1.Controls.Add(ganreLabel, 0, 2);
            tableLayoutPanel1.Controls.Add(authorField, 1, 1);
            tableLayoutPanel1.Controls.Add(bookNameField, 1, 0);
            tableLayoutPanel1.Controls.Add(bookNameLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(authorLabel, 0, 1);
            tableLayoutPanel1.Controls.Add(pagesCountNumbericUpDown, 1, 3);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 6);
            tableLayoutPanel1.Controls.Add(ganreComboBox, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(20, 40, 20, 20);
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(786, 411);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // idField
            // 
            idField.Anchor = AnchorStyles.Left;
            idField.Location = new Point(185, 246);
            idField.Margin = new Padding(15, 3, 3, 3);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(134, 27);
            idField.TabIndex = 11;
            // 
            // idLabel
            // 
            idLabel.Anchor = AnchorStyles.Right;
            idLabel.AutoSize = true;
            idLabel.Location = new Point(131, 250);
            idLabel.Margin = new Padding(3, 0, 15, 0);
            idLabel.Name = "idLabel";
            idLabel.Size = new Size(24, 20);
            idLabel.TabIndex = 10;
            idLabel.Text = "ID";
            // 
            // priceNumbericUpDown
            // 
            priceNumbericUpDown.Anchor = AnchorStyles.Left;
            priceNumbericUpDown.Location = new Point(185, 206);
            priceNumbericUpDown.Margin = new Padding(15, 3, 3, 3);
            priceNumbericUpDown.Name = "priceNumbericUpDown";
            priceNumbericUpDown.Size = new Size(134, 27);
            priceNumbericUpDown.TabIndex = 9;
            // 
            // priceLabel
            // 
            priceLabel.Anchor = AnchorStyles.Right;
            priceLabel.AutoSize = true;
            priceLabel.Location = new Point(110, 210);
            priceLabel.Margin = new Padding(3, 0, 15, 0);
            priceLabel.Name = "priceLabel";
            priceLabel.Size = new Size(45, 20);
            priceLabel.TabIndex = 8;
            priceLabel.Text = "Цена";
            // 
            // pagesCountLabel
            // 
            pagesCountLabel.Anchor = AnchorStyles.Right;
            pagesCountLabel.AutoSize = true;
            pagesCountLabel.Location = new Point(61, 160);
            pagesCountLabel.Margin = new Padding(3, 0, 15, 0);
            pagesCountLabel.Name = "pagesCountLabel";
            pagesCountLabel.Size = new Size(94, 40);
            pagesCountLabel.TabIndex = 6;
            pagesCountLabel.Text = "Количество страниц";
            // 
            // ganreLabel
            // 
            ganreLabel.Anchor = AnchorStyles.Right;
            ganreLabel.AutoSize = true;
            ganreLabel.Location = new Point(107, 130);
            ganreLabel.Margin = new Padding(3, 0, 15, 0);
            ganreLabel.Name = "ganreLabel";
            ganreLabel.Size = new Size(48, 20);
            ganreLabel.TabIndex = 4;
            ganreLabel.Text = "Жанр";
            // 
            // authorField
            // 
            authorField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            authorField.Location = new Point(185, 86);
            authorField.Margin = new Padding(15, 3, 3, 3);
            authorField.MaximumSize = new Size(1000, 0);
            authorField.Name = "authorField";
            authorField.Size = new Size(578, 27);
            authorField.TabIndex = 3;
            // 
            // bookNameField
            // 
            bookNameField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookNameField.Location = new Point(185, 46);
            bookNameField.Margin = new Padding(15, 3, 3, 3);
            bookNameField.MaximumSize = new Size(1000, 0);
            bookNameField.Name = "bookNameField";
            bookNameField.Size = new Size(578, 27);
            bookNameField.TabIndex = 0;
            // 
            // bookNameLabel
            // 
            bookNameLabel.Anchor = AnchorStyles.Right;
            bookNameLabel.AutoSize = true;
            bookNameLabel.Location = new Point(34, 50);
            bookNameLabel.Margin = new Padding(3, 0, 15, 0);
            bookNameLabel.Name = "bookNameLabel";
            bookNameLabel.Size = new Size(121, 20);
            bookNameLabel.TabIndex = 1;
            bookNameLabel.Text = "Название книги";
            // 
            // authorLabel
            // 
            authorLabel.Anchor = AnchorStyles.Right;
            authorLabel.AutoSize = true;
            authorLabel.Location = new Point(104, 90);
            authorLabel.Margin = new Padding(3, 0, 15, 0);
            authorLabel.Name = "authorLabel";
            authorLabel.Size = new Size(51, 20);
            authorLabel.TabIndex = 2;
            authorLabel.Text = "Автор";
            // 
            // pagesCountNumbericUpDown
            // 
            pagesCountNumbericUpDown.Anchor = AnchorStyles.Left;
            pagesCountNumbericUpDown.Location = new Point(185, 166);
            pagesCountNumbericUpDown.Margin = new Padding(15, 3, 3, 3);
            pagesCountNumbericUpDown.Name = "pagesCountNumbericUpDown";
            pagesCountNumbericUpDown.Size = new Size(134, 27);
            pagesCountNumbericUpDown.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(createBookBtn);
            flowLayoutPanel1.Controls.Add(generateBookBtn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(173, 283);
            flowLayoutPanel1.Margin = new Padding(3, 3, 15, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(578, 105);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // createBookBtn
            // 
            createBookBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            createBookBtn.Location = new Point(15, 3);
            createBookBtn.Margin = new Padding(15, 3, 3, 3);
            createBookBtn.Name = "createBookBtn";
            createBookBtn.Size = new Size(99, 60);
            createBookBtn.TabIndex = 0;
            createBookBtn.Text = "Создать";
            createBookBtn.UseVisualStyleBackColor = true;
            // 
            // generateBookBtn
            // 
            generateBookBtn.Anchor = AnchorStyles.None;
            generateBookBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            generateBookBtn.Location = new Point(132, 3);
            generateBookBtn.Margin = new Padding(15, 3, 3, 3);
            generateBookBtn.Name = "generateBookBtn";
            generateBookBtn.Size = new Size(157, 60);
            generateBookBtn.TabIndex = 1;
            generateBookBtn.Text = "Сгенерировать";
            generateBookBtn.UseVisualStyleBackColor = true;
            // 
            // ganreComboBox
            // 
            ganreComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ganreComboBox.FormattingEnabled = true;
            ganreComboBox.Items.AddRange(new object[] { "Роман", "Повесть", "Рассказ", "Проза", "Эпос", "Лирика", "Драма", "Фантастика", "Фэнтези", "Детектив", "Триллер", "Любовный роман", "Биография", "Психология", "Научно-популярная литература" });
            ganreComboBox.Location = new Point(185, 128);
            ganreComboBox.Margin = new Padding(15, 3, 3, 3);
            ganreComboBox.MaximumSize = new Size(1000, 0);
            ganreComboBox.Name = "ganreComboBox";
            ganreComboBox.Size = new Size(578, 28);
            ganreComboBox.TabIndex = 5;
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
            newBookPage.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)priceNumbericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)pagesCountNumbericUpDown).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl bookFormTabControl;
        private TabPage shopPage;
        private TabPage newBookPage;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox authorField;
        private TextBox bookNameField;
        private Label bookNameLabel;
        private Label authorLabel;
        private Label ganreLabel;
        private ComboBox ganreComboBox;
        private NumericUpDown priceNumbericUpDown;
        private Label priceLabel;
        private Label pagesCountLabel;
        private NumericUpDown pagesCountNumbericUpDown;
        private TextBox idField;
        private Label idLabel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button createBookBtn;
        private Button generateBookBtn;
    }
}
