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
        private readonly string titlesFile = "titles.txt";
        private readonly string authorsFile = "authors.txt";
        private readonly string genresFile = "genres.txt";

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
        }

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

        private void bookInfoLayoutPanel_Paint(object sender, PaintEventArgs e) { }

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

        private void CreateDataFilesIfNotExist()
        {
            try
            {
                if (!File.Exists(titlesFile))
                {
                    File.WriteAllLines(titlesFile, new string[] {
                        "Война и мир", "Преступление и наказание", "Анна Каренина",
                        "Мастер и Маргарита", "Идиот", "Отцы и дети", "Обломов",
                        "Герой нашего времени", "Мертвые души", "Тихий Дон"
                    });
                }

                if (!File.Exists(authorsFile))
                {
                    File.WriteAllLines(authorsFile, new string[] {
                        "Лев Толстой", "Федор Достоевский", "Михаил Булгаков",
                        "Иван Тургенев", "Иван Гончаров", "Михаил Лермонтов",
                        "Николай Гоголь", "Михаил Шолохов", "Антон Чехов",
                        "Александр Пушкин"
                    });
                }

                if (!File.Exists(genresFile))
                {
                    File.WriteAllLines(genresFile, new string[] {
                        "Роман", "Фантастика", "Детектив", "Приключения",
                        "Поэзия", "Драма", "Триллер", "Фэнтези",
                        "Биография", "Научная литература"
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
                MessageBox.Show("Цена должна быть больше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void CreateBookBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBookFields()) return;

                string selectedGenre = ganreComboBox.SelectedItem.ToString();
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
                        MessageBox.Show("Нет доступных шкафов. Достигнут максимальный лимит.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (targetShelf.CurrentCount >= targetShelf.Capacity)
                {
                    MessageBox.Show($"В шкафу нет места! Максимальная вместимость: {targetShelf.Capacity}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Book newBook = new Book(
                    nextId,
                    bookNameField.Text.Trim(),
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

                    MessageBox.Show($"Книга '{newBook.Title}' успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Book generatedBook = Book.GenerateRandom(nextId);

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

        // ===== Исправленный метод поиска книги =====
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
                    bookSelectCmb.Items.Add($"{book.Id}: {book.Title} - {book.Author}");
                }

                shelfCapacity.Text = $"Загруженность {booksInShelf.Count}/{selectedShelf.Capacity}";
                shelfCapacity.ForeColor = booksInShelf.Count >= selectedShelf.Capacity ? Color.Red : Color.Black;
            }
        }

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