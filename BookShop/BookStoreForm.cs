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
    /// Главная форма приложения "Книжный магазин".
    /// Предоставляет интерфейс для управления книгами, шкафами и продажами.
    /// </summary>
    public partial class BookStoreForm : Form
    {
        private Shop shop;
        private List<BookShelf> bookShelves;
        private int nextId = 1;
        private readonly string titlesFile = "titles.txt";
        private readonly string authorsFile = "authors.txt";
        private readonly string genresFile = "genres.txt";

        // Поле для хранения выбранной сложности
        private string gameDifficulty;

        /// <summary>
        /// Конструктор формы. Инициализирует магазин, элементы управления и загружает данные.
        /// </summary>
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

            gameDifficulty = "Нормальный"; // значение по умолчанию
        }

        /// <summary>
        /// Конструктор формы с выбором сложности. Запускается из титульного экрана.
        /// </summary>
        /// <param name="difficulty">Выбранный режим сложности (Лёгкий/Нормальный/Сложный)</param>
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

            gameDifficulty = difficulty;

            // Настраиваем параметры игры в зависимости от сложности
            ConfigureGameDifficulty();
        }

        /// <summary>
        /// Настройка параметров игры в зависимости от выбранного режима сложности.
        /// </summary>
        private void ConfigureGameDifficulty()
        {
            switch (gameDifficulty)
            {
                case "Лёгкий":
                    // Легкий режим: больше баланс
                    if (shop != null)
                    {
                        shop.AddToBalance(2000);
                    }
                    MessageBox.Show("Запущен ЛЁГКИЙ режим!\n\n" +
                        "• Стартовый баланс: 2000 руб.\n" +
                        "• Книги приходят реже\n" +
                        "• Покупатели появляются реже\n" +
                        "• Очередь до 5 покупателей",
                        "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Нормальный":
                    // Нормальный режим: стандартные настройки
                    if (shop != null)
                    {
                        shop.AddToBalance(1000);
                    }
                    MessageBox.Show("Запущен НОРМАЛЬНЫЙ режим!\n\n" +
                        "• Стартовый баланс: 1000 руб.\n" +
                        "• Стандартная частота событий\n" +
                        "• Очередь до 4 покупателей",
                        "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Сложный":
                    // Сложный режим: меньше баланс
                    if (shop != null)
                    {
                        shop.AddToBalance(500);
                    }
                    MessageBox.Show("Запущен СЛОЖНЫЙ режим!\n\n" +
                        "• Стартовый баланс: 500 руб.\n" +
                        "• Книги приходят чаще\n" +
                        "• Покупатели появляются чаще\n" +
                        "• Очередь до 3 покупателей",
                        "Режим игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    gameDifficulty = "Нормальный";
                    if (shop != null)
                    {
                        shop.AddToBalance(1000);
                    }
                    break;
            }
        }

        /// <summary>
        /// Подписывает элементы управления на обработчики событий.
        /// </summary>
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
        }

        /// <summary>
        /// Пустой обработчик события Paint для панели информации о книге.
        /// </summary>
        private void bookInfoLayoutPanel_Paint(object sender, PaintEventArgs e) { }

        /// <summary>
        /// Инициализирует объект магазина и список шкафов.
        /// </summary>
        private void InitializeShop()
        {
            try
            {
                shop = new Shop(0, 5);
                bookShelves = new List<BookShelf>();
                UpdateShelfComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации магазина: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Настраивает начальные значения элементов управления формы (NumericUpDown, поля ID и баланса).
        /// </summary>
        private void InitializeFormControls()
        {
            pagesCountNumbericUpDown.Minimum = 1;
            pagesCountNumbericUpDown.Maximum = 10000;
            pagesCountNumbericUpDown.Value = 100;

            priceNumbericUpDown.Minimum = 0.01m;
            priceNumbericUpDown.Maximum = 100000;
            priceNumbericUpDown.DecimalPlaces = 2;
            priceNumbericUpDown.Value = 500;

            idField.Text = nextId.ToString();
            UpdateBalanceDisplay();
        }

        /// <summary>
        /// Создаёт файлы с данными (названия, авторы, жанры), если они не существуют.
        /// </summary>
        private void CreateDataFilesIfNotExist()
        {
            try
            {
                if (!File.Exists(titlesFile))
                {
                    File.WriteAllLines(titlesFile, new string[] {
                        "Тайна в тумане", "Путешествие в неизвестность", "Мир приключений",
                        "Сердце в опасности", "Звёзды", "Путь к мечте", "Легенда",
                        "Хранитель древних знаний", "Загадочный остров", "Время любви"
                    });
                }

                if (!File.Exists(authorsFile))
                {
                    File.WriteAllLines(authorsFile, new string[] {
                        "Иван Петров", "Мария Соколова", "Алексей Волков",
                        "Елена Морозова", "Дмитрий Козлов", "Анна Лебедева",
                        "Сергей Новиков", "Ольга Смирнова", "Павел Орлов",
                        "Екатерина Васильева"
                    });
                }

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

        /// <summary>
        /// Загружает список жанров из файла в ComboBox.
        /// </summary>
        private void LoadGenresFromFile()
        {
            try
            {
                if (File.Exists(genresFile))
                {
                    string[] genres = File.ReadAllLines(genresFile);
                    ganreComboBox.Items.Clear();
                    foreach (string genre in genres)
                    {
                        if (!string.IsNullOrWhiteSpace(genre))
                        {
                            ganreComboBox.Items.Add(genre.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке жанров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Проверяет заполненность всех обязательных полей для создания книги.
        /// </summary>
        /// <returns>True если все поля заполнены корректно, иначе False.</returns>
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

        /// <summary>
        /// Обработчик нажатия клавиш в текстовых полях. Запрещает ввод цифр в поля текста.
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
        /// Обработчик нажатия клавиш в поле поиска. Разрешает только цифры при поиске по ID.
        /// </summary>
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

        /// <summary>
        /// Проверяет существование книги с таким названием во всём магазине 
        /// и при необходимости добавляет суффикс (2, 3, 4...) для уникальности.
        /// </summary>
        /// <param name="baseTitle">Исходное название книги</param>
        /// <returns>Уникальное название книги</returns>
        private string GetUniqueTitle(string baseTitle)
        {
            string uniqueTitle = baseTitle;
            int suffix = 2;

            // Проверяем все книги во всех шкафах магазина
            while (bookShelves.Any(shelf =>
                shelf.GetAllBooks().Any(b =>
                    b.Title.Equals(uniqueTitle, StringComparison.OrdinalIgnoreCase))))
            {
                uniqueTitle = $"{baseTitle} {suffix}";
                suffix++;
            }

            return uniqueTitle;
        }

        /// <summary>
        /// Обработчик кнопки создания книги. Добавляет книгу в соответствующий шкаф.
        /// </summary>
        private void CreateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBookFields()) return;

                string selectedGenre = ganreComboBox.SelectedItem.ToString();

                // Проверка на дубликаты названия
                string baseTitle = bookNameField.Text.Trim();
                string uniqueTitle = GetUniqueTitle(baseTitle);
                if (uniqueTitle != baseTitle)
                {
                    bookNameField.Text = uniqueTitle; // Обновляем поле для отображения
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
                    uniqueTitle, // Используем уникальное название
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

                    MessageBox.Show($"Книга '{newBook.Title}' успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Обработчик кнопки генерации случайной книги. Заполняет поля случайными данными.
        /// </summary>
        private void GenerateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Book generatedBook = Book.GenerateRandom(nextId);

                // Проверка на дубликаты для сгенерированного названия
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

        // ===== Обработчики вкладки "Магазин" =====

        /// <summary>
        /// Обработчик кнопки поиска книги по названию или ID.
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

        /// <summary>
        /// Обработчик кнопки продажи книги. Удаляет книгу из шкафа и добавляет деньги в баланс.
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

                string selectedBookInfo = bookSelectCmb.SelectedItem.ToString();
                int bookId = ExtractBookId(selectedBookInfo);

                BookShelf targetShelf = null;
                Book bookToSell = null;

                foreach (var shelf in bookShelves)
                {
                    bookToSell = shelf.FindBookById(bookId);
                    if (bookToSell != null)
                    {
                        targetShelf = shelf;
                        break;
                    }
                }

                if (bookToSell != null && targetShelf != null)
                {
                    if (targetShelf.RemoveBook(bookId))
                    {
                        shop.AddToBalance(bookToSell.Price);
                        UpdateBalanceDisplay();
                        UpdateBooksInShelf();
                        ClearBookInfo();

                        MessageBox.Show($"Книга '{bookToSell.Title}' продана за {bookToSell.Price} руб.", "Продажа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при продаже: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик изменения выбранного шкафа. Обновляет список книг в шкафу.
        /// </summary>
        private void ShelfSelectCmb_SelectedIndexChanged(object sender, EventArgs e) => UpdateBooksInShelf();

        /// <summary>
        /// Обработчик изменения выбранной книги. Отображает информацию о книге.
        /// </summary>
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

        /// <summary>
        /// Обновляет список шкафов в ComboBox.
        /// </summary>
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

        /// <summary>
        /// Возвращает выбранный шкаф из ComboBox.
        /// </summary>
        /// <returns>Объект BookShelf или null если ничего не выбрано.</returns>
        private BookShelf GetSelectedBookShelf()
        {
            if (shelfSelectCmb.SelectedItem == null) return null;

            string selectedShelf = shelfSelectCmb.SelectedItem.ToString();
            int shelfId = ExtractShelfId(selectedShelf);

            return bookShelves.FirstOrDefault(s => s.Id == shelfId);
        }

        /// <summary>
        /// Очищает все поля на вкладке "Новая книга".
        /// </summary>
        private void ClearNewBookFields()
        {
            bookNameField.Clear();
            authorField.Clear();
            ganreComboBox.SelectedIndex = -1;
            pagesCountNumbericUpDown.Value = pagesCountNumbericUpDown.Minimum;
            priceNumbericUpDown.Value = priceNumbericUpDown.Minimum;
        }

        /// <summary>
        /// Обновляет список книг в выбранном шкафу и отображает загруженность.
        /// </summary>
        private void UpdateBooksInShelf()
        {
            bookSelectCmb.Items.Clear();
            BookShelf selectedShelf = GetSelectedBookShelf();
            if (selectedShelf != null)
            {
                var booksInShelf = selectedShelf.GetAllBooks();
                foreach (var book in booksInShelf)
                {
                    bookSelectCmb.Items.Add($"{book.Id}: {book.Title} - {book.Author}");
                }

                shelfCapacity.Text = $"Загруженность {booksInShelf.Count}/{selectedShelf.Capacity}";
                shelfCapacity.ForeColor = booksInShelf.Count >= selectedShelf.Capacity ? Color.Red : Color.Black;
            }
        }

        /// <summary>
        /// Отображает информацию о книге в полях на вкладке "Магазин".
        /// </summary>
        /// <param name="book">Книга для отображения</param>
        private void DisplayBookInfo(Book book)
        {
            if (book != null)
            {
                bookTitleField.Text = book.Title;
                bookAuthorField.Text = book.Author;
                bookIDField.Text = book.Id.ToString();
                bookPriceField.Text = book.Price.ToString("C");
                bookPagesCountField.Text = book.Pages.ToString();
            }
        }

        /// <summary>
        /// Очищает поля информации о книге на вкладке "Магазин".
        /// </summary>
        private void ClearBookInfo()
        {
            bookTitleField.Clear();
            bookAuthorField.Clear();
            bookIDField.Clear();
            bookPriceField.Clear();
            bookPagesCountField.Clear();
        }

        /// <summary>
        /// Обновляет отображение баланса магазина.
        /// </summary>
        private void UpdateBalanceDisplay()
        {
            if (shop != null)
                balanceLb.Text = $"Баланс: {shop.Balance:C}";
        }

        /// <summary>
        /// Извлекает ID книги из строки отображения в ComboBox.
        /// </summary>
        /// <param name="bookInfo">Строка формата "ID: Название - Автор"</param>
        /// <returns>ID книги или 0 если не удалось извлечь</returns>
        private int ExtractBookId(string bookInfo)
        {
            if (string.IsNullOrEmpty(bookInfo)) return 0;
            string[] parts = bookInfo.Split(':');
            return int.TryParse(parts[0], out int id) ? id : 0;
        }

        /// <summary>
        /// Извлекает ID шкафа из строки отображения в ComboBox.
        /// </summary>
        /// <param name="shelfInfo">Строка формата "Шкаф ID (Жанр)"</param>
        /// <returns>ID шкафа или 0 если не удалось извлечь</returns>
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