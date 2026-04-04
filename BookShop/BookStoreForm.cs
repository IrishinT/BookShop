#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BookShopLibrary;

namespace BookShop
{
    /// <summary>
    /// Главная форма приложения "Книжный Магнат". 
    /// Отвечает за отрисовку интерфейса и передачу действий пользователя в ядро игры (GameManager).
    /// </summary>
    public partial class BookStoreForm : Form
    {
        /// <summary> Главный менеджер игровой логики </summary>
        private GameManager _gameManager;

        /// <summary> Основной игровой таймер (тик = 1 секунда) </summary>
        private System.Windows.Forms.Timer _mainTimer;

        /// <summary> Счётчик прошедших секунд для генерации событий </summary>
        private int _ticksElapsed = 0;

        /// <summary> Интервал появления новых поставок (в секундах) </summary>
        private int _deliveryIntervalSec;

        /// <summary> Интервал появления новых покупателей (в секундах) </summary>
        private int _customerIntervalSec;

        private readonly string genresFile = "genres.txt";
        private readonly string bookAuthorPairsFile = "book-author-pairs.txt";

        /// <summary>
        /// Конструктор формы магазина. Инициализирует игру, настраивает таймеры и интерфейс.
        /// </summary>
        /// <param name="difficulty">Выбранный уровень сложности (по умолчанию "Нормальный")</param>
        public BookStoreForm(string difficulty = "Нормальный")
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            EnableDoubleBuffering(bookFormTabControl);

            // 1. Создаем файлы, чтобы DatabaseManager мог успешно запуститься
            CreateDataFilesIfNotExist();

            // 2. Инициализируем ядро игры
            try
            {
                _gameManager = new GameManager(difficulty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации игры: {ex.Message}");
                return;
            }

            // 3. Настройка UI и интервалов
            ConfigureGameDifficulty(difficulty);
            InitializeFormControls();
            SubscribeToEvents();

            // 4. Запуск игрового времени
            _mainTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _mainTimer.Tick += MainTimer_Tick;
            _mainTimer.Start();

            UpdateUI();
        }

