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
using InfiniteRuntimeTagViewer.Halo.TagObjects;

namespace InfiniteRuntimeTagViewer.Interface.Controls
{
	/// <summary>
	/// Interaction logic for FunctionBlock.xaml
	/// </summary>
	public partial class FunctionBlock : UserControl
	{

		public TagEditorControl EditorControl { get; }

		public FunctionBlock(TagEditorControl editorControl, TagStruct tagStruct)
		{
			InitializeComponent();
			EditorControl = editorControl;
			TagStruct = tagStruct;
		}

		public KeyValuePair<long, TagLayouts.C> Children { get; set; }
		public long BlockAddress { get; set; }
		public TagStruct TagStruct { get; set; }
	}
}
