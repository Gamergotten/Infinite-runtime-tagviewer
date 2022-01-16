
using System;
using System.Windows;
using System.Windows.Media;
namespace InfiniteRuntimeTagViewer.Interface.Controls
{
    /// <summary>
    /// Interaction logic for valueBlock.xaml
    /// </summary>
    public partial class TagRGBBlock
    {

		public TagRGBBlock()
        {
            InitializeComponent();
        }


		private void Color_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			string hex_color = rgb_colorpicker.SelectedColor.ToString().Replace("#FF", string.Empty);
			
			int r_dec = int.Parse(hex_color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			decimal r_norm_dec = Math.Round((decimal)r_dec / 255, 2);
			
			int g_dec = int.Parse(hex_color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			decimal g_norm_dec = Math.Round((decimal) g_dec / 255, 2);

			int b_dec = int.Parse(hex_color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			decimal b_norm_dec = Math.Round((decimal) b_dec / 255, 2);

			r_value.Text = r_norm_dec.ToString("G29");
			g_value.Text = g_norm_dec.ToString("G29");
			b_value.Text = b_norm_dec.ToString("G29");
		}

	}
}