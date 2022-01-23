using System;
using System.Windows;
using System.Windows.Controls;
using Memory;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InfiniteRuntimeTagViewer.Interface.Windows
{
	public partial class TeleportWindow
	{
		private readonly Mem _m;
		private long CoordAddress = -1;
		private long[] AoBCoordsResults;

		public TeleportWindow(Mem m)
		{
			StateChanged += MainWindowStateChangeRaised;
			_m = m;
			InitializeComponent();

		}

		#region TitleBar Commands
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

		private void MainWindowStateChangeRaised(object? sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				MainWindowBorder.BorderThickness = new Thickness(8);
				RestoreButton.Visibility = Visibility.Visible;
			}
			else
			{
				MainWindowBorder.BorderThickness = new Thickness(0);
				RestoreButton.Visibility = Visibility.Collapsed;
			}
		}
		#endregion

		public async Task GetCoordsFromAoB()
		{
			try
			{
				CoordAddress = -1;
				Address_List.Children.Clear();
				x_cur.Text = "";
				y_cur.Text = "";
				z_cur.Text = "";
				selected_address.Text = "";
				
				if (player_search.IsChecked == true)
				{
					AoBCoordsResults = (await _m.AoBScan("FF FF FF 7F ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 03 3C ?? ?? 00 ?? FF FF FF 7F", true)).ToArray();
				}
				else if (ai_search.IsChecked == true)
				{
					AoBCoordsResults = (await _m.AoBScan("FF FF FF 7F ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 03 3C", true)).ToArray();
				}
				else if (all_entity_search.IsChecked == true)
				{
					AoBCoordsResults = (await _m.AoBScan("FF FF FF 7F 00 00 00 00 00 00 ?? ?? ?? 00 00 00 ?? ?? FF ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 ?? FF FF FF 7F", true)).ToArray();
				}

				selected_address.Text = AoBCoordsResults[0].ToString("X");
				CoordAddress = AoBCoordsResults[0];
				
				foreach (var coord in AoBCoordsResults)
				{

					TextBox crd = new TextBox();
					crd.Text = coord.ToString("X");
					crd.IsReadOnly = true;
					Address_List.Children.Add(crd);

				}
				TextBox filler_tbx = new TextBox();
				Address_List.Children.Add(filler_tbx);

			}
			catch (Exception)
			{
			}
		}
		private async void Scan_Coords(object sender, RoutedEventArgs e)
		{
			Status.Text = "Scanning...";
			await GetCoordsFromAoB();
			
			try
			{
				x_cur.Text = _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString();
				y_cur.Text = _m.ReadFloat((CoordAddress + 0x58).ToString("X")).ToString();
				z_cur.Text = _m.ReadFloat((CoordAddress + 0x5C).ToString("X")).ToString();

				Status.Text = "Scan Complete: Found " + AoBCoordsResults.Length.ToString() + " Results!";
			}
			catch (Exception)
			{
				Status.Text = "Scan Failed!";
			}
		}
		private void Update_Coords(object sender, RoutedEventArgs e)
		{

			try
			{
				CoordAddress = long.Parse(selected_address.Text, System.Globalization.NumberStyles.HexNumber);

				x_cur.Text = _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString();
				y_cur.Text = _m.ReadFloat((CoordAddress + 0x58).ToString("X")).ToString();
				z_cur.Text = _m.ReadFloat((CoordAddress + 0x5C).ToString("X")).ToString();

				Status.Text = "Coords Updated";
			}
			catch (Exception)
			{
				Status.Text = "Coords Failed to Update!";
			}
		}

		private void Poke_Coords(object sender, RoutedEventArgs e)
		{
			try
			{
				CoordAddress = long.Parse(selected_address.Text, System.Globalization.NumberStyles.HexNumber);

				if (x_des.Text == "")
				{
					x_des.Text = x_cur.Text;
				}
				if (y_des.Text == "")
				{
					y_des.Text = y_cur.Text;
				}
				if (z_des.Text == "")
				{
					z_des.Text = z_cur.Text;
				}

				_m.WriteMemory(((CoordAddress + 0x54).ToString("X")), "float", x_des.Text);
				_m.WriteMemory(((CoordAddress + 0x58).ToString("X")), "float", y_des.Text);
				_m.WriteMemory(((CoordAddress + 0x5C).ToString("X")), "float", z_des.Text);

				Status.Text = "Coords Poked";
			}
			catch (Exception)
			{
				Status.Text = "Coords Failed to Poke!";
			}
		}

		private void Poke_All_Coords(object sender, RoutedEventArgs e)
		{
			if (x_des.Text == "")
			{
				x_des.Text = x_cur.Text;
			}
			if (y_des.Text == "")
			{
				y_des.Text = y_cur.Text;
			}
			if (z_des.Text == "")
			{
				z_des.Text = z_cur.Text;
			}
			
			try
			{
				foreach (var coord in AoBCoordsResults)
				{
					CoordAddress = coord;

					_m.WriteMemory(((CoordAddress + 0x54).ToString("X")), "float", x_des.Text);
					_m.WriteMemory(((CoordAddress + 0x58).ToString("X")), "float", y_des.Text);
					_m.WriteMemory(((CoordAddress + 0x5C).ToString("X")), "float", z_des.Text);
				}

				Status.Text = "All Coords Poked";
			}
			catch (Exception)
			{
				Status.Text = "Coords Failed to Poke!";
			}
		}

		private void Set_Coords(object sender, RoutedEventArgs e)
		{
			if (location_selector.Text == "FOB Alpha")
			{
				x_des.Text = "845.5";
				y_des.Text = "387.73";
				z_des.Text = "157.18";
			}
			else if (location_selector.Text == "FOB Bravo")
			{
				x_des.Text = "-412.48";
				y_des.Text = "331.12";
				z_des.Text = "81.27";
			}
			else if (location_selector.Text == "FOB Charlie")
			{
				x_des.Text = "-640.6";
				y_des.Text = "44.26";
				z_des.Text = "140.58";
			}
			else if (location_selector.Text == "FOB Delta")
			{
				x_des.Text = "-406";
				y_des.Text = "-225.76";
				z_des.Text = "125.48";
			}
			else if (location_selector.Text == "FOB Echo")
			{
				x_des.Text = "-812.68";
				y_des.Text = "-231.12";
				z_des.Text = "131.54";
			}
			else if (location_selector.Text == "FOB Foxtrot")
			{
				x_des.Text = "-855.25";
				y_des.Text = "-540.15";
				z_des.Text = "93.86";
			}
			else if (location_selector.Text == "FOB Golf")
			{
				x_des.Text = "-650.38";
				y_des.Text = "-610.84";
				z_des.Text = "133.5";
			}
			else if (location_selector.Text == "FOB Hotel")
			{
				x_des.Text = "-273.62";
				y_des.Text = "-841.55";
				z_des.Text = "42.24";
			}
			else if (location_selector.Text == "FOB Juliet")
			{
				x_des.Text = "-40.66";
				y_des.Text = "-765.19";
				z_des.Text = "35.93";
			}
			else if (location_selector.Text == "FOB Kilo")
			{
				x_des.Text = "16.25";
				y_des.Text = "-954.85";
				z_des.Text = "80.27";
			}
			else if (location_selector.Text == "FOB Lima")
			{
				x_des.Text = "505.26";
				y_des.Text = "-1020.71";
				z_des.Text = "121.95";
			}
			else if (location_selector.Text == "FOB November")
			{
				x_des.Text = "583.8";
				y_des.Text = "-744.65";
				z_des.Text = "146.77";
			}
		}
		private void Clear_Des_Coords(object sender, RoutedEventArgs e)
		{
			x_des.Text = "";
			y_des.Text = "";
			z_des.Text = "";
		}

	}
}
