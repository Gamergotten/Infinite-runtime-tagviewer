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
		private long WaypointCoordAddress = -1;
		private int scan_test = 0; // Used to prevent incorrect output
		private long[] AoBCoordsResults;

		private int scan_type = 0; // 0 is player, 1 is AI

		System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


		public TeleportWindow(Mem m)
		{
			StateChanged += MainWindowStateChangeRaised;
			_m = m;
			InitializeComponent();

			dispatcherTimer.Tick += OnTimedEvent;
			dispatcherTimer.Interval = new TimeSpan(20000);
			

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
				
				Address_List.Children.Clear(); // Clear Window
				x_cur.Text = "";
				y_cur.Text = "";
				z_cur.Text = "";
				selected_address.Text = "";

				if (scan_type == 0) // Search for player coordinats
				{
					Status.Text = "Scanning for player coordinates...";
					AoBCoordsResults = (await _m.AoBScan("FF FF FF 7F ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 03 3C ?? ?? 00 ?? FF FF FF 7F", true)).ToArray();
				}
				else if (scan_type == 1) // Search for AI coordinates
				{
					Status.Text = "Scanning for AI coordinates...";
					AoBCoordsResults = (await _m.AoBScan("FF FF FF 7F ?? ?? ?? ?? ?? ?? ?? ?? 01 00 00 00 80 5F FF 03 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? F0 FF 7F 5F ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? F0 FF 7F 5F ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 ?? ?? ?? ?? 00 ?? ?? ?? ?? ??", true)).ToArray();
					
				}

				int add_count = 0;
				
				foreach (var coord in AoBCoordsResults) //Creates address list
				{
					string address_verify = _m.ReadFloat((coord + 0x54).ToString("X")).ToString();

					if (address_verify != "0")
					{
						Button crd = new Button();
						crd.Content = coord.ToString("X");
						Address_List.Children.Add(crd);

						crd.Click += Address_Select_Click;

						add_count++;
					}

					if (address_verify != "0" && add_count == 1)
					{
						selected_address.Text = coord.ToString("X");
						CoordAddress = coord;
					}
				}
				
				TextBox filler_tbx = new TextBox(); // Filler so you can scroll to the bottom
				Address_List.Children.Add(filler_tbx);
				
				if (add_count == 1)
				{
					Status.Text = "Scan Complete: Found " + add_count.ToString() + " Result!";
				}
				
				else if (add_count > 1)
				{
					Status.Text = "Scan Complete: Found " + add_count.ToString() + " Results!";
				}
				scan_test = 0;

			}
			catch (Exception)
			{
				Status.Text = "Scan Failed!";
				scan_test = 1;
			}
		}

		private void Auto_Updater_Toggle(object sender, EventArgs e)
		{
			if (dispatcherTimer.IsEnabled == false)
			{
				dispatcherTimer.Start();

				auto_updater.Header = "Auto Update [ENABLED]";

				Status.Text = "Auto Update Enabled!";
			}
			else if (dispatcherTimer.IsEnabled == true)
			{
				dispatcherTimer.Stop();

				auto_updater.Header = "Auto Update [DISABLED]";

				Status.Text = "Auto Update Disabled!";
			}
		}

		private void Player_Scan_Selected(object sender, EventArgs e)
		{
			if (scan_type == 0)
			{
				Status.Text = "Player Scan Already Enabled!";
			}
			else if (scan_type == 1)
			{
				ai_scan.Header = "AI Scan";
				player_scan.Header = "Player Scan [ENABLED]";

				scan_type = 0;

				Status.Text = "Player Scan Enabled!";
			}
		}

		private void AI_Scan_Selected(object sender, EventArgs e)
		{
			if (scan_type == 0)
			{
				ai_scan.Header = "AI Scan [ENABLED]";
				player_scan.Header = "Player Scan";

				scan_type = 1;

				Status.Text = "AI Scan Enabled!";
			}
			else if (scan_type == 1)
			{
				Status.Text = "AI Scan Already Enabled!";
			}
		}

		private void OnTimedEvent(object sender, EventArgs e)
		{
			try
			{
				CoordAddress = long.Parse(selected_address.Text, System.Globalization.NumberStyles.HexNumber);

				x_cur.Text = _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString();
				y_cur.Text = _m.ReadFloat((CoordAddress + 0x58).ToString("X")).ToString();
				z_cur.Text = _m.ReadFloat((CoordAddress + 0x5C).ToString("X")).ToString();
			}
			catch (Exception)
			{
				x_cur.Text = "";
				y_cur.Text = "";
				z_cur.Text = "";
			}
		}

		private void Address_Select_Click(object sender, RoutedEventArgs e)
		{
			Button? found_add = sender as Button;
			string selection = found_add.Content.ToString();
			selected_address.Text = selection;

			CoordAddress = long.Parse(selection, System.Globalization.NumberStyles.HexNumber);
			x_cur.Text = _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString();
			y_cur.Text = _m.ReadFloat((CoordAddress + 0x58).ToString("X")).ToString();
			z_cur.Text = _m.ReadFloat((CoordAddress + 0x5C).ToString("X")).ToString();

			Status.Text = "Address Selected: " + selection;
		}

		public async Task GetWaypointFromAoB()
		{
			try
			{
				Status.Text = "Scanning for Waypoint!";

				WaypointCoordAddress = -1; // Clear data
				x_des.Text = "";
				y_des.Text = "";
				z_des.Text = "";
				
				WaypointCoordAddress = (await _m.AoBScan("10 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 12 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00", true)).First();

				waypoint_address.Text = WaypointCoordAddress.ToString("X");

				Status.Text = "Found Waypoint Coords!";
				scan_test = 0;
			}
			catch (Exception)
			{
				Status.Text = "Waypint Scan Failed!";
				scan_test = 1;
			}
		}

		private async void Scan_Waypoint(object sender, RoutedEventArgs e)
		{
			await GetWaypointFromAoB();
			
			if (scan_test == 0)
			{
				x_des.Text = _m.ReadFloat((WaypointCoordAddress + 0x4).ToString("X")).ToString();
				y_des.Text = _m.ReadFloat((WaypointCoordAddress + 0x8).ToString("X")).ToString();
				z_des.Text = _m.ReadFloat((WaypointCoordAddress + 0xC).ToString("X")).ToString();
			}

		}

		private void Set_Waypoint(object sender, RoutedEventArgs e)
		{
			if (_m.ReadFloat((WaypointCoordAddress + 0x4).ToString("X")).ToString() != "0" || _m.ReadFloat((WaypointCoordAddress + 0x4).ToString("X")).ToString() != x_des.Text)
			{
				x_des.Text = _m.ReadFloat((WaypointCoordAddress + 0x4).ToString("X")).ToString();
				y_des.Text = _m.ReadFloat((WaypointCoordAddress + 0x8).ToString("X")).ToString();
				z_des.Text = _m.ReadFloat((WaypointCoordAddress + 0xC).ToString("X")).ToString();
			}
			else
			{
				Status.Text = "Failed to set waypoint. Make sure a new one has been marked. If you have done so, rescan.";
			}

		}
		private async void Scan_Coords(object sender, RoutedEventArgs e)
		{

			await GetCoordsFromAoB();

			if (scan_test == 0)
			{
				x_cur.Text = _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString();
				y_cur.Text = _m.ReadFloat((CoordAddress + 0x58).ToString("X")).ToString();
				z_cur.Text = _m.ReadFloat((CoordAddress + 0x5C).ToString("X")).ToString();
			}
			
		}
		private void Update_Coords(object sender, RoutedEventArgs e)
		{

			if (CoordAddress != -1 || _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString() != "0") // A value of 0 occurs when the entity despawns or is invalid
			{
				CoordAddress = long.Parse(selected_address.Text, System.Globalization.NumberStyles.HexNumber);

				x_cur.Text = _m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString();
				y_cur.Text = _m.ReadFloat((CoordAddress + 0x58).ToString("X")).ToString();
				z_cur.Text = _m.ReadFloat((CoordAddress + 0x5C).ToString("X")).ToString();

				Status.Text = "Coords Updated";
			}
			else
			{
				Status.Text = "Coords failed to update, select a different address or rescan.";
			}
		}

		private void Poke_Coords(object sender, RoutedEventArgs e)
		{
			try
			{
				CoordAddress = long.Parse(selected_address.Text, System.Globalization.NumberStyles.HexNumber); // Converts from Hex to Long

				if (x_des.Text == "") // Checks to see if blocks are empty to autofill
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
				if (_m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString() != "0")
				{
					_m.WriteMemory(((CoordAddress + 0x54).ToString("X")), "float", x_des.Text);
					_m.WriteMemory(((CoordAddress + 0x58).ToString("X")), "float", y_des.Text);
					_m.WriteMemory(((CoordAddress + 0x5C).ToString("X")), "float", z_des.Text);

					Status.Text = "Coords Poked";
				}
				else
				{
					Status.Text = "Invalid Address!";
				}
			}
			catch (Exception)
			{
				Status.Text = "Coords Failed to Poke!";
			}
		}

		private void Poke_All_Coords(object sender, RoutedEventArgs e)
		{
			int counter = 0;
			
			if (x_des.Text == "") // Checks to see if blocks are empty to autofill
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
				foreach (var coord in AoBCoordsResults) // Pokes every address in coord list
				{
					CoordAddress = coord;
					if (_m.ReadFloat((CoordAddress + 0x54).ToString("X")).ToString() != "0")
					{
						_m.WriteMemory(((CoordAddress + 0x54).ToString("X")), "float", x_des.Text);
						_m.WriteMemory(((CoordAddress + 0x58).ToString("X")), "float", y_des.Text);
						_m.WriteMemory(((CoordAddress + 0x5C).ToString("X")), "float", z_des.Text);
					}
				}

				Status.Text = "All Valid Coords Poked";
			}
			catch (Exception)
			{
				Status.Text = "Coords Failed to Poke!";
			}
		}

		private void Set_Coords(object sender, RoutedEventArgs e) // List of locations to teleport to
		{
			if (location_selector.Text == "FOB Alpha")
			{
				x_des.Text = "-845.5";
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
		private void Clear_Des_Coords(object sender, RoutedEventArgs e) // Clears desired coords section
		{
			x_des.Text = "";
			y_des.Text = "";
			z_des.Text = "";
		}

	}
}
