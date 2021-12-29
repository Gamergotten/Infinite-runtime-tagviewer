using System;
using System.Windows.Controls;

using InfiniteRuntimeTagViewer.Halo.TagObjects;

namespace InfiniteRuntimeTagViewer.Halo
{
	// Changed to class because we are storing references to this now, saves on memory usage.
	public class TagStruct
	{
		public string Datnum;

		public string ObjectId;

		public string TagGroup;

		public long TagData;

		public string TagTypeDesc;

		public string TagFullName;

		public string TagFile;
	}

	public class GroupTagStruct
	{
		public string TagGroupDesc;

		public string TagGroupName;

		public string TagGroupDefinitition;

		public string TagExtraType;

		public string TagExtraName;

		public TreeViewItem TagCategory;
	}


	[Flags]
	public enum TagEditorDefType : UInt32
	{
		TagEditorDefinition = 0,
		TED_TagRefGroup = 1
	}

	public class TagEditorDefinition
	{
		public TagEditorDefType TEDType = TagEditorDefType.TagEditorDefinition;
		public string MemoryType;
		public string? OffsetOverride = null;

		public TagLayouts.C TagDef;
		public TagStruct TagStruct;

		public string DatNum;
		public string TagId;

		public string GetTagOffset()
		{
			if (OffsetOverride != null)
			{
				return OffsetOverride;
			}

			return TagDef.AbsoluteTagOffset;
		}

		public TagEditorDefinition() { }

		public TagEditorDefinition(TagEditorDefinition ted)
		{
			this.MemoryType = ted.MemoryType;
			this.OffsetOverride = ted.OffsetOverride;
			this.TagDef = ted.TagDef;
			this.TagStruct = ted.TagStruct;
			this.DatNum = ted.DatNum;
			this.TagId = ted.TagId;
		}

	}

	public class TED_TagRefGroup : TagEditorDefinition {
		public string TagGroup;

		public TED_TagRefGroup() : base() {
			TEDType = TagEditorDefType.TED_TagRefGroup;
		}

		public TED_TagRefGroup(TED_TagRefGroup ted) : base(ted) 
		{
			TEDType = TagEditorDefType.TED_TagRefGroup;
			this.TagGroup = ted.TagGroup;
		}
	}
}
