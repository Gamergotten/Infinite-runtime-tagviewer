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
using System.Reflection;

namespace InfiniteRuntimeTagViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		// #### NOTES
		//
		// tagref dropdown actually gives the wrong tags, may be because some objects have the same datnums
		//
		// loading levl tag on campaign takes FOREVER + 3gigs RAM // fixed
		//
		// i think our "readstring" is capped to a specific amount of characters // fixed
		//
		// something since the inital release is causing windows defender to act up
		// my thoughts - either the AOB or .net 5.0? could just be random too tho
		// - Callum : I believe its AOB, That kind of stuff always flags up the AV's.
		//            They smell it like blood in the water.
		//
		private readonly Timer _t;
		public Mem M = new();

		public MainWindow()
		{
			InitializeComponent();
			GetAllMethods();
			StateChanged += MainWindowStateChangeRaised;
			_t = new Timer();
			_t.Elapsed += OnTimedEvent;
			_t.Interval = 2000;
			_t.AutoReset = true;
			inhale_tagnames();
		}

		public bool loadedTags = false;
		public bool hooked = false;
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(async () =>
			{
				//hook_text.Text = "Opening process...";
				processSelector.hookProcess(M);
				if (M.pHandle == IntPtr.Zero || processSelector.selected == false || loadedTags == false)
				{
					// Could not find the process
					hook_text.Text = "Cant find HaloInfinite.exe";
					hooked = false;
					loadedTags = false;
				}

				if (hooked == false)
				{
					// Get the base address
					await ScanMem();
				}
				if (BaseAddress != -1 && loadedTags == false)
				{
					LoadTagsMem();
					if (hooked == true)
					{


						Loadtags();

						Searchbox_TextChanged(null, null);

						System.Diagnostics.Debugger.Log(0, "DBGTIMING", "Done loading tags");
						if (TagsTree.Items.Count > 0)
						{
							loadedTags = true;
						}
					}
				}
			}));
		}

		public async Task ScanMem()
		{
			BaseAddress = M.ReadLong("HaloInfinite.exe+0x4879758");
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
					long? aobScan = (await M.AoBScan("74 61 67 20 69 6E 73 74 61 6E 63 65 73", true))
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

		private void CheckBoxProcessCheck(object sender, RoutedEventArgs e)
		{
			if (CbxSearchProcess.IsChecked != null && CbxSearchProcess.IsChecked == true)
			{
				_t.Enabled = true;
			}
		}

		private long BaseAddress = -1;
		private int TagCount = -1;

		public Dictionary<string, TagStruct> TagsList { get; set; } = new(); // and now we can convert it back because we just sort it elsewhere
		public SortedDictionary<string, GroupTagStruct> TagGroups { get; set; } = new();

		public async Task ArrayOfByteScanAsync()
		{
			try
			{
				long? aobScan = (await M.AoBScan("74 61 67 20 69 6E 73 74 61 6E 63 65 73", true))
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

		// load tags from Mem
		private async void BtnLoadTags_Click(object sender, RoutedEventArgs e)
		{
			//hook_text.Text = "Opening process...";
		    bool reset = processSelector.hookProcess(M); 
			//System.Diagnostics.Debug.WriteLine(processSelector.selected);
			if (M.pHandle == IntPtr.Zero || processSelector.selected == false || loadedTags == false)
			{
				// Could not find the process
				hook_text.Text = "Cant find HaloInfinite.exe";
				hooked = false;
				loadedTags = false;
				TagsTree.Items.Clear();
				//return;
			}

			if (hooked == false || reset)
			{
				// Get the base address
				BaseAddress = M.ReadLong("HaloInfinite.exe+0x4879758");
				string validtest = M.ReadString(BaseAddress.ToString("X"));
				System.Diagnostics.Debug.WriteLine(M.ReadLong("HaloInfinite.exe+0x3D13E38"));
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

			if (BaseAddress != -1)
			{
				LoadTagsMem();
				if (hooked == true) // apparently we dont hook if we *have* the address :frown //This is to load the tags, we only load the tags if we are already hooked. // i put that comment when i was fixing it ya goober
				{

					Loadtags();

					Searchbox_TextChanged(null, null);

					System.Diagnostics.Debugger.Log(0, "DBGTIMING", "Done loading tags");
					if (TagsTree.Items.Count > 0)
					{
						loadedTags = true;
					}
				}
			}

		}

		public void LoadTagsMem()
		{
			if (TagCount != -1)
			{
				TagCount = -1;
				TagGroups.Clear();
				TagsList.Clear();
			}

			TagsTree.Items.Clear();

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

				// do the tag definitition
				if (!TagsList.ContainsKey(currentTag.ObjectId))
				{
					TagsList.Add(currentTag.ObjectId, currentTag);
				}
			}
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

		public void Loadtags()
		{
			// TagsTree
			for (int i = 0; i < TagGroups.Count; i++)
			{
				GroupTagStruct displayGroup = TagGroups.ElementAt(i).Value;

				TreeViewItem sortheader = new()
				{
					Header = displayGroup.TagGroupName + " (" + displayGroup.TagGroupDesc + ")",
					ToolTip = new TextBlock { Foreground = Brushes.Black, Text = displayGroup.TagGroupDefinitition }
				};

				displayGroup.TagCategory = sortheader;

				TagsTree.Items.Add(sortheader);

				TagGroups[TagGroups.ElementAt(i).Key] = displayGroup;
			}
			// var sortedList = TagsList.OrderBy(x => x.TagFullName).ToList();

			foreach (KeyValuePair<string, TagStruct> curr_tag in TagsList.OrderBy(key => key.Value.TagFullName))
			{
				TreeViewItem t = new();
				TagStruct tag = curr_tag.Value;
				TagGroups.TryGetValue(tag.TagGroup, out GroupTagStruct dictTagGroup);

				t.Header = "(" + tag.Datnum + ") " + convert_ID_to_tag_name(tag.ObjectId);

				t.Tag = curr_tag.Key; // our index to our tag

				//t.Tag = TagsList.FindIndex(x => x.ObjectId == tag.ObjectId); // yucky, this thing causes way too much latency // remember we do this up to 70000 times

				//t.MouseLeftButtonDown += new MouseButtonEventHandler(Select_Tag_click);
				t.Selected += Select_Tag_click;

				dictTagGroup.TagCategory.Items.Add(t);
			}
		}

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
					// Set the tag as active
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
								bool? found = false; // used for debugging
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

		private void Select_Tag_click(object sender, RoutedEventArgs e)
		{
			TreeViewItem? item = sender as TreeViewItem;
			CreateTagEditorTabByTagIndex(item.Tag.ToString());
		}

		// list of changes to ammend to the memory when we phit the poke button
		public Dictionary<long, KeyValuePair<string, string>> Pokelist = new();

		// to keep track of the UI elements we're gonna use a dictionary, will probably be better
		public Dictionary<long, TagChangesBlock> UIpokelist = new();

		// type (TagrefGroup, TagrefTag)
		// address,
		// WARNING: Please note if something calls this function it wont be replicated
		// in the future when saving or netcode is added!!!
		public void AddPokeChange(long offset, string type, string value)
		{


			// hmm we need to change this so we either update or add a new UI element
			Pokelist[offset] = new KeyValuePair<string, string>(type, value);

			// there we go, now we aren't touching the pokelist code
			if (UIpokelist.ContainsKey(offset))
			{
				TagChangesBlock updateElement = UIpokelist[offset];
				updateElement.address.Text = "0x" + offset.ToString("X");
				updateElement.type.Text = type;
				updateElement.value.Text = value;
			}
			else
			{
				TagChangesBlock newBlock = new() {
					address = { Text = "0x" + offset.ToString("X") },
					type = { Text = type },
					value = { Text = value },
				};

				changes_panel.Children.Add(newBlock);
				UIpokelist.Add(offset, newBlock);
			}

			change_text.Text = Pokelist.Count + " changes queued";
		}

		public void AddPokeChange(TagEditorDefinition def, string value)
		{
			// Hmm we need to change this so we either update or add a new UI element
			Pokelist[def.MemoryAddress] = new KeyValuePair<string, string>(def.MemoryType, value);

			// there we go, now we aren't touching the pokelist code
			if (UIpokelist.ContainsKey(def.MemoryAddress))
			{
				TagChangesBlock updateElement = UIpokelist[def.MemoryAddress];
				updateElement.address.Text = "0x" + def.MemoryAddress.ToString("X");
				updateElement.type.Text = def.MemoryType;
				updateElement.value.Text = value;
				updateElement.tagSource.Text = def.TagStruct.TagFile + " + " + def.GetTagOffset();
			}
			else
			{
				TagChangesBlock newBlock = new()
				{
					address = { Text = "0x" + def.MemoryAddress.ToString("X") },
					type = { Text = def.MemoryType },
					value = { Text = value },
					tagSource = { Text = def.TagStruct.TagFile + " + " + def.GetTagOffset() }
				};

				changes_panel.Children.Add(newBlock);
				UIpokelist.Add(def.MemoryAddress, newBlock);
			}

			change_text.Text = Pokelist.Count + " changes queued";
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

			return "wtf does this even do";
		}


		// POKE OUR CHANGES LETSGOOOO
		private void BtnPokeChanges_Click(object sender, RoutedEventArgs e)
		{
			foreach (KeyValuePair<long, KeyValuePair<string, string>> pair in Pokelist)
			{
				long address = pair.Key;
				string type = pair.Value.Key;
				string value = pair.Value.Value;

				switch (type)
				{
					case "4Byte":
						M.WriteMemory(address.ToString("X"), "int", value);
						break;
					case "2Byte":
						M.WriteMemory(address.ToString("X"), "2bytes", value);
						break;
					case "Flags":
						M.WriteMemory(address.ToString("X"), "byte", Convert.ToByte(value).ToString("X"));
						break;
					case "Float":
						M.WriteMemory(address.ToString("X"), "float", value);
						break;

					case "Pointer":
						string? willThisWork = new System.ComponentModel.Int64Converter().ConvertFromString(value).ToString();
						M.WriteMemory(address.ToString("X"), "long", willThisWork); // apparently it does
						break;

					case "String":
						M.WriteMemory(address.ToString("X"), "string", value + "\0");
						break;

					case "TagrefGroup":
						M.WriteMemory(address.ToString("X"), "string", ReverseString(value));
						break;

					case "TagrefTag":
						string temp = Regex.Replace(value, @"(.{2})", "$1 ");
						temp = temp.TrimEnd();
						M.WriteMemory(address.ToString("X"), "bytes", temp);
						break;

					case "mmr3Hash":
						string temp2 = Regex.Replace(value, @"(.{2})", "$1 ");
						temp = temp2.TrimEnd();
						M.WriteMemory(address.ToString("X"), "bytes", temp);
						break;
				}
			}

			poke_text.Text = Pokelist.Count + " changes poked";

			changes_panel.Children.Clear();
			Pokelist.Clear();
			UIpokelist.Clear();
			change_text.Text = Pokelist.Count + " changes queued";
		}

		private void BtnClearQueue_Click(object sender, RoutedEventArgs e)
		{
			changes_panel.Children.Clear();
			Pokelist.Clear();
			UIpokelist.Clear();
			change_text.Text = Pokelist.Count + " changes queued";
		}

		private void DockManager_DocumentClosing(object sender, AvalonDock.DocumentClosingEventArgs e)
		{
			// On tag window closing.
			UpdateLayout();

			GC.Collect(3, GCCollectionMode.Forced);
		}

		private void BtnShowHideQueue_Click(object sender, RoutedEventArgs e)
		{
			Button? btn = (Button) sender;

			changes_panel_container.Visibility = changes_panel_container.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
			btn.Content =
				changes_panel_container.Visibility == Visibility.Visible
				? "Hide Queue"
				: "Show Queue";
		}

		/* 4Byte
         * Float
         * TagRef
         * Pointer
         * Tagblock
         * String
         * TagrefGroup
         * TagrefTag
         */

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
			string[] supportedTags = Halo.TagObjects.TagLayouts.Tags.Keys.ToArray();
			string search = Searchbox.Text;
			foreach (TreeViewItem? tv in TagsTree.Items)
			{
				bool isSupportedTag = supportedTags.Contains(tv.Header.ToString().Split(' ')[0].ToLower());

				// Ignore tags that are not implemented
				if ((bool) cbxFilterOnlyMapped.IsChecked && !isSupportedTag)
				{
					tv.Visibility = Visibility.Collapsed;
					continue;
				}

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

		private void cbxFilterOnlyMapped_Changed(object sender, RoutedEventArgs e)
		{
			string[] supportedTags = Halo.TagObjects.TagLayouts.Tags.Keys.ToArray();
			if (Searchbox != null)
			{
				string search = Searchbox.Text;

				// If we have a filter just call the search function
				if (!string.IsNullOrEmpty(search))
				{
					Searchbox_TextChanged(null, null);
					return;
				}
				if (TagsTree != null)
				{
					foreach (TreeViewItem tv in TagsTree.Items)
					{
						// Ignore tags that are not implemented
						if ((bool) cbxFilterOnlyMapped.IsChecked)
						{
							tv.Visibility = supportedTags.Contains(tv.Header.ToString().Split(' ')[0].ToLower()) ? Visibility.Visible : Visibility.Collapsed;
						}
						else
						{
							tv.Visibility = Visibility.Visible;
						}
					}
				}
			}
		}
		public void ClickExit(object sender, RoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		public void SettingsControl(object sender, RoutedEventArgs e)
		{
			SettingsControl win2 = new();
			win2.Show();
		}

		public ProcessSelector GetProcessSelector()
		{
			return processSelector;
		}

		public void AnyProcess(object sender, RoutedEventArgs e)
		{
			CbxAnyProcess.IsChecked = true;
			CbxSpecificProcess.IsChecked = false;

			processSelector.SelectedProcess = null;
			CbxSpecificProcess.IsChecked = false;
		}

		public void SpecificProcess(object sender, RoutedEventArgs e)
		{
			CbxSpecificProcess.IsChecked = true;
			CbxAnyProcess.IsChecked = false;
		}

		public void UnloadTags(object sender, RoutedEventArgs e)
		{
			TagsTree.Items.Clear();
			//Need to unload memory items somehow here too!
		}
		public void GetAllMethods()
		{
			Type myType = (typeof(MainWindow));
			// Get the public methods.
			MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			Console.WriteLine("\nThe number of public methods is {0}.", myArrayMethodInfo.Length);
			// Add all public methods to menu.
			DisplayMethodInfo(myArrayMethodInfo);
			// Add all non-public methods to array.
			MethodInfo[] myArrayMethodInfo1 = myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			// Add all non-public methods to menu.
			DisplayMethodInfo(myArrayMethodInfo1);
		}


		public void DisplayMethodInfo(MethodInfo[] myArrayMethodInfo)
		{
			// Display information for all methods.
			for (int i = 0; i < myArrayMethodInfo.Length; i++)
			{
				MethodInfo myMethodInfo = (MethodInfo) myArrayMethodInfo[i];
				//Console.WriteLine("\nThe name of the method is {0}.", myMethodInfo.Name);
				MenuItem methods = new MenuItem();
				MenuItem methodToAdd = (MenuItem) DebugMenu.Items[1];
				methods.Header = myMethodInfo.Name;
				methods.Click += CallMethod;
				methodToAdd.Items.Add(methods);
			}
		}

		public void CallMethod(object sender, RoutedEventArgs e)
		{
			//Code that will call the specified method.
			MenuItem? MI = sender as MenuItem;
			if (MI != null)
			{
				try
				{
					Type mainType = (typeof(MainWindow));
					string? clickedMethod = MI.Header.ToString();
					System.Diagnostics.Debug.WriteLine(clickedMethod);
					MethodInfo? method = mainType.GetMethod(clickedMethod);
					if (method != null)
					{
						int paramCount = method.GetParameters().Length;
						method.Invoke(this, null);
					}
				}
				catch (Exception)
				{
					System.Diagnostics.Debug.WriteLine("Invalid parameter count. Consider calling a method with no parameters.");
				}
			}
	}
	}
}