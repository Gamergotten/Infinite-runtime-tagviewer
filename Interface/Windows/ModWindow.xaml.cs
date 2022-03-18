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
using System.Windows.Shapes;
using System.IO;
using InfiniteRuntimeTagViewer.Interface.Controls;
using System.Diagnostics;

namespace InfiniteRuntimeTagViewer.Interface.Windows
{
	/// <summary>
	/// Interaction logic for ModWindow.xaml
	/// </summary>
	public partial class ModWindow : Window
	{
		public ModWindow()
		{
			InitializeComponent();
		}
		public MainWindow main;
		public Dictionary<string, modcheckfilter> loaded_filters = new();
		public List<modinstance> loaded_modsui = new List<modinstance>();


		private void Window_Closed(object sender, EventArgs e)
		{
			main.mwidow = null;
		}
		public void load_mods_from_directories()
		{
			loaded_filters.Clear();
			loaded_modsui.Clear();
			filterspanel.Children.Clear();
			mods_panel.Children.Clear();


			// physical program directory
			string appdata_ = Directory.GetCurrentDirectory()+ "\\MODS";
			if (Directory.Exists(appdata_))
			{
				load_mod_directory(appdata_);
			}
			// appdata folder
			string program_data = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\IRTV";
			if (Directory.Exists(program_data))
			{
				load_mod_directory(program_data);
			}
		}
		public void load_mod_directory(string path)
		{
		 	string[] files = Directory.GetFiles(path);
			foreach(string file in files)
			{
				if (System.IO.Path.GetExtension(file) == ".irtv")
				{

					modinstance mi = new();
					mods_panel.Children.Add(mi);
					loaded_modsui.Add(mi);
					mi.filepath = file;
					mi.main = main;
					mi.title.Text = System.IO.Path.GetFileNameWithoutExtension(file);
					List<string> dump_filters = new();
					using (StreamReader reader = new StreamReader(file))
					{
						int counter = 0;
						string ln;
						bool isheader = false;
						string desc_of_file = null;
						while ((ln = reader.ReadLine()) != null)
						{
							if (isheader)
							{
								if (ln.Length > 0)
								{
									if (ln[0] == "^"[0])
									{
										isheader = false;
										mi.desc_text.Text = desc_of_file;
									}
									else
									{
										desc_of_file += ln + "\r\n";
									}
								}
								else
								{
									desc_of_file += "\r\n";
								}
							}
							if (ln.Length > 0)
							{
								if (counter == 0 && ln[0] == "^"[0])
								{
									desc_of_file = "";
									isheader = true;
									string[] filters = ln.Substring(1).Split("~");
									foreach (string filterthing in filters)
									{
										if (filterthing != "")
										{
											mi.filter_panel.Children.Add(new TextBox { TextWrapping = TextWrapping.Wrap, Text = filterthing, FontSize = 12 });
											dump_filters.Add(filterthing);
											if (!loaded_filters.Keys.Contains(filterthing))
											{
												modcheckfilter moe = new();
												moe.mwidow = this;
												filterspanel.Children.Add(moe);
												loaded_filters.Add(filterthing, moe);
												moe.filtercount.Text = "(" + moe.debug_count + ")";
												moe.filterbox.Content = filterthing;
											}
											else
											{
												modcheckfilter moe = loaded_filters[filterthing];
												moe.debug_count++;
												moe.filtercount.Text = "(" + moe.debug_count + ")";
											}
										}
									}
								}
							}
							counter++;
						}
						reader.Close();
					}
					mi.filters = dump_filters;
				}
			}
		}
		public void delete_file(string path)
		{
			if (File.Exists(path))
				File.Delete(path);
			load_mods_from_directories();
		}

		public void one_of_our_filtercheckboxes_was_clicked()
		{
			List<string> required_filters = new List<string>();
			foreach (KeyValuePair<string, modcheckfilter> thing in loaded_filters)
			{
				if (thing.Value.filterbox.IsChecked == true)
					required_filters.Add(thing.Key);
			}

			foreach (modinstance mso in loaded_modsui)
			{
				bool passedFilter = true;

				foreach (string filt in required_filters)
					if (!mso.filters.Contains(filt))
						passedFilter = false;

				if (!mso.title.Text.Contains(search_filter.Text))
					passedFilter = false;

				if (passedFilter == true)
					mso.Visibility = Visibility.Visible;
				else
					mso.Visibility = Visibility.Collapsed;	
			}
		}

		private void search_filter_TextChanged(object sender, TextChangedEventArgs e)
		{
			one_of_our_filtercheckboxes_was_clicked();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			load_mods_from_directories();
			debug_text.Text = "Refreshed mods";
		}
		// go to halocustoms dot com
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo("https://www.halocustoms.com/maps/categories/irtv-poke-files.43/") { UseShellExecute = true });
		}

		// find and import mods into local appdata folder
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension 
			dlg.DefaultExt = ".irtv";
			dlg.Filter = "IRTV Files (*.irtv)|*.irtv";
			dlg.Multiselect = true;
			// Display OpenFileDialog by calling ShowDialog method 
			bool? result = dlg.ShowDialog();

			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				int overwritten_files = 0;
				foreach (string filein in dlg.FileNames)
				{
					string fileNameWithExt = System.IO.Path.GetFileName(filein);
					string target_folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IRTV";
					if (!Directory.Exists(target_folder))
						Directory.CreateDirectory(target_folder);
					string destPath = System.IO.Path.Combine(target_folder, fileNameWithExt);
					if (File.Exists(destPath))
					{
						File.Delete(destPath);
						overwritten_files++;
					}
					File.Copy(dlg.FileName, destPath);
				}
				if (dlg.FileNames.Length == 1)
				{
					if (overwritten_files > 0)
					{
						debug_text.Text = "replaced " + dlg.FileNames[0];
					}
					else
					{
						debug_text.Text = "Imported " + dlg.FileNames[0];
					}
				}
				else
				{
					if (overwritten_files > 0)
					{
						debug_text.Text = "Imported " + dlg.FileNames.Length + " mods, " + overwritten_files + " were overwritten";
					}
					else
					{
						debug_text.Text = "Successfully imported " + dlg.FileNames.Length + " mods";
					}
				}

				load_mods_from_directories();
			}
		}

		// refresh tags
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			if (main.SlientHookAndLoad(true))
			{
				debug_text.Text = "Successfully reloaded";
			}
			else
			{
				debug_text.Text = "Can't find HaloInfinite.exe";
			}
		}

		// poke the changes 
		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			if (main.SlientHookAndLoad(false))
			{
				main.PokeChanges();
			}
			else
			{
				debug_text.Text = "Halo hasn't been hooked";
			}
		}

		// clear changes
		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			main.clear_all_pokelists();
			debug_text.Text = "Queue cleared";
			load_mods_from_directories();
		}

		// Can execute
		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		// Minimize
		private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
		}

		// Maximize
		private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MaximizeWindow(this);
		}

		// Restore
		private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.RestoreWindow(this);
		}

		// Close
		private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		// State change
		private void MainWindowStateChangeRaised(object? sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				MainWindowBorder.BorderThickness = new Thickness(8);
				RestoreButton.Visibility = Visibility.Visible;
				MaximizeButton.Visibility = Visibility.Collapsed;
			}
			else
			{
				MainWindowBorder.BorderThickness = new Thickness(0);
				RestoreButton.Visibility = Visibility.Collapsed;
				MaximizeButton.Visibility = Visibility.Visible;
			}
		}
	}
}
