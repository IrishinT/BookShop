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
            bookInfoLayoutPanel = new TableLayoutPanel();
            bookPagesCountField = new TextBox();
            bookPagesCountLb = new Label();
            bookPriceField = new TextBox();
            bookPriceLb = new Label();
            bookIDField = new TextBox();
            bookIDLb = new Label();
            bookAuthorField = new TextBox();
            bookAuthorLb = new Label();
            bookTitleLb = new Label();
            bookTitleField = new TextBox();
            bookSellBtn = new Button();
            shopTablePanel = new TableLayoutPanel();
            shelfSelectLb = new Label();
            shelfSelectCmb = new ComboBox();
            bookSelectLb = new Label();
            bookSelectCmb = new ComboBox();
            shelfCapacity = new Label();
            balanceLb = new Label();
            searchPanel = new FlowLayoutPanel();
            searchTypeCmb = new ComboBox();
            searchField = new TextBox();
            searchBtn = new Button();
            newBookPage = new TabPage();
            newBookTablePanel = new TableLayoutPanel();
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
            shopPage.SuspendLayout();
            bookInfoLayoutPanel.SuspendLayout();
            shopTablePanel.SuspendLayout();
            searchPanel.SuspendLayout();
            newBookPage.SuspendLayout();
            newBookTablePanel.SuspendLayout();
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
            bookFormTabControl.Margin = new Padding(3, 4, 3, 4);
            bookFormTabControl.Name = "bookFormTabControl";
            bookFormTabControl.SelectedIndex = 0;
            bookFormTabControl.Size = new Size(914, 600);
            bookFormTabControl.TabIndex = 0;
            // 
            // shopPage
            // 
            shopPage.BackgroundImage = (Image)resources.GetObject("shopPage.BackgroundImage");
            shopPage.BackgroundImageLayout = ImageLayout.Stretch;
            shopPage.Controls.Add(bookInfoLayoutPanel);
            shopPage.Controls.Add(shopTablePanel);
            shopPage.Controls.Add(balanceLb);
            shopPage.Controls.Add(searchPanel);
            shopPage.Location = new Point(4, 34);
            shopPage.Margin = new Padding(3, 4, 3, 4);
            shopPage.Name = "shopPage";
            shopPage.Padding = new Padding(3, 4, 3, 4);
            shopPage.Size = new Size(906, 562);
            shopPage.TabIndex = 0;
            shopPage.Text = "Магазин";
            shopPage.UseVisualStyleBackColor = true;
            // 
            // bookInfoLayoutPanel
            // 
            bookInfoLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            bookInfoLayoutPanel.ColumnCount = 2;
            bookInfoLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
            bookInfoLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 361F));
            bookInfoLayoutPanel.Controls.Add(bookPagesCountField, 1, 4);
            bookInfoLayoutPanel.Controls.Add(bookPagesCountLb, 0, 4);
            bookInfoLayoutPanel.Controls.Add(bookPriceField, 1, 3);
            bookInfoLayoutPanel.Controls.Add(bookPriceLb, 0, 3);
            bookInfoLayoutPanel.Controls.Add(bookIDField, 1, 2);
            bookInfoLayoutPanel.Controls.Add(bookIDLb, 0, 2);
            bookInfoLayoutPanel.Controls.Add(bookAuthorField, 1, 1);
            bookInfoLayoutPanel.Controls.Add(bookAuthorLb, 0, 1);
            bookInfoLayoutPanel.Controls.Add(bookTitleLb, 0, 0);
            bookInfoLayoutPanel.Controls.Add(bookTitleField, 1, 0);
            bookInfoLayoutPanel.Controls.Add(bookSellBtn, 1, 5);
            bookInfoLayoutPanel.Location = new Point(386, 89);
            bookInfoLayoutPanel.Margin = new Padding(3, 4, 3, 4);
            bookInfoLayoutPanel.Name = "bookInfoLayoutPanel";
            bookInfoLayoutPanel.RowCount = 6;
            bookInfoLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            bookInfoLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            bookInfoLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            bookInfoLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            bookInfoLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            bookInfoLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            bookInfoLayoutPanel.Size = new Size(510, 437);
            bookInfoLayoutPanel.TabIndex = 3;
            bookInfoLayoutPanel.Paint += bookInfoLayoutPanel_Paint;
            // 
            // bookPagesCountField
            // 
            bookPagesCountField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookPagesCountField.Location = new Point(166, 308);
            bookPagesCountField.Margin = new Padding(17, 4, 17, 4);
            bookPagesCountField.MaximumSize = new Size(228, 4);
            bookPagesCountField.Name = "bookPagesCountField";
            bookPagesCountField.ReadOnly = true;
            bookPagesCountField.Size = new Size(228, 4);
            bookPagesCountField.TabIndex = 10;
            // 
            // bookPagesCountLb
            // 
            bookPagesCountLb.Anchor = AnchorStyles.Right;
            bookPagesCountLb.AutoSize = true;
            bookPagesCountLb.Location = new Point(18, 299);
            bookPagesCountLb.Margin = new Padding(17, 4, 17, 4);
            bookPagesCountLb.Name = "bookPagesCountLb";
            bookPagesCountLb.Size = new Size(114, 50);
            bookPagesCountLb.TabIndex = 9;
            bookPagesCountLb.Text = "Количество страниц";
            // 
            // bookPriceField
            // 
            bookPriceField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookPriceField.Location = new Point(166, 236);
            bookPriceField.Margin = new Padding(17, 4, 17, 4);
            bookPriceField.MaximumSize = new Size(228, 4);
            bookPriceField.Name = "bookPriceField";
            bookPriceField.ReadOnly = true;
            bookPriceField.Size = new Size(228, 4);
            bookPriceField.TabIndex = 8;
            // 
            // bookPriceLb
            // 
            bookPriceLb.Anchor = AnchorStyles.Right;
            bookPriceLb.AutoSize = true;
            bookPriceLb.Location = new Point(75, 239);
            bookPriceLb.Margin = new Padding(17, 4, 17, 4);
            bookPriceLb.Name = "bookPriceLb";
            bookPriceLb.Size = new Size(57, 25);
            bookPriceLb.TabIndex = 7;
            bookPriceLb.Text = "Цена";
            // 
            // bookIDField
            // 
            bookIDField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookIDField.Location = new Point(166, 164);
            bookIDField.Margin = new Padding(17, 4, 17, 4);
            bookIDField.MaximumSize = new Size(228, 4);
            bookIDField.Name = "bookIDField";
            bookIDField.ReadOnly = true;
            bookIDField.Size = new Size(228, 4);
            bookIDField.TabIndex = 6;
            // 
            // bookIDLb
            // 
            bookIDLb.Anchor = AnchorStyles.Right;
            bookIDLb.AutoSize = true;
            bookIDLb.Location = new Point(102, 167);
            bookIDLb.Margin = new Padding(17, 4, 17, 4);
            bookIDLb.Name = "bookIDLb";
            bookIDLb.Size = new Size(30, 25);
            bookIDLb.TabIndex = 5;
            bookIDLb.Text = "ID";
            // 
            // bookAuthorField
            // 
            bookAuthorField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookAuthorField.Location = new Point(166, 106);
            bookAuthorField.Margin = new Padding(17, 4, 17, 4);
            bookAuthorField.MaximumSize = new Size(799, 4);
            bookAuthorField.Name = "bookAuthorField";
            bookAuthorField.ReadOnly = true;
            bookAuthorField.Size = new Size(327, 4);
            bookAuthorField.TabIndex = 4;
            // 
            // bookAuthorLb
            // 
            bookAuthorLb.Anchor = AnchorStyles.Right;
            bookAuthorLb.AutoSize = true;
            bookAuthorLb.Location = new Point(68, 95);
            bookAuthorLb.Margin = new Padding(17, 4, 17, 4);
            bookAuthorLb.Name = "bookAuthorLb";
            bookAuthorLb.Size = new Size(64, 25);
            bookAuthorLb.TabIndex = 3;
            bookAuthorLb.Text = "Автор";
            // 
            // bookTitleLb
            // 
            bookTitleLb.Anchor = AnchorStyles.Right;
            bookTitleLb.AutoSize = true;
            bookTitleLb.Location = new Point(37, 23);
            bookTitleLb.Margin = new Padding(17, 4, 17, 4);
            bookTitleLb.Name = "bookTitleLb";
            bookTitleLb.Size = new Size(95, 25);
            bookTitleLb.TabIndex = 1;
            bookTitleLb.Text = "Название";
            // 
            // bookTitleField
            // 
            bookTitleField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookTitleField.Location = new Point(166, 34);
            bookTitleField.Margin = new Padding(17, 4, 17, 4);
            bookTitleField.MaximumSize = new Size(799, 4);
            bookTitleField.Name = "bookTitleField";
            bookTitleField.ReadOnly = true;
            bookTitleField.Size = new Size(327, 4);
            bookTitleField.TabIndex = 2;
            // 
            // bookSellBtn
            // 
            bookSellBtn.Anchor = AnchorStyles.Left;
            bookSellBtn.Location = new Point(166, 376);
            bookSellBtn.Margin = new Padding(17, 4, 17, 4);
            bookSellBtn.Name = "bookSellBtn";
            bookSellBtn.Size = new Size(135, 45);
            bookSellBtn.TabIndex = 0;
            bookSellBtn.Text = "Продать";
            bookSellBtn.UseVisualStyleBackColor = true;
            // 
            // shopTablePanel
            // 
            shopTablePanel.ColumnCount = 1;
            shopTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            shopTablePanel.Controls.Add(shelfSelectLb, 0, 0);
            shopTablePanel.Controls.Add(shelfSelectCmb, 0, 1);
            shopTablePanel.Controls.Add(bookSelectLb, 0, 2);
            shopTablePanel.Controls.Add(bookSelectCmb, 0, 3);
            shopTablePanel.Controls.Add(shelfCapacity, 0, 4);
            shopTablePanel.Location = new Point(24, 89);
            shopTablePanel.Margin = new Padding(3, 4, 3, 4);
            shopTablePanel.Name = "shopTablePanel";
            shopTablePanel.RowCount = 5;
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 56.8627434F));
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 43.1372566F));
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 95F));
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            shopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            shopTablePanel.Size = new Size(355, 437);
            shopTablePanel.TabIndex = 2;
            // 
            // shelfSelectLb
            // 
            shelfSelectLb.Anchor = AnchorStyles.Left;
            shelfSelectLb.AutoSize = true;
            shelfSelectLb.Location = new Point(17, 32);
            shelfSelectLb.Margin = new Padding(17, 4, 17, 4);
            shelfSelectLb.Name = "shelfSelectLb";
            shelfSelectLb.Size = new Size(149, 25);
            shelfSelectLb.TabIndex = 0;
            shelfSelectLb.Text = "Выберите шкаф";
            // 
            // shelfSelectCmb
            // 
            shelfSelectCmb.Dock = DockStyle.Fill;
            shelfSelectCmb.FormattingEnabled = true;
            shelfSelectCmb.Location = new Point(17, 93);
            shelfSelectCmb.Margin = new Padding(17, 4, 17, 4);
            shelfSelectCmb.Name = "shelfSelectCmb";
            shelfSelectCmb.Size = new Size(321, 33);
            shelfSelectCmb.TabIndex = 1;
            // 
            // bookSelectLb
            // 
            bookSelectLb.Anchor = AnchorStyles.Left;
            bookSelectLb.AutoSize = true;
            bookSelectLb.Location = new Point(17, 166);
            bookSelectLb.Margin = new Padding(17, 4, 17, 4);
            bookSelectLb.Name = "bookSelectLb";
            bookSelectLb.Size = new Size(149, 25);
            bookSelectLb.TabIndex = 2;
            bookSelectLb.Text = "Выберите книгу";
            // 
            // bookSelectCmb
            // 
            bookSelectCmb.Dock = DockStyle.Fill;
            bookSelectCmb.FormattingEnabled = true;
            bookSelectCmb.Location = new Point(17, 205);
            bookSelectCmb.Margin = new Padding(17, 4, 17, 4);
            bookSelectCmb.Name = "bookSelectCmb";
            bookSelectCmb.Size = new Size(321, 33);
            bookSelectCmb.TabIndex = 3;
            // 
            // shelfCapacity
            // 
            shelfCapacity.Anchor = AnchorStyles.Left;
            shelfCapacity.AutoSize = true;
            shelfCapacity.Location = new Point(17, 354);
            shelfCapacity.Margin = new Padding(17, 4, 17, 4);
            shelfCapacity.Name = "shelfCapacity";
            shelfCapacity.Size = new Size(185, 25);
            shelfCapacity.TabIndex = 4;
            shelfCapacity.Text = "Загруженность 0/10";
            // 
            // balanceLb
            // 
            balanceLb.AutoSize = true;
            balanceLb.Dock = DockStyle.Right;
            balanceLb.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            balanceLb.Location = new Point(771, 4);
            balanceLb.Margin = new Padding(3, 4, 3, 4);
            balanceLb.Name = "balanceLb";
            balanceLb.Size = new Size(132, 32);
            balanceLb.TabIndex = 1;
            balanceLb.Text = "Баланс: 0р";
            // 
            // searchPanel
            // 
            searchPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchPanel.Controls.Add(searchTypeCmb);
            searchPanel.Controls.Add(searchField);
            searchPanel.Controls.Add(searchBtn);
            searchPanel.Location = new Point(3, 4);
            searchPanel.Margin = new Padding(3, 4, 3, 4);
            searchPanel.Name = "searchPanel";
            searchPanel.Padding = new Padding(17, 20, 17, 20);
            searchPanel.Size = new Size(898, 77);
            searchPanel.TabIndex = 0;
            // 
            // searchTypeCmb
            // 
            searchTypeCmb.FormattingEnabled = true;
            searchTypeCmb.Items.AddRange(new object[] { "По названию", "По ID" });
            searchTypeCmb.Location = new Point(20, 24);
            searchTypeCmb.Margin = new Padding(3, 4, 3, 4);
            searchTypeCmb.Name = "searchTypeCmb";
            searchTypeCmb.Size = new Size(187, 33);
            searchTypeCmb.TabIndex = 0;
            // 
            // searchField
            // 
            searchField.Location = new Point(213, 24);
            searchField.Margin = new Padding(3, 4, 3, 4);
            searchField.Name = "searchField";
            searchField.Size = new Size(417, 32);
            searchField.TabIndex = 1;
            // 
            // searchBtn
            // 
            searchBtn.Anchor = AnchorStyles.None;
            searchBtn.Location = new Point(636, 24);
            searchBtn.Margin = new Padding(3, 4, 3, 4);
            searchBtn.Name = "searchBtn";
            searchBtn.Size = new Size(86, 37);
            searchBtn.TabIndex = 2;
            searchBtn.Text = "Поиск";
            searchBtn.UseVisualStyleBackColor = true;
            // 
            // newBookPage
            // 
            newBookPage.BackColor = Color.White;
            newBookPage.BackgroundImage = (Image)resources.GetObject("newBookPage.BackgroundImage");
            newBookPage.BackgroundImageLayout = ImageLayout.Stretch;
            newBookPage.Controls.Add(newBookTablePanel);
            newBookPage.Location = new Point(4, 34);
            newBookPage.Margin = new Padding(3, 4, 3, 4);
            newBookPage.Name = "newBookPage";
            newBookPage.Padding = new Padding(3, 4, 3, 4);
            newBookPage.Size = new Size(906, 562);
            newBookPage.TabIndex = 1;
            newBookPage.Text = "Новая книга";
            // 
            // newBookTablePanel
            // 
            newBookTablePanel.BackColor = Color.Transparent;
            newBookTablePanel.BackgroundImageLayout = ImageLayout.None;
            newBookTablePanel.ColumnCount = 2;
            newBookTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 171F));
            newBookTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            newBookTablePanel.Controls.Add(idField, 1, 5);
            newBookTablePanel.Controls.Add(idLabel, 0, 5);
            newBookTablePanel.Controls.Add(priceNumbericUpDown, 1, 4);
            newBookTablePanel.Controls.Add(priceLabel, 0, 4);
            newBookTablePanel.Controls.Add(pagesCountLabel, 0, 3);
            newBookTablePanel.Controls.Add(ganreLabel, 0, 2);
            newBookTablePanel.Controls.Add(authorField, 1, 1);
            newBookTablePanel.Controls.Add(bookNameField, 1, 0);
            newBookTablePanel.Controls.Add(bookNameLabel, 0, 0);
            newBookTablePanel.Controls.Add(authorLabel, 0, 1);
            newBookTablePanel.Controls.Add(pagesCountNumbericUpDown, 1, 3);
            newBookTablePanel.Controls.Add(flowLayoutPanel1, 1, 6);
            newBookTablePanel.Controls.Add(ganreComboBox, 1, 2);
            newBookTablePanel.Dock = DockStyle.Fill;
            newBookTablePanel.Location = new Point(3, 4);
            newBookTablePanel.Margin = new Padding(3, 4, 3, 4);
            newBookTablePanel.Name = "newBookTablePanel";
            newBookTablePanel.Padding = new Padding(23, 53, 23, 27);
            newBookTablePanel.RowCount = 7;
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            newBookTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            newBookTablePanel.Size = new Size(900, 554);
            newBookTablePanel.TabIndex = 0;
            // 
            // idField
            // 
            idField.Anchor = AnchorStyles.Left;
            idField.Location = new Point(211, 328);
            idField.Margin = new Padding(17, 4, 3, 4);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(153, 32);
            idField.TabIndex = 11;
            // 
            // idLabel
            // 
            idLabel.Anchor = AnchorStyles.Right;
            idLabel.AutoSize = true;
            idLabel.Location = new Point(147, 332);
            idLabel.Margin = new Padding(3, 0, 17, 0);
            idLabel.Name = "idLabel";
            idLabel.Size = new Size(30, 25);
            idLabel.TabIndex = 10;
            idLabel.Text = "ID";
            // 
            // priceNumbericUpDown
            // 
            priceNumbericUpDown.Anchor = AnchorStyles.Left;
            priceNumbericUpDown.Location = new Point(211, 275);
            priceNumbericUpDown.Margin = new Padding(17, 4, 3, 4);
            priceNumbericUpDown.Name = "priceNumbericUpDown";
            priceNumbericUpDown.Size = new Size(153, 32);
            priceNumbericUpDown.TabIndex = 9;
            // 
            // priceLabel
            // 
            priceLabel.Anchor = AnchorStyles.Right;
            priceLabel.AutoSize = true;
            priceLabel.Location = new Point(120, 279);
            priceLabel.Margin = new Padding(3, 0, 17, 0);
            priceLabel.Name = "priceLabel";
            priceLabel.Size = new Size(57, 25);
            priceLabel.TabIndex = 8;
            priceLabel.Text = "Цена";
            // 
            // pagesCountLabel
            // 
            pagesCountLabel.Anchor = AnchorStyles.Right;
            pagesCountLabel.AutoSize = true;
            pagesCountLabel.Location = new Point(58, 213);
            pagesCountLabel.Margin = new Padding(3, 0, 17, 0);
            pagesCountLabel.Name = "pagesCountLabel";
            pagesCountLabel.Size = new Size(119, 50);
            pagesCountLabel.TabIndex = 6;
            pagesCountLabel.Text = "Количество страниц";
            // 
            // ganreLabel
            // 
            ganreLabel.Anchor = AnchorStyles.Right;
            ganreLabel.AutoSize = true;
            ganreLabel.Location = new Point(117, 173);
            ganreLabel.Margin = new Padding(3, 0, 17, 0);
            ganreLabel.Name = "ganreLabel";
            ganreLabel.Size = new Size(60, 25);
            ganreLabel.TabIndex = 4;
            ganreLabel.Text = "Жанр";
            // 
            // authorField
            // 
            authorField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            authorField.Location = new Point(211, 116);
            authorField.Margin = new Padding(17, 4, 3, 4);
            authorField.MaximumSize = new Size(1142, 4);
            authorField.Name = "authorField";
            authorField.Size = new Size(663, 4);
            authorField.TabIndex = 3;
            // 
            // bookNameField
            // 
            bookNameField.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bookNameField.Location = new Point(211, 63);
            bookNameField.Margin = new Padding(17, 4, 3, 4);
            bookNameField.MaximumSize = new Size(1142, 4);
            bookNameField.Name = "bookNameField";
            bookNameField.Size = new Size(663, 4);
            bookNameField.TabIndex = 0;
            // 
            // bookNameLabel
            // 
            bookNameLabel.Anchor = AnchorStyles.Right;
            bookNameLabel.AutoSize = true;
            bookNameLabel.Location = new Point(28, 67);
            bookNameLabel.Margin = new Padding(3, 0, 17, 0);
            bookNameLabel.Name = "bookNameLabel";
            bookNameLabel.Size = new Size(149, 25);
            bookNameLabel.TabIndex = 1;
            bookNameLabel.Text = "Название книги";
            // 
            // authorLabel
            // 
            authorLabel.Anchor = AnchorStyles.Right;
            authorLabel.AutoSize = true;
            authorLabel.Location = new Point(113, 120);
            authorLabel.Margin = new Padding(3, 0, 17, 0);
            authorLabel.Name = "authorLabel";
            authorLabel.Size = new Size(64, 25);
            authorLabel.TabIndex = 2;
            authorLabel.Text = "Автор";
            // 
            // pagesCountNumbericUpDown
            // 
            pagesCountNumbericUpDown.Anchor = AnchorStyles.Left;
            pagesCountNumbericUpDown.Location = new Point(211, 222);
            pagesCountNumbericUpDown.Margin = new Padding(17, 4, 3, 4);
            pagesCountNumbericUpDown.Name = "pagesCountNumbericUpDown";
            pagesCountNumbericUpDown.Size = new Size(153, 32);
            pagesCountNumbericUpDown.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(createBookBtn);
            flowLayoutPanel1.Controls.Add(generateBookBtn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(197, 375);
            flowLayoutPanel1.Margin = new Padding(3, 4, 17, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(663, 148);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // createBookBtn
            // 
            createBookBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            createBookBtn.Location = new Point(17, 4);
            createBookBtn.Margin = new Padding(17, 4, 3, 4);
            createBookBtn.Name = "createBookBtn";
            createBookBtn.Size = new Size(113, 80);
            createBookBtn.TabIndex = 0;
            createBookBtn.Text = "Создать";
            createBookBtn.UseVisualStyleBackColor = true;
            // 
            // generateBookBtn
            // 
            generateBookBtn.Anchor = AnchorStyles.None;
            generateBookBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            generateBookBtn.Location = new Point(150, 4);
            generateBookBtn.Margin = new Padding(17, 4, 3, 4);
            generateBookBtn.Name = "generateBookBtn";
            generateBookBtn.Size = new Size(179, 80);
            generateBookBtn.TabIndex = 1;
            generateBookBtn.Text = "Сгенерировать";
            generateBookBtn.UseVisualStyleBackColor = true;
            // 
            // ganreComboBox
            // 
            ganreComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ganreComboBox.FormattingEnabled = true;
            ganreComboBox.Items.AddRange(new object[] { "Роман", "Повесть", "Рассказ", "Проза", "Эпос", "Лирика", "Драма", "Фантастика", "Фэнтези", "Детектив", "Триллер", "Любовный роман", "Биография", "Психология", "Научно-популярная литература" });
            ganreComboBox.Location = new Point(211, 171);
            ganreComboBox.Margin = new Padding(17, 4, 3, 4);
            ganreComboBox.MaximumSize = new Size(1142, 0);
            ganreComboBox.Name = "ganreComboBox";
            ganreComboBox.Size = new Size(663, 33);
            ganreComboBox.TabIndex = 5;
            // 
            // BookStoreForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(bookFormTabControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "BookStoreForm";
            Text = "Книжный магазин";
            bookFormTabControl.ResumeLayout(false);
            shopPage.ResumeLayout(false);
            shopPage.PerformLayout();
            bookInfoLayoutPanel.ResumeLayout(false);
            bookInfoLayoutPanel.PerformLayout();
            shopTablePanel.ResumeLayout(false);
            shopTablePanel.PerformLayout();
            searchPanel.ResumeLayout(false);
            searchPanel.PerformLayout();
            newBookPage.ResumeLayout(false);
            newBookTablePanel.ResumeLayout(false);
            newBookTablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)priceNumbericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)pagesCountNumbericUpDown).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl bookFormTabControl;
        private TabPage shopPage;
        private TabPage newBookPage;
        private TableLayoutPanel newBookTablePanel;
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
        private FlowLayoutPanel searchPanel;
        private ComboBox searchTypeCmb;
        private TextBox searchField;
        private Button searchBtn;
        private Label balanceLb;
        private TableLayoutPanel shopTablePanel;
        private Label shelfSelectLb;
        private ComboBox shelfSelectCmb;
        private Label bookSelectLb;
        private ComboBox bookSelectCmb;
        private Label shelfCapacity;
        private TableLayoutPanel bookInfoLayoutPanel;
        private Label bookTitleLb;
        private TextBox bookPriceField;
        private Label bookPriceLb;
        private TextBox bookIDField;
        private Label bookIDLb;
        private TextBox bookAuthorField;
        private Label bookAuthorLb;
        private TextBox bookTitleField;
        private TextBox bookPagesCountField;
        private Label bookPagesCountLb;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button bookSellBtn;
    }
}
