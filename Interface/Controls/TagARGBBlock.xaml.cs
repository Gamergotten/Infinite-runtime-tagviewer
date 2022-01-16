using System;
using System.Windows;
using System.Windows.Media;
namespace InfiniteRuntimeTagViewer.Interface.Controls
{
    /// <summary>
    /// Interaction logic for valueBlock.xaml
    /// </summary>
    public partial class TagARGBBlock
    {

		public TagARGBBlock()
        {
            InitializeComponent();
        }


		private void Color_SelectionChanged2(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			string hex_color = argb_colorpicker.SelectedColor.ToString().Replace("#", string.Empty);

			color_hash.Text = "#" + hex_color;

			int a_dec = int.Parse(hex_color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			decimal a_norm_dec = Math.Round((decimal) a_dec / 255, 2);

			int r_dec = int.Parse(hex_color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			decimal r_norm_dec = Math.Round((decimal) r_dec / 255, 2);

			int g_dec = int.Parse(hex_color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			decimal g_norm_dec = Math.Round((decimal) g_dec / 255, 2);

			int b_dec = int.Parse(hex_color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
			decimal b_norm_dec = Math.Round((decimal) b_dec / 255, 2);

			a_value.Text = a_norm_dec.ToString("G29");
			r_value.Text = r_norm_dec.ToString("G29");
			g_value.Text = g_norm_dec.ToString("G29");
			b_value.Text = b_norm_dec.ToString("G29");
		}

	}
}