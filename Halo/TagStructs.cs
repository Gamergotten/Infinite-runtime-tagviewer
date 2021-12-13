using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Assembly69.Halo.TagObjects;

namespace Assembly69.Halo
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



	public class TagEditorDefinition
	{
		public long MemoryAddress;
		public string MemoryType;
		public long? OffsetOverride = null;

		public Vehi.C TagDef;
		public TagStruct TagStruct;

		public string DatNum;
		public string TagId;

		public long GetTagOffset()
		{
			if (OffsetOverride != null)
			{
				return (long) OffsetOverride.Value;
			}

			return (long) TagDef.AbsoluteTagOffset;
		}

		public TagEditorDefinition() { }

		public TagEditorDefinition(TagEditorDefinition ted)
		{
			this.MemoryAddress = ted.MemoryAddress;
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

		public TED_TagRefGroup() : base() { }
		public TED_TagRefGroup(TED_TagRefGroup ted) : base(ted) 
		{
			this.TagGroup = ted.TagGroup;
		}
	}
}
