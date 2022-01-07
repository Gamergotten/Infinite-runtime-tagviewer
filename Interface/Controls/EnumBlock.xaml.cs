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
using InfiniteRuntimeTagViewer.Halo;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
	/// <summary>
	/// Interaction logic for EnumBlock.xaml
	/// </summary>
	public partial class EnumBlock : UserControl
	{
		public EnumBlock()
		{
			InitializeComponent();
		}

		public TagEditorDefinition ValueDefinition { get; set; }


		public MainWindow main;
		public string poketype = "";

		public long startAddress;
		public int? amountOfBytes;

		public void initialize_this_block_thanks()
		{

		}

		private void enums_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (main != null)
			{
				main.AddPokeChange(ValueDefinition, enums.SelectedIndex.ToString());

			}
		}
	}
}
