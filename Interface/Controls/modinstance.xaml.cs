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

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
	/// <summary>
	/// Interaction logic for modinstance.xaml
	/// </summary>
	public partial class modinstance : UserControl
	{
		public modinstance()
		{
			InitializeComponent();
		}
		public string filepath;
		public MainWindow main;

		public List<string> filters;

		// add pokes to mainwindow
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			border_to_change.BorderBrush = Brushes.YellowGreen;
			main.recieve_file_to_inhalo_pokes(filepath);
		}

		// delete button
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			main.mwidow.delete_file(filepath);
		}
	}
}
