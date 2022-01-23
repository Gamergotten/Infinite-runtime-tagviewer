using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InfiniteRuntimeTagViewer.Halo.TagObjects;
using InfiniteRuntimeTagViewer.Interface.Windows;
using AvalonDock.Controls;
using AvalonDock.Layout;
using Memory;
using System.Linq;

using static InfiniteRuntimeTagViewer.MainWindow;
using InfiniteRuntimeTagViewer.Halo;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
	/// <summary>
	/// Interaction logic for TagEditorControl.xaml
	/// </summary>
	public partial class TagEditorControl : IDisposable
	{

		private MainWindow _mainWindow;
		private readonly Mem _m;

		public LayoutDocument? LayoutDocument { get; internal set; }
		public Button? TheLastTagrefButtonWePressed { get; set; } // since we did it for the window why not also do it for the button

		private bool disposedValue;

		public TagEditorControl(MainWindow mw)
		{
			_mainWindow = mw;
			_m = _mainWindow.M;

			InitializeComponent();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~TagEditorControl()
		{
			int debugMeme = 0;
		}

		public void Inhale_tag(string tagID) // i love dictionaries
		{
			TagStruct loadingTag = _mainWindow.TagsList[tagID];
			Tagname_text.Text = _mainWindow.convert_ID_to_tag_name(loadingTag.ObjectId);
			tagID_text.Text = "ID: " + loadingTag.ObjectId;
			tagdatnum_text.Text = "Datnum: " + loadingTag.Datnum;
			tagdata_text.Text = "Tag data address: 0x" + loadingTag.TagData.ToString("X");

			tagfilter_text.Text = "";

			tagview_panels.Children.Clear();

			// OK, now we do a proper check to see if this tag is loaded, finally looked into this

			try // never done this before and i hope im doing it terribly wrong
			{
				// pointer check
				TagValueBlock p_block = new() { HorizontalAlignment = HorizontalAlignment.Left };
				p_block.value_type.Text = "Pointer check";
				p_block.value.Text = _m.ReadLong((loadingTag.TagData).ToString("X")).ToString("X");
				tagview_panels.Children.Add(p_block);

				// ID check
				TagValueBlock id_block = new() { HorizontalAlignment = HorizontalAlignment.Left };
				id_block.value_type.Text = "ID check";
				string checked_ID = BitConverter.ToString(_m.ReadBytes((loadingTag.TagData + 8).ToString("X"), 4)).Replace("-", string.Empty);
				id_block.value.Text = checked_ID;
				tagview_panels.Children.Add(id_block);

				// Datnum check
				TagValueBlock dat_block = new() { HorizontalAlignment = HorizontalAlignment.Left };
				dat_block.value_type.Text = "Datnum check";
				string checked_datnum = BitConverter.ToString(_m.ReadBytes((loadingTag.TagData + 12).ToString("X"), 4)).Replace("-", string.Empty);
				dat_block.value.Text = checked_datnum;
				tagview_panels.Children.Add(dat_block);

				if (checked_ID != loadingTag.ObjectId || checked_datnum != loadingTag.Datnum)
				{
					TextBox tb1 = new TextBox { Text = "Datnum/ID mismatch; Tag appears to be unloaded, meaning it may not be active on the map, else try reloading the tags" };
					tagview_panels.Children.Add(tb1);
					return;
				}

				//if (TagLayouts.Tags.ContainsKey(loadingTag.TagGroup))
				//{
					Dictionary<long, TagLayouts.C> tags = TagLayouts.Tags(loadingTag.TagGroup);
					readTagsAndCreateControls(loadingTag, 0, tags, loadingTag.TagData, tagview_panels, tagID+":");
				//}
				//else
				//{
				//	TextBox tb = new TextBox { Text = "This tag isn't mapped out ):" };
				//	tagview_panels.Children.Add(tb);
				//}
			}
			catch
			{
				TextBox tb = new TextBox { Text = "ran into an oopsie woopsie, this tag is probably broken/unloaded right now" };
				tagview_panels.Children.Add(tb);
			}


		}

		// hmm we need a system that reads the pointer and adds it
		// also, we need to beable to read multiple tag things but i may put that on hold
		public void recall_blockloop(TagStruct tagStruct, long tagOffset, KeyValuePair<long, TagLayouts.C> entry, long loadingTag, StackPanel parentpanel, string abso_whatever_it_was)
		{
			parentpanel.Children.Clear();
			if (entry.Value.B != null)
			{
				try
				{
					readTagsAndCreateControls(tagStruct, tagOffset, entry.Value.B, loadingTag, parentpanel, abso_whatever_it_was);
				}
				catch
				{
					TextBox tb = new TextBox { Text = "this tagblock is fucked uwu" };
					parentpanel.Children.Add(tb);
				}
			}
		}

		// for text boxes
		private void value_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox? tb = sender as TextBox;
			TagEditorDefinition ted = tb.Tag as TagEditorDefinition;
			System.Diagnostics.Debug.Assert(ted != null);

			_mainWindow.AddPokeChange(ted, tb.Text);

			// string[] s = tb.Tag.ToString().Split(":");
			// _mainWindow.AddPokeChange(long.Parse(s[0]), s[1], tb.Text);
		}

		// for tag group
		private void taggroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox? cb = sender as ComboBox;
			TagEditorDefinition ted = cb.Tag as TagEditorDefinition;
			System.Diagnostics.Debug.Assert(ted != null);
			string new_group = cb.SelectedValue.ToString();
			if (new_group != "Null")
			{
				ted.MemoryType = "TagrefGroup";
				_mainWindow.AddPokeChange(ted, new_group);
			}
			else
			{
				ted.MemoryType = "4Byte";
				_mainWindow.AddPokeChange(ted, "-1");

			}
			//_mainWindow.AddPokeChange(long.Parse(s: cb.Tag.ToString()), "TagrefGroup", value: cb.SelectedValue.ToString());

			// What the actual fuck is all of this? // it wouldnt let me get the owner UI element so i did what i needed
			Grid? td = cb.Parent as Grid;
			Button? b = td.Children[1] as Button;
			TED_TagRefGroup? btnTed = b.Tag as TED_TagRefGroup;
			btnTed.TagGroup = cb.SelectedValue.ToString();

			//string[] s = b.Tag.ToString().Split(":");
			//b.Tag = s[0] + ":" + cb.SelectedValue.ToString();

			// THAT WAS PROBABLY THE MOST DODGY THING IVE EVER DONE WTFFFF
		}

		public void Gotobutton(object sender, RoutedEventArgs e)
		{
			Button? b = sender as Button;
			string? sTagId = b.Tag.ToString();
			//int iTagId = int.Parse(sTagId);

			//if (sTagId != -1) // i forsee this becoming a problematic fix
				_mainWindow.CreateTagEditorTabByTagIndex(sTagId);

		}

		private DependencyObject GetTopLevelControl(DependencyObject control)
		{
			DependencyObject? tmp = control;
			DependencyObject? parent = null;
			while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
			{
				parent = tmp;
			}
			return parent;
		}

		private T? GetTopLevelControlOfType<T>(DependencyObject control) where T : DependencyObject
		{
			DependencyObject? tmp = control;
			T? target = default(T);

			while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
			{
				// System.Diagnostics.Debug.WriteLine("- " + tmp.GetType());
				if (tmp is T dependencyObject)
				{
					target = dependencyObject;
				}
			}

			return target;
		}

		TagRefDropdown? trd;
		private void TagRefButton(object sender, RoutedEventArgs e)
		{
			Button? b = sender as Button;
			TED_TagRefGroup ted = b.Tag as TED_TagRefGroup;
			System.Diagnostics.Debug.Assert(ted != null);

			trd = new TagRefDropdown();
			double trdWidth = trd.Width = b.ActualWidth + 116;
			double trdHeight = trd.Height = 400;

			//
			{
				Window? controlsWindow = GetTopLevelControlOfType<Window>((DependencyObject) sender);

				// There seems to be no way to get the window, so we will find the LayoutDocumentPaneGroupControl
				// instead and we can use that to crawl backwards through the Windows to find it.
				LayoutDocumentPaneGroupControl? dockingPaneGroup = GetTopLevelControlOfType<LayoutDocumentPaneGroupControl>(b);
				bool foundDockingWindow = false;

				// Handle if we have a popped out window.
				if (controlsWindow == null && dockingPaneGroup != null)
				{
					// Check if we can figure out where this docking pane is located
					foreach (Window appWindow in Application.Current.Windows)
					{
						LayoutDocumentFloatingWindowControl? floatingWind = appWindow as LayoutDocumentFloatingWindowControl;
						if (floatingWind == null)
						{
							continue;
						}

						// Make sure we have a FloatingWindowContentHost
						object? floatingWindContHost = floatingWind.Content; // AvalonDock.Controls.LayoutFloatingWindowControl.FloatingWindowContentHost
						if (floatingWindContHost == null && floatingWindContHost.GetType().Name != "FloatingWindowContentHost")
						{
							continue;
						}

						// Get the public property "Content", we cant get this normally because this
						// is a internal / sealed class. We need to use reflection to get our
						// grubby mitts on it.
						System.Reflection.PropertyInfo? prop = floatingWindContHost.GetType().GetProperty("Content");
						LayoutDocumentPaneGroupControl? contentResult = prop.GetValue(floatingWindContHost) as LayoutDocumentPaneGroupControl;

						// Check if this is our DockingPanelGroup
						if (contentResult == dockingPaneGroup)
						{
							// Get the control's point relative to the parent window.
							Point relativeControlLocation = b.TranslatePoint(new Point(0, b.ActualHeight), appWindow);

							// Set the location to the parent window + control location
							// This sets it to just above the control, by adding the height by a factor of 1.5 it seems
							// to be an almost fit.
							trd.Left = appWindow.GetWindowLeft() + relativeControlLocation.X;
							trd.Top = appWindow.GetWindowTop() + relativeControlLocation.Y;

							foundDockingWindow = true;
							break;
						}
					}
				}

				// If we can get the topmost window, use a precise approach
				if (controlsWindow != null)
				{
					// Get the control's point relative to the parent window.
					Point relativeControlLocation = b.TranslatePoint(new Point(0, b.ActualHeight), controlsWindow);

					// Set the location to the parent window + control location
					// This sets it to just above the control, by adding the height by a factor of 1.5 it seems
					// to be an almost fit.
					trd.Left = controlsWindow.GetWindowLeft() + relativeControlLocation.X;
					trd.Top = controlsWindow.GetWindowTop() + relativeControlLocation.Y;
				}
				else if (foundDockingWindow == false)
				{
					Point myButtonLocation = b.PointToScreen(new Point(0, 0));

					trd.Left = myButtonLocation.X - 8;
					trd.Top = myButtonLocation.Y + 1;
				}

			}

			trd.MainWindow = _mainWindow;
			TheLastTagrefButtonWePressed = b;
			List<string> big = new();
			List<string> targets = new();

			// Null type
			//TreeViewItem? tvi = new()
			//{
			//	Header = _mainWindow.convert_ID_to_tag_name("FFFFFFFF"),
			//	Tag = new TED_TagRefGroup(ted)
			//	{
			//		MemoryType = "TagrefTag",
			//		DatNum = "FFFFFFFF"
			//	} //s[0] + ":" + "FFFFFFFF"
			//};
			big.Add(_mainWindow.convert_ID_to_tag_name("FFFFFFFF"));
			targets.Add("FFFFFFFF");


			//trd.tag_select_panel.Items.Add(tvi);
			//tvi.Selected += update_tagref;
			bool filter_unmapped = _mainWindow.CbxFilterUnloaded.IsChecked;

			foreach (KeyValuePair<string, TagStruct> tg in _mainWindow.TagsList.OrderBy(key => key.Value.TagFullName)) // should probably store this instead of sorting everytime
			{
				if (tg.Value.TagGroup == ted.TagGroup)
				{
					if (filter_unmapped && tg.Value.unloaded == true)
					{
						continue;
					}
					big.Add(_mainWindow.convert_ID_to_tag_name(tg.Key));
					targets.Add(tg.Value.Datnum);
					//TreeViewItem? tvi2 = new()
					//{
					//	Header = _mainWindow.convert_ID_to_tag_name(tg.Key),
					//	//Tag = s[0] + ":" + tg.Datnum
					//	Tag = new TED_TagRefGroup(ted)
					//	{
					//		MemoryType = "TagrefTag",
					//		DatNum = tg.Value.Datnum
					//	}
					//};

					//trd.tag_select_panel.Items.Add(tvi2);
					//tvi2.Selected += update_tagref;
				}
			}
			trd.TheLastTagrefButtonWePressed = TheLastTagrefButtonWePressed;
			trd.ted = ted;
			trd.datnums = targets;
			trd.source = big;
			trd.tag_select_panel.ItemsSource = big;
			trd.Show();
			return;
		}

		// this is for our dropdown thingo for changing tag refs


		// had to adapt this to bealbe to read tagblocks and forgot to allow it to iterate through them *sigh* good enough for now
		// second time this happened, i wish i had more time in a day to get this all done
		private void readTagsAndCreateControls(TagStruct tagStruct, long startingTagOffset, Dictionary<long, TagLayouts.C> tagDefinitions, long address, StackPanel parentpanel, string absolute_address_chain)
		{
			// im hoping no one was using 'startingTagOffset' for anything spefic // repurposed
			KeyValuePair<long, TagLayouts.C> prevEntry;
			foreach (KeyValuePair<long, TagLayouts.C> entry in tagDefinitions)
			{
				entry.Value.MemoryAddress = address + entry.Key;
				entry.Value.AbsoluteTagOffset = absolute_address_chain +"," +(entry.Key + startingTagOffset);

				switch (entry.Value.T)
				{
					case "Comment":
						CommentBlock? vb0 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						parentpanel.Children.Add(vb0);
						vb0.comment.Text = entry.Value.N;
						break;
					case "FUNCTION":
						long functAddress = _m.ReadLong((address + entry.Key).ToString("X"));

						FunctionBlock? fb1 = new(this, tagStruct){HorizontalAlignment = HorizontalAlignment.Left};
						// BASE ADDRESS FOR FUNCTION
						fb1.tagblock_address.Text = "0x" + functAddress.ToString("X");
						fb1.tagblock_title.Text = entry.Value.N;
						string BytelengthCount = _m.ReadInt((address + entry.Key + 20).ToString("X")).ToString();
						fb1.tagblock_count.Text = BytelengthCount;
						parentpanel.Children.Add(fb1);
						fb1.tagblock_address.Tag = new TagEditorDefinition()
						{
							MemoryType = "Pointer",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};
						fb1.tagblock_address.TextChanged += value_TextChanged;
						fb1.tagblock_count.Tag = new TagEditorDefinition()
						{
							MemoryType = "4Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 20),
						};
						fb1.tagblock_count.TextChanged += value_TextChanged;

						try 
						{
							fb1.BlockAddress = functAddress;
							// next base for thingos
							if (functAddress != 0)
							{
								// 1st byte
								TagValueBlock? fb_vb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb1.value_type.Text = "Byte";
								fb_vb1.value.Text = _m.ReadByte((functAddress).ToString("X")).ToString();
								fb_vb1.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Byte",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",0"
								};
								fb_vb1.value.TextChanged += value_TextChanged;
								fb_vb1.value_name.Text = "Function Type1";
								fb1.dockpanel.Children.Add(fb_vb1);
								// 2nd byte
								TagValueBlock? fb_vb2 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb2.value_type.Text = "Byte";
								fb_vb2.value.Text = _m.ReadByte((functAddress + 1).ToString("X")).ToString();
								fb_vb2.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Byte",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",1"
								};
								fb_vb2.value.TextChanged += value_TextChanged;
								fb_vb2.value_name.Text = "Function Type2";
								fb1.dockpanel.Children.Add(fb_vb2);
								// 3rd byte
								TagValueBlock? fb_vb3 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb3.value_type.Text = "Byte";
								fb_vb3.value.Text = _m.ReadByte((functAddress + 2).ToString("X")).ToString();
								fb_vb3.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Byte",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",2"
								};
								fb_vb3.value.TextChanged += value_TextChanged;
								fb_vb3.value_name.Text = "Function Type3";
								fb1.dockpanel.Children.Add(fb_vb3);
								// 4th byte
								TagValueBlock? fb_vb4 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb4.value_type.Text = "Byte";
								fb_vb4.value.Text = _m.ReadByte((functAddress + 3).ToString("X")).ToString();
								fb_vb4.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Byte",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",3"
								};
								fb_vb4.value.TextChanged += value_TextChanged;
								fb_vb4.value_name.Text = "Function Type4";
								fb1.dockpanel.Children.Add(fb_vb4);
								// MIN FLOAT
								TagValueBlock? fb_vb5 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb5.value_type.Text = "Float";
								fb_vb5.value.Text = _m.ReadFloat((functAddress + 4).ToString("X"), "", false).ToString();
								fb_vb5.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Float",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",4"
								};
								fb_vb5.value.TextChanged += value_TextChanged;
								fb_vb5.value_name.Text = "Min";
								fb1.dockpanel.Children.Add(fb_vb5);
								// MAX FLOAT
								TagValueBlock? fb_vb6 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb6.value_type.Text = "Float";
								fb_vb6.value.Text = _m.ReadFloat((functAddress + 8).ToString("X"), "", false).ToString();
								fb_vb6.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Float",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",8"
								};
								fb_vb6.value.TextChanged += value_TextChanged;
								fb_vb6.value_name.Text = "Max";
								fb1.dockpanel.Children.Add(fb_vb6);
								// UNKNOWN1
								TagValueBlock? fb_vb7 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb7.value_type.Text = "Float";
								fb_vb7.value.Text = _m.ReadFloat((functAddress + 12).ToString("X"), "", false).ToString();
								fb_vb7.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Float",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",12"
								};
								fb_vb7.value.TextChanged += value_TextChanged;
								fb_vb7.value_name.Text = "Unknown1";
								fb1.dockpanel.Children.Add(fb_vb7);
								// UNKNOWN2
								TagValueBlock? fb_vb8 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb8.value_type.Text = "Float";
								fb_vb8.value.Text = _m.ReadFloat((functAddress + 16).ToString("X"), "", false).ToString();
								fb_vb8.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Float",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",16"
								};
								fb_vb8.value.TextChanged += value_TextChanged;
								fb_vb8.value_name.Text = "Unknown2";
								fb1.dockpanel.Children.Add(fb_vb8);
								// UNK MIN
								TagValueBlock? fb_vb9 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb9.value_type.Text = "Float";
								fb_vb9.value.Text = _m.ReadFloat((functAddress + 20).ToString("X"), "", false).ToString();
								fb_vb9.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Float",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",20"
								};
								fb_vb9.value.TextChanged += value_TextChanged;
								fb_vb9.value_name.Text = "Unk Min";
								fb1.dockpanel.Children.Add(fb_vb9);
								// UNK MAX
								TagValueBlock? fb_vb10 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb10.value_type.Text = "Float";
								fb_vb10.value.Text = _m.ReadFloat((functAddress + 24).ToString("X"), "", false).ToString();
								fb_vb10.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "Float",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",24"
								};
								fb_vb10.value.TextChanged += value_TextChanged;
								fb_vb10.value_name.Text = "Unk Max";
								fb1.dockpanel.Children.Add(fb_vb10);
								// leftover bytes -- used for the curvature information
								int leftoverbytes = _m.ReadInt((functAddress + 28).ToString("X"));
								//
								TagValueBlock? fb_vb11 = new() { HorizontalAlignment = HorizontalAlignment.Left };
								fb_vb11.value_type.Text = "4Byte";
								fb_vb11.value.Text = leftoverbytes.ToString();
								fb_vb11.value.Tag = new TagEditorDefinition()
								{
									MemoryType = "4Byte",
									TagDef = entry.Value,
									TagStruct = tagStruct,
									OffsetOverride = entry.Value.AbsoluteTagOffset + ",28"
								};
								fb_vb11.value.TextChanged += value_TextChanged;
								fb_vb11.value_name.Text = "Curvature Bytecount";
								fb1.dockpanel.Children.Add(fb_vb11);
								// write the rest of the junk to here
								if (leftoverbytes > 0)
								{
									TagValueBlock? fb_vb12 = new() { HorizontalAlignment = HorizontalAlignment.Left };
									fb_vb12.value_type.Text = "Bytes or something";
									fb_vb12.value.Text = BitConverter.ToString(_m.ReadBytes((address + entry.Key).ToString("X"), leftoverbytes)).Replace("-", string.Empty);
									fb_vb12.value.Tag = new TagEditorDefinition()
									{
										MemoryType = "mmr3Hash",
										TagDef = entry.Value,
										TagStruct = tagStruct,
										OffsetOverride = entry.Value.AbsoluteTagOffset + ",32"
									};
									fb_vb12.value.TextChanged += value_TextChanged;
									fb_vb12.value_name.Text = "Curvature Bytes";
									fb1.dockpanel.Children.Add(fb_vb12);
								}
								else
								{
									CommentBlock? vb99 = new() { HorizontalAlignment = HorizontalAlignment.Left };
									fb1.dockpanel.Children.Add(vb99);
									vb99.comment.Text = "no curvature";
								}
							}
						}
						catch
						{
							CommentBlock? vb99 = new() { HorizontalAlignment = HorizontalAlignment.Left };
							fb1.dockpanel.Children.Add(vb99);
							vb99.comment.Text = "this is what happens when i only add partial support for things";
						}


						break;
					case "EnumGroup":
						// make sure we got a enumgroup def
						if (!(entry.Value is TagLayouts.EnumGroup))
						{
							continue;
						}
						TagLayouts.EnumGroup? fg3 = entry.Value as TagLayouts.EnumGroup;
						EnumBlock eb1 = new EnumBlock() { HorizontalAlignment = HorizontalAlignment.Left };

						foreach (KeyValuePair<int, string> gvsdahb in fg3.STR)
						{
							ComboBoxItem cbi = new() { Content=gvsdahb.Value };
							eb1.enums.Items.Add(cbi);
						}

						if (fg3.A == 1)
						{
							int test_this = _m.ReadByte((address + entry.Key).ToString("X"));
							if (eb1.enums.Items.Count >= test_this)
							{
								eb1.enums.SelectedIndex = test_this;
							}
							else
							{
								TextBox tb = new TextBox { Text = "the enum below is broken :(" };
								parentpanel.Children.Add(tb);
							}
							eb1.ValueDefinition = new TagEditorDefinition()
							{
								MemoryType = "Byte",
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset
							};
						}
						else if (fg3.A == 2)
						{
							int test_this = _m.Read2Byte((address + entry.Key).ToString("X"));
							if (eb1.enums.Items.Count >= test_this)
							{
								eb1.enums.SelectedIndex = test_this;
							}
							else
							{
								TextBox tb = new TextBox { Text = "the enum below is broken :(" };
								parentpanel.Children.Add(tb);
							}
							eb1.ValueDefinition = new TagEditorDefinition()
							{
								MemoryType = "2Byte",
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset
							};
						}
						else if (fg3.A == 4)
						{
							int test_this = _m.ReadInt((address + entry.Key).ToString("X"));
							if (eb1.enums.Items.Count >= test_this)
							{
								eb1.enums.SelectedIndex = test_this;
							}
							else
							{
								TextBox tb = new TextBox { Text = "the enum below is broken :(" };
								parentpanel.Children.Add(tb);
							}
							eb1.ValueDefinition = new TagEditorDefinition()
							{
								MemoryType = "4Byte",
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset
							};
						}
						else
						{
							string put_breakpoint_here;
							eb1.ValueDefinition = new TagEditorDefinition()
							{
								MemoryType = "Enums",
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset
							};
						}

						// add appropriate values to combobox
						// read selected



						parentpanel.Children.Add(eb1);
						eb1.value_name.Text = fg3.N;
						eb1.main = _mainWindow;
						break;
					case "4Byte":
						TagValueBlock? vb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb1.value_type.Text = "4 Byte";
						vb1.value.Text = _m.ReadInt((address + entry.Key).ToString("X")).ToString(); // (+entry.Key?) lmao, no wonder why it wasn't working
						parentpanel.Children.Add(vb1);

						//vb1.value.Tag = address + entry.Key + ":4Byte";
						vb1.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "4Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						vb1.value.TextChanged += value_TextChanged;

						vb1.value_name.Text = entry.Value.N;
						break;
					case "2Byte":
						TagValueBlock? vb6 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb6.value_type.Text = "2 Byte";
						vb6.value.Text = _m.Read2Byte((address + entry.Key).ToString("X")).ToString();
						parentpanel.Children.Add(vb6);

						vb6.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "2Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						vb6.value.TextChanged += value_TextChanged;

						vb6.value_name.Text = entry.Value.N;

						break;
					case "Byte":
						TagValueBlock? vb19 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb19.value_type.Text = "Byte";
						vb19.value.Text = _m.ReadByte((address + entry.Key).ToString("X")).ToString();
						parentpanel.Children.Add(vb19);

						vb19.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						vb19.value.TextChanged += value_TextChanged;

						vb19.value_name.Text = entry.Value.N;

						break;
					case "Float":
						TagValueBlock? vb2 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb2.value_type.Text = "Float";
						vb2.value.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						parentpanel.Children.Add(vb2);

						vb2.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						vb2.value.TextChanged += value_TextChanged;


						vb2.value_name.Text = entry.Value.N;

						break;

					case "TagRef":
						TagRefBlock? tfb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						tfb1.taggroup.Items.Add("Null");
						foreach (string s in _mainWindow.TagGroups.Keys)
						{
							tfb1.taggroup.Items.Add(s);
						}

						string tagGroup = ReverseString(_m.ReadString((address + entry.Key + 20).ToString("X"), "", 4));
						tfb1.taggroup.SelectedItem = tagGroup;

						// read tagID rather than datnum // or rather, convert datnum to ID
						try
						{
							string datNum = BitConverter.ToString(_m.ReadBytes((address + entry.Key + 24).ToString("X"), 4)).Replace("-", string.Empty);
							string tagId = _mainWindow.get_tagid_by_datnum(datNum);
							string tagName = _mainWindow.convert_ID_to_tag_name(tagId);

							tfb1.tag_button.Content = tagName;
							parentpanel.Children.Add(tfb1);

							//tfb1.taggroup.Tag = (address + entry.Key + 20);


							tfb1.taggroup.Tag = new TED_TagRefGroup()
							{
								MemoryType = "TagrefGroup",
								TagId = tagId,
								DatNum = datNum,
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 20),

								TagGroup = tagGroup
							};
							tfb1.taggroup.SelectionChanged += taggroup_SelectionChanged;

							//tfb1.tag_button.Tag = (address + entry.Key + 24) + ":" + testGroup;
							tfb1.tag_button.Tag = new TED_TagRefGroup()
							{
								MemoryType = tagGroup,
								TagId = tagId,
								DatNum = datNum,
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 24),

								TagGroup = tagGroup
							};
							tfb1.tag_button.Click += TagRefButton;

							string id = _mainWindow.get_tagID_by_datnum(datNum);

							// tag
							tfb1.goto_button.Tag = id; // 
							tfb1.goto_button.Click += Gotobutton;

							tfb1.value_name.Text = entry.Value.N;

						}
						catch
						{
							break;
						}
						break;

					case "Pointer":
						TagValueBlock? vb3 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb3.value_type.Text = "Pointer";
						vb3.value.Text = _m.ReadLong((address + entry.Key).ToString("X")).ToString("X");
						parentpanel.Children.Add(vb3);

						vb3.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Pointer",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						vb3.value.TextChanged += value_TextChanged;

						vb3.value_name.Text = entry.Value.N;

						break;

					case "Tagblock": 
						TagBlock? tb1 = new(this, tagStruct)
						{
							HorizontalAlignment = HorizontalAlignment.Left
						};

						long newAddress = _m.ReadLong((address + entry.Key).ToString("X"));
						tb1.tagblock_address.Text = "0x" + newAddress.ToString("X");

						long stringAddress = _m.ReadLong((address + entry.Key + 8).ToString("X"));

						// switch either reading from mem or reading OUR tagblock name, just incase someone has to rename something for convienience or whatever
						string our_name = entry.Value.N;
						if (our_name == null)
						{
							tb1.tagblock_title.Text = _m.ReadString((address + entry.Key + 8).ToString("X") + ",0,0", "", 100); // this is the only thing that causes errors with unloaded tags
						}
						else
						{
							tb1.tagblock_title.Text = our_name;
						}

						//tb1.tagblock_title.Text = "Error: tag Unloaded";
						//parentpanel.Children.Add(tb1);

						string childrenCount = _m.ReadInt((address + entry.Key + 16).ToString("X")).ToString();
						tb1.tagblock_count.Text = childrenCount;
						parentpanel.Children.Add(tb1);

						//tb1.tagblock_address.Tag = (address + entry.Key) + ":Pointer";
						tb1.tagblock_address.Tag = new TagEditorDefinition()
						{
							MemoryType = "Pointer",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};
						tb1.tagblock_address.TextChanged += value_TextChanged;

						//tb1.tagblock_count.Tag = (address + entry.Key + 16) + ":4Byte";
						tb1.tagblock_count.Tag = new TagEditorDefinition()
						{
							MemoryType = "4Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 16),

						};
						tb1.tagblock_count.TextChanged += value_TextChanged;

						//tb1.indexbox.SelectionChanged += new SelectionChangedEventHandler(indexbox_SelectionChanged);

						tb1.Children = entry;
						tb1.BlockAddress = newAddress;

						int childs = int.Parse(childrenCount);
						if (childs > 2000000)
						{
							TextBox tb = new TextBox { Text = "ran into a major fucky wucky, some tagblock exceeded 2million entries" };
							tagview_panels.Children.Add(tb);
							return;
						}
						if (entry.Value.B != null) // this should optimize the hell outta opening tags // like we were literally instaniating 1million items for the levl tag
						{
							for (int y = 0; y < childs; y++)
							{
								tb1.indexbox.Items.Add(new ComboBoxItem { Content = y }); // this should be a combobox item?
							}
							if (childs > 0)
							{
								tb1.indexbox.SelectedIndex = -1;
							}
							else
							{
								tb1.Expand_Collapse_Button.IsEnabled = false;
								tb1.Expand_Collapse_Button.Content = "";
								tb1.indexbox.IsEnabled = false;
							}
						}
						else
						{
							tb1.Expand_Collapse_Button.IsEnabled = false;
							tb1.Expand_Collapse_Button.Content = "";
							tb1.indexbox.IsEnabled = false;

						}
						tb1.stored_num_on_index = 0;
						//recall_blockloop(entry, new_address, tb1.dockpanel);
						break;
					
					case "TagStructBlock":
						//--THIS DOESN'T WORK, DON'T USE. IF YOU WANT TO TRY TO GET IT WORKING, GOOD LUCK.
						
						TagStructBlock? ts1 = new(this, tagStruct, entry, address + entry.Key)
						{
							HorizontalAlignment = HorizontalAlignment.Left
						};
						parentpanel.Children.Add(ts1);
						ts1.tagblock_title.Text = entry.Value.N;
						ts1.Children = entry;
						
						break;

					case "String":
						TagValueBlock? vb4 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb4.value_type.Text = "String";
						vb4.value.Text = _m.ReadString((address + entry.Key).ToString("X"), "", 100).ToString();
						parentpanel.Children.Add(vb4);

						//vb4.value.Tag = address + entry.Key + ":String";
						vb4.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "String",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};
						vb4.value.TextChanged += value_TextChanged;

						vb4.value_name.Text = entry.Value.N;

						break;

					case "Flags":
						TagsFlags? vb9 = new()
						{
							HorizontalAlignment = HorizontalAlignment.Left,
							ValueDefinition = new TagEditorDefinition()
							{
								MemoryType = "Flags",
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset
							}
						};
						byte flags_value = (byte) _m.ReadByte((address + entry.Key).ToString("X"));
						parentpanel.Children.Add(vb9);

						vb9.flag1.IsChecked = flags_value.GetBit(0);
						vb9.flag2.IsChecked = flags_value.GetBit(1);
						vb9.flag3.IsChecked = flags_value.GetBit(2);
						vb9.flag4.IsChecked = flags_value.GetBit(3);
						vb9.flag5.IsChecked = flags_value.GetBit(4);
						vb9.flag6.IsChecked = flags_value.GetBit(5);
						vb9.flag7.IsChecked = flags_value.GetBit(6);
						vb9.flag8.IsChecked = flags_value.GetBit(7);

						vb9._mainwindow = _mainWindow;
						vb9.address = address + entry.Key;

						break;

					case "FlagGroup":
						// make sure we got a flaggroup def
						if (!(entry.Value is TagLayouts.FlagGroup))
						{
							continue;
						}

						TagLayouts.FlagGroup? fg = entry.Value as TagLayouts.FlagGroup;
						TagFlagsGroup? tfg = new() { HorizontalAlignment = HorizontalAlignment.Left };
						tfg.ValueDefinition = new TagEditorDefinition()
						{
							MemoryType = "Flags",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};
						tfg.M = _mainWindow.M;
						tfg.mainWindow = _mainWindow;

						parentpanel.Children.Add(tfg);
						tfg.generateBits(address + entry.Key, fg.A, fg.MB, fg.STR);
						tfg.flag_name.Text = fg.N;

						break;
					case "mmr3Hash":
						TagValueBlock? vb5 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb5.value_type.Text = "mmr3Hash";
						vb5.value.Text = BitConverter.ToString(_m.ReadBytes((address + entry.Key).ToString("X"), 4)).Replace("-", string.Empty);
						parentpanel.Children.Add(vb5);

						//vb4.value.Tag = address + entry.Key + ":String";
						vb5.value.Tag = new TagEditorDefinition()
						{
							MemoryType = "mmr3Hash",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};
						vb5.value.TextChanged += value_TextChanged;

						vb5.value_name.Text = entry.Value.N;

						break;
					case "RGB":
						TagRGBBlock? rgb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						rgb1.r_value.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						rgb1.g_value.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						rgb1.b_value.Text = _m.ReadFloat((address + entry.Key + 8).ToString("X")).ToString();

						byte r_hex = (byte) Math.Round(_m.ReadFloat((address + entry.Key).ToString("X")) * 255);
						byte g_hex = (byte) Math.Round(_m.ReadFloat((address + entry.Key + 4).ToString("X")) * 255);
						byte b_hex = (byte) Math.Round(_m.ReadFloat((address + entry.Key + 8).ToString("X")) * 255);
						string hex_color = r_hex.ToString("X2") + g_hex.ToString("X2") + b_hex.ToString("X2");

						parentpanel.Children.Add(rgb1);

						rgb1.r_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						rgb1.g_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};
						
						rgb1.b_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 8),
						};

						rgb1.r_value.TextChanged += value_TextChanged;
						rgb1.g_value.TextChanged += value_TextChanged;
						rgb1.b_value.TextChanged += value_TextChanged;

						rgb1.rgb_name.Text = entry.Value.N;
						rgb1.color_hash.Text = "#" + hex_color;
						rgb1.rgb_colorpicker.SelectedColor = Color.FromRgb(r_hex, g_hex, b_hex);

						break;
					case "ARGB":
						TagARGBBlock? argb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						argb1.a_value.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						argb1.r_value.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						argb1.g_value.Text = _m.ReadFloat((address + entry.Key + 8).ToString("X")).ToString();
						argb1.b_value.Text = _m.ReadFloat((address + entry.Key + 12).ToString("X")).ToString();

						byte a_hex2 = (byte) Math.Round(_m.ReadFloat((address + entry.Key).ToString("X")) * 255);
						byte r_hex2 = (byte) Math.Round(_m.ReadFloat((address + entry.Key + 4).ToString("X")) * 255);
						byte g_hex2 = (byte) Math.Round(_m.ReadFloat((address + entry.Key + 8).ToString("X")) * 255);
						byte b_hex2 = (byte) Math.Round(_m.ReadFloat((address + entry.Key + 12).ToString("X")) * 255);

						parentpanel.Children.Add(argb1);

						argb1.a_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						argb1.r_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						argb1.g_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 8),
						};

						argb1.b_value.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 12),
						};

						argb1.a_value.TextChanged += value_TextChanged;
						argb1.r_value.TextChanged += value_TextChanged;
						argb1.g_value.TextChanged += value_TextChanged;
						argb1.b_value.TextChanged += value_TextChanged;

						argb1.rgb_name.Text = entry.Value.N;
						argb1.argb_colorpicker.SelectedColor = Color.FromArgb(a_hex2 ,r_hex2, g_hex2, b_hex2);

						break;
					case "BoundsFloat":
						TagTwoBlock? ttb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						
						ttb1.f_label1.Text = "Min:";
						ttb1.f_label2.Text = "Max:";

						ttb1.f_type.Text = "Float";

						ttb1.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						ttb1.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						parentpanel.Children.Add(ttb1);

						ttb1.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						ttb1.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						ttb1.f_value1.TextChanged += value_TextChanged;
						ttb1.f_value2.TextChanged += value_TextChanged;

						ttb1.f_name.Text = entry.Value.N;

						break;

					case "Bounds2Byte":
						TagTwoBlock? ttb2 = new() { HorizontalAlignment = HorizontalAlignment.Left };

						ttb2.f_label1.Text = "Min:";
						ttb2.f_label2.Text = "Max:";

						ttb2.f_type.Text = "2 Byte";

						ttb2.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						ttb2.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						parentpanel.Children.Add(ttb2);

						ttb2.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "2Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						ttb2.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "2Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						ttb2.f_value1.TextChanged += value_TextChanged;
						ttb2.f_value2.TextChanged += value_TextChanged;

						ttb2.f_name.Text = entry.Value.N;

						break;
					
					case "2DPoint_Float":
						TagTwoBlock? ttb3 = new() { HorizontalAlignment = HorizontalAlignment.Left };

						ttb3.f_label1.Text = "X:";
						ttb3.f_label2.Text = "Y:";

						ttb3.f_type.Text = "Float";

						ttb3.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						ttb3.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						parentpanel.Children.Add(ttb3);

						ttb3.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						ttb3.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						ttb3.f_value1.TextChanged += value_TextChanged;
						ttb3.f_value2.TextChanged += value_TextChanged;

						ttb3.f_name.Text = entry.Value.N;

						break;

					case "2DPoint_2Byte":
						TagTwoBlock? ttb4 = new() { HorizontalAlignment = HorizontalAlignment.Left };

						ttb4.f_label1.Text = "X:";
						ttb4.f_label2.Text = "Y:";

						ttb4.f_type.Text = "2 Byte";

						ttb4.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						ttb4.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						parentpanel.Children.Add(ttb4);

						ttb4.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "2Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						ttb4.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "2Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						ttb4.f_value1.TextChanged += value_TextChanged;
						ttb4.f_value2.TextChanged += value_TextChanged;

						ttb4.f_name.Text = entry.Value.N;

						break;

					case "3DPoint":
						TagThreeBlock? tthb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };

						tthb1.f_label1.Text = "X:";
						tthb1.f_label2.Text = "Y:";
						tthb1.f_label3.Text = "Z:";

						tthb1.f_type.Text = "Float";

						tthb1.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						tthb1.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						tthb1.f_value3.Text = _m.ReadFloat((address + entry.Key + 8).ToString("X")).ToString();
						parentpanel.Children.Add(tthb1);

						tthb1.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						tthb1.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						tthb1.f_value3.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 8),
						};

						tthb1.f_value1.TextChanged += value_TextChanged;
						tthb1.f_value2.TextChanged += value_TextChanged;
						tthb1.f_value3.TextChanged += value_TextChanged;

						tthb1.f_name.Text = entry.Value.N;

						break;

					case "Quanternion":
						TagFourBlock? tfob1 = new() { HorizontalAlignment = HorizontalAlignment.Left };

						tfob1.f_label1.Text = "W:";
						tfob1.f_label2.Text = "X:";
						tfob1.f_label3.Text = "Y:";
						tfob1.f_label4.Text = "Z:";

						tfob1.f_type.Text = "Float";

						tfob1.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						tfob1.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						tfob1.f_value3.Text = _m.ReadFloat((address + entry.Key + 8).ToString("X")).ToString();
						tfob1.f_value4.Text = _m.ReadFloat((address + entry.Key + 12).ToString("X")).ToString();
						parentpanel.Children.Add(tfob1);

						tfob1.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						tfob1.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						tfob1.f_value3.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 8),
						};
						
						tfob1.f_value4.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 12),
						};

						tfob1.f_value1.TextChanged += value_TextChanged;
						tfob1.f_value2.TextChanged += value_TextChanged;
						tfob1.f_value3.TextChanged += value_TextChanged;
						tfob1.f_value4.TextChanged += value_TextChanged;

						tfob1.f_name.Text = entry.Value.N;

						break;
					case "3DPlane":
						TagFourBlock? tfob2 = new() { HorizontalAlignment = HorizontalAlignment.Left };							
						
						tfob2.f_label1.Text = "X:";
						tfob2.f_label2.Text = "Y:";
						tfob2.f_label3.Text = "Z:";
						tfob2.f_label4.Text = "Point:";

						tfob2.f_type.Text = "Float";

						tfob2.f_value1.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						tfob2.f_value2.Text = _m.ReadFloat((address + entry.Key + 4).ToString("X")).ToString();
						tfob2.f_value3.Text = _m.ReadFloat((address + entry.Key + 8).ToString("X")).ToString();
						tfob2.f_value4.Text = _m.ReadFloat((address + entry.Key + 12).ToString("X")).ToString();
						parentpanel.Children.Add(tfob2);

						tfob2.f_value1.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = entry.Value.AbsoluteTagOffset
						};

						tfob2.f_value2.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 4),
						};

						tfob2.f_value3.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 8),
						};

						tfob2.f_value4.Tag = new TagEditorDefinition()
						{
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct,
							OffsetOverride = SUSSY_BALLS(entry.Value.AbsoluteTagOffset, 12),
						};

						tfob2.f_value1.TextChanged += value_TextChanged;
						tfob2.f_value2.TextChanged += value_TextChanged;
						tfob2.f_value3.TextChanged += value_TextChanged;
						tfob2.f_value4.TextChanged += value_TextChanged;

						tfob2.f_name.Text = entry.Value.N;

						break;

				}

				prevEntry = entry;
			}
		}

		public void fat_chunk_of_code_from_above(long thingo_address, FunctionBlock fb )
		{
			// address + entry.Key



		}

		public static string SUSSY_BALLS(string input, long add_to)
		{
			//SUSSY BALLS
			string[] last_offset = input.Split(",");
			string joined = string.Join(",", last_offset.SkipLast(1));
			long poop = long.Parse(last_offset.Last());
			return joined +","+ (poop += add_to).ToString();
		}


		private void tagfilter_text_Changed(object sender, TextChangedEventArgs e)
		{
			string? text = tagfilter_text.Text;
			UIElementCollection? children = tagview_panels.Children;
			const int all = 0, titles = 1, datatypes = 2, values = 3;

			if (string.IsNullOrWhiteSpace(tagfilter_text.Text))
			{
				filterTags_SetVisibility(children, Visibility.Visible);
				return;
			}

			filterTags_SetVisibility(children, Visibility.Collapsed);

			switch (cbxSearchType.SelectedIndex)
			{
				case all:
					filterTags_Titles(children, text);
					filterTags_Datatypes(children, text);
					filterTags_Values(children, text);
					break;
				case titles:
					filterTags_Titles(children, text);
					break;
				case datatypes:
					filterTags_Datatypes(children, text);
					break;
				case values:
					filterTags_Values(children, text);
					break;
			}
		}

		private void filterTags_SetVisibility(UIElementCollection collection, Visibility vis)
		{
			foreach (Control control in collection)
			{
				if (control is TagBlock)
				{
					TagBlock? tb = (TagBlock) control;
					filterTags_SetVisibility(tb.dockpanel.Children, vis);
				}

				control.Visibility = vis;
			}
		}

		private bool filterTags_Titles(UIElementCollection collection, string filterText)
		{
			bool found = false;
			foreach (Control control in collection)
			{
				if (control is TagBlock)
				{
					TagBlock? trb = (TagBlock) control;
					if (((string) trb.tagblock_title.Text).Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is TagValueBlock)
				{
					TagValueBlock? trb = (TagValueBlock) control;
					string str = (string) trb.value_name.Text;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is CommentBlock)
				{
					CommentBlock? trb = (CommentBlock) control;
					string str = (string) trb.comment.Text;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is EnumBlock)
				{
					EnumBlock? trb = (EnumBlock) control;
					string str = (string) trb.value_name.Text;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is TagRefBlock)
				{
					TagRefBlock? trb = (TagRefBlock) control;
					string str = (string) trb.value_name.Text;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is TagFlagsGroup)
				{
					TagFlagsGroup? trb = (TagFlagsGroup) control;
					bool continue2 = false;
					foreach (var v in trb.spBitCollection.Children)
					{
						if (v is TagsFlags) // im not sure if tagsflags are even used hmm
						{
							TagsFlags? trbaby = (TagsFlags) v;

							foreach (var d in trbaby.goober_panel.Children)
							{
								CheckBox? trbaby2 = (CheckBox) d;
								string str1 = (string) trbaby2.Content;
								if (str1.Contains(filterText, StringComparison.OrdinalIgnoreCase))
								{
									control.Visibility = Visibility.Visible;
									found = true; continue2 = true;
									continue;
								}
							}
							if (continue2)
								continue;

						}
						else if (v is CheckBox)
						{
							CheckBox? trbaby2 = (CheckBox) v;
							string str1 = (string) trbaby2.Content;
							if (str1.Contains(filterText, StringComparison.OrdinalIgnoreCase))
							{
								control.Visibility = Visibility.Visible;
								found = true; 
								continue;
							}
						}
					}
				}
				//
				// tag flags block missing
				//
				else if (control is TagBlock)
				{
					TagBlock? tb = (TagBlock) control;
					StackPanel? dp = tb.dockpanel;
					if (dp == null)
					{
						continue;
					}

					bool f = filterTags_Titles(dp.Children, filterText);
					if (f)
					{
						found = true;
						tb.Visibility = Visibility.Visible;
					}
				}
			}
			return found;
		}

		private bool filterTags_Datatypes(UIElementCollection collection, string filterText)
		{
			bool found = false;

			foreach (Control control in collection)
			{
				if (control is TagRefBlock)
				{
					TagRefBlock? trb = (TagRefBlock) control;
					ComboBoxItem? cbxi = trb.taggroup.SelectedItem as ComboBoxItem;
					if (cbxi == null)
					{
						continue;
					}

					string? str = cbxi.Content as string;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagValueBlock)
				{
					TagValueBlock? trb = (TagValueBlock) control;
					string str = (string) trb.value_type.Text;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagBlock)
				{
					TagBlock? tb = (TagBlock) control;
					StackPanel? dp = tb.dockpanel;
					if (dp == null)
					{
						continue;
					}

					bool f = filterTags_Datatypes(dp.Children, filterText);
					if (f)
					{
						found = true;
						tb.Visibility = Visibility.Visible;
					}
				}
			}
			return found;
		}

		private bool filterTags_Values(UIElementCollection collection, string filterText)
		{
			bool found = false;
			foreach (Control control in collection)
			{
				if (control is TagValueBlock)
				{
					TagValueBlock? tvb = (TagValueBlock) control;
					string? val = tvb.value.Text.ToString();
					if (val != null && val.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is EnumBlock)
				{
					EnumBlock? tvb = (EnumBlock) control;
					string? val = tvb.enums.Text.ToString();
					if (val != null && val.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}
				else if (control is TagRefBlock)
				{
					TagRefBlock? trb = (TagRefBlock) control;
					string? val = (string) trb.tag_button.Content;
					if (val != null && val.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagBlock)
				{
					TagBlock? tb = (TagBlock) control;
					StackPanel? dp = tb.dockpanel;
					if (dp == null)
					{
						continue;
					}

					bool f = filterTags_Values(dp.Children, filterText);
					if (f)
					{
						found = true;
						tb.Visibility = Visibility.Visible;
					}
				}
			}
			return found;
		}

	

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
