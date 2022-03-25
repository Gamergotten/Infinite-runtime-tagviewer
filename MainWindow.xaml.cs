using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InfiniteRuntimeTagViewer.Interface.Controls;
using InfiniteRuntimeTagViewer.Interface.Windows;

using AvalonDock.Layout;

using Memory;
using InfiniteRuntimeTagViewer.Halo;
using System.Xml.Serialization;
using InfiniteRuntimeTagViewer.Halo.TagObjects;
using System.Windows.Media;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Threading;
using System.Collections.ObjectModel;

using InfiniteRuntimeTagViewer.Properties;
using System.ComponentModel;

namespace InfiniteRuntimeTagViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		/* ###### THINGS TO BE FIXED/ADDED (which i will get around to eventually) ######
		
		### BUG FIXES ###
		autoload + dont show unloaded tags + load: cant create new tag ui instances or something
		-- oh i know what happened there, when halo gets unhooked, it wipes the UI without scrubbing the UI references from UItaglist or something

		theres still a ton of opportunites to crash in unlikely scenarios, need to investigate all crashes and create handlers

		### QOL ###
		reload tag button
		optimize tagdata loading by making combobox index a source
		mod poke abort
		-- required tags
		-- can abort

		### FEATURES ###
		randomize tagref option
		single poke doesn't have revert button :(

		### ERROR CATCHING ###

		### TAG STRUCTS ###
		char ' gets turned into the funny unknown char. ex. don't -> dont^t (ok using that character there prompts vs to use unicode mode, no)

		### HASH STUFF (i'll do this next week or sometime) ###
		add tool: hash logger - will read through every loaded tag in the game and log referenced hashes
		add hash database support - so people can convert hashes to known unhashed strings
		add tool: hash guesser - lets users guess ushash strings from unknown hashes
		prolly some more stuff i cant remember how i was gonna do all this

		### STUFF THATS NOT REALLY ON THE LIST ###
		show red border on failed pokes INSIDE tag data tab, and not just in the poke queue

		*/

		public bool
			AutoHookKey,
			AutoLoadKey,
			AutoPokeKey,
			FilterOnlyMappedKey,
			OpacityKey,
			AlwaysOnTopKey;
		public string ProcAsyncBaseAddr = Settings.Default.ProcAsyncBaseAddr;

		public bool done_loading_settings;

		#region poop region

		

		public void ShowPointerDialog(object source, RoutedEventArgs e)
		{
			PointerDialog pointerDialog = new();
			pointerDialog.Show();
			pointerDialog.Focus();
		}

		public void GetGeneralSettingsFromConfig()
		{
			AutoHookKey = Settings.Default.AutoHook;
			AutoLoadKey = Settings.Default.AutoLoad;
			AutoPokeKey = Settings.Default.AutoPoke;
			FilterOnlyMappedKey = Settings.Default.FilterOnlyMapped;
			OpacityKey = Settings.Default.Opacity;
			AlwaysOnTopKey = Settings.Default.AlwaysOnTop;
		}
		public void SetGeneralSettingsFromConfig()
		{
			GetGeneralSettingsFromConfig();
			CbxSearchProcess.IsChecked = AutoHookKey;
			CbxAutoPokeChanges.IsChecked = AutoPokeKey;
			CbxFilterUnloaded.IsChecked = FilterOnlyMappedKey;
			whatdoescbxstandfor.IsChecked = AutoLoadKey;
			CbxOnTop.IsChecked = AlwaysOnTopKey;
			CbxOpacity.IsChecked = OpacityKey;
		}
		public void OnApplyChanges_Click()
		{
			SaveUserChangedSettings();
			Settings.Default.Save();
			SetGeneralSettingsFromConfig();
		}
		public void SaveUserChangedSettings()
		{
			Settings.Default.AutoHook = CbxSearchProcess.IsChecked;
			Settings.Default.AutoLoad = whatdoescbxstandfor.IsChecked;
			Settings.Default.AutoPoke = CbxAutoPokeChanges.IsChecked;
			Settings.Default.FilterOnlyMapped = CbxFilterUnloaded.IsChecked;
			Settings.Default.AlwaysOnTop = CbxOnTop.IsChecked;
			Settings.Default.Opacity = CbxOpacity.IsChecked;

		}


		#endregion

		public delegate void HookAndLoadDelagate();
		public delegate void LoadTagsDelagate();
		private readonly System.Timers.Timer _t;
		public Mem M = new();
		

		//Offsets
		public string
								// Hard-Coded Addresses
								//HookProcessAsyncBaseAddr = "HaloInfinite.exe+0x41A2920",
								HookProcessAsyncBaseAddr,// Tag_List_Function  (did not change for TU9)
								ScanMemAOBBaseAddr = "HaloInfinite.exe+0x360DB10",           // Tag_List_Str       (did not change for TU9)

								// AOB's to scan.
								AOBScanTagStr = "74 61 67 20 69 6E 73 74 61 6E 63 65 73"; // Tag_List_Backup Str to find
		private readonly long
								AOBScanStartAddr = Convert.ToInt64("0000010000000000", 16),
								AOBScanEndAddr = Convert.ToInt64("000003ffffffffff", 16);


		public MainWindow()
		{
			UpdateAddress();
			InitializeComponent();
			//GetAllMethods();
			StateChanged += MainWindowStateChangeRaised;

			_t = new System.Timers.Timer();
			_t.Elapsed += OnTimedEvent;
			_t.Interval = 2000;
			_t.AutoReset = true;

			inhale_tagnames();
			//SettingsControl settings = new();
			SetGeneralSettingsFromConfig();
			done_loading_settings = true;
			//settings.Close();

			add_new_section_to_pokelist("Poke Queue");

		}

		public void UpdateAddress()
		{
			if (Settings.Default.ProcAsyncBaseAddr != "undefined")
			{
				HookProcessAsyncBaseAddr = Settings.Default.ProcAsyncBaseAddr;
			}
			else
			{
				HookProcessAsyncBaseAddr = "HaloInfinite.exe+0x41A2920";
			}
		}

		private async Task HookProcessAsync()
		{
			bool reset = processSelector.hookProcess(M);
			if (M.pHandle == IntPtr.Zero || processSelector.selected == false || loadedTags == false)
			{
				// Could not find the process
				hook_text.Text = "Cant find HaloInfinite.exe";
				BaseAddress = -1;
				hooked = false;
				loadedTags = false;
				TagsTree.Items.Clear();
			}

			if (!hooked || reset)
			{
				// Get the base address
				UpdateAddress();
				BaseAddress = M.ReadLong(HookProcessAsyncBaseAddr);
				string validtest = M.ReadString(BaseAddress.ToString("X"));
				//System.Diagnostics.Debug.WriteLine(M.ReadLong("HaloInfinite .exe+0x3D13E38")); // this is the wrong address lol
				if (validtest == "tag instances")
				{
					hook_text.Text = "Process Hooked: " + M.theProc.Id;
					hooked = true;
				}
				else
				{
					hook_text.Text = "Offset failed, scanning...";
					await ScanMem();
				}
			}
		}



		public async void HookAndLoad()
		{
			await HookProcessAsync();
			if (BaseAddress != -1 && BaseAddress != 0)
			{
				await LoadTagsMem(false);

				
				if (hooked == true)
				{
					Searchbox_TextChanged(null, null);

					System.Diagnostics.Debugger.Log(0, "DBGTIMING", "Done loading tags");

				}
			}
		}

		// AUTO MOD LOADER STUFF
		static int min_tags_changed_for_update = 750;
		static int min_tags_changed_for_interupt_update = 150;

		public int tag_count_last_update = 0;
		public int current_tag_count = 0;

		public bool is_waiting;

		private async void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(async () =>
			{
				if (hooked) // wait till hooked so, the user can decide when to turn this on without it annoyingly firing off
				{
					if (!is_waiting)
					{
						await HookProcessAsync();
						if (BaseAddress > 0)
						{
							int real_tag_count = M.ReadInt((BaseAddress + 0x70).ToString("X"));
							current_tag_count = real_tag_count;
							extra_tag_text.Text = "found (" + real_tag_count + " tags)";
							if (current_tag_count < tag_count_last_update - min_tags_changed_for_update || current_tag_count > tag_count_last_update + min_tags_changed_for_update)
							{
								is_waiting = true;
								extra_tag_text.Text = "one sec (" + real_tag_count + " tags)";
								while (is_waiting)
								{
									await Task.Delay(12000);

									int real_tag_count_again = M.ReadInt((BaseAddress + 0x70).ToString("X"));
									if (!(real_tag_count_again < current_tag_count - min_tags_changed_for_interupt_update) && !(real_tag_count_again > current_tag_count + min_tags_changed_for_interupt_update))
									{
										is_waiting = false;
										extra_tag_text.Text = "reloading (" + real_tag_count_again + " tags)";
										await LoadTagsMem(false);
										if (whatdoescbxstandfor.IsChecked)
										{
											PokeChanges();
										}
										tag_count_last_update = real_tag_count_again;
									}
									else
									{
										current_tag_count = real_tag_count_again;
										extra_tag_text.Text = "Awaiting (" + real_tag_count_again + " tags)";
									}
								}
							}
						}
					}
				}
			}));
		}

		public bool loadedTags = false;
		public bool hooked = false;

		public async Task ScanMem()
		{
			// FALLBACK ADDRESS POINTER (which is literally useless)
			// However, it is faster than scanning memory and is used as a fast reference ptr to load quicker.
			BaseAddress = M.ReadLong(ScanMemAOBBaseAddr);
			string validtest = M.ReadString(BaseAddress.ToString("X"));

			if (validtest == "tag instances")
			{
				hook_text.Text = "Process Hooked: " + M.theProc.Id;
				hooked = true;
			}
			else
			{
				hook_text.Text = "Offset failed, scanning...";
				try
				{
					long? aobScan = (await M.AoBScan(AOBScanStartAddr, AOBScanEndAddr, AOBScanTagStr, true))
						.First(); // "tag instances"

					// Failed to find base tag address
					if (aobScan == null || aobScan == 0)
					{
						BaseAddress = -1;
						loadedTags = false;
						hook_text.Text = "Failed to locate base tag address";
					}
					else
					{
						BaseAddress = aobScan.Value;
						hook_text.Text = "Process Hooked: " + M.theProc.Id + " (AOB)";
						hooked = true;
					}
				}
				catch (Exception)
				{
					hook_text.Text = "Cant find HaloInfinite.exe";
				}
			}
		}

		#region EventHandlers
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			num_of_user_added_lists++;
			add_new_section_to_pokelist("Poke Queue(" + num_of_user_added_lists + ")");
		}

		#region Window Styling

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
		#endregion


		// Search filter
		private void Searchbox_TextChanged(object? sender, TextChangedEventArgs? e)
		{
			//string[] supportedTags = Halo.TagObjects.TagLayouts.Tags.Keys.ToArray();
			string search = Searchbox.Text;
			foreach (TreeViewItem? tv in TagsTree.Items)
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

		#region MenuCommands
		public void ClickExit(object sender, RoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		public void OpenTeleportMenu(object sender, RoutedEventArgs e)
		{
			TeleportWindow tp_win = new(M);
			tp_win.Show();
		}

		public ProcessSelector GetProcessSelector()
		{
			return processSelector;
		}

		public bool specific;

		public void UnloadTags(object sender, RoutedEventArgs e)
		{
			TagsTree.Items.Clear();
			loadedTags = false;
		}

		//Commented out because mainly this has no function right now.

		//public void GetAllMethods()
		//{
		//	Type myType = (typeof(MainWindow));
		//	// Get the public methods.
		//	MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		//	Console.WriteLine("\nThe number of public methods is {0}.", myArrayMethodInfo.Length);
		//	// Add all public methods to menu.
		//	DisplayMethodInfo(myArrayMethodInfo);
		//	// Add all non-public methods to array.
		//	MethodInfo[] myArrayMethodInfo1 = myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		//	// Add all non-public methods to menu.
		//	DisplayMethodInfo(myArrayMethodInfo1);
		//}


		//public void DisplayMethodInfo(MethodInfo[] myArrayMethodInfo)
		//{
		//	// Display information for all methods.
		//	for (int i = 0; i < myArrayMethodInfo.Length; i++)
		//	{
		//		MethodInfo myMethodInfo = (MethodInfo) myArrayMethodInfo[i];
		//		//Console.WriteLine("\nThe name of the method is {0}.", myMethodInfo.Name);
		//		MenuItem methods = new MenuItem();
		//		MenuItem methodToAdd = (MenuItem) DebugMenu.Items[1];
		//		methods.Header = myMethodInfo.Name;
		//		methods.Click += CallMethod;
		//		methodToAdd.Items.Add(methods);
		//	}
		//}

		//public void CallMethod(object sender, RoutedEventArgs e)
		//{
		//	//Code that will call the specified method.
		//	MenuItem? MI = sender as MenuItem;
		//	if (MI != null)
		//	{
		//		try
		//		{
		//			Type mainType = (typeof(MainWindow));
		//			string? clickedMethod = MI.Header.ToString();
		//			System.Diagnostics.Debug.WriteLine(clickedMethod);
		//			MethodInfo? method = mainType.GetMethod(clickedMethod);
		//			if (method != null)
		//			{
		//				int paramCount = method.GetParameters().Length;
		//				method.Invoke(this, null);
		//			}
		//		}
		//		catch (Exception)
		//		{
		//			System.Diagnostics.Debug.WriteLine("Invalid parameter count. Consider calling a method with no parameters.");
		//		}
		//	}
		//}

		#endregion

		// open mods window
		public ModWindow? mwidow;
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (mwidow == null)
				{
					mwidow = new ModWindow();
					mwidow.Show();
					mwidow.Focus();
					mwidow.main = this;
					mwidow.load_mods_from_directories();
				}
				else
				{
					mwidow.Show();
					mwidow.Focus();
				}
				if (CbxOpacity.IsChecked)
				{
					mwidow.Opacity = 0.90;
				}
				else
				{
					mwidow.Opacity = 1;
				}
			}
			catch (System.InvalidOperationException)
			{
				mwidow = null;
				MenuItem_Click(sender, e);
			}
		}


		// REVERT POKE STUFF
		private void REVERT_ALL_BUTTON(object sender, RoutedEventArgs e)
		{
			int fails = 0;
			int pokes = 0;
			for (int q = 0; q < Pokelistlist.Count; q++)
			{
				KeyValuePair<int, int> kv = revertlist(Pokelistlist.ElementAt(q).Key);
				fails += kv.Value;
				pokes += kv.Key;
			}
			if (fails < 1)
			{
				poke_text.Text = pokes + " changes reverted!";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = pokes + " changes reverted!";
				}
			}
			else
			{
				poke_text.Text = pokes + " reverted, " + fails + " failed";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = pokes + " reverted, " + fails + " failed";
				}
			}

			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
		}
		private void REVERT_SINGLE_BUTTON(object sender, RoutedEventArgs e)
		{
			KeyValuePair<int, int> kv = revertlist(current_pokelist);
			int fails = kv.Value;
			int pokes = kv.Key;
			if (fails < 1)
			{
				poke_text.Text = pokes + " changes reverted!";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = pokes + " changes reverted!";
				}
			}
			else
			{
				poke_text.Text = pokes + " reverted, " + fails + " failed";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = pokes + " reverted, " + fails + " failed";
				}
			}

			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
		}

		private void BtnREMOVEQueueSingle_Click(object sender, RoutedEventArgs e)
		{
			clear_pokes_list(current_pokelist);
			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
			if (mwidow != null)
			{
				mwidow.test_changes.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
			}
			Pokelistlist.Remove(current_pokelist);
			PokeList_Combobox.Items.Remove(PokeList_Combobox.SelectedItem);
			if (PokeList_Combobox.Items.Count > 0)
				PokeList_Combobox.SelectedIndex = 0;

			poke_text.Text = "Poke List Removed";
		}


		private void DockManager_DocumentClosing(object sender, AvalonDock.DocumentClosingEventArgs e)
		{
			// On tag window closing.
			UpdateLayout();

			GC.Collect(3, GCCollectionMode.Forced);
		}

		private void BtnShowHideQueue_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Controls.Button? btn = (System.Windows.Controls.Button) sender;

			queuelist.Visibility = queuelist.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
			btn.Content =
				queuelist.Visibility == Visibility.Visible
				? "Hide Queue"
				: "Show Queue";
		}

		private void BtnClearQueueSingle_Click(object sender, RoutedEventArgs e)
		{
			clear_pokes_list(current_pokelist);
			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
			if (mwidow != null)
			{
				mwidow.test_changes.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
			}
			poke_text.Text = "Poke List Cleared";
		}

		private void BtnClearQueue_Click(object sender, RoutedEventArgs e)
		{
			clear_all_pokelists();
		}

		// POKE OUR CHANGES LETSGOOOO
		private void BtnPokeChanges_Click(object sender, RoutedEventArgs e)
		{
			PokeChanges();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			KeyValuePair<int, int> kv = pokelist(current_pokelist);
			poke_text.Text = kv.Key + " poked, " + kv.Value + " failed";
			if (mwidow != null)
			{
				mwidow.debug_text.Text = kv.Key + " poked, " + kv.Value + " failed";
			}
		}

		private void Open_pokes(object sender, RoutedEventArgs e)
		{
			if (!loadedTags)
			{
				HookAndLoad();
			}
			// Create OpenFileDialog 
			Microsoft.Win32.OpenFileDialog dlg = new()
			{

				// Set filter for file extension and default file extension 
				DefaultExt = ".irtv",
				Filter = "IRTV Files (*.irtv)|*.irtv"
			};

			// Display OpenFileDialog by calling ShowDialog method 
			bool? result = dlg.ShowDialog();

			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				string fullFileName = dlg.FileName;
				string fileNameWithExt = Path.GetFileName(fullFileName);
				add_new_section_to_pokelist(fileNameWithExt);

				recieve_file_to_inhalo_pokes(dlg.FileName);
				string target_folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IRTV";
				if (!Directory.Exists(target_folder))
					Directory.CreateDirectory(target_folder);
				string destPath = Path.Combine(target_folder, fileNameWithExt);
				if (File.Exists(destPath))
					File.Delete(destPath);
				File.Copy(dlg.FileName, destPath);
			}
		}

		private void Save_pokes(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog? sfd = new()
			{
				Filter = "IRTV Files (*.irtv)|*.irtv|All files (*.*)|*.*",
				// Set other options depending on your needs ...
			};
			if (sfd.ShowDialog() == true)
			{


				string filename = sfd.FileName;
				// save the file
				//File.WriteAllText(filename, contents);

				//KeyValuePair<string, KeyValuePair<string, string>>
				string big_ol_poke_dump = "";
				foreach (KeyValuePair<string, KeyValuePair<string, string>> k in Pokelistlist[current_pokelist].Pokelist)
				{
					big_ol_poke_dump += k.Key + ";" + k.Value.Key + ";" + k.Value.Value + "\r\n";
				}
				Savewindow sw = new();
				sw.Show();
				sw.main = this;
				sw.ill_take_it_from_here_mainwindow(filename, big_ol_poke_dump);

				poke_text.Text = Pokelistlist[current_pokelist].Pokelist.Count + " Pokes Saved!";
			}
		}

		private void PokeList_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem? comboBoxItem = PokeList_Combobox.SelectedItem as ComboBoxItem;
			if (comboBoxItem != null)
			{
				load_a_pokelist(comboBoxItem.Content.ToString());
			}

		}

		private void Select_Tag_click(object sender, RoutedEventArgs e)
		{
			TreeViewItem? item = sender as TreeViewItem;
			CreateTagEditorTabByTagIndex(item.Tag.ToString());
		}

		private void CheckBoxProcessCheck(object sender, RoutedEventArgs e)
		{
			_t.Enabled = CbxSearchProcess.IsChecked;
			if (done_loading_settings)
			{
				OnApplyChanges_Click();
			}
		}

		private void UpdateOptionsFromSettings(object sender, RoutedEventArgs e)
		{
			if (done_loading_settings)
				OnApplyChanges_Click();
		}
		private void UpdateOption_for_hiding_unloaded(object sender, RoutedEventArgs e)
		{
			if (done_loading_settings)
			{
				OnApplyChanges_Click();
				if (loadedTags == true)
				{
					HookAndLoad();
				}
			}
		}


		// load tags from Mem
		public void BtnReLoadTags_Click(object sender, RoutedEventArgs e)
		{
			TagsTree.Items.Clear();
			groups_headers.Clear();
			tags_headers.Clear();
			HookAndLoad();
		}
		private void BtnLoadTags_Click(object sender, RoutedEventArgs e)
		{
			HookAndLoad();
			Reload_Button.IsEnabled = true;
		}

		private void Window_Deactivated(object sender, EventArgs e)
		{
			Window window = (Window) sender;
			if (CbxOnTop.IsChecked == true)
			{
				window.Topmost = true;
			}
			else
			{
				window.Topmost = false;
			}
		}


		private void Ppacity(object sender, RoutedEventArgs e)
		{
			UpdateOptionsFromSettings(sender, e);
			if (CbxOpacity.IsChecked)
			{
				window.Opacity = 0.90;
				if (mwidow != null)
				{
					mwidow.Opacity = 0.90;
				}
			}
			else
			{
				window.Opacity = 1;
				if (mwidow != null)
				{
					mwidow.Opacity = 1;
				}
			}



		}

		#endregion
		private long BaseAddress = -1;
		private int TagCount = -1;

		#region TagLoading
		public Dictionary<string, TagStruct> TagsList { get; set; } = new(); // and now we can convert it back because we just sort it elsewhere
		public SortedDictionary<string, GroupTagStruct> TagGroups { get; set; } = new();

		//public ObservableCollection<GroupTagStruct> TagGroups { get; set; } = new();



		private bool is_checked;
		public async Task LoadTagsMem(bool is_silent)
		{
			is_checked = CbxFilterUnloaded.IsChecked;
			await Task.Run(() =>
			{

				if (TagCount != -1)
				{
					TagCount = -1;
					TagGroups.Clear();
					TagsList.Clear();
				}
				TagCount = M.ReadInt((BaseAddress + 0x6C).ToString("X"));
				long tagsStart = M.ReadLong((BaseAddress + 0x78).ToString("X"));

				// each tag is 52 bytes long // was it 52 or was it 0x52? whatever
				// 0x0 datnum 4bytes
				// 0x4 ObjectID 4bytes
				// 0x8 Tag_group Pointer 8bytes
				// 0x10 Tag_data Pointer 8bytes
				// 0x18 Tag_type_desc Pointer 8bytes

				TagsList = new Dictionary<string, TagStruct>();
				for (int tagIndex = 0; tagIndex < TagCount; tagIndex++)
				{
					TagStruct currentTag = new();
					long tagAddress = tagsStart + (tagIndex * 52);

					byte[] test1 = M.ReadBytes(tagAddress.ToString("X"), 4);
					try
					{
						currentTag.Datnum = BitConverter.ToString(test1).Replace("-", string.Empty);
						loadedTags = false;
					}
					catch (System.ArgumentNullException)
					{
						hooked = false;
						return;
					}
					byte[] test = (M.ReadBytes((tagAddress + 4).ToString("X"), 4));

					// = String.Concat(bytes.Where(c => !Char.IsWhiteSpace(c)));
					currentTag.ObjectId = BitConverter.ToString(test).Replace("-", string.Empty);
					currentTag.TagGroup = read_tag_group(M.ReadLong((tagAddress + 0x8).ToString("X")));
					currentTag.TagData = M.ReadLong((tagAddress + 0x10).ToString("X"));
					currentTag.TagFullName = convert_ID_to_tag_name(currentTag.ObjectId).Trim();
					currentTag.TagFile = currentTag.TagFullName.Split('\\').Last().Trim();

					if (is_checked)
					{
						byte[] b = M.ReadBytes((currentTag.TagData + 12).ToString("X"), 4);
						if (b != null)
						{
							string checked_datnum = BitConverter.ToString(b).Replace("-", string.Empty);
							if (checked_datnum != currentTag.Datnum)
							{
								currentTag.unloaded = true;
							}
						}
						else
						{
							currentTag.unloaded = true;
						}
					}
					// do the tag definitition
					if (!TagsList.ContainsKey(currentTag.ObjectId))
					{
						TagsList.Add(currentTag.ObjectId, currentTag);
					}
				}
			});
			if (!is_silent)
				await Loadtags();

		}
		//public ObservableCollection<GroupTagStruct>;

		public async Task Loadtags()
		{
			

			Dictionary<string, TreeViewItem> groups_headers_diff = new();

			await Task.Run(async () =>
			{

			// cycle through and evaluate against diff

			// act accordingly

			// save

			// TagsTree
			loadedTags = true;
			for (int i = 0; i < TagGroups.Count; i++) // per group
			{
				KeyValuePair<string, GroupTagStruct> goop = TagGroups.ElementAt(i);

					//ObservableCollection<GroupTagStruct> tagGroups = new(TagGroups.Values);

					if (groups_headers.Keys.Contains(goop.Key)) // is included in group_headers
					{

						TreeViewItem t = groups_headers[goop.Key];
						groups_headers_diff.Add(goop.Key, t);
						groups_headers.Remove(goop.Key);

						GroupTagStruct displayGroup = goop.Value;
						displayGroup.TagCategory = t;
						TagGroups[goop.Key] = displayGroup;

					}
					else
					{
						GroupTagStruct displayGroup = goop.Value;
						Dispatcher.Invoke(new Action(() =>
						{ 
						TreeViewItem sortheader = new()
						{
							Header = displayGroup.TagGroupName + " (" + displayGroup.TagGroupDesc + ")",
							ToolTip = new TextBlock { Foreground = Brushes.Black, Text = displayGroup.TagGroupDefinitition }
						};
						displayGroup.TagCategory = sortheader;
						TagGroups[goop.Key] = displayGroup;

						TagsTree.Items.Add(sortheader); //The tree view in the UI

								groups_headers_diff.Add(goop.Key, sortheader);


					}));



				}

			}


				Dispatcher.Invoke(new Action(async () =>
				{
					foreach (KeyValuePair<string, TreeViewItem> poop in groups_headers) // per group
					{
						if (poop.Value != null)
						{
							TagsTree.Items.Remove(poop.Value);
						}
					}
					groups_headers = groups_headers_diff;
				}));


			Dictionary<string, TreeViewItem> tags_headers_diff = new();

				int iteration = 0;
			foreach (KeyValuePair<string, TagStruct> curr_tag in TagsList.OrderBy(key => key.Value.TagFullName)) // per tag
			{
					iteration += 1;
				if (!curr_tag.Value.unloaded)
				{
				Dispatcher.Invoke(new Action(() =>
				{
					if (tags_headers.Keys.Contains(curr_tag.Key)) // is included in tag_headers UI
					{
						TreeViewItem t = tags_headers[curr_tag.Key];
						t.Tag = curr_tag.Key;
						tags_headers_diff.Add(curr_tag.Key, t);
						tags_headers.Remove(curr_tag.Key);
					}
					else // tag isnt in UI
					{
						TreeViewItem t = new();
						TagStruct tag = curr_tag.Value;
						TagGroups.TryGetValue(tag.TagGroup, out GroupTagStruct? dictTagGroup);

						t.Header = "(" + tag.Datnum + ") " + convert_ID_to_tag_name(tag.ObjectId);

						t.Tag = curr_tag.Key; // our index to our tag

						t.Selected += Select_Tag_click;


						if (dictTagGroup != null && dictTagGroup.TagCategory != null)
						{
							dictTagGroup.TagCategory.Items.Add(t);
						}

						tags_headers_diff.Add(curr_tag.Key, t);

					}
					
					}));
						if (iteration > 200)
						{
							Thread.Sleep(1);
							iteration = 0;
						}
					}
				}
			foreach (KeyValuePair<string, TreeViewItem> poop in tags_headers) // per tag remove
			{
				if (poop.Value != null)
				{
						Dispatcher.Invoke(new Action(() =>
						{
							TreeViewItem ownber = poop.Value.Parent as TreeViewItem;
							ownber.Items.Remove(poop.Value);
						}));
				}
			}
			tags_headers = tags_headers_diff;


			if (TagsTree.Items.Count < 1)
			{
				loadedTags = false;
			}
				Dispatcher.Invoke(new Action(() =>
				{
					hook_text.Text = "Loaded Tags";
					//Sort the tags tree alphabetically.
					TagsTree.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
				}));
			});
		}

		#endregion

		// instead of using the other method i made a new one because the last one yucky,
		public bool SlientHookAndLoad(bool load_tags_too)
		{
			_ = HookProcessAsync();
			if (BaseAddress != -1 && BaseAddress != 0)
			{
				if (load_tags_too)
				{
					TagsTree.Items.Clear();
					groups_headers.Clear();
					tags_headers.Clear();
					LoadTagsMem(true);

					if (hooked == true)
					{
						Searchbox_TextChanged(null, null);
					}
				}
				return true;
			}
			return false;
		}
		
		public string? read_tag_group(long tagGroupAddress)
		{
			try
			{
				string key = ReverseString(M.ReadString((tagGroupAddress + 0xC).ToString("X"), "", 8).Substring(0, 4));
				if (!TagGroups.ContainsKey(key))
				{
					GroupTagStruct currentGroup = new()
					{
						TagGroupDesc = M.ReadString((tagGroupAddress).ToString("X") + ",0x0"),
						TagGroupName = key,
						TagGroupDefinitition = M.ReadString((tagGroupAddress + 0x20).ToString("X") + ",0x0,0x0"),
						TagExtraType = M.ReadString((tagGroupAddress + 0x2C).ToString("X"), "", 12)
					};

					long testAddress = M.ReadLong((tagGroupAddress + 0x48).ToString("X"));
					if (testAddress != 0)
					{
						currentGroup.TagExtraName = M.ReadString((testAddress).ToString("X"));
					}

					// Doing the UI here so we dont have to literally reconstruct the elements elsewhere // lol // xd how'd that work out for you
					//TreeViewItem sortheader = new TreeViewItem();
					//sortheader.Header = ReverseString(current_group.tag_group_name.Substring(0, 4)) + " (" + current_group.tag_group_desc + ")";
					//sortheader.ToolTip = current_group.tag_group_definitition;
					//TagsTree.Items.Add(sortheader);
					//current_group.tag_category = sortheader;

					TagGroups.Add(key, currentGroup);
				}

				return key;
			}
			catch (Exception)
			{
				return null;
			}
		}
		// as far as im aware this is still running on the main thread :frown:

		// group 4chars, group instance
		// eg. weap, { system.whatever.balls }
		public Dictionary<string, TreeViewItem> groups_headers = new();
		public Dictionary<string, TreeViewItem> tags_headers = new();
		public ObservableCollection<string> second_level = new();

		

		public Dictionary<string, string> InhaledTagnames = new();

		public void inhale_tagnames()
		{
			string filename = Directory.GetCurrentDirectory() + @"\files\tagnames.txt";
			IEnumerable<string>? lines = System.IO.File.ReadLines(filename);
			foreach (string? line in lines)
			{
				string[] hexString = line.Split(" : ");
				if (!InhaledTagnames.ContainsKey(hexString[0]))
				{
					InhaledTagnames.Add(hexString[0], hexString[1]);
				}
			}
		}

		public string convert_ID_to_tag_name(string value)
		{
			_ = InhaledTagnames.TryGetValue(value, value: out string? potentialName);

			return potentialName ??= "ObjectID: " + value;
		}

		public static string ReverseString(string myStr)
		{
			char[] myArr = myStr.ToCharArray();
			Array.Reverse(myArr);
			return new string(myArr);
		}

		public void CreateTagEditorTabByTagIndex(string tagID)
		{
			TagStruct? tag = TagsList[tagID];
			string? tagFull = "(" + tag.Datnum + ") " + convert_ID_to_tag_name(tag.ObjectId);
			string tagName = tagFull.Split('\\').Last();

			// Find the existing layout document ( draggable panel item )
			if (dockManager.Layout.Descendents().OfType<LayoutDocument>().Any())
			{
				LayoutDocument? dockSearch = dockManager.Layout.Descendents()
					.OfType<LayoutDocument>()
					.FirstOrDefault(a => a.ContentId == tagFull);

				// Check if we found the tag
				if (dockSearch != null)
				{
					// Set the tag as active. What does this even do, if the statement is true, you don't need to set it to true again...
					if (dockSearch.IsActive)
					{
						dockSearch.IsActive = true;
					}

					// Set the tag as the active tab
					if (dockSearch.Parent is LayoutDocumentPane ldp)
					{
						for (int x = 0; x < ldp.Children.Count; x++)
						{
							LayoutContent dlp = ldp.Children[x];

							if (dlp == dockSearch)
							{
								bool? found = true;
								ldp.SelectedContentIndex = x;
							}
							else
							{
								bool? found = false; // used for debugging. Debugging what exactly? lol
							}
						}
					}

					return;
				}
			}

			// Create the tag editor.
			TagEditorControl? tagEditor = new TagEditorControl(this);
			tagEditor.Inhale_tag(tagID);

			// Create the layout document for docking.
			LayoutDocument doc = tagEditor.LayoutDocument = new LayoutDocument();
			doc.Title = tagName;
			doc.IsActive = true;
			doc.Content = tagEditor;
			doc.ContentId = tagFull;

			dockLayoutDocPane.Children.Add(doc);
			dockLayoutRoot.ActiveContent = doc;
		}


		// list of changes to ammend to the memory when we phit the poke button
		// i think it goes: address, type, value
		// address instructions | value type | value value

		public Dictionary<string, Poke_queue> Pokelistlist = new();

		public class Poke_queue
		{
			public Dictionary<string, KeyValuePair<string, string>> Pokelist =new();
			public Dictionary<string, KeyValuePair<string, string>>? revertlist = new();
		} 

		public string current_pokelist = "";

		public void add_new_section_to_pokelist(string newname)
		{
			if (!Pokelistlist.ContainsKey(newname))
			{
				ComboBoxItem? comboBoxItem = new() { Content = newname };
				PokeList_Combobox.Items.Add(comboBoxItem);
				Pokelistlist[newname] = new();
				PokeList_Combobox.SelectedIndex = PokeList_Combobox.Items.Count - 1;
			}
			current_pokelist = newname;
		}
		
		public void load_a_pokelist(string queuename)
		{
			if (Pokelistlist.ContainsKey(queuename))
			{
				current_pokelist = queuename;
				if (queuename == "Poke Queue")
				{
					remove_from_quee_button.IsEnabled = false;
				}
				else
				{
					remove_from_quee_button.IsEnabled = true;
				}
				changes_panel.Children.Clear();
				UIpokelist.Clear();

				for (int i = 0; i< Pokelistlist[queuename].Pokelist.Count; i++)
				{
					KeyValuePair<string, KeyValuePair<string, string>> peep_this_one = Pokelistlist[queuename].Pokelist.ElementAt(i);
					string instuctions = peep_this_one.Key;
					string value_type = peep_this_one.Value.Key;
					string value_itself = peep_this_one.Value.Value;
					string revert = "none";
					if (Pokelistlist[queuename].revertlist.ContainsKey(instuctions))
					{
						revert = Pokelistlist[queuename].revertlist[instuctions].Value;
					}

					if (UIpokelist.ContainsKey(instuctions))
					{
						TagChangesBlock updateElement = UIpokelist[instuctions];
						updateElement.address.Text = instuctions;
						updateElement.sig_address_path = instuctions;
						updateElement.type.Text = value_type;
						updateElement.value.Text = value_itself;
						//updateElement.tagSource.Text = def.TagStruct.TagFile + " + " + def.GetTagOffset();
						string dont_Be_null = convert_ID_to_tag_name(instuctions.Split(":").FirstOrDefault());
						updateElement.tagSource.Text = dont_Be_null;
						updateElement.bordercolor.BorderBrush = new SolidColorBrush(Colors.Yellow);
						updateElement.revert.Text = revert;
					}
					else
					{
						TagChangesBlock newBlock = new()
						{
							address = { Text = instuctions },
							type = { Text = value_type },
							value = { Text = value_itself },
						};
						string dont_Be_null = convert_ID_to_tag_name(instuctions.Split(":").FirstOrDefault());
						newBlock.tagSource.Text = dont_Be_null;
						newBlock.sig_address_path = instuctions;
						newBlock.revert.Text = revert;
						newBlock.main = this;
						changes_panel.Children.Add(newBlock);
						UIpokelist.Add(instuctions, newBlock);
					}
				}

			}
		}

		// to keep track of the UI elements we're gonna use a dictionary, will probably be better
		public Dictionary<string, TagChangesBlock> UIpokelist = new(); // i *think* we can just leave this as is

		
		public void recieve_file_to_inhalo_pokes(string filename)
		{
			int prev = 0;
			int fails = 0;
			// Open document 

			add_new_section_to_pokelist(Path.GetFileName(filename));

			using (StreamReader inputFile = new StreamReader(filename))
			{
				string line;
				while ((line = inputFile.ReadLine()) != null)
				{
					string[] parts = line.Split(";");
					if (parts.Length == 3)
					{
						prev++;
						AddPokeChange(new TagEditorDefinition { OffsetOverride = parts[0], MemoryType = parts[1], }, parts[2]);
					}
				}
			}
			// nothing could cause an issue here // was that sarcastic lol
			if (fails < 1)
			{
				poke_text.Text = prev + " Loaded!";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = prev + " Changes Loaded!";
				}
			}
			else
			{
				poke_text.Text = prev + " Loaded, " + fails + " Failed";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = prev + " Changes Loaded, " + fails + " Changes Failed";
				}
			}
		}

		public void AddPokeChange(TagEditorDefinition def, string value)
		{
			// Hmm we need to change this so we either update or add a new UI element

			//used things
			// offset override
			// memory type
			// value
			// tagname
			Pokelistlist[current_pokelist].Pokelist[def.OffsetOverride] = new KeyValuePair<string, string>(def.MemoryType, value);

			// there we go, now we aren't touching the pokelist code
			if (UIpokelist.ContainsKey(def.OffsetOverride))
			{
				TagChangesBlock updateElement = UIpokelist[def.OffsetOverride];
				updateElement.address.Text = def.OffsetOverride;
				updateElement.sig_address_path = def.OffsetOverride;
				updateElement.type.Text = def.MemoryType;
				updateElement.value.Text = value;
				//updateElement.tagSource.Text = def.TagStruct.TagFile + " + " + def.GetTagOffset();
				string dont_Be_null = convert_ID_to_tag_name(def.OffsetOverride.Split(":").FirstOrDefault());
				updateElement.tagSource.Text = dont_Be_null;
				updateElement.bordercolor.BorderBrush = new SolidColorBrush(Colors.Yellow); 
			}
			else
			{
				TagChangesBlock newBlock = new()
				{
					address = { Text = def.OffsetOverride },
					type = { Text = def.MemoryType },
					value = { Text = value },
				};
				string dont_Be_null = convert_ID_to_tag_name(def.OffsetOverride.Split(":").FirstOrDefault());
				newBlock.tagSource.Text = dont_Be_null;
				newBlock.sig_address_path = def.OffsetOverride;
				newBlock.main = this;
				changes_panel.Children.Add(newBlock);
				UIpokelist.Add(def.OffsetOverride, newBlock);
			}

			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
			if (mwidow != null)
			{
				mwidow.test_changes.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
			}

			if (CbxAutoPokeChanges.IsChecked)
			{
				bool passed = pokesingle(def.OffsetOverride, def.MemoryType, value, current_pokelist);
				if (!passed)
				{
					UIpokelist[def.OffsetOverride].bordercolor.BorderBrush = new SolidColorBrush(Colors.Red);
				}
				else
				{
					UIpokelist[def.OffsetOverride].bordercolor.BorderBrush = null;
				}
			}
		}

		// need this to read tagref blocks - because we only get a datnum to figure out the name with
		// so we find what else has the same datnum and then run the other method to get name based off of ID
		public string get_tagid_by_datnum(string datnum)
		{
			foreach (KeyValuePair<string, TagStruct> t in TagsList)
			{
				if (t.Value.Datnum == datnum)
				{
					return t.Value.ObjectId;
				}
			}

			return "Tag not present(" + datnum + ")";
		}
		// wtf is this one for
		// WHY ARE THEY BOTH USED HAHAHAHA
		public string get_tagID_by_datnum(string datnum)
		{
			//tag_struct t in Tags_List
			foreach (KeyValuePair<string, TagStruct> curr_tag in TagsList)
			{
				if (curr_tag.Value.Datnum == datnum)
				{
					return curr_tag.Key;
				}
			}
			return "FFFFFFFF"; // ok i found out what this was for: when we poke FFFFFFFF tag // annnnd it didnt work
		}
		
		public void PokeChanges()
		{
			if (!hooked)
			{
				poke_text.Text = "you MUST 'Load' first";
				return;
			}
			int fails = 0;
			int pokes = 0;
			for (int q = 0; q < Pokelistlist.Count; q++)
			{
			 	KeyValuePair<int, int> kv = pokelist(Pokelistlist.ElementAt(q).Key);
				fails += kv.Value;
				pokes += kv.Key;
			}
			if (fails < 1)
			{
				poke_text.Text = pokes + " changes poked!";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = pokes + " changes poked!";
				}
			}
			else
			{
				poke_text.Text = pokes + " poked, " + fails + " failed";
				if (mwidow != null)
				{
					mwidow.debug_text.Text = pokes + " poked, " + fails + " failed";
				}
			}
			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
		}
		
		public KeyValuePair<int, int> pokelist(string listname)
		{
			int fails = 0;
			int pokes = 0;
			// pokes || fails
			foreach (KeyValuePair<string, KeyValuePair<string, string>> pair in Pokelistlist[listname].Pokelist)
			{
				pokes++;
				bool failed = false;
				string do_the_thing = (pair.Key); 
				if (do_the_thing != "")
				{
					if (!pokesingle(do_the_thing, pair.Value.Key, pair.Value.Value, listname))
					{
						fails++;
						pokes--;
						failed = true;
					}
				}
				else
				{
					fails++;
					pokes--;
					failed = true;
				}
				if (UIpokelist.ContainsKey(pair.Key))
				{
					if (failed)
					{
						UIpokelist[pair.Key].bordercolor.BorderBrush = new SolidColorBrush(Colors.Red);
					}
					else
					{
						UIpokelist[pair.Key].bordercolor.BorderBrush = null;
					}
				}
			}
			return  new KeyValuePair<int, int> (pokes, fails);
		}


		
		public void tagchangesblock_fetchdata_by_ID(TagChangesBlock target) // aka do a single poke lol ?
		{
			
			KeyValuePair<string, string> pair = Pokelistlist[current_pokelist].Pokelist[target.sig_address_path];
			//pokesingle(target.sig_address_ID, pair.Key, pair.Value);
			//SUSSY_BALLS_2
			//string do_the_thing = SUSSY_BALLS_2();
			bool failed = false;
			if (target.sig_address_path != "")
			{
				if(!pokesingle(target.sig_address_path, pair.Key, pair.Value, current_pokelist))
				{
					poke_text.Text = "poke error";
					failed = true;
				}
				else
				{
					poke_text.Text = 1 + " change poked";
				}
			}
			else
			{
				poke_text.Text = "poke error";
				failed = true;
			}
			if (failed)
			{
				target.bordercolor.BorderBrush = new SolidColorBrush(Colors.Red);
			}
			else
			{
				target.bordercolor.BorderBrush = null;
			}
		}

		public bool pokesingle(string instruction_address, string type, string value, string reverthost_pokelist)
		{
			string address = SUSSY_BALLS_2(instruction_address);
			if (address == "") return false;
			if (value.Contains("`"))
			{
				string[] hooked_string = value.Split('`');
				if (hooked_string.Length == 2)
				{
					string do_the_thing = SUSSY_BALLS_2(hooked_string[1]);
					if (do_the_thing != "")
					{
						string read = readmem_for_1_very_specific_task(do_the_thing, type);
						if (read != "")
						{
							if (read == "FFFFFFFF" && type == "TagrefGroup")
							{
								pokesingle(instruction_address, "4Byte", "-1", reverthost_pokelist);
							}
							else
							{
								pokesingle(instruction_address, type, read, reverthost_pokelist);
							}
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}

				}
				else
				{
					return false;
				}
				return true;
			}
			else
			{
				if (!Pokelistlist[reverthost_pokelist].revertlist.ContainsKey(instruction_address))
				{
					string REVERT_value = readmem_for_1_very_specific_task(address, type);
					if (REVERT_value == "FFFFFFFF" && type == "TagrefGroup")
					{
						Pokelistlist[reverthost_pokelist].revertlist[instruction_address] = new KeyValuePair<string, string>("4Byte", "-1");
						if (UIpokelist.ContainsKey(instruction_address))
						{
							TagChangesBlock tcb = UIpokelist[instruction_address];
							tcb.revert.Text = "-1";
						}
					}
					else
					{
						Pokelistlist[reverthost_pokelist].revertlist[instruction_address] = new KeyValuePair<string, string>(type, REVERT_value);
						if (UIpokelist.ContainsKey(instruction_address))
						{
							UIpokelist[instruction_address].revert.Text = REVERT_value;
						}
					}
				}
				return writemem(type, address, value);
			}
			return false;
		}

		public bool writemem(string type, string address, string value)
		{
			if (!hooked)
				return false;
			switch (type)
			{
				case "4Byte":
					try { M.WriteMemory(address, "int", value); }
					catch { return false; }
					return true;
				case "2Byte": // needs to cap value
					try { M.WriteMemory(address, "2bytes", value); }
					catch { return false; }
					return true;
				case "Byte":
					try { M.WriteMemory(address, "byte", int.Parse(value).ToString("X")); }
					catch { return false; }
					return true;
				case "Flags":
					try { M.WriteMemory(address, "byte", Convert.ToByte(value).ToString("X")); }
					catch { return false; }
					return true;
				case "Float":
					try { M.WriteMemory(address, "float", value); }
					catch { return false; }
					return true;
				case "Pointer":
					try
					{
						string? willThisWork = new System.ComponentModel.Int64Converter().ConvertFromString(value).ToString();
						M.WriteMemory(address, "long", willThisWork); // apparently it does
					}
					catch
					{
						return false;
					}
					return true;
				case "String":
					try { M.WriteMemory(address, "string", value + "\0"); }
					catch { return false; }
					return true;
				case "TagrefGroup":
					try { M.WriteMemory(address, "string", ReverseString(value)); }
					catch { return false; }
					return true;
				case "TagrefTag":
					try
					{
						// convert ID to datnum
						if (TagsList.Keys.Contains(value))
						{
							var big_w = TagsList[value];
							string checked_ID = BitConverter.ToString(M.ReadBytes((big_w.TagData + 8).ToString("X"), 4)).Replace("-", string.Empty);
							string checked_datnum = BitConverter.ToString(M.ReadBytes((big_w.TagData + 12).ToString("X"), 4)).Replace("-", string.Empty);

							if (checked_ID == big_w.ObjectId && checked_datnum == big_w.Datnum)
							{
								string datnum_from_ID = TagsList[value].Datnum;
								string temp = Regex.Replace(datnum_from_ID, @"(.{2})", "$1 ");
								temp = temp.TrimEnd();
								M.WriteMemory(address, "bytes", temp);
							}
							else
							{
								return false;
							}
						}
						else if (value == "FFFFFFFF")
						{
							string temp = Regex.Replace(value, @"(.{2})", "$1 ");
							temp = temp.TrimEnd();
							M.WriteMemory(address, "bytes", temp);
						}
						else
						{
							return false;
						}
					}
					catch { return false; }
					return true;
				case "mmr3Hash":
					try
					{
						string temp2 = Regex.Replace(value, @"(.{2})", "$1 ");
						temp2 = temp2.TrimEnd();
						M.WriteMemory(address, "bytes", temp2);
					}
					catch
					{
						return false;
					}
					return true;
			}
			return false;
		}


		public string readmem_for_1_very_specific_task(string address, string type)
		{
			string output = "";
			switch (type)
			{
				case "4Byte": 
					try 
					{
						output = M.ReadInt(address).ToString(); 
					}
					catch { }
					return output;
				case "2Byte": // needs to cap value
					try
					{
						output = M.Read2Byte(address).ToString();
					}
					catch { }
					return output;
				case "Byte":
					try 
					{
						output = M.ReadByte(address).ToString();
					}
					catch { }
					return output;
				case "Flags":
					try 
					{
						output = M.ReadByte(address).ToString("X");
					}
					catch { }
					return output;
				case "Float":
					try 
					{
						output = M.ReadFloat(address).ToString();

					}
					catch { }
					return output;
				case "Pointer":
					try
					{
						output = M.ReadLong(address).ToString();
					}
					catch { }
					return output;
				case "String":
					try 
					{
						output = M.ReadString(address, "", 100).ToString();
					}
					catch { }
					return output;
				case "TagrefGroup":
					try 
					{
						string read_incase_bad_string = BitConverter.ToString(M.ReadBytes(address, 4)).Replace("-", string.Empty);
						if (read_incase_bad_string == "FFFFFFFF")
						{
							return "FFFFFFFF";
						}
						output = ReverseString(M.ReadString(address, "", 4));
					}
					catch { }
					return output;
				case "TagrefTag":
					try
					{
						output = get_tagID_by_datnum(BitConverter.ToString(M.ReadBytes(address, 4)).Replace("-", string.Empty)); // need to convert to tagID
					}
					catch { }
					return output;
				case "mmr3Hash":
					try
					{
						output = BitConverter.ToString(M.ReadBytes(address, 4)).Replace("-", string.Empty);
					}
					catch { }
					return output;
			}
			return output;
		}

		public string SUSSY_BALLS_2(string input)
		{
			//TAKE FIRST AND ADD INSTEAD OF LAST
		 	string[] p = input.Split(":");

			//p[0] = tagID
			//p[1] = address

			string[] last_offset = p[1].Split(",");
			
			for (int i5 = 2; i5 < last_offset.Length; i5++)
				last_offset[i5] = "0x" + long.Parse(last_offset[i5]).ToString("X");

			string joined = string.Join(",", last_offset.Skip(2));
			long poop = long.Parse(last_offset.Skip(1).First());
			// exception handling required here // i gotchu
			if (TagsList.Keys.Contains(p[0]))
			{
				var tag_thing = TagsList[p[0]];

				string checked_ID = BitConverter.ToString(M.ReadBytes((tag_thing.TagData + 8).ToString("X"), 4)).Replace("-", string.Empty);
				string checked_datnum = BitConverter.ToString(M.ReadBytes((tag_thing.TagData + 12).ToString("X"), 4)).Replace("-", string.Empty);

				if (checked_ID == tag_thing.ObjectId && checked_datnum == tag_thing.Datnum)
				{
					return "0x" + (poop += tag_thing.TagData).ToString("X") + ((joined == "") ? "" : "," + joined);
				}
			}
			return "";
		}

		public string return_real_number_of_pokes_queued_okk()
		{
			int num_of_pokes_counted = 0;
			for (int i = 0; i < Pokelistlist.Count; i++)
			{
				num_of_pokes_counted += Pokelistlist.ElementAt(i).Value.Pokelist.Count;
			}

			return num_of_pokes_counted.ToString();
		}

		public void Update_poke_value(TagChangesBlock target, string new_value)
		{
			KeyValuePair<string, string> pair = Pokelistlist[current_pokelist].Pokelist[target.sig_address_path];
			Pokelistlist[current_pokelist].Pokelist[target.sig_address_path] = new KeyValuePair<string, string>(pair.Key, new_value);
			poke_text.Text = "Poke Value Updated";

		}

		public void clearsingle(TagChangesBlock target)
		{
			Pokelistlist[current_pokelist].Pokelist.Remove(target.sig_address_path);
			UIpokelist.Remove(target.sig_address_path);
			changes_panel.Children.Remove(target);
			change_text.Text = return_real_number_of_pokes_queued_okk() + " changes queued";
		}

		
		public void clear_all_pokelists()
		{
			Pokelistlist.Clear();
			PokeList_Combobox.Items.Clear();
			add_new_section_to_pokelist("Poke Queue");

			if (PokeList_Combobox.Items.Count > 0)
				PokeList_Combobox.SelectedIndex = 0;

			change_text.Text = "0 changes queued";
			if (mwidow != null)
			{
				mwidow.test_changes.Text = "0 changes queued";
			}
			poke_text.Text = "All queues cleared";

		}
		
		public void clear_pokes_list(string queue)
		{
			changes_panel.Children.Clear();
			Pokelistlist[queue].Pokelist.Clear();
			UIpokelist.Clear();

			poke_text.Text = "Poke Lists Cleared";
		}
		

		/* 4Byte
		 * 2Byte
		 * Byte
         * Float
         * TagRef
         * Pointer
         * Tagblock
         * String
         * TagrefGroup
         * TagrefTag
         */

	
		public KeyValuePair<int, int> revertlist(string listname)
		{
			int fails = 0;
			int pokes = 0;
			// pokes || fails
			foreach (KeyValuePair<string, KeyValuePair<string, string>> pair in Pokelistlist[listname].revertlist)
			{
				pokes++;
				bool failed = false;
				if (pair.Key != "")
				{
					if (!revert_single_poke(pair.Key, pair.Value.Key, pair.Value.Value))
					{
						fails++;
						pokes--;
						failed = true;
					}
				}
				else
				{
					fails++;
					pokes--;
					failed = true;
				}
				if (UIpokelist.ContainsKey(pair.Key))
				{
					if (failed)
					{
						UIpokelist[pair.Key].bordercolor.BorderBrush = new SolidColorBrush(Colors.Red);
					}
					else
					{
						UIpokelist[pair.Key].bordercolor.BorderBrush = null;
					}
				}
			}
			return new KeyValuePair<int, int>(pokes, fails);
		}
		public bool revert_single_poke(string instruction_address, string type, string value)
		{
			string address = SUSSY_BALLS_2(instruction_address);
			if (address == "") return false;
			return writemem(type, address, value);
		}

		// addd new poke list
		public int num_of_user_added_lists = 0;
		
	}


	public class Tags
	{

	}
}