        /// <summary>
        /// Настраивает интервалы появления событий в зависимости от сложности игры.
        /// </summary>
        private void ConfigureGameDifficulty(string difficulty)
        {
            switch (difficulty)
            {
                case "Лёгкий":
                    _deliveryIntervalSec = 15;
                    _customerIntervalSec = 20;
                    MessageBox.Show("Запущен ЛЁГКИЙ режим!\n\n• Стартовый баланс: 2000 руб.\n• Книги приходят реже (15 сек)\n• Покупатели реже (20 сек)\n• Очередь до 5 чел.", "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "Сложный":
                    _deliveryIntervalSec = 5;
                    _customerIntervalSec = 8;
                    MessageBox.Show("Запущен СЛОЖНЫЙ режим!\n\n• Стартовый баланс: 500 руб.\n• Книги приходят часто (5 сек)\n• Покупатели часто (8 сек)\n• Очередь до 3 чел.", "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default: // Нормальный
                    _deliveryIntervalSec = 10;
                    _customerIntervalSec = 15;
                    MessageBox.Show("Запущен НОРМАЛЬНЫЙ режим!\n\n• Стартовый баланс: 1000 руб.\n• Книги приходят каждые 10 сек\n• Покупатели каждые 15 сек\n• Очередь до 4 чел.", "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        /// <summary>
        /// Заставляет всю форму (и все дочерние элементы) рисоваться в оперативной памяти ДО того, как они будут выведены на экран.
        /// Полностью убивает лаги, черные квадраты и мерцания при разворачивании на весь экран
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Включаем флаг WS_EX_COMPOSITED (0x02000000)
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        // Игровой цикл

        /// <summary>
        /// Обработчик тика игрового таймера. Вызывается каждую секунду.
        /// Обновляет время, генерирует покупателей и поставки, проверяет условия поражения.
        /// </summary>
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (_gameManager.State != GameState.Playing)
            {
                _mainTimer.Stop();
                MessageBox.Show(_gameManager.EndGameReason, _gameManager.State == GameState.Won ? "Победа!" : "Игра окончена", MessageBoxButtons.OK, _gameManager.State == GameState.Won ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            _gameManager.TickSecond();
            _ticksElapsed++;

            // Генерация поставок и покупателей по таймеру
            if (_ticksElapsed % _deliveryIntervalSec == 0)
                _gameManager.GenerateRandomDelivery();

            if (_ticksElapsed % _customerIntervalSec == 0)
                _gameManager.GenerateCustomer();

            UpdateUI();
        }

        // Обновления интерфейса

        /// <summary>
        /// Полностью обновляет визуальную часть окна: статус-бар в заголовке окна, баланс, списки шкафов.
        /// </summary>
        private void UpdateUI()
        {
            if (_gameManager == null) return;

            if (balanceLb != null) balanceLb.Text = $"Баланс: {_gameManager.Shop.Balance:C}";

            // Вывод статуса игры в заголовок окна
            this.Text = $"Магнат | Время: {_gameManager.TimeRemainingSeconds}с | " +
                        $"Очередь клиентов: {_gameManager.CustomerQueue.Count}/{_gameManager.MaxQueueSize} | " +
                        $"Недовольные: {_gameManager.DissatisfiedCustomers}/{_gameManager.MaxDissatisfied}";

            UpdateShelvesUI();
            ShowCurrentDelivery();
            UpdateCustomersTab();
        }

        /// <summary>
        /// Обновляет выпадающий список со шкафами магазина.
        /// </summary>
        private void UpdateShelvesUI()
        {
            // Запоминаем текущий выбранный шкаф (текст)
            string selectedShelfText = shelfSelectCmb.SelectedItem?.ToString();

            shelfSelectCmb.Items.Clear();
            foreach (var shelf in _gameManager.Shop.Shelves)
            {
                shelfSelectCmb.Items.Add($"Шкаф {shelf.Id} ({shelf.Genre})");
            }

            if (shelfSelectCmb.Items.Count > 0)
            {
                // Восстанавливаем выбор, если этот шкаф всё ещё существует
                if (!string.IsNullOrEmpty(selectedShelfText) && shelfSelectCmb.Items.Contains(selectedShelfText))
                    shelfSelectCmb.SelectedItem = selectedShelfText;
                else
                    shelfSelectCmb.SelectedIndex = 0;
            }

            UpdateBooksInShelf();
        }

        /// <summary>
        /// Обновляет выпадающий список книг для выбранного шкафа.
        /// </summary>
        private void UpdateBooksInShelf()
        {
            // Запоминаем текущую выбранную книгу (текст)
            string selectedBookText = bookSelectCmb.SelectedItem?.ToString();

            bookSelectCmb.Items.Clear();
            BookShelf selectedShelf = GetSelectedBookShelf();

            if (selectedShelf != null)
            {
                var booksInShelf = selectedShelf.GetAllBooks();
                foreach (var book in booksInShelf)
                {
                    bookSelectCmb.Items.Add($"{book.Id}: {book.DisplayTitle} - {book.Author}");
                }

                if (shelfCapacity != null)
                {
                    shelfCapacity.Text = $"Загруженность {booksInShelf.Count}/{selectedShelf.Capacity}";
                    shelfCapacity.ForeColor = booksInShelf.Count >= selectedShelf.Capacity ? Color.Red : Color.Black;
                }

                // Восстанавливаем выбор книги
                if (bookSelectCmb.Items.Count > 0)
                {
                    if (!string.IsNullOrEmpty(selectedBookText) && bookSelectCmb.Items.Contains(selectedBookText))
                        bookSelectCmb.SelectedItem = selectedBookText;
                    else
                        bookSelectCmb.SelectedIndex = 0; // Автоматически выбираем первую книгу
                }
            }
            else
            {
                if (shelfCapacity != null) shelfCapacity.Text = "Шкаф не выбран";
            }
        }

        // Работа с поставками

        /// <summary>
        /// Отображает информацию о текущей поставке из очереди на вкладке доставок.
        /// </summary>
        private void ShowCurrentDelivery()
        {
            if (_gameManager.DeliveryQueue.Count == 0)
            {
                deliveriesPage.Visible = false;
                if (bookFormTabControl.TabPages.Contains(deliveriesPage))
                    bookFormTabControl.TabPages.Remove(deliveriesPage);

                ClearDeliveryFields();
                return;
            }

            if (!bookFormTabControl.TabPages.Contains(deliveriesPage))
                bookFormTabControl.TabPages.Add(deliveriesPage);

            deliveriesPage.Visible = true;

            Delivery current = _gameManager.DeliveryQueue.Peek();
            deliveryTitleField.Text = current.Book.DisplayTitle;
            deliveryAuthorField.Text = current.Book.Author;
            deliveryGenreField.Text = current.Book.Genre;
            deliveryPriceField.Text = current.Book.BaseCost.ToString("C");
            deliveryPagesField.Text = current.Book.Pages.ToString();
        }

        /// <summary>
        /// Очищает текстовые поля на вкладке доставок.
        /// </summary>
        private void ClearDeliveryFields()
        {
            deliveryTitleField.Text = ""; deliveryAuthorField.Text = "";
            deliveryGenreField.Text = ""; deliveryPriceField.Text = ""; deliveryPagesField.Text = "";
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Принять поставку".
        /// </summary>
        private void BtnAcceptDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                string result = _gameManager.ProcessDelivery(accept: true, markAsError: false);
                MessageBox.Show(result, "Поставка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateUI();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Отклонить поставку" (отбраковка).
        /// </summary>
        private void BtnRejectDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                string result = _gameManager.ProcessDelivery(accept: false, markAsError: true);
                MessageBox.Show(result, "Отказ от поставки", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateUI();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Взаимодействие с книгами и покупателями

        /// <summary>
        /// Обрабатывает нажатие кнопки "Создать/Заказать книгу".
        /// </summary>
        private void CreateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBookFields()) return;

                string title = bookNameField.Text.Trim();
                string author = authorField.Text.Trim();
                string genre = ganreComboBox.SelectedItem.ToString();
                int pages = (int)pagesCountNumbericUpDown.Value;
                decimal cost = priceNumbericUpDown.Value;

                _gameManager.OrderBook(title, author, genre, pages, cost);

                MessageBox.Show("Книга заказана! Ожидайте доставку в очереди.", "Заказ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearNewBookFields();
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Автоматическая генерация случайных полей для книги (вспомогательная кнопка).
        /// </summary>
        private void GenerateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (DatabaseManager.BookAuthorPairs.Count == 0 || DatabaseManager.Genres.Count == 0)
                {
                    MessageBox.Show("База данных пуста!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Random rand = new Random();
                var pair = DatabaseManager.BookAuthorPairs[rand.Next(DatabaseManager.BookAuthorPairs.Count)];
                string genre = DatabaseManager.Genres[rand.Next(DatabaseManager.Genres.Count)];

                bookNameField.Text = pair.Title;
                authorField.Text = pair.Author;
                ganreComboBox.SelectedItem = genre;

                pagesCountNumbericUpDown.Value = rand.Next(50, 800);
                priceNumbericUpDown.Value = rand.Next(100, 1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Продать книгу".
        /// </summary>
        private void BookSellBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (bookSelectCmb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите книгу для продажи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int bookId = ExtractId(bookSelectCmb.SelectedItem.ToString());
                BookShelf shelf = GetSelectedBookShelf();
                Book bookToSell = shelf?.FindBookById(bookId);

                if (bookToSell == null) return;

                if (_gameManager.CustomerQueue.Count > 0)
                {
                    Customer customer = _gameManager.CustomerQueue.Peek();

                    // Устанавливаем наценку 10% от базовой цены
                    decimal sellPrice = bookToSell.BaseCost * 1.10m;

                    var result = _gameManager.ServeCustomer(bookToSell, sellPrice);

                    switch (result)
                    {
                        case CustomerServiceResult.Success:
                            // Сделали сообщение более понятным для игрока!
                            MessageBox.Show($"Себестоимость: {bookToSell.BaseCost:C}\nНаценка магазина: 10%\n\nКнига успешно продана клиенту за {sellPrice:C}!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case CustomerServiceResult.PriceTooHigh:
                            MessageBox.Show("Клиент ушел: слишком высокая цена (наценка > 15%)!", "Провал", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case CustomerServiceResult.WrongBook:
                            MessageBox.Show($"Клиент хотел другое: {customer}\nОн ушел недовольным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    // Если клиентов нет - списываем по себестоимости
                    decimal price = _gameManager.Shop.SellBookDirectly(shelf.Id, bookId);
                    MessageBox.Show($"Очередь пуста.\nКнига продана напрямую по себестоимости: {price:C}.", "Прямая продажа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ClearBookInfo();
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при продаже: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ищет книгу на полках по названию или по ID.
        /// </summary>
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

                if (searchTypeCmb.SelectedIndex == 0) // Поиск по названию
                {
                    foreach (var shelf in _gameManager.Shop.Shelves)
                    {
                        foundBook = shelf.FindBookByTitle(searchText);
                        if (foundBook != null) break;
                    }
                }
                else // Поиск по ID
                {
                    if (int.TryParse(searchText, out int id))
                    {
                        foreach (var shelf in _gameManager.Shop.Shelves)
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
                    BookShelf foundShelf = _gameManager.Shop.Shelves.FirstOrDefault(s => s.FindBookById(foundBook.Id) != null);
                    if (foundShelf != null)
                    {
                        shelfSelectCmb.SelectedIndex = _gameManager.Shop.Shelves.ToList().IndexOf(foundShelf);
                        UpdateBooksInShelf();

                        int bookIndex = bookSelectCmb.Items.Cast<string>()
                                        .ToList()
                                        .FindIndex(item => ExtractId(item) == foundBook.Id);
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

        // Вспомогательные методы и события

        /// <summary>
        /// Пустой обработчик отрисовки панели. Нужен для дизайнера формы (иначе будет ошибка компиляции).
        /// </summary>
        private void bookInfoLayoutPanel_Paint(object sender, PaintEventArgs e) { }

        /// <summary>
        /// Подписка на события UI элементов.
        /// </summary>
        private void SubscribeToEvents()
        {
            createBookBtn.Click += CreateBookBtn_Click;
            generateBookBtn.Click += GenerateBookBtn_Click;
            searchBtn.Click += SearchBtn_Click;
            bookSellBtn.Click += BookSellBtn_Click;
            btnAcceptDelivery.Click += BtnAcceptDelivery_Click;
            btnRejectDelivery.Click += BtnRejectDelivery_Click;

            shelfSelectCmb.SelectedIndexChanged += (s, e) => UpdateBooksInShelf();
            bookSelectCmb.SelectedIndexChanged += BookSelectCmb_SelectedIndexChanged;

            searchField.KeyPress += SearchField_KeyPress;
            bookNameField.KeyPress += TextBox_KeyPress;
            authorField.KeyPress += TextBox_KeyPress;

            searchTypeCmb.SelectedIndex = 0;
            if (DatabaseManager.Genres.Count > 0)
            {
                ganreComboBox.Items.Clear();
                ganreComboBox.Items.AddRange(DatabaseManager.Genres.ToArray());
            }
        }

        /// <summary>
        /// Инициализация ограничений для элементов управления вводом.
        /// </summary>
        private void InitializeFormControls()
        {
            pagesCountNumbericUpDown.Minimum = 50;
            pagesCountNumbericUpDown.Maximum = 1000;
            pagesCountNumbericUpDown.Value = 200;

            priceNumbericUpDown.Minimum = 50;
            priceNumbericUpDown.Maximum = 5000;
            priceNumbericUpDown.DecimalPlaces = 0;
            priceNumbericUpDown.Value = 200;
        }

        /// <summary>
        /// Запрет ввода цифр в текстовые поля автора и названия.
        /// </summary>
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Поле не должно содержать цифры", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Валидация ввода в поле поиска (запрет букв при поиске по ID).
        /// </summary>
        private void SearchField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (searchTypeCmb.SelectedIndex == 1) // Если поиск по ID
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Обработчик смены выбранной книги в комбобоксе. Отображает информацию о выбранной книге.
        /// </summary>
        private void BookSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bookSelectCmb.SelectedItem != null)
            {
                int bookId = ExtractId(bookSelectCmb.SelectedItem.ToString());
                BookShelf selectedShelf = GetSelectedBookShelf();
                if (selectedShelf != null)
                {
                    Book selectedBook = selectedShelf.FindBookById(bookId);
                    if (selectedBook != null) DisplayBookInfo(selectedBook);
                }
            }
        }

        /// <summary>
        /// Выводит данные о книге в текстовые поля на панели информации.
        /// </summary>
        private void DisplayBookInfo(Book book)
        {
            bookTitleField.Text = book.DisplayTitle;
            bookAuthorField.Text = book.Author;
            bookIDField.Text = book.Id.ToString();

            // СРАЗУ показываем итоговую розничную цену с нашей наценкой 10%
            decimal retailPrice = book.BaseCost * 1.10m;
            bookPriceField.Text = retailPrice.ToString("C");

            bookPagesCountField.Text = book.Pages.ToString();
        }

        /// <summary>
        /// Обновляет тексты на вкладке Покупателей
        /// </summary>
        private void UpdateCustomersTab()
        {
            // Обновляем текст с количеством недовольных
            unsatisfiedLabel.Text = $"Недовольных клиентов: {_gameManager.DissatisfiedCustomers}/{_gameManager.MaxDissatisfied}";

            // Проверяем очередь
            if (_gameManager.CustomerQueue.Count == 0)
            {
                lblNoCustomers.Text = "У Вас пока нет ни одного покупателя";
                lblNoCustomers.ForeColor = Color.Gray;
            }
            else
            {
                // Если покупатель есть, выводим его желание
                Customer currentCustomer = _gameManager.CustomerQueue.Peek();
                lblNoCustomers.Text = $"ПОКУПАТЕЛЬ ПРОСИТ:\n{currentCustomer.ToString()}";
                lblNoCustomers.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Возвращает объект шкафа, выбранного в данный момент в комбобоксе шкафов.
        /// </summary>
        private BookShelf GetSelectedBookShelf()
        {
            if (shelfSelectCmb.SelectedItem == null) return null;
            int shelfId = ExtractId(shelfSelectCmb.SelectedItem.ToString().Replace("Шкаф ", ""));
            return _gameManager.Shop.Shelves.FirstOrDefault(s => s.Id == shelfId);
        }

        /// <summary>
        /// Извлекает числовой идентификатор из строки.
        /// </summary>
        private int ExtractId(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            string[] parts = text.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return int.TryParse(parts[0], out int id) ? id : 0;
        }

        /// <summary>
        /// Очищает текстовые поля информации об уже добавленной книге.
        /// </summary>
        private void ClearBookInfo()
        {
            bookTitleField.Clear(); bookAuthorField.Clear();
            bookIDField.Clear(); bookPriceField.Clear(); bookPagesCountField.Clear();
        }

        /// <summary>
        /// Очищает поля формы создания/заказа новой книги.
        /// </summary>
        private void ClearNewBookFields()
        {
            bookNameField.Clear(); authorField.Clear(); ganreComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Проверяет, заполнены ли все обязательные поля при создании книги.
        /// </summary>
        private bool ValidateBookFields()
        {
            if (string.IsNullOrWhiteSpace(bookNameField.Text) || string.IsNullOrWhiteSpace(authorField.Text) || ganreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля (Название, Автор, Жанр)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Создает текстовые файлы "Базы данных" при первом запуске игры.
        /// </summary>
        private void CreateDataFilesIfNotExist()
        {
            try
            {
                if (!File.Exists(genresFile))
                    File.WriteAllLines(genresFile, new[] { "Роман", "Фантастика", "Детектив", "Фэнтези" });

                if (!File.Exists(bookAuthorPairsFile))
                {
                    File.WriteAllLines(bookAuthorPairsFile, new[] {
                        "Бесы;Достоевский", "Преступление и наказание;Достоевский", "Война и мир;Толстой",
                        "Анна Каренина;Толстой", "Мастер и Маргарита;Булгаков", "Собачье сердце;Булгаков",
                        "Евгений Онегин;Пушкин", "Капитанская дочка;Пушкин", "Герой нашего времени;Лермонтов",
                        "Мцыри;Лермонтов"
                    });
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Принудительно включает двойную буферизацию для контролов
        /// </summary>
        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void searchBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void generateBookBtn_Click_1(object sender, EventArgs e)
        {

        }
    }
}