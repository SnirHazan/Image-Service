using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for SettingUserControl.xaml
    /// </summary>
    public partial class SettingUserControl : UserControl
    {
        private SettingsViewModal svm;
        /// <summary>
        /// Constructor of SettingsUserControl
        /// </summary>
        public SettingUserControl()
        {
            InitializeComponent();
            svm = new SettingsViewModal(new SettingModal());
            DataContext = svm;
        }
    }
}
