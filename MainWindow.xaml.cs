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
using System.Windows.Media;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using InfiniteRuntimeTagViewer.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Xml;
using static InfiniteRuntimeTagViewer.StructureLayouts;

namespace InfiniteRuntimeTagViewer
{
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
			//If the user has opted to check for updates automatically
			if (Settings.Default.Updater)
			{
				//Check for updates
				Task.Run(() => CheckForUpdates(this, null));
			}
		}

		#region Variables
		public bool AutoHookKey;
		public bool AutoLoadKey;
		public bool AutoPokeKey;
		public bool FilterOnlyMappedKey;
		public bool OpacityKey;
		public bool CheckForUpdatesKey;
		public bool AlwaysOnTopKey;
		public bool done_loading_settings;
		public string ProcAsyncBaseAddr = Settings.Default.ProcAsyncBaseAddr;

		public delegate void HookAndLoadDelagate();
		public delegate void LoadTagsDelagate();
		private readonly System.Timers.Timer _t;
		public Mem M = new();

		// Hard-Coded Addresses
		//public string HookProcessAsyncBaseAddr = "HaloInfinite.exe+0x40AD150"; // TU11
		public string HookProcessAsyncBaseAddr; // Tag_List_Function
		public string ScanMemAOBBaseAddr = "HaloInfinite.exe+0x305B1B0"; // Tag_List_Str
		public string AOBScanTagStr = "74 61 67 20 69 6E 73 74 61 6E 63 65 73"; // Tag_List_Backup Str to find
		private readonly long AOBScanStartAddr = Convert.ToInt64("0000010000000000", 16);
		private readonly long AOBScanEndAddr = Convert.ToInt64("000003ffffffffff", 16);
		#endregion

		#region Settings
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
			CheckForUpdatesKey = Settings.Default.Updater;
		}

		public void SetGeneralSettingsFromConfig()
		{
			GetGeneralSettingsFromConfig();
			CbxSearchProcess.IsChecked = AutoHookKey;
			CbxAutoPokeChanges.IsChecked = AutoPokeKey;
			CbxFilterUnloaded.IsChecked = FilterOnlyMappedKey;
			whatdoescbxstandfor.IsChecked = AutoLoadKey; // Probably check box... -Z
			CbxOnTop.IsChecked = AlwaysOnTopKey;
			CbxOpacity.IsChecked = OpacityKey;
			CbxCheckForUpdates.IsChecked = CheckForUpdatesKey;
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
			Settings.Default.Updater = CbxCheckForUpdates.IsChecked;
		}
		#endregion

		#region Tag Loading
		// group 4chars, group instance
		// eg. weap, { system.whatever.balls }
		public Dictionary<string, TreeViewItem> groups_headers = new();
		public Dictionary<string, TreeViewItem> tags_headers = new();
		public ObservableCollection<string> second_level = new();
		public Dictionary<string, string> InhaledTagnames = new();
		public Dictionary<string, TagStruct> TagsList { get; set; } = new(); // and now we can convert it back because we just sort it elsewhere
		public SortedDictionary<string, GroupTagStruct> TagGroups { get; set; } = new();
		
		private bool is_checked;
		public bool loadedTags = false;
		public bool hooked = false;
		public long aobStart;
		private long BaseAddress = -1;
		private int TagCount = -1;

		public async void HookAndLoad()
		{
			try
			{
				await HookProcessAsync();
			}
			catch (System.ArgumentNullException)
			{

			}
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

		private async Task HookProcessAsync()
		{
			try
			{
				bool reset = processSelector.hookProcess(M);
				if (M.mProc.Process.Handle == IntPtr.Zero || processSelector.selected == false || loadedTags == false)
				{
					// Could not find the process
					SetStatus("Cant find HaloInfinite.exe");
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
						SetStatus("Process Hooked: " + M.mProc.Process.Id);
						hooked = true;
					}
					else
					{
						SetStatus("Offset failed, scanning...");
						await ScanMem();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				//If exception is a null reference exception, set the hook text to "Game Not Open"
				if (ex.GetType().IsAssignableFrom(typeof(NullReferenceException)))
				{
					SetStatus("Can't Find HaloInfinite.exe");
					//Show a message box that prompts the user if they want to launch the game
					MessageBoxResult result = (MessageBoxResult) System.Windows.Forms.MessageBox.Show("HaloInfinite.exe is not open. Do you want to open it?", "HaloInfinite.exe Not Open", System.Windows.Forms.MessageBoxButtons.YesNo);
					if (result == MessageBoxResult.Yes)
					{
						//Check if the setting for the game location is set
						if (Settings.Default.GameLocation != "")
						{
							//If it is set, open the game
							System.Diagnostics.Process.Start(Settings.Default.GameLocation);
							//wait 15 seconds before trying to load again
							await Task.Delay(15000);
							//Try to hook again
							await HookProcessAsync();
						}
						else
						{
							//If it is not set, allow the user to browse to an exe file, then open that exe file, wait 15 seconds, and resume loading.
							OpenFileDialog ofd = new()
							{
								Filter = "HaloInfinite.exe|HaloInfinite.exe",
								Title = "Please select HaloInfinite.exe"
							};
							ofd.ShowDialog();
							Settings.Default.GameLocation = ofd.FileName;
							Settings.Default.Save();
							System.Diagnostics.Process.Start(Settings.Default.GameLocation);
							await Task.Delay(15000);
							await HookProcessAsync();
						}
					}
				}
			}
		}

		public void UpdateAddress()
		{
			if (Settings.Default.ProcAsyncBaseAddr != "undefined")
			{
				HookProcessAsyncBaseAddr = Settings.Default.ProcAsyncBaseAddr;
			}
			else
			{
				HookProcessAsyncBaseAddr = "HaloInfinite.exe+0x40C3048";
			}
		}

		public async Task LoadTagsMem(bool is_silent)
		{
			is_checked = CbxFilterUnloaded.IsChecked;
			await Task.Run((Action) (() =>
			{
				if (TagCount != -1)
				{
					TagCount = -1;
					TagGroups.Clear();
					TagsList.Clear();
				}

				TagCount = this.M.ReadInt((BaseAddress + 0x6C).ToString("X"));
				long tagsStart = this.M.ReadLong((BaseAddress + 0x78).ToString("X"));

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

					byte[] test1 = this.M.ReadBytes(tagAddress.ToString("X"), (long) 4);
					try
					{
						currentTag.Datnum = BitConverter.ToString(test1).Replace("-", string.Empty);
						loadedTags = false;
					}
					catch (ArgumentNullException)
					{
						hooked = false;
						return;
					}
					byte[] test = (this.M.ReadBytes((tagAddress + 4).ToString("X"), (long) 4));

					// = String.Concat(bytes.Where(c => !Char.IsWhiteSpace(c)));
					currentTag.ObjectId = BitConverter.ToString(test).Replace("-", string.Empty);
					currentTag.TagGroup = read_tag_group((long) this.M.ReadLong((tagAddress + 0x8).ToString("X")));
					currentTag.TagData = this.M.ReadLong((tagAddress + 0x10).ToString("X"));
					currentTag.TagFullName = convert_ID_to_tag_name(currentTag.ObjectId).Trim();
					currentTag.TagFile = currentTag.TagFullName.Split('\\').Last<string>().Trim();

					if (is_checked)
					{
						byte[] b = this.M.ReadBytes((currentTag.TagData + 12).ToString("X"), (long) 4);
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
			}));

			if (!is_silent)
			{
				Loadtags();
			}
		}

		public void Loadtags()
		{
			Dictionary<string, TreeViewItem> tags_headers_diff = new();
			Dictionary<string, TreeViewItem> groups_headers_diff = new();
			loadedTags = true;

			for (int i = 0; i < TagGroups.Count; i++)
			{
				KeyValuePair<string, GroupTagStruct> goop = TagGroups.ElementAt(i);

				if (groups_headers.Keys.Contains(goop.Key))
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
					TreeViewItem sortheader = new();

					sortheader.Header = displayGroup.TagGroupName + " (" + displayGroup.TagGroupDesc + ")";
					sortheader.ToolTip = new TextBlock { Foreground = Brushes.Black, Text = displayGroup.TagGroupDefinitition };
					displayGroup.TagCategory = sortheader;

					TagGroups[goop.Key] = displayGroup;
					TagsTree.Items.Add(sortheader);

					groups_headers_diff.Add(goop.Key, sortheader);
				}
			}

			foreach (KeyValuePair<string, TagStruct> curr_tag in TagsList.OrderBy(key => key.Value.TagFullName))
			{
				if (!curr_tag.Value.unloaded)
				{
					if (tags_headers.Keys.Contains(curr_tag.Key))
					{
						TreeViewItem t = tags_headers[curr_tag.Key];
						t.Tag = curr_tag.Key;
						tags_headers_diff.Add(curr_tag.Key, t);
						tags_headers.Remove(curr_tag.Key);
					}
					else
					{
						TreeViewItem t = new();
						TagStruct tag = curr_tag.Value;
						TagGroups.TryGetValue(tag.TagGroup, out GroupTagStruct? dictTagGroup);

						t.Header = "(" + tag.Datnum + ") " + convert_ID_to_tag_name(tag.ObjectId);
						t.Tag = curr_tag.Key;
						t.Selected += Select_Tag_click;

						if (dictTagGroup != null && dictTagGroup.TagCategory != null)
						{
							dictTagGroup.TagCategory.Items.Add(t);
						}

						tags_headers_diff.Add(curr_tag.Key, t);
					}
				}
			}

			foreach (KeyValuePair<string, TreeViewItem> poop in groups_headers)
			{
				if (poop.Value != null)
				{
					TagsTree.Items.Remove(poop.Value);
				}
			}

			foreach (KeyValuePair<string, TreeViewItem> poop in tags_headers)
			{
				if (poop.Value != null)
				{
					TreeViewItem ownber = (TreeViewItem) poop.Value.Parent;
					ownber.Items.Remove(poop.Value);
				}
			}

			tags_headers = tags_headers_diff;
			groups_headers = groups_headers_diff;

			if (TagsTree.Items.Count < 1)
			{
				loadedTags = false;
			}

			TagsTree.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));

			SetStatus("Loaded Tags!");
		}

		public async Task ScanMem()
		{
			// FALLBACK ADDRESS POINTER (which is literally useless)
			// However, it is faster than scanning memory and is used as a fast reference ptr to load quicker.
			BaseAddress = M.ReadLong(ScanMemAOBBaseAddr);
			string validtest = M.ReadString(BaseAddress.ToString("X"));

			if (validtest == "tag instances")
			{
				SetStatus("Process Hooked: " + M.mProc.Process.Id);
				hooked = true;
			}
			else
			{
				SetStatus("Offset failed, scanning...");
				try
				{
					long? aobScan = (await M.AoBScan(AOBScanStartAddr, AOBScanEndAddr, AOBScanTagStr, true)).First(); // "tag instances"

					long haloInfinite = 0;
					if (aobScan != null)
					{
						//get all processes named HaloInfinite
						foreach (Process process in Process.GetProcessesByName("HaloInfinite"))
						{
							//get the base address of the process
							haloInfinite = (long) process.MainModule.BaseAddress;
						}
						string aobHex = aobScan.Value.ToString("X");
						IEnumerable<string> aobStr = SplitThis("0" + aobHex, 2);
						IEnumerable<string> aobReversed = aobStr.Reverse().ToArray();
						string aobSingle = string.Join("", aobReversed);
						aobSingle = Regex.Replace(aobSingle, ".{2}", "$0 ");
						aobSingle = aobSingle.TrimEnd();
						Debugger.Log(0, "DBGTIMING", "AOB: " + aobSingle);
						long pointer = (await M.AoBScan(haloInfinite, 140737488289791, aobSingle + " 00 00", true, true, true)).First();
						Debug.WriteLine(pointer);
						Settings.Default.ProcAsyncBaseAddr = "HaloInfinite.exe+0x" + (pointer - haloInfinite).ToString("X");
						Settings.Default.Save();
						Debug.WriteLine(Settings.Default.ProcAsyncBaseAddr);

					}

					// Failed to find base tag address
					if (aobScan == null || aobScan == 0)
					{
						BaseAddress = -1;
						loadedTags = false;
						SetStatus("Failed to locate base tag address");
					}
					else
					{
						BaseAddress = aobScan.Value;
						SetStatus("Process Hooked: " + M.mProc.Process.Id + " (AOB)");
						hooked = true;
					}
				}
				catch (Exception)
				{
					SetStatus("Cant find HaloInfinite.exe");
				}
			}
		}

		public static IEnumerable<string> SplitThis(string str, int n)
		{
			return Enumerable.Range(0, str.Length / n).Select(i => str.Substring(i * n, n));
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
			TagEditorControl? tagEditor = new(this);
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
		#endregion

		#region Auto Loader
		static int min_tags_changed_for_update = 750;
		static int min_tags_changed_for_interupt_update = 150;
		public int tag_count_last_update = 0;
		public int current_tag_count = 0;
		public bool is_waiting;

		private async void OnTimedEvent(object source, ElapsedEventArgs e)
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
						//extra_tag_text.Text = "found (" + real_tag_count + " tags)";
						if (current_tag_count < tag_count_last_update - min_tags_changed_for_update || current_tag_count > tag_count_last_update + min_tags_changed_for_update)
						{
							is_waiting = true;
							//extra_tag_text.Text = "one sec (" + real_tag_count + " tags)";
							while (is_waiting)
							{
								await Task.Delay(12000);

								int real_tag_count_again = M.ReadInt((BaseAddress + 0x70).ToString("X"));
								if (!(real_tag_count_again < current_tag_count - min_tags_changed_for_interupt_update) && !(real_tag_count_again > current_tag_count + min_tags_changed_for_interupt_update))
								{
									is_waiting = false;
									//extra_tag_text.Text = "reloading (" + real_tag_count_again + " tags)";
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
									//extra_tag_text.Text = "Awaiting (" + real_tag_count_again + " tags)";
								}
							}
						}
					}
				}
			}
		}
		#endregion

		#region UI Event Handlers
		//Adjust UI element size depending on window size.
		private void window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			double newWindowWidth = e.NewSize.Width;
			//Debug.WriteLine(newWindowWidth);
			if (newWindowWidth < 1200)
			{
				CloseProcBtn.IsHitTestVisible = false;
				CloseProcBtn.Visibility = Visibility.Collapsed;
			}
			else
			{
				CloseProcBtn.IsHitTestVisible = true;
				CloseProcBtn.Visibility = Visibility.Visible;
			}
			if (newWindowWidth < 1000)
			{
				ReloadProcBtn.IsHitTestVisible = false;
				ReloadProcBtn.Visibility = Visibility.Collapsed;
			}
			else
			{
				ReloadProcBtn.IsHitTestVisible = true;
				ReloadProcBtn.Visibility = Visibility.Visible;
			}
		}

		private void BtnReloadProcessClick(object sender, RoutedEventArgs e)
		{
			foreach (Process? process in Process.GetProcessesByName("HaloInfinite"))
			{
				string? filePath = process.MainModule.FileName;
				process.Kill();
				Process.Start(filePath);
			}
		}

		private void BtnCloseClick(object sender, RoutedEventArgs e)
		{
			foreach (Process? process in Process.GetProcessesByName("HaloInfinite"))
			{
				process.Kill();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			num_of_user_added_lists++;
			add_new_section_to_pokelist("Poke Queue(" + num_of_user_added_lists + ")");
		}

		private void SetStatus(string message)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				statusText.Text = message;
			}));
		}

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
		#endregion

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

		public void ShowGameExeDialog(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "Halo Infinite Executable|HaloInfinite.exe",
				Title = "Select the Halo Infinite executable"
			};
			ofd.ShowDialog();
			if (ofd.FileName != "")
			{
				//Save the setting for the exe file
				Settings.Default.GameLocation = ofd.FileName;
				SetStatus("Updated Game Location");
			}
		}
		
		public void CheckForUpdates(object sender, RoutedEventArgs e)
		{
			//Check for recent commits on GitHub
			//If there are recent commits, display a message box asking if the user would like to download the latest version
			//If the user clicks yes, open the link to the GitHub repo and download the latest version

			//Get recent commits from GitHub
			string commits = "";
			string node_id;
			try
			{
				
				using (WebClient client = new WebClient())
				{
					client.Headers.Add("user-agent", "request");
					commits = client.DownloadString("https://api.github.com/repos/Gamergotten/Infinite-runtime-tagviewer/commits/master");
					Debug.WriteLine("Commits: " + commits);
					//parse the json and set node_id to the value of the node_id field
					JObject json = JObject.Parse(commits);
					node_id = (string) json["node_id"];
					Debug.WriteLine("Node ID: " + node_id);
				}
				if (Settings.Default.Version == "")
				{
					Settings.Default.Version = node_id;
					Settings.Default.Save();
					return;
				}
			}
			catch (Exception)
			{
				Debug.WriteLine("API Fail");
				System.Windows.Forms.MessageBox.Show("Unable to check for updates. Please check your internet connection.");
				return;
			}
			string storedVersion;
			try
			{
				//Get the stored version number from the settings file
				storedVersion = Settings.Default.Version;
				Debug.WriteLine("Stored version: " + storedVersion);
			}
			catch (Exception)
			{
				System.Windows.Forms.MessageBox.Show("Unable to check for updates. Please check your internet connection.");
				return;
			}
			//Compare the two and if the stored version is less than the current version, display a message box asking if the user would like to download the latest version
			if (storedVersion != node_id)
			{
				DialogResult result = System.Windows.Forms.MessageBox.Show("A new version of the tag viewer is available. Would you like to download the latest version?", "Update Available", MessageBoxButtons.YesNoCancel);
				if (result == System.Windows.Forms.DialogResult.Yes)
				{
					SetStatus("Downloading latest version...");
					//Download the file
					try
					{
						using (WebClient client = new WebClient())
						{
							client.DownloadProgressChanged += (s, e) =>
							{
								SetStatus("Downloading..." + e.ProgressPercentage.ToString() + "%");
							};
							client.DownloadFileCompleted += (s, e) =>
							{
								SetStatus("Download Complete");
								// any other code to process the file
								//Write the node id to the settings file
								Settings.Default.Version = node_id;
								Settings.Default.Save();
								//Start the unzip.bat file
								ProcessStartInfo startInfo = new()
								{
									FileName = "unzip.bat",
									Arguments = "Infinite-runtime-tagviewer.zip"
								};
								Process.Start(startInfo);
								//Close the current process
								Environment.Exit(0);
							};
							//Download the file on a background thread
							client.DownloadFileAsync(new Uri("https://nightly.link/Gamergotten/Infinite-runtime-tagviewer/workflows/dotnet/master/IRTV.zip"), "IRTV.zip");
							while (client.IsBusy)
							{
								System.Windows.Forms.Application.DoEvents();
							}

						}
					}
					catch (Exception)
					{
						System.Windows.Forms.MessageBox.Show("Unable to download the latest version. Please check your internet connection.");
					}
				}
			}
			else
			{
				SetStatus("Client up to date!");
			}
			return;
		}

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

		#region Poking
		// list of changes to ammend to the memory when we phit the poke button
		// i think it goes: address, type, value
		// address instructions | value type | value value
		public Dictionary<string, Poke_queue> Pokelistlist = new();
		public Dictionary<string, TagChangesBlock> UIpokelist = new(); // i *think* we can just leave this as is

		public class Poke_queue
		{
			public Dictionary<string, KeyValuePair<string, string>> Pokelist = new();
			public Dictionary<string, KeyValuePair<string, string>>? revertlist = new();
		}
		public string current_pokelist = "";
		public int num_of_user_added_lists = 0; // Add a new poke list.

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

				for (int i = 0; i < Pokelistlist[queuename].Pokelist.Count; i++)
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
			try
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
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
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
			return new KeyValuePair<int, int>(pokes, fails);
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
				if (!pokesingle(target.sig_address_path, pair.Key, pair.Value, current_pokelist))
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
		#endregion

		#region Tag Struct Dumper
		public XmlWriter textWriter;
		private XmlWriterSettings xmlWriterSettings = new()
		{
			Indent = true,
			IndentChars = "\t",
		};
		private long startAddress = 0;
		private int tagCount = 0;
		private string outDIR = @".\Plugins";

		private async void DumpClick(object sender, RoutedEventArgs e)
		{
			SetStatus("Dumping tag structs...");
			ClearPlugins();

			await AoBScan();

			if (startAddress != 0)
			{
				SetStatus("Address Found: " + startAddress.ToString("X"));

				int warnings = 0;
				long curAddress = startAddress;
				bool scanning = true;

				while (scanning)
				{
					if (M.ReadInt((curAddress + 80).ToString("X")) == 257)
					{
						tagCount++;
						curAddress += 88;
						warnings = 0;
					}
					else
					{
						warnings++;
						curAddress += 88;
					}

					if (warnings > 3)
					{
						scanning = false;
					}
				}

				SetStatus("Found " + tagCount + " tag structs!");

				DumpStructs();

				SetStatus("Done!");
			}
		}

		private void ClearPlugins()
		{
			foreach (string file in Directory.EnumerateFiles(outDIR))
			{
				File.Delete(file);
			}	
		}

		private async Task AoBScan()
		{
			long[] results = (await M.AoBScan("?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 53 62 6F 47 67 61 54 61", true, false)).ToArray();
			startAddress = results[0];
		}

		private void DumpStructs()
		{
			try
			{
				for (int iteration_index = 0; iteration_index < tagCount; iteration_index++)
				{
					string temp_filename = outDIR + @"\dump" + iteration_index + ".xml";
					using (XmlWriter w = XmlWriter.Create(temp_filename, xmlWriterSettings))
					{
						textWriter = w;
						textWriter.WriteStartDocument();
						textWriter.WriteStartElement("root");

						long offset_from_start = iteration_index * 88;
						long current_tag_struct_Address = startAddress + offset_from_start;
						long gdshgfjasdf = (current_tag_struct_Address);
						string group_name_thingo = M.ReadString((current_tag_struct_Address + 12).ToString("X"), "", 4);
						GetGDLS(M.ReadLong((current_tag_struct_Address + 32).ToString("X")));

						textWriter.WriteEndElement();
						textWriter.WriteEndDocument();
						textWriter.Close();

						System.IO.FileInfo fi = new System.IO.FileInfo(temp_filename);
						if (fi.Exists)
						{
							string s33 = ReverseString(group_name_thingo);
							if (!s33.Contains("*"))
							{
								if (s33 != "cmpS")
								{
									if (File.Exists(outDIR + @"\" + s33 + ".xml"))
									{
										fi.MoveTo(outDIR + @"\" + s33 + "1.xml");
									}
									else
									{
										fi.MoveTo(outDIR + @"\" + s33 + ".xml");
									}
								}
							}
							else
							{
								string s331 = s33.Replace("*", "_");
								fi.MoveTo(outDIR + @"\" + s331 + ".xml");
								SetStatus(s33 + " replaced with " + s331);
							}
						}
					}
				}
			}
			catch
			{
				SetStatus("Failed to dump!");
			}
		}

		private Group_definitions_link_struct GetGDLS(long address)
		{
			Group_definitions_link_struct gdls = new Group_definitions_link_struct
			{
				name1 = M.ReadString(M.ReadLong(address.ToString("X")).ToString("X"), "", 300),
				name2 = M.ReadString(M.ReadLong((address + 8).ToString("X")).ToString("X"), "", 300),

				int1 = M.ReadInt((address + 16).ToString("X")),
				int2 = M.ReadInt((address + 20).ToString("X")), // potential count

				Table2_struct_pointer2 = M.ReadLong((address + 24).ToString("X")),
				Table2_struct = ReadChunk(M.ReadLong((address + 24).ToString("X"))), // next

			};

			return gdls;
		}

		private Table2_struct ReadChunk(long address)
		{

			int amount_of_things_to_read = M.ReadInt((address + 120).ToString("X"));

			long address_for_our_string_bruh = M.ReadLong(address.ToString("X"));
			string take_this_mf_and_pass_it_down_for_gods_sake = M.ReadString(address_for_our_string_bruh.ToString("X"), "", 300);

			for (int index = 0; index < amount_of_things_to_read; index++)
			{
				long address_next_next = M.ReadLong((address + 32).ToString("X")) + (index * 24);

				int group = M.ReadInt((address_next_next + 8).ToString("X"));
				string n_name = M.ReadString(M.ReadLong(address_next_next.ToString("X")).ToString("X"), "", 300);

				long next_next_next_address = M.ReadLong((address_next_next + 16).ToString("X"));
				//    , group, address_next_next, ); // real_name_100
				//
				textWriter.WriteStartElement("_" + group.ToString("X"));
				textWriter.WriteAttributeString("v", n_name);
				switch (group)
				{
					case 0x2:
						possible_t1_struct_c_instance ptsct_02 = new possible_t1_struct_c_instance
						{
							_02_ = new _02
							{
								exe_pointer = M.ReadLong(next_next_next_address.ToString("X"))
							}
						};
						break;
					case 0xA:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xB:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xC:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xD:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xE:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xF:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0x29:
						new possible_t1_struct_c_instance
						{
							_29_ = new _29
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2A:
						new possible_t1_struct_c_instance
						{
							_2A_ = new _2A
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2B:
						new possible_t1_struct_c_instance
						{
							_2B_ = new _2B
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2C:
						new possible_t1_struct_c_instance
						{
							_2C_ = new _2C
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2D:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x2E:
						new possible_t1_struct_c_instance
						{
							_2E_ = new _2E
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2F:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x30:
						new possible_t1_struct_c_instance
						{
							_30_ = new _30
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x31:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x34:
						textWriter.WriteAttributeString("length", next_next_next_address.ToString());
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x35:
						textWriter.WriteAttributeString("length", next_next_next_address.ToString());
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x36:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x37:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x38:
						new possible_t1_struct_c_instance
						{
							_38_ = new _38
							{
								table2_ref = ReadChunk(next_next_next_address)
							}
						};
						break;
					case 0x39:
						new possible_t1_struct_c_instance
						{
							_39_ = new _39
							{
								Name1 = M.ReadString(M.ReadLong(next_next_next_address.ToString("X")).ToString("X"), "", 300),
								int1 = M.ReadInt((next_next_next_address + 8).ToString("X")),
								int2 = M.ReadInt((next_next_next_address + 12).ToString("X")),
								long1 = M.ReadLong((next_next_next_address + 16).ToString("X")),
								//table2_ref = read_the_big_chunky_one(address) // bruh this in the wrong spot
							}
						};
						// i think we can just ingore that stuff
						int Repeatamount = M.ReadInt((next_next_next_address + 8).ToString("X"));

						for (int i = 0; i < Repeatamount; i++)
						{
							ReadChunk(M.ReadLong((next_next_next_address + 24).ToString("X")));
						}
						break;
					case 0x40:
						new possible_t1_struct_c_instance
						{
							_40_ = new _40
							{
								tag_struct_pointer = GetGDLS(next_next_next_address)
							}
						};
						break;
					case 0x41:
						long child_address = M.ReadLong((next_next_next_address + 136).ToString("X"));
						new possible_t1_struct_c_instance
						{
							_41_ = new _41
							{
								int1 = M.ReadInt((next_next_next_address + 0).ToString("X")),
								taggroup1 = M.ReadString((next_next_next_address + 4).ToString("X"), "", 4),

								taggroup2 = M.ReadString((child_address + 0).ToString("X"), "", 4),
								taggroup3 = M.ReadString((child_address + 4).ToString("X"), "", 4),
								taggroup4 = M.ReadString((child_address + 8).ToString("X"), "", 4),
								taggroup5 = M.ReadString((child_address + 12).ToString("X"), "", 4)
							}
						};
						break;
					case 0x42:
						new possible_t1_struct_c_instance
						{
							_42_ = new _42
							{
								Name1 = M.ReadString(M.ReadLong(next_next_next_address.ToString("X")).ToString("X"), "", 300),
								int1 = M.ReadInt((next_next_next_address + 8).ToString("X")),
								int2 = M.ReadInt((next_next_next_address + 12).ToString("X")),
								int3 = M.ReadInt((next_next_next_address + 16).ToString("X")),
								int4 = M.ReadInt((next_next_next_address + 20).ToString("X")),
								long1 = M.ReadLong((next_next_next_address + 24).ToString("X")),
								long2 = M.ReadLong((next_next_next_address + 32).ToString("X")),
								long3 = M.ReadLong((next_next_next_address + 40).ToString("X")),
								long4 = M.ReadLong((next_next_next_address + 48).ToString("X")),
								long5 = M.ReadLong((next_next_next_address + 56).ToString("X")),
								long6 = M.ReadLong((next_next_next_address + 64).ToString("X")),
							}
						};
						break;
					case 0x43:
						new possible_t1_struct_c_instance
						{
							_43_ = new _43
							{
								Name1 = M.ReadString(M.ReadLong(next_next_next_address.ToString("X")).ToString("X"), "", 300),
								long1 = M.ReadLong((next_next_next_address + 8).ToString("X")),
								//table2_ref = read_the_big_chunky_one(address+16),
								long2 = M.ReadLong((next_next_next_address + 24).ToString("X")),
							}
						};
						break;
				}

				//
				textWriter.WriteEndElement();


			}
			return new Table2_struct { };
		}

		private possible_t1_struct_c_instance TryGetPossibleStructInstance(long address)
		{


			int count_of_children = M.ReadInt((address + 8).ToString("X"));
			long children_address = M.ReadLong((address + 16).ToString("X"));
			List<string> childs = new();

			for (int i = 0; i < count_of_children; i++)
			{
				textWriter.WriteStartElement("Flag");

				long address_WHY_WONT_YOU_WORK = M.ReadLong((address + 16).ToString("X"));

				string reuse_me_uh = M.ReadString(M.ReadLong((address_WHY_WONT_YOU_WORK + (i * 8)).ToString("X")).ToString("X"), "", 300);
				childs.Add(reuse_me_uh);

				textWriter.WriteAttributeString("v", reuse_me_uh);


				textWriter.WriteEndElement();
			}

			possible_t1_struct_c_instance ptsct_0A = new possible_t1_struct_c_instance
			{
				_0B_through_0F_ = new _0B_through_0F
				{
					name = M.ReadString(M.ReadLong(address.ToString("X")).ToString("X"), "", 300),
					count = count_of_children,
					children = childs
				}
			};

			return ptsct_0A;
		}
		#endregion
	}

	public class StructureLayouts
	{
		public struct Group_definitions_link_struct // 40 bytes
		{
			public string name1;
			public string name2;

			public int int1;
			public int int2;

			public Table2_struct Table2_struct; // Table2_struct
			public long Table2_struct_pointer2; // Table2_struct
		}
		public struct Table2_struct // 200 bytes
		{
			public string Name1;
			public string Name2;

			public string hash1;
			public string hash2;
			public string hash3;
			public string hash4;

			public List<Table1_struct> tag_struct_lookup1;

			public int int1; // 384
			public int int2; // 0

			public long exe_pointer1;

			public int int3; // 17
			public int int4; // 0
			public int int5; // 384
			public int int6; // 0

			public string hash5;
			public string hash6;

			public int int7; // 1
			public int int8; // 0

			public long tag_struct_lookup2;

			public string Name3;

			public int int9; // 384
			public int int10; // 0

			public long unknown_pointer1;
			public string unknown_string4;

			public int int11; // 1
			public int int12; // 0
			public int int13; //12387

			public long tag_struct_lookup3;

			public int STRUCTCOUNT; // num of child elements
			public int int14; // 1
			public int int15; // 2664
			public int int16; // 3159044
			public int int17; // 164626464
			public int int18; // 6
			public int int19; // 3159044
			public int int20; // 164626464
			public int int21; // 6
			public int int22; // 0
			public int int23; // 0
			public int int24; // 0

			public long exe_pointer2; // doesn't seem to ever point anywhere
		}



		public struct Table1_struct // 24 bytes
		{
			public string name;
			public int struct_type_index;
			public int int2;
			public possible_t1_struct_c_instance? dodgy_struct; // alternates based on "struct_type_index"
																// primarily a pointer, can also be an int
		}


		public static Dictionary<string, long> group_lengths_dict = new()
		{
			{ "_0", 32 }, // _field_string
            { "_1", 256 }, // _field_long_string
            { "_2", 4 }, // _field_string_id
            { "_3", 4 },
			{ "_4", 1 },
			{ "_5", 2 }, // _field_short_integer
            { "_6", 4 }, // _field_long_integer
            { "_7", 8 }, // _field_int64_integer
            { "_8", 4 }, // _field_angle
            { "_9", 4 },
			{ "_A", 1 }, // _field_char_enum
            { "_B", 2 }, // _field_short_enum
            { "_C", 4 }, // _field_long_enum
            { "_D", 4 }, // _field_long_flags
            { "_E", 2 }, // _field_word_flags
            { "_F", 1 }, // _field_byte_flags
            { "_10", 4 },
			{ "_11", 4 },
			{ "_12", 4 },
			{ "_13", 4 },
			{ "_14", 4 }, // _field_real
            { "_15", 4 }, // _field_real_fraction
            { "_16", 4 },
			{ "_17", 12 }, // _field_real_point_3d
            { "_18", 12 },
			{ "_19", 12 }, // _field_real_vector_3d
            { "_1A", 12 }, // quarternion 4
            { "_1B", 12 },
			{ "_1C", 12 }, // _field_real_euler_angles_3d
            { "_1D", 12 },
			{ "_1E", 12 },
			{ "_1F", 12 }, // _field_real_rgb_color
            { "_20", 4 },
			{ "_21", 4 },
			{ "_22", 4 },
			{ "_23", 4 },
			{ "_24", 8 }, // _field_angle_bounds
            { "_25", 8 }, // _field_real_bounds
            { "_26", 4 },
			{ "_27", 4 },
			{ "_28", 4 },
			{ "_29", 4 },
			{ "_2A", 4 },
			{ "_2B", 4 },
			{ "_2C", 1 }, // _field_char_block_index
            { "_2D", 1 },
			{ "_2E", 2 }, // _field_short_block_index
            { "_2F", 2 },
			{ "_30", 4 }, // _field_long_block_index
            { "_31", 4 },
			{ "_32", 4 },
			{ "_33", 4 },
			{ "_34", 4 }, // _field_pad
            { "_35", 4 },
			{ "_36", 0 }, // _field_explanation
            { "_37", 0 }, // _field_custom
            { "_38", 0 }, // _field_struct
            { "_39", 32 }, // something verticies
            { "_3A", 4 },
			{ "_3B", 0 }, // end of struct or something
            { "_3C", 1 }, // _field_byte_integer
            { "_3D", 2 },
			{ "_3E", 4 }, // _field_dword_integer
            { "_3F", 8 },
			{ "_40", 20 }, // _field_block_v2
            { "_41", 28 }, // _field_reference_v2
            { "_42", 24 }, // _field_data_v2
            { "_43", 4 },
			{ "_44", 4 },
			{ "_45", 4 },
		};

		// 00 // _field_string
		// 01 // _field_long_string
		// 02 // _field_string_id
		// 03
		// 04
		// 05 // _field_short_integer
		// 06 // _field_long_integer
		// 07 // _field_int64_integer
		// 08 // _field_angle
		// 09
		// 0A // _field_char_enum
		// 0B // _field_short_enum
		// 0C // _field_long_enum
		// 0D // _field_long_flags
		// 0E // _field_word_flags
		// 0F // _field_byte_flags
		// 10
		// 11
		// 12
		// 13
		// 14 // _field_real
		// 15 // _field_real_fraction
		// 16
		// 17 // _field_real_point_3d
		// 18 
		// 19 // _field_real_vector_3d
		// 1A
		// 1B // 
		// 1C // _field_real_euler_angles_3d
		// 1D
		// 1E
		// 1F // _field_real_rgb_color
		// 20
		// 21
		// 22
		// 23
		// 24 // _field_angle_bounds
		// 25 // _field_real_bounds
		// 26
		// 27
		// 28
		// 29
		// 2A
		// 2B
		// 2C // _field_char_block_index
		// 2D
		// 2E // _field_short_block_index
		// 2F
		// 30 // _field_long_block_index
		// 31
		// 32
		// 33
		// 34 // _field_pad -- LENGTH IS REQUIRED
		// 35
		// 36 // _field_explanation
		// 37 // _field_custom
		// 38 // _field_struct
		// 39
		// 3A
		// 3B -- END STRUCT
		// 3C // _field_byte_integer
		// 3D
		// 3E // _field_dword_integer
		// 3F
		// 40 // _field_block_v2
		// 41 // _field_reference_v2
		// 42 // _field_data_v2
		// 43




		public struct possible_t1_struct_c_instance
		{
			public long actual_value;
			public _02 _02_;
			public _0B_through_0F _0B_through_0F_; // flags and enums
			public _29 _29_;
			public _2A _2A_;
			public _2B _2B_;
			public _2C _2C_;
			public _2D _2D_;
			public _2E _2E_;
			public _2F _2F_;
			public _30 _30_;
			public _31 _31_;
			public _34 _34_; // generated pad
			public _35 _35_; // another pad?
			public _36 _36_;
			public _37 _37_;
			public _38 _38_;
			public _39 _39_;
			public _40 _40_;
			public _41 _41_;
			public _42 _42_;
			public _43 _43_;
		} // bruh howtf do you store these as a single variable

		public struct _02 // unknown
		{
			public long exe_pointer; // mostly invalid
		}
		public struct _0B_through_0F // flags and enums
		{
			public string name;
			public long count;
			public List<string> children;
		}
		public struct _29
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2A
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2B
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2C
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2D
		{
			// nothing
		}
		public struct _2E
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2F
		{
			// pointer to somewhere
		}
		public struct _30
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _31
		{
			// pointer to who knows wheree
		}
		public struct _34
		{
			//public long generated_pad_length;
			// so thers actually nothing in this struct
		}
		public struct _35
		{
			//public long generated_pad_length;
			// so thers actually nothing in this struct
		}
		public struct _36
		{
			// potentially nothing, this points to render stuff
		}
		public struct _37
		{
			// nothing notable aiside from the 4 byte after count
		}
		public struct _38
		{
			public Table2_struct table2_ref;
		}
		public struct _39
		{
			public string Name1;
			public int int1;
			public int int2;
			public long long1;
			public Table2_struct table2_ref;
		}
		public struct _40
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _41
		{
			public int int1;
			public string taggroup1;
			// pointer to
			public string taggroup2;
			public string taggroup3;
			public string taggroup4;
			public string taggroup5;
		}
		public struct _42 // length 72 bytes
		{
			public string Name1;
			public int int1;
			public int int2;
			public int int3;
			public int int4;

			public long long1;
			public long long2;
			public long long3;
			public long long4;
			public long long5;
			public long long6;
		}
		public struct _43 // length 72 bytes
		{
			public string Name1;
			public long long1;
			public Table2_struct table2_ref;
			public long long2;

		}
	}
}
