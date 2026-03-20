using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BookShopLibrary;

namespace BookShop
{
    public partial class BookStoreForm : Form
    {
        private Shop shop;
        private List<BookShelf> bookShelves;
        private int nextId = 1;
        private readonly string genresFile = "genres.txt";
        private readonly string bookAuthorPairsFile = "book-author-pairs.txt";

        private List<(string Title, string Author)> _bookAuthorPairs = new List<(string, string)>();
        private List<string> _genres = new List<string>();

        private string gameDifficulty;

        // Таймер и очередь поставок
        private System.Windows.Forms.Timer deliveryTimer;
        private Queue<Delivery> deliveriesQueue = new Queue<Delivery>();
        private int deliveryInterval = 10000;

        public BookStoreForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            searchTypeCmb.SelectedIndex = 0;

            SubscribeToEvents();
            InitializeShop();
            InitializeFormControls();
            CreateDataFilesIfNotExist();
            LoadGenresFromFile();
            LoadBookAuthorPairs();

            deliveryTimer = new System.Windows.Forms.Timer();
            deliveryTimer.Tick += DeliveryTimer_Tick;

            gameDifficulty = "Нормальный";
            ConfigureGameDifficulty();
        }

        public BookStoreForm(string difficulty)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            searchTypeCmb.SelectedIndex = 0;

            SubscribeToEvents();
            InitializeShop();
            InitializeFormControls();
            CreateDataFilesIfNotExist();
            LoadGenresFromFile();
            LoadBookAuthorPairs();

            deliveryTimer = new System.Windows.Forms.Timer();
            deliveryTimer.Tick += DeliveryTimer_Tick;

