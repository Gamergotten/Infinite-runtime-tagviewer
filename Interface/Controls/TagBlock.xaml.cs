using System.Collections.Generic;
using System.Windows.Controls;
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

        public KeyValuePair<long, vehi.c> children;
        public long block_address;


        private void indexbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditorControl.recall_blockloop(children, block_address + (indexbox.SelectedIndex * children.Value.S), dockpanel);
        }
    }
}
