using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRuntimeTagViewer.Halo.TagStructDump
{
	public class StructureLayouts
	{
		public struct Group_definitions_link_struct // 40 bytes
		{
			public string name1;
			public string name2;

			public int int1;
			public int int2;

			public Table2_struct Table2_struct; // Table2_struct
			public long Table2_struct_pointer2; // Table2_struct
		}
		public struct Table2_struct // 200 bytes
		{
			public string Name1;
			public string Name2;

			public string hash1;
			public string hash2;
			public string hash3;
			public string hash4;

			public List<Table1_struct> tag_struct_lookup1;

			public int int1; // 384
			public int int2; // 0

			public long exe_pointer1;

			public int int3; // 17
			public int int4; // 0
			public int int5; // 384
			public int int6; // 0

			public string hash5;
			public string hash6;

			public int int7; // 1
			public int int8; // 0

			public long tag_struct_lookup2;

			public string Name3;

			public int int9; // 384
			public int int10; // 0

			public long unknown_pointer1;
			public string unknown_string4;

			public int int11; // 1
			public int int12; // 0
			public int int13; //12387

			public long tag_struct_lookup3;

			public int STRUCTCOUNT; // num of child elements
			public int int14; // 1
			public int int15; // 2664
			public int int16; // 3159044
			public int int17; // 164626464
			public int int18; // 6
			public int int19; // 3159044
			public int int20; // 164626464
			public int int21; // 6
			public int int22; // 0
			public int int23; // 0
			public int int24; // 0

			public long exe_pointer2; // doesn't seem to ever point anywhere
		}



		public struct Table1_struct // 24 bytes
		{
			public string name;
			public int struct_type_index;
			public int int2;
			public possible_t1_struct_c_instance? dodgy_struct; // alternates based on "struct_type_index"
																// primarily a pointer, can also be an int
		}


		public static Dictionary<string, long> group_lengths_dict = new()
		{
			{ "_0", 32 }, // _field_string
            { "_1", 256 }, // _field_long_string
            { "_2", 4 }, // _field_string_id
            { "_3", 4 },
			{ "_4", 1 },
			{ "_5", 2 }, // _field_short_integer
            { "_6", 4 }, // _field_long_integer
            { "_7", 8 }, // _field_int64_integer
            { "_8", 4 }, // _field_angle
            { "_9", 4 },
			{ "_A", 1 }, // _field_char_enum
            { "_B", 2 }, // _field_short_enum
            { "_C", 4 }, // _field_long_enum
            { "_D", 4 }, // _field_long_flags
            { "_E", 2 }, // _field_word_flags
            { "_F", 1 }, // _field_byte_flags
            { "_10", 4 },
			{ "_11", 4 },
			{ "_12", 4 },
			{ "_13", 4 },
			{ "_14", 4 }, // _field_real
            { "_15", 4 }, // _field_real_fraction
            { "_16", 4 },
			{ "_17", 12 }, // _field_real_point_3d
            { "_18", 12 },
			{ "_19", 12 }, // _field_real_vector_3d
            { "_1A", 12 }, // quarternion 4
            { "_1B", 12 },
			{ "_1C", 12 }, // _field_real_euler_angles_3d
            { "_1D", 12 },
			{ "_1E", 12 },
			{ "_1F", 12 }, // _field_real_rgb_color
            { "_20", 4 },
			{ "_21", 4 },
			{ "_22", 4 },
			{ "_23", 4 },
			{ "_24", 8 }, // _field_angle_bounds
            { "_25", 8 }, // _field_real_bounds
            { "_26", 4 },
			{ "_27", 4 },
			{ "_28", 4 },
			{ "_29", 4 },
			{ "_2A", 4 },
			{ "_2B", 4 },
			{ "_2C", 1 }, // _field_char_block_index
            { "_2D", 1 },
			{ "_2E", 2 }, // _field_short_block_index
            { "_2F", 2 },
			{ "_30", 4 }, // _field_long_block_index
            { "_31", 4 },
			{ "_32", 4 },
			{ "_33", 4 },
			{ "_34", 4 }, // _field_pad
            { "_35", 4 },
			{ "_36", 0 }, // _field_explanation
            { "_37", 0 }, // _field_custom
            { "_38", 0 }, // _field_struct
            { "_39", 32 }, // something verticies
            { "_3A", 4 },
			{ "_3B", 0 }, // end of struct or something
            { "_3C", 1 }, // _field_byte_integer
            { "_3D", 2 },
			{ "_3E", 4 }, // _field_dword_integer
            { "_3F", 8 },
			{ "_40", 20 }, // _field_block_v2
            { "_41", 28 }, // _field_reference_v2
            { "_42", 24 }, // _field_data_v2
            { "_43", 4 },
			{ "_44", 4 },
			{ "_45", 4 },
		};

		// 00 // _field_string
		// 01 // _field_long_string
		// 02 // _field_string_id
		// 03
		// 04
		// 05 // _field_short_integer
		// 06 // _field_long_integer
		// 07 // _field_int64_integer
		// 08 // _field_angle
		// 09
		// 0A // _field_char_enum
		// 0B // _field_short_enum
		// 0C // _field_long_enum
		// 0D // _field_long_flags
		// 0E // _field_word_flags
		// 0F // _field_byte_flags
		// 10
		// 11
		// 12
		// 13
		// 14 // _field_real
		// 15 // _field_real_fraction
		// 16
		// 17 // _field_real_point_3d
		// 18 
		// 19 // _field_real_vector_3d
		// 1A
		// 1B // 
		// 1C // _field_real_euler_angles_3d
		// 1D
		// 1E
		// 1F // _field_real_rgb_color
		// 20
		// 21
		// 22
		// 23
		// 24 // _field_angle_bounds
		// 25 // _field_real_bounds
		// 26
		// 27
		// 28
		// 29
		// 2A
		// 2B
		// 2C // _field_char_block_index
		// 2D
		// 2E // _field_short_block_index
		// 2F
		// 30 // _field_long_block_index
		// 31
		// 32
		// 33
		// 34 // _field_pad -- LENGTH IS REQUIRED
		// 35
		// 36 // _field_explanation
		// 37 // _field_custom
		// 38 // _field_struct
		// 39
		// 3A
		// 3B -- END STRUCT
		// 3C // _field_byte_integer
		// 3D
		// 3E // _field_dword_integer
		// 3F
		// 40 // _field_block_v2
		// 41 // _field_reference_v2
		// 42 // _field_data_v2
		// 43




		public struct possible_t1_struct_c_instance
		{
			public long actual_value;
			public _02 _02_;
			public _0B_through_0F _0B_through_0F_; // flags and enums
			public _29 _29_;
			public _2A _2A_;
			public _2B _2B_;
			public _2C _2C_;
			public _2D _2D_;
			public _2E _2E_;
			public _2F _2F_;
			public _30 _30_;
			public _31 _31_;
			public _34 _34_; // generated pad
			public _35 _35_; // another pad?
			public _36 _36_;
			public _37 _37_;
			public _38 _38_;
			public _39 _39_;
			public _40 _40_;
			public _41 _41_;
			public _42 _42_;
			public _43 _43_;
		} // bruh howtf do you store these as a single variable

		public struct _02 // unknown
		{
			public long exe_pointer; // mostly invalid
		}
		public struct _0B_through_0F // flags and enums
		{
			public string name;
			public long count;
			public List<string> children;
		}
		public struct _29
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2A
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2B
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2C
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2D
		{
			// nothing
		}
		public struct _2E
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _2F
		{
			// pointer to somewhere
		}
		public struct _30
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _31
		{
			// pointer to who knows wheree
		}
		public struct _34
		{
			//public long generated_pad_length;
			// so thers actually nothing in this struct
		}
		public struct _35
		{
			//public long generated_pad_length;
			// so thers actually nothing in this struct
		}
		public struct _36
		{
			// potentially nothing, this points to render stuff
		}
		public struct _37
		{
			// nothing notable aiside from the 4 byte after count
		}
		public struct _38
		{
			public Table2_struct table2_ref;
		}
		public struct _39
		{
			public string Name1;
			public int int1;
			public int int2;
			public long long1;
			public Table2_struct table2_ref;
		}
		public struct _40
		{
			public Group_definitions_link_struct tag_struct_pointer;
		}
		public struct _41
		{
			public int int1;
			public string taggroup1;
			// pointer to
			public string taggroup2;
			public string taggroup3;
			public string taggroup4;
			public string taggroup5;
		}
		public struct _42 // length 72 bytes
		{
			public string Name1;
			public int int1;
			public int int2;
			public int int3;
			public int int4;

			public long long1;
			public long long2;
			public long long3;
			public long long4;
			public long long5;
			public long long6;
		}
		public struct _43 // length 72 bytes
		{
			public string Name1;
			public long long1;
			public Table2_struct table2_ref;
			public long long2;

		}
	}

}
