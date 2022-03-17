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
using InfiniteRuntimeTagViewer;
using InfiniteRuntimeTagViewer.Properties;

namespace InfiniteRuntimeTagViewer.Interface.Windows
{
	/// <summary>
	/// Interaction logic for PointerDialog.xaml
	/// </summary>
	public partial class PointerDialog : Window
	{
		public PointerDialog()
		{
			InitializeComponent();
		}

		private readonly MainWindow mw = new();
		public void okButton_Click(object sender, RoutedEventArgs e)
		{
			if (pointer != null)
			{
				Settings.Default.ProcAsyncBaseAddr = pointer.Text;
				Settings.Default.Save();
				mw.UpdateAddress();
				
			}
			Close();
		}

		public void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		#region Window Styling

		// Can execute
		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		// Minimize
		private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
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

		// State change
		private void MainWindowStateChangeRaised(object? sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				MainWindowBorder.BorderThickness = new Thickness(8);
				RestoreButton.Visibility = Visibility.Visible;
				MaximizeButton.Visibility = Visibility.Collapsed;
			}
			else
			{
				MainWindowBorder.BorderThickness = new Thickness(0);
				RestoreButton.Visibility = Visibility.Collapsed;
				MaximizeButton.Visibility = Visibility.Visible;
			}
		}
		#endregion
	}
}