            gameDifficulty = difficulty;
            ConfigureGameDifficulty();
        }

        private void ConfigureGameDifficulty()
        {
            switch (gameDifficulty)
            {
                case "Лёгкий":
                    deliveryInterval = 15000;
                    if (shop != null) shop.AddToBalance(2000);
                    MessageBox.Show("Запущен ЛЁГКИЙ режим!\n\n" +
                        "• Стартовый баланс: 2000 руб.\n" +
                        "• Книги приходят реже (15 сек)\n" +
                        "• Покупатели появляются реже (20 сек)\n" +
                        "• Очередь до 5 покупателей",
                        "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Нормальный":
                    deliveryInterval = 10000;
                    if (shop != null) shop.AddToBalance(1000);
                    MessageBox.Show("Запущен НОРМАЛЬНЫЙ режим!\n\n" +
                        "• Стартовый баланс: 1000 руб.\n" +
                        "• Книги приходят каждые 10 сек\n" +
                        "• Покупатели каждые 15 сек\n" +
                        "• Очередь до 4 покупателей",
                        "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Сложный":
                    deliveryInterval = 5000;
                    if (shop != null) shop.AddToBalance(500);
                    MessageBox.Show("Запущен СЛОЖНЫЙ режим!\n\n" +
                        "• Стартовый баланс: 500 руб.\n" +
                        "• Книги приходят каждые 5 сек\n" +
                        "• Покупатели каждые 8 сек\n" +
                        "• Очередь до 3 покупателей",
                        "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    gameDifficulty = "Нормальный";
                    deliveryInterval = 10000;
                    if (shop != null) shop.AddToBalance(1000);
                    break;
            }

            deliveryTimer.Interval = deliveryInterval;
            deliveryTimer.Start();
            UpdateBalanceDisplay();
        }

        // ========== МЕТОДЫ ДЛЯ ПОСТАВОК ==========

        private void DeliveryTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_bookAuthorPairs.Count == 0 || _genres.Count == 0)
                    return;

                Random rand = new Random();
                var pair = _bookAuthorPairs[rand.Next(_bookAuthorPairs.Count)];
                var genre = _genres[rand.Next(_genres.Count)];

                // Адекватное количество страниц: 50-1000
                int pages = rand.Next(50, 1001);

                // Адекватная цена: 50 руб + 0.5 руб за страницу, с вариацией ±15%
                // 100 стр = ~100 руб, 500 стр = ~300 руб, 1000 стр = ~550 руб
                decimal basePrice = 50 + (pages * 0.5m);
                decimal variation = (decimal)(rand.NextDouble() * 0.3 - 0.15);
                decimal price = Math.Round(basePrice * (1 + variation), 0);

                // Ограничиваем цену
                if (price < 30) price = 30;
                if (price > 800) price = 800;

                Book randomBook = new Book(nextId, pair.Title, pair.Author, genre, pages, price);
                Delivery delivery = new Delivery(randomBook, false);

                AddDelivery(delivery);

                nextId++;
                idField.Text = nextId.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации поставки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddDelivery(Delivery delivery)
        {
            deliveriesQueue.Enqueue(delivery);

            if (!bookFormTabControl.TabPages.Contains(deliveriesPage))
            {
                bookFormTabControl.TabPages.Add(deliveriesPage);
            }

            deliveriesPage.Visible = true;

            if (deliveriesQueue.Count == 1)
                ShowCurrentDelivery();
        }

        private void ShowCurrentDelivery()
        {
            if (deliveriesQueue.Count == 0)
            {
                deliveriesPage.Visible = false;
                if (bookFormTabControl.TabPages.Contains(deliveriesPage))
                {
                    bookFormTabControl.TabPages.Remove(deliveriesPage);
                }
                ClearDeliveryFields();
                return;
            }

            Delivery current = deliveriesQueue.Peek();

            deliveryTitleField.Text = current.Book.DisplayTitle;
            deliveryAuthorField.Text = current.Book.Author;
            deliveryGenreField.Text = current.Book.Genre;
            deliveryPriceField.Text = current.Book.Price.ToString("C");
            deliveryPagesField.Text = current.Book.Pages.ToString();
        }

        private void ClearDeliveryFields()
        {
            deliveryTitleField.Text = "";
            deliveryAuthorField.Text = "";
            deliveryGenreField.Text = "";
            deliveryPriceField.Text = "";
            deliveryPagesField.Text = "";
        }

        private void AcceptCurrentDelivery()
        {
            if (deliveriesQueue.Count == 0) return;

            Delivery delivery = deliveriesQueue.Peek();

            if (shop.Balance < delivery.Book.Price)
            {
                MessageBox.Show($"Недостаточно средств! Нужно: {delivery.Book.Price:C}, доступно: {shop.Balance:C}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            shop.AddToBalance(-delivery.Book.Price);

            if (shop.TryAddBook(delivery.Book))
            {
                MessageBox.Show($"Книга '{delivery.Book.DisplayTitle}' успешно принята!\nСписано: {delivery.Book.Price:C}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Обновляем список шкафов из магазина
                bookShelves = shop.GetShelves();

                UpdateBalanceDisplay();
                UpdateShelfComboBox();
                UpdateBooksInShelf();

                deliveriesQueue.Dequeue();
                ShowCurrentDelivery();
            }
            else
            {
                shop.AddToBalance(delivery.Book.Price);
                MessageBox.Show($"Нет места для книги '{delivery.Book.DisplayTitle}'!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RejectCurrentDelivery()
        {
            if (deliveriesQueue.Count == 0) return;

            Delivery delivery = deliveriesQueue.Dequeue();

            MessageBox.Show($"Поставка '{delivery.Book.DisplayTitle}' отклонена.",
                "Отказ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ShowCurrentDelivery();
        }

        private void BtnAcceptDelivery_Click(object sender, EventArgs e)
        {
            try { AcceptCurrentDelivery(); }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void BtnRejectDelivery_Click(object sender, EventArgs e)
        {
            try { RejectCurrentDelivery(); }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        // ========== ВАШИ СУЩЕСТВУЮЩИЕ МЕТОДЫ ==========

        private void SubscribeToEvents()
        {
            createBookBtn.Click += CreateBookBtn_Click;
            generateBookBtn.Click += GenerateBookBtn_Click;
            searchBtn.Click += SearchBtn_Click;
            bookSellBtn.Click += BookSellBtn_Click;

            shelfSelectCmb.SelectedIndexChanged += ShelfSelectCmb_SelectedIndexChanged;
            bookSelectCmb.SelectedIndexChanged += BookSelectCmb_SelectedIndexChanged;

            searchField.KeyPress += SearchField_KeyPress;
            bookNameField.KeyPress += TextBox_KeyPress;
            authorField.KeyPress += TextBox_KeyPress;

            btnAcceptDelivery.Click += BtnAcceptDelivery_Click;
            btnRejectDelivery.Click += BtnRejectDelivery_Click;
        }

        private void bookInfoLayoutPanel_Paint(object sender, PaintEventArgs e) { }

        private void InitializeShop()
        {
            try
            {
                shop = new Shop(0, 5);
                // Получаем шкафы из магазина
                bookShelves = shop.GetShelves();
                UpdateShelfComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации магазина: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeFormControls()
        {
            // Количество страниц
            pagesCountNumbericUpDown.Minimum = 50;
            pagesCountNumbericUpDown.Maximum = 1000;
            pagesCountNumbericUpDown.Value = 200;

            // Цена
            priceNumbericUpDown.Minimum = 100;
            priceNumbericUpDown.Maximum = 1500;
            priceNumbericUpDown.DecimalPlaces = 0;
            priceNumbericUpDown.Value = 100;

            idField.Text = nextId.ToString();
            UpdateBalanceDisplay();
        }

        private void CreateDataFilesIfNotExist()
        {
            try
            {
                if (!File.Exists(genresFile))
                {
                    File.WriteAllLines(genresFile, new string[] {
                        "Роман", "Повесть", "Рассказ", "Проза", "Эпос",
                        "Лирика", "Драма", "Фантастика", "Фэнтези", "Детектив",
                        "Триллер", "Любовный роман", "Биография", "Психология", "Научно-популярная литература"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании файлов данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGenresFromFile()
        {
            try
            {
                if (File.Exists(genresFile))
                {
                    string[] genres = File.ReadAllLines(genresFile);
                    _genres.Clear();
                    ganreComboBox.Items.Clear();
                    foreach (string genre in genres)
                    {
                        if (!string.IsNullOrWhiteSpace(genre))
                        {
                            string trimmed = genre.Trim();
                            _genres.Add(trimmed);
                            ganreComboBox.Items.Add(trimmed);
                        }
                    }
                }
                else
                {
                    _genres = new List<string> { "Фантастика", "Детектив", "Роман", "Поэзия", "Драма" };
                    ganreComboBox.Items.AddRange(_genres.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке жанров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBookAuthorPairs()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, bookAuthorPairsFile);
                if (File.Exists(path))
                {
                    var lines = File.ReadAllLines(path);
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var parts = line.Split(';');
                        if (parts.Length == 2)
                        {
                            _bookAuthorPairs.Add((parts[0].Trim(), parts[1].Trim()));
                        }
                    }
                }
                else
                {
                    CreateDefaultBookAuthorPairsFile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пар книга-автор: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDefaultBookAuthorPairsFile()
        {
            try
            {
                string[] defaultPairs = new string[]
                {
                    "Бесы;Достоевский",
                    "Преступление и наказание;Достоевский",
                    "Война и мир;Толстой",
                    "Анна Каренина;Толстой",
                    "Мастер и Маргарита;Булгаков",
                    "Собачье сердце;Булгаков",
                    "Евгений Онегин;Пушкин",
                    "Капитанская дочка;Пушкин",
                    "Герой нашего времени;Лермонтов",
                    "Мцыри;Лермонтов"
                };

                File.WriteAllLines(bookAuthorPairsFile, defaultPairs);

                foreach (var line in defaultPairs)
                {
                    var parts = line.Split(';');
                    _bookAuthorPairs.Add((parts[0], parts[1]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось создать файл: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateBookFields()
        {
            if (string.IsNullOrWhiteSpace(bookNameField.Text))
            {
                MessageBox.Show("Введите название книги", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(authorField.Text))
            {
                MessageBox.Show("Введите автора книги", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ganreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр книги", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (pagesCountNumbericUpDown.Value <= 0)
            {
                MessageBox.Show("Количество страниц должно быть больше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (priceNumbericUpDown.Value <= 0)
            {
                MessageBox.Show("Цена книги должна быть больше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Поле не должно содержать цифры", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SearchField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (searchTypeCmb.SelectedIndex == 1)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private string GetUniqueTitle(string baseTitle)
        {
            string currentAuthor = authorField.Text.Trim();

            bool isSequel = bookShelves.Any(shelf =>
                shelf.GetAllBooks().Any(b =>
                    b.Title.Equals(baseTitle, StringComparison.OrdinalIgnoreCase) &&
                    b.Author.Equals(currentAuthor, StringComparison.OrdinalIgnoreCase)));

            if (isSequel)
            {
                return baseTitle;
            }

            string uniqueTitle = baseTitle;
            int suffix = 2;

            while (bookShelves.Any(shelf =>
                shelf.GetAllBooks().Any(b =>
                    b.Title.Equals(uniqueTitle, StringComparison.OrdinalIgnoreCase))))
            {
                uniqueTitle = $"{baseTitle} {suffix}";
                suffix++;
            }

            return uniqueTitle;
        }

        private void CreateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBookFields()) return;

                string selectedGenre = ganreComboBox.SelectedItem.ToString();

                string baseTitle = bookNameField.Text.Trim();
                string uniqueTitle = GetUniqueTitle(baseTitle);
                if (uniqueTitle != baseTitle)
                {
                    bookNameField.Text = uniqueTitle;
                }

                BookShelf targetShelf = bookShelves.FirstOrDefault(s =>
                    s.Genre == selectedGenre && s.CurrentCount < s.Capacity);

                if (targetShelf == null)
                {
                    BookShelf emptyShelf = bookShelves.FirstOrDefault(s => s.CurrentCount == 0);
                    if (emptyShelf != null)
                    {
                        emptyShelf.ChangeGenre(selectedGenre);
                        targetShelf = emptyShelf;
                    }
                }

                if (targetShelf == null)
                {
                    if (bookShelves.Count < shop.MaxShelves)
                    {
                        int newShelfId = bookShelves.Count > 0 ? bookShelves.Max(s => s.Id) + 1 : 1;
                        targetShelf = new BookShelf(newShelfId, selectedGenre, 10);
                        bookShelves.Add(targetShelf);
                        shop.AddBookShelf(targetShelf);
                    }
                    else
                    {
                        MessageBox.Show("Все шкафы заняты. Освободите место для нового жанра.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (targetShelf.CurrentCount >= targetShelf.Capacity)
                {
                    MessageBox.Show($"Шкаф переполнен! Максимум: {targetShelf.Capacity}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Book newBook = new Book(
                    nextId,
                    uniqueTitle,
                    authorField.Text.Trim(),
                    selectedGenre,
                    (int)pagesCountNumbericUpDown.Value,
                    priceNumbericUpDown.Value
                );

                if (targetShelf.AddBook(newBook))
                {
                    nextId++;
                    idField.Text = nextId.ToString();

                    ClearNewBookFields();
                    UpdateShelfComboBox();
                    UpdateBooksInShelf();

                    MessageBox.Show($"Книга '{newBook.DisplayTitle}' успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании книги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (_bookAuthorPairs.Count == 0)
                {
                    MessageBox.Show("Не загружены данные о книгах! Проверьте файл book-author-pairs.txt",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_genres.Count == 0)
                {
                    MessageBox.Show("Не загружены данные о жанрах!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Book generatedBook = Book.GenerateRandom(nextId, _bookAuthorPairs, _genres);

                generatedBook.Title = GetUniqueTitle(generatedBook.Title);

                bookNameField.Text = generatedBook.Title;
                authorField.Text = generatedBook.Author;

                for (int i = 0; i < ganreComboBox.Items.Count; i++)
                {
                    if (ganreComboBox.Items[i].ToString() == generatedBook.Genre)
                    {
                        ganreComboBox.SelectedIndex = i;
                        break;
                    }
                }

                pagesCountNumbericUpDown.Value = generatedBook.Pages;
                priceNumbericUpDown.Value = generatedBook.Price;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации книги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = searchField.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    MessageBox.Show("Введите текст для поиска", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Book foundBook = null;

                if (searchTypeCmb.SelectedIndex == 0)
                {
                    foreach (var shelf in bookShelves)
                    {
                        foundBook = shelf.FindBookByTitle(searchText);
                        if (foundBook != null) break;
                    }
                }
                else
                {
                    if (int.TryParse(searchText, out int id))
                    {
                        foreach (var shelf in bookShelves)
                        {
                            foundBook = shelf.FindBookById(id);
                            if (foundBook != null) break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("ID должен быть числом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (foundBook != null)
                {
                    BookShelf foundShelf = bookShelves.FirstOrDefault(s => s.FindBookById(foundBook.Id) != null);
                    if (foundShelf != null)
                    {
                        shelfSelectCmb.SelectedIndex = bookShelves.IndexOf(foundShelf);
                        UpdateBooksInShelf();

                        int bookIndex = bookSelectCmb.Items.Cast<string>()
                                        .ToList()
                                        .FindIndex(item => ExtractBookId(item) == foundBook.Id);
                        if (bookIndex >= 0) bookSelectCmb.SelectedIndex = bookIndex;

                        DisplayBookInfo(foundBook);
                    }
                }
                else
                {
                    MessageBox.Show("Книга не найдена", "Результат поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BookSellBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (bookSelectCmb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите книгу для продажи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedBookInfo = bookSelectCmb.SelectedItem.ToString();
                int bookId = ExtractBookId(selectedBookInfo);

                BookShelf targetShelf = GetSelectedBookShelf();
                Book bookToSell = targetShelf?.FindBookById(bookId);

                if (bookToSell != null && targetShelf != null)
                {
                    decimal price = shop.SellBook(targetShelf.Id, bookId);

                    // Обновляем список шкафов после продажи
                    bookShelves = shop.GetShelves();

                    UpdateBalanceDisplay();
                    UpdateShelfComboBox();
                    UpdateBooksInShelf();
                    ClearBookInfo();

                    MessageBox.Show($"Книга '{bookToSell.DisplayTitle}' продана за {price:C}.",
                        "Продажа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Книга не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при продаже: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShelfSelectCmb_SelectedIndexChanged(object sender, EventArgs e) => UpdateBooksInShelf();

        private void BookSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bookSelectCmb.SelectedItem != null)
            {
                string selectedBookInfo = bookSelectCmb.SelectedItem.ToString();
                int bookId = ExtractBookId(selectedBookInfo);

                BookShelf selectedShelf = GetSelectedBookShelf();
                if (selectedShelf != null)
                {
                    Book selectedBook = selectedShelf.FindBookById(bookId);
                    if (selectedBook != null)
                        DisplayBookInfo(selectedBook);
                }
            }
        }

        private void UpdateShelfComboBox()
        {
            shelfSelectCmb.Items.Clear();
            foreach (var shelf in bookShelves)
            {
                shelfSelectCmb.Items.Add($"Шкаф {shelf.Id} ({shelf.Genre})");
            }

            if (shelfSelectCmb.Items.Count > 0)
                shelfSelectCmb.SelectedIndex = 0;
        }

        private BookShelf GetSelectedBookShelf()
        {
            if (shelfSelectCmb.SelectedItem == null) return null;

            string selectedShelf = shelfSelectCmb.SelectedItem.ToString();
            int shelfId = ExtractShelfId(selectedShelf);

            return bookShelves.FirstOrDefault(s => s.Id == shelfId);
        }

        private void ClearNewBookFields()
        {
            bookNameField.Clear();
            authorField.Clear();
            ganreComboBox.SelectedIndex = -1;
            pagesCountNumbericUpDown.Value = pagesCountNumbericUpDown.Minimum;
            priceNumbericUpDown.Value = priceNumbericUpDown.Minimum;
        }

        private void UpdateBooksInShelf()
        {
            bookSelectCmb.Items.Clear();
            BookShelf selectedShelf = GetSelectedBookShelf();
            if (selectedShelf != null)
            {
                var booksInShelf = selectedShelf.GetAllBooks();
                foreach (var book in booksInShelf)
                {
                    bookSelectCmb.Items.Add($"{book.Id}: {book.DisplayTitle} - {book.Author}");
                }

                shelfCapacity.Text = $"Загруженность {booksInShelf.Count}/{selectedShelf.Capacity}";
                shelfCapacity.ForeColor = booksInShelf.Count >= selectedShelf.Capacity ? Color.Red : Color.Black;
            }
        }

        private void DisplayBookInfo(Book book)
        {
            if (book != null)
            {
                bookTitleField.Text = book.DisplayTitle;
                bookAuthorField.Text = book.Author;
                bookIDField.Text = book.Id.ToString();
                bookPriceField.Text = book.Price.ToString("C");
                bookPagesCountField.Text = book.Pages.ToString();
            }
        }

        private void ClearBookInfo()
        {
            bookTitleField.Clear();
            bookAuthorField.Clear();
            bookIDField.Clear();
            bookPriceField.Clear();
            bookPagesCountField.Clear();
        }

        private void UpdateBalanceDisplay()
        {
            if (shop != null)
                balanceLb.Text = $"Баланс: {shop.Balance:C}";
        }

        private int ExtractBookId(string bookInfo)
        {
            if (string.IsNullOrEmpty(bookInfo)) return 0;
            string[] parts = bookInfo.Split(':');
            return int.TryParse(parts[0], out int id) ? id : 0;
        }

        private int ExtractShelfId(string shelfInfo)
        {
            if (string.IsNullOrEmpty(shelfInfo)) return 0;

            int startIndex = shelfInfo.IndexOf(' ') + 1;
            int endIndex = shelfInfo.IndexOf(' ', startIndex);

            if (startIndex > 0 && endIndex > startIndex)
            {
                string idStr = shelfInfo.Substring(startIndex, endIndex - startIndex);
                return int.TryParse(idStr, out int id) ? id : 0;
            }

            return 0;
        }
    }
}