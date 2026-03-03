namespace BookShop
{
    public partial class BookStoreForm : Form
    {
        public BookStoreForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            searchTypeCmb.SelectedIndex = 0;
        }
    }
}
