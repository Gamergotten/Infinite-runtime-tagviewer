using Assembly69.Halo.TagObjects;
using Assembly69.Interface.Windows;

using AvalonDock.Layout;

using Memory;

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

using static Assembly69.MainWindow;

namespace Assembly69.Interface.Controls {
    /// <summary>
    /// Interaction logic for TagEditorControl.xaml
    /// </summary>
    public partial class TagEditorControl : UserControl {
        MainWindow mainWindow;
        Mem m;

        public LayoutDocument LayoutDocument { get; internal set; }

        public TagEditorControl(MainWindow mw) {
            this.mainWindow = mw;
            this.m = mainWindow.m;

            InitializeComponent();
        }

        public void inhale_tag(int tag_index) // as in a literal index to the tag
        {
            tag_struct loading_tag = mainWindow.Tags_List[tag_index];
            Tagname_text.Text = mainWindow.convert_ID_to_tag_name(loading_tag.ObjectID);
            tagID_text.Text = "ID: " + loading_tag.ObjectID;
            tagdatnum_text.Text = "Datnum: " + loading_tag.Datnum;
            tagdata_text.Text = "Tag data address: 0x" + loading_tag.Tag_data.ToString("X");

            tagview_panels.Children.Clear();

            if (loading_tag.Tag_group == "vehi") {

                Dictionary<long, vehi.c> strings = vehi.VehicleTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            } else if (loading_tag.Tag_group == "weap") {

                Dictionary<long, vehi.c> strings = vehi.WeaponTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            } else if (loading_tag.Tag_group == "proj") {

                Dictionary<long, vehi.c> strings = vehi.projectileTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            } else if (loading_tag.Tag_group == "hlmt") {

                Dictionary<long, vehi.c> strings = vehi.HLMTTag;
                do_the_tag_thing(strings, loading_tag.Tag_data, tagview_panels);
            }
        }

        
        // hmm we need a system that reads the pointer and adds it
        // also, we need to beable to read multiple tag things but i may put that on hold
        public void recall_blockloop(KeyValuePair<long, vehi.c> entry, long loading_tag, StackPanel parentpanel) {
            parentpanel.Children.Clear();
            if (entry.Value.B != null) {
                do_the_tag_thing(entry.Value.B, loading_tag, parentpanel);
            }
        }


        // for text boxes
        private void value_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox tb = sender as TextBox;
            string[] s = tb.Tag.ToString().Split(":");
            mainWindow.addpokechange(long.Parse(s[0]), s[1], tb.Text);
        }

        // for tag group
        private void taggroup_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = sender as ComboBox;

            mainWindow.addpokechange(long.Parse(cb.Tag.ToString()), "TagrefGroup", cb.SelectedValue.ToString());

