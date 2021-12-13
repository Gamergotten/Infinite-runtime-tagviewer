using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Diagnostics;
using InfiniteRuntimeTagViewer.Halo.TagObjects;
using InfiniteRuntimeTagViewer.Interface.Windows;
using AvalonDock.Controls;
using AvalonDock.Layout;
using Memory;

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

		private List<Action> disposeActions = new List<Action>();
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
					// TODO: call the action list
					for (int x = 0; x < disposeActions.Count; x++)
						disposeActions[x].Invoke();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~TagEditorControl()
		{
			int x = 0;
		}

		public void Inhale_tag(int tagIndex) // as in a literal index to the tag
		{
			TagStruct loadingTag = _mainWindow.TagsList[tagIndex];
			Tagname_text.Text = _mainWindow.convert_ID_to_tag_name(loadingTag.ObjectId);
			tagID_text.Text = "ID: " + loadingTag.ObjectId;
			tagdatnum_text.Text = "Datnum: " + loadingTag.Datnum;
			tagdata_text.Text = "Tag data address: 0x" + loadingTag.TagData.ToString("X");

			tagfilter_text.Text = "";

			tagview_panels.Children.Clear();

			// OK, now we do a proper check to see if this tag is loaded, finally looked into this

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
				TextBox tb = new TextBox { Text = "Datnum/ID mismatch; Tag appears to be unloaded, meaning it may not be active on the map, else try reloading the tags" };
				tagview_panels.Children.Add(tb);
				return;
			}

			if (Vehi.Tags.ContainsKey(loadingTag.TagGroup))
			{
				Dictionary<long, Vehi.C> tags = Vehi.Tags[loadingTag.TagGroup];
				readTagsAndCreateControls(loadingTag, 0, tags, loadingTag.TagData, tagview_panels);
			}
			else
			{
				TextBox tb = new TextBox { Text = "This tag isn't mapped out ):" };
				tagview_panels.Children.Add(tb);
			}
		}

		// hmm we need a system that reads the pointer and adds it
		// also, we need to beable to read multiple tag things but i may put that on hold
		public void recall_blockloop(TagStruct tagStruct, long tagOffset, KeyValuePair<long, Vehi.C> entry, long loadingTag, StackPanel parentpanel)
		{
			parentpanel.Children.Clear();
			if (entry.Value.B != null)
			{
				readTagsAndCreateControls(tagStruct, tagOffset, entry.Value.B, loadingTag, parentpanel);
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

			_mainWindow.AddPokeChange(ted, cb.SelectedValue.ToString());
			//_mainWindow.AddPokeChange(long.Parse(s: cb.Tag.ToString()), "TagrefGroup", value: cb.SelectedValue.ToString());

			// What the actual fuck is all of this? 
			Grid? td = cb.Parent as Grid;
			Button? b = td.Children[1] as Button;
			var btnTed = b.Tag as TED_TagRefGroup;
			btnTed.TagGroup = cb.SelectedValue.ToString();

			//string[] s = b.Tag.ToString().Split(":");
			//b.Tag = s[0] + ":" + cb.SelectedValue.ToString();

			// THAT WAS PROBABLY THE MOST DODGY THING IVE EVER DONE WTFFFF
		}

		public void Gotobutton(object sender, RoutedEventArgs e)
		{
			Button? b = sender as Button;
			string? sTagId = b.Tag.ToString();
			int iTagId = int.Parse(sTagId);

			if (iTagId != -1)
			{
				_mainWindow.CreateTagEditorTabByTagIndex(iTagId);
			}
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
				var dockingPaneGroup = GetTopLevelControlOfType<LayoutDocumentPaneGroupControl>(b);
				bool foundDockingWindow = false;

				// Handle if we have a popped out window.
				if (controlsWindow == null && dockingPaneGroup != null)
				{
					// Check if we can figure out where this docking pane is located
					foreach (Window appWindow in Application.Current.Windows)
					{
						var floatingWind = appWindow as LayoutDocumentFloatingWindowControl;
						if (floatingWind == null)
							continue;

						// Make sure we have a FloatingWindowContentHost
						var floatingWindContHost = floatingWind.Content; // AvalonDock.Controls.LayoutFloatingWindowControl.FloatingWindowContentHost
						if (floatingWindContHost == null && floatingWindContHost.GetType().Name != "FloatingWindowContentHost")
							continue;

						// Get the public property "Content", we cant get this normally because this
						// is a internal / sealed class. We need to use reflection to get our
						// grubby mitts on it.
						var prop = floatingWindContHost.GetType().GetProperty("Content");
						var contentResult = prop.GetValue(floatingWindContHost) as LayoutDocumentPaneGroupControl;

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
					var myButtonLocation = b.PointToScreen(new Point(0, 0));

					trd.Left = myButtonLocation.X - 8;
					trd.Top = myButtonLocation.Y + 1;
				}

			}

			trd.MainWindow = _mainWindow;
			TheLastTagrefButtonWePressed = b;

			// Null type
			TreeViewItem? tvi = new()
			{
				Header = _mainWindow.convert_ID_to_tag_name("FFFFFFFF"),
				Tag = new TED_TagRefGroup(ted)
				{
					MemoryType = "TagrefTag",
					DatNum = "FFFFFFFF"
				} //s[0] + ":" + "FFFFFFFF"
			};

			trd.tag_select_panel.Items.Add(tvi);
			tvi.Selected += update_tagref;
			disposeActions.Add(() => tvi.Selected -= update_tagref);

			foreach (TagStruct tg in _mainWindow.TagsList)
			{
				if (tg.TagGroup == ted.TagGroup)
				{
					TreeViewItem? tvi2 = new()
					{
						Header = _mainWindow.convert_ID_to_tag_name(tg.ObjectId),
						//Tag = s[0] + ":" + tg.Datnum
						Tag = new TED_TagRefGroup(ted)
						{
							MemoryType = "TagrefTag",
							DatNum = tg.Datnum
						}
					};

					trd.tag_select_panel.Items.Add(tvi2);
					tvi2.Selected += update_tagref;
					disposeActions.Add(() => tvi2.Selected -= update_tagref);
				}
			}

			trd.Show();
			return;
		}

		// this is for our dropdown thingo for changing tag refs
		public void update_tagref(object sender, RoutedEventArgs e)
		{
			TreeViewItem? b = sender as TreeViewItem;
			TED_TagRefGroup ted = b.Tag as TED_TagRefGroup;

			_mainWindow.AddPokeChange(ted, ted.DatNum);

			string id = _mainWindow.get_tagid_by_datnum(ted.DatNum);
			TheLastTagrefButtonWePressed.Content = _mainWindow.convert_ID_to_tag_name(id);

			// need to do this the lazy way again, have to head off in a sec
			Grid? td = TheLastTagrefButtonWePressed.Parent as Grid;
			Button? x = td.Children[2] as Button;
			//X.Tag = ID;

			x.Tag = _mainWindow.get_tagindex_by_datnum(ted.DatNum);

			if (trd != null)
			{
				trd.Closethis();
			}
		}

		// had to adapt this to bealbe to read tagblocks and forgot to allow it to iterate through them *sigh* good enough for now
		private void readTagsAndCreateControls(TagStruct tagStruct, long startingTagOffset, Dictionary<long, Vehi.C> tagDefinitions, long address, StackPanel parentpanel)
		{
			KeyValuePair<long, Vehi.C> prevEntry;
			foreach (KeyValuePair<long, Vehi.C> entry in tagDefinitions)
			{
				entry.Value.MemoryAddress = address + entry.Key;
				entry.Value.AbsoluteTagOffset = startingTagOffset + entry.Key;

				switch (entry.Value.T)
				{
					case "4Byte":
						TagValueBlock? vb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb1.value_type.Text = "4 Byte";
						vb1.value.Text = _m.ReadInt((address + entry.Key).ToString("X")).ToString(); // (+entry.Key?) lmao, no wonder why it wasn't working
						parentpanel.Children.Add(vb1);

						//vb1.value.Tag = address + entry.Key + ":4Byte";
						vb1.value.Tag = new TagEditorDefinition()
						{
							MemoryAddress = address + entry.Key,
							MemoryType = "4Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};

						vb1.value.TextChanged += value_TextChanged;
						disposeActions.Add(() => vb1.value.TextChanged -= value_TextChanged);
						break;
					case "2Byte":
						TagValueBlock? vb6 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb6.value_type.Text = "2 Byte";
						vb6.value.Text = _m.Read2Byte((address + entry.Key).ToString("X")).ToString();
						parentpanel.Children.Add(vb6);

						vb6.value.Tag = new TagEditorDefinition()
						{
							MemoryAddress = address + entry.Key,
							MemoryType = "2Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};

						vb6.value.TextChanged += value_TextChanged;
						disposeActions.Add(() => vb6.value.TextChanged -= value_TextChanged);
						break;

					case "Float":
						TagValueBlock? vb2 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb2.value_type.Text = "Float";
						vb2.value.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
						parentpanel.Children.Add(vb2);

						vb2.value.Tag = new TagEditorDefinition()
						{
							MemoryAddress = address + entry.Key,
							MemoryType = "Float",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};

						vb2.value.TextChanged += value_TextChanged;
						disposeActions.Add(() => vb2.value.TextChanged -= value_TextChanged);

						break;

					case "TagRef":
						TagRefBlock? tfb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
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
								MemoryAddress = (address + entry.Key + 20),
								MemoryType = "TagrefGroup",
								TagId = tagId,
								DatNum = datNum,
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset + 20,

								TagGroup = tagGroup
							};
							tfb1.taggroup.SelectionChanged += taggroup_SelectionChanged;
							disposeActions.Add(() => tfb1.taggroup.SelectionChanged -= taggroup_SelectionChanged);

							//tfb1.tag_button.Tag = (address + entry.Key + 24) + ":" + testGroup;
							tfb1.tag_button.Tag = new TED_TagRefGroup()
							{
								MemoryAddress = (address + entry.Key + 24),
								MemoryType = tagGroup,
								TagId = tagId,
								DatNum = datNum,
								TagDef = entry.Value,
								TagStruct = tagStruct,
								OffsetOverride = entry.Value.AbsoluteTagOffset + 24,

								TagGroup = tagGroup
							};
							tfb1.tag_button.Click += TagRefButton;
							disposeActions.Add(() => tfb1.tag_button.Click -= TagRefButton);

							int id = _mainWindow.get_tagindex_by_datnum(datNum);

							// tag
							tfb1.goto_button.Tag = id; // need to get the index of the tag not the ID
							tfb1.goto_button.Click += Gotobutton;
							disposeActions.Add(() => tfb1.goto_button.Click -= Gotobutton);
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
							MemoryAddress = address + entry.Key,
							MemoryType = "Pointer",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};

						vb3.value.TextChanged += value_TextChanged;
						disposeActions.Add(() => vb3.value.TextChanged -= value_TextChanged);

						break;

					case "Tagblock": // need to find some kinda "whoops that tag isnt actually loaded"; keep erroring with the hlmt tag
						TagBlock? tb1 = new(this, startingTagOffset + entry.Key, tagStruct)
						{
							HorizontalAlignment = HorizontalAlignment.Left
						};

						long newAddress = _m.ReadLong((address + entry.Key).ToString("X"));
						tb1.tagblock_address.Text = "0x" + newAddress.ToString("X");

						long stringAddress = _m.ReadLong((address + entry.Key + 8).ToString("X"));

						tb1.tagblock_title.Text = _m.ReadString((address + entry.Key + 8).ToString("X") + ",0,0", "", 100); // this is the only thing that causes errors with unloaded tags

						//tb1.tagblock_title.Text = "Error: tag Unloaded";
						//parentpanel.Children.Add(tb1);

						string childrenCount = _m.ReadInt((address + entry.Key + 16).ToString("X")).ToString();
						tb1.tagblock_count.Text = childrenCount;
						parentpanel.Children.Add(tb1);

						//tb1.tagblock_address.Tag = (address + entry.Key) + ":Pointer";
						tb1.tagblock_address.Tag = new TagEditorDefinition()
						{
							MemoryAddress = address + entry.Key,
							MemoryType = "Pointer",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};
						tb1.tagblock_address.TextChanged += value_TextChanged;
						disposeActions.Add(() => tb1.tagblock_address.TextChanged -= value_TextChanged);

						//tb1.tagblock_count.Tag = (address + entry.Key + 16) + ":4Byte";
						tb1.tagblock_count.Tag = new TagEditorDefinition()
						{
							MemoryAddress = address + entry.Key,
							MemoryType = "4Byte",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};
						tb1.tagblock_count.TextChanged += value_TextChanged;
						disposeActions.Add(() => tb1.tagblock_count.TextChanged -= value_TextChanged);

						//tb1.indexbox.SelectionChanged += new SelectionChangedEventHandler(indexbox_SelectionChanged);

						tb1.Children = entry;
						tb1.BlockAddress = newAddress;

						int childs = int.Parse(childrenCount);
						if (entry.Value.B != null) // this should optimize the hell outta opening tags // like we were literally instaniating 1million items for the levl tag
						{
							for (int y = 0; y < childs; y++)
							{
								tb1.indexbox.Items.Add(new ComboBoxItem { Content = y }); // this should be a combobox item?
							}
							if (childs > 0)
							{
								tb1.indexbox.SelectedIndex = 0;
							}
							else
							{
								tb1.indexbox.IsEnabled = false;
							}
						}
						else
						{
							tb1.indexbox.IsEnabled = false;

						}

						//recall_blockloop(entry, new_address, tb1.dockpanel);
						break;

					case "String":
						TagValueBlock? vb4 = new() { HorizontalAlignment = HorizontalAlignment.Left };
						vb4.value_type.Text = "String";
						vb4.value.Text = _m.ReadString((address + entry.Key).ToString("X"), "", 100).ToString();
						parentpanel.Children.Add(vb4);

						//vb4.value.Tag = address + entry.Key + ":String";
						vb4.value.Tag = new TagEditorDefinition()
						{
							MemoryAddress = address + entry.Key,
							MemoryType = "String",
							TagDef = entry.Value,
							TagStruct = tagStruct
						};
						vb4.value.TextChanged += value_TextChanged;
						disposeActions.Add(() => vb4.value.TextChanged -= value_TextChanged);
						break;

					case "Flags":
						TagsFlags? vb9 = new()
						{
							HorizontalAlignment = HorizontalAlignment.Left,
							ValueDefinition = new TagEditorDefinition()
							{
								MemoryType = "Flags",
								MemoryAddress = (address + entry.Key),
								TagDef = entry.Value,
								TagStruct = tagStruct
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
						if (!(entry.Value is Vehi.FlagGroup))
							continue;

						var fg = entry.Value as Vehi.FlagGroup;
						TagFlagsGroup? tfg = new() { HorizontalAlignment = HorizontalAlignment.Left };
						tfg.ValueDefinition = new TagEditorDefinition()
						{
							MemoryType = "Flags",
							MemoryAddress = (address + entry.Key),
							TagDef = entry.Value,
							TagStruct = tagStruct
						};
						tfg.M = _mainWindow.M;
						tfg.mainWindow = _mainWindow;

						parentpanel.Children.Add(tfg);
						tfg.generateBits(address + entry.Key, fg.A, fg.MB, fg.STR);

						break;
				}

				prevEntry = entry;
			}
		}



		private void tagfilter_text_Changed(object sender, TextChangedEventArgs e)
		{
			var text = tagfilter_text.Text;
			var children = tagview_panels.Children;
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
					var tb = (TagBlock) control;
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
					var trb = (TagBlock) control;
					if (((string) trb.tagblock_title.Text).Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagBlock)
				{
					var tb = (TagBlock) control;
					var dp = tb.dockpanel;
					if (dp == null) continue;
					var f = filterTags_Titles(dp.Children, filterText);
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
					var trb = (TagRefBlock) control;
					ComboBoxItem? cbxi = trb.taggroup.SelectedItem as ComboBoxItem;
					if (cbxi == null) continue;

					string? str = cbxi.Content as string;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagValueBlock)
				{
					var trb = (TagValueBlock) control;
					string str = (string) trb.value_type.Text;
					if (str != null && str.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagBlock)
				{
					var tb = (TagBlock) control;
					var dp = tb.dockpanel;
					if (dp == null) continue;
					var f = filterTags_Datatypes(dp.Children, filterText);
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
					var tvb = (TagValueBlock) control;
					var val = tvb.value.Text.ToString();
					if (val != null && val.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagRefBlock)
				{
					var trb = (TagRefBlock) control;
					var val = (string) trb.tag_button.Content;
					if (val != null && val.Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
						control.Visibility = Visibility.Visible;
						found = true;
					}
				}

				else if (control is TagBlock)
				{
					var tb = (TagBlock) control;
					var dp = tb.dockpanel;
					if (dp == null) continue;
					var f = filterTags_Values(dp.Children, filterText);
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