﻿using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace InfiniteRuntimeTagViewer.Halo.TagObjects
{
	// ###### for anyone interested, check out https://github.com/Lord-Zedd/H5Tags/tree/master/tags // thank you lord zedd
	// ###### its quite useful for mapping out descriptions and stuff
	public class TagLayouts
	{
		public class C
		{
			public string? T { get; set; } // T = type
			public Dictionary<long, C>? B { get; set; } = null; // B = blocks? i forgot what B stands for

			/// <summary>
			/// Length of the tagblock
			/// </summary>
			public long S { get; set; } // S = size // length of tagblock

			public string? N { get; set; } // N = name // our name for the block 


			/// <summary>
			/// Set during load, will be used when I add netcode 
			/// </summary>
			public long MemoryAddress { get; set; }

			/// <summary>
			/// The absolute offset from the base address of the tag
			/// eg C2 will resolve to assault_rifle_mp.weapon + C2 
			/// 
			/// This will be recursive so the actual value might be 
			///		assault_rifle_mp.weapon + C2 + (nested block) 12 + (nested block) 4
			///		
			/// This will allow us to sync up changes across the server and client without
			/// the need to re-resolve memory addresses.
			/// </summary>
			public string AbsoluteTagOffset { get; set; } // as a string we can append offsets rather than mathmatically adding them
		}

		public class FlagGroup : C
		{
			public FlagGroup()
			{
				T = "FlagGroup";
			}

			/// <summary>
			/// Amount of bytes for flags
			/// </summary>
			public int A { get; set; }

			/// <summary>
			/// The max bit, if 0 then defaults to A * 8
			/// </summary>
			public int MB { get; set; }

			public string N;
			/// <summary>
			/// String description of the flags
			/// </summary>
			public Dictionary<int, string> STR { get; set; } = new Dictionary<int, string>();
		}
		public class EnumGroup : C
		{
			public EnumGroup()
			{
				T = "EnumGroup";
			}

			/// <summary>
			/// Amount of bytes for enum
			/// </summary>
			public int A { get; set; }

			public string N;
			/// <summary>
			/// String description of the flags
			/// </summary>
			public Dictionary<int, string> STR { get; set; } = new Dictionary<int, string>();
		}

		public static Dictionary<long, C> Tags(string grouptype)
		{
			run_parse r = new run_parse();
			return r.parse_the_mfing_xmls(grouptype);
		}

		public class run_parse
		{

			public long evalutated_index_PREVENT_DICTIONARYERROR = 99999;
			public Dictionary<long, C?> parse_the_mfing_xmls(string file_to_find)
			{
				Dictionary<long, C?> poopdict = new();

				// we still need to evalute the string and find the value withoin our plugins folder

				if (file_to_find.Contains("*"))
				{
					file_to_find = file_to_find.Replace("*", "_");
				}

				string predicted_file = Directory.GetCurrentDirectory() + "\\Plugins\\"+ file_to_find+".xml";

				if (File.Exists(predicted_file))
				{
					XmlDocument xd = new XmlDocument();
					xd.Load(predicted_file);
					XmlNode xn = xd.SelectSingleNode("root");
					XmlNodeList xnl = xn.ChildNodes;
					long current_offset = 0;
					foreach (XmlNode xntwo in xnl)
					{
						current_offset += the_switch_statement(xntwo, current_offset, ref poopdict);
					}
				}



				return poopdict;
			}

			public long the_switch_statement(XmlNode xn, long offset, ref Dictionary<long, C?> pairs)
			{
				switch (xn.Name)
				{
					case "_0":
						pairs.Add(offset, new C { T = "String", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_1":
						pairs.Add(offset, new C { T = "String", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_2":
						pairs.Add(offset, new C { T = "mmr3Hash", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_3":// unmapped - This case isn't found in any tag file
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_4":
						pairs.Add(offset, new C { T = "Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_5":
						pairs.Add(offset, new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_6":
						pairs.Add(offset, new C { T = "4Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_7":
						pairs.Add(offset, new C { T = "Pointer", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_8":
						pairs.Add(offset, new C { T = "Float", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_9": 
						pairs.Add(offset, new C { T = "String", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
						// This is the special case mentioned in previous versions.
						// Example, needing to change the empty tag reference in effects to spawn AI.
						// I tested it in game and its functional.

					case "_A":
						Dictionary<int, string> childdictionary1 = new();
						for (int iu = 0; iu < xn.ChildNodes.Count; iu++)
						{
							childdictionary1.Add(iu, xn.ChildNodes[iu].Attributes.GetNamedItem("v").InnerText);
						}
						pairs.Add(offset, new EnumGroup { A = 1, N = xn.Attributes.GetNamedItem("v").InnerText, STR = childdictionary1 });

						return group_lengths_dict[xn.Name];
					case "_B":
						Dictionary<int, string> childdictionary2 = new();
						for (int iu = 0; iu < xn.ChildNodes.Count; iu++)
						{
							childdictionary2.Add(iu, xn.ChildNodes[iu].Attributes.GetNamedItem("v").InnerText);
						}
						pairs.Add(offset, new EnumGroup { A = 2, N = xn.Attributes.GetNamedItem("v").InnerText, STR = childdictionary2 });

						return group_lengths_dict[xn.Name];
					case "_C":
						Dictionary<int, string> childdictionary3 = new();
						for (int iu = 0; iu < xn.ChildNodes.Count; iu++)
						{
							childdictionary3.Add(iu, xn.ChildNodes[iu].Attributes.GetNamedItem("v").InnerText);
						}
						pairs.Add(offset, new EnumGroup { A = 4, N = xn.Attributes.GetNamedItem("v").InnerText, STR = childdictionary3 });

						return group_lengths_dict[xn.Name];
					case "_D":
						Dictionary<int, string> childdictionary4 = new();
						for (int iu = 0; iu < xn.ChildNodes.Count; iu++)
						{
							childdictionary4.Add(iu, xn.ChildNodes[iu].Attributes.GetNamedItem("v").InnerText);
						}
						pairs.Add(offset, new FlagGroup { A = 4, N = xn.Attributes.GetNamedItem("v").InnerText, STR = childdictionary4 });

						return group_lengths_dict[xn.Name];
					case "_E":
						Dictionary<int, string> childdictionary5 = new();
						for (int iu = 0; iu < xn.ChildNodes.Count; iu++)
						{
							childdictionary5.Add(iu, xn.ChildNodes[iu].Attributes.GetNamedItem("v").InnerText);
						}
						pairs.Add(offset, new FlagGroup { A = 2, N = xn.Attributes.GetNamedItem("v").InnerText, STR = childdictionary5 });

						return group_lengths_dict[xn.Name];
					case "_F":
						Dictionary<int, string> childdictionary6 = new();
						for (int iu = 0; iu < xn.ChildNodes.Count; iu++)
						{
							childdictionary6.Add(iu, xn.ChildNodes[iu].Attributes.GetNamedItem("v").InnerText);
						}
						pairs.Add(offset, new FlagGroup { A = 1, N = xn.Attributes.GetNamedItem("v").InnerText, STR = childdictionary6 });

						return group_lengths_dict[xn.Name];

					case "_10": // im not 100% on this one
						pairs.Add(offset, new C { T = "2DPoint_2Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_11":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_12":
						pairs.Add(offset, new C { T = "mmr3Hash", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_13":// unmapped - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_14":
						pairs.Add(offset, new C { T = "Float", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_15":
						pairs.Add(offset, new C { T = "Float", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_16":
						pairs.Add(offset, new C { T = "2DPoint_Float", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_17":
						pairs.Add(offset, new C { T = "3DPoint", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_18":
						pairs.Add(offset, new C { T = "2DPoint_Float", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_19":
						pairs.Add(offset, new C { T = "3DPoint", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_1A":
						pairs.Add(offset, new C { T = "Quanternion", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_1B":
						pairs.Add(offset, new C { T = "2DPoint_Float", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_1C":
						pairs.Add(offset, new C { T = "3DPoint", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_1D":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_1E": // pretty sure this is currect, could be wrong though. I referenced calculus equations
						pairs.Add(offset, new C { T = "3DPlane", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_1F":
						pairs.Add(offset, new C { T = "RGB", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_20":
						pairs.Add(offset, new C { T = "ARGB", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_21":// unmapped - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_22":// unmapped  - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_23":
						pairs.Add(offset, new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText + ".min" });
						pairs.Add((offset + 2), new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText + ".max" });
						return group_lengths_dict[xn.Name];
					case "_24":
						pairs.Add(offset, new C { T = "BoundsFloat", N = xn.Attributes.GetNamedItem("v").InnerText});
						return group_lengths_dict[xn.Name];
					case "_25":
						pairs.Add(offset, new C { T = "BoundsFloat", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_26":
						pairs.Add(offset, new C { T = "BoundsFloat", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_27":// unmapped - This case isn't found in any tag file
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_28":// unmapped - This case isn't found in any tag file
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_29":// unmapped  - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_2A":// unmapped - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_2B":// unmapped - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_2C":
						pairs.Add(offset, new C { T = "Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_2D":// unmapped - only found in ttag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_2E":
						pairs.Add(offset, new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_2F":
						pairs.Add(offset, new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_30":
						pairs.Add(offset, new C { T = "4Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_31":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_32":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_33":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_34": // field pad
						int length = int.Parse(xn.Attributes.GetNamedItem("length").InnerText);
						if (length == 1)
						{
							pairs.Add(offset, new C { T = "Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						}
						else if (length == 2)
						{
							pairs.Add(offset, new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						}
						else if (length == 4)
						{
							pairs.Add(offset, new C { T = "4Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						}
						else
						{
							pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText });
						}
						return length;
					case "_35":
						pairs.Add(offset, new C { T = "4Byte", N = xn.Attributes.GetNamedItem("v").InnerText + " Index" }); // Definitely could be wrong, just guessing here.
						pairs.Add(offset + 4, new C { T = "mmr3Hash", N = xn.Attributes.GetNamedItem("v").InnerText + " Name" });
						return int.Parse(xn.Attributes.GetNamedItem("length").InnerText);
					case "_36":
						if (xn.Attributes.GetNamedItem("v").InnerText != "")
						{
							pairs.Add(offset + evalutated_index_PREVENT_DICTIONARYERROR, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText });
							evalutated_index_PREVENT_DICTIONARYERROR++;
						}
						else
						{

						}
						return 0;
					case "_37":
						if (xn.Attributes.GetNamedItem("v").InnerText != "")
						{
							pairs.Add(offset + evalutated_index_PREVENT_DICTIONARYERROR, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText });
							evalutated_index_PREVENT_DICTIONARYERROR++;
						}
						else
						{
							// what
						}
						return 0;
					case "_38": //struct
						// --THIS WAS A TEST FOR ADDING STRUCT UI--
						//Dictionary<long, C> subthings1 = new Dictionary<long, C>();
						//XmlNodeList xnl1 = xn.ChildNodes;
						//int childnodescount = xnl1.Count;
						//long current_offset1 = offset;
						//foreach (XmlNode xntwo2 in xnl1)
						//{
						//	current_offset1 += the_switch_statement(xntwo2, current_offset1, ref subthings1);
						//}
						//pairs.Add(offset, new C { T = "TagStructBlock", N = xn.Attributes.GetNamedItem("v").InnerText + " Nodes = " + childnodescount, B = subthings1 });

						pairs.Add(offset + evalutated_index_PREVENT_DICTIONARYERROR, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText });
						evalutated_index_PREVENT_DICTIONARYERROR++;
						XmlNodeList xnl1 = xn.ChildNodes;
						long current_offset1 = offset;
						foreach (XmlNode xntwo2 in xnl1)
						{
							current_offset1 += the_switch_statement(xntwo2, current_offset1, ref pairs);
						}
						return current_offset1 - offset;


					case "_39":
						pairs.Add(offset + evalutated_index_PREVENT_DICTIONARYERROR, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText });
						evalutated_index_PREVENT_DICTIONARYERROR++;
						XmlNodeList xnl3 = xn.ChildNodes;
						long current_offset3 = offset;
						foreach (XmlNode xntwo2 in xnl3)
						{
							current_offset3 += the_switch_statement(xntwo2, current_offset3, ref pairs);
						}
						return current_offset3 - offset;
					case "_3A":// unmapped - Not found in any tag
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_3B":
						return group_lengths_dict[xn.Name];
					case "_3C":
						pairs.Add(offset, new C { T = "Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_3D":
						pairs.Add(offset, new C { T = "2Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_3E":
						pairs.Add(offset, new C { T = "4Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_3F":
						pairs.Add(offset, new C { T = "Pointer", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_40":
						if (xn.ChildNodes.Count > 0)
						{
							Dictionary<long, C> subthings = new Dictionary<long, C>();
							XmlNodeList xnl2 = xn.ChildNodes;
							long current_offset2 = 0;
							foreach (XmlNode xntwo2 in xnl2)
							{
								current_offset2 += the_switch_statement(xntwo2, current_offset2, ref subthings); // its gonna append that to the main, rather than our struct
							}

							pairs.Add(offset, new C { T = "Tagblock", N = xn.Attributes.GetNamedItem("v").InnerText, B = subthings, S = current_offset2 });

						}
						else
						{
							pairs.Add(offset, new C { T = "Tagblock", N = xn.Attributes.GetNamedItem("v").InnerText });
						}
						return group_lengths_dict[xn.Name];
					case "_41":
						pairs.Add(offset, new C { T = "TagRef", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_42":
						pairs.Add(offset, new C { T = "FUNCTION", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_43":// Mapping these to fix errors. The new length seems to fix some issues. Check pfnd > mobileNavMeshes to understand.
						pairs.Add(offset, new C { T = "Pointer", N = xn.Attributes.GetNamedItem("v").InnerText });
						pairs.Add(offset + 8, new C { T = "mmr3Hash", N = xn.Attributes.GetNamedItem("v").InnerText});
						pairs.Add(offset + 12, new C { T = "4Byte", N = xn.Attributes.GetNamedItem("v").InnerText });
						return group_lengths_dict[xn.Name];
					case "_44":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_45":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), may cause errors)" });
						return group_lengths_dict[xn.Name];
					case "_69":// unmapped
						pairs.Add(offset, new C { T = "Comment", N = xn.Attributes.GetNamedItem("v").InnerText + " (unmapped type(" + xn.Name + "), fuck this one value specifically)" });
						return group_lengths_dict[xn.Name];

				}
				return group_lengths_dict[xn.Name];
			}


			public static Dictionary<string, long> group_lengths_dict = new()
			{
				{ "_0", 32 }, // _field_string
				{ "_1", 256 }, // _field_long_string
				{ "_2", 4 }, // _field_string_id
				{ "_3", 4 }, // ## Not found in any tag type
				{ "_4", 1 }, // _field_char_integer
				{ "_5", 2 }, // _field_short_integer
				{ "_6", 4 }, // _field_long_integer
				{ "_7", 8 }, // _field_int64_integer
				{ "_8", 4 }, // _field_angle
				{ "_9", 4 }, // _field_tag
				{ "_A", 1 }, // _field_char_enum
				{ "_B", 2 }, // _field_short_enum
				{ "_C", 4 }, // _field_long_enum
				{ "_D", 4 }, // _field_long_flags
				{ "_E", 2 }, // _field_word_flags
				{ "_F", 1 }, // _field_byte_flags
				{ "_10", 4 }, // _field_point_2d -- 2 2bytes?
				{ "_11", 4 }, // _field_rectangle_2d
				{ "_12", 4 }, // _field_rgb_color -- hex color codes - it's technically only 3 bytes but the final byte is FF
				{ "_13", 4 }, // _field_argb_color 
				{ "_14", 4 }, // _field_real
				{ "_15", 4 }, // _field_real_fraction
				{ "_16", 8 }, // _field_real_point_2d
				{ "_17", 12 }, // _field_real_point_3d
				{ "_18", 8 }, // _field_real_vector_2d -- 
				{ "_19", 12 }, // _field_real_vector_3d
				{ "_1A", 16 }, // _field_real_quaternion
				{ "_1B", 8 }, // _field_real_euler_angles_2d
				{ "_1C", 12 }, // _field_real_euler_angles_3d
				{ "_1D", 12 }, // _field_real_plane_2d
				{ "_1E", 16 }, // _field_real_plane_3d
				{ "_1F", 12 }, // _field_real_rgb_color
				{ "_20", 16 }, // _field_real_argb_color
				{ "_21", 4 }, // _field_real_hsv_colo
				{ "_22", 4 }, // _field_real_ahsv_color
				{ "_23", 4 }, // _field_short_bounds
				{ "_24", 8 }, // _field_angle_bounds
				{ "_25", 8 }, // _field_real_bounds
				{ "_26", 8 }, // _field_real_fraction_bounds
				{ "_27", 4 }, // ## Not found in any tag type
				{ "_28", 4 }, // ## Not found in any tag type
				{ "_29", 4 }, // _field_long_block_flags
				{ "_2A", 4 }, // _field_word_block_flags
				{ "_2B", 4 }, // _field_byte_block_flags
				{ "_2C", 1 }, // _field_char_block_index
				{ "_2D", 1 }, // _field_custom_char_block_index
				{ "_2E", 2 }, // _field_short_block_index
				{ "_2F", 2 }, // _field_custom_short_block_index
				{ "_30", 4 }, // _field_long_block_index
				{ "_31", 4 }, // _field_custom_long_block_index
				{ "_32", 4 }, // ## Not found in any tag type
				{ "_33", 4 }, // ## Not found in any tag type
				{ "_34", 4 }, // _field_pad ## variable length
				{ "_35", 4 }, // 'field_skip' ## iirc
				{ "_36", 0 }, // _field_explanation
				{ "_37", 0 }, // _field_custom
				{ "_38", 0 }, // _field_struct
				{ "_39", 32 }, // _field_array
				{ "_3A", 4 },
				{ "_3B", 0 }, // ## end of struct or something
				{ "_3C", 1 }, // _field_byte_integer
				{ "_3D", 2 }, // _field_word_integer
				{ "_3E", 4 }, // _field_dword_integer
				{ "_3F", 8 }, // _field_qword_integer
				{ "_40", 20 }, // _field_block_v2
				{ "_41", 28 }, // _field_reference_v2
				{ "_42", 24 }, // _field_data_v2

				{ "_43", 16 }, // tag_resource, length could be wrong, probably not though.

				{ "_44", 4 },
				{ "_45", 4 },
			};


		}
	}
}
