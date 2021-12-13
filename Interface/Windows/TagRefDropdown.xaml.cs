using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assembly69.Interface.Windows
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
                this.DragMove();
        }

        private void Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = search_block.Text;
            foreach (TreeViewItem tv in tag_select_panel.Items)
            {
                if (!tv.Header.ToString().Contains(search))
                {
                    tv.Visibility = Visibility.Collapsed;
                    foreach (TreeViewItem tc in tv.Items)
                    {
                        if (tc.Header.ToString().Contains(search))
                        {
                            tc.Visibility = Visibility.Visible;
                            tv.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            tc.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    tv.Visibility = Visibility.Visible;
                    foreach (TreeViewItem tc in tv.Items)
                    {
                        tc.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}