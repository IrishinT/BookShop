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
        // Ссылки на объекты библиотеки классов
        private Shop shop;
        private List<BookShelf> bookShelves;
        private int nextId = 1;

        // Пути к файлам с данными для генерации
        private readonly string titlesFile = "titles.txt";
        private readonly string authorsFile = "authors.txt";

        public BookStoreForm()
        {
            InitializeComponent();

            // Настройка при загрузке формы
            this.DoubleBuffered = true;
            searchTypeCmb.SelectedIndex = 0;

            // Подписка на события
            SubscribeToEvents();

            // Инициализация магазина
            InitializeShop();

            // Начальная настройка элементов
            InitializeFormControls();

            // Создание файлов с данными, если их нет
            CreateDataFilesIfNotExist();
        }

        /// <summary>
        /// Подписка на все события формы
        /// </summary>
        private void SubscribeToEvents()
        {
            // Кнопки
            createBookBtn.Click += CreateBookBtn_Click;
            generateBookBtn.Click += GenerateBookBtn_Click;
            searchBtn.Click += SearchBtn_Click;
            bookSellBtn.Click += BookSellBtn_Click;

            // Комбобоксы
            shelfSelectCmb.SelectedIndexChanged += ShelfSelectCmb_SelectedIndexChanged;
            bookSelectCmb.SelectedIndexChanged += BookSelectCmb_SelectedIndexChanged;

            // Обработка клавиш
            searchField.KeyPress += SearchField_KeyPress;
            bookNameField.KeyPress += TextBox_KeyPress;
            authorField.KeyPress += TextBox_KeyPress;

            // Обработка события Paint для панели информации о книге
            bookInfoLayoutPanel.Paint += bookInfoLayoutPanel_Paint;
        }

        /// <summary>
        /// Обработчик события Paint для панели информации о книге
        /// </summary>
        private void bookInfoLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
            // Здесь можно добавить код для рисования на панели, если нужно
            // Например, можно нарисовать рамку или фон
            Control panel = sender as Control;
            if (panel != null)
            {
                // Рисуем простую рамку вокруг панели
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            }
        }

        /// <summary>
        /// Инициализация магазина и шкафов
        /// </summary>
        private void InitializeShop()
        {
            try
            {
                // Создаем магазин с балансом 0 и максимальным количеством шкафов 5
                shop = new Shop(0, 5);

                // Создаем несколько шкафов
                bookShelves = new List<BookShelf>
                {
                    new BookShelf(1, "Детектив", 10),
                    new BookShelf(2, "Фантастика", 10),
                    new BookShelf(3, "Роман", 10)
                };

                // Добавляем шкафы в магазин
                foreach (var shelf in bookShelves)
                {
                    shop.AddBookShelf(shelf);
                }

                // Заполняем комбобокс шкафов
                UpdateShelfComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации магазина: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Начальная настройка элементов формы
        /// </summary>
        private void InitializeFormControls()
        {
            // Настройка NumericUpDown
            pagesCountNumbericUpDown.Minimum = 1;
            pagesCountNumbericUpDown.Maximum = 10000;
            pagesCountNumbericUpDown.Value = 100;

            priceNumbericUpDown.Minimum = 0.01m;
            priceNumbericUpDown.Maximum = 100000;
            priceNumbericUpDown.DecimalPlaces = 2;
            priceNumbericUpDown.Value = 500;

            // Автоматическая генерация ID
            idField.Text = nextId.ToString();

            // Обновление баланса
            UpdateBalanceDisplay();
        }

        /// <summary>
        /// Создание файлов с данными для генерации
        /// </summary>
        private void CreateDataFilesIfNotExist()
        {
            try
            {
                // Файл с названиями книг
                if (!File.Exists(titlesFile))
                {
                    File.WriteAllLines(titlesFile, new string[] {
                        "Война и мир", "Преступление и наказание", "Анна Каренина",
                        "Мастер и Маргарита", "Идиот", "Отцы и дети", "Обломов",
                        "Герой нашего времени", "Мертвые души", "Тихий Дон"
                    });
                }

                // Файл с авторами
                if (!File.Exists(authorsFile))
                {
                    File.WriteAllLines(authorsFile, new string[] {
                        "Лев Толстой", "Федор Достоевский", "Михаил Булгаков",
                        "Иван Тургенев", "Иван Гончаров", "Михаил Лермонтов",
                        "Николай Гоголь", "Михаил Шолохов", "Антон Чехов",
                        "Александр Пушкин"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании файлов данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Валидация полей

        private bool ValidateBookFields()
        {
            if (string.IsNullOrWhiteSpace(bookNameField.Text))
            {
                MessageBox.Show("Введите название книги", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(authorField.Text))
            {
                MessageBox.Show("Введите автора книги", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ganreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр книги", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (pagesCountNumbericUpDown.Value <= 0)
            {
                MessageBox.Show("Количество страниц должно быть больше 0", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (priceNumbericUpDown.Value <= 0)
            {
                MessageBox.Show("Цена должна быть больше 0", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрещаем ввод цифр в текстовых полях (для названия и автора)
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Поле не должно содержать цифры", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SearchField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (searchTypeCmb.SelectedIndex == 1) // Поиск по ID
            {
                // Разрешаем только цифры и управляющие клавиши
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Обработчики кнопок

        private void CreateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка валидации всех полей
                if (!ValidateBookFields())
                {
                    return;
                }

                // Получаем выбранный шкаф
                BookShelf selectedShelf = GetSelectedBookShelf();
                if (selectedShelf == null)
                {
                    MessageBox.Show("Выберите шкаф для книги", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем жанр книги и шкафа
                string selectedGenre = ganreComboBox.SelectedItem.ToString();
                if (selectedShelf.Genre != selectedGenre)
                {
                    // Проверяем, пуст ли шкаф (можно сменить жанр)
                    if (selectedShelf.CurrentCount == 0)
                    {
                        // Меняем жанр пустого шкафа
                        selectedShelf.ChangeGenre(selectedGenre);
                    }
                    else
                    {
                        MessageBox.Show($"В этот шкаф можно ставить только книги жанра '{selectedShelf.Genre}'",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Проверяем, есть ли место в шкафу
                if (selectedShelf.CurrentCount >= selectedShelf.Capacity)
                {
                    MessageBox.Show($"В шкафу нет места! Максимальная вместимость: {selectedShelf.Capacity}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Создание книги через библиотеку
                Book newBook = new Book(
                    nextId,
                    bookNameField.Text.Trim(),
                    authorField.Text.Trim(),
                    selectedGenre,
                    (int)pagesCountNumbericUpDown.Value,
                    priceNumbericUpDown.Value
                );

                // Добавление книги в шкаф через библиотеку
                if (selectedShelf.AddBook(newBook))
                {
                    nextId++;
                    idField.Text = nextId.ToString();

                    // Очистка полей для новой книги
                    ClearNewBookFields();

                    // Обновление списка книг и названия шкафа
                    UpdateBooksInShelf();
                    UpdateShelfComboBox();

                    MessageBox.Show($"Книга '{newBook.Title}' успешно создана!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании книги: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Random rand = new Random();

                // Загрузка названий из файла
                string[] titles = File.ReadAllLines(titlesFile);
                string[] authors = File.ReadAllLines(authorsFile);

                // Получаем все книги из всех шкафов для проверки уникальности
                var allBooks = bookShelves.SelectMany(s => s.GetAllBooks()).ToList();

                // Проверка на уникальность названия
                string generatedTitle = titles[rand.Next(titles.Length)];
                int duplicateCount = allBooks.Count(b => b.Title.StartsWith(generatedTitle));

                if (duplicateCount > 0)
                {
                    generatedTitle = $"{generatedTitle} {duplicateCount + 1}";
                }

                // Заполнение полей
                bookNameField.Text = generatedTitle;
                authorField.Text = authors[rand.Next(authors.Length)];

                // Случайный выбор жанра
                if (ganreComboBox.Items.Count > 0)
                {
                    ganreComboBox.SelectedIndex = rand.Next(ganreComboBox.Items.Count);
                }

                // Случайные числа
                pagesCountNumbericUpDown.Value = rand.Next(50, 1000);
                priceNumbericUpDown.Value = rand.Next(100, 5000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации книги: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = searchField.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    MessageBox.Show("Введите текст для поиска", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Book foundBook = null;

                if (searchTypeCmb.SelectedIndex == 0) // Поиск по названию
                {
                    // Ищем книгу во всех шкафах
                    foreach (var shelf in bookShelves)
                    {
                        foundBook = shelf.FindBookByTitle(searchText);
                        if (foundBook != null)
                            break;
                    }
                }
                else // Поиск по ID
                {
                    if (int.TryParse(searchText, out int id))
                    {
                        // Ищем книгу во всех шкафах
                        foreach (var shelf in bookShelves)
                        {
                            foundBook = shelf.FindBookById(id);
                            if (foundBook != null)
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("ID должен быть числом", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (foundBook != null)
                {
                    DisplayBookInfo(foundBook);

                    // Выбираем нужный шкаф в комбобоксе
                    SelectShelfByBook(foundBook);
                }
                else
                {
                    MessageBox.Show("Книга не найдена", "Результат поиска",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BookSellBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (bookSelectCmb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите книгу для продажи", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Получаем выбранную книгу
                string selectedBookInfo = bookSelectCmb.SelectedItem.ToString();
                int bookId = ExtractBookId(selectedBookInfo);

                // Находим шкаф с этой книгой
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
                    // Продажа через библиотеку
                    if (targetShelf.RemoveBook(bookId))
                    {
                        // Увеличиваем баланс магазина
                        shop.AddToBalance(bookToSell.Price);
                        UpdateBalanceDisplay();

                        // Обновляем списки
                        UpdateBooksInShelf();
                        ClearBookInfo();

                        MessageBox.Show($"Книга '{bookToSell.Title}' продана за {bookToSell.Price} руб.",
                            "Продажа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при продаже: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Обработчики комбобоксов

        private void ShelfSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBooksInShelf();
        }

        private void BookSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bookSelectCmb.SelectedItem != null)
            {
                string selectedBookInfo = bookSelectCmb.SelectedItem.ToString();
                int bookId = ExtractBookId(selectedBookInfo);

                // Ищем книгу в текущем шкафу
                BookShelf selectedShelf = GetSelectedBookShelf();
                if (selectedShelf != null)
                {
                    Book selectedBook = selectedShelf.FindBookById(bookId);
                    if (selectedBook != null)
                    {
                        DisplayBookInfo(selectedBook);
                    }
                }
            }
        }

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Обновление списка шкафов в комбобоксе
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
        /// Получение выбранного шкафа
        /// </summary>
        private BookShelf GetSelectedBookShelf()
        {
            if (shelfSelectCmb.SelectedItem == null)
                return null;

            string selectedShelf = shelfSelectCmb.SelectedItem.ToString();
            int shelfId = ExtractShelfId(selectedShelf);

            return bookShelves.FirstOrDefault(s => s.Id == shelfId);
        }

        /// <summary>
        /// Очистка полей для новой книги
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
        /// Обновление списка книг в выбранном шкафу
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

                // Обновление информации о загруженности шкафа
                shelfCapacity.Text = $"Загруженность {booksInShelf.Count}/{selectedShelf.Capacity}";

                if (booksInShelf.Count >= selectedShelf.Capacity)
                {
                    shelfCapacity.ForeColor = Color.Red;
                }
                else
                {
                    shelfCapacity.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Отображение информации о книге
        /// </summary>
        private void DisplayBookInfo(Book book)
        {
            bookTitleField.Text = book.Title;
            bookAuthorField.Text = book.Author;
            bookIDField.Text = book.Id.ToString();
            bookPriceField.Text = book.Price.ToString("C");
            bookPagesCountField.Text = book.Pages.ToString();
        }

        /// <summary>
        /// Очистка информации о книге
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
        /// Обновление отображения баланса
        /// </summary>
        private void UpdateBalanceDisplay()
        {
            if (shop != null)
            {
                balanceLb.Text = $"Баланс: {shop.Balance:C}";
            }
        }

        /// <summary>
        /// Извлечение ID книги из строки комбобокса
        /// </summary>
        private int ExtractBookId(string bookInfo)
        {
            if (string.IsNullOrEmpty(bookInfo)) return 0;
            string[] parts = bookInfo.Split(':');
            return int.TryParse(parts[0], out int id) ? id : 0;
        }

        /// <summary>
        /// Извлечение ID шкафа из строки комбобокса
        /// </summary>
        private int ExtractShelfId(string shelfInfo)
        {
            if (string.IsNullOrEmpty(shelfInfo)) return 0;

            // Формат: "Шкаф 1 (Детектив)"
            int startIndex = shelfInfo.IndexOf(' ') + 1;
            int endIndex = shelfInfo.IndexOf(' ', startIndex);

            if (startIndex > 0 && endIndex > startIndex)
            {
                string idStr = shelfInfo.Substring(startIndex, endIndex - startIndex);
                return int.TryParse(idStr, out int id) ? id : 0;
            }

            return 0;
        }

        /// <summary>
        /// Выбор шкафа по книге
        /// </summary>
        private void SelectShelfByBook(Book book)
        {
            for (int i = 0; i < bookShelves.Count; i++)
            {
                if (bookShelves[i].GetAllBooks().Any(b => b.Id == book.Id))
                {
                    shelfSelectCmb.SelectedIndex = i;
                    break;
                }
            }
        }

        #endregion
    }
}