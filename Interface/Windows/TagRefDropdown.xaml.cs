using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using InfiniteRuntimeTagViewer.Halo;
using System.Linq;

namespace InfiniteRuntimeTagViewer.Interface.Windows
{
    /// <summary>
    /// Interaction logic for tagref_dropdown.xaml
    /// </summary>
    public partial class TagRefDropdown
    {
        public TagRefDropdown()
        {
            InitializeComponent();
        }

        public MainWindow? MainWindow;

		public List<string> source;
		public List<string> datnums;


		private void Window_Deactivated(object sender, EventArgs e)
        {
            Closethis();
        }

        public bool IsClosing;

        public void Closethis()
        {
            if (!IsClosing)
            {
                IsClosing = true;
                Close();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // If the user is holding down left mouse let them drag the window
            if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}

        private void Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = search_block.Text;
			tag_select_panel.ItemsSource = source.Where(p => p.Contains(search)).ToList();

			//foreach (string tv in tag_select_panel.Items)
   //         {


   //             if (!tv.Contains(search))
   //             {
   //                 tv.Visibility = Visibility.Collapsed;
   //                 foreach (TreeViewItem tc in tv.Items)
   //                 {
   //                     if (tc.Header.ToString().Contains(search))
   //                     {
   //                         tc.Visibility = Visibility.Visible;
   //                         tv.Visibility = Visibility.Visible;
   //                     }
   //                     else
   //                     {
   //                         tc.Visibility = Visibility.Collapsed;
   //                     }
   //                 }
   //             }
   //             else
   //             {
   //                 tv.Visibility = Visibility.Visible;
   //                 foreach (TreeViewItem tc in tv.Items)
   //                 {
   //                     tc.Visibility = Visibility.Visible;
   //                 }
   //             }
   //         }
        }

		public TED_TagRefGroup ted;
		public Button TheLastTagrefButtonWePressed;
		private void tag_select_panel_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{


			string _out = datnums[source.IndexOf(e.AddedItems[0].ToString())];
			TED_TagRefGroup newthing = new TED_TagRefGroup(ted)
			{
				MemoryType = "TagrefTag",
				DatNum = _out
			};

			MainWindow.AddPokeChange(newthing, MainWindow.get_tagID_by_datnum(newthing.DatNum));

			string id = MainWindow.get_tagid_by_datnum(newthing.DatNum);
			TheLastTagrefButtonWePressed.Content = MainWindow.convert_ID_to_tag_name(id);


			Grid? td = TheLastTagrefButtonWePressed.Parent as Grid;
			Button? x = td.Children[2] as Button;
			//X.Tag = ID;

			x.Tag = MainWindow.get_tagID_by_datnum(newthing.DatNum); // need to do tagID rather

			if (this != null)
			{
				Closethis();
			}
		}
	}
}