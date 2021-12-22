using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

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
			public long AbsoluteTagOffset { get; set; }
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

			/// <summary>
			/// String description of the flags
			/// </summary>
			public Dictionary<int, string> STR { get; set; } = new Dictionary<int, string>();
		}

		public static Dictionary<string, Dictionary<long, C>> Tags = new Dictionary<string, Dictionary<long, C>>()
		{
			{
				"vehi",
				new() {

					{ 40, new C { T = "Float" } },
					{ 44, new C { T = "Float" } },
					{ 48, new C { T = "Float" } },
					{ 52, new C { T = "Float" } },
					{ 56, new C { T = "Float" } },
					{ 60, new C { T = "Float" } },
					{ 64, new C { T = "Float" } },
					{ 76, new C { T = "Float" } },
					{ 80, new C { T = "Float" } },
					{ 84, new C { T = "Float" } },

					{ 92, new C { T = "Tagblock" } }, // SidecarPathDefinition

					{ 112, new C { T = "mmr3Hash", N = "Default Variant" } }, // default variant
					{ 116, new C { T = "Float" } },

					{ 120, new C { T = "TagRef", N = "Model" } }, // vehicle model 
					{ 148, new C { T = "TagRef" } }, // aset tag ref
					{ 176, new C { T = "TagRef" } },
					{ 204, new C { T = "TagRef" } },

					{ 232, new C { T = "4Byte" } },

					{ 240, new C { T = "TagRef" } },

					{ 276, new C { T = "Float" } },

					{ 288, new C { T = "TagRef" } },
					{ 316, new C { T = "TagRef" } }, // foot tag ref
					{ 344, new C { T = "TagRef" } }, // vemd tag ref
					{ 372, new C { T = "TagRef" } }, // smed tag ref
					{ 400, new C { T = "TagRef" } },

					{ 432, new C { T = "Float" } },

					{ 448, new C { T = "Tagblock" } }, // object_ai_properties
					{ 468, new C { T = "Tagblock" } }, // s_object_function_definition
					{ 488, new C { T = "4Byte" } },
					{ 492, new C { T = "Tagblock" } }, // ObjectRuntimeInterpolatorFunctionsBlock
					{ 512, new C { T = "Tagblock" } }, // ObjectFunctionSwitchDefinition
					{ 532, new C { T = "Tagblock" } }, // i343::Objects::ObjectFunctionForwarding
					{ 552, new C { T = "4Byte" } },
					{ 556, new C { T = "Tagblock" } }, // i343::Objects::AmmoRefillVariant
					{ 576, new C { T = "4Byte" } },
					{ 0x248,new C{T = "Tagblock",B = new Dictionary<long, C> // object_attachment_definition
					{
						{ 4, new C{ T="TagRef"}}, // effe
						{ 32, new C{ T="TagRef"}}, // effe
						{ 64, new C{ T="Tagblock"}},
						{ 84, new C{ T="TagRef"}}, //
						{ 112, new C{ T="Tagblock"}}
					},S = 148}},

					{ 604, new C { T = "Tagblock" } }, // object_indirect_lighting_settings_definition
					{ 624, new C { T = "Tagblock" } }, // s_water_physics_hull_surface_definition
					{ 644, new C { T = "Tagblock" } }, // s_jetwash_definition
					{ 664, new C { T = "Tagblock" } }, // object_definition_widget
					{ 684, new C { T = "Tagblock" } }, // object_change_color_definition
					{ 704, new C { T = "Tagblock" } }, // s_multiplayer_object_properties_definition
					{ 724, new C { T = "Tagblock" } }, // i343::Objects::ForgeObjectEntryDefinition

					{ 744, new C { T = "TagRef" } },
					{ 772, new C { T = "TagRef" } },

					{ 800, new C { T = "Tagblock" } }, // s_object_spawn_effects
					{ 820, new C { T = "Tagblock" } }, // ModelDissolveDataBlock

					{ 840, new C { T = "String" } }, // (vehicle_partial_emp)

					{ 0x448, new C { T = "Tagblock" } }, // HsReferencesBlock
					{ 0x45C, new C { T = "TagRef" } },

					{ 0x480, new C { T = "Float" } },

					{ 0x484, new C { T = "Tagblock" } }, // metalabel somethiong ?? recheck this one
					{ 0x498, new C { T = "Tagblock" } }, // SoundRTPCBlockDefinition
					{ 0x4AC, new C { T = "Tagblock" } }, // SoundSweetenerBlockDefinition

					{ 0x4C0, new C { T = "Pointer" } },

					{ 0x4C8, new C { T = "Tagblock" } }, // i343::Objects::ComputeFunctionSmoothingBlockDefinition

					{ 0x4DC, new C { T = "Float" } },
					{ 0x4E0, new C { T = "Float" } },

					{ 0x4E4, new C { T = "TagRef" } },
					{ 0x500, new C { T = "Tagblock" } }, // i343::SpartanTracking::ObjectDefinition

					{ 0x514, new C { T = "TagRef" } },
					{ 0x530, new C { T = "Tagblock" } }, // InteractionOpportunityDefinition

					{ 0x544, new C { T = "Float" } },
					{ 0x548, new C { T = "Tagblock" } }, // ScriptedSequenceActionDefinition
					{ 0x55C, new C { T = "TagRef" } },
					{ 0x578, new C { T = "Tagblock" } }, // AnimChannelEntry
					{ 0x58C, new C { T = "Tagblock" } }, // AnimSetTableEntry

					{ 0x5A0, new C { T = "Float" } },
					{ 0x5A4, new C { T = "Float" } },
					{ 0x5A8, new C { T = "Float" } },
					{ 0x5AC, new C { T = "Float" } },
					{ 0x5B0, new C { T = "Float" } },
					{ 0x5B4, new C { T = "Float" } },
					{ 0x5B8, new C { T = "Float" } },
					{ 0x5BC, new C { T = "Float" } },
					{ 0x5C0, new C { T = "Float" } },
					{ 0x5CC, new C { T = "4Byte" } },
					{ 0x5DC, new C { T = "Float" } },
					{ 0x5E0, new C { T = "Float" } },
					{ 0x5E4, new C { T = "Float" } },

					{ 0x5E8, new C { T = "Tagblock" } }, // LegGroundingSettings

					{ 0x5FC, new C { T = "Float" } },
					{ 0x600, new C { T = "Float" } },
					{ 0x604, new C { T = "Float" } },
					{ 0x608, new C { T = "Float" } },

					{ 0x60C, new C { T = "Tagblock" } }, // i343::Objects::ObjectNodeGraphDefinition
					{ 0x620, new C { T = "Tagblock" } }, // i343::Objects::AnimationMatchingTableEntry

					{ 0x634, new C { T = "Float" } },

					{ 0x638, new C { T = "Tagblock" } }, // i343::Objects::ModelVariantSwappingTableEntry

					{ 0x63C, new C { T = "Float" } },

					{ 0x650, new C { T = "Tagblock" } }, // i343::Items::LocationSensorDefinition
					{ 0x664, new C { T = "Tagblock" } }, // i343::Items::ShroudGeneratorDefinition
					{ 0x678, new C { T = "Tagblock" } }, // i343::Objects::PowerComponentDefinition
					{ 0x68C, new C { T = "Tagblock" } }, // i343::Objects::SelfDestructHandlerDefinition
					{ 0x6A0, new C { T = "Tagblock" } }, // i343::Objects::IndirectLightingComponentDefinition

					{ 0x6B4, new C { T = "Float" } },
					{ 0x6B8, new C { T = "4Byte" } },
					{ 0x6BC, new C { T = "4Byte" } },

					{ 0x6C4, new C { T = "TagRef" } },
					{ 0x6E0, new C { T = "Tagblock" } }, // s_campaign_metagame_bucket
					{ 0x6F4, new C { T = "Tagblock" } }, // s_unit_screen_effect_definition

					{ 0x70C, new C { T = "Float" } },
					{ 0x720, new C { T = "Tagblock" } }, // s_unit_camera_track
					{ 0x768, new C { T = "Tagblock" } }, // s_unit_camera_acceleration
					{ 0x77C, new C { T = "Float" } },
					{ 0x790, new C { T = "Tagblock" } }, // s_unit_camera_track
					{ 0x7D8, new C { T = "Tagblock" } }, // s_unit_camera_acceleration

					{ 0x7F0, new C { T = "TagRef" } },

					{ 0x818, new C { T = "Float" } },
					{ 0x81C, new C { T = "Float" } },
					{ 0x830, new C { T = "Float" } },

					{ 0x870, new C { T = "TagRef" } },

					{ 0x898, new C { T = "Float" } },

					{ 0x8BC, new C { T = "Tagblock" } }, // WeaponSpecificMarkers
					{ 0x8D0, new C { T = "TagRef" } },
					{ 0x908, new C { T = "TagRef" } },
					{ 0x924, new C { T = "TagRef" } },
					{ 0x940, new C { T = "TagRef" } },
					{ 0x95C, new C { T = "TagRef" } },
					{ 0x978, new C { T = "TagRef" } },
					{ 0x994, new C { T = "TagRef" } },
					{ 0x9B0, new C { T = "TagRef" } },

					{ 0x9D0, new C { T = "Float" } },

					{ 0x9E0, new C { T = "Tagblock" } }, // HudUnitSoundDefinitions
					{ 0x9F4, new C { T = "Tagblock" } }, // dialogue_variant_definition

					{ 0xA30, new C { T = "Float" } },
					{ 0xA34, new C { T = "Float" } },
					{ 0xA38, new C { T = "Float" } },

					{ 0xA64, new C { T = "Tagblock" } }, // powered_seat_definition
					{0xA78,new C{T = "Tagblock",B = new Dictionary<long, C> // unit_initial_weapon
					{
						{ 0, new C{ T="TagRef", N = "Weapon"}}, // weap
						{ 40, new C{ T="Float"}},
						{ 48, new C{ T="Float"}},
						{ 148, new C{ T="TagRef"}}, //
						{ 188, new C{ T="Tagblock"}}
					},S = 212}},

					{ 0xA8C, new C { T = "Tagblock" } }, // s_target_tracking_parameters

					{ 0xAA0, new C {T = "Tagblock",B = new Dictionary<long, C>  // unit_seat
					{
						{ 0x0, new FlagGroup {A = 4,STR = new Dictionary<int, string>() {
							{ 0,  "invisible"  },
							{ 1,  "locked"  },
							{ 2,  "driver"  },
							{ 3,  "gunner"  },
							{ 4,  "third person camera"  },
							{ 5,  "allows weapons"  },
							{ 6,  "third person on enter"  },
							{ 7,  "first person camera slaved to gun."  },
							{ 8,  "not valid without driver"  },
							{ 9,  "allow AI noncombatants"  },
							{ 10, "boarding seat"  },
							{ 11, "ai firing disabled by max acceleration"  },
							{ 12, "boarding enters seat"  },
							{ 13, "boarding need any passenger"  },
							{ 14, "invalid for player"  },
							{ 15, "invalid for non-player"  },
							{ 16, "invalid for hero"  },
							{ 17, "gunner (player only)"  },
							{ 18, "invisible under major damage"  },
							{ 19, "melee instant killable"  },
							{ 20, "leader preference"  },
							{ 21, "allows exit and detach"  },
							{ 22, "blocks headshots"  },
							{ 23, "exits to ground"  },
							{ 24, "forward from attachment"  },
							{ 25, "disallow AI shooting"  },
							{ 26, "prevents weapon stowing"  },
							{ 27, "takes top level aoe damage"  },
							{ 28, "disallow exit"  },
							{ 29, "local aiming"  },
							{ 30, "pelvis relative attachment"  },
							{ 31, "apply velocity on death exit"  }
						}} },

						{ 0x3, new FlagGroup {A = 4, STR = new Dictionary<int, string>() 
						{
							{ 0, "skip obstacle check"  },
							{ 1, "search parent for entry marker"  },
							{ 2, "gunner release aim on exit"  },
							{ 3, "fully open before allowing exit"  },
							{ 4, "finish melee before allowing exit"  },
							{ 5, "kill parent if unit in seat dies"  },
							{ 6, "co-pilot"  },
							{ 7, "ejectable seat"  },
							{ 8, "kill on ejection"  },
							{ 9, "use head marker for navpoint"  },
							{ 10, "allows equipment and grenade switching"  },
						} } },

						{ 0x10, new C{ T="mmr3Hash", N = "Animation Hash"}},

						{ 0x14, new C{ T="String"}},
						{ 0x58, new C{ T="Tagblock"}},
						{ 0x06C, new C { T = "Float" }},

						{ 0x074, new C{ T="TagRef"}},

						{ 0x90, new C { T = "Float" }},

						{ 0x98, new C{ T="Tagblock"}},

						{ 0xE8, new C { T = "Float" }},
						{ 0xEC, new C { T = "Float" }},
						{ 0xF0, new C { T = "Float" }},
						{ 0xF4, new C { T = "Float" }},
						{ 0xF8, new C { T = "Float" }},
						{ 0xFC, new C { T = "Float" }},
						{ 0x100, new C { T = "Float" }},

						{ 0x11C, new C{ T="Tagblock"}},

						{ 0x130, new C { T = "Float" }},
						{ 0x134, new C { T = "Float" }},
						{ 0x138, new C { T = "Float" }},

						{ 0x148, new C{ T="Float"}},

						{ 0x164, new C{ T="Tagblock"}},
						{ 0x178, new C{ T="TagRef"}},
						{ 0x194, new C{ T="TagRef"}},
						{ 0x1B4, new C{ T="Tagblock"}},

						{ 0x1E8, new C{ T="TagRef"}},
						{ 0x212, new C{ T="TagRef"}},
						{ 0x24C, new C{ T="TagRef"}},

					},S = 632}},

					{ 0xAB4, new C { T = "Float" } },
					{ 0xAB8, new C { T = "Float" } },
					{ 0xABC, new C { T = "Float" } },

					{ 0xAC8, new C { T = "TagRef" } },
					{ 0xAE4, new C { T = "Tagblock" } }, // i343::Objects::PowerComponentDefinition
					{ 0xAF8, new C { T = "TagRef" } },
					{ 0xB14, new C { T = "TagRef" } },

					{ 0xB38, new C { T = "Float" } },
					{ 0xB3C, new C { T = "Float" } },
					{ 0xB40, new C { T = "Float" } },
					{ 0xB44, new C { T = "Float" } },
					{ 0xB48, new C { T = "Float" } },
					{ 0xB4C, new C { T = "Float" } },

					{ 0xB54, new C { T = "Pointer" } },
					{ 0xB5C, new C { T = "Pointer" } },

					{ 0xB7C, new C { T = "TagRef" } },
					{ 0xB98, new C { T = "TagRef" } },

					{ 0xBB4, new C { T = "Tagblock" } }, // ExitAndDetachVariant

					{ 0xBCC, new C { T = "Float" } },

					{ 0xBE4, new C { T = "TagRef" } },

					{ 0xC00, new C { T = "Float" } },
					{ 0xC04, new C { T = "Float" } },
					{ 0xC08, new C { T = "Float" } },
					{ 0xC0C, new C { T = "Float" } },
					{ 0xC10, new C { T = "Float" } },
					{ 0xC18, new C { T = "Float" } },
					{ 0xC1C, new C { T = "Float" } },
					{ 0xC28, new C { T = "Float" } },
					{ 0xC30, new C { T = "Float" } },
					{ 0xC34, new C { T = "Float" } },
					{ 0xC3C, new C { T = "Float" } },
					{ 0xC40, new C { T = "Float" } },
					{ 0xC5C, new C { T = "Float" } },
					{ 0xC64, new C { T = "Float" } },
					{ 0xC68, new C { T = "Float" } },
					{ 0xC70, new C { T = "Float" } },
					{ 0xC74, new C { T = "Float" } },
					{ 0xC7C, new C { T = "Float" } },
					{ 0xC80, new C { T = "Float" } },
					{ 0xC84, new C { T = "Float" } },
					{ 0xC88, new C { T = "Float" } },
					{ 0xC8C, new C { T = "Float" } },
					{ 0xC94, new C { T = "Float" } },
					{ 0xC98, new C { T = "Float" } },
					{ 0xCA0, new C { T = "Float" } },
					{ 0xCAC, new C { T = "Float" } },
					{ 0xCB8, new C { T = "Float" } },
					{ 0xCBC, new C { T = "Float" } },
					{ 0xCC4, new C { T = "Float" } },
					{ 0xCC8, new C { T = "Float" } },
					{ 0xCD0, new C { T = "Float" } },
					{ 0xCD4, new C { T = "Float" } },
					{ 0xCDC, new C { T = "Float" } },
					{ 0xCE0, new C { T = "Float" } },
					{ 0xCE8, new C { T = "Float" } },
					{ 0xCF4, new C { T = "Float" } },
					{ 0xCF8, new C { T = "Float" } },
					{ 0xD00, new C { T = "Float" } },
					{ 0xD04, new C { T = "Float" } },
					{ 0xD0C, new C { T = "Float" } },
					{ 0xD10, new C { T = "Float" } },
					{ 0xD18, new C { T = "Float" } },
					{ 0xD1C, new C { T = "Float" } },
					{ 0xD24, new C { T = "Float" } },

					{ 0xD28, new C { T = "TagRef" } },
					{ 0xD44, new C { T = "TagRef" } }, // EFFECT TAG REF
					{ 0xD64, new C { T = "TagRef" } },
					{ 0xD80, new C { T = "TagRef" } }, // VEHI TAG REF

					{ 0xDA0, new C { T = "Tagblock" } }, // s_vehicle_human_tank_definition
					{ 0xDB4, new C { T = "Tagblock" } }, // s_vehicle_human_jeep_definition
					{ 0xDC8, new C { T = "Tagblock" } }, // s_vehicle_human_plane_definition
					
					{ 0xDDC, new C { T = "Tagblock", B = new Dictionary<long, C>
					{
						{0x18, new C { T="Float", N = "Max Forward Speed" } },
						{0x1C, new C { T="Float", N = "Max Reverse Speed" } },
						{0x20, new C { T="Float", N = "Speed Acceleration" } },
						{0x24, new C { T="Float", N = "Speed Deceleration" } },
						{0x28, new C { T="Float", N = "Max Left Slide" } },
						{0x2C, new C { T="Float", N = "Max Right Slide" } },
						{0x30, new C { T="Float", N = "Slide Acceleration" } },
						{0x34, new C { T="Float", N = "Slide Deceleration" } },
						{0x50, new C { T="Float", N = "Torque Scale" } },

						{0x54, new C { T = "mmr3Hash", N = "Engine Object Function Region" } },
						{0x58, new C { T="Float", N = "Min Anti Gravity Engine Speed" } },
						{0x5C, new C { T="Float", N = "Max Anti Gravity Engine Speed" } },
						{0x60, new C { T="Float", N = "Engine Speed Acceleration" } },
						{0x64, new C { T="Float", N = "Maximum Vehicle Speed" } },

						{0x68, new C { T = "mmr3Hash", N = "Contrail Object Function Region" } },
						{0x6C, new C { T="Float", N = "Min Anti Gravity Engine Speed" } },
						{0x70, new C { T="Float", N = "Max Anti Gravity Engine Speed" } },
						{0x74, new C { T="Float", N = "Engine Speed Acceleration" } },
						{0x78, new C { T="Float", N = "Maximum Vehicle Speed" } },
					} } }, // s_vehicle_alien_scout_definition
					
					{ 0xDF0, new C { T = "Tagblock", B = new Dictionary<long, C>
					{
						{0x20, new C { T="Float", N = "Turn Rate" } },
						{0x24, new C { T="Float", N = "Forward Speed" } },
						{0x28, new C { T="Float", N = "Reverse Speed" } },
						{0x2C, new C { T="Float", N = "Speed Acceleration" } },
						{0x30, new C { T="Float", N = "Speed Deceleration" } }
					} } }, // s_vehicle_alien_fighter_definition
					
					{ 0xE04, new C { T = "Tagblock" } }, // s_vehicle_turret_definition
										
					{ 0xE18, new C { T = "Tagblock", B = new Dictionary<long, C>
					{
						{0x4C, new C { T="Float", N = "Up Acceleration" } },
						{0x50, new C { T="Float", N = "Down Acceleration" } },
						{0x60, new C { T="Float", N = "Turn Acceleration Min" } },
						{0x64, new C { T="Float", N = "Turn Acceleration Max" } },
						{0x208, new C { T="Float", N = "Left Acceleration" } },
						{0x20C, new C { T="Float", N = "Reverse Acceleration" } },
						{0x22C, new C { T="Float", N = "Forward Acceleration" } }
					} } }, // s_vehicle_vtol_definition
					
					{ 0xE2C, new C { T = "Tagblock" } }, // s_vehicle_chopper_definition
					{ 0xE40, new C { T = "Tagblock" } }, // s_vehicle_guardian_definition
					{ 0xE54, new C { T = "Tagblock" } }, // s_vehicle_jackal_glider_definition
					{ 0xE68, new C { T = "Tagblock" } }, // s_vehicle_space_fighter_definition
					{ 0xE7C, new C { T = "Tagblock" } }, // s_vehicle_revenant_definition

					{ 0xE90, new C { T = "Float" } },
					{ 0xE94, new C { T = "4Byte" } },

					{ 0xE98, new C { T = "Tagblock" } }, // i343::Vehicles::AntiGravityPointConfiguration
					{ 0xEAC, new C { T = "Tagblock" } }, // i343::Vehicles::AntiGravityPointDefinition
					{ 0xEC0, new C { T = "Tagblock" } }, // i343::Vehicles::FrictionPointConfiguration
					{ 0xED4, new C { T = "Tagblock" } }, // i343::Vehicles::FrictionPointDefinition

					{ 0xEE8, new C { T = "Float" } },
					{ 0xEEC, new C { T = "Float" } },

					{ 0xEF0, new C { T = "Pointer" } },
					{ 0xEF8, new C { T = "Pointer" } },
					{ 0xF08, new C { T = "Pointer" } },
					{ 0xF10, new C { T = "Pointer" } },

					{ 0xF2C, new C { T = "Tagblock" } }, // s_unit_trick_definition
					{ 0xF40, new C { T = "Float" } },
					{ 0xF50, new C { T = "Float" } },
					{ 0xF54, new C { T = "Float" } },
					{ 0xF58, new C { T = "Float" } },
					{ 0xF60, new C { T = "Float" } },
					{ 0xF74, new C { T = "4Byte" } },

					{ 0xF78, new C { T = "TagRef" } },
					{ 0xF94, new C { T = "TagRef" } },
					{ 0xFB0, new C { T = "TagRef" } },
					{ 0xFD0, new C { T = "TagRef" } },
					{ 0xFF0, new C { T = "TagRef" } },
					{ 0x1024, new C { T = "Tagblock" } }, // SoundRTPCBlockDefinition
					{ 0x1038, new C { T = "Tagblock" } }, // SoundSweetenerBlockDefinition

					{ 0x104C, new C { T = "TagRef" } },
					{ 0x1068, new C { T = "TagRef" } },
					{ 0x1084, new C { T = "TagRef" } },

					{ 0x10A4, new C { T = "Tagblock" } }, // s_vehicleAiCruiseControl
					{ 0x10B8, new C { T = "Tagblock" } }, // Interface::UIItemInfo

					{ 0x10CC, new C { T = "TagRef" } },
					{ 0x10EC, new C { T = "Float" } },
				}
			}
			,
			{
				"weap",
				new() {
					{ 0x05C, new C { T = "Tagblock" } },
					{ 112, new C { T = "mmr3Hash" , N = "Default variant" } }, // default variant

					{ 0x078, new C { T = "TagRef", N = "Model" } }, // HLMT
					{ 0x094, new C { T = "TagRef" } },
					{ 0x0B0, new C { T = "TagRef" } },
					{ 0x0CC, new C { T = "TagRef" } },
					{ 0x0F0, new C { T = "TagRef" } },

					{ 0x10C, new C { T = "Tagblock" } },

					{ 0x120, new C { T = "TagRef" } },
					{ 0x13C, new C { T = "TagRef" } }, // FOOT
					{ 0x158, new C { T = "TagRef" } }, // VMED
					{ 0x174, new C { T = "TagRef" } }, // SMED
					{ 0x190, new C { T = "TagRef" } },

					{ 0x1C0, new C { T = "Tagblock" } },
					{ 0x1D4, new C { T = "Tagblock" } },
					{ 0x1EC, new C { T = "Tagblock" } },
					{ 0x200, new C { T = "Tagblock" } },
					{ 0x214, new C { T = "Tagblock" } },
					{ 0x22C, new C { T = "Tagblock" } },
					{ 0x248,new C {T = "Tagblock", B = new Dictionary<long, C> // attachment block
					{
						{ 4, new C{ T="TagRef"}}, // effe
						{ 32, new C{ T="TagRef"}}, // effe
						{ 64, new C{ T="Tagblock"}},
						{ 84, new C{ T="TagRef"}}, //
						{ 112, new C{ T="Tagblock"}}
					},S = 148} },

					{ 0x25C, new C { T = "Tagblock" } },
					{ 0x270, new C { T = "Tagblock" } },
					{ 0x284, new C { T = "Tagblock" } },
					{ 0x298, new C { T = "Tagblock" } },
					{ 0x2AC, new C { T = "Tagblock" } },
					{ 0x2C0, new C { T = "Tagblock" } },
					{ 0x2D4, new C { T = "Tagblock" } },

					{ 0x2E8, new C { T = "TagRef" } },
					{ 0x304, new C { T = "TagRef" } },

					{ 0x320, new C { T = "Tagblock" } },
					{ 0x334, new C { T = "Tagblock" } },
					{ 0x448, new C { T = "Tagblock" } },

					{ 0x45C, new C { T = "TagRef" } },

					{ 0x484, new C { T = "Tagblock" } },
					{ 0x498, new C { T = "Tagblock" } },
					{ 0x4AC, new C { T = "Tagblock" } },
					{ 0x4C8, new C { T = "Tagblock" } },

					{ 0x4E4, new C { T = "TagRef" } },

					{ 0x500, new C { T = "Tagblock" } },
					{ 0x514, new C { T = "TagRef" } },

					{ 0x530, new C { T = "Tagblock" } },
					{ 0x548, new C { T = "Tagblock" } },

					{ 0x55C, new C { T = "TagRef" } },

					{ 0x578, new C { T = "Tagblock" } },
					{ 0x58C, new C { T = "Tagblock" } },
					{ 0x5E8, new C { T = "Tagblock" } },
					{ 0x60C, new C { T = "Tagblock" } },
					{ 0x620, new C { T = "Tagblock" } },
					{ 0x638, new C { T = "Tagblock" } },
					{ 0x650, new C { T = "Tagblock" } },
					{ 0x664, new C { T = "Tagblock" } },
					{ 0x678, new C { T = "Tagblock" } },
					{ 0x68C, new C { T = "Tagblock" } },
					{ 0x6A0, new C { T = "Tagblock" } },
					{ 0x730, new C { T = "Tagblock" } },
					{ 0x744, new C { T = "Tagblock" } },

					{ 0x758, new C { T = "TagRef" } },
					{ 0x77C, new C { T = "TagRef" } },
					{ 0x798, new C { T = "TagRef" } },
					{ 0x7D8, new C { T = "TagRef" } },
					{ 0x7F4, new C { T = "TagRef" } },
					{ 0x850, new C { T = "TagRef" } }, // WEAP

					{ 0x86C, new FlagGroup { A = 4, STR = new Dictionary<int, string>() 
					{ 
						{ 0,  "Vertical Heat Display"  },
						{ 1,  "Mutually Exclusive Triggers"  },
						{ 2,  "Attacks Automatically On Bump"  },
						{ 3,  "Must Be Readied"  },
						{ 4,  "Doesn't Count Toward Maximum"  },
						{ 5,  "Aim Assists Only When Zoomed"  },
						{ 6,  "Prevents Grenade Throwing"  },
						{ 7,  "Must Be Picked Up"  },
						{ 8,  "Holds Triggers When Dropped"  },
						{ 9,  "Prevents Melee Attack"  },
						{ 10, "Detonates When Dropped"  },
						{ 11, "Cannot Fire At Maximum Age"  },
						{ 12, "Secondary Trigger Overrides Grenades"  },
						{ 13, "Support Weapon"  },
						{ 14, "Hide FP Weapon When In Iron Sights"  },
						{ 15, "AIs Use Weapon Melee Damage"  },
						{ 16, "Allows Binoculars"  },
						{ 17, "Loop FP Firing Animation"  },
						{ 18, "Prevents Crouching"  },
						{ 19, "Use Empty Melee On Empty"  }, // Cannot Fire While Boosting // cut out this value as it doesn't allign with infinites things
						{ 20, "Uses 3rd Person Camera"  }, // thrid person
						{ 21, "Can Be Dual Wielded"  },
						{ 22, "Can Only Be Dual Wielded"  },
						{ 23, "Melee Only"  },
						{ 24, "Can't Fire If Parent Dead"  },
						{ 25, "Weapon Ages With Each Kill"  },
						{ 26, "Allows Unaimed Lunge"  },
						{ 27, "Cannot Be Used By Player"  },
						{ 28, "Hold FP Firing Animation"  },
						{ 29, "Strict Deviation Angle"  },
						{ 30, "Notifies Target Units"  },
						{ 31, "flag 31"  }
					} } },
					{ 0x870, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
					{
						{ 0,  "i just copied these from H2A lol, not all apply as intended"  }, // Magnetizes Only When Zoomed
						{ 1,  "Force Enable Equipment Tossing"  },
						{ 2,  "Non-Lunge Melee Dash Disabled"  },
						{ 3,  "Don't Drop On Dual Wield Melee"  },
						{ 4,  "Is Equipment Special Weapon"  },
						{ 5,  "Uses Ghost Reticle"  },
						{ 6,  "Never Overheats"  },
						{ 7,  "Force Tracers To Come From Weapon Barrel"  },
						{ 8,  "Cannot Fire During EMP"  },
						{ 9,  "Weapon Can Headshot"  },
						{ 10, "AI Cannot Fire Tracking Projectiles"  },
						{ 11, "Second Barrel Fires If Friend Is Targeted"  },
						{ 12, "Weapon Unzooms On Damage"  },
						{ 13, "Do Not Drop On Equipment Activation"  },
						{ 14, "Weapon Can Not Be Dropped"  },
						{ 15, "Disable Function Overlays During Reload"  },
						{ 16, "Throw Weapon Instead Of Grenade"  },
						{ 17, "Do Not Drop 'Must Be Readied' On Primary Trigger"  },
						{ 18, "Delete On Drop"  },
						{ 19, "Allow Melee When Using Device"  },
						{ 20, "Do Not Lower Weapon When Using Device"  },
						{ 21, "Cannot Fire While Zooming"  },
						{ 22, "Weapon Ages When Damage Is Inflicted"  },
						{ 23, "Apply Gunner Armor Mod Abilites"  },
						{ 24, "Weapon Drops Further Away"  }, //Wielders Sprint Is Unaffected By Soft Ping // swapped with weapon drops further away
						{ 25, "flag 25"  }, 
						{ 26, "Use Automatic Firing Looping Sounds"  },
						{ 27, "Do Not Drop On Assassination"  },
						{ 28, "Is Part Of Body"  },
						{ 29, "Force Deny Equipment Use"  },
						{ 30, "Hide Pickup Prompt Unless Special Pickup Priority"  },
						{ 31, "Weapon Ignores Player Pickup Allowed Trait"  }
					} } },

					// { 0x86C, new C { T = "Flags" } },
					// { 0x86D, new C { T = "Flags" } },
					// { 0x86E, new C { T = "Flags" } }, // thrid person (5)
					// { 0x86F, new C { T = "Flags" } },
					// 
					// { 0x870, new C { T = "Flags" } },
					// { 0x871, new C { T = "Flags" } },
					// { 0x872, new C { T = "Flags" } },
					// { 0x873, new C { T = "Flags" } },

					{ 0x88C, new C { T = "TagRef" } },
					{ 0x8A8, new C { T = "TagRef" } },

					{ 0x924, new C { T = "TagRef" } },
					{ 0x940, new C { T = "TagRef" } },
					{ 0x95C, new C { T = "TagRef" } },
					{ 0x978, new C { T = "TagRef" } },

					{ 0x994, new C { T = "Tagblock" } },

					{ 0x9A8, new C { T = "TagRef" } },
					{ 0x9D4, new C { T = "TagRef" } },

					{ 0x9F0, new C { T = "Tagblock" } },

					{ 0xA08, new C{ T="Float"}},
					{ 0xA14, new C{ T="Float"}},
					{ 0xA44, new C{ T="Float"}},

					{ 0xA80, new C { T = "Tagblock" } },
					{ 0xA94, new C { T = "Tagblock" } },

					{ 0xAF0, new C { T = "TagRef" } },
					{ 0xB0C, new C { T = "TagRef" } },

					{ 0xB6C, new C { T = "Tagblock" } },
					{ 0xB88, new C { T = "Tagblock" } },
					{ 0xBA4, new C { T = "Tagblock" } },

					{ 0xBB8, new C { T = "TagRef" } },
					{ 0xBE4, new C { T = "TagRef" } },
					{ 0xC00, new C { T = "TagRef" } },

					{ 0xC1C, new C { T = "Tagblock" } },

					{ 0xC30, new C { T = "TagRef" } }, // BITM

					{ 0xC4C,new C {T = "Tagblock", B = new Dictionary<long, C> // magazine block
					{
						{ 0x6, new C { T = "2Byte", N="Starting ammo (clip+reserve)" } },
						{ 0x8, new C { T = "2Byte" } },
						{ 0xA, new C { T = "2Byte", N="Maximum clip ammo" } },
						{ 0xC, new C { T = "2Byte", N="Maximum reserve ammo" } },
						{ 0xE, new C { T = "2Byte" } },
						{ 0x10, new C { T = "2Byte" } },
						{ 0x12, new C { T = "2Byte" } },
						{ 0x14, new C { T = "2Byte", N="Rounds per reload" } },
						{ 0x16, new C { T = "2Byte" } },
						{ 0x18, new C { T = "2Byte" } },
						{ 0x1A, new C { T = "2Byte" } },
						{ 0x1C, new C { T = "TagRef" } },
						{ 0x38, new C { T = "TagRef" } },
						{ 0x54, new C { T = "TagRef" } },
						{ 0x70, new C { T = "TagRef" } },
						{ 0x8C, new C{ T="Tagblock", B=new Dictionary<long, C> // refill object block
						{
							{ 0x4, new C{ T="TagRef"}}, //
						}, S=32}},
					}, S = 160}},

					{ 0xC60, new C { T = "TagRef" } },
					{ 0xC7C, new C { T = "Tagblock" } },
					{ 0xC90, new C {T = "Tagblock", B = new Dictionary<long, C> // barrel block
					{
						{ 4, new C{ T="Float", N = "Minimum Rounds per Minute"}},
						{ 8, new C{ T="Float", N = "Maximum Rounds per Minute"}},

						{ 60, new C{ T="Float", N = "Fire Recovery Time"}},
						{ 64, new C{ T="Float"}},

						{ 76, new C{ T="Float"}},
						{ 80, new C{ T="Float"}},
						{ 86, new C { T = "2Byte", N = "Rounds Per Shot" } },
						
						{ 92, new C{ T="Float"}},
						{ 130, new C { T = "2Byte", N = "Projectiles Per Shot" } },
						
						{ 0x64, new C{ T="4Byte"}},
						{ 0x68, new C{ T="Float"}},

						{ 0x84, new C{ T="Tagblock"}},
						{ 0x98, new C{ T="Tagblock"}},
						{ 0xB8, new C{ T="Float"}},
						{ 0xBC, new C{ T="Float"}},
						{ 0xFC, new C{ T="Tagblock" } },

						{ 0x110, new C{ T="Tagblock"}},
						{ 0x124, new C{ T="Tagblock"}},
						{ 0x138, new C{ T="Tagblock"}},
						{ 0x150, new C{ T="Tagblock"}},
						{ 0x164, new C{ T="Tagblock"}},
						{ 0x178, new C{ T="Tagblock"}},
						{ 0x18C, new C{ T="Tagblock"}},
						{ 0x1A4, new C{ T="Tagblock"}},

						{ 0x1B8, new C{ T="TagRef"}}, // PROJ
						{ 0x1D4, new C{ T="TagRef", N = "Projectile"}}, // PROJ

						{ 0x1F4, new C{ T="Tagblock"}},
						{ 0x208, new C{ T="TagRef"}},
						{ 0x224, new C{ T="TagRef"}},
						{ 0x240, new C{ T="TagRef", N = "Crate Projectile"}},

						{ 0x25C, new C{ T="Float", N = "Crate Velocity"}},
						{ 0x260, new C{ T="Float"}},
						{ 0x264, new C{ T="Float"}},
						{ 0x268, new C{ T="Float"}},
						{ 0x26C, new C{ T="Float"}},
						{ 0x270, new C{ T="Float"}},
						{ 0x274, new C{ T="Float"}},
						{ 0x2B0, new C{ T="Tagblock", B = new Dictionary<long, C> // THE FIRING EFFECTS
						{
							{ 0x04, new C{ T="TagRef"}},

							{ 0x24, new C{ T="Tagblock"}},
							{ 0x38, new C{ T="Tagblock"}},

							{ 0x4C, new C{ T="TagRef"}},
							{ 0x6C, new C{ T="Tagblock"}},
							{ 0x80, new C{ T="Tagblock"}},

							{ 0x94, new C{ T="TagRef"}},
							{ 0xB4, new C{ T="Tagblock"}},
							{ 0xC8, new C{ T="Tagblock"}},

							{ 0xDC, new C{ T="TagRef"}},
							{ 0xFC, new C{ T="Tagblock"}},
							{ 0x110, new C{ T="Tagblock"}},

							{ 0x124, new C{ T="TagRef"}},
							{ 0x144, new C{ T="Tagblock"}},
							{ 0x158, new C{ T="Tagblock"}},

							{ 0x16C, new C{ T="TagRef"}},
							{ 0x188, new C{ T="TagRef"}},
							{ 0x1A4, new C{ T="TagRef"}},
							{ 0x1C0, new C{ T="TagRef"}},
							{ 0x1DC, new C{ T="TagRef"}},
							{ 0x1F8, new C{ T="TagRef"}},
							{ 0x214, new C{ T="TagRef"}},
							{ 0x230, new C{ T="TagRef"}},
							{ 0x24C, new C{ T="TagRef"}},
							{ 0x268, new C{ T="TagRef"}},
							{ 0x284, new C{ T="TagRef"}},
							{ 0x2A0, new C{ T="TagRef"}},
							{ 0x2BC, new C{ T="TagRef"}},
							{ 0x2D8, new C{ T="TagRef"}},
							{ 0x2F4, new C{ T="TagRef"}}
						},S=784} },
						{ 0x2E8, new C{ T="Tagblock"}},
						{ 0x320, new C{ T="Tagblock"}},
					},S = 848}},

					{ 0xCBC, new C { T = "TagRef" } },
					{ 0xCD8, new C { T = "TagRef" } },
					{ 0xCF4, new C { T = "TagRef" } },
					{ 0xD10, new C { T = "TagRef" } },

					{ 0xDB4, new C { T = "Tagblock" } },
					{ 0xDC8, new C { T = "TagRef" } },
					{ 0xDEC, new C { T = "Tagblock" } },

					{ 0xE00, new C { T = "TagRef" } },
					{ 0xE1C, new C { T = "TagRef" } },
					{ 0xE38, new C { T = "TagRef" } },
					{ 0xE54, new C { T = "TagRef" } },
					{ 0xE70, new C { T = "TagRef" } },
					{ 0xE8C, new C { T = "TagRef" } },
					{ 0xEA8, new C { T = "TagRef" } },
					{ 0xEC4, new C { T = "TagRef" } },

					{ 0xF80, new C { T = "Tagblock" } },
					{ 0xFBC, new C { T = "Tagblock" } },

					{ 0x1074, new C { T = "TagRef" } },
					{ 0x109C, new C { T = "TagRef" } },
					{ 0x10BC, new C { T = "Tagblock" } },
					{ 0x10D0, new C { T = "TagRef" } },
					{ 0x10EC, new C { T = "TagRef" } },
					{ 0x1108, new C { T = "TagRef" } },

					{ 0x1128, new C { T = "Tagblock" } },
					{ 0x113C, new C { T = "Tagblock" } }
				}
			}
			,
			{
				"hlmt",
				new() {
					{ 0x10, new C { T = "TagRef", N = "Model" } }, // mode
					{ 0x2C, new C { T = "TagRef", N = "Collision model" } }, // COLL
					{ 0x48, new C { T = "TagRef", N = "Animations" } }, // JMAD
					{ 0x64, new C { T = "TagRef", N = "Physics model" } }, // PHMO

					{ 0xAC, new C { T = "Tagblock" } },
					{0xF4,new C{T = "Tagblock",B = new Dictionary<long, C> // object variant
					{
						{ 0x0, new C { T = "mmr3Hash", N = "Variant Hash" } }, // variant
						{ 0x34, new C{ T="Tagblock"}},
						{ 0x48, new C{ T="Tagblock", B=new Dictionary<long, C> // object block
						{
							{ 12, new C{ T="TagRef", N = "Attached Object"}}, //
						    { 40, new C{ T="TagRef"}}, //
						}, S=72}},
						{ 0x5C, new C{ T="Tagblock"}},
						{ 0x70, new C{ T="Tagblock"}},
						{ 0x104, new C{ T="TagRef"}},
						{ 0x120, new C{ T="TagRef"}},
						{ 0x13C, new C{ T="TagRef"}},
						{ 0x158, new C{ T="TagRef"}},
						{ 0x174, new C{ T="Tagblock"}},
					},S = 392}},

					{ 0x108, new C { T = "Tagblock" } },
					{ 0x11C, new C { T = "Tagblock" } },
					{ 0x130, new C { T = "Tagblock" } },
					{ 0x150, new C { T = "TagRef" } },

					{ 0x16C, new C { T = "Tagblock" } },
					{ 0x180, new C { T = "Tagblock" } },
					{ 0x194, new C { T = "Tagblock" } },

					{ 0x1E8, new C { T = "TagRef" } },

					{ 0x204, new C { T = "Tagblock" } },
					{ 0x218, new C { T = "Tagblock" } },
					{ 0x22C, new C { T = "Tagblock" } },
					{ 0x240, new C { T = "Tagblock" } },
					{ 0x258, new C { T = "Tagblock" } },
					{ 0x26C, new C { T = "Tagblock" } },

					{ 0x280, new C { T = "TagRef" } },
					{ 0x29C, new C { T = "TagRef" } },

					{ 0x2B8, new C { T = "Tagblock" } },
					{ 0x2CC, new C { T = "Tagblock" } },
					{ 0x2E0, new C { T = "Tagblock" } },
				}
			}
			,
			{
				"proj",
				new()
				{
					{ 40, new C { T = "Float" } },
					{ 44, new C { T = "Float" } },
					{ 48, new C { T = "Float" } },
					{ 52, new C { T = "Float" } },
					{ 56, new C { T = "Float" } },
					{ 60, new C { T = "Float" } },
					{ 64, new C { T = "Float" } },
					{ 76, new C { T = "Float" } },
					{ 80, new C { T = "Float" } },
					{ 84, new C { T = "Float" } },

					{ 92, new C { T = "Tagblock" } }, // SidecarPathDefinition

					{ 112, new C { T = "Float" } },
					{ 116, new C { T = "Float" } },

					{ 120, new C { T = "TagRef" } }, // vehicle model
					{ 148, new C { T = "TagRef" } }, // aset tag ref
					{ 176, new C { T = "TagRef" } },
					{ 204, new C { T = "TagRef" } },

					{ 232, new C { T = "4Byte" } },

					{ 240, new C { T = "TagRef" } },

					{ 276, new C { T = "Float" } },

					{ 288, new C { T = "TagRef" } },
					{ 316, new C { T = "TagRef" } }, // foot tag ref
					{ 344, new C { T = "TagRef" } }, // vemd tag ref
					{ 372, new C { T = "TagRef" } }, // smed tag ref
					{ 400, new C { T = "TagRef" } },

					{ 432, new C { T = "Float" } },

					{ 448, new C { T = "Tagblock" } }, // object_ai_properties
					{ 468, new C { T = "Tagblock" } }, // s_object_function_definition
					{ 488, new C { T = "4Byte" } },
					{ 492, new C { T = "Tagblock" } }, // ObjectRuntimeInterpolatorFunctionsBlock
					{ 512, new C { T = "Tagblock" } }, // ObjectFunctionSwitchDefinition
					{ 532, new C { T = "Tagblock" } }, // i343::Objects::ObjectFunctionForwarding
					{ 552, new C { T = "4Byte" } },
					{ 556, new C { T = "Tagblock" } }, // i343::Objects::AmmoRefillVariant
					{ 576, new C { T = "4Byte" } },
					{
						0x248,
						new C
						{
							T = "Tagblock",
							B = new Dictionary<long, C> // object_attachment_definition
							{
								{ 4, new C{ T="TagRef"}}, // effe
							    { 32, new C{ T="TagRef"}}, // effe
							    { 64, new C{ T="Tagblock"}},
								{ 84, new C{ T="TagRef"}}, //
							    { 112, new C{ T="Tagblock"}}
							},
							S = 148
						}
					},
					{ 604, new C { T = "Tagblock" } }, // object_indirect_lighting_settings_definition
					{ 624, new C { T = "Tagblock" } }, // s_water_physics_hull_surface_definition
					{ 644, new C { T = "Tagblock" } }, // s_jetwash_definition
					{ 664, new C { T = "Tagblock" } }, // object_definition_widget
					{ 684, new C { T = "Tagblock" } }, // object_change_color_definition
					{ 704, new C { T = "Tagblock" } }, // s_multiplayer_object_properties_definition
					{ 724, new C { T = "Tagblock" } }, // i343::Objects::ForgeObjectEntryDefinition

					{ 744, new C { T = "TagRef" } },
					{ 772, new C { T = "TagRef" } },

					{ 800, new C { T = "Tagblock" } }, // s_object_spawn_effects
					{ 820, new C { T = "Tagblock" } }, // ModelDissolveDataBlock

					{ 0x448, new C { T = "Tagblock" } },
					{ 0x45C, new C { T = "TagRef" } },
					{ 0x484, new C { T = "Tagblock" } },
					{ 0x4C8, new C { T = "Tagblock" } },
					{ 0x4E4, new C { T = "TagRef" } },
					{ 0x500, new C { T = "Tagblock" } },
					{ 0x514, new C { T = "TagRef" } },
					{ 0x530, new C { T = "Tagblock" } },
					{ 0x548, new C { T = "Tagblock" } },
					{ 0x55C, new C { T = "TagRef" } },

					{ 0x578, new C { T = "Tagblock" } },
					{ 0x58C, new C { T = "Tagblock" } },

					{ 0x5A0, new C { T = "Float" } },
					{ 0x5A4, new C { T = "Float" } },
					{ 0x5A8, new C { T = "Float" } },
					{ 0x5AC, new C { T = "Float" } },
					{ 0x5B0, new C { T = "Float" } },
					{ 0x5B4, new C { T = "Float" } },
					{ 0x5B8, new C { T = "Float" } },
					{ 0x5BC, new C { T = "Float" } },
					{ 0x5C0, new C { T = "Float" } },
					{ 0x5E0, new C { T = "Float" } },

					{ 0x5E8, new C { T = "Tagblock" } },
					{ 0x60C, new C { T = "Tagblock" } },
					{ 0x620, new C { T = "Tagblock" } },
					{ 0x638, new C { T = "Tagblock" } },
					{ 0x64C, new C { T = "Float" } },
					{ 0x650, new C { T = "Tagblock" } },
					{ 0x664, new C { T = "Tagblock" } },
					{ 0x678, new C { T = "Tagblock" } },
					{ 0x68C, new C { T = "Tagblock" } }, // hm, that address was off by +0x4
					{ 0x6A0, new C { T = "Tagblock" } },

					{ 0x6B8, new C { T = "TagRef" } }, //PROJ

					{ 0x6D4, new C { T = "Float" } },
					{ 0x6F8, new C { T = "Float" } },
					{ 0x704, new C { T = "Float" } },
					{ 0x708, new C { T = "Float" } },
					{ 0x710, new C { T = "Float" } },
					{ 0x720, new C { T = "Float" } },
					{ 0x72C, new C { T = "Float" } },

					{ 0x738, new C { T = "TagRef" } }, // EFFE
					{ 0x754, new C { T = "TagRef" } }, // EEFFE
					{ 0x770, new C { T = "TagRef" } },
					{ 0x78C, new C { T = "TagRef" } }, // JPT
					{ 0x7A8, new C { T = "TagRef" } }, // JPT
					{ 0x7C4, new C { T = "TagRef" } },
					{ 0x7E0, new C { T = "TagRef" } },
					{ 0x800, new C { T = "TagRef" } },
					{ 0x81C, new C { T = "TagRef" } },

					{ 0x83C, new C { T = "Tagblock" } },

					{ 0x850, new C { T = "TagRef" } },
					{ 0x86C, new C { T = "TagRef" } },
					{ 0x888, new C { T = "TagRef" } }, // SND
					{ 0x8A4, new C { T = "TagRef" } },
					{ 0x8C0, new C { T = "TagRef" } }, // SND
					{ 0x8DC, new C { T = "TagRef" } },

					{ 0x8FC, new C { T = "Float" } },

					{ 0x908, new C { T = "TagRef" } },
					{ 0x924, new C { T = "TagRef" } },
					{ 0x944, new C { T = "TagRef" } },
					{ 0x984, new C { T = "TagRef" } },
					{ 0x9A0, new C { T = "TagRef" } }, // JPT
					{ 0x9BC, new C { T = "TagRef" } }, // JPT

					{ 0x9DC, new C { T = "Tagblock" } },

					{ 0x9F0, new C { T = "Float" } },

					{ 0x9F4, new C { T = "TagRef" } }, //JPT
					{ 0xA10, new C { T = "TagRef" } }, // JPT

					{ 0xA2C, new C { T = "Float", N = "Weight" } },
					{ 0xA34, new C { T = "Float" } },
					{ 0xA38, new C { T = "Float" } },
					{ 0xA40, new C { T = "Float" } },
					{ 0xA44, new C { T = "Float", N = "Velocity" } },
					{ 0xA80, new C { T = "Float" } },

					{ 0xAA0, new C { T = "TagRef" } },

					{ 0xABC, new C { T = "Tagblock" } },
					{ 0xAD0, new C { T = "Tagblock" } },
					{ 0xAE4, new C { T = "Tagblock" } },
					{ 0xAF8, new C { T = "Tagblock" } },
					{ 0xB0C, new C { T = "Tagblock" } },

					{ 0xB20, new C { T = "TagRef" } },

					{ 0xB3C, new C { T = "Float" } },
					{ 0xB40, new C { T = "Float" } },
					{ 0xB44, new C { T = "Float" } },

					{ 0xB48, new C { T = "Tagblock" } },
					{ 0xB60, new C { T = "Tagblock" } },

					{ 0xB8C, new C { T = "Float" } },
					{ 0xB90, new C { T = "Float" } },

					{ 0xBA0, new C { T = "TagRef" } }, // JPT
					{ 0xBBC, new C { T = "TagRef" } },
					{ 0xBDC, new C { T = "TagRef" } }, // EFFE
					{ 0xBF5, new C { T = "TagRef" } }, // EFFE
				}
			}
			,
			{
				"sddt",
				new()
				{
					{ 0x40, new C { T = "4Byte" } },

					{ 0x44, new C { T = "Tagblock" } },
					{ 0x58, new C { T = "Tagblock" } },
					{ 0x6C, new C { T = "Tagblock" } },
					{ 0x80, new C { T = "Tagblock" } },
					{ 0x94, new C { T = "Tagblock" } },
					{ 0xA8, new C { T = "Tagblock" } },

					{ 0xC0, new C { T = "4Byte" } },

					{ 0xC4, new C { T = "Tagblock" } },
					{ 0xD8, new C { T = "Tagblock" } },
					{ 0xEC, new C { T = "Tagblock" } },
					{ 0x100, new C { T = "Tagblock" } },
					{ 0x114, new C { T = "Tagblock" } },
					{ 0x128, new C { T = "Tagblock" } },
					{ 0x13C, new C { T = "Tagblock" } },
					{ 0x158, new C { T = "Tagblock" } },
					{ 0x16C, new C { T = "Tagblock" } },
					{ 0x180, new C { T = "Tagblock" } },
					{ 0x194, new C { T = "Tagblock" } },
					{ 0x1A8, new C { T = "Tagblock" } },
					{ 0x1BC, new C { T = "Tagblock" } },
					{ 0x1D0, new C { T = "Tagblock" } },
					{ 0x1F0, new C { T = "Tagblock" } },
					{ 0x204, new C { T = "Tagblock" } },
				}
			}
			,
			{
				"levl",
				new()
				{
					{ 0x10, new C { T = "Tagblock" } },
					{ 0x24, new C { T = "Tagblock" } },
					{ 0x38, new C { T = "Tagblock" } },
					{ 0x4C, new C { T = "Tagblock" } },

					{ 0x5C, new C { T = "4Byte" } },

					{ 0x60, new C { T = "Tagblock" } },
					{ 0x74, new C { T = "Tagblock" } },
					{ 0x88, new C { T = "Tagblock" } },
					{ 0x9C, new C { T = "Tagblock" } },
					{ 0xB0, new C { T = "Tagblock" } },
					{ 0xC4, new C { T = "Tagblock" } },
					{ 0xD8, new C { T = "Tagblock" } },
					{ 0xEC, new C { T = "Tagblock" } },
					{ 0x100, new C { T = "Tagblock" } },
					{ 0x114, new C { T = "Tagblock" } },
					{ 0x128, new C { T = "Tagblock" } },
					{ 0x13C, new C { T = "Tagblock" } },
					{ 0x150, new C { T = "Tagblock" } },
					{ 0x164, new C { T = "Tagblock" } },
					{ 0x178, new C { T = "Tagblock" } },
					{ 0x18C, new C { T = "Tagblock" } },
					{ 0x1A0, new C { T = "Tagblock" } },
					{ 0x1B4, new C { T = "Tagblock" } },
					{ 0x1C8, new C { T = "Tagblock" } },
					{ 0x1DC, new C { T = "Tagblock" } },
					{ 0x1F0, new C { T = "Tagblock" } },
					{ 0x204, new C { T = "Tagblock" } },
					{ 0x218, new C { T = "Tagblock" } },
					{ 0x22C, new C { T = "Tagblock" } },
					{ 0x240, new C { T = "Tagblock" } },
					{ 0x254, new C { T = "Tagblock" } },
					{ 0x268, new C { T = "Tagblock" } },
					{ 0x27C, new C { T = "Tagblock" } },
					{ 0x290, new C { T = "Tagblock" } },
					{ 0x2A4, new C { T = "Tagblock" } },
					{ 0x2B8, new C { T = "Tagblock" } },
					{ 0x2CC, new C { T = "Tagblock" } },
					{ 0x2E0, new C { T = "Tagblock" } },
					{ 0x2F4, new C { T = "Tagblock" } },
					{ 0x308, new C { T = "Tagblock" } },
					{ 0x31C, new C { T = "Tagblock" } },
					{ 0x330, new C { T = "Tagblock" } },
					{ 0x344, new C { T = "Tagblock" } },
					{ 0x358, new C { T = "Tagblock" } },
					{ 0x36C, new C { T = "Tagblock" } },
					{ 0x380, new C { T = "Tagblock" } },
					{ 0x394, new C { T = "Tagblock" } },
					{ 0x3A8, new C { T = "Tagblock" } },
					{ 0x3BC, new C { T = "Tagblock" } },
					{ 0x3D0, new C { T = "Tagblock" } },
					{ 0x3E4, new C { T = "Tagblock" } },
					{ 0x3F8, new C { T = "Tagblock" } },
					{ 0x40C, new C { T = "Tagblock" } },
					{ 0x420, new C { T = "Tagblock" } },
					{ 0x434, new C { T = "Tagblock" } },
					{ 0x448, new C { T = "Tagblock" } },
					{ 0x45C, new C { T = "Tagblock" } },
					{ 0x470, new C { T = "Tagblock" } },
					{ 0x484, new C { T = "Tagblock" } },
					{ 0x498, new C { T = "Tagblock" } },
					{ 0x4AC, new C { T = "Tagblock" } },
					{ 0x4C0, new C { T = "Tagblock" } },
					{ 0x4D4, new C { T = "Tagblock" } },
					{ 0x4E8, new C { T = "Tagblock" } },
					{ 0x4FC, new C { T = "Tagblock" } },
					{ 0x510, new C { T = "Tagblock" } },
					{ 0x524, new C { T = "Tagblock" } },
					{ 0x54C, new C { T = "Tagblock" } },
					{ 0x560, new C { T = "Tagblock" } },
					{ 0x574, new C { T = "Tagblock" } },
					{ 0x588, new C { T = "Tagblock" } },
					{ 0x59C, new C { T = "Tagblock" } },
					{ 0x5B0, new C { T = "Tagblock" } },
					{ 0x5C4, new C { T = "Tagblock" } },
					{ 0x5D8, new C { T = "Tagblock" } },
					{ 0x5EC, new C { T = "Tagblock" } },
					{ 0x600, new C { T = "Tagblock" } },
					{ 0x614, new C { T = "Tagblock" } },
					{ 0x628, new C { T = "Tagblock" } },
					{ 0x63C, new C { T = "Tagblock" } },
					{ 0x650, new C { T = "Tagblock" } },
					{ 0x664, new C { T = "Tagblock" } },
					{ 0x678, new C { T = "Tagblock" } },
					{ 0x68C, new C { T = "Tagblock" } },
					{ 0x6A0, new C { T = "Tagblock" } },
					{ 0x6B4, new C { T = "Tagblock" } },
					{ 0x6C8, new C { T = "Tagblock" } },
					{ 0x6DC, new C { T = "Tagblock" } },
					{ 0x6F0, new C { T = "Tagblock" } },
					{ 0x71C, new C { T = "Tagblock" } },

					{ 0x730, new C { T = "TagRef" } },
					{ 0x74C, new C { T = "TagRef" } },

					{ 0x788, new C { T = "Tagblock" } },

					{ 0x79C, new C { T = "TagRef" } },
					{ 0x7B8, new C { T = "TagRef" } },
					{ 0x7D4, new C { T = "TagRef" } },
					{ 0x7F0, new C { T = "TagRef" } },

					{ 0x80C, new C { T = "Tagblock" } },
					{ 0x820, new C { T = "Tagblock" } },

					{ 0x834, new C { T = "TagRef" } },

					{ 0x850, new C { T = "Tagblock" } },
					{ 0x864, new C { T = "Tagblock" } },
					{ 0x878, new C { T = "Tagblock" } },
					{ 0x88C, new C { T = "Tagblock" } },
					{ 0x8A0, new C { T = "Tagblock" } },
					{ 0x8B4, new C { T = "Tagblock" } },

					{ 0x8C8, new C { T = "TagRef" } },
					{ 0x8E4, new C { T = "TagRef" } },
					{ 0x900, new C { T = "TagRef" } },
					{ 0x91C, new C { T = "TagRef" } },
					{ 0x938, new C { T = "TagRef" } },

					{ 0x954, new C { T = "Tagblock" } },
					{ 0x968, new C { T = "Tagblock" } },
					{ 0x97C, new C { T = "Tagblock" } },
					{ 0x990, new C { T = "Tagblock" } },
					{ 0x9A4, new C { T = "Tagblock" } },
					{ 0x9B8, new C { T = "Tagblock" } },

					{ 0x9CC, new C { T = "TagRef" } },
					{ 0x9E8, new C { T = "TagRef" } },
					{ 0xA04, new C { T = "TagRef" } },
					{ 0xA20, new C { T = "TagRef" } },
					{ 0xA3C, new C { T = "TagRef" } },
					{ 0xA58, new C { T = "TagRef" } },
					{ 0xA74, new C { T = "TagRef" } },
					{ 0xA90, new C { T = "TagRef" } },
					{ 0xAAC, new C { T = "TagRef" } },

					{ 0xAE8, new C { T = "Tagblock" } },
					{ 0xAFC, new C { T = "Tagblock" } },
					{ 0xB10, new C { T = "Tagblock" } },
					{ 0xB24, new C { T = "Tagblock" } },

					{ 0xB38, new C { T = "TagRef" } },
					{ 0xB54, new C { T = "Tagblock" } },
					{ 0xB68, new C { T = "Tagblock" } },
					{ 0xB7C, new C { T = "TagRef" } },
					{ 0xB98, new C { T = "TagRef" } },

					{ 0xBB8, new C { T = "Tagblock" } },

					{ 0xBD4, new C { T = "TagRef" } },
					{ 0xBF0, new C { T = "TagRef" } },
					{ 0xC0C, new C { T = "TagRef" } },
					{ 0xC48, new C { T = "TagRef" } },
					{ 0xC64, new C { T = "TagRef" } },

					{ 0xC9C, new C { T = "Float", N="Bounds X+" } },
					{ 0xCA0, new C { T = "Float", N="Bounds Y+"  } },
					{ 0xCA4, new C { T = "Float", N="Bounds Z+"  } },
					{ 0xCA8, new C { T = "Float", N="Bounds X-"  } },
					{ 0xCAC, new C { T = "Float", N="Bounds Y-"  } },
					{ 0xCB0, new C { T = "Float", N="Bounds Z-"  } },


					{ 0xC84, new C { T = "Tagblock" } },
					{ 0xCB4, new C { T = "Tagblock" } },
					{ 0xCC8, new C { T = "Tagblock" } },
					{ 0xCDC, new C { T = "Tagblock" } }
				}
			}
			,
			{
				"effe",
				new()
				{

					{ 0x10, new C{ T="Tagblock"}},
					{ 0x5C, new C{ T="Tagblock", B = new Dictionary<long, C> // 
						{
							{ 0x2A, new C{ T="TagRef"}},

							{ 0x50, new C{ T="Tagblock"}},
							{ 0x64, new C{ T="Tagblock", B=new Dictionary<long, C> // 
						    {
								{ 0x14, new C { T = "Float" } },
								{ 0x18, new C { T = "Float" } },

								{ 0x20, new C{ T="Tagblock", B=new Dictionary<long, C> // 
						        {
									{ -8, new C{ T="TagRef", N = "Tag Group"}}, // we should probably create a special class for this but whatev
						            { 0x30, new C{ T="TagRef", N = "Spawned Tag"}},

								}, S=140}},
								{ 0x34, new C{ T="Tagblock"}},
								{ 0x48, new C{ T="Tagblock"}},

							}, S=92}},

							{ 0x78, new C{ T="Tagblock"}},
						}, S=140}
					},
				}
			
			}
			,
			{
				"matg",
				new() 
				{
					{ 0x10, new C { T = "TagRef" } },
					{ 0x2C, new C { T = "TagRef" } },
					{ 0x48, new C { T = "TagRef" } },
					{ 0x64, new C { T = "TagRef" } },
					{ 0x80, new C { T = "TagRef" } },
					{ 0x9C, new C { T = "TagRef" } },
					{ 0xB8, new C { T = "TagRef" } },
					{ 0xD4, new C { T = "TagRef" } },
					{ 0xF0, new C { T = "TagRef" } },
					{ 0x10C, new C { T = "TagRef" } },
					{ 0x128, new C { T = "TagRef" } },
					{ 0x144, new C { T = "TagRef" } },
					{ 0x160, new C { T = "TagRef" } },
					{ 0x17C, new C { T = "TagRef" } },
					{ 0x198, new C { T = "TagRef" } },
					{ 0x1B4, new C { T = "TagRef" } },
					{ 0x1D0, new C { T = "TagRef" } },
					{ 0x1EC, new C { T = "TagRef" } },
					{ 0x208, new C { T = "TagRef" } },
					{ 0x224, new C { T = "TagRef" } },
					{ 0x240, new C { T = "TagRef" } },
					{ 0x25C, new C { T = "TagRef" } },
					{ 0x278, new C { T = "TagRef" } },
					{ 0x294, new C { T = "TagRef" } },
					{ 0x2B0, new C { T = "TagRef" } },
					{ 0x2CC, new C { T = "TagRef" } },
					{ 0x2E8, new C { T = "TagRef" } },
					{ 0x304, new C { T = "TagRef" } },
					{ 0x320, new C { T = "TagRef" } },
					{ 0x33C, new C { T = "TagRef" } },
					{ 0x358, new C { T = "TagRef" } },
					{ 0x374, new C { T = "TagRef" } },
					{ 0x390, new C { T = "TagRef" } },
					{ 0x3AC, new C { T = "TagRef" } },
					{ 0x3C8, new C { T = "TagRef" } },
					{ 0x3E4, new C { T = "TagRef" } },
					{ 0x400, new C { T = "TagRef" } },
					{ 0x41C, new C { T = "TagRef" } },
					{ 0x438, new C { T = "TagRef" } },
					{ 0x454, new C { T = "TagRef" } },
					{ 0x470, new C { T = "TagRef" } },
					{ 0x48C, new C { T = "TagRef" } },
					{ 0x4A8, new C { T = "TagRef" } },
					{ 0x4C4, new C { T = "TagRef" } },
					{ 0x4E0, new C { T = "TagRef" } },
					{ 0x4FC, new C { T = "TagRef" } },
					{ 0x518, new C { T = "TagRef" } },
					{ 0x534, new C { T = "TagRef" } },
					{ 0x550, new C { T = "TagRef" } },
					{ 0x56C, new C { T = "TagRef" } },
					{ 0x588, new C { T = "TagRef" } },
					{ 0x5A4, new C { T = "TagRef" } },
					{ 0x5C0, new C { T = "TagRef" } },
					{ 0x5DC, new C { T = "TagRef" } },
					{ 0x5F8, new C { T = "TagRef" } },
					{ 0x614, new C { T = "TagRef" } },
					{ 0x630, new C { T = "TagRef" } },
					{ 0x64C, new C { T = "TagRef" } },
					{ 0x668, new C { T = "TagRef" } },
					{ 0x684, new C { T = "TagRef" } },
					{ 0x6A0, new C { T = "TagRef" } },
					{ 0x6BC, new C { T = "TagRef" } },
					{ 0x6D8, new C { T = "TagRef" } },
					{ 0x6F4, new C { T = "TagRef" } },
					{ 0x710, new C { T = "Tagblock" } },
					{ 0x724, new C { T = "TagRef" } },
					{ 0x740, new C { T = "TagRef" } },
					{ 0x75C, new C { T = "TagRef" } },
					{ 0x778, new C { T = "TagRef" } },
					{ 0x794, new C { T = "TagRef" } },
					{ 0x7B0, new C { T = "TagRef" } },
					{ 0x7CC, new C { T = "TagRef" } },
				}
			}
			,
			{
				"pmcg",
				new()
				{
					{ 0x10, new C{ T="TagRef"}},
					{ 0x2C, new C{ T="TagRef"}},

					{ 0x048, new C{ T="Tagblock"}},
					{ 0x05C, new C{ T="Tagblock"}},
					{ 0x070, new C{ T="Tagblock"}},
					{ 0x084, new C{ T="Tagblock"}},
					{ 0x098, new C{ T="Tagblock"}},
					{ 0x0AC, new C{ T="Tagblock"}},
					{ 0x0C0, new C{ T="Tagblock"}},
					{ 0x0D4, new C{ T="Tagblock"}},
					{ 0x0E8, new C{ T="Tagblock"}},
					{ 0x0FC, new C{ T="Tagblock"}},
					{ 0x110, new C{ T="Tagblock"}},
					{ 0x124, new C{ T="Tagblock"}},

					{ 0x138, new C{ T="Tagblock", B=new Dictionary<long, C> // 
						{
							{ 0x14, new C{ T="TagRef"}},

							{ 0x30, new C{ T="String"}},
							{ 0x130, new C{ T="Tagblock", B=new Dictionary<long, C> // 
						    {
								{ 0x0, new C{ T="4Byte"}},
								{ 0x4, new C{ T="4Byte"}},
								{ 0x8, new C{ T="4Byte"}},

								{ 0x0C, new C{ T="String"}},

								{ 0x10C, new C{ T="Tagblock", B=new Dictionary<long, C> // 
						        {
									{ 0x0, new C{ T="4Byte"}},
									{ 0x4, new C{ T="4Byte"}},

								}, S=8}},
							}, S=288}},
						}, S=324}},

					{ 0x14C, new C{ T="Tagblock"}},
				}
			}
			,
			{
				"glpa",
				new()
				{
					{ 0x014, new C{ T="Tagblock", B=new Dictionary<long, C> // object_attachment_definition
					{
						{ 0x0C, new C{ T="TagRef"}},
						{ 0x2C, new C{ T="TagRef"}},
						{ 0x4C, new C{ T="TagRef"}},
						{ 0x6C, new C{ T="TagRef"}},
						{ 0x8C, new C{ T="TagRef"}},
						{ 0xA8, new C{ T="TagRef"}},
						{ 0xCC, new C{ T="Tagblock", B=new Dictionary<long, C>
						{
							{ 0x0, new C{ T="4Byte"}}, // datnum
						    { 0x4, new C{ T="4Byte"}}, // datnum
						}, S=8}},
						{ 0x0E4, new C{ T="TagRef"}},
						{ 0x100, new C{ T="TagRef"}},
						{ 0x11C, new C{ T="TagRef"}},
						{ 0x138, new C{ T="TagRef"}},
						{ 0x154, new C{ T="TagRef"}},
						{ 0x170, new C{ T="TagRef"}},
						{ 0x18C, new C{ T="TagRef"}},
						{ 0x1A8, new C{ T="TagRef"}},
						{ 0x1C4, new C{ T="TagRef"}},
						{ 0x1E0, new C{ T="TagRef"}},
						{ 0x1FC, new C{ T="TagRef"}},
					}, S=536}},
					{ 0x028, new C{ T="Tagblock"}},
					{ 0x03C, new C{ T="Tagblock"}},
					{ 0x050, new C{ T="Tagblock"}},
					{ 0x064, new C{ T="Tagblock"}},
				}
			},

			{"foot",new()
			{
				{ 0x10, new C{ T="Tagblock", B=new Dictionary<long, C> // 
				{
					{ 0x0, new C{ T="Tagblock", B=new Dictionary<long, C> // 
					{
						{ 0x00, new C{ T="TagRef"}},
						{ 0x1C, new C{ T="TagRef"}}
					}, S=72 } },
					{ 0x14, new C{ T="Tagblock", B=new Dictionary<long, C> // 
					{
						{ 0x00, new C{ T="TagRef"}},
						{ 0x1C, new C{ T="TagRef"}}
					}, S=72 } }
				}, S=40 } }
			}},

			{
				"ocgd", // ocgd* no wonder why i couldn't find it 
				new()  
				{
					{ 0x10, new C{ T="Tagblock", B= new Dictionary<long, C>
						{
							{ 0x4, new C{ T="TagRef"}},
							{ 0x20, new C{ T="TagRef"}},
							{ 0x3C, new C{ T="Tagblock", B= new Dictionary<long, C>
							{
								{ 0x0, new C{ T="4Byte"} },
								{ 0x4, new C{ T="4Byte"} },
								{ 0x8, new C{ T="TagRef"}}

							}, S=36 }},
						}, S=80 }},

					{ 0x24, new C{ T="Tagblock", B= new Dictionary<long, C>
						{
							{ 0x0, new C{ T="Tagblock", B= new Dictionary<long, C>
								{ { 0x0, new C{ T="4Byte"} } }, S=4 }},
							{ 0x14, new C{ T="Tagblock", B= new Dictionary<long, C>
								{ { 0x0, new C{ T="4Byte"} } }, S=4 }},
							{ 0x28, new C{ T="Tagblock", B= new Dictionary<long, C>
								{ { 0x0, new C{ T="4Byte"} } }, S=4 }},
						}, S=60 }
					},

					{ 0x38, new C{ T="Tagblock", B= new Dictionary<long, C>
						{ { 0x0, new C{ T="TagRef"} } }, S=28 }},

					{ 0x4C, new C{ T="Tagblock", B= new Dictionary<long, C>
						{ { 0x0, new C{ T="TagRef"} } }, S=28 }},

					{ 0x60, new C{ T="Tagblock", B= new Dictionary<long, C>
						{
							{ 0x0, new C{ T="4Byte"} },
							{ 0x4, new C{ T="TagRef"} },
							{ 0x20, new C{ T="TagRef"} },
							{ 0x3C, new C{ T="Tagblock", B= new Dictionary<long, C>
							{
								{ 0x0, new C{ T="4Byte"} },
								{ 0x4, new C{ T="4Byte"} },
								{ 0x8, new C{ T="TagRef"} },
								{ 0x24, new C{ T="TagRef"} },

							}, S=64 }},
						}, S=80 }},
					{ 0x74, new C{ T="Tagblock"}},
					{ 0x88, new C{ T="Tagblock"}},
					{ 0x9C, new C{ T="Tagblock"}},
					{ 0xB0, new C{ T="Tagblock"}},
					{ 0xC4, new C{ T="Tagblock"}},
					{ 0xD8, new C{ T="Tagblock"}},
					{ 0xEC, new C{ T="Tagblock"}},
					{ 0x100, new C{ T="Tagblock"}},

					{ 0x114, new C{ T="TagRef"}},
					{ 0x130, new C{ T="TagRef"}},


					{ 0x14C, new C{ T="Tagblock"}},
					{ 0x160, new C{ T="Tagblock"}},
					{ 0x174, new C{ T="Tagblock"}},
				}
			},

			{"mode",new()
			{

				{ 0x28, new C{ T="Tagblock"}},
				{ 0x40, new C{ T="Tagblock"}},
				{ 0x54, new C{ T="Tagblock"}},
				{ 0x68, new C{ T="Tagblock"}},
				{ 0x7C, new C{ T="Tagblock"}},
				{ 0x90, new C{ T="Tagblock"}},

				{ 0xA8, new C{ T="4Byte"}},

				{ 0xAC, new C{ T="Tagblock"}},
				{ 0xC0, new C{ T="Tagblock"}},
				{ 0xD4, new C{ T="Tagblock"}},
				{ 0xE8, new C{ T="Tagblock"}},
				{ 0xFC, new C{ T="Tagblock"}},
				{ 0x110, new C{ T="Tagblock"}},
				{ 0x124, new C{ T="Tagblock"}},
				{ 0x140, new C{ T="Tagblock"}},
				{ 0x154, new C{ T="Tagblock"}},
				{ 0x168, new C{ T="Tagblock"}},
				{ 0x17C, new C{ T="Tagblock"}},
				{ 0x190, new C{ T="Tagblock"}},
				{ 0x1A4, new C{ T="Tagblock"}},
				{ 0x1B8, new C{ T="Tagblock"}},
				{ 0x1D8, new C{ T="Tagblock"}},
				{ 0x1EC, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x0, new C{ T="Float", N = "Quaternion_1"} },
					{ 0x4, new C{ T="Float", N = "Quaternion_2"} },
					{ 0x8, new C{ T="Float", N = "Quaternion_3"} },
					{ 0xC, new C{ T="Float", N = "Quaternion_4"} },
					{ 0x10, new C{ T="Float", N = "Location_X"} },
					{ 0x14, new C{ T="Float", N = "Location_Y"} },
					{ 0x18, new C{ T="Float", N = "Location_Z"} },
					{ 0x1C, new C{ T="Float", N = "Scale"} },
				}, S=32 }},
				{ 0x200, new C{ T="Tagblock"}},
				{ 0x214, new C{ T="Tagblock"}},
				{ 0x228, new C{ T="Tagblock"}},
				{ 0x23C, new C{ T="TagRef"}},
				{ 0x258, new C{ T="Tagblock"}},
			}},

			{"wgtz",new()
			{
				{ 0x10, new C{ T="Tagblock"}},
				{ 0x24, new C{ T="Tagblock"}},
				{ 0x38, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x0, new C{ T="TagRef"}},
				}, S=28 }},
				{ 0x4C, new C{ T="Tagblock"}},
				{ 0x60, new C{ T="Tagblock"}},
				{ 0x78, new C{ T="Tagblock"}},
				{ 0x8C, new C{ T="Tagblock"}},
				{ 0xA0, new C{ T="Tagblock"}},
				{ 0xB4, new C{ T="Tagblock"}},
				{ 0xC8, new C{ T="Tagblock"}},
				{ 0xDC, new C{ T="Tagblock"}},
				{ 0xF0, new C{ T="Tagblock"}},
				{ 0x104, new C{ T="Tagblock"}},
				{ 0x118, new C{ T="Tagblock"}},
				{ 0x12C, new C{ T="Tagblock"}},
				{ 0x140, new C{ T="Tagblock"}},
				{ 0x154, new C{ T="Tagblock"}},
				{ 0x168, new C{ T="Tagblock"}},
				{ 0x17C, new C{ T="Tagblock"}},

				{ 0x190, new C{ T="TagRef"}},
				{ 0x1A4, new C{ T="TagRef"}},
			}},

			{"char",new()
			{
				{ 0x14, new C{ T="TagRef", N="Parent"}},
				{ 0x30, new C{ T="TagRef", N="Biped"}},
				{ 0x4C, new C{ T="TagRef", N="Creature"}},
				{ 0x68, new C{ T="TagRef", N="Style"}},
				{ 0x84, new C{ T="TagRef", N="Major"}},
				{ 0xA0, new C{ T="TagRef"}},

				{ 0xBC + 0x14*0, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*1, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*2, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*3, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*4, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*5, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*6, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*7, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*8, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*9, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*10, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*11, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*12, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*13, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*14, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*15, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*16, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*17, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*18, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*19, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*20, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*21, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*22, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*23, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*24, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*25, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*26, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*27, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*28, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*29, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*30, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*31, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*32, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*33, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*34, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*35, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*36, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*37, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*38, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*39, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*40, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*41, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*42, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*43, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*44, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*45, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*46, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*47, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*48, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*49, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*50, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*51, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*52, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*53, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*54, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*55, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*56, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*57, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*58, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*59, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*60, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*61, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*62, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*63, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*64, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*65, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*66, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*67, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*68, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*69, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*70, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*71, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*72, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*73, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*74, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*75, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*76, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*77, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*78, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*79, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*80, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*81, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*82, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*83, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*84, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*85, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*86, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*87, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*88, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*89, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*90, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*91, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*92, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*93, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*94, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*95, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*96, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*97, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*98, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*99, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*100, new C{ T="Tagblock"}},
				{ 0xBC + 0x14*101, new C{ T="Tagblock"}},
			}},

			{"bipd",new()
			{
				{ 40, new C { T = "Float" } },
				{ 44, new C { T = "Float" } },
				{ 48, new C { T = "Float" } },
				{ 52, new C { T = "Float" } },
				{ 56, new C { T = "Float" } },
				{ 60, new C { T = "Float" } },
				{ 64, new C { T = "Float" } },
				{ 76, new C { T = "Float" } },
				{ 80, new C { T = "Float" } },
				{ 84, new C { T = "Float" } },

				{ 92, new C { T = "Tagblock" } }, // SidecarPathDefinition

				{ 112, new C { T = "mmr3Hash", N = "Default Variant" } }, // default variant
				{ 116, new C { T = "Float" } },

				{ 120, new C { T = "TagRef", N = "Model" } }, // vehicle model 
				{ 148, new C { T = "TagRef" } }, // aset tag ref
				{ 176, new C { T = "TagRef" } },
				{ 204, new C { T = "TagRef" } },

				{ 232, new C { T = "4Byte" } },

				{ 240, new C { T = "TagRef" } },

				{ 276, new C { T = "Float" } },

				{ 288, new C { T = "TagRef" } },
				{ 316, new C { T = "TagRef" } }, // foot tag ref
				{ 344, new C { T = "TagRef" } }, // vemd tag ref
				{ 372, new C { T = "TagRef" } }, // smed tag ref
				{ 400, new C { T = "TagRef" } },

				{ 432, new C { T = "Float" } },

				{ 448, new C { T = "Tagblock" } }, // object_ai_properties
				{ 468, new C { T = "Tagblock" } }, // s_object_function_definition
				{ 488, new C { T = "4Byte" } },
				{ 492, new C { T = "Tagblock" } }, // ObjectRuntimeInterpolatorFunctionsBlock
				{ 512, new C { T = "Tagblock" } }, // ObjectFunctionSwitchDefinition
				{ 532, new C { T = "Tagblock" } }, // i343::Objects::ObjectFunctionForwarding
				{ 552, new C { T = "4Byte" } },
				{ 556, new C { T = "Tagblock" } }, // i343::Objects::AmmoRefillVariant
				{ 576, new C { T = "4Byte" } },
				{ 0x248,new C{T = "Tagblock",B = new Dictionary<long, C> // object_attachment_definition
				{
					{ 4, new C{ T="TagRef"}}, // effe
					{ 32, new C{ T="TagRef"}}, // effe
					{ 64, new C{ T="Tagblock"}},
					{ 84, new C{ T="TagRef"}}, //
					{ 112, new C{ T="Tagblock"}}
				},S = 148}},

				{ 604, new C { T = "Tagblock" } }, // object_indirect_lighting_settings_definition
				{ 624, new C { T = "Tagblock" } }, // s_water_physics_hull_surface_definition
				{ 644, new C { T = "Tagblock" } }, // s_jetwash_definition
				{ 664, new C { T = "Tagblock" } }, // object_definition_widget
				{ 684, new C { T = "Tagblock" } }, // object_change_color_definition
				{ 704, new C { T = "Tagblock" } }, // s_multiplayer_object_properties_definition
				{ 724, new C { T = "Tagblock" } }, // i343::Objects::ForgeObjectEntryDefinition

				{ 744, new C { T = "TagRef" } },
				{ 772, new C { T = "TagRef" } },

				{ 800, new C { T = "Tagblock" } }, // s_object_spawn_effects
				{ 820, new C { T = "Tagblock" } }, // ModelDissolveDataBlock


				{ 0x348, new C { T = "String" } },
				{ 0x45C, new C { T = "TagRef" } },

				{ 0x484, new C { T = "Tagblock" } },
				{ 0x498, new C { T = "Tagblock" } },
				{ 0x4AC, new C { T = "Tagblock" } },
				{ 0x4C8, new C { T = "Tagblock" } },
				{ 0x4E0, new C { T = "Float" } },

				{ 0x4E4, new C { T = "TagRef" } },
				{ 0x500, new C { T = "Tagblock" } },
				{ 0x514, new C { T = "TagRef" } },

				{ 0x530, new C { T = "Tagblock" } },
				{ 0x548, new C { T = "Tagblock" } },
				{ 0x55C, new C { T = "TagRef" } },
				{ 0x578, new C { T = "Tagblock" } },
				{ 0x58C, new C { T = "Tagblock" } },

				{ 0x5A0, new C { T = "Float" } },
				{ 0x5A4, new C { T = "Float" } },
				{ 0x5A8, new C { T = "Float" } },
				{ 0x5AC, new C { T = "Float" } },
				{ 0x5B0, new C { T = "Float" } },
				{ 0x5B4, new C { T = "Float" } },
				{ 0x5B8, new C { T = "Float" } },
				{ 0x5BC, new C { T = "Float" } },
				{ 0x5C0, new C { T = "Float" } },
				{ 0x5C4, new C { T = "Float" } },
				{ 0x5C8, new C { T = "Float" } },
				{ 0x5CC, new C { T = "Float" } },
				{ 0x5E0, new C { T = "Float" } },

				{ 0x5E8, new C { T = "Tagblock" } },

				{ 0x5FC, new C { T = "Float" } },
				{ 0x600, new C { T = "Float" } },
				{ 0x604, new C { T = "Float" } },

				{ 0x60C, new C { T = "Tagblock" } },
				{ 0x620, new C { T = "Tagblock" } },
				{ 0x638, new C { T = "Tagblock" } },

				{ 0x64C, new C { T = "Float" } },

				{ 0x650, new C { T = "Tagblock" } },
				{ 0x664, new C { T = "Tagblock" } },
				{ 0x678, new C { T = "Tagblock" } },
				{ 0x68C, new C { T = "Tagblock" } },
				{ 0x6A0, new C { T = "Tagblock" } },

				{ 0x6B8, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					{ 0,  "Circular Aiming"  },
					{ 1,  "Destroyed After Dying"  },
					{ 2,  "Half-Speed Interpolation"  },
					{ 3,  "Fires From Camera"  },
					{ 4,  "Entrance Inside Bounding Sphere"  },
					{ 5,  "Doesn't Show Readied Weapon"  },
					{ 6,  "Causes Passenger Dialogue"  },
					{ 7,  "Resists Pings"  },
					{ 8,  "Melee Attack Is Fatal"  },
					{ 9,  "Don't Reface During Pings"  },
					{ 10, "Has No Aiming"  },
					{ 11, "Impact Melee Attaches To Unit"  },
					{ 12, "Impact Melee Dies On Shields"  },
					{ 13, "Cannot Open Doors Automatically"  },
					{ 14, "Melee Attackers Cannot Attach"  },
					{ 15, "Not Instantly Killed By Melee"  },
					{ 16, "Flashlight Power Doesnt Transfer To Weapon"  },
					{ 17, "Runs Around Flaming"  },
					{ 18, "Top Level For AOE Damage"  },
					{ 19, "Special Cinematic Unit"  },
					{ 20, "Ignored By Autoaiming"  },
					{ 21, "Shields Fry Infection Forms"  },
					{ 22, "Use Velocity As Acceleration"  },
					{ 23, "Can Dual Wield"  },
					{ 24, "Acts As Gunner For Parent"  },
					{ 25, "Controlled By Parent Gunner"  },
					{ 26, "Parent's Primary Weapon"  },
					{ 27, "Parent's Secondary Weapon"  },
					{ 28, "Unit Has Boost"  },
					{ 29, "Unit Has Vectored Thrust"  },
					{ 30, "Allow Aim While Opening Or Closing"  },
					{ 31, "Compute Acceleration From Aiming"  }
				} } },
				{ 0x6BC, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					{ 0,  "Override All Pings"  },
					{ 1,  "Unit Supports Bailout"  },
					{ 2,  "Flying Or Vehicle Hard Pings Allowed"  },
					{ 3,  "Attempt To Fire From Weapon Matching Camera"  },
					{ 4,  "Treat As Vehicle"  },
					{ 5,  "Dropped Weapons Can Dissolve"  },
					{ 6,  "Hard Pings Not Allowed For Driver-Less Vehicle"  },
					{ 7,  "No Friendly Bump Damage"  },
					{ 8,  "Ignores Attachment Feedback Forced Hard Pings"  },
					{ 9,  "Unit Appears On Radar As Dot Not Sprite"  },
					{ 10, "Suppress Radar Blip"  },
					{ 11, "Do Not Ignore Parents For Line Of Sight Tests"  },
					{ 12, "Do Not Pass Attached AOE Damage To Children"  },
					{ 13, "Do Not Generate AI Line Of Fire Pill For Unit"  }
				} } },

				{ 0x6C0, new C { T = "2Byte", N="Unit Team Enum" } },

				{ 0x6C4, new C { T = "TagRef", N="Hologram Unit" } },

				{ 0x6E0, new C { T = "Tagblock" } }, // metagame bucket
				{ 0x6F4, new C { T = "Tagblock" } },
				{ 0x720, new C { T = "Tagblock", B = new Dictionary<long, C> // camera tracks
				{
					{ 0x0, new C{ T="TagRef", N="3rdperson cam"}},
					{ 0x1C, new C{ T="TagRef", N = "Screen effect"}},

				},S = 64}},
				{ 0x768, new C { T = "Tagblock" } },
				{ 0x790, new C { T = "Tagblock", B = new Dictionary<long, C> // 
				{
					{ 0x0, new C{ T="TagRef", N="3rdperson cam"}},
					{ 0x1C, new C{ T="TagRef", N = "Screen effect"}},

				},S = 64}},
				{ 0x7D8, new C { T = "Tagblock" } },

				{ 0x7F0, new C { T = "TagRef" } },

				{ 0x80C, new C { T = "Float" } },
				{ 0x810, new C { T = "Float" } },
				{ 0x814, new C { T = "Float" } },
				{ 0x818, new C { T = "Float" } },
				{ 0x820, new C { T = "Float" } },
				{ 0x824, new C { T = "Float" } },
				{ 0x830, new C { T = "Float" } },
				{ 0x834, new C { T = "Float" } },
				{ 0x838, new C { T = "Float" } },
				{ 0x83C, new C { T = "Float" } },
				{ 0x840, new C { T = "Float" } },
				{ 0x854, new C { T = "Float" } },
				{ 0x858, new C { T = "Float" } },
				{ 0x864, new C { T = "Float" } },

				{ 0x870, new C { T = "TagRef" } },

				{ 0x898, new C { T = "Float" } },
				{ 0x8A4, new C { T = "Float" } },

				{ 0x8BC, new C { T = "Tagblock" } },

				{ 0x8D0, new C { T = "TagRef" } },
				{ 0x8EC, new C { T = "TagRef" } },
				{ 0x908, new C { T = "TagRef" } },
				{ 0x924, new C { T = "TagRef" } },
				{ 0x940, new C { T = "TagRef" } },
				{ 0x95C, new C { T = "TagRef" } },
				{ 0x978, new C { T = "TagRef" } },
				{ 0x994, new C { T = "TagRef" } },
				{ 0x9B0, new C { T = "TagRef" } },

				{ 0x9E0, new C { T = "Tagblock" } },
				{ 0x9F4, new C { T = "Tagblock" } },

				{ 0xA08, new C { T = "Float" } },
				{ 0xA0C, new C { T = "Float" } },
				{ 0xA10, new C { T = "Float" } },
				{ 0xA30, new C { T = "Float" } },
				{ 0xA34, new C { T = "Float" } },
				{ 0xA38, new C { T = "Float" } },
				{ 0xA3C, new C { T = "Float" } },
				{ 0xA40, new C { T = "Float" } },
				{ 0xA44, new C { T = "Float" } },
				{ 0xA48, new C { T = "Float" } },

				{ 0xA64, new C { T = "Tagblock" } },
				{ 0xA78, new C { T = "Tagblock" } },
				{ 0xA8C, new C { T = "Tagblock" } },
				{ 0xAA0, new C { T = "Tagblock" } },

				{ 0xAB4, new C { T = "Float" } },
				{ 0xAB8, new C { T = "Float" } },

				{ 0xAC8, new C { T = "TagRef" } },

				{ 0xAE4, new C { T = "Tagblock" } },

				{ 0xAF8, new C { T = "TagRef" } },
				{ 0xB14, new C { T = "TagRef" } },
				{ 0xB7C, new C { T = "TagRef" } },
				{ 0xB98, new C { T = "TagRef" } },
				{ 0xBB4, new C { T = "Tagblock" } },
				{ 0xBE4, new C { T = "TagRef" } },

				{ 0xC04, new C { T = "Float" } },
				{ 0xC08, new C { T = "Float" } },
				{ 0xC0C, new C { T = "Float" } },
				{ 0xC10, new C { T = "Float" } },
				{ 0xC14, new C { T = "Float" } },
				{ 0xC18, new C { T = "Float" } },
				{ 0xC1C, new C { T = "Float" } },
				{ 0xC20, new C { T = "Float" } },
				{ 0xC24, new C { T = "Float" } },
				{ 0xC30, new C { T = "Float" } },
				{ 0xC34, new C { T = "Float" } },
				{ 0xC3C, new C { T = "Float" } },

				{ 0xC40, new C { T = "TagRef" } },

				{ 0xC5C, new C { T = "Float" } },
				{ 0xC60, new C { T = "Float" } },
				{ 0xC64, new C { T = "Float" } },
				{ 0xC68, new C { T = "Float" } },
				{ 0xC70, new C { T = "Float" } },
				{ 0xC74, new C { T = "Float" } },
				{ 0xC78, new C { T = "Float" } },
				{ 0xC7C, new C { T = "Float" } },
				{ 0xC80, new C { T = "Float" } },
				{ 0xC8C, new C { T = "Float" } },

				{ 0xC94, new C { T = "Float" } },
				{ 0xC98, new C { T = "Float" } },
				{ 0xCA0, new C { T = "Float" } },
				{ 0xCAC, new C { T = "Float" } },
				{ 0xCB8, new C { T = "Float" } },
				{ 0xCBC, new C { T = "Float" } },
				{ 0xCC4, new C { T = "Float" } },
				{ 0xCC8, new C { T = "Float" } },
				{ 0xCD0, new C { T = "Float" } },
				{ 0xCD4, new C { T = "Float" } },
				{ 0xCDC, new C { T = "Float" } },
				{ 0xCE0, new C { T = "Float" } },
				{ 0xCE8, new C { T = "Float" } },
				{ 0xCEC, new C { T = "Float" } },
				{ 0xCF8, new C { T = "Float" } },
				{ 0xCFC, new C { T = "Float" } },
				{ 0xD00, new C { T = "Float" } },
				{ 0xD04, new C { T = "Float" } },
				{ 0xD0C, new C { T = "Float" } },
				{ 0xD10, new C { T = "Float" } },

				{ 0xD18, new C { T = "Float" } },
				{ 0xD24, new C { T = "Float" } },

				{ 0xD28, new C { T = "TagRef" } },
				{ 0xD44, new C { T = "TagRef" } },
				{ 0xD64, new C { T = "TagRef" } },
				{ 0xD80, new C { T = "TagRef" } },

				{ 0xD9C, new C { T = "Float", N="Moving Turning speed" } },

				{ 0xDA0, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					//{ 0,  "Turns Without Animating"  },
					//{ 1,  "Has Physical Rigid Bodies When Alive"  },
					//{ 2,  "Immune To Falling Damage"  },
					//{ 3,  "Has Animated Jetpack"  },
					//{ 4,  "Flag 4"  },
					//{ 5,  "Flag 5"  },
					//{ 6,  "Random Speed Increase"  },
					//{ 7,  "Flag 7"  },
					//{ 8,  "Spawn Death Children On Destroy"  },
					//{ 9,  "Stunned By EMP Damage"  },
					//{ 10, "Dead Physics When Stunned"  },
					//{ 11, "Always Ragdoll When Dead"  },
					//{ 12, "Snaps Turns"  },
					//{ 13, "Sync Action Always Projects On Ground"  },
					//{ 14, "Orient Facing To Movement"  },
				}}},

				{ 0xDA4, new C { T = "Float", N="Stationary turning speed" } },

				{ 0xDAC, new C { T = "TagRef" } },

				{ 0xDD0, new C { T = "Float", N = "Jump Velocity" } },

				{ 0xDD4, new C { T = "Tagblock" } },

				{ 0xDEC, new C { T = "Float" } },
				{ 0xDF0, new C { T = "Float" } },
				{ 0xDF4, new C { T = "Float" } },
				{ 0xDF8, new C { T = "Float" } },
				{ 0xDFC, new C { T = "Float" } },
				{ 0xE00, new C { T = "Float" } },
				{ 0xE04, new C { T = "Float" } },
				{ 0xE08, new C { T = "Float" } },
				{ 0xE0C, new C { T = "Float" } },
				{ 0xE10, new C { T = "Float" } },
				{ 0xE14, new C { T = "Float" } },
				{ 0xE18, new C { T = "Float" } },
				{ 0xE1C, new C { T = "Float" } },
				{ 0xE24, new C { T = "Float" } },

				{ 0xE64, new C { T = "Tagblock" } },

				{ 0xE78, new C { T = "Float" } },
				{ 0xE7C, new C { T = "Float" } },
				{ 0xE80, new C { T = "Float" } },
				{ 0xE88, new C { T = "Float" } },
				{ 0xE8C, new C { T = "Float" } },
				{ 0xE90, new C { T = "Float" } },
				{ 0xE94, new C { T = "Float" } },
				{ 0xE98, new C { T = "Float" } },
				{ 0xE9C, new C { T = "Float" } },
				{ 0xEA0, new C { T = "Float" } },

				{ 0xEA4, new C { T = "TagRef" } },
				{ 0xEC0, new C { T = "TagRef" } },

				{ 0xEDC, new C { T = "Tagblock" } },
				{ 0xEF0, new C { T = "Tagblock" } },

				{ 0xF14, new FlagGroup {A = 4,STR = new Dictionary<int, string>() {
					{ 0,  "Centered At origin"  },
					{ 1,  "Shape Spherical"  },
					{ 2,  "Use Player Physics"  },
					{ 3,  "Climb Any Surface"  },
					{ 4,  "Flying"  },
					{ 5,  "Not Physical"  },
					{ 6,  "Dead Character Collision Group"  },
					{ 7,  "Suppress Ground Planes On Bipeds"  },
					{ 8,  "Physical Ragdoll"  },
					{ 9,  "Do Not Resize Dead Spheres"  },
					{ 10, "Multiple Mantis Shapes"  },
					{ 11, "I Am An Extreme Slipsurface"  },
					{ 12, "Slips Off Movers"  },
					//{ 13, "wwwww"  },
					//{ 14, "wwwww"  },
					//{ 15, "wwwww"  },
					//{ 16, "wwwww"  },
					//{ 17, "wwwww"  },
					//{ 18, "wwwww"  },
					//{ 19, "wwwww"  },
					//{ 20, "wwwww"  },
					//{ 21, "wwwww"  },
					//{ 22, "wwwww"  },
					//{ 23, "wwwww"  },
					//{ 24, "wwwww"  },
					//{ 25, "wwwww"  },
					//{ 26, "wwwww"  },
					//{ 27, "wwwww"  },
					//{ 28, "wwwww"  },
					//{ 29, "wwwww"  },
					//{ 30, "wwwww"  },
					//{ 31, "wwwww"  }
				}} },


				{ 0xF18, new C { T = "Float" } },
				{ 0xF1C, new C { T = "Float" } },
				{ 0xF20, new C { T = "Float" } },
				{ 0xF24, new C { T = "Float" } },

				{ 0xF38, new C { T = "Tagblock" } },
				{ 0xF4C, new C { T = "Tagblock" } },
				{ 0xF60, new C { T = "Tagblock" } },
				{ 0xF74, new C { T = "Tagblock" } },
				{ 0xF88, new C { T = "Tagblock" } },

				{ 0xFA0, new C { T = "Float" } },
				{ 0xFA4, new C { T = "Float" } },
				{ 0xFA8, new C { T = "Float" } },
				{ 0xFAC, new C { T = "Float" } },
				{ 0xFB0, new C { T = "Float" } },
				{ 0xFB4, new C { T = "Float" } },
				{ 0xFB8, new C { T = "Float" } },
				{ 0xFBC, new C { T = "Float" } },
				{ 0xFC0, new C { T = "Float" } },
				{ 0xFC4, new C { T = "Float" } },
				{ 0xFC8, new C { T = "Float" } },
				{ 0xFCC, new C { T = "Float" } },
				{ 0xFD0, new C { T = "Float" } },
				{ 0xFD4, new C { T = "Float" } },
				{ 0xFD8, new C { T = "Float" } },
				{ 0xFDC, new C { T = "Float" } },
				{ 0xFF0, new C { T = "Float" } },
				{ 0xFF4, new C { T = "Float" } },
				{ 0xFF8, new C { T = "Float" } },
				{ 0xFFC, new C { T = "Float" } },
				{ 0x1000, new C { T = "Float" } },
				{ 0x1004, new C { T = "Float" } },
				{ 0x1008, new C { T = "Float" } },

				{ 0x101C, new C { T = "TagRef" } },

				{ 0x1038, new C { T = "Float" } },
				{ 0x103C, new C { T = "Float" } },
				{ 0x1040, new C { T = "Float" } },
				{ 0x1044, new C { T = "Float" } },
				{ 0x1048, new C { T = "Float" } },
				{ 0x104C, new C { T = "Float" } },
				{ 0x1050, new C { T = "Float" } },
				{ 0x1054, new C { T = "Float" } },
				{ 0x1058, new C { T = "Float" } },
				{ 0x105C, new C { T = "Float" } },
				{ 0x1060, new C { T = "Float" } },
				{ 0x1064, new C { T = "Float" } },

				{ 0x1068, new C { T = "TagRef" } },
				{ 0x1084, new C { T = "Tagblock" } },

				{ 0x1098, new C { T = "TagRef" } },
				{ 0x10B4, new C { T = "TagRef" } },
				{ 0x11B8, new C { T = "TagRef" } },
				{ 0x11D4, new C { T = "TagRef" } },
				{ 0x11F0, new C { T = "TagRef" } },
				{ 0x120C, new C { T = "TagRef" } },
				{ 0x1228, new C { T = "TagRef" } },

				{ 0x1248, new C { T = "Tagblock" } },
				{ 0x125C, new C { T = "Tagblock" } },
				{ 0x1270, new C { T = "Tagblock" } },

				{ 0x1284, new C { T = "TagRef" } },
				{ 0x12A0, new C { T = "TagRef" } },

				{ 0x13E8, new C { T = "Tagblock" } },

				{ 0x150C, new C { T = "TagRef" } },
				{ 0x1528, new C { T = "TagRef" } },

				{ 0x1548, new C { T = "Tagblock" } }
			}},

			{"sqds",new()
			{
				{ 0x00, new C{ T="mmr3Hash"}},
				{ 0x14, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x10, new C{ T="TagRef"}},
					{ 0x2C, new C{ T="TagRef"}},
					{ 0x48, new C{ T="TagRef"}},
				}, S=104 }},
				{ 0x28, new C{ T="Tagblock"}},
				{ 0x3C, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x00, new C{ T="mmr3Hash"}},
					{ 0x04, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0x10, new C{ T="TagRef"}},
						{ 0x2C, new C{ T="TagRef"}},
						{ 0x48, new C{ T="TagRef"}},
					}, S=104 }},
					{ 0x18, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0x00, new C{ T="TagRef"}},
						{ 0x1C, new C{ T="TagRef"}},
						{ 0x38, new C{ T="Tagblock", B= new Dictionary<long, C>
						{
							{ 0x10, new C{ T="TagRef"}},
							{ 0x2C, new C{ T="TagRef"}},
							{ 0x48, new C{ T="TagRef"}},
						}, S=112 }},
					}, S=76 }},
				}, S=44 }},
			}},

			{"jpt!",new()
			{
				{ 0x10, new C{ T="TagRef"}},

				{ 0x2C, new C { T = "Float", N="Min Radius" } },
				{ 0x30, new C { T = "Float", N="Max Radius" } },
				{ 0x34, new C { T = "Float", N="Cutoff Scale" } },
				{ 0x38, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					{ 0,  "Don't Scale Damage By Distance"  },
					{ 1,  "Area Damage Players Only"  },
					{ 2,  "Affects Model Targets"  },
					{ 3,  "Explosive Area Of Effect"  },
					{ 4,  "these flags probably aren't right lol"  },
				}}},



				{ 0x40, new C { T = "2Byte", N="side effect" } },
				{ 0x42, new C { T = "2Byte", N="category" } },

				{ 0x44, new C { T = "4Byte", N="death vocalization" } },

				{ 0x48, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					{ 0,  "does not hurt owner"  },
					{ 1,  "can cause headshots"  },
					{ 2,  "ignores headshot obstructions"  },
					{ 3,  "pings resistant units"  },
					{ 4,  "does not hurt friends"  },
					{ 5,  "does not ping units"  },
					{ 6,  "detonates explosives"  },
					{ 7,  "only hurts shields"  },
					{ 8,  "causes flaming death"  },
					{ 9,  "skips shields"  },
					{ 10, "wwwww"  },
					{ 11, "ignore seat scale for dir. dmg"  },
					{ 12, "forces hard ping if body dmg"  },
					{ 13, "forces hard ping always"  },
					{ 14, "does not hurt players"  },
					{ 15, "enables special death"  },
					{ 16, "cannot cause betrayals"  },
					{ 17, "uses old EMP behavior"  },
					{ 18, "ignores damage resistance"  },
					{ 19, "force s_kill on death"  },
					{ 20, "cause magic deceleration"  },
					{ 21, "aoe skip obstruction test"  },
					{ 22, "does not spill over"  },
					{ 23, "does not hurt boarders"  },
					{ 24, "does not cause biped aoe effect"  },
					{ 25, "DEPRECATED apply tree of life"  },
					{ 26, "hurt only friends"  },
					{ 27, "causes incineration dissolve"  },
					{ 28, "causes incineration dissolve on headshot"  },
					{ 29, "does not hurt damage source"  },
					{ 30, "damage vehicles only"  },
					{ 31, "triggers interact logi"  }
				}}},
				{ 0x4C, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					{ 0,  "causes incineration dissolve to dead units"  },
					{ 1,  "force hard ping as attachment feedback"  },
					{ 2,  "detaches object dispenser items"  },
					{ 3,  "hard pings from this damage stun AI"  },
					{ 4,  "deaths are non revivable"  },
					{ 5,  "forces hard ping when available"  },
				}}},

				{ 0x50, new C{ T="TagRef"}},

				{ 0x6C, new C { T = "Headshot Damage" } },
				{ 0x70, new C { T = "Headshot Shielded Damage" } },

				{ 0x74, new C { T = "Float", N="AOE core radius" } },
				{ 0x78, new C { T = "Float" } },
				{ 0x7C, new C { T = "Float", N="damage lower " } },
				{ 0x80, new C { T = "Float", N="damage upper min" } },
				{ 0x84, new C { T = "Float", N="damage upper max" } },
				{ 0x88, new C { T = "Float"} },
				{ 0x8C, new C { T = "Float"} },
				{ 0x90, new C { T = "Float" } },
				{ 0x94, new C { T = "Float" } },
				{ 0x98, new C { T = "Float" } },
				{ 0x9C, new C { T = "Float" } },
				{ 0xA0, new C { T = "Float" } },
				{ 0xA4, new C { T = "Float" } },
				{ 0xA8, new C { T = "Float" } },
				{ 0xAC, new C { T = "Float" } },
				{ 0xB0, new C { T = "Float" } },
				{ 0xB4, new C { T = "Float" } },
				{ 0xB8, new C { T = "Float" } },
				{ 0xBC, new C { T = "Float" } },
				{ 0xC0, new C { T = "Float" } },
				{ 0xC4, new C { T = "Float" } },
				{ 0xC8, new C { T = "Float" } },
				{ 0xCC, new C { T = "Float", N="Instantaneous Acceleration" } },
				{ 0xD0, new C { T = "Float" } },
				{ 0xD4, new C { T = "Float" } },

				{ 0x114, new C{ T="TagRef"}},

				{ 0x130, new C{ T="Tagblock"}},

				{ 0x150, new C { T = "2Byte" } },

				{ 0x154, new C { T = "Float" } },
				{ 0x158, new C { T = "Float" } },
				{ 0x15C, new C { T = "Float" } },
				{ 0x160, new C { T = "Float" } },
				{ 0x164, new C { T = "Float" } },
				{ 0x168, new C { T = "Float" } },
				{ 0x16C, new C { T = "Float" } },

				{ 0x170, new C{ T="TagRef"}},
				{ 0x18C, new C{ T="Tagblock"}},

				{ 0x1A0, new C { T = "Float" } },
				{ 0x1A4, new C { T = "Float" } },

				{ 0x1A8, new C{ T="TagRef"}},
				{ 0x1C4, new C{ T="TagRef"}},
				{ 0x1E0, new C{ T="TagRef"}},
				{ 0x1FC, new C{ T="TagRef"}},
				{ 0x218, new C{ T="TagRef"}},
				{ 0x234, new C{ T="TagRef"}},
				{ 0x250, new C{ T="TagRef"}},
				{ 0x26C, new C{ T="TagRef"}},
				{ 0x288, new C{ T="TagRef"}},
				{ 0x2A4, new C{ T="TagRef"}},
				{ 0x2C0, new C{ T="TagRef"}},
				{ 0x2DC, new C{ T="TagRef"}},
				{ 0x2F8, new C{ T="TagRef"}},
				{ 0x314, new C{ T="TagRef"}},
				{ 0x330, new C{ T="TagRef"}},
				{ 0x34C, new C{ T="TagRef"}},
				{ 0x368, new C{ T="TagRef"}},

				{ 0x384, new C { T = "Float" } },
				{ 0x388, new C { T = "Float" } },

			}},
			
			{"sagh",new()
			{

				{ 0x14, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Use 3rd Person Camera When Active"  },
							{ 1, "Hide Reticle When Active"  },
							{ 2, "Is Motion Tracked (By Enemies) When Active"  },
							{ 3, "One-Shot Activation Effect"  },
							{ 4, "Can be activated when weapon state is relaxed"  },
							{ 5, "Does Not Appear In Extended Motion Tracker Range"  },
							{ 6, "Energy persists across deaths"  },
							{ 7, "Activation interrupts weapon switching"  },
							{ 8, "Allow activation during weapon throw"  },
							{ 9, "Allow activation while sliding"  }
				} } },
				
				{ 0x18, new C { T = "Float", N = "Activation Energy Cost" } },
				{ 0x20, new C { T = "Float", N = "Recharge Duration" } },
				{ 0x24, new C { T = "Float", N = "Recharge Delay" } },
				{ 0x28, new C { T = "Float", N = "Cooldown Delay" } },

				{ 0x88, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "allow mount passenger to activate"  },
							{ 1, "allow activation in mount if seat allows it"  },
							{ 2, "suppressed by EMP"  },
							{ 3, "drop support weapon when activated"  },
							{ 4, "drop readied weapon when activated"  },
							{ 5, "disable vehicle entering assist"  },
							{ 6, "disable jump interruptions"  },
							{ 7, "disable automelee"  },
							{ 8, "disable crouch interruptions"  },
							{ 9, "disable line of sight breaking"  },
							{ 10, "enable bash"  },
							{ 11, "show bash in first person"  },
							{ 12, "owner uses crouched pill during pull"  },
							{ 13, "owner uses crouched pill during bash"  },
							{ 14, "suppress activation while holding support weapon"  },
							{ 15, "suppress activation while holding must be readied weapon"  },
							{ 16, "ignore soft ceilings"  }
				} } },

				{ 0x154, new C { T = "Float", N = "Warmup Time" } },
				{ 0x16C, new C { T = "Float", N = "Line of Sight Testing" } },
				
				{ 0x1D0, new C { T = "Float", N = "Target Velocity" } },
				{ 0x1D4, new C { T = "Float", N = "Max Exit Velocity" } },
				{ 0x20C, new C { T = "Float", N = "Aim Influence" } },

				{ 0x284, new C { T = "Float", N = "Completion Distance (Level Geo)" } },
				{ 0x288, new C { T = "Float", N = "Completion Distance (Ceilings)" } },
				{ 0x28C, new C { T = "Float", N = "Completion Distance (Floors)" } },
				{ 0x290, new C { T = "Float", N = "Completion Distance (Biped)" } },
				{ 0x294, new C { T = "Float", N = "Completion Distance (Vehicles)" } },
				{ 0x298, new C { T = "Float", N = "Vehicle Interaction Distance" } },
				{ 0x29C, new C { T = "Float", N = "Vehicle Interaction Time" } },
				{ 0x2A0, new C { T = "Float", N = "Automelee Max Angle" } },
				{ 0x2A4, new C { T = "Float", N = "Max Allowed Angle Deviation (Ceiling)" } },
				{ 0x2A8, new C { T = "Float", N = "Max Allowed Angle Deviation (Floor)" } },
				{ 0x2AC, new C { T = "Float", N = "Line of Sight Break Time" } },
			}},
		};





		// need this mf here so i can copy paste when doing more flag groups
		//{ 0x 000, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
		//{
		//	{ 0,  "wwwww"  },
		//	{ 1,  "wwwww"  },
		//	{ 2,  "wwwww"  },
		//	{ 3,  "wwwww"  },
		//	{ 4,  "wwwww"  },
		//	{ 5,  "wwwww"  },
		//	{ 6,  "wwwww"  },
		//	{ 7,  "wwwww"  },
		//	{ 8,  "wwwww"  },
		//	{ 9,  "wwwww"  },
		//	{ 10, "wwwww"  },
		//	{ 11, "wwwww"  },
		//	{ 12, "wwwww"  },
		//	{ 13, "wwwww"  },
		//	{ 14, "wwwww"  },
		//	{ 15, "wwwww"  },
		//	{ 16, "wwwww"  },
		//	{ 17, "wwwww"  },
		//	{ 18, "wwwww"  },
		//	{ 19, "wwwww"  },
		//	{ 20, "wwwww"  },
		//	{ 21, "wwwww"  },
		//	{ 22, "wwwww"  },
		//	{ 23, "wwwww"  },
		//	{ 24, "wwwww"  },
		//	{ 25, "wwwww"  },
		//	{ 26, "wwwww"  },
		//	{ 27, "wwwww"  },
		//	{ 28, "wwwww"  },
		//	{ 29, "wwwww"  },
		//	{ 30, "wwwww"  },
		//	{ 31, "wwwww"  }
		//}}}

	}
}
