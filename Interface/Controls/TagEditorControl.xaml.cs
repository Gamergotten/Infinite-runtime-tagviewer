using Assembly69.Halo.TagObjects;
using Memory;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        public TagEditorControl(MainWindow mw)
        {
            _mainWindow = mw;
            _m = _mainWindow.M;

            InitializeComponent();
        }

        public void inhale_tag(int tagIndex) // as in a literal index to the tag
        {
            TagStruct loadingTag = _mainWindow.TagsList[tagIndex];
            Tagname_text.Text = _mainWindow.convert_ID_to_tag_name(loadingTag.ObjectId);
            tagID_text.Text = "ID: " + loadingTag.ObjectId;
            tagdatnum_text.Text = "Datnum: " + loadingTag.Datnum;
            tagdata_text.Text = "Tag data address: 0x" + loadingTag.TagData.ToString("X");

            tagview_panels.Children.Clear();

            if (loadingTag.TagGroup == "vehi")
            {
                Dictionary<long, Vehi.C> strings = Vehi.VehicleTag;
                do_the_tag_thing(strings, loadingTag.TagData, tagview_panels);
            }
            else if (loadingTag.TagGroup == "weap")
            {
                Dictionary<long, Vehi.C> strings = Vehi.WeaponTag;
                do_the_tag_thing(strings, loadingTag.TagData, tagview_panels);
            }
            else if (loadingTag.TagGroup == "proj")
            {
                Dictionary<long, Vehi.C> strings = Vehi.ProjectileTag;
                do_the_tag_thing(strings, loadingTag.TagData, tagview_panels);
            }
            else if (loadingTag.TagGroup == "hlmt")
            {
                Dictionary<long, Vehi.C> strings = Vehi.HlmtTag;
                do_the_tag_thing(strings, loadingTag.TagData, tagview_panels);
            }
        }

        // hmm we need a system that reads the pointer and adds it
        // also, we need to beable to read multiple tag things but i may put that on hold
        public void recall_blockloop(KeyValuePair<long, Vehi.C> entry, long loadingTag, StackPanel parentpanel)
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
            TextBox tb = sender as TextBox ?? throw new InvalidOperationException();
            string[] s = tb.Tag.ToString()!.Split(":");
            _mainWindow.Addpokechange(long.Parse(s[0]), s[1], tb.Text);
        }

        // for tag group
        private void taggroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox ?? throw new InvalidOperationException();

            _mainWindow.Addpokechange(long.Parse(cb.Tag.ToString()), "TagrefGroup", cb.SelectedValue.ToString());

            Grid td = cb.Parent as Grid;
            Button b = td.Children[1] as Button;
            string[] s = b.Tag.ToString().Split(":");
            b.Tag = s[0] + ":" + cb.SelectedValue;
            // THAT WAS PROBABLY THE MOST DODGY THING IVE EVER DONE WTFFFF
        }

        public void Gotobutton(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int w = int.Parse(b.Tag.ToString());
            if (w != -1)
                inhale_tag(w);
        }

        private void Tagrefbutton(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            string[] s = b.Tag.ToString().Split(":");

            if (s.Length > 1)
            {
                _mainWindow.Trd = new TagRefDropdown();

                var myButtonLocation = b.PointToScreen(new Point(0, 0));

                _mainWindow.Trd.Width = b.ActualWidth + 116;
                _mainWindow.Trd.Height = 400;
                _mainWindow.Trd.Left = myButtonLocation.X - 8;
                _mainWindow.Trd.Top = myButtonLocation.Y + 1;

                _mainWindow.Trd.MainWindow = _mainWindow;
                _mainWindow.TheLastTagrefButtonWePressed = b;

                TreeViewItem item = new TreeViewItem();

                item.Header = _mainWindow.convert_ID_to_tag_name("FFFFFFFF");
                item.Tag = s[0] + ":" + "FFFFFFFF";

                _mainWindow.Trd.tag_select_panel.Items.Add(item);
                item.Selected += update_tagref;

                foreach (TagStruct tg in _mainWindow.TagsList)
                {
                    if (tg.TagGroup == s[1])
                    {
                        TreeViewItem testing = new TreeViewItem();

                        testing.Header = _mainWindow.convert_ID_to_tag_name(tg.ObjectId);
                        testing.Tag = s[0] + ":" + tg.Datnum;

                        _mainWindow.Trd.tag_select_panel.Items.Add(testing);
                        testing.Selected += update_tagref;
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
            x.Tag = id;

            if (_mainWindow.Trd != null)
            {
                _mainWindow.Trd.closethis();
            }
        }

        // had to adapt this to bealbe to read tagblocks and forgot to allow it to iterate through them *sigh* good enough for now
        private void do_the_tag_thing(Dictionary<long, Vehi.C> vehicleTag, long address, StackPanel parentpanel)
        {
            foreach (KeyValuePair<long, Vehi.C> entry in vehicleTag)
            {
                switch (entry.Value.T)
                {
                    case "4Byte":
                        TagValueBlock vb1 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb1.value_type.Text = "4 Byte";
                        vb1.value.Text = _m.ReadInt((+entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb1);

                        vb1.value.Tag = address + entry.Key + ":4Byte";
                        vb1.value.TextChanged += value_TextChanged;
                        break;

                    case "Float":
                        TagValueBlock vb2 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb2.value_type.Text = "Float";
                        vb2.value.Text = _m.ReadFloat((address + entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb2);

                        vb2.value.Tag = address + entry.Key + ":Float";
                        vb2.value.TextChanged += value_TextChanged;
                        break;

                    case "TagRef":
                        TagRefBlock tfb1 = new TagRefBlock { HorizontalAlignment = HorizontalAlignment.Left };
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
                        tfb1.taggroup.SelectionChanged += taggroup_SelectionChanged;

                        tfb1.tag_button.Tag = (address + entry.Key + 24) + ":" + testGroup;
                        tfb1.tag_button.Click += Tagrefbutton;

                        int id = _mainWindow.get_tagindex_by_datnum(test);

                        tfb1.goto_button.Tag = id; // need to get the index of the tag not the ID
                        tfb1.goto_button.Click += Gotobutton;

                        break;

                    case "Pointer":
                        TagValueBlock vb3 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb3.value_type.Text = "Pointer";
                        vb3.value.Text = _m.ReadLong((address + entry.Key).ToString("X")).ToString("X");
                        parentpanel.Children.Add(vb3);

                        vb3.value.Tag = address + entry.Key + ":Pointer";
                        vb3.value.TextChanged += value_TextChanged;
                        break;

                    case "Tagblock": // need to find some kinda "whoops that tag isnt actually loaded"; keep erroring with the hlmt tag
                        TagBlock tb1 = new TagBlock(this) { HorizontalAlignment = HorizontalAlignment.Left };
                        long newAddress = _m.ReadLong((address + entry.Key).ToString("X"));
                        tb1.tagblock_address.Text = "0x" + newAddress.ToString("X");
                        if (newAddress != 0x100000000)
                            tb1.tagblock_title.Text = _m.ReadString((address + entry.Key + 8).ToString("X") + ",0,0");
                        string childrenCount = _m.ReadInt((address + entry.Key + 16).ToString("X")).ToString();
                        tb1.tagblock_count.Text = childrenCount;
                        parentpanel.Children.Add(tb1);

                        tb1.tagblock_address.Tag = (address + entry.Key) + ":Pointer";
                        tb1.tagblock_address.TextChanged += value_TextChanged;

                        tb1.tagblock_count.Tag = (address + entry.Key + 16) + ":4Byte";
                        tb1.tagblock_count.TextChanged += value_TextChanged;

                        //tb1.indexbox.SelectionChanged += new SelectionChangedEventHandler(indexbox_SelectionChanged);

                        tb1.Children = entry;
                        tb1.BlockAddress = newAddress;

                        int childs = int.Parse(childrenCount);
                        for (int y = 0; y < childs; y++)
                        {
                            tb1.indexbox.Items.Add(new ListViewItem { Content = y });
                        }
                        if (childs > 0)
                        {
                            tb1.indexbox.SelectedIndex = 0;
                        }
                        else
                        {
                            tb1.indexbox.IsEnabled = false;
                        }

                        //recall_blockloop(entry, new_address, tb1.dockpanel);
                        break;

                    case "String":
                        TagValueBlock vb4 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb4.value_type.Text = "String";
                        vb4.value.Text = _m.ReadString((address + entry.Key).ToString("X"));
                        parentpanel.Children.Add(vb4);

                        vb4.value.Tag = address + entry.Key + ":String";
                        vb4.value.TextChanged += value_TextChanged;
                        break;
                }
            }
        }
    }
}