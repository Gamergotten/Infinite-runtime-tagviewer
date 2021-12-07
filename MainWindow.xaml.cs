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
using System.Text.RegularExpressions;
using Assembly69.Halo.TagObjects;
using Assembly69.Interface.Controls;
using AvalonDock.Layout;

namespace Assembly69 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Mem m = new Mem();

        public TagRefDropdown? trd { get; set; } = null; // this is our dropdown box for selecting tag references
        public Button? the_last_tagref_button_we_pressed { get; set; } = null; // since we did it for the window why not also do it for the button

        public Dictionary<string, TagEditorControl> TagEditors { get; set; } = new Dictionary<string, TagEditorControl>();

        public MainWindow()
        {
            InitializeComponent();

            inhale_tagnames();
        }

        public long base_address = -1;
        public int tag_count = -1;

        // Hook to halo infinite
        private async void BtnHook_Click(object sender, RoutedEventArgs e)
        {
            hook_text.Text = "Openning process...";
            m.OpenProcess("HaloInfinite.exe");

            // Get the base address
            base_address = m.ReadLong("HaloInfinite.exe+0x3E82120");
            string validtest = m.ReadString(base_address.ToString("X"));

            if (validtest == "tag instances") {
                hook_text.Text = "Process Hooked";
            } else {
                hook_text.Text = "Offset failed, scanning...";

                long? aobScan = (await m.AoBScan("74 61 67 20 69 6E 73 74 61 6E 63 65 73", true))
                .First(); // "tag instances"

                // Failed to find base tag address
                if (aobScan == null || aobScan == 0) {
                    base_address = -1;
                    hook_text.Text = "Failed to locate base tag address";
                } else {
                    base_address = aobScan.Value;
                    hook_text.Text = "Process Hooked";
                }
            }

            
        }

        public List<tag_struct> Tags_List { get; set; }
        public SortedDictionary<string, group_tag_struct> Tag_groups { get; set; } = new SortedDictionary<string, group_tag_struct>();

        public struct tag_struct
        {
            public string Datnum;

            public string ObjectID;

            public string Tag_group;

            public long Tag_data;

            public string Tag_type_desc;
        }


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
        private void BtnLoadTags_Click(object sender, RoutedEventArgs e) {
            if (tag_count != -1) {
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
            TreeViewItem? item = sender as TreeViewItem;
            string? tagFull = (string) item.Header;
            string? tagName = tagFull.Split('\\').Last();

			// Find the existing layout document ( draggable panel item )
            if (dockManager.Layout.Descendents().OfType<LayoutDocument>().Any()) 
            {
                var dockSearch = dockManager.Layout.Descendents()
                    .OfType<LayoutDocument>()
                    .FirstOrDefault(a => a.ContentId == tagFull);

                // Check if we found the tag
                if (dockSearch != null) 
                {
                    // Set the tag as active
                    if (dockSearch.IsActive) 
                        dockSearch.IsActive = true;

                    // Set the tag as the active tab
					bool found = false;
					var ldp = dockSearch.Parent as AvalonDock.Layout.LayoutDocumentPane;
					if (ldp != null) 
                    {
						for (int x = 0; x < ldp.Children.Count; x++) 
                        {
							var dlp = ldp.Children[x];

							if (dlp == dockSearch) 
                            {
								found = true;
								ldp.SelectedContentIndex = x;
							}
						}
					}

					return;
                }
            }

			// Create the tag editor.
            var tagEditor = new TagEditorControl(this);
            tagEditor.inhale_tag( int.Parse(item.Tag.ToString()) );

			// Create the layout document for docking.
            var doc = new LayoutDocument();
            doc.Title = tagName;
            doc.IsActive = true;
            doc.Content = tagEditor;
            doc.ContentId = tagFull;
			dockLayoutDocPane.Children.Add(doc);
			dockLayoutRoot.ActiveContent = doc;
		}


        // list of changes to ammend to the memory when we phit the poke button
        public Dictionary<long, KeyValuePair<string, string>> pokelist = new Dictionary<long, KeyValuePair<string, string>>();

        // to keep track of the UI elements we're gonna use a dictionary, will probably be better
        public Dictionary<long, TagChangesBlock> UIpokelist = new Dictionary<long, TagChangesBlock>();

        // type (TagrefGroup, TagrefTag)
        // address, 
        public void addpokechange(long offset, string type, string value)
        {
            // hmm we need to change this so we either update or add a new UI element
            pokelist[offset] = new KeyValuePair<string, string>(type, value);

            // there we go, now we aren't touching the pokelist code
            if (UIpokelist.ContainsKey(offset))
            {
                TagChangesBlock update_element = UIpokelist[offset];
                update_element.address.Text = "0x" + offset.ToString("X");
                update_element.type.Text = type;
                update_element.value.Text = value;
            }
            else
            {
                TagChangesBlock NEW_BLOCK = new TagChangesBlock();
                NEW_BLOCK.address.Text = "0x" + offset.ToString("X");
                NEW_BLOCK.type.Text = type;
                NEW_BLOCK.value.Text = value;

                changes_panel.Children.Add(NEW_BLOCK);
                UIpokelist.Add(offset, NEW_BLOCK);
            }

            change_text.Text = pokelist.Count + " changes queued";
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
        private void BtnPokeChanges_Click(object sender, RoutedEventArgs e)
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

        private void BtnClearQueue_Click(object sender, RoutedEventArgs e)
        {
            changes_panel.Children.Clear();
            pokelist.Clear();
            UIpokelist.Clear();
            change_text.Text = pokelist.Count + " changes queued";
        }

        private void DockManager_DocumentClosing(object sender, AvalonDock.DocumentClosingEventArgs e) {
            // On tag window closing.

        }

        private void BtnShowHideQueue_Click(object sender, RoutedEventArgs e) {
            if (changes_panel.Visibility == Visibility.Visible) {
                changes_panel.Visibility = Visibility.Collapsed;
            } else {
                changes_panel.Visibility = Visibility.Visible;
            }
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
