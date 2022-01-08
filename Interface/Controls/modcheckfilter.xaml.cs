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
using InfiniteRuntimeTagViewer.Interface.Windows;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
	/// <summary>
	/// Interaction logic for modcheckfilter.xaml
	/// </summary>
	public partial class modcheckfilter : UserControl
	{
		public modcheckfilter()
		{
			InitializeComponent();
		}

		public ModWindow mwidow;

		public int debug_count = 1;
 
		private void filterbox_Click(object sender, RoutedEventArgs e)
		{
			mwidow.one_of_our_filtercheckboxes_was_clicked();
		}
	}
}
