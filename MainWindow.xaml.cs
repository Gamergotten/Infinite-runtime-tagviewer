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

namespace Assembly69
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mem m = new Mem();

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
            if(validtest != null)
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

                t.Header = convert_ID_to_tag_name(tag.ObjectID);
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

        public void inhale_tag(int tag_index)
        {
            tag_struct loading_tag = Tags_List[tag_index];
            Tagname_text.Text = convert_ID_to_tag_name(loading_tag.ObjectID);
            tagID_text.Text = "ID: " + loading_tag.ObjectID;
            tagdatnum_text.Text = "Datnum: " + loading_tag.Datnum;
            tagdata_text.Text = "Tag data address: 0x" + loading_tag.Tag_data.ToString("X");

        }




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

    }
}