            Grid td = cb.Parent as Grid;
            Button b = td.Children[1] as Button;
            string[] s = b.Tag.ToString().Split(":");
            b.Tag = s[0] + ":" + cb.SelectedValue.ToString();
            // THAT WAS PROBABLY THE MOST DODGY THING IVE EVER DONE WTFFFF
        }

        public void gotobutton(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            var sTagId = b.Tag.ToString();
            int iTagId = int.Parse(sTagId);

            if (iTagId != -1) {
                mainWindow.CreateTagEditorTabByTagIndex(iTagId);
            }
        }

        // Gets the absolute mouse position, relative to screen
        //Point GetMousePos() => _window.PointToScreen(Mouse.GetPosition(_window));

        DependencyObject GetTopLevelControl(DependencyObject control) {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null) {
                parent = tmp;
            }
            return parent;
        }

        T? GetTopLevelControlOfType<T>(DependencyObject control) where T : DependencyObject {
            DependencyObject tmp = control;
            T? target = default(T);

            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null) {
                System.Diagnostics.Debug.WriteLine("- " + tmp.GetType());

                if (tmp is T)
                    target = (T) tmp;
            }

            return target;
        }

        private Rect GetAbsolutePlacement(FrameworkElement element, bool relativeToScreen = false) {
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            if (relativeToScreen) {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }

            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }

        private void tagrefbutton(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string[] s = b.Tag.ToString().Split(":");

            if (s.Length > 1) {
                var trd = mainWindow.trd = new TagRefDropdown();
                var trdWidth = trd.Width = b.ActualWidth + 116;
                var trdHeight = trd.Height = 400;

                // TODO figure out how to do this on popped out windows
                Window controlsWindow = GetTopLevelControlOfType<Window>((DependencyObject) sender);

                var wind = Window.GetWindow(b);

                // There seems to be no way to get the window
                // that the control is assigned to, I'll come up with
                // a solution soon.
                var ld = this.LayoutDocument;
                LayoutDocumentPane ldp = ld.Parent as LayoutDocumentPane;
                var dockingWindow = ldp.FindParent<LayoutDocumentFloatingWindow>();

                // If we can get the topmost window, use a precise approach
                if (controlsWindow != null) {
                    // Get the control's point relative to the parent window.
                    Point relativeControlLocation = b.TranslatePoint(new Point(0, b.ActualHeight), controlsWindow);
                    
                    // Set the location to the parent window + control location
                    // This sets it to just above the control, by adding the height by a factor of 1.5 it seems
                    // to be an almost fit. 
                    trd.Left = controlsWindow.Left + relativeControlLocation.X;
                    trd.Top  = controlsWindow.Top  + relativeControlLocation.Y + (b.ActualHeight * 1.5);
                }
                // else if (dockingWindow != null) {
                //     //Point relativeControlLocation = b.TranslatePoint(new Point(0, b.ActualHeight), dockingWindow);
                // }
                // If we cannot find the topmost window, use 
                else {
                    var myButtonLocation = b.PointToScreen(new Point(0, 0));
                    
                    trd.Left = myButtonLocation.X - 8;
                    trd.Top = myButtonLocation.Y + 1;
                }

                trd.MainWindow = mainWindow;
                mainWindow.the_last_tagref_button_we_pressed = b;

                TreeViewItem item = new TreeViewItem();

                item.Header = mainWindow.convert_ID_to_tag_name("FFFFFFFF");
                item.Tag = s[0] + ":" + "FFFFFFFF";

                mainWindow.trd.tag_select_panel.Items.Add(item);
                item.Selected += new RoutedEventHandler(update_tagref);


                foreach (tag_struct tg in mainWindow.Tags_List) {
                    if (tg.Tag_group == s[1]) {
                        TreeViewItem testing = new TreeViewItem();

                        testing.Header = mainWindow.convert_ID_to_tag_name(tg.ObjectID);
                        testing.Tag = s[0] + ":" + tg.Datnum;

                        mainWindow.trd.tag_select_panel.Items.Add(testing);
                        testing.Selected += new RoutedEventHandler(update_tagref);
                    }
                }

                mainWindow.trd.Show();
            }
        }


        // this is for our dropdown thingo for changing tag refs
        public void update_tagref(object sender, RoutedEventArgs e) {
            TreeViewItem b = sender as TreeViewItem;

            string[] s = b.Tag.ToString().Split(":");
            mainWindow.addpokechange(long.Parse(s[0]), "TagrefTag", s[1]);

            string ID = mainWindow.get_tagid_by_datnum(s[1]);
            mainWindow.the_last_tagref_button_we_pressed.Content = mainWindow.convert_ID_to_tag_name(ID);

            // need to do this the lazy way again, have to head off in a sec
            Grid td = mainWindow.the_last_tagref_button_we_pressed.Parent as Grid;
            Button X = td.Children[2] as Button;
            X.Tag = ID;

            if (mainWindow.trd != null) {
                mainWindow.trd.closethis();
            }
        }

        // had to adapt this to bealbe to read tagblocks and forgot to allow it to iterate through them *sigh* good enough for now
        void do_the_tag_thing(Dictionary<long, vehi.c> VehicleTag, long address, StackPanel parentpanel) {
            foreach (KeyValuePair<long, vehi.c> entry in VehicleTag) {
                switch (entry.Value.T) {
                case "4Byte":
                    TagValueBlock vb1 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                    vb1.value_type.Text = "4 Byte";
                    vb1.value.Text = m.ReadInt((+entry.Key).ToString("X")).ToString();
                    parentpanel.Children.Add(vb1);

                    vb1.value.Tag = address + entry.Key + ":4Byte";
                    vb1.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                    break;
                case "Float":
                    TagValueBlock vb2 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                    vb2.value_type.Text = "Float";
                    vb2.value.Text = m.ReadFloat((address + entry.Key).ToString("X")).ToString();
                    parentpanel.Children.Add(vb2);

                    vb2.value.Tag = address + entry.Key + ":Float";
                    vb2.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                    break; 
                case "TagRef":
                    TagRefBlock tfb1 = new TagRefBlock { HorizontalAlignment = HorizontalAlignment.Left };
                    foreach (string s in mainWindow.Tag_groups.Keys) {
                        tfb1.taggroup.Items.Add(s);
                    }
                    string test_group = ReverseString(m.ReadString((address + entry.Key + 20).ToString("X"), "", 4));
                    tfb1.taggroup.SelectedItem = test_group;

                    // read tagID rather than datnum // or rather, convert datnum to ID
                    string test = BitConverter.ToString(m.ReadBytes((address + entry.Key + 24).ToString("X"), 4)).Replace("-", string.Empty);
                    string test_nameID = mainWindow.convert_ID_to_tag_name(mainWindow.get_tagid_by_datnum(test));

                    tfb1.tag_button.Content = test_nameID;
                    parentpanel.Children.Add(tfb1);

                    tfb1.taggroup.Tag = (address + entry.Key + 20);
                    tfb1.taggroup.SelectionChanged += new SelectionChangedEventHandler(taggroup_SelectionChanged);

                    tfb1.tag_button.Tag = (address + entry.Key + 24) + ":" + test_group;
                    tfb1.tag_button.Click += new RoutedEventHandler(tagrefbutton);

                    int ID = mainWindow.get_tagindex_by_datnum(test);

                    // tag

                    tfb1.goto_button.Tag = ID; // need to get the index of the tag not the ID
                    tfb1.goto_button.Click += new RoutedEventHandler(gotobutton);

                    break;
                case "Pointer":
                    TagValueBlock vb3 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                    vb3.value_type.Text = "Pointer";
                    vb3.value.Text = m.ReadLong((address + entry.Key).ToString("X")).ToString("X");
                    parentpanel.Children.Add(vb3);

                    vb3.value.Tag = address + entry.Key + ":Pointer";
                    vb3.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                    break;
                case "Tagblock": // need to find some kinda "whoops that tag isnt actually loaded"; keep erroring with the hlmt tag
                    TagBlock tb1 = new TagBlock (this) { HorizontalAlignment = HorizontalAlignment.Left };
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
                    tb1.block_address = new_address;

                    int childs = int.Parse(children_count);
                    for (int y = 0; y < childs; y++) {
                        tb1.indexbox.Items.Add(new ListViewItem { Content = y });
                    }
                    if (childs > 0) {
                        tb1.indexbox.SelectedIndex = 0;

                    } else {
                        tb1.indexbox.IsEnabled = false;
                    }

                    //recall_blockloop(entry, new_address, tb1.dockpanel);
                    break;

                case "String":
                    TagValueBlock vb4 = new TagValueBlock { HorizontalAlignment = HorizontalAlignment.Left };
                    vb4.value_type.Text = "String";
                    vb4.value.Text = m.ReadString((address + entry.Key).ToString("X")).ToString();
                    parentpanel.Children.Add(vb4);

                    vb4.value.Tag = address + entry.Key + ":String";
                    vb4.value.TextChanged += new TextChangedEventHandler(value_TextChanged);
                    break;

                }
            }
        }
    }
}
