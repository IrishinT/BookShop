using System;
using System.Windows.Forms;

namespace BookShop
{
    public partial class TitleForm : Form
    {
        public string SelectedDifficulty { get; private set; }

        public TitleForm()
        {
            InitializeComponent();
        }

        private void btnEasy_Click(object sender, EventArgs e)
        {
            ShowConfirmation("Лёгкий");
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            ShowConfirmation("Нормальный");
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            ShowConfirmation("Сложный");
        }

        private void ShowConfirmation(string difficulty)
        {
            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите начать игру в режиме {difficulty}?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SelectedDifficulty = difficulty;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "ИГРА «КНИЖНЫЙ МАГНАТ»\n\n" +
                "Вы управляете книжным магазином.\n\n" +
                "Ваши задачи:\n" +
                "• Заказывать книги\n" +
                "• Проверять поставки на плагиат и опечатки\n" +
                "• Обслуживать покупателей\n" +
                "• Следить за балансом и очередью\n\n" +
                "Условия поражения:\n" +
                "• Баланс = 0\n" +
                "• Очередь > 5 покупателей\n" +
                "• 3 недовольных клиента\n\n" +
                "Приятной игры!\n\n" +
                "Разработчик: Команда DevTeam",
                "Об игре",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}