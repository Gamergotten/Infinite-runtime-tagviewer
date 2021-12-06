using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Assembly69.object_classes;

namespace Assembly69.theUIstuff
{
    /// <summary>
    /// Interaction logic for tagblock.xaml
    /// </summary>
    public partial class tagblock : UserControl
    {
        public tagblock()
        {
            InitializeComponent();
        }

        public KeyValuePair<long, vehi.c> children;
        public long block_address;
        public MainWindow mainWindow;

        private void indexbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.recall_blockloop(children, block_address + (indexbox.SelectedIndex * children.Value.S), dockpanel);
        }
    }
}
