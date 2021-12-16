using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

		public KeyValuePair<long, TagLayouts.C> Children { get; set; }
		public long BlockAddress { get; set; }
		public long BlockOffset { get; set; }
		public TagStruct TagStruct { get; set; }

        private void indexbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditorControl.recall_blockloop(TagStruct, BlockOffset + (indexbox.SelectedIndex * Children.Value.S), 
				Children, BlockAddress + (indexbox.SelectedIndex * Children.Value.S), dockpanel);
        }

		Random rand = new Random();
		private void GroupBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Storyboard sb = (Storyboard) TryFindResource("AnimateRotationStoryBoard");

			if (rand == null)
			{
				rand = new Random();
			}

			if (rand.Next(1, 30) == 1)
			{
				sb.Begin();
			}
		}
	}
}