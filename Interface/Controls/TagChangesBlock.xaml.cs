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
	}
}