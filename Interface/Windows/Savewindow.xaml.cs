using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using System.IO;
namespace InfiniteRuntimeTagViewer.Interface.Windows
{
	/// <summary>
	/// Interaction logic for Savewindow.xaml
	/// </summary>
	public partial class Savewindow : Window
	{
		public Savewindow()
		{
			InitializeComponent();
		}

		public MainWindow main;

		public string extrastufftoappend;

		public string base_path;

		public void ill_take_it_from_here_mainwindow(string file_path, string all_the_poke_text)
		{
			NAME_FIELD.Text = System.IO.Path.GetFileNameWithoutExtension(file_path);
			FileInfo file_info = new FileInfo(file_path);
			base_path = file_info.DirectoryName;
			extrastufftoappend = all_the_poke_text;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			// SAVE
			string dump = "";
			string destination = base_path + "\\" + NAME_FIELD.Text + ".irtv";
			if (DESC_FIELD.Text == "" && filters_panel.Children.Count == 0)
			{
				System.IO.File.WriteAllText(destination, extrastufftoappend);
			}
			else
			{
				dump = "^";	
				foreach (var pee in filters_panel.Children)
				{
					TextBox tb = pee as TextBox;
					if (tb.Text != "" && tb.Text != null && !tb.Text.Contains("~"))
					{
						dump += tb.Text + "~";
					}
				}
				dump += "\r\n"+DESC_FIELD.Text + "\r\n";
				dump += "^\r\n";
				dump += extrastufftoappend;
				System.IO.File.WriteAllText(destination, dump);

			}
			if (copycheck.IsChecked == true)
			{
				string fileNameWithExt = System.IO.Path.GetFileName(destination);
				string target_folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IRTV";
				if (!Directory.Exists(target_folder))
					Directory.CreateDirectory(target_folder);
				string destPath = System.IO.Path.Combine(target_folder, fileNameWithExt);
				if (File.Exists(destPath))
				{
					File.Delete(destPath);
				}
				File.Copy(destination, destPath);
			}
			main.Focus();
			this.Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			// CANCEL
			main.Focus();
			this.Close();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			// new FILTER
			TextBox tb = new() { };
			filters_panel.Children.Add(tb);
			tb.Text = "filter" + filters_panel.Children.Count;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			// REMOVE FILTER
			filters_panel.Children.RemoveAt(filters_panel.Children.Count-1);
		}
	}
}
