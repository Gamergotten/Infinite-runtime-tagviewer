using Assembly69.Halo.TagObjects;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Assembly69.Interface.Controls
{
    /// <summary>
    /// Interaction logic for tagblock.xaml
    /// </summary>
    public partial class TagBlock
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