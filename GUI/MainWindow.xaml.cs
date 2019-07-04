using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The constructor of the MainWindows.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModal mainModal = new MainWindowViewModal(new MainWindowModal());
            DataContext = mainModal;
        }
    }
}
