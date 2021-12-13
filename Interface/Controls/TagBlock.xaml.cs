using System.Collections.Generic;
using System.Windows.Controls;

using InfiniteRuntimeTagViewer.Halo;
using InfiniteRuntimeTagViewer.Halo.TagObjects;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
    /// <summary>
    /// Interaction logic for tagblock.xaml
    /// </summary>
    public partial class TagBlock
    {
        public TagEditorControl EditorControl { get; }

		public TagBlock(TagEditorControl editorControl, long blockOffset, TagStruct tagStruct)
		{
			InitializeComponent();
			EditorControl = editorControl;
			BlockOffset = blockOffset;	
			TagStruct = tagStruct;
		}

		public KeyValuePair<long, Vehi.C> Children { get; set; }
		public long BlockAddress { get; set; }
		public long BlockOffset { get; set; }
		public TagStruct TagStruct { get; set; }

        private void indexbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditorControl.recall_blockloop(TagStruct, BlockOffset + (indexbox.SelectedIndex * Children.Value.S), 
				Children, BlockAddress + (indexbox.SelectedIndex * Children.Value.S), dockpanel);
        }
    }
}