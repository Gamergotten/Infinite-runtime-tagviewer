using Assembly69.Interface.Controls;
using AvalonDock;
using AvalonDock.Layout;
using Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Assembly69
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Mem M = new Mem();

        public TagRefDropdown? Trd { get; set; } = null; // this is our dropdown box for selecting tag references
        public Button? TheLastTagrefButtonWePressed { get; set; } = null; // since we did it for the window why not also do it for the button

        public Dictionary<string, TagEditorControl> TagEditors { get; set; } = new Dictionary<string, TagEditorControl>();

        public MainWindow()
        {
            InitializeComponent();

            inhale_tagnames();
        }

        public long BaseAddress = -1;
        public int TagCount = -1;

        // Hook to halo infinite
        private async void BtnHook_Click(object sender, RoutedEventArgs e)
        {
            hook_text.Text = "Openning process...";
            M.OpenProcess("HaloInfinite.exe");

            // Get the base address
            BaseAddress = M.ReadLong("HaloInfinite.exe+0x3E82120");
            string validtest = M.ReadString(BaseAddress.ToString("X"));

            if (validtest == "tag instances")
            {
                hook_text.Text = "Process Hooked";
            }
            else
            {
                hook_text.Text = "Offset failed, scanning...";

                long? aobScan = (await M.AoBScan("74 61 67 20 69 6E 73 74 61 6E 63 65 73", true))
                .First(); // "tag instances"

                // Failed to find base tag address
                if (aobScan == null || aobScan == 0)
                {
                    BaseAddress = -1;
                    hook_text.Text = "Failed to locate base tag address";
                }
                else
                {
                    BaseAddress = aobScan.Value;
                    hook_text.Text = "Process Hooked";
                }
            }
        }

        public List<TagStruct> TagsList { get; set; } = null!;
        public SortedDictionary<string, GroupTagStruct> TagGroups { get; set; } = new SortedDictionary<string, GroupTagStruct>();

        public struct TagStruct
        {
            public string Datnum;

            public string ObjectId;

            public string TagGroup;

            public long TagData;

            public string TagTypeDesc;
        }

        public struct GroupTagStruct
        {
            public string TagGroupDesc;

            public string TagGroupName;

            public string TagGroupDefinitition;

            public string TagExtraType;

            public string TagExtraName;

            public TreeViewItem TagCategory;
        }

        // load tags from Mem
        private void BtnLoadTags_Click(object sender, RoutedEventArgs e)
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

            TagsList = new List<TagStruct>();

            for (int tagIndex = 0; tagIndex < TagCount; tagIndex++)
            {
                TagStruct currentTag = new TagStruct();

                long tagAddress = tagsStart + (tagIndex * 52);

                byte[] test1 = M.ReadBytes(tagAddress.ToString("X"), 4);

                currentTag.Datnum = BitConverter.ToString(test1).Replace("-", string.Empty);

                byte[] test = (M.ReadBytes((tagAddress + 4).ToString("X"), 4));

                // = String.Concat(bytes.Where(c => !Char.IsWhiteSpace(c)));
                currentTag.ObjectId = BitConverter.ToString(test).Replace("-", string.Empty);

                currentTag.TagGroup = read_tag_group(M.ReadLong((tagAddress + 0x8).ToString("X")));

                currentTag.TagData = M.ReadLong((tagAddress + 0x10).ToString("X"));

                // do the tag definitition
                TagsList.Add(currentTag);
            }
            Loadtags();
        }

        public string read_tag_group(long tagGroupAddress)
        {
            string key = ReverseString(M.ReadString((tagGroupAddress + 0xC).ToString("X"), "", 8).Substring(0, 4));
            if (!TagGroups.ContainsKey(key))
            {
                GroupTagStruct currentGroup = new GroupTagStruct();

                currentGroup.TagGroupDesc = M.ReadString((tagGroupAddress).ToString("X") + ",0x0");

                currentGroup.TagGroupName = key;

                currentGroup.TagGroupDefinitition = M.ReadString((tagGroupAddress + 0x20).ToString("X") + ",0x0,0x0");

                currentGroup.TagExtraType = M.ReadString((tagGroupAddress + 0x2C).ToString("X"), "", 12);

                long testAddress = M.ReadLong((tagGroupAddress + 0x48).ToString("X"));
                if (testAddress != 0)
                    currentGroup.TagExtraName = M.ReadString((testAddress).ToString("X"));

                // Doing the UI here so we dont have to literally reconstruct the elements elsewhere
                //TreeViewItem sortheader = new TreeViewItem();
                //sortheader.Header = ReverseString(current_group.tag_group_name.Substring(0, 4)) + " (" + current_group.tag_group_desc + ")";
                //sortheader.ToolTip = current_group.tag_group_definitition;
                //TagsTree.Items.Add(sortheader);
                //current_group.tag_category = sortheader;

                TagGroups.Add(key, currentGroup);
            }

            return key;
        }

        public void Loadtags()
        {
            // TagsTree

            for (int i = 0; i < TagGroups.Count; i++)
            {
                GroupTagStruct displayGroup = TagGroups.ElementAt(i).Value;

                TreeViewItem sortheader = new TreeViewItem();

                sortheader.Header = displayGroup.TagGroupName + " (" + displayGroup.TagGroupDesc + ")";

                sortheader.ToolTip = displayGroup.TagGroupDefinitition;

                displayGroup.TagCategory = sortheader;

                TagsTree.Items.Add(sortheader);

                TagGroups[TagGroups.ElementAt(i).Key] = displayGroup;
            }

            for (int i = 0; i < TagsList.Count; i++)
            {
                TreeViewItem t = new TreeViewItem();
                TagStruct tag = TagsList[i];
                TagGroups.TryGetValue(tag.TagGroup, out GroupTagStruct dictTagGroup);

                t.Header = "(" + tag.Datnum + ") " + convert_ID_to_tag_name(tag.ObjectId);
                t.Tag = i;
                //t.MouseLeftButtonDown += new MouseButtonEventHandler(Select_Tag_click);
                t.Selected += Select_Tag_click;

                dictTagGroup.TagCategory.Items.Add(t);
            }
        }

        public Dictionary<string, string> InhaledTagnames = new Dictionary<string, string>();

        public void inhale_tagnames()
        {
            string filename = Directory.GetCurrentDirectory() + @"\files\tagnames.txt";
            var lines = File.ReadLines(filename);
            foreach (var line in lines)
            {
                string[] hexString = line.Split(" : ");
                if (!InhaledTagnames.ContainsKey(hexString[0]))
                    InhaledTagnames.Add(hexString[0], hexString[1]);
            }
        }

        public string convert_ID_to_tag_name(string value)
        {
            InhaledTagnames.TryGetValue(value, out string potentialName);

            if (potentialName == null)
            {
                potentialName = "ObjectID: " + value;
            }

            return potentialName;
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
            string? tagFull = (string)item.Header;
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
                    var ldp = dockSearch.Parent as LayoutDocumentPane;
                    if (ldp != null)
                    {
                        for (int x = 0; x < ldp.Children.Count; x++)
                        {
                            var dlp = ldp.Children[x];

                            if (dlp == dockSearch)
                            {
                                ldp.SelectedContentIndex = x;
                            }
                        }
                    }

                    return;
                }
            }

            // Create the tag editor.
            var tagEditor = new TagEditorControl(this);
            tagEditor.inhale_tag(int.Parse(item.Tag.ToString()));

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
        public Dictionary<long, KeyValuePair<string, string>> Pokelist = new Dictionary<long, KeyValuePair<string, string>>();

        // to keep track of the UI elements we're gonna use a dictionary, will probably be better
        public Dictionary<long, TagChangesBlock> UIpokelist = new Dictionary<long, TagChangesBlock>();

        // type (TagrefGroup, TagrefTag)
        // address,
        public void Addpokechange(long offset, string type, string value)
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
                TagChangesBlock newBlock = new TagChangesBlock();
                newBlock.address.Text = "0x" + offset.ToString("X");
                newBlock.type.Text = type;
                newBlock.value.Text = value;

                changes_panel.Children.Add(newBlock);
                UIpokelist.Add(offset, newBlock);
            }

            change_text.Text = Pokelist.Count + " changes queued";
        }

        // need this to read tagref blocks - because we only get a datnum to figure out the name with
        // so we find what else has the same datnum and then run the other method to get name based off of ID
        public string get_tagid_by_datnum(string datnum)
        {
            foreach (TagStruct t in TagsList)
            {
                if (t.Datnum == datnum)
                    return t.ObjectId;
            }

            return "Tag not present(" + datnum + ")";
        }

        public int get_tagindex_by_datnum(string datnum)
        {
            //tag_struct t in Tags_List
            for (int i = 0; i < TagsList.Count; i++)
            {
                TagStruct t = TagsList[i];
                if (t.Datnum == datnum)
                    return i;
            }

            return -1;
        }

        // search filter
        private void Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = Searchbox.Text;
            foreach (TreeViewItem tv in TagsTree.Items)
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

                    case "Float":
                        M.WriteMemory(address.ToString("X"), "float", value);
                        break;

                    case "Pointer":
                        string willThisWork = new Int64Converter().ConvertFromString(value).ToString();
                        M.WriteMemory(address.ToString("X"), "long", willThisWork); // apparently it does
                        break;

                    case "String":
                        M.WriteMemory(address.ToString("X"), "string", value + "\0");
                        break;

                    case "TagrefGroup":
                        M.WriteMemory(address.ToString("X"), "string", ReverseString(value));

                        break;

                    case "TagrefTag":
                        string whyDoINeedToConvertEverything = Convert.ToInt32(value, 16).ToString();
                        // THAT FLIPS IT BACKWARDS

                        string temp = Regex.Replace(value, @"(.{2})", "$1 ");
                        temp = temp.TrimEnd();
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

        private void DockManager_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            // On tag window closing.
        }

        private void BtnShowHideQueue_Click(object sender, RoutedEventArgs e)
        {
            if (changes_panel.Visibility == Visibility.Visible)
            {
                changes_panel.Visibility = Visibility.Collapsed;
            }
            else
            {
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