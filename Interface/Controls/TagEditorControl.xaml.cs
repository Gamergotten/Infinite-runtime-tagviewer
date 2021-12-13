using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Assembly69.Halo.TagObjects;
using Assembly69.Interface.Windows;
using AvalonDock.Controls;
using AvalonDock.Layout;
using Memory;
using static Assembly69.MainWindow;

namespace Assembly69.Interface.Controls
{
    /// <summary>
    /// Interaction logic for TagEditorControl.xaml
    /// </summary>
    public partial class TagEditorControl
    {
        private MainWindow _mainWindow;
        private Mem _m;

        public LayoutDocument LayoutDocument { get; internal set; }

        public TagEditorControl(MainWindow mw)
        {
            this._mainWindow = mw;
            this._m = _mainWindow.M;

            InitializeComponent();
        }

        public void inhale_tag(int tagIndex) // as in a literal index to the tag
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
                TextBox tb = new TextBox {Text = "Datnum/ID mismatch; Tag appears to be unloaded, meaning it may not be active on the map, else try reloading the tags" };
                tagview_panels.Children.Add(tb);

                return;
            }

            // there we go, finally fixed that
            switch (loadingTag.TagGroup)
            {
                case "vehi":
                    Dictionary<long, Vehi.C> strings1 = Vehi.VehicleTag;
                    do_the_tag_thing(strings1, loadingTag.TagData, tagview_panels);
                    break;
                case "weap":
                    Dictionary<long, Vehi.C> strings2 = Vehi.WeaponTag; // why // why are theses varaibles considered to be in the same scope
                    do_the_tag_thing(strings2, loadingTag.TagData, tagview_panels);
                    break;
                case "proj":
                    Dictionary<long, Vehi.C> strings3 = Vehi.ProjectileTag;
                    do_the_tag_thing(strings3, loadingTag.TagData, tagview_panels);
                    break;
                case "hlmt":
                    Dictionary<long, Vehi.C> strings4 = Vehi.HlmtTag;
                    do_the_tag_thing(strings4, loadingTag.TagData, tagview_panels);
                    break;
                case "sddt":
                    Dictionary<long, Vehi.C> strings5 = Vehi.SddtTag;
                    do_the_tag_thing(strings5, loadingTag.TagData, tagview_panels);
                    break;
                case "levl":
                    Dictionary<long, Vehi.C> strings6 = Vehi.LevlTag;
                    do_the_tag_thing(strings6, loadingTag.TagData, tagview_panels);
                    break;
                case "effe":
                    Dictionary<long, Vehi.C> strings7 = Vehi.effeTag;
                    do_the_tag_thing(strings7, loadingTag.TagData, tagview_panels);
                    break;
                case "matg":
                    Dictionary<long, Vehi.C> strings8 = Vehi.matgTag;
                    do_the_tag_thing(strings8, loadingTag.TagData, tagview_panels);
                    break;
                case "pmcg":
                    Dictionary<long, Vehi.C> strings9 = Vehi.pmcgTag;
                    do_the_tag_thing(strings9, loadingTag.TagData, tagview_panels);
                    break;
                case "glpa":
                    Dictionary<long, Vehi.C> strings10 = Vehi.glpaTag;
                    do_the_tag_thing(strings10, loadingTag.TagData, tagview_panels);
                    break;
                case "foot":
                    Dictionary<long, Vehi.C> strings11 = Vehi.footTag;
                    do_the_tag_thing(strings11, loadingTag.TagData, tagview_panels);
                    break;
                case "ocgd":
                    Dictionary<long, Vehi.C> strings12 = Vehi.ocgdTag;
                    do_the_tag_thing(strings12, loadingTag.TagData, tagview_panels);
                    break;
                default:
                    TextBox tb = new TextBox { Text = "This tag isn't mapped out ):" };
                    tagview_panels.Children.Add(tb);
                    break;
            }

        }

        // hmm we need a system that reads the pointer and adds it
        // also, we need to beable to read multiple tag things but i may put that on hold
        public void recall_blockloop(KeyValuePair<long, Vehi.C> entry, long loadingTag, VirtualizingStackPanel parentpanel)
        {
            parentpanel.Children.Clear();
            if (entry.Value.B != null)
            {
                do_the_tag_thing(entry.Value.B, loadingTag, parentpanel);
            }
        }

        // for text boxes
        private void value_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string[] s = tb.Tag.ToString().Split(":");
            _mainWindow.Addpokechange(long.Parse(s[0]), s[1], tb.Text);
        }

        // for tag group
        private void taggroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            _mainWindow.Addpokechange(long.Parse(cb.Tag.ToString()), "TagrefGroup", cb.SelectedValue.ToString());

            Grid td = cb.Parent as Grid;
            Button b = td.Children[1] as Button;
            string[] s = b.Tag.ToString().Split(":");
            b.Tag = s[0] + ":" + cb.SelectedValue.ToString();
            // THAT WAS PROBABLY THE MOST DODGY THING IVE EVER DONE WTFFFF
        }

        public void Gotobutton(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var sTagId = b.Tag.ToString();
            int iTagId = int.Parse(sTagId);

            if (iTagId != -1)
            {
                _mainWindow.CreateTagEditorTabByTagIndex(iTagId);
            }
        }

        private DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                parent = tmp;
            }
            return parent;
        }

        private T? GetTopLevelControlOfType<T>(DependencyObject control) where T : DependencyObject
        {
            DependencyObject tmp = control;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // Make sure RECT is actually OUR defined struct, not the windows rect.
        public static RECT GetWindowRectangle(Window window)
        {
            RECT rect;
            GetWindowRect((new WindowInteropHelper(window)).Handle, out rect);

            return rect;
        }

        private void Tagrefbutton(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string[] s = b.Tag.ToString().Split(":");

            if (s.Length > 1)
            {
                var trd = _mainWindow.Trd = new TagRefDropdown();
                var trdWidth = trd.Width = b.ActualWidth + 116;
                var trdHeight = trd.Height = 400;

                //
                Window controlsWindow = GetTopLevelControlOfType<Window>((DependencyObject) sender);

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

                trd.MainWindow = _mainWindow;
                _mainWindow.TheLastTagrefButtonWePressed = b;

                TreeViewItem item = new() {
                    Header = _mainWindow.convert_ID_to_tag_name("FFFFFFFF"),
                    Tag = s[0] + ":" + "FFFFFFFF"
                };

                _mainWindow.Trd.tag_select_panel.Items.Add(item);
                item.Selected += new RoutedEventHandler(update_tagref);

                foreach (TagStruct tg in _mainWindow.TagsList)
                {
                    if (tg.TagGroup == s[1])
                    {
                        TreeViewItem testing = new() {
                            Header = _mainWindow.convert_ID_to_tag_name(tg.ObjectId),
                            Tag = s[0] + ":" + tg.Datnum
                        };

                        _mainWindow.Trd.tag_select_panel.Items.Add(testing);
                        testing.Selected += new RoutedEventHandler(update_tagref);
                    }
                }

                _mainWindow.Trd.Show();
            }
        }

        // this is for our dropdown thingo for changing tag refs
        public void update_tagref(object sender, RoutedEventArgs e)
        {
            TreeViewItem b = sender as TreeViewItem;

            string[] s = b.Tag.ToString().Split(":");
            _mainWindow.Addpokechange(long.Parse(s[0]), "TagrefTag", s[1]);

            string id = _mainWindow.get_tagid_by_datnum(s[1]);
            _mainWindow.TheLastTagrefButtonWePressed.Content = _mainWindow.convert_ID_to_tag_name(id);

            // need to do this the lazy way again, have to head off in a sec
            Grid td = _mainWindow.TheLastTagrefButtonWePressed.Parent as Grid;
            Button x = td.Children[2] as Button;
            //X.Tag = ID;

            x.Tag = _mainWindow.get_tagindex_by_datnum(s[1]);

            if (_mainWindow.Trd != null)
            {
                _mainWindow.Trd.Closethis();
            }
        }

        // had to adapt this to bealbe to read tagblocks and forgot to allow it to iterate through them *sigh* good enough for now
        private void do_the_tag_thing(Dictionary<long, Vehi.C> vehicleTag, long address, VirtualizingStackPanel parentpanel)
        {
            KeyValuePair<long, Vehi.C> prevEntry;
            foreach (KeyValuePair<long, Vehi.C> entry in vehicleTag)
            {
                switch (entry.Value.T)
                {
                    case "4Byte":
                        TagValueBlock vb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        vb1.value_type.Text = "4 Byte";
                        vb1.value.Text = _m.ReadInt((address + entry.Key).ToString("X")).ToString(); // (+entry.Key?) lmao, no wonder why it wasn't working
                        parentpanel.Children.Add(vb1);

                        vb1.value.Tag = address + entry.Key + ":4Byte";
                        vb1.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;
                    case "2Byte":
                        TagValueBlock vb6 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        vb6.value_type.Text = "2 Byte";
                        vb6.value.Text = _m.Read2Byte((address + entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb6);

                        vb6.value.Tag = address + entry.Key + ":2Byte";
                        vb6.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;

                    case "Float":
                        TagValueBlock vb2 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        vb2.value_type.Text = "Float";
                        vb2.value.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb2);

                        vb2.value.Tag = address + entry.Key + ":Float";
                        vb2.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;

                    case "TagRef":
                        TagRefBlock tfb1 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        foreach (string s in _mainWindow.TagGroups.Keys)
                        {
                            tfb1.taggroup.Items.Add(s);
                        }
                        string testGroup = ReverseString(_m.ReadString((address + entry.Key + 20).ToString("X"), "", 4));
                        tfb1.taggroup.SelectedItem = testGroup;

                        // read tagID rather than datnum // or rather, convert datnum to ID
                        string test = BitConverter.ToString(_m.ReadBytes((address + entry.Key + 24).ToString("X"), 4)).Replace("-", string.Empty);
                        string testNameId = _mainWindow.convert_ID_to_tag_name(_mainWindow.get_tagid_by_datnum(test));

                        tfb1.tag_button.Content = testNameId;
                        parentpanel.Children.Add(tfb1);

                        tfb1.taggroup.Tag = (address + entry.Key + 20);
                        tfb1.taggroup.SelectionChanged += new SelectionChangedEventHandler(taggroup_SelectionChanged);

                        tfb1.tag_button.Tag = (address + entry.Key + 24) + ":" + testGroup;
                        tfb1.tag_button.Click += new RoutedEventHandler(Tagrefbutton);

                        int id = _mainWindow.get_tagindex_by_datnum(test);

                        // tag

                        tfb1.goto_button.Tag = id; // need to get the index of the tag not the ID
                        tfb1.goto_button.Click += new RoutedEventHandler(Gotobutton);

                        break;

                    case "Pointer":
                        TagValueBlock vb3 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        vb3.value_type.Text = "Pointer";
                        vb3.value.Text = _m.ReadLong((address + entry.Key).ToString("X")).ToString("X");
                        parentpanel.Children.Add(vb3);

                        vb3.value.Tag = address + entry.Key + ":Pointer";
                        vb3.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;

                    case "Tagblock": // need to find some kinda "whoops that tag isnt actually loaded"; keep erroring with the hlmt tag
                        TagBlock tb1 = new(this) { HorizontalAlignment = HorizontalAlignment.Left };
                        long newAddress = _m.ReadLong((address + entry.Key).ToString("X"));
                        tb1.tagblock_address.Text = "0x" + newAddress.ToString("X");

                        long stringAddress = _m.ReadLong((address + entry.Key + 8).ToString("X"));

                        tb1.tagblock_title.Text = _m.ReadString((address + entry.Key + 8).ToString("X") + ",0,0", "", 100); // this is the only thing that causes errors with unloaded tags
                       
                        //tb1.tagblock_title.Text = "Error: tag Unloaded";
                        //parentpanel.Children.Add(tb1);


                        string childrenCount = _m.ReadInt((address + entry.Key + 16).ToString("X")).ToString();
                        tb1.tagblock_count.Text = childrenCount;
                        parentpanel.Children.Add(tb1);

                        tb1.tagblock_address.Tag = (address + entry.Key) + ":Pointer";
                        tb1.tagblock_address.TextChanged += new TextChangedEventHandler(value_TextChanged);

                        tb1.tagblock_count.Tag = (address + entry.Key + 16) + ":4Byte";
                        tb1.tagblock_count.TextChanged += new TextChangedEventHandler(value_TextChanged);

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
                        TagValueBlock vb4 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        vb4.value_type.Text = "String";
                        vb4.value.Text = _m.ReadString((address + entry.Key).ToString("X"), "", 100).ToString();
                        parentpanel.Children.Add(vb4);

                        vb4.value.Tag = address + entry.Key + ":String";
                        vb4.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;

                    case "Flags":
                        TagsFlags vb9 = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        byte flags_value = (byte)_m.ReadByte((address + entry.Key).ToString("X"));
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
                        //if (1 == 1)
                        //    continue;
                        // make sure we got a flaggroup def
                        if (! (entry.Value is Vehi.FlagGroup)) 
                            continue;
                        
                        var fg = entry.Value as Vehi.FlagGroup;
                        TagFlagsGroup tfg = new() { HorizontalAlignment = HorizontalAlignment.Left };
                        tfg.M = _mainWindow.M;
                        tfg._mainwindow = _mainWindow;

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
                    if (((string)trb.tagblock_title.Text).Contains(filterText, StringComparison.OrdinalIgnoreCase))
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
                    ComboBoxItem cbxi = trb.taggroup.SelectedItem as ComboBoxItem;
                    if (cbxi == null) continue;

                    string str = cbxi.Content as string;
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
    }
}