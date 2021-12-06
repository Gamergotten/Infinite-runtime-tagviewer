using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Memory;
using System.IO;
using Assembly69.object_classes;
using Assembly69.theUIstuff;
using System.Text.RegularExpressions;

namespace Assembly69
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mem m = new Mem();

        public tagref_dropdown trd; // this is our dropdown box for selecting tag references
        public Button the_last_tagref_button_we_pressed; // since we did it for the window why not also do it for the button

        public MainWindow()
        {
            InitializeComponent();

            inhale_tagnames();
        }

        public long base_address = -1;

        public int tag_count = -1;

        // hook to halo infinite
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m.OpenProcess("HaloInfinite.exe");
            base_address = m.ReadLong("HaloInfinite.exe+0x3E82120");

            string validtest = m.ReadString(base_address.ToString("X"));
            if(validtest == "tag instances")
            {
                hook_text.Text = "Process Hooked";
            }
            else
            {
                hook_text.Text = "Epic Hook fail";
            }
        }

        public List<tag_struct> Tags_List;

        public struct tag_struct
        {
            public string Datnum;

            public string ObjectID;

            public string Tag_group;

            public long Tag_data;

            public string Tag_type_desc;
        }

        public SortedDictionary<string, group_tag_struct> Tag_groups = new SortedDictionary<string, group_tag_struct>();

        public struct group_tag_struct
        {
            public string tag_group_desc;

            public string tag_group_name;

            public string tag_group_definitition;

            public string tag_extra_type;

            public string tag_extra_name;

            public TreeViewItem tag_category;
        }


        // load tags from Mem
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (tag_count != -1)
            {
                tag_count = -1;
                Tag_groups.Clear();
                Tags_List.Clear();
            }
            


            TagsTree.Items.Clear();

            tag_count = m.ReadInt((base_address + 0x6C).ToString("X"));
            long tags_start = m.ReadLong((base_address + 0x78).ToString("X"));

            // each tag is 52 bytes long // was it 52 or was it 0x52? whatever
            // 0x0 datnum 4bytes
            // 0x4 ObjectID 4bytes
            // 0x8 Tag_group Pointer 8bytes
            // 0x10 Tag_data Pointer 8bytes
            // 0x18 Tag_type_desc Pointer 8bytes

            Tags_List = new List<tag_struct>();

            for (int tag_index = 0; tag_index < tag_count; tag_index++)
            {
                tag_struct current_tag = new tag_struct();

                long tag_address = tags_start + (tag_index*52);

                
                byte[] test1 =  m.ReadBytes(tag_address.ToString("X"), 4);

                current_tag.Datnum = BitConverter.ToString(test1).Replace("-", string.Empty);

                byte[] test = (m.ReadBytes((tag_address + 4).ToString("X"), 4));

                // = String.Concat(bytes.Where(c => !Char.IsWhiteSpace(c)));
                current_tag.ObjectID = BitConverter.ToString(test).Replace("-", string.Empty);

                current_tag.Tag_group = read_tag_group(m.ReadLong((tag_address + 0x8).ToString("X")));

                current_tag.Tag_data = m.ReadLong((tag_address+0x10).ToString("X"));

                // do the tag definitition


                Tags_List.Add(current_tag);
            }
            Loadtags();
        }

        public string read_tag_group(long tag_group_address)
        {
            string key = ReverseString(m.ReadString((tag_group_address + 0xC).ToString("X"), "", 8).Substring(0, 4));
            if (!Tag_groups.ContainsKey(key))
            {
                group_tag_struct current_group = new group_tag_struct();

                current_group.tag_group_desc = m.ReadString((tag_group_address).ToString("X") + ",0x0");

                current_group.tag_group_name = key;

                current_group.tag_group_definitition = m.ReadString((tag_group_address + 0x20).ToString("X") + ",0x0,0x0");

                current_group.tag_extra_type = m.ReadString((tag_group_address + 0x2C).ToString("X"), "", 12);

                long test_address = m.ReadLong((tag_group_address + 0x48).ToString("X"));
                if (test_address != 0)
                    current_group.tag_extra_name = m.ReadString((test_address).ToString("X"));

                // Doing the UI here so we dont have to literally reconstruct the elements elsewhere

                //TreeViewItem sortheader = new TreeViewItem();

                //sortheader.Header = ReverseString(current_group.tag_group_name.Substring(0, 4)) + " (" + current_group.tag_group_desc + ")";

                //sortheader.ToolTip = current_group.tag_group_definitition;

                //TagsTree.Items.Add(sortheader);

                //current_group.tag_category = sortheader;

                Tag_groups.Add(key, current_group);
            }


            return key;
        }

        public void Loadtags()
        {
            // TagsTree

            for (int i = 0; i < Tag_groups.Count; i++)
            {
                group_tag_struct display_group = Tag_groups.ElementAt(i).Value;

                TreeViewItem sortheader = new TreeViewItem();

                sortheader.Header = display_group.tag_group_name + " (" + display_group.tag_group_desc + ")";

                sortheader.ToolTip = display_group.tag_group_definitition;

                display_group.tag_category = sortheader;

                TagsTree.Items.Add(sortheader);

                Tag_groups[Tag_groups.ElementAt(i).Key] = display_group;
            }


            for (int i = 0; i < Tags_List.Count; i++)
            {
                TreeViewItem t = new TreeViewItem();
                tag_struct tag = Tags_List[i];
                Tag_groups.TryGetValue(tag.Tag_group, out group_tag_struct dict_tag_group);

                t.Header = "(" + tag.Datnum + ") " + convert_ID_to_tag_name(tag.ObjectID);
                t.Tag = i;
                //t.MouseLeftButtonDown += new MouseButtonEventHandler(Select_Tag_click);
                t.Selected += Select_Tag_click;

                dict_tag_group.tag_category.Items.Add(t);

            }

        }

        public Dictionary<string, string> inhaled_tagnames = new Dictionary<string, string>();

        public void inhale_tagnames()
        {
            string filename = Directory.GetCurrentDirectory() + @"\files\tagnames.txt";
            var lines = System.IO.File.ReadLines(filename);
            foreach (var line in lines)
            {
                string[] hex_string = line.Split(" : ");
                if (!inhaled_tagnames.ContainsKey(hex_string[0]))
                inhaled_tagnames.Add(hex_string[0], hex_string[1]);
            }
        }

        public string convert_ID_to_tag_name(string value)
        {

            inhaled_tagnames.TryGetValue(value, out string potential_name);

            if (potential_name == null)
            {
                potential_name = "ObjectID: " + value;
            }

            return potential_name;
        }

        public static string ReverseString(string myStr)
        {
            char[] myArr = myStr.ToCharArray();
            Array.Reverse(myArr);
            return new string(myArr);
        }


        private void Select_Tag_click(object sender, RoutedEventArgs e)
        {
            TreeViewItem thing = sender as TreeViewItem;
            
            inhale_tag(int.Parse(thing.Tag.ToString()));
        }

        public void inhale_tag(int tag_index) // as in a literal index to the tag
        {
            tag_struct loading_tag = Tags_List[tag_index];
            Tagname_text.Text = convert_ID_to_tag_name(loading_tag.ObjectID);
            tagID_text.Text = "ID: " + loading_tag.ObjectID;
            tagdatnum_text.Text = "Datnum: " + loading_tag.Datnum;
            tagdata_text.Text = "Tag data address: 0x" + loading_tag.Tag_data.ToString("X");

            tagview_panels.Children.Clear();

            if (loading_tag.Tag_group == "vehi")
            {

                Dictionary<long, vehi.c> strings = vehi.VehicleTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            }
            else if (loading_tag.Tag_group == "weap")
            {

                Dictionary<long, vehi.c> strings = vehi.WeaponTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            }
            else if (loading_tag.Tag_group == "proj")
            {

                Dictionary<long, vehi.c> strings = vehi.projectileTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            }
            else if (loading_tag.Tag_group == "hlmt")
            {

                Dictionary<long, vehi.c> strings = vehi.HLMTTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            }

        }

        // had to adapt this to bealbe to read tagblocks and forgot to allow it to iterate through them *sigh* good enough for now
        void do_the_tag_thing(Dictionary<long, vehi.c> VehicleTag, long address, StackPanel parentpanel)
        {
            foreach (KeyValuePair<long, vehi.c> entry in VehicleTag)
            {
                switch (entry.Value.T)
                {
                    case "4Byte":
                        valueBlock vb1 = new valueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb1.value_type.Text = "4 Byte";
                        vb1.value.Text = m.ReadInt(( + entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb1);

                        vb1.value.Tag = address + entry.Key + ":4Byte";
                        vb1.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;
                    case "Float":
                        valueBlock vb2 = new valueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb2.value_type.Text = "Float";
                        vb2.value.Text = m.ReadFloat((address + entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb2);

                        vb2.value.Tag = address + entry.Key + ":Float";
                        vb2.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;
                    case "TagRef":
                        tagrefblock tfb1 = new tagrefblock { HorizontalAlignment = HorizontalAlignment.Left };
                        foreach (string s in Tag_groups.Keys)
                        {
                            tfb1.taggroup.Items.Add(s);
                        }
                        string test_group = ReverseString(m.ReadString((address + entry.Key + 20).ToString("X"), "", 4));
                        tfb1.taggroup.SelectedItem = test_group;

                        // read tagID rather than datnum // or rather, convert datnum to ID
                        string test = BitConverter.ToString(m.ReadBytes((address + entry.Key + 24).ToString("X"), 4)).Replace("-", string.Empty);
                        string test_nameID = convert_ID_to_tag_name(get_tagid_by_datnum(test));

                        tfb1.tag_button.Content = test_nameID;
                        parentpanel.Children.Add(tfb1);

                        tfb1.taggroup.Tag = (address + entry.Key + 20);
                        tfb1.taggroup.SelectionChanged += new SelectionChangedEventHandler(taggroup_SelectionChanged);

                        tfb1.tag_button.Tag = (address + entry.Key + 24) + ":" + test_group;
                        tfb1.tag_button.Click += new RoutedEventHandler(tagrefbutton);

                        int ID = get_tagindex_by_datnum(test);

                        tfb1.goto_button.Tag = ID; // need to get the index of the tag not the ID
                        tfb1.goto_button.Click += new RoutedEventHandler(gotobutton);

                        break;
                    case "Pointer":
                        valueBlock vb3 = new valueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb3.value_type.Text = "Pointer";
                        vb3.value.Text = m.ReadLong((address + entry.Key).ToString("X")).ToString("X");
                        parentpanel.Children.Add(vb3);

                        vb3.value.Tag = address + entry.Key + ":Pointer";
                        vb3.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;
                    case "Tagblock": // need to find some kinda "whoops that tag isnt actually loaded"; keep erroring with the hlmt tag
                        tagblock tb1 = new tagblock { HorizontalAlignment = HorizontalAlignment.Left };
                        long new_address = m.ReadLong((address + entry.Key).ToString("X"));
                        tb1.tagblock_address.Text = "0x" + new_address.ToString("X");
                        if (new_address != 0x100000000)
                            tb1.tagblock_title.Text = m.ReadString((address + entry.Key + 8).ToString("X") + ",0,0");
                        string children_count = m.ReadInt((address + entry.Key + 16).ToString("X")).ToString();
                        tb1.tagblock_count.Text = children_count;
                        parentpanel.Children.Add(tb1);

                        tb1.tagblock_address.Tag = (address + entry.Key) + ":Pointer";
                        tb1.tagblock_address.TextChanged += new TextChangedEventHandler(value_TextChanged);

                        tb1.tagblock_count.Tag = (address + entry.Key + 16) + ":4Byte";
                        tb1.tagblock_count.TextChanged += new TextChangedEventHandler(value_TextChanged);

                        //tb1.indexbox.SelectionChanged += new SelectionChangedEventHandler(indexbox_SelectionChanged);

                        tb1.children = entry;

                        tb1.mainWindow = this;
                        tb1.block_address = new_address;
                        int childs = int.Parse(children_count);
                        for (int y=0; y <childs; y++)
                        {
                            tb1.indexbox.Items.Add(new ListViewItem { Content=y });
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
                        valueBlock vb4 = new valueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                        vb4.value_type.Text = "String";
                        vb4.value.Text = m.ReadString((address + entry.Key).ToString("X")).ToString();
                        parentpanel.Children.Add(vb4);

                        vb4.value.Tag = address + entry.Key + ":String";
                        vb4.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                        break;

                }
                
            }
        }
        // hmm we need a system that reads the pointer and adds it
        // also, we need to beable to read multiple tag things but i may put that on hold
        public void recall_blockloop(KeyValuePair<long, vehi.c> entry, long loading_tag, StackPanel parentpanel)
        {
            parentpanel.Children.Clear();
            if (entry.Value.B != null)
            {
                do_the_tag_thing(entry.Value.B, loading_tag, parentpanel);
            }

        }




        // list of changes to ammend to the memory when we phit the poke button
        public Dictionary<long, KeyValuePair<string, string>> pokelist = new Dictionary<long, KeyValuePair<string, string>>();

        // to keep track of the UI elements we're gonna use a dictionary, will probably be better
        public Dictionary<long, Changesblock> UIpokelist = new Dictionary<long, Changesblock>();

        // type (TagrefGroup, TagrefTag)
        // address, 
        public void addpokechange(long offset, string type, string value)
        {
            // hmm we need to change this so we either update or add a new UI element

            pokelist[offset] = new KeyValuePair<string, string>(type, value);
            // there we go, now we aren't touching the pokelist code
            if (UIpokelist.ContainsKey(offset))
            {
                Changesblock update_element = UIpokelist[offset];
                update_element.address.Text = "0x" + offset.ToString("X");
                update_element.type.Text = type;
                update_element.value.Text = value;
            }
            else
            {
                Changesblock NEW_BLOCK = new Changesblock();
                NEW_BLOCK.address.Text = "0x" + offset.ToString("X");
                NEW_BLOCK.type.Text = type;
                NEW_BLOCK.value.Text = value;

                changes_panel.Children.Add(NEW_BLOCK);
                UIpokelist.Add(offset, NEW_BLOCK);
            }


            change_text.Text = pokelist.Count + " changes queued";
        }

        // for text boxes
        private void value_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string[] s = tb.Tag.ToString().Split(":");
            addpokechange(long.Parse(s[0]), s[1], tb.Text);
        }
        // for tag group
        private void taggroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            addpokechange(long.Parse(cb.Tag.ToString()), "TagrefGroup", cb.SelectedValue.ToString());

            Grid td = cb.Parent as Grid;
            Button b = td.Children[1] as Button;
            string[] s = b.Tag.ToString().Split(":");
            b.Tag = s[0] + ":" + cb.SelectedValue.ToString();
            // THAT WAS PROBABLY THE MOST DODGY THING IVE EVER DONE WTFFFF
        }

        public void gotobutton(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int w = int.Parse(b.Tag.ToString());
            if (w != -1)
                inhale_tag(w);
        }

        private void tagrefbutton(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            string[] s = b.Tag.ToString().Split(":");

            if (s.Length > 1)
            {
                trd = new tagref_dropdown();

                var myButtonLocation = b.PointToScreen(new Point(0, 0));

                trd.Width = b.ActualWidth + 116;
                trd.Height = 400;
                trd.Left = myButtonLocation.X - 8;
                trd.Top = myButtonLocation.Y + 1;



                trd.MainWindow = this;
                the_last_tagref_button_we_pressed = b;

                TreeViewItem NULL = new TreeViewItem();

                NULL.Header = convert_ID_to_tag_name("FFFFFFFF");
                NULL.Tag = s[0] + ":" + "FFFFFFFF";

                trd.tag_select_panel.Items.Add(NULL);
                NULL.Selected += new RoutedEventHandler(update_tagref);


                foreach (tag_struct tg in Tags_List)
                {
                    if (tg.Tag_group == s[1])
                    {
                        TreeViewItem testing = new TreeViewItem();

                        testing.Header = convert_ID_to_tag_name(tg.ObjectID);
                        testing.Tag = s[0] + ":" + tg.Datnum;

                        trd.tag_select_panel.Items.Add(testing);
                        testing.Selected += new RoutedEventHandler(update_tagref);
                    }
                }
                
                trd.Show();
            }
        }

        // this is for our dropdown thingo for changing tag refs
        public void update_tagref(object sender, RoutedEventArgs e)
        {
            TreeViewItem b = sender as TreeViewItem;

            string[] s = b.Tag.ToString().Split(":");
            addpokechange(long.Parse(s[0]), "TagrefTag", s[1]);
            string ID = get_tagid_by_datnum(s[1]);
            the_last_tagref_button_we_pressed.Content = convert_ID_to_tag_name(ID);
            // need to do this the lazy way again, have to head off in a sec
            Grid td = the_last_tagref_button_we_pressed.Parent as Grid;
            Button X = td.Children[2] as Button;
            X.Tag = ID;

            if (trd != null)
            {
                trd.closethis();
            }
        }

        // need this to read tagref blocks - because we only get a datnum to figure out the name with
        // so we find what else has the same datnum and then run the other method to get name based off of ID
        public string get_tagid_by_datnum(string datnum)
        {
            foreach (tag_struct t in Tags_List)
            {
                if (t.Datnum == datnum)
                    return t.ObjectID;
            }

            return "Tag not present(" + datnum + ")";
        }

        public int get_tagindex_by_datnum(string datnum)
        {
            //tag_struct t in Tags_List
            for (int i = 0; i < Tags_List.Count; i++)
            {
                tag_struct t = Tags_List[i];
                if (t.Datnum == datnum)
                    return i;
            }

            return -1;
        }

        // search filter
        private void Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = Searchbox.Text;
            foreach(TreeViewItem tv in TagsTree.Items)
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



        // POKE OUR CHANGES LETSGOOOO
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (KeyValuePair<long, KeyValuePair<string, string>> pair in pokelist)
            {
                long address = pair.Key;
                string type = pair.Value.Key;
                string value = pair.Value.Value;
                switch (type)
                {
                    case "4Byte":
                        m.WriteMemory(address.ToString("X"), "int", value);
                        break;
                    case "Float":
                        m.WriteMemory(address.ToString("X"), "float", value);
                        break;
                    case "Pointer":
                        string will_this_work = new System.ComponentModel.Int64Converter().ConvertFromString(value).ToString();
                        m.WriteMemory(address.ToString("X"), "long", will_this_work); // apparently it does
                        break;
                    case "String":
                        m.WriteMemory(address.ToString("X"), "string", value + "\0");
                        break;
                    case "TagrefGroup":
                        m.WriteMemory(address.ToString("X"), "string", ReverseString(value));

                        break;
                    case "TagrefTag":
                        string why_do_i_need_to_convert_EVERYTHING = Convert.ToInt32(value, 16).ToString();
                        // THAT FLIPS IT BACKWARDS

                        string temp = Regex.Replace(value, @"(.{2})", "$1 ");
                        temp = temp.TrimEnd();
                        m.WriteMemory(address.ToString("X"), "bytes", temp);
                        int w2 = 0;


                        break;
                }
            }

            poke_text.Text = pokelist.Count + " changes poked";

            changes_panel.Children.Clear();
            pokelist.Clear();
            UIpokelist.Clear();
            change_text.Text = pokelist.Count + " changes queued";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            changes_panel.Children.Clear();
            pokelist.Clear();
            UIpokelist.Clear();
            change_text.Text = pokelist.Count + " changes queued";
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
    }
}
