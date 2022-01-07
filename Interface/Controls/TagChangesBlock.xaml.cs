namespace InfiniteRuntimeTagViewer.Interface.Controls
{
    /// <summary>
    /// Interaction logic for Changesblock.xaml
    /// </summary>
    public partial class TagChangesBlock
    {
        public TagChangesBlock()
        {
            InitializeComponent();
        }

		public long sig_address_ID = -1;
		public string sig_address_path = "";
		public MainWindow main;

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			main.tagchangesblock_fetchdata_by_ID(this);
		}

		private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
		{
			main.clearsingle(this);
		}

		private void value_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			if (main != null)
			{
				main.Update_poke_value(this, value.Text); // this literally causes it to double update the tag
				// not an issue really, but we do this whenever we change the text
				// which just so happens when we update the text from outside of this valueblock
			}
		}
	}
}