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
    public partial class TagStructBlock
    {
        public TagEditorControl EditorControl { get; }

		public KeyValuePair<long, TagLayouts.C> Children { get; set; }
		public long BlockAddress { get; set; }
		public TagStruct TagStruct { get; set; }

		public TagStructBlock(TagEditorControl editorControl, TagStruct tagStruct, KeyValuePair<long, TagLayouts.C> entry, long the_address)
		{
			InitializeComponent();
			
			EditorControl = editorControl;
			TagStruct = tagStruct;
			Children = entry;
			BlockAddress = the_address;
			
			TextBox tb = new TextBox { Text = "Test" };
			dockpanel.Children.Add(tb);
			
			EditorControl.recall_blockloop(TagStruct, Children.Value.S, Children, BlockAddress + Children.Value.S, dockpanel, Children.Value.AbsoluteTagOffset);
		}		
	}
}