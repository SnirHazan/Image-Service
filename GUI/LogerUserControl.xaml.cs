using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for LogerUserControl.xaml
    /// </summary>
    public partial class LogerUserControl : UserControl
    {
        /// <summary>
        /// The constructor of the LogerUserControl.
        /// </summary>
        public LogerUserControl()
        {
            InitializeComponent();
            LogViewModel lvm = new LogViewModel(new LogModel());
            DataContext = lvm;
        }
    }
}
