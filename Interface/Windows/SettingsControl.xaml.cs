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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{


	public partial class SettingsControl : Window
	{
		public SettingsControl()
		{
			InitializeComponent();
			StateChanged += MainWindowStateChangeRaised;
		}

		#region TitleBar Commands
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
		#endregion


		private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
		{
			//Ensure SettingsTree.SelectedItem has a value.
			if (SettingsTree.SelectedItem != null)
			{
				TreeViewItem? item = SettingsTree.SelectedItem as TreeViewItem;
				if (item != null)
				{
					ItemsControl parent = GetSelectedTreeViewItemParent(item);
					TreeViewItem? parentText = parent as TreeViewItem;
					if (parentText != null)
					{
						SettingsTitleText.Text = "Settings > " + parentText.Header.ToString() + " > " + item.Header.ToString();
					}
					//SettingsTitle.Visibility = Visibility.Visible;
				}
				#region Text Settings
				if (SettingsTree.SelectedItem == TextGeneral)
				{
					TextGeneralLayout.Visibility = Visibility.Visible;
				}
				else
				{
					TextGeneralLayout.Visibility = Visibility.Hidden;
				}
				#endregion

				#region Window Settings
				if (SettingsTree.SelectedItem == WindowGeneral)
				{
					WindowGeneralLayout.Visibility = Visibility.Visible;
				}
				else
				{
					WindowGeneralLayout.Visibility = Visibility.Hidden;
				}
				#endregion
			}

		}

		public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(item);
			while (!(parent is TreeViewItem || parent is TreeView))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}

			return parent as ItemsControl;
		}
	}
}
