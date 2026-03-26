using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookShopLibrary
{
    /// <summary>
    /// Работа с "Базой данных" (текстовыми файлами) без хардкода.
    /// </summary>
    public static class DatabaseManager
    {
        private static readonly string LogFilePath = "Database.txt";
        private static readonly string PairsFilePath = "book-author-pairs.txt";
        private static readonly string GenresFilePath = "genres.txt";

        public static List<(string Title, string Author)> BookAuthorPairs { get; private set; } = new List<(string, string)>();
        public static List<string> Genres { get; private set; } = new List<string>();

        /// <summary>
        /// Инициализация БД при старте игры.
        /// </summary>
        public static void Initialize()
        {
            LoadPairs();
            LoadGenres();
            
            // ТРЕБОВАНИЕ ТЗ: В "Базу данных" изначально должны добавляться все 10 пар
            LogEvent("--- НОВЫЙ ИГРОВОЙ ДЕНЬ НАЧАТ ---");
            string initialPairsLog = "Изначально загруженные пары в Базу Данных:\n" + 
                                     string.Join("\n", BookAuthorPairs.Select(p => $"{p.Title} - {p.Author}"));
            LogEvent(initialPairsLog);
        }

        private static void LoadPairs()
        {
            BookAuthorPairs.Clear();
            if (!File.Exists(PairsFilePath))
                throw new FileNotFoundException($"Файл {PairsFilePath} не найден! Требуется текстовый файл минимум с 10 парами.");

            var lines = File.ReadAllLines(PairsFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length == 2)
                    BookAuthorPairs.Add((parts[0].Trim(), parts[1].Trim()));
            }

            // Проверка строгого требования ТЗ
            if (BookAuthorPairs.Count < 10)
                throw new Exception("По ТЗ в файле book-author-pairs.txt должно быть минимум 10 пар книга-автор!");
        }

        private static void LoadGenres()
        {
            Genres.Clear();
            if (File.Exists(GenresFilePath))
                Genres = File.ReadAllLines(GenresFilePath).Where(g => !string.IsNullOrWhiteSpace(g)).ToList();
        }

        /// <summary>
        /// Ручное добавление новой книги (Заказ). Записывается в файл и лог (Требование ТЗ).
        /// </summary>
        public static void AddNewPair(string title, string author)
        {
            if (!BookAuthorPairs.Any(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                BookAuthorPairs.Add((title, author));
                
                // Перезаписываем файл пар
                var lines = BookAuthorPairs.Select(p => $"{p.Title};{p.Author}");
                File.WriteAllLines(PairsFilePath, lines);
                
                // Дописываем в лог-базу
                LogEvent($"Новая пара добавлена в БД пользователем: {title} - {author}");
            }
        }

        /// <summary>
        /// Запись события в основной файл Базы Данных
        /// </summary>
        public static void LogEvent(string message)
        {
            string line = $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} | {message}{Environment.NewLine}";
            File.AppendAllText(LogFilePath, line);
        }

        /// <summary>
        /// Финальная запись итога игры
        /// </summary>
        public static void LogGameResult(bool isWin, decimal finalBalance, string reason)
        {
            string status = isWin ? "ПОБЕДА" : "ПОРАЖЕНИЕ";
            string log = $"=== ИТОГ ИГРЫ: {status} ===\nФинальный баланс: {finalBalance:C}\nПричина: {reason}\n====================================\n";
            File.AppendAllText(LogFilePath, log);
        }
    }
}