using System;
using System.Windows.Forms;

namespace BookShop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Показываем титульный экран
            using (TitleForm titleForm = new TitleForm())
            {
                if (titleForm.ShowDialog() == DialogResult.OK)
                {
                    // Пользователь выбрал режим и подтвердил
                    string difficulty = titleForm.SelectedDifficulty;

                    // Запускаем главную форму с выбранной сложностью
                    Application.Run(new BookStoreForm(difficulty));
                }
                // Если пользователь закрыл титульник (крестик) или нажал "Нет" — программа завершается
            }
        }
    }
}