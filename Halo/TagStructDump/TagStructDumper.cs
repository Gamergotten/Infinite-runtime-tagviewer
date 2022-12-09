using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using InfiniteRuntimeTagViewer.Halo.TagStructDump;
using static InfiniteRuntimeTagViewer.Halo.TagStructDump.StructureLayouts;
using Memory;
using System.Diagnostics;

namespace InfiniteRuntimeTagViewer.Halo.TagStructDump
{
	public class TagStructDumper
	{
		public TagStructDumper(long address, int count, Mem mem)
		{
			startAddress = address;
			tagCount = count;
			M = mem;
		}

		private XmlWriterSettings xmlWriterSettings = new()
		{
			Indent = true,
			IndentChars = "\t",
		};
		private XmlWriter? textWriter;
		private Mem M = new();

		private long startAddress = 0;
		private int tagCount = 0;
		private string outDIR = @".\Plugins";	

		public void DumpStructs()
		{
			try
			{
				ClearPlugins();

				for (int iteration_index = 0; iteration_index < tagCount; iteration_index++)
				{
					string temp_filename = outDIR + @"\dump" + iteration_index + ".xml";
					using (XmlWriter w = XmlWriter.Create(temp_filename, xmlWriterSettings))
					{
						textWriter = w;
						textWriter.WriteStartDocument();
						textWriter.WriteStartElement("root");

						long offset_from_start = iteration_index * 88;
						long current_tag_struct_Address = startAddress + offset_from_start;
						long gdshgfjasdf = (current_tag_struct_Address);
						string group_name_thingo = M.ReadString((current_tag_struct_Address + 12).ToString("X"), "", 4);
						GetGDLS(M.ReadLong((current_tag_struct_Address + 32).ToString("X")));

						textWriter.WriteEndElement();
						textWriter.WriteEndDocument();
						textWriter.Close();

						System.IO.FileInfo fi = new System.IO.FileInfo(temp_filename);
						if (fi.Exists)
						{
							string s33 = ReverseString(group_name_thingo);
							if (!s33.Contains("*"))
							{
								if (s33 != "cmpS")
								{
									if (File.Exists(outDIR + @"\" + s33 + ".xml"))
									{
										fi.MoveTo(outDIR + @"\" + s33 + "1.xml");
									}
									else
									{
										fi.MoveTo(outDIR + @"\" + s33 + ".xml");
									}
								}
							}
							else
							{
								string s331 = s33.Replace("*", "_");
								fi.MoveTo(outDIR + @"\" + s331 + ".xml");
								Debug.WriteLine("[DEBUG] " + s33 + " replaced with " + s331);
							}
						}
					}
				}
			}
			catch
			{
				Debug.WriteLine("[DEBUG] Failed to dump tag structs!");
			}
		}

		private string ReverseString(string myStr)
		{
			char[] myArr = myStr.ToCharArray();
			Array.Reverse(myArr);
			return new string(myArr);
		}

		private void ClearPlugins()
		{
			foreach (string file in Directory.EnumerateFiles(outDIR))
			{
				File.Delete(file);
			}
		}

		private Table2_struct ReadChunk(long address)
		{

			int amount_of_things_to_read = M.ReadInt((address + 120).ToString("X"));

			long address_for_our_string_bruh = M.ReadLong(address.ToString("X"));
			string take_this_mf_and_pass_it_down_for_gods_sake = M.ReadString(address_for_our_string_bruh.ToString("X"), "", 300);

			for (int index = 0; index < amount_of_things_to_read; index++)
			{
				long address_next_next = M.ReadLong((address + 32).ToString("X")) + (index * 24);

				int group = M.ReadInt((address_next_next + 8).ToString("X"));
				string n_name = M.ReadString(M.ReadLong(address_next_next.ToString("X")).ToString("X"), "", 300);

				long next_next_next_address = M.ReadLong((address_next_next + 16).ToString("X"));
				//    , group, address_next_next, ); // real_name_100
				//
				textWriter.WriteStartElement("_" + group.ToString("X"));
				textWriter.WriteAttributeString("v", n_name);
				switch (group)
				{
					case 0x2:
						possible_t1_struct_c_instance ptsct_02 = new possible_t1_struct_c_instance
						{
							_02_ = new _02
							{
								exe_pointer = M.ReadLong(next_next_next_address.ToString("X"))
							}
						};
						break;
					case 0xA:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xB:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xC:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xD:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xE:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0xF:
						TryGetPossibleStructInstance(next_next_next_address);
						break;
					case 0x29:
						new possible_t1_struct_c_instance
						{
							_29_ = new _29
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2A:
						new possible_t1_struct_c_instance
						{
							_2A_ = new _2A
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2B:
						new possible_t1_struct_c_instance
						{
							_2B_ = new _2B
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2C:
						new possible_t1_struct_c_instance
						{
							_2C_ = new _2C
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2D:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x2E:
						new possible_t1_struct_c_instance
						{
							_2E_ = new _2E
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x2F:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x30:
						new possible_t1_struct_c_instance
						{
							_30_ = new _30
							{
								//tag_struct_pointer = read_a_Group_definitions_link_struct(address)
							}
						};
						break;
					case 0x31:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x34:
						textWriter.WriteAttributeString("length", next_next_next_address.ToString());
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x35:
						textWriter.WriteAttributeString("length", next_next_next_address.ToString());
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x36:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x37:
						new possible_t1_struct_c_instance
						{
							actual_value = next_next_next_address
						};
						break;
					case 0x38:
						new possible_t1_struct_c_instance
						{
							_38_ = new _38
							{
								table2_ref = ReadChunk(next_next_next_address)
							}
						};
						break;
					case 0x39:
						new possible_t1_struct_c_instance
						{
							_39_ = new _39
							{
								Name1 = M.ReadString(M.ReadLong(next_next_next_address.ToString("X")).ToString("X"), "", 300),
								int1 = M.ReadInt((next_next_next_address + 8).ToString("X")),
								int2 = M.ReadInt((next_next_next_address + 12).ToString("X")),
								long1 = M.ReadLong((next_next_next_address + 16).ToString("X")),
								//table2_ref = read_the_big_chunky_one(address) // bruh this in the wrong spot
							}
						};
						// i think we can just ingore that stuff
						int Repeatamount = M.ReadInt((next_next_next_address + 8).ToString("X"));

						for (int i = 0; i < Repeatamount; i++)
						{
							ReadChunk(M.ReadLong((next_next_next_address + 24).ToString("X")));
						}
						break;
					case 0x40:
						new possible_t1_struct_c_instance
						{
							_40_ = new _40
							{
								tag_struct_pointer = GetGDLS(next_next_next_address)
							}
						};
						break;
					case 0x41:
						long child_address = M.ReadLong((next_next_next_address + 136).ToString("X"));
						new possible_t1_struct_c_instance
						{
							_41_ = new _41
							{
								int1 = M.ReadInt((next_next_next_address + 0).ToString("X")),
								taggroup1 = M.ReadString((next_next_next_address + 4).ToString("X"), "", 4),

								taggroup2 = M.ReadString((child_address + 0).ToString("X"), "", 4),
								taggroup3 = M.ReadString((child_address + 4).ToString("X"), "", 4),
								taggroup4 = M.ReadString((child_address + 8).ToString("X"), "", 4),
								taggroup5 = M.ReadString((child_address + 12).ToString("X"), "", 4)
							}
						};
						break;
					case 0x42:
						new possible_t1_struct_c_instance
						{
							_42_ = new _42
							{
								Name1 = M.ReadString(M.ReadLong(next_next_next_address.ToString("X")).ToString("X"), "", 300),
								int1 = M.ReadInt((next_next_next_address + 8).ToString("X")),
								int2 = M.ReadInt((next_next_next_address + 12).ToString("X")),
								int3 = M.ReadInt((next_next_next_address + 16).ToString("X")),
								int4 = M.ReadInt((next_next_next_address + 20).ToString("X")),
								long1 = M.ReadLong((next_next_next_address + 24).ToString("X")),
								long2 = M.ReadLong((next_next_next_address + 32).ToString("X")),
								long3 = M.ReadLong((next_next_next_address + 40).ToString("X")),
								long4 = M.ReadLong((next_next_next_address + 48).ToString("X")),
								long5 = M.ReadLong((next_next_next_address + 56).ToString("X")),
								long6 = M.ReadLong((next_next_next_address + 64).ToString("X")),
							}
						};
						break;
					case 0x43:
						new possible_t1_struct_c_instance
						{
							_43_ = new _43
							{
								Name1 = M.ReadString(M.ReadLong(next_next_next_address.ToString("X")).ToString("X"), "", 300),
								long1 = M.ReadLong((next_next_next_address + 8).ToString("X")),
								//table2_ref = read_the_big_chunky_one(address+16),
								long2 = M.ReadLong((next_next_next_address + 24).ToString("X")),
							}
						};
						break;
				}

				//
				textWriter.WriteEndElement();


			}
			return new Table2_struct { };
		}

		private Group_definitions_link_struct GetGDLS(long address)
		{
			Group_definitions_link_struct gdls = new Group_definitions_link_struct
			{
				name1 = M.ReadString(M.ReadLong(address.ToString("X")).ToString("X"), "", 300),
				name2 = M.ReadString(M.ReadLong((address + 8).ToString("X")).ToString("X"), "", 300),

				int1 = M.ReadInt((address + 16).ToString("X")),
				int2 = M.ReadInt((address + 20).ToString("X")), // potential count

				Table2_struct_pointer2 = M.ReadLong((address + 24).ToString("X")),
				Table2_struct = ReadChunk(M.ReadLong((address + 24).ToString("X"))), // next

			};

			return gdls;
		}

		private possible_t1_struct_c_instance TryGetPossibleStructInstance(long address)
		{


			int count_of_children = M.ReadInt((address + 8).ToString("X"));
			long children_address = M.ReadLong((address + 16).ToString("X"));
			List<string> childs = new();

			for (int i = 0; i < count_of_children; i++)
			{
				textWriter.WriteStartElement("Flag");

				long address_WHY_WONT_YOU_WORK = M.ReadLong((address + 16).ToString("X"));

				string reuse_me_uh = M.ReadString(M.ReadLong((address_WHY_WONT_YOU_WORK + (i * 8)).ToString("X")).ToString("X"), "", 300);
				childs.Add(reuse_me_uh);

				textWriter.WriteAttributeString("v", reuse_me_uh);


				textWriter.WriteEndElement();
			}

			possible_t1_struct_c_instance ptsct_0A = new possible_t1_struct_c_instance
			{
				_0B_through_0F_ = new _0B_through_0F
				{
					name = M.ReadString(M.ReadLong(address.ToString("X")).ToString("X"), "", 300),
					count = count_of_children,
					children = childs
				}
			};

			return ptsct_0A;
		}
	}
}
