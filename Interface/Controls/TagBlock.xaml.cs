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

using Assembly69.Halo.TagObjects;

namespace Assembly69.Interface.Controls {
    /// <summary>
    /// Interaction logic for tagblock.xaml
    /// </summary>
    public partial class TagBlock : UserControl
    {
        public TagEditorControl EditorControl { get; }


        public TagBlock(TagEditorControl editorControl)
        {
            InitializeComponent();
            EditorControl = editorControl;
        }

        public KeyValuePair<long, Vehi.C> Children;
        public long BlockAddress;


        private void indexbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditorControl.recall_blockloop(Children, BlockAddress + (indexbox.SelectedIndex * Children.Value.S), dockpanel);
        }
    }
}
