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

		public TagBlock(TagEditorControl editorControl, TagStruct tagStruct)
		{
			InitializeComponent();
			EditorControl = editorControl;
			TagStruct = tagStruct;
		}

		public KeyValuePair<long, TagLayouts.C> Children { get; set; }
		public long BlockAddress { get; set; }
		public TagStruct TagStruct { get; set; }

        private void indexbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (indexbox.SelectedIndex > -1)
			{
				EditorControl.recall_blockloop(TagStruct, (indexbox.SelectedIndex * Children.Value.S),Children, BlockAddress + (indexbox.SelectedIndex * Children.Value.S), dockpanel, Children.Value.AbsoluteTagOffset); // bro
				stored_num_on_index = indexbox.SelectedIndex; // unless from creation hmmm
				its_too_late_at_night_for_me_to_think_of_a_better_way_to_do_this = true;
				Expand_Collapse_Button.Content = "-";
			}
			else
			{
				dockpanel.Children.Clear();

			}


			// OK WE HAVE A PROBLEM HERE, + (indexbox.SelectedIndex * Children.Value.S)
			// i dont see a problem here, past me
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

		public int stored_num_on_index = 0;

		public bool its_too_late_at_night_for_me_to_think_of_a_better_way_to_do_this;

		private void Expand_Collapse_Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			its_too_late_at_night_for_me_to_think_of_a_better_way_to_do_this = !its_too_late_at_night_for_me_to_think_of_a_better_way_to_do_this;
			if (its_too_late_at_night_for_me_to_think_of_a_better_way_to_do_this)
			{
				indexbox.SelectedIndex = stored_num_on_index;
				Expand_Collapse_Button.Content = "-";
			}
			else
			{
				indexbox.SelectedIndex = -1;
				Expand_Collapse_Button.Content = "+";
			}
		}
	}
}
