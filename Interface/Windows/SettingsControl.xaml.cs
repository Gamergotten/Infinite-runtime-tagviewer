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
			StateChanged += MainWindowStateChangeRaised;
		}

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

		private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
		{
			//Ensure SettingsTree.SelectedItem has a value.
			if (SettingsTree.SelectedItem != null)
			{
				//if selected item in tree is "General" under the window tree.
				if (SettingsTree.SelectedItem == WindowGeneral)
				{

				}
				else if (SettingsTree.SelectedItem == WindowColors)
				{

				}
				else if (SettingsTree.SelectedItem == WindowTheme)
				{

				}
				else if (SettingsTree.SelectedItem == TextGeneral)
				{
					TextGeneralLayout.Visibility = Visibility.Visible;
				}
			}

		}
	}
}
