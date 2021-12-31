using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;
using System.Collections.Specialized;
using InfiniteRuntimeTagViewer;
using InfiniteRuntimeTagViewer.Properties;

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
				}
				if (item == GeneralGen) 
				{
					GeneralGenLayout.Visibility = Visibility.Visible;
					UpdateComboBoxIndex();
				}
				else
				{
					GeneralGenLayout.Visibility = Visibility.Collapsed;
				}
				#region Text Settings
				//if (SettingsTree.SelectedItem == TextGeneral)
				//{
				//	TextGeneralLayout.Visibility = Visibility.Visible;
				//}
				//else
				//{
				//	TextGeneralLayout.Visibility = Visibility.Collapsed;
				//}
				#endregion

				#region Window Settings
				//if (SettingsTree.SelectedItem == WindowGeneral)
				//{
				//	WindowGeneralLayout.Visibility = Visibility.Visible;
				//}
				//else
				//{
				//	WindowGeneralLayout.Visibility = Visibility.Collapsed;
				//}
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

		public bool AutoHookKey;
		public bool AutoLoadKey;
		public bool AutoPokeKey;
		public bool AutoSaveKey;
		public bool FilterOnlyMappedKey;
		
		public void GetGeneralSettingsFromConfig()
		{
			AutoHookKey = Settings.Default.AutoHook;
			AutoLoadKey = Settings.Default.AutoLoad;
			AutoPokeKey = Settings.Default.AutoPoke;
			//AutoSaveKey = InfiniteRuntimeTagViewer.Properties.Settings.Default.AutoSave;
			FilterOnlyMappedKey = Settings.Default.FilterOnlyMapped;
		}


		public void UpdateComboBoxIndex()
		{
			GetGeneralSettingsFromConfig();
			if (AutoHookKey)
			{
				AutoHookComboBox.SelectedIndex = 1;
			}
			else
			{
				AutoHookComboBox.SelectedIndex = 0;
			}
			if (AutoLoadKey)
			{
				AutoLoadComboBox.SelectedIndex = 1;
			}
			else
			{
				AutoLoadComboBox.SelectedIndex = 0;
			}
			if (AutoPokeKey)
			{
				AutoPokeComboBox.SelectedIndex = 1;
			}
            else
            {
                AutoPokeComboBox.SelectedIndex = 0;
            }
			if (FilterOnlyMappedKey)
			{
				FilterOnlyMappedComboBox.SelectedIndex = 1;
			}
            else
            {
                FilterOnlyMappedComboBox.SelectedIndex = 0;
            }
		}

		public void SetGeneralSettingsFromConfig()
		{
			GetGeneralSettingsFromConfig();
			Window window = Application.Current.MainWindow;
			if (AutoHookKey)
			{
				(window as MainWindow).CbxSearchProcess.IsChecked = true;
			}
			else
			{
				(window as MainWindow).CbxSearchProcess.IsChecked = false;
			}

			if (AutoPokeKey)
			{
				(window as MainWindow).CbxAutoPokeChanges.IsChecked = true;
			}
			else
			{
				(window as MainWindow).CbxAutoPokeChanges.IsChecked = false;
			}

			if (AutoLoadKey)
			{
				(window as MainWindow).CbxAutoLoadTags.IsChecked = true;
			}
			else
			{
				(window as MainWindow).CbxAutoLoadTags.IsChecked = false;
			}
			if (FilterOnlyMappedKey)
			{
				(window as MainWindow).cbxFilterOnlyMapped.IsChecked = true;
			}
			else
			{
				(window as MainWindow).cbxFilterOnlyMapped.IsChecked = false;
			}
		}
		public void OnApplyChanges_Click(object sender, RoutedEventArgs e)
		{
			SaveUserChangedSettings();
			Settings.Default.Save();
			SetGeneralSettingsFromConfig();
		}

		public void SaveUserChangedSettings()
		{
			if (AutoHookComboBox.SelectedIndex == 1)
			{
				Settings.Default.AutoHook = true;
				System.Diagnostics.Debug.WriteLine(Settings.Default.AutoHook);
			}
			else
			{
				Settings.Default.AutoHook = false;
			}

			if (AutoLoadComboBox.SelectedIndex == 1)
			{
				Settings.Default.AutoLoad = true;
			}
			else
			{
				Settings.Default.AutoLoad = false;
			}

			if (AutoPokeComboBox.SelectedIndex == 1)
			{
				Settings.Default.AutoPoke = true;
			}
			else
			{
				Settings.Default.AutoPoke = false;
			}

			if (FilterOnlyMappedComboBox.SelectedIndex == 1)
			{
				Settings.Default.FilterOnlyMapped = true;
			}
			else
			{
				Settings.Default.FilterOnlyMapped = false;
			}
		}

	}
}
