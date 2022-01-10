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

namespace InfiniteRuntimeTagViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		/* ###### THINGS TO BE FIXED (which i will get around to eventually)
		
		### BUG FIXES
		tag evaluation checks - when poking we should check to see that the tag is still valid

		### QOL
		add colapse to tagblocks
		reload tag button
		auto poke changes as tags are loaded/reloaded
		revert changes list
		only change UI tags loaded things that need to be added/removed to save perfromance

		### ERROR CATCHING
		extra exception handling for tags - where we check for tag validity when poking?

		### TAG STRUCTS
		.style flags are invis
		.jamd tag broke
		.hlmt tag needs _39 mapped
		.phmo tag kind broke
		char ' gets turned into the funny unknown char. ex. don't -> dont^t (i dont actually have the symbol so thats not a good example)


		*/

		public delegate void HookAndLoadDelagate();
		public delegate void LoadTagsDelagate();
		private readonly System.Timers.Timer _t;
		public Mem M = new();

		public MainWindow()
		{
			InitializeComponent();
			//GetAllMethods();
			StateChanged += MainWindowStateChangeRaised;
			_t = new System.Timers.Timer();
			_t.Elapsed += OnTimedEvent;
			_t.Interval = 2000;
			_t.AutoReset = true;
			inhale_tagnames();
			SettingsControl settings = new();
			settings.SetGeneralSettingsFromConfig();
			settings.Close();
		}

		private async Task HookProcessAsync()
		{
			bool reset = processSelector.hookProcess(M);
			if (M.pHandle == IntPtr.Zero || processSelector.selected == false || loadedTags == false)
			{
				// Could not find the process
				hook_text.Text = "Cant find HaloInfinite.exe";
				hooked = false;
				loadedTags = false;
				TagsTree.Items.Clear();
			}

			if (!hooked || reset)
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
		}

		public void HookAndLoad()
		{
			_ = HookProcessAsync();
			if (BaseAddress != -1 )
			{
				Dispatcher.BeginInvoke(new Action(async () =>
				{
					await LoadTagsMem();
				}), DispatcherPriority.SystemIdle);
				
				if (hooked == true)
				{
					Searchbox_TextChanged(null, null);

					System.Diagnostics.Debugger.Log(0, "DBGTIMING", "Done loading tags");

				}
			}
		}


		public bool loadedTags = false;
		public bool hooked = false;
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				_ = HookProcessAsync();
				if (CbxAutoLoadTags.IsChecked && !loadedTags)
				{
					_ = LoadTagsMem();
				}
				if (CbxAutoPokeChanges.IsChecked)
				{
					PokeChanges();
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
			if (CbxSearchProcess.IsChecked == true)
			{
				_t.Enabled = true;
			}
		}

		private long BaseAddress = -1;
		private int TagCount = -1;

		public Dictionary<string, TagStruct> TagsList { get; set; } = new(); // and now we can convert it back because we just sort it elsewhere
		public SortedDictionary<string, GroupTagStruct> TagGroups { get; set; } = new();

		// load tags from Mem
		private void BtnLoadTags_Click(object sender, RoutedEventArgs e)
		{
			HookAndLoad();
		}

		// instead of using the other method i made a new one because the last one yucky,
		public bool SlientHookAndLoad(bool load_tags_too)
		{
			_ = HookProcessAsync();
			if (BaseAddress != -1 && BaseAddress != 0)
			{
				if (load_tags_too)
				{
					SlientLoadTagsMem();

					if (hooked == true)
					{
						Searchbox_TextChanged(null, null);
					}
				}
				return true;
			}
			return false;
		}
		public void SlientLoadTagsMem()
		{
			// silent denotes that we aren't loading anything into the UI // which slices off a significant load time
			// which we'll be using this mainly for our mod loader because you really dont need to reload the goddamn ui everytime
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

				// do the tag definitition
				if (!TagsList.ContainsKey(currentTag.ObjectId))
				{
					TagsList.Add(currentTag.ObjectId, currentTag);
				}
			}
		}

		public async Task LoadTagsMem()
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

				// do the tag definitition
				if (!TagsList.ContainsKey(currentTag.ObjectId))
				{
					TagsList.Add(currentTag.ObjectId, currentTag);
				}
			}
			await Loadtags();
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
		public async Task Loadtags()
		{
			TagsTree.Items.Clear();
			// TagsTree
			loadedTags = true;
			for (int i = 0; i < TagGroups.Count; i++)
			{
				int tagsPercent = (int) (i / (double) TagGroups.Count * 100);
				hook_text.Text = "Loading Tags..." + tagsPercent + "%";
				await Task.Delay(1); // overall this causes aprox 500 ms delay. that is half a second
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
			hook_text.Text = "Loaded Tags";
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
			if (TagsTree.Items.Count < 1)
			{
				loadedTags = false;
			}
			//had to do this cause for whatever reason the multithreading prevented it from actually filtering the tags
			cbxFilterOnlyMapped.IsChecked = false;
			cbxFilterOnlyMapped.IsChecked = true;

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
		// i think it goes: address, type, value
		public Dictionary<string, KeyValuePair<string, string>> Pokelist = new();

		// to keep track of the UI elements we're gonna use a dictionary, will probably be better
		public Dictionary<string, TagChangesBlock> UIpokelist = new();

		// type (TagrefGroup, TagrefTag)
		// address,
		// WARNING: Please note if something calls this function it wont be replicated
		// in the future when saving or netcode is added!!!
		//public void AddPokeChange(string offset, string type, string value)
		//{


		//	// hmm we need to change this so we either update or add a new UI element
		//	Pokelist[offset] = new KeyValuePair<string, string>(type, value);

		//	// there we go, now we aren't touching the pokelist code
		//	if (UIpokelist.ContainsKey(offset))
		//	{
		//		TagChangesBlock updateElement = UIpokelist[offset];
		//		updateElement.address.Text = offset;
		//		updateElement.type.Text = type;
		//		updateElement.value.Text = value;
		//	}
		//	else
		//	{
		//		TagChangesBlock newBlock = new() {
		//			address = { Text = offset },
		//			type = { Text = type },
		//			value = { Text = value },
		//		};

		//		changes_panel.Children.Add(newBlock);
		//		UIpokelist.Add(offset, newBlock);
		//	}

		//	change_text.Text = Pokelist.Count + " changes queued";
		//}

		private void Save_pokes(object sender, RoutedEventArgs e)
		{
			if (loadedTags)
			{
				if (Pokelist.Count > 0)
				{
					var sfd = new Microsoft.Win32.SaveFileDialog
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
						foreach (var k in Pokelist)
						{
							big_ol_poke_dump+=k.Key + ";" + k.Value.Key + ";" + k.Value.Value + "\r\n";
						}
						Savewindow sw = new();
						sw.Show();
						sw.main = this;
						sw.ill_take_it_from_here_mainwindow(filename, big_ol_poke_dump);

						poke_text.Text = Pokelist.Count + " Pokes Saved!";
					}

				}
				else
				{
					poke_text.Text = "no pokes to save";
				}
			}
			else
			{
				poke_text.Text = "You MUST 'load' first";
			}

		}

		private void Open_pokes(object sender, RoutedEventArgs e)
		{
			if (!loadedTags)
			{
				HookAndLoad();
			}
			// Create OpenFileDialog 
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension 
			dlg.DefaultExt = ".irtv";
			dlg.Filter = "IRTV Files (*.irtv)|*.irtv";

			// Display OpenFileDialog by calling ShowDialog method 
			bool? result = dlg.ShowDialog();

			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				recieve_file_to_inhalo_pokes(dlg.FileName);
				string fullFileName = dlg.FileName;
				string fileNameWithExt = Path.GetFileName(fullFileName);
				string target_folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IRTV";
				if (!Directory.Exists(target_folder))
					Directory.CreateDirectory(target_folder);
				string destPath = Path.Combine(target_folder, fileNameWithExt);
				if (File.Exists(destPath))
					File.Delete(destPath);
				File.Copy(dlg.FileName, destPath);
			}
		}
		public void recieve_file_to_inhalo_pokes(string filename)
		{
			int prev = 0;
			int fails = 0;
			// Open document 
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
			// nothing could cause an issue here
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

		//var lines = File.ReadLines(filename);
		//              foreach (var line in lines)
		//              {
		//                  string[] parts = line.Split(":");

		//Hashedstrings[parts[1]] = parts[0];
		//              }

		public void AddPokeChange(TagEditorDefinition def, string value)
		{
			// Hmm we need to change this so we either update or add a new UI element

			//used things
			// offset override
			// memory type
			// value
			// tagname
			Pokelist[def.OffsetOverride] = new KeyValuePair<string, string>(def.MemoryType, value);

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

			change_text.Text = Pokelist.Count + " changes queued";
			if (mwidow != null)
			{
				mwidow.test_changes.Text = Pokelist.Count + " changes queued";
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

			return "wtf does this even do";
		}
		
		public void PokeChanges()
		{
			int fails = 0;
			int pokes = 0;
			foreach (KeyValuePair<string, KeyValuePair<string, string>> pair in Pokelist)
			{
				//pokesingle(pair.Key, pair.Value.Key, pair.Value.Value);
				pokes++;
				bool failed = false;
				string do_the_thing = SUSSY_BALLS_2(pair.Key);
				if (do_the_thing != "")
				{
					if (!pokesingle(do_the_thing, pair.Value.Key, pair.Value.Value))
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
				if (failed)
				{
					UIpokelist[pair.Key].bordercolor.BorderBrush = new SolidColorBrush(Colors.Red); 
				}
				else
				{
					UIpokelist[pair.Key].bordercolor.BorderBrush = null;
				}
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

			change_text.Text = Pokelist.Count + " changes queued";
		}


		// POKE OUR CHANGES LETSGOOOO
		private void BtnPokeChanges_Click(object sender, RoutedEventArgs e)
		{
			PokeChanges();
		}
		public void tagchangesblock_fetchdata_by_ID(TagChangesBlock target)
		{
			
			KeyValuePair<string, string> pair = Pokelist[target.sig_address_path];
			//pokesingle(target.sig_address_ID, pair.Key, pair.Value);
			//SUSSY_BALLS_2
			string do_the_thing = SUSSY_BALLS_2(target.sig_address_path);
			bool failed = false;
			if (do_the_thing != "")
			{
				if(!pokesingle(do_the_thing, pair.Key, pair.Value))
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

		public bool pokesingle(string address, string type, string value)
		{
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
							pokesingle(address, type, read);
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
							return false; }
						return true;
				}
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
						output = M.ReadInt(address).ToString(); // (+entry.Key?) lmao, no wonder why it wasn't working
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
						output = ReverseString(M.ReadString(address, "", 4));
					}
					catch { }
					return output;
				case "TagrefTag":
					try
					{
						output = BitConverter.ToString(M.ReadBytes(address, 4)).Replace("-", string.Empty);
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
			// exception handling required here
			if (TagsList.Keys.Contains(p[0]))
			{
				var tag_thing = TagsList[p[0]];
				return "0x" + (poop += tag_thing.TagData).ToString("X") + ((joined == "") ? "" : "," + joined);

			}
			return "";
		}
		public void Update_poke_value(TagChangesBlock target, string new_value)
		{
			KeyValuePair<string, string> pair = Pokelist[target.sig_address_path];
			Pokelist[target.sig_address_path] = new KeyValuePair<string, string>(pair.Key, new_value);
			poke_text.Text = "Poke Value Updated";

		}

		public void clearsingle(TagChangesBlock target)
		{
			Pokelist.Remove(target.sig_address_path);
			UIpokelist.Remove(target.sig_address_path);
			changes_panel.Children.Remove(target);
			change_text.Text = Pokelist.Count + " changes queued";
		}

		private void BtnClearQueue_Click(object sender, RoutedEventArgs e)
		{
			clear_pokes_list();
		}
		public void clear_pokes_list()
		{
			changes_panel.Children.Clear();
			Pokelist.Clear();
			UIpokelist.Clear();
			change_text.Text = Pokelist.Count + " changes queued";
			if (mwidow != null)
			{
				mwidow.test_changes.Text = Pokelist.Count + " changes queued";
			}
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

			changes_panel_container.Visibility = changes_panel_container.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
			btn.Content =
				changes_panel_container.Visibility == Visibility.Visible
				? "Hide Queue"
				: "Show Queue";
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
				bool isSupportedTag = true; // supportedTags.Contains(tv.Header.ToString().Split(' ')[0].ToLower());

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
			//string[] supportedTags = Halo.TagObjects.TagLayouts.Tags.Keys.ToArray();
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
							tv.Visibility = Visibility.Visible;
						}
						else
						{
							tv.Visibility = Visibility.Visible;
						}
					}
				}
			}
		}

		#region MenuCommands
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
			loadedTags = false;
			//Need to unload memory items somehow here too!
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
		public ModWindow mwidow;
		private void MenuItem_Click(object sender, RoutedEventArgs e)
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
				mwidow.Focus();
			}
		}
	}
}