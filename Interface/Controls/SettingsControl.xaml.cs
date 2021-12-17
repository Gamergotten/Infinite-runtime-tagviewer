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
using System.Windows.Shapes;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class SettingsControl : Window
	{
		public SettingsControl()
		{
			InitializeComponent();
		}

		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		// Maximize
		private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MaximizeWindow(this);
		}

		// Restore
		private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.RestoreWindow(this);
		}

		// Close
		private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}
	}
}
