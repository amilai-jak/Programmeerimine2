using System.Windows;

namespace KooliProjekt.WpfApplication
{
    // 09.04.2026 - aken seotakse view modeliga
    // 10.04.2026 - andmete laadimine toimub vormi avamisel
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            Loaded += async (s, e) => await viewModel.LoadData();
        }
    }
}
