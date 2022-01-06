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

			/// <summary>
			/// String description of the flags
			/// </summary>
			public Dictionary<int, string> STR { get; set; } = new Dictionary<int, string>();
		}

		public static Dictionary<string, Dictionary<long, C>> Tags = new Dictionary<string, Dictionary<long, C>>()
		{
{ "vehi", new()
{
{ 0x0516, new C{ T="Comment", N = "_unit" }},
{ 0x0517, new C{ T="Comment", N = "_object" }},
{ 0x0518, new C{ T="Comment", N = "_AnyTag" }},
{ 0x0, new C{ T="Pointer", N = "_vtable space" }},
{ 0x8519, new C{ T="Comment", N = "_internal struct" }},
{ 0x8, new C{ T="4Byte", N = "_global tag ID" }},
{ 0xC, new C{ T="Unmapped", N = "_local tag handle" }},
{ 0x10520, new C{ T="Comment", N = "_OBJECT" }},
{ 0x10, new C{ T="2Byte", N = "_runtime object type" }},
{ 0x12, new C{ T="Byte", N = "_Nav Mesh Cutting" }},
{ 0x13, new C{ T="Byte", N = "_Nav Mesh Cutting mode" }},
{ 0x14, new C{ T="Float", N = "_Nav Mesh Silhouette Expansion" }},
{ 0x18, new C{ T="Byte", N = "_navmesh collision node opt in" }},
{ 0x19, new C{ T="Unmapped", N = "_generated_pad19c8" }},
{ 0x1C521, new C{ T="Comment", N = "_Nav Dangerous Objects" }},
{ 0x1C522, new C{ T="Comment", N = "_Avoidance" }},
{ 0x1C, new C{ T="Float", N = "_Avoidance Multiplier" }},
{ 0x20, new C{ T="Float", N = "_Avoidance Radius" }},
{ 0x24523, new C{ T="Comment", N = "_" }},
{ 0x24, new C{ T="4Byte", N = "_object flags" }},
{ 0x28, new C{ T="Float", N = "_bounding radius" }},
{ 0x2C, new C{ T="Float", N = "_bounding offset.X" }},
{ 0x30, new C{ T="Float", N = "_bounding offset.Y" }},
{ 0x34, new C{ T="Float", N = "_bounding offset.Z" }},
{ 0x38, new C{ T="Float", N = "_horizontal acceleration scale" }},
{ 0x3C, new C{ T="Float", N = "_vertical acceleration scale" }},
{ 0x40, new C{ T="Float", N = "_angular acceleration scale" }},
{ 0x44, new C{ T="Byte", N = "_lightmap shadow mode" }},
{ 0x45, new C{ T="Byte", N = "_multiple airprobe mode" }},
{ 0x46, new C{ T="Byte", N = "_water density" }},
{ 0x47524, new C{ T="Comment", N = "_Streaming" }},
{ 0x47525, new C{ T="Comment", N = "_Streaming" }},
{ 0x47, new C{ T="Byte", N = "_Streaming control flags" }},
{ 0x48526, new C{ T="Comment", N = "_" }},
{ 0x48527, new C{ T="Comment", N = "_impact audio" }},
{ 0x48528, new C{ T="Comment", N = "_IMPACT AUDIO" }},
{ 0x48, new C{ T="Float", N = "_small impact minimum speed" }},
{ 0x4C, new C{ T="Float", N = "_medium impact minimum speed" }},
{ 0x50, new C{ T="Float", N = "_large impact minimum speed" }},
{ 0x54, new C{ T="Float", N = "_large impact maximum speed" }},
{ 0x58529, new C{ T="Comment", N = "_" }},
{ 0x58, new C{ T="4Byte", N = "_runtime flags" }},
{ 0x5C, new C{ T="Tagblock", N = "_source sidecar", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="String", N = "_sidecar path" }},
}, S=256}},
{ 0x70, new C{ T="mmr3Hash", N = "_default model variant" }},
{ 0x74, new C{ T="mmr3Hash", N = "_default style id" }},
{ 0x78, new C{ T="TagRef", N = "_model" }},
{ 0x94, new C{ T="TagRef", N = "_asset" }},
{ 0xB0, new C{ T="TagRef", N = "_frame" }},
{ 0xCC, new C{ T="TagRef", N = "_crate object" }},
{ 0xE8530, new C{ T="Comment", N = "_Damage Source" }},
{ 0xE8531, new C{ T="Comment", N = "_damageSource" }},
{ 0xE8, new C{ T="mmr3Hash", N = "_Damage Source Name" }},
{ 0xEC, new C{ T="Float", N = "_navpoint vertical offset" }},
{ 0xF0532, new C{ T="Comment", N = "_only set this tag if you want to" }},
{ 0xF0, new C{ T="TagRef", N = "_collision damage" }},
{ 0x10C, new C{ T="Tagblock", N = "_early mover OBB", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_node name" }},
{ 0x4, new C{ T="4Byte", N = "_runtime node index" }},
{ 0x8, new C{ T="Float", N = "_x0" }},
{ 0xC, new C{ T="Float", N = "_x1" }},
{ 0x10, new C{ T="Float", N = "_y0" }},
{ 0x14, new C{ T="Float", N = "_y1" }},
{ 0x18, new C{ T="Float", N = "_z0" }},
{ 0x1C, new C{ T="Float", N = "_z1" }},
{ 0x20, new C{ T="Float", N = "_angles.X" }},
{ 0x24, new C{ T="Float", N = "_angles.Y" }},
{ 0x28, new C{ T="Float", N = "_angles.Z" }},
}, S=44}},
{ 0x120, new C{ T="TagRef", N = "_creation effect" }},
{ 0x13C533, new C{ T="Comment", N = "_material effect references" }},
{ 0x13C534, new C{ T="Comment", N = "_Material Effects Section" }},
{ 0x13C, new C{ T="TagRef", N = "_material effects" }},
{ 0x158, new C{ T="TagRef", N = "_visual material effects" }},
{ 0x174, new C{ T="TagRef", N = "_sound material effects" }},
{ 0x190535, new C{ T="Comment", N = "_" }},
{ 0x190, new C{ T="TagRef", N = "_melee sound" }},
{ 0x1AC, new C{ T="Float", N = "_Damage Shader Min Vitality" }},
{ 0x1B0, new C{ T="Float", N = "_Damage Shader Intensity Scalar" }},
{ 0x1B4, new C{ T="Float", N = "_kill on first contact or damage " }},
{ 0x1B8, new C{ T="Float", N = "_self-destruct time" }},
{ 0x1BC, new C{ T="Float", N = "_self-destruct time max" }},
{ 0x1C0, new C{ T="Tagblock", N = "_ai properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_ai flags" }},
{ 0x4, new C{ T="mmr3Hash", N = "_ai type name" }},
{ 0x8, new C{ T="mmr3Hash", N = "_interaction name" }},
{ 0xC, new C{ T="2Byte", N = "_ai size" }},
{ 0xE, new C{ T="2Byte", N = "_leap jump speed" }},
{ 0x10, new C{ T="Float", N = "_unattached damage modifier" }},
{ 0x14, new C{ T="Byte", N = "_Bot markup flags" }},
{ 0x15, new C{ T="Unmapped", N = "_generated_pad9518" }},
{ 0x18536, new C{ T="Comment", N = "_Heat Override" }},
{ 0x18, new C{ T="Tagblock", N = "_override heat generated per roun", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_heat generated per round overrid" }},
{ 0x4, new C{ T="Float", N = "_heat generated per round overrid" }},
{ 0x8, new C{ T="Float", N = "_heat generated per round overrid" }},
{ 0xC, new C{ T="Float", N = "_heat generated per round overrid" }},
}, S=16}},
{ 0x2C, new C{ T="Float", N = "_Actor LOD cost" }},
}, S=48}},
{ 0x1D4, new C{ T="Tagblock", N = "_functions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_flags" }},
{ 0x4, new C{ T="mmr3Hash", N = "_import name" }},
{ 0x8, new C{ T="Float", N = "_no import default" }},
{ 0xC, new C{ T="mmr3Hash", N = "_export name" }},
{ 0x10, new C{ T="mmr3Hash", N = "_turn off with" }},
{ 0x14, new C{ T="mmr3Hash", N = "_ranged interpolation name" }},
{ 0x18, new C{ T="Float", N = "_min value" }},
{ 0x1C537, new C{ T="Comment", N = "_default function" }},
{ 0x1C538, new C{ T="Comment", N = "_default function" }},
{ 0x1C539, new C{ T="Comment", N = "_" }},
{ 0x1C, new C{ T="Unmapped", N = "_data" }},
{ 0x34540, new C{ T="Comment", N = "_" }},
{ 0x34, new C{ T="mmr3Hash", N = "_scale by" }},
{ 0x38, new C{ T="Byte", N = "_interpolation point of view" }},
{ 0x39, new C{ T="Unmapped", N = "_generated_pad73a4" }},
{ 0x3C, new C{ T="Tagblock", N = "_interpolation", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_interpolation mode" }},
{ 0x4, new C{ T="Float", N = "_linear travel time" }},
{ 0x8, new C{ T="Float", N = "_acceleration" }},
{ 0xC541, new C{ T="Comment", N = "_springs" }},
{ 0xC, new C{ T="Float", N = "_spring k" }},
{ 0x10, new C{ T="Float", N = "_spring c" }},
{ 0x14, new C{ T="Float", N = "_fraction" }},
}, S=24}},
{ 0x50, new C{ T="4Byte", N = "_runtime interpolator index" }},
}, S=84}},
{ 0x1E8, new C{ T="4Byte", N = "_num predicted runtime interpolat" }},
{ 0x1EC, new C{ T="Tagblock", N = "_runtime interpolator functions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_runtime interpolator to object f" }},
}, S=4}},
{ 0x200, new C{ T="Tagblock", N = "_function switches", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_switch function name" }},
{ 0x4, new C{ T="mmr3Hash", N = "_export name" }},
{ 0x8, new C{ T="Tagblock", N = "_switched functions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_selection bounds.min" }},
{ 0x4, new C{ T="Float", N = "_selection bounds.max" }},
{ 0x8, new C{ T="mmr3Hash", N = "_function name" }},
}, S=12}},
}, S=28}},
{ 0x214, new C{ T="Tagblock", N = "_functions forwarded to parent", B = new Dictionary<long, C>
{
{ 0x0542, new C{ T="Comment", N = "_Object Function Forwarding" }},
{ 0x0, new C{ T="mmr3Hash", N = "_function to forward" }},
}, S=4}},
{ 0x228, new C{ T="2Byte", N = "_Ammo Refill Flags" }},
{ 0x22A, new C{ T="Byte", N = "_Ammo Refill Behavior Flags" }},
{ 0x22B, new C{ T="Unmapped", N = "_generated_pad33a9" }},
{ 0x22C, new C{ T="Tagblock", N = "_Ammo Refill Variant Flags", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_variant name" }},
{ 0x4, new C{ T="2Byte", N = "_Ammo Refill Flags" }},
{ 0x6, new C{ T="Byte", N = "_Ammo Refill Behavior Flags" }},
{ 0x7, new C{ T="Unmapped", N = "_generated_pad055c" }},
}, S=8}},
{ 0x240, new C{ T="4Byte", N = "_object secondary flags" }},
{ 0x244543, new C{ T="Comment", N = "_Customization" }},
{ 0x244, new C{ T="mmr3Hash", N = "_customization set" }},
{ 0x248544, new C{ T="Comment", N = "_" }},
{ 0x248, new C{ T="Tagblock", N = "_attachments", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="TagRef", N = "_type" }},
{ 0x20, new C{ T="TagRef", N = "_tag graph output" }},
{ 0x3C, new C{ T="mmr3Hash", N = "_output node name" }},
{ 0x40, new C{ T="Tagblock", N = "_tag graph float params", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="Float", N = "_value" }},
}, S=8}},
{ 0x54, new C{ T="TagRef", N = "_override type" }},
{ 0x70545, new C{ T="Comment", N = "_variant names" }},
{ 0x70, new C{ T="Tagblock", N = "_variant names", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_variant name" }},
}, S=4}},
{ 0x84546, new C{ T="Comment", N = "_" }},
{ 0x84, new C{ T="mmr3Hash", N = "_marker" }},
{ 0x88, new C{ T="Byte", N = "_change color" }},
{ 0x89, new C{ T="Byte", N = "_flags" }},
{ 0x8A, new C{ T="Unmapped", N = "_generated_padbd3f" }},
{ 0x8C, new C{ T="mmr3Hash", N = "_primary scale" }},
{ 0x90, new C{ T="mmr3Hash", N = "_secondary scale" }},
}, S=148}},
{ 0x25C, new C{ T="Tagblock", N = "_indirect lighting data", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Name" }},
{ 0x4, new C{ T="Tagblock", N = "_Variant names", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_variant name" }},
}, S=4}},
{ 0x18, new C{ T="TagRef", N = "_Object Lightmap" }},
{ 0x34, new C{ T="Tagblock", N = "_Object Cubemap set", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Name" }},
{ 0x4, new C{ T="4Byte", N = "_Flags" }},
{ 0x8547, new C{ T="Comment", N = "_Dumpling" }},
{ 0x8, new C{ T="Tagblock", N = "_inner points", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_position.X" }},
{ 0x4, new C{ T="Float", N = "_position.Y" }},
{ 0x8, new C{ T="Float", N = "_position.Z" }},
}, S=12}},
{ 0x1C, new C{ T="Tagblock", N = "_outer points", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_position.X" }},
{ 0x4, new C{ T="Float", N = "_position.Y" }},
{ 0x8, new C{ T="Float", N = "_position.Z" }},
}, S=12}},
{ 0x30, new C{ T="Float", N = "_height" }},
{ 0x34, new C{ T="Float", N = "_sink" }},
{ 0x38, new C{ T="Float", N = "_inner value" }},
{ 0x3C, new C{ T="Float", N = "_outer value" }},
{ 0x40, new C{ T="Float", N = "_center point.X" }},
{ 0x44, new C{ T="Float", N = "_center point.Y" }},
{ 0x48, new C{ T="Float", N = "_center point.Z" }},
{ 0x4C, new C{ T="Float", N = "_trivial cull radius squared" }},
{ 0x50, new C{ T="Float", N = "_bound volume" }},
{ 0x54, new C{ T="TagRef", N = "_Cubemap Bitmap" }},
{ 0x70, new C{ T="2Byte", N = "_Enable Parallax Correction" }},
{ 0x72, new C{ T="2Byte", N = "_Cubemap Volume Priority" }},
{ 0x74, new C{ T="Float", N = "_Cubemap Origin.X" }},
{ 0x78, new C{ T="Float", N = "_Cubemap Origin.Y" }},
{ 0x7C, new C{ T="Float", N = "_Cubemap Origin.Z" }},
{ 0x80, new C{ T="Float", N = "_Depth Positive.X" }},
{ 0x84, new C{ T="Float", N = "_Depth Positive.Y" }},
{ 0x88, new C{ T="Float", N = "_Depth Positive.Z" }},
{ 0x8C, new C{ T="Float", N = "_Depth Negative.X" }},
{ 0x90, new C{ T="Float", N = "_Depth Negative.Y" }},
{ 0x94, new C{ T="Float", N = "_Depth Negative.Z" }},
{ 0x98, new C{ T="2Byte", N = "_Active Volume" }},
{ 0x9A, new C{ T="2Byte", N = "_Maximum cubemap size" }},
{ 0x9C, new C{ T="Float", N = "_Intensity.X" }},
{ 0xA0, new C{ T="Float", N = "_Intensity.Y" }},
{ 0xA4, new C{ T="Float", N = "_Intensity.Z" }},
{ 0xA8, new C{ T="Float", N = "_Self-illum Scale Down" }},
{ 0xAC, new C{ T="Tagblock", N = "_EditorMetadata", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="String", N = "_folder name" }},
{ 0x100, new C{ T="Float", N = "_local space pivot point position.X" }},
{ 0x104, new C{ T="Float", N = "_local space pivot point position.Y" }},
{ 0x108, new C{ T="Float", N = "_local space pivot point position.Z" }},
}, S=268}},
{ 0xC0, new C{ T="TagRef", N = "_owner bsp" }},
{ 0xDC, new C{ T="mmr3Hash", N = "_owner variant" }},
{ 0xE0, new C{ T="2Byte", N = "_runtime bsp index" }},
{ 0xE2, new C{ T="2Byte", N = "_runtime variant index" }},
{ 0xE4, new C{ T="Float", N = "_Blend Distance" }},
{ 0xE8, new C{ T="4Byte", N = "_Cubemap Volume Type" }},
{ 0xEC, new C{ T="Float", N = "_Orientation" }},
{ 0xF0, new C{ T="TagRef", N = "_Cubemap Depth Bitmap" }},
{ 0x10C, new C{ T="Float", N = "_Depth Blend Range" }},
{ 0x110, new C{ T="Float", N = "_Intensity Scale" }},
{ 0x114, new C{ T="2Byte", N = "_Tintable" }},
{ 0x116, new C{ T="Unmapped", N = "_generated_pad93d5" }},
{ 0x118, new C{ T="Float", N = "_Relevancy Radius" }},
}, S=284}},
}, S=72}},
{ 0x270, new C{ T="Tagblock", N = "_hull surfaces", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_flags" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_pad978b" }},
{ 0x4548, new C{ T="Comment", N = "_" }},
{ 0x4, new C{ T="mmr3Hash", N = "_marker name" }},
{ 0x8, new C{ T="Float", N = "_radius" }},
{ 0xC, new C{ T="Tagblock", N = "_drag", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_material" }},
{ 0x4549, new C{ T="Comment", N = "_drag" }},
{ 0x4550, new C{ T="Comment", N = "_Pressure" }},
{ 0x4551, new C{ T="Comment", N = "_pressure" }},
{ 0x4552, new C{ T="Comment", N = "_velocity to force" }},
{ 0x4553, new C{ T="Comment", N = "_velocity to pressure" }},
{ 0x4554, new C{ T="Comment", N = "_" }},
{ 0x4, new C{ T="Unmapped", N = "_data" }},
{ 0x1C555, new C{ T="Comment", N = "_" }},
{ 0x1C, new C{ T="Float", N = "_max velocity" }},
{ 0x20556, new C{ T="Comment", N = "_" }},
{ 0x20557, new C{ T="Comment", N = "_Suction" }},
{ 0x20558, new C{ T="Comment", N = "_suction" }},
{ 0x20559, new C{ T="Comment", N = "_velocity to force" }},
{ 0x20560, new C{ T="Comment", N = "_velocity to pressure" }},
{ 0x20561, new C{ T="Comment", N = "_" }},
{ 0x20, new C{ T="Unmapped", N = "_data" }},
{ 0x38562, new C{ T="Comment", N = "_" }},
{ 0x38, new C{ T="Float", N = "_max velocity" }},
{ 0x3C563, new C{ T="Comment", N = "_" }},
{ 0x3C, new C{ T="Float", N = "_linear damping" }},
{ 0x40, new C{ T="Float", N = "_angular damping" }},
}, S=68}},
}, S=32}},
{ 0x284, new C{ T="Tagblock", N = "_jetwash", B = new Dictionary<long, C>
{
{ 0x0564, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="mmr3Hash", N = "_marker name" }},
{ 0x4, new C{ T="Float", N = "_radius" }},
{ 0x8, new C{ T="4Byte", N = "_maximum traces" }},
{ 0xC, new C{ T="Float", N = "_maximum emission length" }},
{ 0x10, new C{ T="Float", N = "_trace yaw angle.min" }},
{ 0x14, new C{ T="Float", N = "_trace yaw angle.max" }},
{ 0x18, new C{ T="Float", N = "_trace pitch angle.min" }},
{ 0x1C, new C{ T="Float", N = "_trace pitch angle.max" }},
{ 0x20, new C{ T="Float", N = "_particle offset" }},
}, S=36}},
{ 0x298, new C{ T="Tagblock", N = "_widgets", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_type" }},
}, S=28}},
{ 0x2AC, new C{ T="Tagblock", N = "_change colors", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_initial permutations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_weight" }},
{ 0x4, new C{ T="Unmapped", N = "_color lower bound" }},
{ 0x10, new C{ T="Unmapped", N = "_color upper bound" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_variant name" }},
}, S=32}},
{ 0x14, new C{ T="Tagblock", N = "_functions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_scale flags" }},
{ 0x4, new C{ T="Unmapped", N = "_color lower bound" }},
{ 0x10, new C{ T="Unmapped", N = "_color upper bound" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_darken by" }},
{ 0x20, new C{ T="mmr3Hash", N = "_scale by" }},
}, S=36}},
}, S=40}},
{ 0x2C0, new C{ T="Tagblock", N = "_multiplayer object", B = new Dictionary<long, C>
{
{ 0x0565, new C{ T="Comment", N = "_TYPE AND FLAGS" }},
{ 0x0, new C{ T="Byte", N = "_type" }},
{ 0x1, new C{ T="Byte", N = "_teleporter passability" }},
{ 0x2, new C{ T="Byte", N = "_spawn timer type" }},
{ 0x3, new C{ T="Unmapped", N = "_generated_padd1ba" }},
{ 0x4, new C{ T="2Byte", N = "_number of fx color overrides" }},
{ 0x6, new C{ T="Unmapped", N = "_generated_pad1e6f" }},
{ 0x8566, new C{ T="Comment", N = "_GOAL AND RESPAWN ZONE OBJECT BOU" }},
{ 0x8, new C{ T="Float", N = "_boundary width/radius" }},
{ 0xC, new C{ T="Float", N = "_boundary box length" }},
{ 0x10, new C{ T="Float", N = "_boundary +height" }},
{ 0x14, new C{ T="Float", N = "_boundary -height" }},
{ 0x18, new C{ T="Byte", N = "_boundary shape" }},
{ 0x19, new C{ T="Unmapped", N = "_pad_shape1" }},
{ 0x1A, new C{ T="Unmapped", N = "_pad_shape2" }},
{ 0x1C567, new C{ T="Comment", N = "_SPAWNING DATA" }},
{ 0x1C, new C{ T="2Byte", N = "_default spawn time" }},
{ 0x1E, new C{ T="2Byte", N = "_default abandonment time" }},
{ 0x20, new C{ T="4Byte", N = "_flags" }},
{ 0x24568, new C{ T="Comment", N = "_RESPAWN ZONE DATA" }},
{ 0x24, new C{ T="Float", N = "_normal weight" }},
{ 0x28, new C{ T="Tagblock", N = "_falloff function", B = new Dictionary<long, C>
{
{ 0x0569, new C{ T="Comment", N = "_function" }},
{ 0x0570, new C{ T="Comment", N = "_function" }},
{ 0x0571, new C{ T="Comment", N = "_function" }},
{ 0x0572, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="Unmapped", N = "_data" }},
{ 0x18573, new C{ T="Comment", N = "_" }},
}, S=24}},
{ 0x3C574, new C{ T="Comment", N = "_MARKER DATA" }},
{ 0x3C575, new C{ T="Comment", N = "_" }},
{ 0x3C, new C{ T="mmr3Hash", N = "_boundary center marker" }},
{ 0x40576, new C{ T="Comment", N = "_" }},
{ 0x40, new C{ T="mmr3Hash", N = "_spawned object marker name" }},
{ 0x44577, new C{ T="Comment", N = "_SPAWNED OBJECT" }},
{ 0x44, new C{ T="TagRef", N = "_spawned object" }},
{ 0x60, new C{ T="mmr3Hash", N = "_NYI boundary material" }},
{ 0x64578, new C{ T="Comment", N = "_BOUNDARY SHADER - DEFAULT" }},
{ 0x64, new C{ T="TagRef", N = "_boundary standard shader" }},
{ 0x80579, new C{ T="Comment", N = "_BOUNDARY SHADER - SPHERE" }},
{ 0x80, new C{ T="TagRef", N = "_sphere standard shader" }},
{ 0x9C580, new C{ T="Comment", N = "_BOUNDARY SHADER - CYLINDER" }},
{ 0x9C, new C{ T="TagRef", N = "_cylinder standard shader" }},
{ 0xB8581, new C{ T="Comment", N = "_BOUNDARY SHADER - BOX" }},
{ 0xB8, new C{ T="TagRef", N = "_box standard shader" }},
{ 0xD4582, new C{ T="Comment", N = "_FORGE DATA" }},
{ 0xD4, new C{ T="2Byte", N = "_Forge Property Flags" }},
{ 0xD6, new C{ T="Byte", N = "_Default Physics Type" }},
{ 0xD7, new C{ T="Unmapped", N = "_generated_pad6b24" }},
{ 0xD8, new C{ T="mmr3Hash", N = "_Default Primary Color" }},
{ 0xDC, new C{ T="mmr3Hash", N = "_Default Secondary Color" }},
{ 0xE0, new C{ T="mmr3Hash", N = "_Default Tertiary Color" }},
{ 0xE4, new C{ T="Byte", N = "_Default Primary Glossiness" }},
{ 0xE5, new C{ T="Byte", N = "_Default Secondary Glossiness" }},
{ 0xE6, new C{ T="Byte", N = "_Default Tertiary Glossiness" }},
{ 0xE7, new C{ T="Unmapped", N = "_generated_padb7d9" }},
{ 0xE8, new C{ T="mmr3Hash", N = "_Default Label [1]" }},
{ 0xEC, new C{ T="mmr3Hash", N = "_Default Label [2]" }},
{ 0xF0, new C{ T="mmr3Hash", N = "_Default Label [3]" }},
{ 0xF4, new C{ T="mmr3Hash", N = "_Default Label [4]" }},
{ 0xF8, new C{ T="Pointer", N = "_m_defaultBoundaryPipelineState" }},
{ 0x100, new C{ T="Pointer", N = "_m_sphereBoundaryPipelineStates" }},
{ 0x108, new C{ T="Pointer", N = "_m_cylinderBoundaryPipelineStates" }},
{ 0x110, new C{ T="Pointer", N = "_m_boxBoundaryPipelineStates" }},
{ 0x118, new C{ T="Float", N = "_Callout Radius" }},
{ 0x11C, new C{ T="Unmapped", N = "_generated_pad048e" }},
}, S=288}},
{ 0x2D4, new C{ T="Tagblock", N = "_Forge data", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_Palette" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_Name" }},
{ 0x20, new C{ T="mmr3Hash", N = "_Description" }},
{ 0x24, new C{ T="Float", N = "_Placement Priority" }},
{ 0x28, new C{ T="mmr3Hash", N = "_Variant Name" }},
{ 0x2C, new C{ T="TagRef", N = "_Configuration" }},
{ 0x48, new C{ T="Float", N = "_Starting yaw/pitch/roll.X" }},
{ 0x4C, new C{ T="Float", N = "_Starting yaw/pitch/roll.Y" }},
{ 0x50, new C{ T="Float", N = "_Starting yaw/pitch/roll.Z" }},
}, S=84}},
{ 0x2E8, new C{ T="TagRef", N = "_simulation_interpolation" }},
{ 0x304, new C{ T="TagRef", N = "_authority trust settings" }},
{ 0x320, new C{ T="Tagblock", N = "_spawn effects", B = new Dictionary<long, C>
{
{ 0x0583, new C{ T="Comment", N = "_Spawn Effects" }},
{ 0x0, new C{ T="TagRef", N = "_multiplayer spawn effect" }},
{ 0x1C, new C{ T="TagRef", N = "_campaign spawn effect" }},
}, S=56}},
{ 0x334, new C{ T="Tagblock", N = "_model dissolve data", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_model dissolve data" }},
}, S=28}},
{ 0x348, new C{ T="String", N = "_class name" }},
{ 0x448, new C{ T="Tagblock", N = "_script tagalongs", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_reference" }},
}, S=28}},
{ 0x45C, new C{ T="TagRef", N = "_prototype script" }},
{ 0x478584, new C{ T="Comment", N = "_Object Abandoment" }},
{ 0x478585, new C{ T="Comment", N = "_object abandonment" }},
{ 0x478, new C{ T="Float", N = "_Vitality Limit To Start Countdow" }},
{ 0x47C, new C{ T="Float", N = "_Countdown Time In Seconds" }},
{ 0x480, new C{ T="Byte", N = "_flags" }},
{ 0x481, new C{ T="Unmapped", N = "_generated_pade8bf" }},
{ 0x484, new C{ T="Tagblock", N = "_Designer Metadata", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_meta label" }},
}, S=4}},
{ 0x498586, new C{ T="Comment", N = "_Object Function Smoothing" }},
{ 0x498, new C{ T="Tagblock", N = "_Object Sound RTPCs", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Attachment Index" }},
{ 0x4, new C{ T="mmr3Hash", N = "_Function" }},
{ 0x8, new C{ T="mmr3Hash", N = "_RTPC Name" }},
{ 0xC, new C{ T="4Byte", N = "_RTPC name hash value" }},
}, S=16}},
{ 0x4AC, new C{ T="Tagblock", N = "_Object Sound Sweeteners", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Function" }},
{ 0x4, new C{ T="TagRef", N = "_sound" }},
{ 0x20, new C{ T="Float", N = "_Switch point" }},
{ 0x24, new C{ T="4Byte", N = "_Mode" }},
}, S=40}},
{ 0x4C0, new C{ T="mmr3Hash", N = "_Audio acoustics root marker over" }},
{ 0x4C4, new C{ T="Float", N = "_Audio Acoustics Transparency" }},
{ 0x4C8, new C{ T="Tagblock", N = "_Object function smoothing", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_id" }},
{ 0x4, new C{ T="mmr3Hash", N = "_alias of" }},
{ 0x8, new C{ T="Float", N = "_smoothing factor" }},
}, S=12}},
{ 0x4DC, new C{ T="Byte", N = "_Animated mesh animation type" }},
{ 0x4DD, new C{ T="Unmapped", N = "_generated_pad805e" }},
{ 0x4E0, new C{ T="Float", N = "_Placement cost multiplier" }},
{ 0x4E4, new C{ T="TagRef", N = "_Trigger volume component info" }},
{ 0x500, new C{ T="Tagblock", N = "_Tracking Info", B = new Dictionary<long, C>
{
{ 0x0587, new C{ T="Comment", N = "_Map these settings to a ping typ" }},
{ 0x0, new C{ T="Byte", N = "_flags" }},
{ 0x1, new C{ T="Byte", N = "_ping type" }},
{ 0x2, new C{ T="Byte", N = "_team visibility" }},
{ 0x3588, new C{ T="Comment", N = "_Misc" }},
{ 0x3, new C{ T="Byte", N = "_priority" }},
{ 0x4589, new C{ T="Comment", N = "_" }},
{ 0x4, new C{ T="mmr3Hash", N = "_ping group" }},
{ 0x8590, new C{ T="Comment", N = "_Nav Point" }},
{ 0x8, new C{ T="mmr3Hash", N = "_nav point screen" }},
{ 0xC, new C{ T="mmr3Hash", N = "_nav point label" }},
{ 0x10, new C{ T="Float", N = "_nav point vertical offset" }},
{ 0x14591, new C{ T="Comment", N = "_" }},
{ 0x14592, new C{ T="Comment", N = "_Range Override" }},
{ 0x14, new C{ T="Float", N = "_range" }},
{ 0x18593, new C{ T="Comment", N = "_Effects" }},
{ 0x18, new C{ T="TagRef", N = "_ping hit instigator effect" }},
{ 0x34, new C{ T="TagRef", N = "_ping hit effect" }},
{ 0x50, new C{ T="TagRef", N = "_outline override" }},
{ 0x6C594, new C{ T="Comment", N = "_" }},
}, S=108}},
{ 0x514, new C{ T="TagRef", N = "_Property Based Effects" }},
{ 0x530, new C{ T="Tagblock", N = "_Interactions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Marker" }},
{ 0x4, new C{ T="Tagblock", N = "_Ambient Info", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_desire type" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_pad5f2a" }},
{ 0x4, new C{ T="4Byte", N = "_action" }},
{ 0x8, new C{ T="mmr3Hash", N = "_Animation Token Override" }},
{ 0xC, new C{ T="Float", N = "_Time playing animation" }},
{ 0x10, new C{ T="Float", N = "_Cooldown time" }},
{ 0x14, new C{ T="mmr3Hash", N = "_composition name" }},
{ 0x18, new C{ T="TagRef", N = "_composition definition" }},
{ 0x34, new C{ T="Float", N = "_Max distance agents will conside" }},
{ 0x38, new C{ T="Tagblock", N = "_Ambient meta labels", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_meta label" }},
}, S=4}},
{ 0x4C, new C{ T="Tagblock", N = "_Exclusion meta labels", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_meta label" }},
}, S=4}},
{ 0x60, new C{ T="Float", N = "_Min Danger to Activate" }},
{ 0x64, new C{ T="Float", N = "_Max Danger to Activate" }},
{ 0x68, new C{ T="Float", N = "_Abort Danger Level" }},
{ 0x6C, new C{ T="Float", N = "_Chance Per Second" }},
{ 0x70, new C{ T="Float", N = "_Min Enemy Target Distance" }},
{ 0x74, new C{ T="Float", N = "_Max Enemy Target Distance" }},
{ 0x78, new C{ T="Float", N = "_Max Height Difference" }},
}, S=124}},
}, S=24}},
{ 0x544595, new C{ T="Comment", N = "_Motion tracker blips" }},
{ 0x544, new C{ T="Byte", N = "_blip sprite index override" }},
{ 0x545, new C{ T="Unmapped", N = "_generated_pad9a3f" }},
{ 0x548596, new C{ T="Comment", N = "_" }},
{ 0x548, new C{ T="Tagblock", N = "_data driven scripted sequence ac", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="String", N = "_Prototype trigger lua function n" }},
{ 0x20, new C{ T="Tagblock", N = "_Component Actions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="String", N = "_Request action lua function name" }},
{ 0x20, new C{ T="String", N = "_Go to next action condition lua " }},
}, S=64}},
}, S=52}},
{ 0x55C, new C{ T="TagRef", N = "_Animation Node Graph" }},
{ 0x578597, new C{ T="Comment", N = "_Anim Set Table" }},
{ 0x578, new C{ T="Tagblock", N = "_channels", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_channel" }},
}, S=4}},
{ 0x58C, new C{ T="Tagblock", N = "_table entries", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_token" }},
{ 0x4, new C{ T="TagRef", N = "_animset" }},
{ 0x20, new C{ T="TagRef", N = "_Runtime animset (used by code on" }},
{ 0x3C, new C{ T="Byte", N = "_is default" }},
{ 0x3D, new C{ T="Unmapped", N = "_generated_padca09" }},
{ 0x3E, new C{ T="2Byte", N = "_flags" }},
{ 0x40, new C{ T="Tagblock", N = "_Variants", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Variant" }},
}, S=4}},
}, S=84}},
{ 0x5A0, new C{ T="Float", N = "_Inertialization Duration" }},
{ 0x5A4, new C{ T="Float", N = "_Pill Offset Blend Rate For Groun" }},
{ 0x5A8, new C{ T="Float", N = "_Stationary Root Offset Limit.min" }},
{ 0x5AC, new C{ T="Float", N = "_Stationary Root Offset Limit.max" }},
{ 0x5B0, new C{ T="Float", N = "_Moving Root Offset Limit.min" }},
{ 0x5B4, new C{ T="Float", N = "_Moving Root Offset Limit.max" }},
{ 0x5B8, new C{ T="Float", N = "_Ground IK marker above ground to" }},
{ 0x5BC, new C{ T="Float", N = "_Ground Probe Start Position Heig" }},
{ 0x5C0, new C{ T="Float", N = "_Ground Probe Additional Height D" }},
{ 0x5C4, new C{ T="Unmapped", N = "_Anim Set Nodegraph Metadata" }},
{ 0x5DC598, new C{ T="Comment", N = "_Root Bone Correction Settings" }},
{ 0x5DC599, new C{ T="Comment", N = "_RootBoneCorrectionSettings" }},
{ 0x5DC, new C{ T="Byte", N = "_flags" }},
{ 0x5DD, new C{ T="Byte", N = "_Root Bone Correction Type" }},
{ 0x5DE, new C{ T="Unmapped", N = "_generated_pad0a63" }},
{ 0x5E0, new C{ T="Float", N = "_Root Bone Correction Duration" }},
{ 0x5E4, new C{ T="Byte", N = "_Root Bone Correction Bone Index" }},
{ 0x5E5, new C{ T="Unmapped", N = "_generated_padbdae" }},
{ 0x5E8600, new C{ T="Comment", N = "_" }},
{ 0x5E8, new C{ T="Tagblock", N = "_Leg Grounding Settings", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Marker" }},
{ 0x4, new C{ T="Float", N = "_Planted Threshold" }},
{ 0x8, new C{ T="Float", N = "_Lifted Threshold" }},
{ 0xC, new C{ T="Float", N = "_Lifted Minimum Time" }},
{ 0x10, new C{ T="2Byte", N = "_Event" }},
{ 0x12, new C{ T="Unmapped", N = "_generated_paddc16" }},
}, S=20}},
{ 0x5FC601, new C{ T="Comment", N = "_Wrist Break Fixup" }},
{ 0x5FC, new C{ T="Float", N = "_Wrist Break Interp Time" }},
{ 0x600, new C{ T="Float", N = "_Wrist Break Angle Limit" }},
{ 0x604, new C{ T="Float", N = "_Wrist Break Fixup Scale" }},
{ 0x608602, new C{ T="Comment", N = "_" }},
{ 0x608, new C{ T="Byte", N = "_Flags" }},
{ 0x609, new C{ T="Unmapped", N = "_generated_pad6f99" }},
{ 0x60C603, new C{ T="Comment", N = "_Object Node Graph" }},
{ 0x60C, new C{ T="Tagblock", N = "_Object Node Graphs", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Name" }},
{ 0x4, new C{ T="TagRef", N = "_Exported Node Graph" }},
{ 0x20, new C{ T="Byte", N = "_Flags" }},
{ 0x21, new C{ T="Byte", N = "_Enable Node Graph" }},
{ 0x22, new C{ T="Unmapped", N = "_generated_pad8671" }},
}, S=36}},
{ 0x620604, new C{ T="Comment", N = "_" }},
{ 0x620, new C{ T="Tagblock", N = "_parent animation matching", B = new Dictionary<long, C>
{
{ 0x0605, new C{ T="Comment", N = "_ANIMATION REMAPPING" }},
{ 0x0, new C{ T="mmr3Hash", N = "_animation on parent" }},
{ 0x4, new C{ T="mmr3Hash", N = "_animation to play on object" }},
{ 0x8, new C{ T="mmr3Hash", N = "_animation set to use with the ch" }},
}, S=12}},
{ 0x634606, new C{ T="Comment", N = "_model variant switching" }},
{ 0x634607, new C{ T="Comment", N = "_MODEL VARIANT SWITCHING" }},
{ 0x634, new C{ T="mmr3Hash", N = "_model variant switching function" }},
{ 0x638, new C{ T="Tagblock", N = "_model variant switching table", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_function result range.min" }},
{ 0x4, new C{ T="Float", N = "_function result range.max" }},
{ 0x8, new C{ T="mmr3Hash", N = "_model variant name" }},
}, S=12}},
{ 0x64C608, new C{ T="Comment", N = "_" }},
{ 0x64C609, new C{ T="Comment", N = "_knockback" }},
{ 0x64C, new C{ T="Float", N = "_knockback impulse scalar" }},
{ 0x650610, new C{ T="Comment", N = "_" }},
{ 0x650611, new C{ T="Comment", N = "_Ability Components" }},
{ 0x650, new C{ T="Tagblock", N = "_location sensor", B = new Dictionary<long, C>
{
{ 0x0612, new C{ T="Comment", N = "_Sensor Parameters" }},
{ 0x0, new C{ T="4Byte", N = "_sensor flags" }},
{ 0x4, new C{ T="Float", N = "_ping frequency" }},
{ 0x8, new C{ T="Float", N = "_ping radius" }},
{ 0xC, new C{ T="Float", N = "_reveal duration" }},
{ 0x10, new C{ T="Float", N = "_scanning duration" }},
{ 0x14, new C{ T="Float", N = "_minimum delay between marks" }},
{ 0x18, new C{ T="Float", N = "_delay per distance" }},
{ 0x1C613, new C{ T="Comment", N = "_" }},
{ 0x1C614, new C{ T="Comment", N = "_Sensor Visuals" }},
{ 0x1C, new C{ T="TagRef", N = "_sensor radius effect" }},
{ 0x38, new C{ T="mmr3Hash", N = "_sensor radius effect marker" }},
{ 0x3C, new C{ T="TagRef", N = "_sensor ping effect" }},
{ 0x58615, new C{ T="Comment", N = "_" }},
{ 0x58616, new C{ T="Comment", N = "_Target Visuals" }},
{ 0x58, new C{ T="TagRef", N = "_target outline" }},
{ 0x74, new C{ T="TagRef", N = "_target visor ping effect" }},
{ 0x90, new C{ T="mmr3Hash", N = "_target visor ping effect marker" }},
{ 0x94617, new C{ T="Comment", N = "_" }},
{ 0x94618, new C{ T="Comment", N = "_Navpoints" }},
{ 0x94, new C{ T="mmr3Hash", N = "_sensor navpoint name" }},
{ 0x98, new C{ T="mmr3Hash", N = "_sensor navpoint marker" }},
{ 0x9C, new C{ T="mmr3Hash", N = "_target navpoint name" }},
{ 0xA0, new C{ T="mmr3Hash", N = "_target navpoint marker" }},
{ 0xA4619, new C{ T="Comment", N = "_" }},
}, S=164}},
{ 0x664, new C{ T="Tagblock", N = "_shroud generator", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_outer radius" }},
{ 0x4, new C{ T="mmr3Hash", N = "_edge effect marker" }},
{ 0x8, new C{ T="TagRef", N = "_biped enter effect" }},
{ 0x24, new C{ T="TagRef", N = "_biped inside effect" }},
{ 0x40, new C{ T="TagRef", N = "_biped leave effect" }},
{ 0x5C620, new C{ T="Comment", N = "_Mapping function" }},
{ 0x5C621, new C{ T="Comment", N = "_inside shrouded mapping" }},
{ 0x5C622, new C{ T="Comment", N = "_" }},
{ 0x5C, new C{ T="Unmapped", N = "_data" }},
{ 0x74623, new C{ T="Comment", N = "_" }},
{ 0x74, new C{ T="Float", N = "_max distance inside for shroud" }},
{ 0x78624, new C{ T="Comment", N = "_Mapping function" }},
{ 0x78625, new C{ T="Comment", N = "_outside shrouded mapping" }},
{ 0x78626, new C{ T="Comment", N = "_" }},
{ 0x78, new C{ T="Unmapped", N = "_data" }},
{ 0x90627, new C{ T="Comment", N = "_" }},
{ 0x90, new C{ T="Float", N = "_max distance outside for shroud" }},
{ 0x94628, new C{ T="Comment", N = "_Blocker" }},
{ 0x94, new C{ T="TagRef", N = "_blocker object" }},
{ 0xB0, new C{ T="mmr3Hash", N = "_blocker attach marker" }},
{ 0xB4, new C{ T="mmr3Hash", N = "_blocker attach child marker" }},
{ 0xB8, new C{ T="Float", N = "_blocker scale" }},
{ 0xBC629, new C{ T="Comment", N = "_" }},
}, S=188}},
{ 0x678630, new C{ T="Comment", N = "_" }},
{ 0x678, new C{ T="Tagblock", N = "_Power Component", B = new Dictionary<long, C>
{
{ 0x0631, new C{ T="Comment", N = "_Power Component System" }},
{ 0x0, new C{ T="Tagblock", N = "_power source configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="Float", N = "_capacity" }},
}, S=8}},
{ 0x14, new C{ T="Tagblock", N = "_power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_region name" }},
{ 0x4, new C{ T="4Byte", N = "_runtime region index" }},
{ 0x8632, new C{ T="Comment", N = "_Model Region Damage State Config" }},
{ 0x8, new C{ T="Tagblock", N = "_state configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_region state" }},
{ 0x2, new C{ T="2Byte", N = "_configuration" }},
}, S=4}},
}, S=28}},
{ 0x28633, new C{ T="Comment", N = "_communication node capacity" }},
{ 0x28, new C{ T="Tagblock", N = "_communication node power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_component" }},
}, S=2}},
{ 0x3C, new C{ T="mmr3Hash", N = "_communication node power modifie" }},
{ 0x40634, new C{ T="Comment", N = "_" }},
{ 0x40635, new C{ T="Comment", N = "_locomotion capacity" }},
{ 0x40, new C{ T="Tagblock", N = "_locomotion power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_component" }},
}, S=2}},
{ 0x54, new C{ T="mmr3Hash", N = "_locomotion power modifier id" }},
{ 0x58636, new C{ T="Comment", N = "_locomotion power modifier" }},
{ 0x58637, new C{ T="Comment", N = "_locomotion power modifier" }},
{ 0x58, new C{ T="Float", N = "_Base Value" }},
{ 0x5C, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0x60, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0x64638, new C{ T="Comment", N = "_" }},
{ 0x64639, new C{ T="Comment", N = "_weapon capacity" }},
{ 0x64, new C{ T="Tagblock", N = "_weapon power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_component" }},
}, S=2}},
{ 0x78, new C{ T="mmr3Hash", N = "_weapon power modifier id" }},
{ 0x7C640, new C{ T="Comment", N = "_" }},
}, S=124}},
{ 0x68C, new C{ T="Tagblock", N = "_self destruct handler", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_Self destruct sounds", B = new Dictionary<long, C>
{
{ 0x0641, new C{ T="Comment", N = "_SELF DESTRUCTION SOUNDS" }},
{ 0x0, new C{ T="4Byte", N = "_active damage section count" }},
{ 0x4, new C{ T="TagRef", N = "_sound to play" }},
}, S=32}},
}, S=20}},
{ 0x6A0, new C{ T="Tagblock", N = "_Indirect Lighting Component", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Lightmap attachment bone" }},
}, S=4}},
{ 0x6B4642, new C{ T="Comment", N = "_" }},
{ 0x6B4, new C{ T="Unmapped", N = "_generated_paddd0c" }},
{ 0x6B8643, new C{ T="Comment", N = "_$$$ UNIT $$$" }},
{ 0x6B8, new C{ T="4Byte", N = "_unit flags" }},
{ 0x6BC, new C{ T="4Byte", N = "_flags2" }},
{ 0x6C0, new C{ T="2Byte", N = "_default team" }},
{ 0x6C2, new C{ T="2Byte", N = "_constant sound volume" }},
{ 0x6C4, new C{ T="TagRef", N = "_hologram unit reference" }},
{ 0x6E0, new C{ T="Tagblock", N = "_campaign metagame bucket", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_flags" }},
{ 0x1, new C{ T="Byte", N = "_type" }},
{ 0x2, new C{ T="Byte", N = "_class" }},
{ 0x3, new C{ T="Unmapped", N = "_generated_pad6695" }},
{ 0x4, new C{ T="2Byte", N = "_point count" }},
}, S=6}},
{ 0x6F4, new C{ T="Tagblock", N = "_screen effects", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_screen effect" }},
}, S=28}},
{ 0x708, new C{ T="Float", N = "_camera stiffness" }},
{ 0x70C644, new C{ T="Comment", N = "_unit camera" }},
{ 0x70C645, new C{ T="Comment", N = "_Unit Camera" }},
{ 0x70C, new C{ T="2Byte", N = "_flags" }},
{ 0x70E, new C{ T="Unmapped", N = "_generated_padc79e" }},
{ 0x710646, new C{ T="Comment", N = "_" }},
{ 0x710, new C{ T="mmr3Hash", N = "_camera marker name" }},
{ 0x714, new C{ T="Float", N = "_pitch auto-level" }},
{ 0x718, new C{ T="Float", N = "_pitch range.min" }},
{ 0x71C, new C{ T="Float", N = "_pitch range.max" }},
{ 0x720, new C{ T="Tagblock", N = "_camera tracks", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_track" }},
{ 0x1C, new C{ T="TagRef", N = "_screen effect" }},
{ 0x38, new C{ T="Float", N = "_transition time in" }},
{ 0x3C, new C{ T="Float", N = "_transition time out" }},
}, S=64}},
{ 0x734, new C{ T="Float", N = "_pitch minimum spring" }},
{ 0x738, new C{ T="Float", N = "_pitch mmaximum spring" }},
{ 0x73C, new C{ T="Float", N = "_spring velocity" }},
{ 0x740, new C{ T="Float", N = "_look acceleration" }},
{ 0x744, new C{ T="Float", N = "_look deceleration" }},
{ 0x748, new C{ T="Float", N = "_look acc smoothing fraction" }},
{ 0x74C, new C{ T="Float", N = "_field of view bias" }},
{ 0x750647, new C{ T="Comment", N = "_camera obstruction" }},
{ 0x750, new C{ T="Float", N = "_cylinder fraction" }},
{ 0x754, new C{ T="Float", N = "_obstruction test angle" }},
{ 0x758, new C{ T="Float", N = "_obstruction max inward accel" }},
{ 0x75C, new C{ T="Float", N = "_obstruction max outward accel" }},
{ 0x760, new C{ T="Float", N = "_obstruction max velocity" }},
{ 0x764, new C{ T="Float", N = "_obstruction return delay" }},
{ 0x768, new C{ T="Tagblock", N = "_camera acceleration", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_maximum camera velocity" }},
{ 0x4648, new C{ T="Comment", N = "_" }},
{ 0x4649, new C{ T="Comment", N = "_forward/back" }},
{ 0x4, new C{ T="Byte", N = "_Input Variable" }},
{ 0x5, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x8650, new C{ T="Comment", N = "_mapping" }},
{ 0x8651, new C{ T="Comment", N = "_mapping" }},
{ 0x8652, new C{ T="Comment", N = "_" }},
{ 0x8, new C{ T="Unmapped", N = "_data" }},
{ 0x20653, new C{ T="Comment", N = "_" }},
{ 0x20, new C{ T="Float", N = "_maximum value" }},
{ 0x24, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x28, new C{ T="Float", N = "_camera scale (perpendicular)" }},
{ 0x2C654, new C{ T="Comment", N = "_left/right" }},
{ 0x2C, new C{ T="Byte", N = "_Input Variable" }},
{ 0x2D, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x30655, new C{ T="Comment", N = "_mapping" }},
{ 0x30656, new C{ T="Comment", N = "_mapping" }},
{ 0x30657, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Unmapped", N = "_data" }},
{ 0x48658, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="Float", N = "_maximum value" }},
{ 0x4C, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x50, new C{ T="Float", N = "_camera scale (perpendicular)" }},
{ 0x54659, new C{ T="Comment", N = "_up/down" }},
{ 0x54, new C{ T="Byte", N = "_Input Variable" }},
{ 0x55, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x58660, new C{ T="Comment", N = "_mapping" }},
{ 0x58661, new C{ T="Comment", N = "_mapping" }},
{ 0x58662, new C{ T="Comment", N = "_" }},
{ 0x58, new C{ T="Unmapped", N = "_data" }},
{ 0x70663, new C{ T="Comment", N = "_" }},
{ 0x70, new C{ T="Float", N = "_maximum value" }},
{ 0x74, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x78, new C{ T="Float", N = "_camera scale (perpendicular)" }},
}, S=124}},
{ 0x77C664, new C{ T="Comment", N = "_" }},
{ 0x77C665, new C{ T="Comment", N = "_sync action camera fields" }},
{ 0x77C666, new C{ T="Comment", N = "_sync action camera" }},
{ 0x77C667, new C{ T="Comment", N = "_Unit Camera" }},
{ 0x77C, new C{ T="2Byte", N = "_flags" }},
{ 0x77E, new C{ T="Unmapped", N = "_generated_padc79e" }},
{ 0x780668, new C{ T="Comment", N = "_" }},
{ 0x780, new C{ T="mmr3Hash", N = "_camera marker name" }},
{ 0x784, new C{ T="Float", N = "_pitch auto-level" }},
{ 0x788, new C{ T="Float", N = "_pitch range.min" }},
{ 0x78C, new C{ T="Float", N = "_pitch range.max" }},
{ 0x790, new C{ T="Tagblock", N = "_camera tracks", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_track" }},
{ 0x1C, new C{ T="TagRef", N = "_screen effect" }},
{ 0x38, new C{ T="Float", N = "_transition time in" }},
{ 0x3C, new C{ T="Float", N = "_transition time out" }},
}, S=64}},
{ 0x7A4, new C{ T="Float", N = "_pitch minimum spring" }},
{ 0x7A8, new C{ T="Float", N = "_pitch mmaximum spring" }},
{ 0x7AC, new C{ T="Float", N = "_spring velocity" }},
{ 0x7B0, new C{ T="Float", N = "_look acceleration" }},
{ 0x7B4, new C{ T="Float", N = "_look deceleration" }},
{ 0x7B8, new C{ T="Float", N = "_look acc smoothing fraction" }},
{ 0x7BC, new C{ T="Float", N = "_field of view bias" }},
{ 0x7C0669, new C{ T="Comment", N = "_camera obstruction" }},
{ 0x7C0, new C{ T="Float", N = "_cylinder fraction" }},
{ 0x7C4, new C{ T="Float", N = "_obstruction test angle" }},
{ 0x7C8, new C{ T="Float", N = "_obstruction max inward accel" }},
{ 0x7CC, new C{ T="Float", N = "_obstruction max outward accel" }},
{ 0x7D0, new C{ T="Float", N = "_obstruction max velocity" }},
{ 0x7D4, new C{ T="Float", N = "_obstruction return delay" }},
{ 0x7D8, new C{ T="Tagblock", N = "_camera acceleration", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_maximum camera velocity" }},
{ 0x4670, new C{ T="Comment", N = "_" }},
{ 0x4671, new C{ T="Comment", N = "_forward/back" }},
{ 0x4, new C{ T="Byte", N = "_Input Variable" }},
{ 0x5, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x8672, new C{ T="Comment", N = "_mapping" }},
{ 0x8673, new C{ T="Comment", N = "_mapping" }},
{ 0x8674, new C{ T="Comment", N = "_" }},
{ 0x8, new C{ T="Unmapped", N = "_data" }},
{ 0x20675, new C{ T="Comment", N = "_" }},
{ 0x20, new C{ T="Float", N = "_maximum value" }},
{ 0x24, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x28, new C{ T="Float", N = "_camera scale (perpendicular)" }},
{ 0x2C676, new C{ T="Comment", N = "_left/right" }},
{ 0x2C, new C{ T="Byte", N = "_Input Variable" }},
{ 0x2D, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x30677, new C{ T="Comment", N = "_mapping" }},
{ 0x30678, new C{ T="Comment", N = "_mapping" }},
{ 0x30679, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Unmapped", N = "_data" }},
{ 0x48680, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="Float", N = "_maximum value" }},
{ 0x4C, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x50, new C{ T="Float", N = "_camera scale (perpendicular)" }},
{ 0x54681, new C{ T="Comment", N = "_up/down" }},
{ 0x54, new C{ T="Byte", N = "_Input Variable" }},
{ 0x55, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x58682, new C{ T="Comment", N = "_mapping" }},
{ 0x58683, new C{ T="Comment", N = "_mapping" }},
{ 0x58684, new C{ T="Comment", N = "_" }},
{ 0x58, new C{ T="Unmapped", N = "_data" }},
{ 0x70685, new C{ T="Comment", N = "_" }},
{ 0x70, new C{ T="Float", N = "_maximum value" }},
{ 0x74, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x78, new C{ T="Float", N = "_camera scale (perpendicular)" }},
}, S=124}},
{ 0x7EC686, new C{ T="Comment", N = "_" }},
{ 0x7EC, new C{ T="mmr3Hash", N = "_unit markup name for weapon clas" }},
{ 0x7F0, new C{ T="TagRef", N = "_seat acceleration" }},
{ 0x80C, new C{ T="Float", N = "_pain major total damage threshol" }},
{ 0x810687, new C{ T="Comment", N = "_pings" }},
{ 0x810, new C{ T="Float", N = "_soft ping threshold" }},
{ 0x814, new C{ T="Float", N = "_soft ping interrupt time" }},
{ 0x818, new C{ T="Float", N = "_hard ping threshold" }},
{ 0x81C, new C{ T="Float", N = "_hard ping interrupt time" }},
{ 0x820, new C{ T="Float", N = "_soft death direction speed thres" }},
{ 0x824, new C{ T="Float", N = "_hard death threshold" }},
{ 0x828, new C{ T="Float", N = "_feign death threshold" }},
{ 0x82C, new C{ T="Float", N = "_feign death time" }},
{ 0x830, new C{ T="Float", N = "_pain screen duration" }},
{ 0x834, new C{ T="Float", N = "_pain screen region fade out dura" }},
{ 0x838, new C{ T="Float", N = "_pain screen region fade out weig" }},
{ 0x83C, new C{ T="Float", N = "_pain screen angle tolerance" }},
{ 0x840, new C{ T="Float", N = "_pain screen angle randomness" }},
{ 0x844, new C{ T="Float", N = "_defensive screen duration" }},
{ 0x848, new C{ T="Float", N = "_defensive screen scrub fallback " }},
{ 0x84C688, new C{ T="Comment", N = "_" }},
{ 0x84C, new C{ T="Float", N = "_distance of dive anim" }},
{ 0x850, new C{ T="Float", N = "_terminal velocity fall ratio" }},
{ 0x854689, new C{ T="Comment", N = "_stun" }},
{ 0x854, new C{ T="Float", N = "_stun movement penalty" }},
{ 0x858, new C{ T="Float", N = "_stun turning penalty" }},
{ 0x85C, new C{ T="Float", N = "_stun jumping penalty" }},
{ 0x860, new C{ T="Float", N = "_minimum stun time" }},
{ 0x864, new C{ T="Float", N = "_maximum stun time" }},
{ 0x868690, new C{ T="Comment", N = "_" }},
{ 0x868, new C{ T="Float", N = "_feign death chance" }},
{ 0x86C, new C{ T="Float", N = "_feign repeat chance" }},
{ 0x870, new C{ T="TagRef", N = "_spawned turret character" }},
{ 0x88C691, new C{ T="Comment", N = "_aiming/looking" }},
{ 0x88C, new C{ T="mmr3Hash", N = "_target aiming pivot marker name" }},
{ 0x890, new C{ T="Float", N = "_aiming velocity maximum" }},
{ 0x894, new C{ T="Float", N = "_aiming acceleration maximum" }},
{ 0x898, new C{ T="Float", N = "_casual aiming modifier" }},
{ 0x89C, new C{ T="Float", N = "_looking velocity maximum" }},
{ 0x8A0, new C{ T="Float", N = "_looking acceleration maximum" }},
{ 0x8A4, new C{ T="Float", N = "_Dropped ability object velocity" }},
{ 0x8A8692, new C{ T="Comment", N = "_" }},
{ 0x8A8, new C{ T="Float", N = "_object velocity maximum" }},
{ 0x8AC, new C{ T="mmr3Hash", N = "_right_hand_node" }},
{ 0x8B0, new C{ T="mmr3Hash", N = "_left_hand_node" }},
{ 0x8B4693, new C{ T="Comment", N = "_more damn nodes" }},
{ 0x8B4, new C{ T="mmr3Hash", N = "_preferred_gun_node" }},
{ 0x8B8694, new C{ T="Comment", N = "_" }},
{ 0x8B8, new C{ T="mmr3Hash", N = "_preferred_grenade_marker" }},
{ 0x8BC695, new C{ T="Comment", N = "_Weapon Specific Markers" }},
{ 0x8BC, new C{ T="Tagblock", N = "_weapon specific markers", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_complete weapon name" }},
{ 0x4, new C{ T="mmr3Hash", N = "_weapon class" }},
{ 0x8, new C{ T="mmr3Hash", N = "_weapon name" }},
{ 0xC696, new C{ T="Comment", N = "_" }},
{ 0xC, new C{ T="mmr3Hash", N = "_right hand marker" }},
{ 0x10697, new C{ T="Comment", N = "_" }},
{ 0x10, new C{ T="mmr3Hash", N = "_left hand marker" }},
}, S=20}},
{ 0x8D0698, new C{ T="Comment", N = "_melee damage" }},
{ 0x8D0, new C{ T="TagRef", N = "_melee damage" }},
{ 0x8EC, new C{ T="TagRef", N = "_native melee override" }},
{ 0x908699, new C{ T="Comment", N = "_your momma" }},
{ 0x908, new C{ T="TagRef", N = "_boarding melee damage" }},
{ 0x924, new C{ T="TagRef", N = "_boarding melee response" }},
{ 0x940, new C{ T="TagRef", N = "_eviction melee damage" }},
{ 0x95C, new C{ T="TagRef", N = "_eviction melee response" }},
{ 0x978, new C{ T="TagRef", N = "_landing melee damage" }},
{ 0x994, new C{ T="TagRef", N = "_flurry melee damage" }},
{ 0x9B0, new C{ T="TagRef", N = "_obstacle smash damage" }},
{ 0x9CC700, new C{ T="Comment", N = "_" }},
{ 0x9CC, new C{ T="2Byte", N = "_motion sensor blip style enemy" }},
{ 0x9CE, new C{ T="2Byte", N = "_motion sensor blip style friendl" }},
{ 0x9D0, new C{ T="2Byte", N = "_item owner size" }},
{ 0x9D2, new C{ T="Unmapped", N = "_generated_pad9055" }},
{ 0x9D4, new C{ T="Float", N = "_motion tracker range modifier" }},
{ 0x9D8, new C{ T="mmr3Hash", N = "_equipment variant name" }},
{ 0x9DC, new C{ T="mmr3Hash", N = "_grounded equipment variant name" }},
{ 0x9E0, new C{ T="Tagblock", N = "_Hud audio cues", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_Hud audio cues", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_sound" }},
{ 0x1C, new C{ T="4Byte", N = "_latched to" }},
{ 0x20, new C{ T="Float", N = "_scale" }},
}, S=36}},
{ 0x14701, new C{ T="Comment", N = "_health thresholds" }},
{ 0x14, new C{ T="Float", N = "_health minor" }},
{ 0x18, new C{ T="Float", N = "_health major" }},
{ 0x1C, new C{ T="Float", N = "_health critical" }},
{ 0x20702, new C{ T="Comment", N = "_" }},
{ 0x20703, new C{ T="Comment", N = "_shield thresholds" }},
{ 0x20, new C{ T="Float", N = "_shield minor" }},
{ 0x24, new C{ T="Float", N = "_shield major" }},
{ 0x28, new C{ T="Float", N = "_shield critical" }},
{ 0x2C704, new C{ T="Comment", N = "_" }},
}, S=44}},
{ 0x9F4, new C{ T="Tagblock", N = "_dialogue variants", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_variant number" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_padef78" }},
{ 0x4, new C{ T="TagRef", N = "_dialogue" }},
}, S=32}},
{ 0xA08705, new C{ T="Comment", N = "_standard grenade throw" }},
{ 0xA08, new C{ T="Float", N = "_grenade angle" }},
{ 0xA0C, new C{ T="Float", N = "_grenade angle max elevation" }},
{ 0xA10, new C{ T="Float", N = "_grenade angle min elevation" }},
{ 0xA14, new C{ T="Float", N = "_grenade velocity" }},
{ 0xA18, new C{ T="Float", N = "_grenade rotational velocity min.X" }},
{ 0xA1C, new C{ T="Float", N = "_grenade rotational velocity min.Y" }},
{ 0xA20, new C{ T="Float", N = "_grenade rotational velocity min.Z" }},
{ 0xA24, new C{ T="Float", N = "_greande rotational velocity max.X" }},
{ 0xA28, new C{ T="Float", N = "_greande rotational velocity max.Y" }},
{ 0xA2C, new C{ T="Float", N = "_greande rotational velocity max.Z" }},
{ 0xA30706, new C{ T="Comment", N = "_grenade throw speed scalar" }},
{ 0xA30707, new C{ T="Comment", N = "_grenade throw speed scalar" }},
{ 0xA30, new C{ T="Float", N = "_Base Value" }},
{ 0xA34, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xA38, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xA3C708, new C{ T="Comment", N = "_" }},
{ 0xA3C709, new C{ T="Comment", N = "_primary weapon toss" }},
{ 0xA3C, new C{ T="Float", N = "_weapon angle" }},
{ 0xA40, new C{ T="Float", N = "_weapon angle max elevation" }},
{ 0xA44, new C{ T="Float", N = "_weapon angle min elevation" }},
{ 0xA48, new C{ T="Float", N = "_weapon velocity" }},
{ 0xA4C, new C{ T="Float", N = "_weapon rotational velocity min.X" }},
{ 0xA50, new C{ T="Float", N = "_weapon rotational velocity min.Y" }},
{ 0xA54, new C{ T="Float", N = "_weapon rotational velocity min.Z" }},
{ 0xA58, new C{ T="Float", N = "_weapon rotational velocity max.X" }},
{ 0xA5C, new C{ T="Float", N = "_weapon rotational velocity max.Y" }},
{ 0xA60, new C{ T="Float", N = "_weapon rotational velocity max.Z" }},
{ 0xA64710, new C{ T="Comment", N = "_" }},
{ 0xA64, new C{ T="Tagblock", N = "_powered seats", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_driver powerup time" }},
{ 0x4, new C{ T="Float", N = "_driver powerdown time" }},
}, S=8}},
{ 0xA78, new C{ T="Tagblock", N = "_weapons", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_weapon" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_weapon name" }},
{ 0x20, new C{ T="mmr3Hash", N = "_variant name" }},
{ 0x24, new C{ T="4Byte", N = "_position" }},
{ 0x28, new C{ T="Byte", N = "_weapon flags" }},
{ 0x29, new C{ T="Unmapped", N = "_generated_paddad2" }},
{ 0x2C, new C{ T="Float", N = "_maximum firing cone angle" }},
{ 0x30, new C{ T="Float", N = "_max firing cone angle cosine" }},
{ 0x34, new C{ T="Float", N = "_minimum retarget time" }},
{ 0x38, new C{ T="Float", N = "_blind fire time" }},
{ 0x3C, new C{ T="Float", N = "_lead fraction" }},
{ 0x40, new C{ T="Float", N = "_engagement range.min" }},
{ 0x44, new C{ T="Float", N = "_engagement range.max" }},
{ 0x48, new C{ T="Float", N = "_proximity score multiplier" }},
{ 0x4C, new C{ T="Float", N = "_direction score multiplier" }},
{ 0x50, new C{ T="Float", N = "_attacker score multiplier" }},
{ 0x54, new C{ T="Float", N = "_targeting weight hologram" }},
{ 0x58, new C{ T="Float", N = "_targeting weight auto turret" }},
{ 0x5C, new C{ T="Float", N = "_primary fire delay from idle" }},
{ 0x60, new C{ T="Float", N = "_secondary fire delay from idle" }},
{ 0x64, new C{ T="Float", N = "_caution duration" }},
{ 0x68, new C{ T="Float", N = "_alert angular speed max" }},
{ 0x6C, new C{ T="Float", N = "_idle angular speed max" }},
{ 0x70, new C{ T="Float", N = "_targeting yaw min" }},
{ 0x74, new C{ T="Float", N = "_targeting yaw max" }},
{ 0x78, new C{ T="Float", N = "_targeting pitch min" }},
{ 0x7C, new C{ T="Float", N = "_targeting pitch max" }},
{ 0x80, new C{ T="Float", N = "_idle scanning yaw min" }},
{ 0x84, new C{ T="Float", N = "_idle scanning yaw max" }},
{ 0x88, new C{ T="Float", N = "_idle scanning pitch min" }},
{ 0x8C, new C{ T="Float", N = "_idle scanning pitch max" }},
{ 0x90, new C{ T="Float", N = "_idle scanning min interest dista" }},
{ 0x94, new C{ T="TagRef", N = "_alert mode effect" }},
{ 0xB0711, new C{ T="Comment", N = "_" }},
{ 0xB0, new C{ T="mmr3Hash", N = "_alert mode effect marker" }},
{ 0xB4, new C{ T="mmr3Hash", N = "_alert mode effect primary scale" }},
{ 0xB8, new C{ T="mmr3Hash", N = "_alert mode effect secondary scal" }},
{ 0xBC, new C{ T="Tagblock", N = "_sentry properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_behavior" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_pad4624" }},
{ 0x4, new C{ T="Float", N = "_sight cone angle" }},
{ 0x8, new C{ T="Float", N = "_sight cone cosine" }},
{ 0xC, new C{ T="Float", N = "_alert range" }},
{ 0x10, new C{ T="Float", N = "_attack range" }},
{ 0x14, new C{ T="Float", N = "_attack range score multiplier" }},
{ 0x18, new C{ T="Float", N = "_light vehicle range scale" }},
{ 0x1C, new C{ T="Float", N = "_heavy vehicle range scale" }},
{ 0x20, new C{ T="Float", N = "_flying vehicle range scale" }},
{ 0x24, new C{ T="Float", N = "_light vehicle score bonus" }},
{ 0x28, new C{ T="Float", N = "_heavy vehicle score bonus" }},
{ 0x2C, new C{ T="Float", N = "_flying vehicle score bonus" }},
{ 0x30, new C{ T="Float", N = "_current target score bonus" }},
{ 0x34, new C{ T="Float", N = "_startup time" }},
{ 0x38, new C{ T="Float", N = "_primary fire time" }},
{ 0x3C, new C{ T="Float", N = "_secondary fire time" }},
{ 0x40, new C{ T="TagRef", N = "_player entered alert range sound" }},
{ 0x5C, new C{ T="TagRef", N = "_player exited alert range sound" }},
{ 0x78712, new C{ T="Comment", N = "_Node graph events" }},
{ 0x78713, new C{ T="Comment", N = "_Events to fire on state transiti" }},
{ 0x78, new C{ T="Tagblock", N = "_Transition Events", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_From state" }},
{ 0x4, new C{ T="4Byte", N = "_To state" }},
{ 0x8, new C{ T="mmr3Hash", N = "_Event" }},
}, S=12}},
{ 0x8C714, new C{ T="Comment", N = "_" }},
}, S=140}},
{ 0xD0, new C{ T="Float", N = "_target camouflage threshold" }},
}, S=212}},
{ 0xA8C, new C{ T="Tagblock", N = "_target tracking", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_target tracking flags" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_pada390" }},
{ 0x4, new C{ T="Tagblock", N = "_tracking types", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_tracking type" }},
}, S=4}},
{ 0x18, new C{ T="Float", N = "_acquire time" }},
{ 0x1C, new C{ T="Float", N = "_grace time" }},
{ 0x20, new C{ T="Float", N = "_decay time" }},
{ 0x24, new C{ T="Byte", N = "_max target locks" }},
{ 0x25, new C{ T="Unmapped", N = "_generated_padbd71" }},
{ 0x28, new C{ T="TagRef", N = "_tracking sound" }},
{ 0x44, new C{ T="TagRef", N = "_locked sound" }},
}, S=96}},
{ 0xAA0, new C{ T="Tagblock", N = "_seats", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_flags" }},
{ 0x4, new C{ T="4Byte", N = "_secondary flags" }},
{ 0x8, new C{ T="4Byte", N = "_passenger seat designator" }},
{ 0xC, new C{ T="Byte", N = "_seat has infinite grenades of ty" }},
{ 0xD, new C{ T="Byte", N = "_seat selection priority" }},
{ 0xE, new C{ T="Unmapped", N = "_generated_pad811e" }},
{ 0x10, new C{ T="mmr3Hash", N = "_label text" }},
{ 0x14, new C{ T="String", N = "_label" }},
{ 0x34715, new C{ T="Comment", N = "_" }},
{ 0x34, new C{ T="mmr3Hash", N = "_marker name" }},
{ 0x38716, new C{ T="Comment", N = "_" }},
{ 0x38, new C{ T="mmr3Hash", N = "_entry marker(s) name" }},
{ 0x3C717, new C{ T="Comment", N = "_" }},
{ 0x3C, new C{ T="mmr3Hash", N = "_exit marker name" }},
{ 0x40718, new C{ T="Comment", N = "_" }},
{ 0x40, new C{ T="mmr3Hash", N = "_ui marker name" }},
{ 0x44, new C{ T="mmr3Hash", N = "_ui navpoint name" }},
{ 0x48719, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="mmr3Hash", N = "_boarding grenade marker" }},
{ 0x4C, new C{ T="mmr3Hash", N = "_boarding grenade string" }},
{ 0x50, new C{ T="mmr3Hash", N = "_boarding melee string" }},
{ 0x54, new C{ T="mmr3Hash", N = "_in-seat string" }},
{ 0x58, new C{ T="Tagblock", N = "_ai model targets", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_model target name" }},
}, S=4}},
{ 0x6C, new C{ T="Float", N = "_ping scale" }},
{ 0x70, new C{ T="Float", N = "_turnover time" }},
{ 0x74720, new C{ T="Comment", N = "_seat acceleration spring" }},
{ 0x74, new C{ T="TagRef", N = "_seat acceleration" }},
{ 0x90, new C{ T="Float", N = "_AI scariness" }},
{ 0x94, new C{ T="2Byte", N = "_ai seat type" }},
{ 0x96, new C{ T="2Byte", N = "_boarding seat" }},
{ 0x98, new C{ T="Tagblock", N = "_additional boarding seats", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_seat" }},
}, S=2}},
{ 0xAC721, new C{ T="Comment", N = "_seat switching" }},
{ 0xAC722, new C{ T="Comment", N = "_explanation" }},
{ 0xAC723, new C{ T="Comment", N = "_seat switching" }},
{ 0xAC724, new C{ T="Comment", N = "_up" }},
{ 0xAC725, new C{ T="Comment", N = "_up" }},
{ 0xAC, new C{ T="mmr3Hash", N = "_destinationSeat" }},
{ 0xB0, new C{ T="mmr3Hash", N = "_animation" }},
{ 0xB4, new C{ T="Float", N = "_seat switch cooldown time" }},
{ 0xB8726, new C{ T="Comment", N = "_" }},
{ 0xB8727, new C{ T="Comment", N = "_down" }},
{ 0xB8728, new C{ T="Comment", N = "_down" }},
{ 0xB8, new C{ T="mmr3Hash", N = "_destinationSeat" }},
{ 0xBC, new C{ T="mmr3Hash", N = "_animation" }},
{ 0xC0, new C{ T="Float", N = "_seat switch cooldown time" }},
{ 0xC4729, new C{ T="Comment", N = "_" }},
{ 0xC4730, new C{ T="Comment", N = "_left" }},
{ 0xC4731, new C{ T="Comment", N = "_left" }},
{ 0xC4, new C{ T="mmr3Hash", N = "_destinationSeat" }},
{ 0xC8, new C{ T="mmr3Hash", N = "_animation" }},
{ 0xCC, new C{ T="Float", N = "_seat switch cooldown time" }},
{ 0xD0732, new C{ T="Comment", N = "_" }},
{ 0xD0733, new C{ T="Comment", N = "_right" }},
{ 0xD0734, new C{ T="Comment", N = "_right" }},
{ 0xD0, new C{ T="mmr3Hash", N = "_destinationSeat" }},
{ 0xD4, new C{ T="mmr3Hash", N = "_animation" }},
{ 0xD8, new C{ T="Float", N = "_seat switch cooldown time" }},
{ 0xDC735, new C{ T="Comment", N = "_" }},
{ 0xDC736, new C{ T="Comment", N = "_" }},
{ 0xDC, new C{ T="Float", N = "_listener interpolation factor" }},
{ 0xE0, new C{ T="Float", N = "_listener orientation interpolati" }},
{ 0xE4, new C{ T="Float", N = "_listener blend towards locked ax" }},
{ 0xE8737, new C{ T="Comment", N = "_speed dependent turn rates" }},
{ 0xE8, new C{ T="Float", N = "_yaw rate bounds.min" }},
{ 0xEC, new C{ T="Float", N = "_yaw rate bounds.max" }},
{ 0xF0, new C{ T="Float", N = "_pitch rate bounds.min" }},
{ 0xF4, new C{ T="Float", N = "_pitch rate bounds.max" }},
{ 0xF8, new C{ T="Float", N = "_pitch interpolation time" }},
{ 0xFC, new C{ T="Float", N = "_min speed reference" }},
{ 0x100, new C{ T="Float", N = "_max speed reference" }},
{ 0x104, new C{ T="Float", N = "_speed exponent" }},
{ 0x108738, new C{ T="Comment", N = "_camera fields" }},
{ 0x108739, new C{ T="Comment", N = "_unit camera" }},
{ 0x108740, new C{ T="Comment", N = "_Unit Camera" }},
{ 0x108, new C{ T="2Byte", N = "_flags" }},
{ 0x10A, new C{ T="Unmapped", N = "_generated_padc79e" }},
{ 0x10C741, new C{ T="Comment", N = "_" }},
{ 0x10C, new C{ T="mmr3Hash", N = "_camera marker name" }},
{ 0x110, new C{ T="Float", N = "_pitch auto-level" }},
{ 0x114, new C{ T="Float", N = "_pitch range.min" }},
{ 0x118, new C{ T="Float", N = "_pitch range.max" }},
{ 0x11C, new C{ T="Tagblock", N = "_camera tracks", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_track" }},
{ 0x1C, new C{ T="TagRef", N = "_screen effect" }},
{ 0x38, new C{ T="Float", N = "_transition time in" }},
{ 0x3C, new C{ T="Float", N = "_transition time out" }},
}, S=64}},
{ 0x130, new C{ T="Float", N = "_pitch minimum spring" }},
{ 0x134, new C{ T="Float", N = "_pitch mmaximum spring" }},
{ 0x138, new C{ T="Float", N = "_spring velocity" }},
{ 0x13C, new C{ T="Float", N = "_look acceleration" }},
{ 0x140, new C{ T="Float", N = "_look deceleration" }},
{ 0x144, new C{ T="Float", N = "_look acc smoothing fraction" }},
{ 0x148, new C{ T="Float", N = "_field of view bias" }},
{ 0x14C742, new C{ T="Comment", N = "_camera obstruction" }},
{ 0x14C, new C{ T="Float", N = "_cylinder fraction" }},
{ 0x150, new C{ T="Float", N = "_obstruction test angle" }},
{ 0x154, new C{ T="Float", N = "_obstruction max inward accel" }},
{ 0x158, new C{ T="Float", N = "_obstruction max outward accel" }},
{ 0x15C, new C{ T="Float", N = "_obstruction max velocity" }},
{ 0x160, new C{ T="Float", N = "_obstruction return delay" }},
{ 0x164, new C{ T="Tagblock", N = "_camera acceleration", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_maximum camera velocity" }},
{ 0x4743, new C{ T="Comment", N = "_" }},
{ 0x4744, new C{ T="Comment", N = "_forward/back" }},
{ 0x4, new C{ T="Byte", N = "_Input Variable" }},
{ 0x5, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x8745, new C{ T="Comment", N = "_mapping" }},
{ 0x8746, new C{ T="Comment", N = "_mapping" }},
{ 0x8747, new C{ T="Comment", N = "_" }},
{ 0x8, new C{ T="Unmapped", N = "_data" }},
{ 0x20748, new C{ T="Comment", N = "_" }},
{ 0x20, new C{ T="Float", N = "_maximum value" }},
{ 0x24, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x28, new C{ T="Float", N = "_camera scale (perpendicular)" }},
{ 0x2C749, new C{ T="Comment", N = "_left/right" }},
{ 0x2C, new C{ T="Byte", N = "_Input Variable" }},
{ 0x2D, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x30750, new C{ T="Comment", N = "_mapping" }},
{ 0x30751, new C{ T="Comment", N = "_mapping" }},
{ 0x30752, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Unmapped", N = "_data" }},
{ 0x48753, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="Float", N = "_maximum value" }},
{ 0x4C, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x50, new C{ T="Float", N = "_camera scale (perpendicular)" }},
{ 0x54754, new C{ T="Comment", N = "_up/down" }},
{ 0x54, new C{ T="Byte", N = "_Input Variable" }},
{ 0x55, new C{ T="Unmapped", N = "_generated_padd066" }},
{ 0x58755, new C{ T="Comment", N = "_mapping" }},
{ 0x58756, new C{ T="Comment", N = "_mapping" }},
{ 0x58757, new C{ T="Comment", N = "_" }},
{ 0x58, new C{ T="Unmapped", N = "_data" }},
{ 0x70758, new C{ T="Comment", N = "_" }},
{ 0x70, new C{ T="Float", N = "_maximum value" }},
{ 0x74, new C{ T="Float", N = "_camera scale (axial)" }},
{ 0x78, new C{ T="Float", N = "_camera scale (perpendicular)" }},
}, S=124}},
{ 0x178759, new C{ T="Comment", N = "_" }},
{ 0x178, new C{ T="TagRef", N = "_hud screen reference" }},
{ 0x194, new C{ T="TagRef", N = "_alt hud screen reference" }},
{ 0x1B0, new C{ T="mmr3Hash", N = "_enter seat string" }},
{ 0x1B4, new C{ T="Tagblock", N = "_button callouts", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_text" }},
}, S=4}},
{ 0x1C8, new C{ T="Float", N = "_pitch minimum" }},
{ 0x1CC, new C{ T="Float", N = "_pitch maximum" }},
{ 0x1D0, new C{ T="Float", N = "_pitch minimum for AI operator" }},
{ 0x1D4, new C{ T="Float", N = "_pitch maximum for AI operator" }},
{ 0x1D8, new C{ T="Float", N = "_yaw minimum" }},
{ 0x1DC, new C{ T="Float", N = "_yaw maximum" }},
{ 0x1E0, new C{ T="Float", N = "_yaw minimum for AI operator" }},
{ 0x1E4, new C{ T="Float", N = "_yaw maximum for AI operator" }},
{ 0x1E8, new C{ T="TagRef", N = "_built-in gunner" }},
{ 0x204760, new C{ T="Comment", N = "_entry fields" }},
{ 0x204, new C{ T="Float", N = "_entry radius" }},
{ 0x208, new C{ T="Float", N = "_entry marker cone angle" }},
{ 0x20C, new C{ T="Float", N = "_entry marker facing angle" }},
{ 0x210, new C{ T="Float", N = "_maximum relative velocity" }},
{ 0x214, new C{ T="TagRef", N = "_equipment" }},
{ 0x230, new C{ T="Float", N = "_open time" }},
{ 0x234, new C{ T="Float", N = "_close time" }},
{ 0x238, new C{ T="mmr3Hash", N = "_open function name" }},
{ 0x23C, new C{ T="mmr3Hash", N = "_opening function name" }},
{ 0x240, new C{ T="mmr3Hash", N = "_closing function name" }},
{ 0x244, new C{ T="mmr3Hash", N = "_invisible seat region" }},
{ 0x248, new C{ T="4Byte", N = "_runtime invisible seat region in" }},
{ 0x24C761, new C{ T="Comment", N = "_seat death grab crate" }},
{ 0x24C, new C{ T="TagRef", N = "_seat death grab crate" }},
{ 0x268, new C{ T="mmr3Hash", N = "_Seat Selection String" }},
{ 0x26C, new C{ T="Float", N = "_bailout velocity" }},
{ 0x270762, new C{ T="Comment", N = "_Seat Ejection Fields" }},
{ 0x270, new C{ T="Float", N = "_maximum acceleration" }},
{ 0x274, new C{ T="Float", N = "_maximum centrifugal force" }},
}, S=632}},
{ 0xAB4, new C{ T="Float", N = "_maximum seat switch linear veloc" }},
{ 0xAB8, new C{ T="Float", N = "_maximum seat switch angular velo" }},
{ 0xABC, new C{ T="2Byte", N = "_empty mount behavior" }},
{ 0xABE, new C{ T="Unmapped", N = "_generated_pad7674" }},
{ 0xAC0763, new C{ T="Comment", N = "_open/close" }},
{ 0xAC0, new C{ T="Float", N = "_opening time" }},
{ 0xAC4, new C{ T="Float", N = "_closing time" }},
{ 0xAC8764, new C{ T="Comment", N = "_" }},
{ 0xAC8, new C{ T="TagRef", N = "_docking site" }},
{ 0xAE4765, new C{ T="Comment", N = "_Deprecated Unit Power Component " }},
{ 0xAE4, new C{ T="Tagblock", N = "_power component", B = new Dictionary<long, C>
{
{ 0x0766, new C{ T="Comment", N = "_Power Component System" }},
{ 0x0, new C{ T="Tagblock", N = "_power source configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="Float", N = "_capacity" }},
}, S=8}},
{ 0x14, new C{ T="Tagblock", N = "_power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_region name" }},
{ 0x4, new C{ T="4Byte", N = "_runtime region index" }},
{ 0x8767, new C{ T="Comment", N = "_Model Region Damage State Config" }},
{ 0x8, new C{ T="Tagblock", N = "_state configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_region state" }},
{ 0x2, new C{ T="2Byte", N = "_configuration" }},
}, S=4}},
}, S=28}},
{ 0x28768, new C{ T="Comment", N = "_communication node capacity" }},
{ 0x28, new C{ T="Tagblock", N = "_communication node power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_component" }},
}, S=2}},
{ 0x3C, new C{ T="mmr3Hash", N = "_communication node power modifie" }},
{ 0x40769, new C{ T="Comment", N = "_" }},
{ 0x40770, new C{ T="Comment", N = "_locomotion capacity" }},
{ 0x40, new C{ T="Tagblock", N = "_locomotion power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_component" }},
}, S=2}},
{ 0x54, new C{ T="mmr3Hash", N = "_locomotion power modifier id" }},
{ 0x58771, new C{ T="Comment", N = "_locomotion power modifier" }},
{ 0x58772, new C{ T="Comment", N = "_locomotion power modifier" }},
{ 0x58, new C{ T="Float", N = "_Base Value" }},
{ 0x5C, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0x60, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0x64773, new C{ T="Comment", N = "_" }},
{ 0x64774, new C{ T="Comment", N = "_weapon capacity" }},
{ 0x64, new C{ T="Tagblock", N = "_weapon power sources", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_component" }},
}, S=2}},
{ 0x78, new C{ T="mmr3Hash", N = "_weapon power modifier id" }},
{ 0x7C775, new C{ T="Comment", N = "_" }},
}, S=124}},
{ 0xAF8776, new C{ T="Comment", N = "_Boost" }},
{ 0xAF8, new C{ T="TagRef", N = "_boost definition" }},
{ 0xB14777, new C{ T="Comment", N = "_Deprecated Boost Fields.  Please" }},
{ 0xB14778, new C{ T="Comment", N = "_boost" }},
{ 0xB14, new C{ T="TagRef", N = "_boost collision damage" }},
{ 0xB30, new C{ T="4Byte", N = "_flags" }},
{ 0xB34, new C{ T="Float", N = "_boost peak power" }},
{ 0xB38, new C{ T="Float", N = "_boost rise time" }},
{ 0xB3C, new C{ T="Float", N = "_boost fall time" }},
{ 0xB40, new C{ T="Float", N = "_boost power per second" }},
{ 0xB44, new C{ T="Float", N = "_boost low warning threshold" }},
{ 0xB48, new C{ T="Float", N = "_recharge rate" }},
{ 0xB4C, new C{ T="Float", N = "_recharge delay" }},
{ 0xB50, new C{ T="Float", N = "_post boost weapon delay" }},
{ 0xB54779, new C{ T="Comment", N = "_trigger to boost" }},
{ 0xB54780, new C{ T="Comment", N = "_trigger to boost" }},
{ 0xB54781, new C{ T="Comment", N = "_" }},
{ 0xB54, new C{ T="Unmapped", N = "_data" }},
{ 0xB6C782, new C{ T="Comment", N = "_" }},
{ 0xB6C, new C{ T="4Byte", N = "_tutorial id" }},
{ 0xB70783, new C{ T="Comment", N = "_" }},
{ 0xB70, new C{ T="4Byte", N = "_tutorial weapon swap id" }},
{ 0xB74784, new C{ T="Comment", N = "_Lipsync" }},
{ 0xB74785, new C{ T="Comment", N = "_lipsync" }},
{ 0xB74, new C{ T="Float", N = "_attack weight" }},
{ 0xB78, new C{ T="Float", N = "_decay weight" }},
{ 0xB7C786, new C{ T="Comment", N = "_Exit and Detach" }},
{ 0xB7C, new C{ T="TagRef", N = "_exit and detach damage" }},
{ 0xB98, new C{ T="TagRef", N = "_exit and detach weapon" }},
{ 0xBB4, new C{ T="Tagblock", N = "_exit and detach variants", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_variant name" }},
{ 0x4, new C{ T="TagRef", N = "_exit and detach weapon" }},
{ 0x20, new C{ T="TagRef", N = "_exit and detach damage" }},
}, S=60}},
{ 0xBC8787, new C{ T="Comment", N = "_Vehicle Death Telemetry Rule" }},
{ 0xBC8, new C{ T="Byte", N = "_Death Telemetry Rule" }},
{ 0xBC9, new C{ T="Unmapped", N = "_generated_pad29bf" }},
{ 0xBCA788, new C{ T="Comment", N = "_Experience" }},
{ 0xBCA, new C{ T="2Byte", N = "_experience for kill" }},
{ 0xBCC, new C{ T="2Byte", N = "_experience for assist" }},
{ 0xBCE, new C{ T="Unmapped", N = "_generated_pad0fde" }},
{ 0xBD0, new C{ T="Float", N = "_bailout threshold" }},
{ 0xBD4, new C{ T="Float", N = "_iron sight weapon dampening" }},
{ 0xBD8789, new C{ T="Comment", N = "_Birthing" }},
{ 0xBD8790, new C{ T="Comment", N = "_birth" }},
{ 0xBD8, new C{ T="2Byte", N = "_seat" }},
{ 0xBDA, new C{ T="Unmapped", N = "_generated_pada2d2" }},
{ 0xBDC, new C{ T="mmr3Hash", N = "_birthing region" }},
{ 0xBE0791, new C{ T="Comment", N = "_" }},
{ 0xBE0, new C{ T="Float", N = "_procedural recoil scaler" }},
{ 0xBE4, new C{ T="TagRef", N = "_flinch system" }},
{ 0xC00792, new C{ T="Comment", N = "_Active Camouflage Malleable Prop" }},
{ 0xC00793, new C{ T="Comment", N = "_active camouflage properties" }},
{ 0xC00, new C{ T="Byte", N = "_camo level" }},
{ 0xC01, new C{ T="Byte", N = "_flags" }},
{ 0xC02, new C{ T="Unmapped", N = "_generated_pad06e4" }},
{ 0xC04794, new C{ T="Comment", N = "_grenade throw penalty" }},
{ 0xC04795, new C{ T="Comment", N = "_grenade throw penalty" }},
{ 0xC04, new C{ T="Float", N = "_Base Value" }},
{ 0xC08, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC0C, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC10796, new C{ T="Comment", N = "_interpolation time" }},
{ 0xC10797, new C{ T="Comment", N = "_interpolation time" }},
{ 0xC10, new C{ T="Float", N = "_Base Value" }},
{ 0xC14, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC18, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC1C798, new C{ T="Comment", N = "_melee penalty" }},
{ 0xC1C799, new C{ T="Comment", N = "_melee penalty" }},
{ 0xC1C, new C{ T="Float", N = "_Base Value" }},
{ 0xC20, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC24, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC28800, new C{ T="Comment", N = "_minimum dinged amount" }},
{ 0xC28801, new C{ T="Comment", N = "_minimum dinged amount" }},
{ 0xC28, new C{ T="Float", N = "_Base Value" }},
{ 0xC2C, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC30, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC34802, new C{ T="Comment", N = "_maximum duration" }},
{ 0xC34803, new C{ T="Comment", N = "_maximum duration" }},
{ 0xC34, new C{ T="Float", N = "_Base Value" }},
{ 0xC38, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC3C, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC40, new C{ T="TagRef", N = "_active camouflage energy gain ef" }},
{ 0xC5C804, new C{ T="Comment", N = "_damage properties" }},
{ 0xC5C805, new C{ T="Comment", N = "_Damage scaling malleable propert" }},
{ 0xC5C806, new C{ T="Comment", N = "_Weapon damage scalar" }},
{ 0xC5C807, new C{ T="Comment", N = "_Weapon damage scalar" }},
{ 0xC5C, new C{ T="Float", N = "_Base Value" }},
{ 0xC60, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC64, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC68808, new C{ T="Comment", N = "_Melee damage scalar" }},
{ 0xC68809, new C{ T="Comment", N = "_Melee damage scalar" }},
{ 0xC68, new C{ T="Float", N = "_Base Value" }},
{ 0xC6C, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC70, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC74810, new C{ T="Comment", N = "_Melee knockback scalar" }},
{ 0xC74811, new C{ T="Comment", N = "_Melee knockback scalar" }},
{ 0xC74, new C{ T="Float", N = "_Base Value" }},
{ 0xC78, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC7C, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC80812, new C{ T="Comment", N = "_Melee recovery speed scalar" }},
{ 0xC80813, new C{ T="Comment", N = "_Melee recovery speed scalar" }},
{ 0xC80, new C{ T="Float", N = "_Base Value" }},
{ 0xC84, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC88, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC8C814, new C{ T="Comment", N = "_Grenade damage scalar" }},
{ 0xC8C815, new C{ T="Comment", N = "_Grenade damage scalar" }},
{ 0xC8C, new C{ T="Float", N = "_Base Value" }},
{ 0xC90, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xC94, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xC98816, new C{ T="Comment", N = "_Grenade knockback scalar" }},
{ 0xC98817, new C{ T="Comment", N = "_Grenade knockback scalar" }},
{ 0xC98, new C{ T="Float", N = "_Base Value" }},
{ 0xC9C, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCA0, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCA4818, new C{ T="Comment", N = "_Body vampirism scalar" }},
{ 0xCA4819, new C{ T="Comment", N = "_Body vampirism scalar" }},
{ 0xCA4, new C{ T="Float", N = "_Base Value" }},
{ 0xCA8, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCAC, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCB0820, new C{ T="Comment", N = "_Shield vampirism scalar" }},
{ 0xCB0821, new C{ T="Comment", N = "_Shield vampirism scalar" }},
{ 0xCB0, new C{ T="Float", N = "_Base Value" }},
{ 0xCB4, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCB8, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCBC822, new C{ T="Comment", N = "_Area of effect radius scalar" }},
{ 0xCBC823, new C{ T="Comment", N = "_Area of effect radius scalar" }},
{ 0xCBC, new C{ T="Float", N = "_Base Value" }},
{ 0xCC0, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCC4, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCC8824, new C{ T="Comment", N = "_weapon properties" }},
{ 0xCC8825, new C{ T="Comment", N = "_Weapon trait malleable propertie" }},
{ 0xCC8826, new C{ T="Comment", N = "_weapon switch speed scalar" }},
{ 0xCC8827, new C{ T="Comment", N = "_weapon switch speed scalar" }},
{ 0xCC8, new C{ T="Float", N = "_Base Value" }},
{ 0xCCC, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCD0, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCD4828, new C{ T="Comment", N = "_weapon empty reload speed scalar" }},
{ 0xCD4829, new C{ T="Comment", N = "_weapon empty reload speed scalar" }},
{ 0xCD4, new C{ T="Float", N = "_Base Value" }},
{ 0xCD8, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCDC, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCE0830, new C{ T="Comment", N = "_weapon overheat loss scalar" }},
{ 0xCE0831, new C{ T="Comment", N = "_weapon overheat loss scalar" }},
{ 0xCE0, new C{ T="Float", N = "_Base Value" }},
{ 0xCE4, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCE8, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCEC832, new C{ T="Comment", N = "_weapon tactical reload speed sca" }},
{ 0xCEC833, new C{ T="Comment", N = "_weapon tactical reload speed sca" }},
{ 0xCEC, new C{ T="Float", N = "_Base Value" }},
{ 0xCF0, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xCF4, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xCF8834, new C{ T="Comment", N = "_weapon tactical cooling scalar" }},
{ 0xCF8835, new C{ T="Comment", N = "_weapon tactical cooling scalar" }},
{ 0xCF8, new C{ T="Float", N = "_Base Value" }},
{ 0xCFC, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xD00, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xD04836, new C{ T="Comment", N = "_movement speed (with turret) sca" }},
{ 0xD04837, new C{ T="Comment", N = "_movement speed (with turret) sca" }},
{ 0xD04, new C{ T="Float", N = "_Base Value" }},
{ 0xD08, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xD0C, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xD10838, new C{ T="Comment", N = "_emp properties" }},
{ 0xD10839, new C{ T="Comment", N = "_EMP malleable properties" }},
{ 0xD10840, new C{ T="Comment", N = "_EMP disable duration scalar " }},
{ 0xD10841, new C{ T="Comment", N = "_EMP disable duration scalar " }},
{ 0xD10, new C{ T="Float", N = "_Base Value" }},
{ 0xD14, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xD18, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xD1C842, new C{ T="Comment", N = "_EMP disable duration MP scalar " }},
{ 0xD1C843, new C{ T="Comment", N = "_EMP disable duration MP scalar " }},
{ 0xD1C, new C{ T="Float", N = "_Base Value" }},
{ 0xD20, new C{ T="Float", N = "_Value Bounds.min" }},
{ 0xD24, new C{ T="Float", N = "_Value Bounds.max" }},
{ 0xD28, new C{ T="TagRef", N = "_EMP unit modifiers" }},
{ 0xD44, new C{ T="TagRef", N = "_emp disabled effect" }},
{ 0xD60, new C{ T="mmr3Hash", N = "_recording unit type" }},
{ 0xD64844, new C{ T="Comment", N = "_" }},
{ 0xD64, new C{ T="TagRef", N = "_knockback collision damage overr" }},
{ 0xD80845, new C{ T="Comment", N = "_$$$ VEHICLE $$$" }},
{ 0xD80, new C{ T="TagRef", N = "_parent" }},
{ 0xD9C, new C{ T="4Byte", N = "_flags" }},
{ 0xDA0846, new C{ T="Comment", N = "_physics type" }},
{ 0xDA0847, new C{ T="Comment", N = "_physics types" }},
{ 0xDA0848, new C{ T="Comment", N = "_" }},
{ 0xDA0, new C{ T="Tagblock", N = "_type-human_tank", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_forward arc" }},
{ 0x4, new C{ T="Float", N = "_perpendicular forward arc" }},
{ 0x8, new C{ T="Float", N = "_flip window" }},
{ 0xC, new C{ T="Float", N = "_pegged fraction" }},
{ 0x10, new C{ T="Float", N = "_forward turn scale" }},
{ 0x14, new C{ T="Float", N = "_reverse turn scale" }},
{ 0x18849, new C{ T="Comment", N = "_forward differential" }},
{ 0x18, new C{ T="Float", N = "_maximum left differential" }},
{ 0x1C, new C{ T="Float", N = "_maximum right differential" }},
{ 0x20, new C{ T="Float", N = "_differential acceleration" }},
{ 0x24, new C{ T="Float", N = "_differential deceleration" }},
{ 0x28850, new C{ T="Comment", N = "_reverse differential" }},
{ 0x28, new C{ T="Float", N = "_maximum left reverse differentia" }},
{ 0x2C, new C{ T="Float", N = "_maximum right reverse differenti" }},
{ 0x30, new C{ T="Float", N = "_differential reverse acceleratio" }},
{ 0x34, new C{ T="Float", N = "_differential reverse deceleratio" }},
{ 0x38851, new C{ T="Comment", N = "_engine" }},
{ 0x38852, new C{ T="Comment", N = "_engine" }},
{ 0x38, new C{ T="Float", N = "_engine moment" }},
{ 0x3C, new C{ T="Float", N = "_engine min angular velocity" }},
{ 0x40, new C{ T="Float", N = "_engine max angular velocity" }},
{ 0x44, new C{ T="Tagblock", N = "_gears", B = new Dictionary<long, C>
{
{ 0x0853, new C{ T="Comment", N = "_loaded torque" }},
{ 0x0854, new C{ T="Comment", N = "_loaded torque curve" }},
{ 0x0855, new C{ T="Comment", N = "_function" }},
{ 0x0856, new C{ T="Comment", N = "_function" }},
{ 0x0857, new C{ T="Comment", N = "_function" }},
{ 0x0858, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="Unmapped", N = "_data" }},
{ 0x18859, new C{ T="Comment", N = "_" }},
{ 0x18860, new C{ T="Comment", N = "_cruising torque" }},
{ 0x18861, new C{ T="Comment", N = "_cruising torque curve" }},
{ 0x18862, new C{ T="Comment", N = "_function" }},
{ 0x18863, new C{ T="Comment", N = "_function" }},
{ 0x18864, new C{ T="Comment", N = "_function" }},
{ 0x18865, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x30866, new C{ T="Comment", N = "_" }},
{ 0x30867, new C{ T="Comment", N = "_gearing" }},
{ 0x30, new C{ T="Float", N = "_min time to upshift" }},
{ 0x34, new C{ T="Float", N = "_engine up-shift scale" }},
{ 0x38, new C{ T="Float", N = "_gear ratio" }},
{ 0x3C, new C{ T="Float", N = "_min time to downshift" }},
{ 0x40, new C{ T="Float", N = "_engine down-shift scale" }},
{ 0x44, new C{ T="Float", N = "_audio engine uprev rate" }},
{ 0x48, new C{ T="Float", N = "_audio engine shift up RPM value" }},
{ 0x4C, new C{ T="Float", N = "_audio engine downrev rate" }},
{ 0x50, new C{ T="Float", N = "_audio engine shift down RPM valu" }},
{ 0x54, new C{ T="TagRef", N = "_gear shift sound - shifting up" }},
{ 0x70, new C{ T="TagRef", N = "_gear shift sound - shifting down" }},
}, S=140}},
{ 0x58, new C{ T="TagRef", N = "_gear shift sound" }},
{ 0x74, new C{ T="Tagblock", N = "_load and cruise sound", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_load cruise function" }},
{ 0x4, new C{ T="4Byte", N = "_attachment index" }},
}, S=8}},
{ 0x88868, new C{ T="Comment", N = "_wheel circumferance" }},
{ 0x88, new C{ T="Float", N = "_wheel circumferance" }},
{ 0x8C, new C{ T="Float", N = "_gravity adjust" }},
{ 0x90869, new C{ T="Comment", N = "_New Tank Controls" }},
{ 0x90, new C{ T="Byte", N = "_control flags" }},
{ 0x91, new C{ T="Unmapped", N = "_generated_pad4b05" }},
{ 0x94, new C{ T="Float", N = "_at rest forward angle(purple)" }},
{ 0x98, new C{ T="Float", N = "_at rest reverse angle(violet)" }},
{ 0x9C, new C{ T="Float", N = "_at rest side on reverse angle cl" }},
{ 0xA0, new C{ T="Float", N = "_at rest side on reverse angle fu" }},
{ 0xA4, new C{ T="Float", N = "_at rest facing forward reverse a" }},
{ 0xA8, new C{ T="Float", N = "_at rest facing backward reverse " }},
{ 0xAC, new C{ T="Float", N = "_in motion opposing direction ang" }},
{ 0xB0, new C{ T="Float", N = "_in motion speed" }},
}, S=180}},
{ 0xDB4, new C{ T="Tagblock", N = "_type-human_jeep", B = new Dictionary<long, C>
{
{ 0x0870, new C{ T="Comment", N = "_steering control" }},
{ 0x0871, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x10872, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x18873, new C{ T="Comment", N = "_" }},
{ 0x18874, new C{ T="Comment", N = "_turning control" }},
{ 0x18875, new C{ T="Comment", N = "_turning" }},
{ 0x18, new C{ T="Float", N = "_maximum left turn" }},
{ 0x1C, new C{ T="Float", N = "_maximum right turn (negative)" }},
{ 0x20, new C{ T="Float", N = "_turn rate" }},
{ 0x24876, new C{ T="Comment", N = "_engine" }},
{ 0x24877, new C{ T="Comment", N = "_engine" }},
{ 0x24, new C{ T="Float", N = "_engine moment" }},
{ 0x28, new C{ T="Float", N = "_engine min angular velocity" }},
{ 0x2C, new C{ T="Float", N = "_engine max angular velocity" }},
{ 0x30, new C{ T="Tagblock", N = "_gears", B = new Dictionary<long, C>
{
{ 0x0878, new C{ T="Comment", N = "_loaded torque" }},
{ 0x0879, new C{ T="Comment", N = "_loaded torque curve" }},
{ 0x0880, new C{ T="Comment", N = "_function" }},
{ 0x0881, new C{ T="Comment", N = "_function" }},
{ 0x0882, new C{ T="Comment", N = "_function" }},
{ 0x0883, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="Unmapped", N = "_data" }},
{ 0x18884, new C{ T="Comment", N = "_" }},
{ 0x18885, new C{ T="Comment", N = "_cruising torque" }},
{ 0x18886, new C{ T="Comment", N = "_cruising torque curve" }},
{ 0x18887, new C{ T="Comment", N = "_function" }},
{ 0x18888, new C{ T="Comment", N = "_function" }},
{ 0x18889, new C{ T="Comment", N = "_function" }},
{ 0x18890, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x30891, new C{ T="Comment", N = "_" }},
{ 0x30892, new C{ T="Comment", N = "_gearing" }},
{ 0x30, new C{ T="Float", N = "_min time to upshift" }},
{ 0x34, new C{ T="Float", N = "_engine up-shift scale" }},
{ 0x38, new C{ T="Float", N = "_gear ratio" }},
{ 0x3C, new C{ T="Float", N = "_min time to downshift" }},
{ 0x40, new C{ T="Float", N = "_engine down-shift scale" }},
{ 0x44, new C{ T="Float", N = "_audio engine uprev rate" }},
{ 0x48, new C{ T="Float", N = "_audio engine shift up RPM value" }},
{ 0x4C, new C{ T="Float", N = "_audio engine downrev rate" }},
{ 0x50, new C{ T="Float", N = "_audio engine shift down RPM valu" }},
{ 0x54, new C{ T="TagRef", N = "_gear shift sound - shifting up" }},
{ 0x70, new C{ T="TagRef", N = "_gear shift sound - shifting down" }},
}, S=140}},
{ 0x44, new C{ T="TagRef", N = "_gear shift sound" }},
{ 0x60, new C{ T="Tagblock", N = "_load and cruise sound", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_load cruise function" }},
{ 0x4, new C{ T="4Byte", N = "_attachment index" }},
}, S=8}},
{ 0x74893, new C{ T="Comment", N = "_wheel circumferance" }},
{ 0x74, new C{ T="Float", N = "_wheel circumferance" }},
{ 0x78, new C{ T="Float", N = "_gravity adjust" }},
{ 0x7C, new C{ T="Float", N = "_antiroll torque factor" }},
{ 0x80894, new C{ T="Comment", N = "_air control torque function" }},
{ 0x80895, new C{ T="Comment", N = "_function" }},
{ 0x80896, new C{ T="Comment", N = "_function" }},
{ 0x80897, new C{ T="Comment", N = "_" }},
{ 0x80, new C{ T="Unmapped", N = "_data" }},
{ 0x98898, new C{ T="Comment", N = "_" }},
{ 0x98, new C{ T="Float", N = "_air control torque max" }},
{ 0x9C899, new C{ T="Comment", N = "_Wheel Identification" }},
{ 0x9C, new C{ T="2Byte", N = "_front left wheel" }},
{ 0x9E, new C{ T="2Byte", N = "_front right wheel" }},
{ 0xA0, new C{ T="2Byte", N = "_back left wheel" }},
{ 0xA2, new C{ T="2Byte", N = "_back right wheel" }},
{ 0xA4900, new C{ T="Comment", N = "_" }},
}, S=164}},
{ 0xDC8, new C{ T="Tagblock", N = "_type-human_plane", B = new Dictionary<long, C>
{
{ 0x0901, new C{ T="Comment", N = "_velocity control variables" }},
{ 0x0, new C{ T="Float", N = "_maximum forward speed" }},
{ 0x4, new C{ T="Float", N = "_maximum reverse speed" }},
{ 0x8, new C{ T="Float", N = "_speed acceleration" }},
{ 0xC, new C{ T="Float", N = "_speed deceleration" }},
{ 0x10, new C{ T="Float", N = "_speed accel against direction" }},
{ 0x14, new C{ T="Float", N = "_maximum forward speed during boo" }},
{ 0x18, new C{ T="Float", N = "_maximum left slide" }},
{ 0x1C, new C{ T="Float", N = "_maximum right slide" }},
{ 0x20, new C{ T="Float", N = "_slide acceleration" }},
{ 0x24, new C{ T="Float", N = "_slide deceleration" }},
{ 0x28, new C{ T="Float", N = "_slide accel against direction" }},
{ 0x2C, new C{ T="Float", N = "_maximum slide speed during boost" }},
{ 0x30, new C{ T="Float", N = "_maximum up rise" }},
{ 0x34, new C{ T="Float", N = "_maximum down rise" }},
{ 0x38, new C{ T="Float", N = "_rise acceleration" }},
{ 0x3C, new C{ T="Float", N = "_rise deceleration" }},
{ 0x40, new C{ T="Float", N = "_rise accel against direction" }},
{ 0x44, new C{ T="Float", N = "_maximum rise speed during boost" }},
{ 0x48902, new C{ T="Comment", N = "_human plane tuning variables" }},
{ 0x48, new C{ T="Float", N = "_flying torque scale" }},
{ 0x4C, new C{ T="Float", N = "_air friction deceleration" }},
{ 0x50, new C{ T="Float", N = "_thrust scale" }},
{ 0x54, new C{ T="Float", N = "_turn rate scale when boosting" }},
{ 0x58, new C{ T="Float", N = "_maximum roll" }},
{ 0x5C903, new C{ T="Comment", N = "_steering animation" }},
{ 0x5C904, new C{ T="Comment", N = "_steering animation and interpola" }},
{ 0x5C, new C{ T="Float", N = "_interpolation scale" }},
{ 0x60, new C{ T="Float", N = "_max angle" }},
}, S=100}},
{ 0xDDC, new C{ T="Tagblock", N = "_type-alien_scout", B = new Dictionary<long, C>
{
{ 0x0905, new C{ T="Comment", N = "_steering control" }},
{ 0x0906, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x10907, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x18908, new C{ T="Comment", N = "_" }},
{ 0x18909, new C{ T="Comment", N = "_velocity control variables" }},
{ 0x18, new C{ T="Float", N = "_maximum forward speed" }},
{ 0x1C, new C{ T="Float", N = "_maximum reverse speed" }},
{ 0x20, new C{ T="Float", N = "_speed acceleration" }},
{ 0x24, new C{ T="Float", N = "_speed deceleration" }},
{ 0x28, new C{ T="Float", N = "_maximum left slide" }},
{ 0x2C, new C{ T="Float", N = "_maximum right slide" }},
{ 0x30, new C{ T="Float", N = "_slide acceleration" }},
{ 0x34, new C{ T="Float", N = "_slide deceleration" }},
{ 0x38, new C{ T="Float", N = "_AI deceleration multiplier" }},
{ 0x3C, new C{ T="Float", N = "_slide accel against direction" }},
{ 0x40, new C{ T="Float", N = "_slide speed at top speed" }},
{ 0x44, new C{ T="Byte", N = "_flags" }},
{ 0x45, new C{ T="Unmapped", N = "_generated_pad1c8d" }},
{ 0x48, new C{ T="Float", N = "_drag coeficient" }},
{ 0x4C, new C{ T="Float", N = "_constant deceleration" }},
{ 0x50, new C{ T="Float", N = "_torque scale" }},
{ 0x54910, new C{ T="Comment", N = "_engine object function" }},
{ 0x54911, new C{ T="Comment", N = "_engine gravity function" }},
{ 0x54, new C{ T="mmr3Hash", N = "_object function damage region" }},
{ 0x58, new C{ T="Float", N = "_min anti gravity engine speed" }},
{ 0x5C, new C{ T="Float", N = "_max anti gravity engine speed" }},
{ 0x60, new C{ T="Float", N = "_engine speed acceleration" }},
{ 0x64, new C{ T="Float", N = "_maximum vehicle speed" }},
{ 0x68912, new C{ T="Comment", N = "_contrail object function" }},
{ 0x68913, new C{ T="Comment", N = "_contrail gravity function" }},
{ 0x68, new C{ T="mmr3Hash", N = "_object function damage region" }},
{ 0x6C, new C{ T="Float", N = "_min anti gravity engine speed" }},
{ 0x70, new C{ T="Float", N = "_max anti gravity engine speed" }},
{ 0x74, new C{ T="Float", N = "_engine speed acceleration" }},
{ 0x78, new C{ T="Float", N = "_maximum vehicle speed" }},
{ 0x7C914, new C{ T="Comment", N = "_engine rotation function" }},
{ 0x7C, new C{ T="Float", N = "_gear rotation speed.min" }},
{ 0x80, new C{ T="Float", N = "_gear rotation speed.max" }},
{ 0x84915, new C{ T="Comment", N = "_steering animation" }},
{ 0x84916, new C{ T="Comment", N = "_steering animation and interpola" }},
{ 0x84, new C{ T="Float", N = "_interpolation scale" }},
{ 0x88, new C{ T="Float", N = "_max angle" }},
{ 0x8C917, new C{ T="Comment", N = "_scout physics" }},
{ 0x8C, new C{ T="Byte", N = "_flags" }},
{ 0x8D, new C{ T="Unmapped", N = "_generated_padddec" }},
{ 0x90918, new C{ T="Comment", N = "_Air control" }},
{ 0x90, new C{ T="Float", N = "_air control angular velocity fac" }},
{ 0x94, new C{ T="Float", N = "_air control throttle acceleratio" }},
{ 0x98, new C{ T="Float", N = "_air control throttle bonus clamp" }},
{ 0x9C, new C{ T="Float", N = "_air control throttle bonus clamp" }},
{ 0xA0, new C{ T="Float", N = "_air control angular acceleration" }},
{ 0xA4, new C{ T="Float", N = "_air control auto level strength" }},
{ 0xA8919, new C{ T="Comment", N = "_Brake control" }},
{ 0xA8, new C{ T="Float", N = "_brake lift angular acceleration" }},
{ 0xAC, new C{ T="Float", N = "_brake lift acceleration up" }},
{ 0xB0, new C{ T="Float", N = "_brake airborne lift acceleration" }},
{ 0xB4, new C{ T="Float", N = "_brake airborne lift acceleration" }},
}, S=184}},
{ 0xDF0, new C{ T="Tagblock", N = "_type-alien_fighter", B = new Dictionary<long, C>
{
{ 0x0920, new C{ T="Comment", N = "_steering control" }},
{ 0x0921, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x10922, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x18923, new C{ T="Comment", N = "_" }},
{ 0x18924, new C{ T="Comment", N = "_turning control" }},
{ 0x18925, new C{ T="Comment", N = "_turning" }},
{ 0x18, new C{ T="Float", N = "_maximum left turn" }},
{ 0x1C, new C{ T="Float", N = "_maximum right turn (negative)" }},
{ 0x20, new C{ T="Float", N = "_turn rate" }},
{ 0x24926, new C{ T="Comment", N = "_velocity control variables" }},
{ 0x24, new C{ T="Float", N = "_maximum forward speed" }},
{ 0x28, new C{ T="Float", N = "_maximum reverse speed" }},
{ 0x2C, new C{ T="Float", N = "_speed acceleration" }},
{ 0x30, new C{ T="Float", N = "_speed deceleration" }},
{ 0x34, new C{ T="Float", N = "_boost maximum forward speed" }},
{ 0x38, new C{ T="Float", N = "_boost maximum reverse speed" }},
{ 0x3C, new C{ T="Float", N = "_boost speed acceleration" }},
{ 0x40, new C{ T="Float", N = "_boost speed deceleration" }},
{ 0x44, new C{ T="Float", N = "_maximum left slide" }},
{ 0x48, new C{ T="Float", N = "_maximum right slide" }},
{ 0x4C, new C{ T="Float", N = "_slide acceleration" }},
{ 0x50, new C{ T="Float", N = "_slide deceleration" }},
{ 0x54, new C{ T="Float", N = "_slide accel against direction" }},
{ 0x58927, new C{ T="Comment", N = "_torque scale" }},
{ 0x58, new C{ T="Float", N = "_flying torque scale" }},
{ 0x5C, new C{ T="Float", N = "_flying torque cap cusp" }},
{ 0x60, new C{ T="Float", N = "_flying torque cap exponent" }},
{ 0x64928, new C{ T="Comment", N = "_fixed gun offset" }},
{ 0x64, new C{ T="Float", N = "_fixed gun yaw" }},
{ 0x68, new C{ T="Float", N = "_fixed gun pitch" }},
{ 0x6C929, new C{ T="Comment", N = "_alien fighter trick variables" }},
{ 0x6C, new C{ T="Float", N = "_maximum trick frequency" }},
{ 0x70, new C{ T="Float", N = "_loop trick duration" }},
{ 0x74, new C{ T="Float", N = "_roll trick duration" }},
{ 0x78930, new C{ T="Comment", N = "_alien fighter fake flight contro" }},
{ 0x78, new C{ T="Float", N = "_zero gravity speed" }},
{ 0x7C, new C{ T="Float", N = "_full gravity speed" }},
{ 0x80, new C{ T="Float", N = "_strafe boost scale" }},
{ 0x84, new C{ T="Float", N = "_off stick deceleration scale" }},
{ 0x88, new C{ T="Float", N = "_cruising throttle" }},
{ 0x8C, new C{ T="Float", N = "_dive speed scale" }},
}, S=144}},
{ 0xE04, new C{ T="Tagblock", N = "_type-turret", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_flags" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_pad4605" }},
{ 0x4, new C{ T="mmr3Hash", N = "_physical yaw node" }},
{ 0x8, new C{ T="mmr3Hash", N = "_physical pitch node" }},
{ 0xC, new C{ T="mmr3Hash", N = "_physical elevate node" }},
{ 0x10, new C{ T="Float", N = "_operating elevation angle" }},
}, S=20}},
{ 0xE18, new C{ T="Tagblock", N = "_type-vtol", B = new Dictionary<long, C>
{
{ 0x0931, new C{ T="Comment", N = "_turning control" }},
{ 0x0932, new C{ T="Comment", N = "_turning" }},
{ 0x0, new C{ T="Float", N = "_maximum left turn" }},
{ 0x4, new C{ T="Float", N = "_maximum right turn (negative)" }},
{ 0x8, new C{ T="Float", N = "_turn rate" }},
{ 0xC933, new C{ T="Comment", N = "_" }},
{ 0xC, new C{ T="mmr3Hash", N = "_left lift marker" }},
{ 0x10934, new C{ T="Comment", N = "_" }},
{ 0x10, new C{ T="mmr3Hash", N = "_right lift marker" }},
{ 0x14935, new C{ T="Comment", N = "_" }},
{ 0x14, new C{ T="mmr3Hash", N = "_thrust marker" }},
{ 0x18936, new C{ T="Comment", N = "_trigger to throttle" }},
{ 0x18937, new C{ T="Comment", N = "_function" }},
{ 0x18938, new C{ T="Comment", N = "_function" }},
{ 0x18939, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x30940, new C{ T="Comment", N = "_" }},
{ 0x30941, new C{ T="Comment", N = "_descent to boost" }},
{ 0x30942, new C{ T="Comment", N = "_descent to boost" }},
{ 0x30943, new C{ T="Comment", N = "_function" }},
{ 0x30944, new C{ T="Comment", N = "_function" }},
{ 0x30945, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Unmapped", N = "_data" }},
{ 0x48946, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="Float", N = "_max downward speed" }},
{ 0x4C947, new C{ T="Comment", N = "_minimum and maximum up accelerat" }},
{ 0x4C, new C{ T="Float", N = "_maximum up acceleration" }},
{ 0x50, new C{ T="Float", N = "_maximum down acceleration" }},
{ 0x54, new C{ T="Float", N = "_vertical deceleration time" }},
{ 0x58948, new C{ T="Comment", N = "_lift arm pivot" }},
{ 0x58, new C{ T="Float", N = "_lift arm pivot length" }},
{ 0x5C, new C{ T="Float", N = "_lift arm pivot length negative z" }},
{ 0x60949, new C{ T="Comment", N = "_turn, left and forward accelerat" }},
{ 0x60, new C{ T="Float", N = "_maximum turn acceleration" }},
{ 0x64, new C{ T="Float", N = "_turn acceleration gain" }},
{ 0x68950, new C{ T="Comment", N = "_interpolation parameters" }},
{ 0x68, new C{ T="Float", N = "_interpolation speed domain" }},
{ 0x6C951, new C{ T="Comment", N = "_function explanation" }},
{ 0x6C952, new C{ T="Comment", N = "_SPEED_TROTTLE_CEILING()" }},
{ 0x6C953, new C{ T="Comment", N = "_function" }},
{ 0x6C954, new C{ T="Comment", N = "_function" }},
{ 0x6C955, new C{ T="Comment", N = "_" }},
{ 0x6C, new C{ T="Unmapped", N = "_data" }},
{ 0x84956, new C{ T="Comment", N = "_" }},
{ 0x84957, new C{ T="Comment", N = "_INTERPOLATION_ACC()" }},
{ 0x84958, new C{ T="Comment", N = "_function" }},
{ 0x84959, new C{ T="Comment", N = "_function" }},
{ 0x84960, new C{ T="Comment", N = "_" }},
{ 0x84, new C{ T="Unmapped", N = "_data" }},
{ 0x9C961, new C{ T="Comment", N = "_" }},
{ 0x9C962, new C{ T="Comment", N = "_A_B_INTERPOLATION() interpolatio" }},
{ 0x9C963, new C{ T="Comment", N = "_function" }},
{ 0x9C964, new C{ T="Comment", N = "_function" }},
{ 0x9C965, new C{ T="Comment", N = "_" }},
{ 0x9C, new C{ T="Unmapped", N = "_data" }},
{ 0xB4966, new C{ T="Comment", N = "_" }},
{ 0xB4, new C{ T="Tagblock", N = "_speed interpolated parameters", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_rotor damping" }},
{ 0x4, new C{ T="Float", N = "_maximum left acceleration" }},
{ 0x8, new C{ T="Float", N = "_maximum forward acceleration" }},
{ 0xC, new C{ T="Float", N = "_drag coeficient" }},
{ 0x10, new C{ T="Float", N = "_constant deceleration" }},
{ 0x14, new C{ T="Float", N = "_magic angular acc exp" }},
{ 0x18, new C{ T="Float", N = "_magic angular acc scale" }},
{ 0x1C, new C{ T="Float", N = "_magic angular acc k" }},
}, S=32}},
{ 0xC8, new C{ T="Float", N = "_lift angles acc" }},
{ 0xCC, new C{ T="Float", N = "_alt. lock offset coefficient" }},
{ 0xD0, new C{ T="Float", N = "_alt. lock velocity coefficient" }},
{ 0xD4967, new C{ T="Comment", N = "_prop rotation" }},
{ 0xD4, new C{ T="Float", N = "_prop rotation speed.min" }},
{ 0xD8, new C{ T="Float", N = "_prop rotation speed.max" }},
{ 0xDC968, new C{ T="Comment", N = "_landing" }},
{ 0xDC, new C{ T="Float", N = "_landing time" }},
{ 0xE0, new C{ T="Float", N = "_takeoff time" }},
{ 0xE4, new C{ T="Float", N = "_landing linear velocity" }},
{ 0xE8, new C{ T="Float", N = "_landing angular velocity" }},
{ 0xEC, new C{ T="Float", N = "_auto take off height offset" }},
{ 0xF0, new C{ T="2Byte", N = "_flags" }},
{ 0xF2, new C{ T="Unmapped", N = "_generated_padac32" }},
{ 0xF4969, new C{ T="Comment", N = "_dodge boost" }},
{ 0xF4970, new C{ T="Comment", N = "_dodge boost force application" }},
{ 0xF4971, new C{ T="Comment", N = "_function" }},
{ 0xF4972, new C{ T="Comment", N = "_function" }},
{ 0xF4973, new C{ T="Comment", N = "_" }},
{ 0xF4, new C{ T="Unmapped", N = "_data" }},
{ 0x10C974, new C{ T="Comment", N = "_" }},
{ 0x10C975, new C{ T="Comment", N = "_dodge boost velocity" }},
{ 0x10C976, new C{ T="Comment", N = "_function" }},
{ 0x10C977, new C{ T="Comment", N = "_function" }},
{ 0x10C978, new C{ T="Comment", N = "_" }},
{ 0x10C, new C{ T="Unmapped", N = "_data" }},
{ 0x124979, new C{ T="Comment", N = "_" }},
{ 0x124, new C{ T="Float", N = "_dodge boost trigger time" }},
{ 0x128, new C{ T="Float", N = "_dodge boost duration" }},
{ 0x12C, new C{ T="Float", N = "_dodge boost recharge duration" }},
{ 0x130, new C{ T="TagRef", N = "_dodge boost damage response" }},
{ 0x14C980, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x14C981, new C{ T="Comment", N = "_steering control" }},
{ 0x14C982, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x14C, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x150, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x154, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0x158, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x15C983, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x15C, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x160, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x164984, new C{ T="Comment", N = "_" }},
}, S=356}},
{ 0xE2C, new C{ T="Tagblock", N = "_type-chopper", B = new Dictionary<long, C>
{
{ 0x0985, new C{ T="Comment", N = "_steering control" }},
{ 0x0986, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x10987, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x18988, new C{ T="Comment", N = "_" }},
{ 0x18989, new C{ T="Comment", N = "_turning control" }},
{ 0x18990, new C{ T="Comment", N = "_turning" }},
{ 0x18, new C{ T="Float", N = "_maximum left turn" }},
{ 0x1C, new C{ T="Float", N = "_maximum right turn (negative)" }},
{ 0x20, new C{ T="Float", N = "_turn rate" }},
{ 0x24991, new C{ T="Comment", N = "_engine" }},
{ 0x24992, new C{ T="Comment", N = "_engine" }},
{ 0x24, new C{ T="Float", N = "_engine moment" }},
{ 0x28, new C{ T="Float", N = "_engine min angular velocity" }},
{ 0x2C, new C{ T="Float", N = "_engine max angular velocity" }},
{ 0x30, new C{ T="Tagblock", N = "_gears", B = new Dictionary<long, C>
{
{ 0x0993, new C{ T="Comment", N = "_loaded torque" }},
{ 0x0994, new C{ T="Comment", N = "_loaded torque curve" }},
{ 0x0995, new C{ T="Comment", N = "_function" }},
{ 0x0996, new C{ T="Comment", N = "_function" }},
{ 0x0997, new C{ T="Comment", N = "_function" }},
{ 0x0998, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="Unmapped", N = "_data" }},
{ 0x18999, new C{ T="Comment", N = "_" }},
{ 0x181000, new C{ T="Comment", N = "_cruising torque" }},
{ 0x181001, new C{ T="Comment", N = "_cruising torque curve" }},
{ 0x181002, new C{ T="Comment", N = "_function" }},
{ 0x181003, new C{ T="Comment", N = "_function" }},
{ 0x181004, new C{ T="Comment", N = "_function" }},
{ 0x181005, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x301006, new C{ T="Comment", N = "_" }},
{ 0x301007, new C{ T="Comment", N = "_gearing" }},
{ 0x30, new C{ T="Float", N = "_min time to upshift" }},
{ 0x34, new C{ T="Float", N = "_engine up-shift scale" }},
{ 0x38, new C{ T="Float", N = "_gear ratio" }},
{ 0x3C, new C{ T="Float", N = "_min time to downshift" }},
{ 0x40, new C{ T="Float", N = "_engine down-shift scale" }},
{ 0x44, new C{ T="Float", N = "_audio engine uprev rate" }},
{ 0x48, new C{ T="Float", N = "_audio engine shift up RPM value" }},
{ 0x4C, new C{ T="Float", N = "_audio engine downrev rate" }},
{ 0x50, new C{ T="Float", N = "_audio engine shift down RPM valu" }},
{ 0x54, new C{ T="TagRef", N = "_gear shift sound - shifting up" }},
{ 0x70, new C{ T="TagRef", N = "_gear shift sound - shifting down" }},
}, S=140}},
{ 0x44, new C{ T="TagRef", N = "_gear shift sound" }},
{ 0x60, new C{ T="Tagblock", N = "_load and cruise sound", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_load cruise function" }},
{ 0x4, new C{ T="4Byte", N = "_attachment index" }},
}, S=8}},
{ 0x74, new C{ T="Float", N = "_wheel circumferance" }},
{ 0x78, new C{ T="Float", N = "_pivot offset.X" }},
{ 0x7C, new C{ T="Float", N = "_pivot offset.Y" }},
{ 0x80, new C{ T="Float", N = "_pivot offset.Z" }},
{ 0x841008, new C{ T="Comment", N = "_Yaw Correction" }},
{ 0x84, new C{ T="Float", N = "_yaw correction coefficient 2" }},
{ 0x88, new C{ T="Float", N = "_yaw correction coefficient 1" }},
{ 0x8C, new C{ T="Float", N = "_yaw correction coefficient 0" }},
{ 0x90, new C{ T="Float", N = "_bank to slide ratio" }},
{ 0x94, new C{ T="Float", N = "_bank slide exponent" }},
{ 0x98, new C{ T="Float", N = "_bank to turn ratio" }},
{ 0x9C, new C{ T="Float", N = "_bank turn exponent" }},
{ 0xA0, new C{ T="Float", N = "_bank fraction" }},
{ 0xA4, new C{ T="Float", N = "_bank rate" }},
{ 0xA8, new C{ T="Float", N = "_wheel accel" }},
{ 0xAC, new C{ T="Float", N = "_gyroscopic damping" }},
}, S=176}},
{ 0xE40, new C{ T="Tagblock", N = "_type-guardian", B = new Dictionary<long, C>
{
{ 0x01009, new C{ T="Comment", N = "_steering control" }},
{ 0x01010, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x101011, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x181012, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Float", N = "_maximum forward speed" }},
{ 0x1C, new C{ T="Float", N = "_maximum reverse speed" }},
{ 0x20, new C{ T="Float", N = "_speed acceleration" }},
{ 0x24, new C{ T="Float", N = "_speed deceleration" }},
{ 0x28, new C{ T="Float", N = "_maximum left slide" }},
{ 0x2C, new C{ T="Float", N = "_maximum right slide" }},
{ 0x30, new C{ T="Float", N = "_slide acceleration" }},
{ 0x34, new C{ T="Float", N = "_slide deceleration" }},
{ 0x38, new C{ T="Float", N = "_torque scale" }},
{ 0x3C, new C{ T="Float", N = "_anti-gravity force z-offset" }},
}, S=64}},
{ 0xE54, new C{ T="Tagblock", N = "_type-jackal-glider", B = new Dictionary<long, C>
{
{ 0x01013, new C{ T="Comment", N = "_steering control" }},
{ 0x01014, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x101015, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x181016, new C{ T="Comment", N = "_" }},
{ 0x181017, new C{ T="Comment", N = "_velocity control variables" }},
{ 0x18, new C{ T="Float", N = "_maximum forward speed" }},
{ 0x1C, new C{ T="Float", N = "_maximum reverse speed" }},
{ 0x20, new C{ T="Float", N = "_speed acceleration" }},
{ 0x24, new C{ T="Float", N = "_speed deceleration" }},
{ 0x28, new C{ T="Float", N = "_maximum left slide" }},
{ 0x2C, new C{ T="Float", N = "_maximum right slide" }},
{ 0x30, new C{ T="Float", N = "_slide acceleration" }},
{ 0x34, new C{ T="Float", N = "_slide deceleration" }},
{ 0x38, new C{ T="Float", N = "_torque scale" }},
{ 0x3C1018, new C{ T="Comment", N = "_engine object function" }},
{ 0x3C1019, new C{ T="Comment", N = "_engine object function" }},
{ 0x3C, new C{ T="mmr3Hash", N = "_object function damage region" }},
{ 0x40, new C{ T="Float", N = "_min anti gravity engine speed" }},
{ 0x44, new C{ T="Float", N = "_max anti gravity engine speed" }},
{ 0x48, new C{ T="Float", N = "_engine speed acceleration" }},
{ 0x4C, new C{ T="Float", N = "_maximum vehicle speed" }},
{ 0x501020, new C{ T="Comment", N = "_contrail object function" }},
{ 0x501021, new C{ T="Comment", N = "_contrail object function" }},
{ 0x50, new C{ T="mmr3Hash", N = "_object function damage region" }},
{ 0x54, new C{ T="Float", N = "_min anti gravity engine speed" }},
{ 0x58, new C{ T="Float", N = "_max anti gravity engine speed" }},
{ 0x5C, new C{ T="Float", N = "_engine speed acceleration" }},
{ 0x60, new C{ T="Float", N = "_maximum vehicle speed" }},
{ 0x641022, new C{ T="Comment", N = "_steering animation" }},
{ 0x641023, new C{ T="Comment", N = "_steering animation and interpola" }},
{ 0x64, new C{ T="Float", N = "_interpolation scale" }},
{ 0x68, new C{ T="Float", N = "_max angle" }},
{ 0x6C, new C{ T="Float", N = "_flying velocity threshold" }},
{ 0x70, new C{ T="Float", N = "_flying look threshold" }},
{ 0x74, new C{ T="Float", N = "_flying hover threshold" }},
{ 0x78, new C{ T="Float", N = "_grounded hover threshold" }},
{ 0x7C, new C{ T="Float", N = "_landing grounded time" }},
{ 0x80, new C{ T="Float", N = "_ground turn radius" }},
{ 0x84, new C{ T="Float", N = "_ground acceleration" }},
{ 0x881024, new C{ T="Comment", N = "_wing lift" }},
{ 0x88, new C{ T="Float", N = "_wing lift q" }},
{ 0x8C, new C{ T="Float", N = "_wing lift k" }},
{ 0x90, new C{ T="Float", N = "_wing lift function ceiling" }},
{ 0x94, new C{ T="Float", N = "_aileron to angular acceleartion " }},
{ 0x98, new C{ T="Float", N = "_aileron yaw tilt angle" }},
{ 0x9C1025, new C{ T="Comment", N = "_wing" }},
{ 0x9C1026, new C{ T="Comment", N = "_wing surface" }},
{ 0x9C, new C{ T="Byte", N = "_offset axis" }},
{ 0x9D, new C{ T="Byte", N = "_pivot axis" }},
{ 0x9E, new C{ T="Byte", N = "_rotation axis" }},
{ 0x9F, new C{ T="Byte", N = "_normal axis" }},
{ 0xA0, new C{ T="Float", N = "_offset distance" }},
{ 0xA4, new C{ T="Float", N = "_pivot distance" }},
{ 0xA81027, new C{ T="Comment", N = "_normal force" }},
{ 0xA8, new C{ T="Float", N = "_q" }},
{ 0xAC, new C{ T="Float", N = "_k" }},
{ 0xB0, new C{ T="Float", N = "_function ceiling" }},
{ 0xB4, new C{ T="Float", N = "_maximum angle" }},
{ 0xB8, new C{ T="Float", N = "_render debug radius" }},
{ 0xBC1028, new C{ T="Comment", N = "_ailerons" }},
{ 0xBC1029, new C{ T="Comment", N = "_aileron surface" }},
{ 0xBC, new C{ T="Byte", N = "_offset axis" }},
{ 0xBD, new C{ T="Byte", N = "_pivot axis" }},
{ 0xBE, new C{ T="Byte", N = "_rotation axis" }},
{ 0xBF, new C{ T="Byte", N = "_normal axis" }},
{ 0xC0, new C{ T="Float", N = "_offset distance" }},
{ 0xC4, new C{ T="Float", N = "_pivot distance" }},
{ 0xC81030, new C{ T="Comment", N = "_normal force" }},
{ 0xC8, new C{ T="Float", N = "_q" }},
{ 0xCC, new C{ T="Float", N = "_k" }},
{ 0xD0, new C{ T="Float", N = "_function ceiling" }},
{ 0xD4, new C{ T="Float", N = "_maximum angle" }},
{ 0xD8, new C{ T="Float", N = "_render debug radius" }},
{ 0xDC1031, new C{ T="Comment", N = "_elevator" }},
{ 0xDC1032, new C{ T="Comment", N = "_elevator surface" }},
{ 0xDC, new C{ T="Byte", N = "_offset axis" }},
{ 0xDD, new C{ T="Byte", N = "_pivot axis" }},
{ 0xDE, new C{ T="Byte", N = "_rotation axis" }},
{ 0xDF, new C{ T="Byte", N = "_normal axis" }},
{ 0xE0, new C{ T="Float", N = "_offset distance" }},
{ 0xE4, new C{ T="Float", N = "_pivot distance" }},
{ 0xE81033, new C{ T="Comment", N = "_normal force" }},
{ 0xE8, new C{ T="Float", N = "_q" }},
{ 0xEC, new C{ T="Float", N = "_k" }},
{ 0xF0, new C{ T="Float", N = "_function ceiling" }},
{ 0xF4, new C{ T="Float", N = "_maximum angle" }},
{ 0xF8, new C{ T="Float", N = "_render debug radius" }},
{ 0xFC1034, new C{ T="Comment", N = "_tail" }},
{ 0xFC1035, new C{ T="Comment", N = "_tail surface" }},
{ 0xFC, new C{ T="Byte", N = "_offset axis" }},
{ 0xFD, new C{ T="Byte", N = "_pivot axis" }},
{ 0xFE, new C{ T="Byte", N = "_rotation axis" }},
{ 0xFF, new C{ T="Byte", N = "_normal axis" }},
{ 0x100, new C{ T="Float", N = "_offset distance" }},
{ 0x104, new C{ T="Float", N = "_pivot distance" }},
{ 0x1081036, new C{ T="Comment", N = "_normal force" }},
{ 0x108, new C{ T="Float", N = "_q" }},
{ 0x10C, new C{ T="Float", N = "_k" }},
{ 0x110, new C{ T="Float", N = "_function ceiling" }},
{ 0x114, new C{ T="Float", N = "_maximum angle" }},
{ 0x118, new C{ T="Float", N = "_render debug radius" }},
{ 0x11C1037, new C{ T="Comment", N = "_rudder" }},
{ 0x11C1038, new C{ T="Comment", N = "_rudder surface" }},
{ 0x11C, new C{ T="Byte", N = "_offset axis" }},
{ 0x11D, new C{ T="Byte", N = "_pivot axis" }},
{ 0x11E, new C{ T="Byte", N = "_rotation axis" }},
{ 0x11F, new C{ T="Byte", N = "_normal axis" }},
{ 0x120, new C{ T="Float", N = "_offset distance" }},
{ 0x124, new C{ T="Float", N = "_pivot distance" }},
{ 0x1281039, new C{ T="Comment", N = "_normal force" }},
{ 0x128, new C{ T="Float", N = "_q" }},
{ 0x12C, new C{ T="Float", N = "_k" }},
{ 0x130, new C{ T="Float", N = "_function ceiling" }},
{ 0x134, new C{ T="Float", N = "_maximum angle" }},
{ 0x138, new C{ T="Float", N = "_render debug radius" }},
{ 0x13C1040, new C{ T="Comment", N = "_taxi" }},
{ 0x13C1041, new C{ T="Comment", N = "_taxi surface" }},
{ 0x13C, new C{ T="Byte", N = "_offset axis" }},
{ 0x13D, new C{ T="Byte", N = "_pivot axis" }},
{ 0x13E, new C{ T="Byte", N = "_rotation axis" }},
{ 0x13F, new C{ T="Byte", N = "_normal axis" }},
{ 0x140, new C{ T="Float", N = "_offset distance" }},
{ 0x144, new C{ T="Float", N = "_pivot distance" }},
{ 0x1481042, new C{ T="Comment", N = "_normal force" }},
{ 0x148, new C{ T="Float", N = "_q" }},
{ 0x14C, new C{ T="Float", N = "_k" }},
{ 0x150, new C{ T="Float", N = "_function ceiling" }},
{ 0x154, new C{ T="Float", N = "_maximum angle" }},
{ 0x158, new C{ T="Float", N = "_render debug radius" }},
{ 0x15C1043, new C{ T="Comment", N = "_ground drag" }},
{ 0x15C1044, new C{ T="Comment", N = "_ground drag struct" }},
{ 0x15C1045, new C{ T="Comment", N = "_drag" }},
{ 0x15C, new C{ T="Float", N = "_q" }},
{ 0x160, new C{ T="Float", N = "_k" }},
{ 0x164, new C{ T="Float", N = "_constant deceleration" }},
{ 0x1681046, new C{ T="Comment", N = "_air drag" }},
{ 0x1681047, new C{ T="Comment", N = "_air drag struct" }},
{ 0x1681048, new C{ T="Comment", N = "_drag" }},
{ 0x168, new C{ T="Float", N = "_q" }},
{ 0x16C, new C{ T="Float", N = "_k" }},
{ 0x170, new C{ T="Float", N = "_constant deceleration" }},
{ 0x1741049, new C{ T="Comment", N = "_takeoff drag" }},
{ 0x1741050, new C{ T="Comment", N = "_takeoff drag struct" }},
{ 0x1741051, new C{ T="Comment", N = "_drag" }},
{ 0x174, new C{ T="Float", N = "_q" }},
{ 0x178, new C{ T="Float", N = "_k" }},
{ 0x17C, new C{ T="Float", N = "_constant deceleration" }},
}, S=384}},
{ 0xE68, new C{ T="Tagblock", N = "_type-space-fighter", B = new Dictionary<long, C>
{
{ 0x01052, new C{ T="Comment", N = "_steering control" }},
{ 0x01053, new C{ T="Comment", N = "_steering overdampening" }},
{ 0x0, new C{ T="Float", N = "_overdampen cusp angle" }},
{ 0x4, new C{ T="Float", N = "_overdampen exponent" }},
{ 0x8, new C{ T="Float", N = "_ebrake overdampen cusp angle" }},
{ 0xC, new C{ T="Float", N = "_ebrake overdampen exponent" }},
{ 0x101054, new C{ T="Comment", N = "_Throttle Steering" }},
{ 0x10, new C{ T="Float", N = "_throttle steering angle" }},
{ 0x14, new C{ T="Float", N = "_throttle steering interpolation " }},
{ 0x181055, new C{ T="Comment", N = "_" }},
{ 0x181056, new C{ T="Comment", N = "_turning control" }},
{ 0x181057, new C{ T="Comment", N = "_turning" }},
{ 0x18, new C{ T="Float", N = "_maximum left turn" }},
{ 0x1C, new C{ T="Float", N = "_maximum right turn (negative)" }},
{ 0x20, new C{ T="Float", N = "_turn rate" }},
{ 0x241058, new C{ T="Comment", N = "_velocity control variables" }},
{ 0x24, new C{ T="Float", N = "_full throttle speed" }},
{ 0x28, new C{ T="Float", N = "_neutral throttle speed" }},
{ 0x2C, new C{ T="Float", N = "_reverse throttle speed" }},
{ 0x30, new C{ T="Float", N = "_speed acceleration" }},
{ 0x34, new C{ T="Float", N = "_speed deceleration" }},
{ 0x38, new C{ T="Float", N = "_maximum left slide" }},
{ 0x3C, new C{ T="Float", N = "_maximum right slide" }},
{ 0x40, new C{ T="Float", N = "_slide acceleration" }},
{ 0x44, new C{ T="Float", N = "_slide deceleration" }},
{ 0x48, new C{ T="Float", N = "_slide accel against direction" }},
{ 0x4C1059, new C{ T="Comment", N = "_torque scale" }},
{ 0x4C, new C{ T="Float", N = "_flying torque scale" }},
{ 0x501060, new C{ T="Comment", N = "_fixed gun offset" }},
{ 0x50, new C{ T="Float", N = "_fixed gun yaw" }},
{ 0x54, new C{ T="Float", N = "_fixed gun pitch" }},
{ 0x581061, new C{ T="Comment", N = "_alien fighter trick variables" }},
{ 0x58, new C{ T="Float", N = "_maximum trick frequency" }},
{ 0x5C, new C{ T="Float", N = "_loop trick duration" }},
{ 0x60, new C{ T="Float", N = "_roll trick duration" }},
{ 0x641062, new C{ T="Comment", N = "_alien fighter fake flight contro" }},
{ 0x64, new C{ T="Float", N = "_strafe boost scale" }},
{ 0x68, new C{ T="Float", N = "_off stick deceleration scale" }},
{ 0x6C, new C{ T="Float", N = "_dive speed scale" }},
{ 0x70, new C{ T="Float", N = "_roll max velocity" }},
{ 0x74, new C{ T="Float", N = "_roll acceleration" }},
{ 0x78, new C{ T="Float", N = "_roll deceleration" }},
{ 0x7C, new C{ T="Float", N = "_roll smoothing fraction" }},
{ 0x801063, new C{ T="Comment", N = "_autolevel" }},
{ 0x80, new C{ T="Float", N = "_autolevel time" }},
{ 0x84, new C{ T="Float", N = "_autolevel pitch cutoff" }},
{ 0x88, new C{ T="Float", N = "_autolevel max velocity" }},
{ 0x8C, new C{ T="Float", N = "_autolevel max acceleration" }},
{ 0x90, new C{ T="Float", N = "_autolevel max user ang. vel." }},
{ 0x94, new C{ T="Float", N = "_autolevel spring k" }},
{ 0x98, new C{ T="Float", N = "_autolevel spring c" }},
{ 0x9C1064, new C{ T="Comment", N = "_cosmetic roll" }},
{ 0x9C, new C{ T="Float", N = "_cosmetic roll scale" }},
{ 0xA0, new C{ T="Float", N = "_cosmetic roll max bank" }},
{ 0xA4, new C{ T="Float", N = "_cosmetic roll max velocity" }},
{ 0xA8, new C{ T="Float", N = "_cosmetic roll acceleration" }},
{ 0xAC, new C{ T="Float", N = "_cosmetic roll spring k" }},
{ 0xB0, new C{ T="Float", N = "_cosmetic roll spring c" }},
{ 0xB41065, new C{ T="Comment", N = "_new roll" }},
{ 0xB4, new C{ T="4Byte", N = "_roll flags" }},
{ 0xB8, new C{ T="Float", N = "_maximum left stick roll angle" }},
{ 0xBC, new C{ T="Float", N = "_left stick rate smoothing" }},
{ 0xC0, new C{ T="Float", N = "_left stick trend smoothing" }},
{ 0xC4, new C{ T="Float", N = "_maximum right stick roll angle" }},
{ 0xC8, new C{ T="Float", N = "_right stick rate smoothing" }},
{ 0xCC, new C{ T="Float", N = "_right stick trend smoothing" }},
{ 0xD01066, new C{ T="Comment", N = "_turn deceleration" }},
{ 0xD0, new C{ T="Float", N = "_turn deceleration threshold" }},
{ 0xD4, new C{ T="Float", N = "_turn deceleration fraction" }},
{ 0xD81067, new C{ T="Comment", N = "_soft ceiling turn back" }},
{ 0xD8, new C{ T="4Byte", N = "_turn back flags" }},
{ 0xDC, new C{ T="Float", N = "_turn back latched period" }},
{ 0xE01068, new C{ T="Comment", N = "_turn back distance to turn rate" }},
{ 0xE01069, new C{ T="Comment", N = "_function" }},
{ 0xE01070, new C{ T="Comment", N = "_function" }},
{ 0xE01071, new C{ T="Comment", N = "_" }},
{ 0xE0, new C{ T="Unmapped", N = "_data" }},
{ 0xF81072, new C{ T="Comment", N = "_" }},
{ 0xF81073, new C{ T="Comment", N = "_thrust params" }},
{ 0xF8, new C{ T="Float", N = "_ideal thrust decay" }},
{ 0xFC, new C{ T="Float", N = "_ideal thrust increase" }},
{ 0x100, new C{ T="Float", N = "_minimum thrust decay" }},
{ 0x104, new C{ T="Float", N = "_minimum thrust increase" }},
{ 0x108, new C{ T="Float", N = "_maximum thrust increase" }},
{ 0x10C1074, new C{ T="Comment", N = "_dive params" }},
{ 0x10C, new C{ T="Float", N = "_minimum dive angle" }},
{ 0x110, new C{ T="Float", N = "_maximum dive angle" }},
{ 0x1141075, new C{ T="Comment", N = "_strafe params" }},
{ 0x114, new C{ T="Float", N = "_strafe boost power" }},
{ 0x1181076, new C{ T="Comment", N = "_wingtip params" }},
{ 0x118, new C{ T="Float", N = "_wingtip contrail turn" }},
{ 0x11C, new C{ T="Float", N = "_wingtip min turn" }},
{ 0x1201077, new C{ T="Comment", N = "_Safety" }},
{ 0x120, new C{ T="Float", N = "_dangerous trajectory prediction " }},
}, S=292}},
{ 0xE7C, new C{ T="Tagblock", N = "_type-revenant", B = new Dictionary<long, C>
{
{ 0x01078, new C{ T="Comment", N = "_tank block" }},
{ 0x0, new C{ T="Float", N = "_forward arc" }},
{ 0x4, new C{ T="Float", N = "_perpendicular forward arc" }},
{ 0x8, new C{ T="Float", N = "_flip window" }},
{ 0xC, new C{ T="Float", N = "_pegged fraction" }},
{ 0x10, new C{ T="Float", N = "_forward turn scale" }},
{ 0x14, new C{ T="Float", N = "_reverse turn scale" }},
{ 0x181079, new C{ T="Comment", N = "_forward differential" }},
{ 0x18, new C{ T="Float", N = "_maximum left differential" }},
{ 0x1C, new C{ T="Float", N = "_maximum right differential" }},
{ 0x20, new C{ T="Float", N = "_differential acceleration" }},
{ 0x24, new C{ T="Float", N = "_differential deceleration" }},
{ 0x281080, new C{ T="Comment", N = "_reverse differential" }},
{ 0x28, new C{ T="Float", N = "_maximum left reverse differentia" }},
{ 0x2C, new C{ T="Float", N = "_maximum right reverse differenti" }},
{ 0x30, new C{ T="Float", N = "_differential reverse acceleratio" }},
{ 0x34, new C{ T="Float", N = "_differential reverse deceleratio" }},
{ 0x381081, new C{ T="Comment", N = "_engine" }},
{ 0x381082, new C{ T="Comment", N = "_engine" }},
{ 0x38, new C{ T="Float", N = "_engine moment" }},
{ 0x3C, new C{ T="Float", N = "_engine min angular velocity" }},
{ 0x40, new C{ T="Float", N = "_engine max angular velocity" }},
{ 0x44, new C{ T="Tagblock", N = "_gears", B = new Dictionary<long, C>
{
{ 0x01083, new C{ T="Comment", N = "_loaded torque" }},
{ 0x01084, new C{ T="Comment", N = "_loaded torque curve" }},
{ 0x01085, new C{ T="Comment", N = "_function" }},
{ 0x01086, new C{ T="Comment", N = "_function" }},
{ 0x01087, new C{ T="Comment", N = "_function" }},
{ 0x01088, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="Unmapped", N = "_data" }},
{ 0x181089, new C{ T="Comment", N = "_" }},
{ 0x181090, new C{ T="Comment", N = "_cruising torque" }},
{ 0x181091, new C{ T="Comment", N = "_cruising torque curve" }},
{ 0x181092, new C{ T="Comment", N = "_function" }},
{ 0x181093, new C{ T="Comment", N = "_function" }},
{ 0x181094, new C{ T="Comment", N = "_function" }},
{ 0x181095, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x301096, new C{ T="Comment", N = "_" }},
{ 0x301097, new C{ T="Comment", N = "_gearing" }},
{ 0x30, new C{ T="Float", N = "_min time to upshift" }},
{ 0x34, new C{ T="Float", N = "_engine up-shift scale" }},
{ 0x38, new C{ T="Float", N = "_gear ratio" }},
{ 0x3C, new C{ T="Float", N = "_min time to downshift" }},
{ 0x40, new C{ T="Float", N = "_engine down-shift scale" }},
{ 0x44, new C{ T="Float", N = "_audio engine uprev rate" }},
{ 0x48, new C{ T="Float", N = "_audio engine shift up RPM value" }},
{ 0x4C, new C{ T="Float", N = "_audio engine downrev rate" }},
{ 0x50, new C{ T="Float", N = "_audio engine shift down RPM valu" }},
{ 0x54, new C{ T="TagRef", N = "_gear shift sound - shifting up" }},
{ 0x70, new C{ T="TagRef", N = "_gear shift sound - shifting down" }},
}, S=140}},
{ 0x58, new C{ T="TagRef", N = "_gear shift sound" }},
{ 0x74, new C{ T="Tagblock", N = "_load and cruise sound", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_load cruise function" }},
{ 0x4, new C{ T="4Byte", N = "_attachment index" }},
}, S=8}},
{ 0x881098, new C{ T="Comment", N = "_wheel circumferance" }},
{ 0x88, new C{ T="Float", N = "_wheel circumferance" }},
{ 0x8C, new C{ T="Float", N = "_gravity adjust" }},
{ 0x901099, new C{ T="Comment", N = "_New Tank Controls" }},
{ 0x90, new C{ T="Byte", N = "_control flags" }},
{ 0x91, new C{ T="Unmapped", N = "_generated_pad4b05" }},
{ 0x94, new C{ T="Float", N = "_at rest forward angle(purple)" }},
{ 0x98, new C{ T="Float", N = "_at rest reverse angle(violet)" }},
{ 0x9C, new C{ T="Float", N = "_at rest side on reverse angle cl" }},
{ 0xA0, new C{ T="Float", N = "_at rest side on reverse angle fu" }},
{ 0xA4, new C{ T="Float", N = "_at rest facing forward reverse a" }},
{ 0xA8, new C{ T="Float", N = "_at rest facing backward reverse " }},
{ 0xAC, new C{ T="Float", N = "_in motion opposing direction ang" }},
{ 0xB0, new C{ T="Float", N = "_in motion speed" }},
{ 0xB41100, new C{ T="Comment", N = "_velocity control variables" }},
{ 0xB4, new C{ T="Float", N = "_maximum forward speed" }},
{ 0xB8, new C{ T="Float", N = "_maximum reverse speed" }},
{ 0xBC, new C{ T="Float", N = "_speed acceleration" }},
{ 0xC0, new C{ T="Float", N = "_speed deceleration" }},
{ 0xC4, new C{ T="Float", N = "_maximum left slide" }},
{ 0xC8, new C{ T="Float", N = "_maximum right slide" }},
{ 0xCC, new C{ T="Float", N = "_slide acceleration" }},
{ 0xD0, new C{ T="Float", N = "_slide deceleration" }},
{ 0xD41101, new C{ T="Comment", N = "_scout physics" }},
{ 0xD4, new C{ T="Byte", N = "_flags" }},
{ 0xD5, new C{ T="Unmapped", N = "_generated_padddec" }},
{ 0xD81102, new C{ T="Comment", N = "_Air control" }},
{ 0xD8, new C{ T="Float", N = "_air control angular velocity fac" }},
{ 0xDC, new C{ T="Float", N = "_air control throttle acceleratio" }},
{ 0xE0, new C{ T="Float", N = "_air control throttle bonus clamp" }},
{ 0xE4, new C{ T="Float", N = "_air control throttle bonus clamp" }},
{ 0xE8, new C{ T="Float", N = "_air control angular acceleration" }},
{ 0xEC, new C{ T="Float", N = "_air control auto level strength" }},
{ 0xF01103, new C{ T="Comment", N = "_Brake control" }},
{ 0xF0, new C{ T="Float", N = "_brake lift angular acceleration" }},
{ 0xF4, new C{ T="Float", N = "_brake lift acceleration up" }},
{ 0xF8, new C{ T="Float", N = "_brake airborne lift acceleration" }},
{ 0xFC, new C{ T="Float", N = "_brake airborne lift acceleration" }},
{ 0x100, new C{ T="Float", N = "_drag coeficient" }},
{ 0x104, new C{ T="Float", N = "_constant deceleration" }},
{ 0x108, new C{ T="Float", N = "_torque scale" }},
}, S=268}},
{ 0xE901104, new C{ T="Comment", N = "_" }},
{ 0xE901105, new C{ T="Comment", N = "_General Vehicle Physics" }},
{ 0xE901106, new C{ T="Comment", N = "_havok vehicle physics" }},
{ 0xE90, new C{ T="Float", N = "_gravity scale" }},
{ 0xE94, new C{ T="4Byte", N = "_iteration count" }},
{ 0xE98, new C{ T="Tagblock", N = "_anti gravity point configuration", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x41107, new C{ T="Comment", N = "_Operational Range- Height" }},
{ 0x4, new C{ T="Float", N = "_inner distance" }},
{ 0x8, new C{ T="Float", N = "_outer distance" }},
{ 0xC1108, new C{ T="Comment", N = "_Banking" }},
{ 0xC, new C{ T="Float", N = "_banking lift" }},
{ 0x10, new C{ T="Float", N = "_steering banking factor" }},
{ 0x141109, new C{ T="Comment", N = "_Maximum Lifting Force" }},
{ 0x14, new C{ T="Float", N = "_strength" }},
{ 0x181110, new C{ T="Comment", N = "_Error" }},
{ 0x18, new C{ T="Float", N = "_error" }},
{ 0x1C1111, new C{ T="Comment", N = "_Operational Range- Orientation" }},
{ 0x1C, new C{ T="Float", N = "_inner rotational limit" }},
{ 0x20, new C{ T="Float", N = "_outer rotational limit" }},
{ 0x241112, new C{ T="Comment", N = "_Damping" }},
{ 0x24, new C{ T="Float", N = "_compression damping" }},
{ 0x28, new C{ T="Float", N = "_extension damping" }},
{ 0x2C1113, new C{ T="Comment", N = "_Ground Impact Effects" }},
{ 0x2C, new C{ T="TagRef", N = "_combined material effects" }},
{ 0x48, new C{ T="TagRef", N = "_visual material effects" }},
{ 0x64, new C{ T="TagRef", N = "_sound material effects" }},
{ 0x801114, new C{ T="Comment", N = "_" }},
}, S=128}},
{ 0xEAC, new C{ T="Tagblock", N = "_anti gravity points", B = new Dictionary<long, C>
{
{ 0x01115, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="mmr3Hash", N = "_marker name" }},
{ 0x4, new C{ T="mmr3Hash", N = "_region name" }},
{ 0x8, new C{ T="4Byte", N = "_runtime region index" }},
{ 0xC1116, new C{ T="Comment", N = "_" }},
{ 0xC, new C{ T="mmr3Hash", N = "_looping sound marker" }},
{ 0x10, new C{ T="Tagblock", N = "_state configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_region state" }},
{ 0x2, new C{ T="2Byte", N = "_configuration" }},
}, S=4}},
}, S=36}},
{ 0xEC0, new C{ T="Tagblock", N = "_friction point configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="Float", N = "_radius" }},
{ 0x81117, new C{ T="Comment", N = "_Normal Force Simulation" }},
{ 0x8, new C{ T="Float", N = "_ground depth" }},
{ 0xC, new C{ T="Float", N = "_ground damp factor" }},
{ 0x101118, new C{ T="Comment", N = "_Physics Simulation Noise Reducti" }},
{ 0x10, new C{ T="Float", N = "_ground normal blend rate" }},
{ 0x14, new C{ T="Float", N = "_max normal force contribution to" }},
{ 0x181119, new C{ T="Comment", N = "_Traction" }},
{ 0x18, new C{ T="Float", N = "_static friction" }},
{ 0x1C, new C{ T="Float", N = "_kinetic friction" }},
{ 0x20, new C{ T="Float", N = "_slide speed difference" }},
{ 0x24, new C{ T="Float", N = "_e-brake static friction" }},
{ 0x28, new C{ T="Float", N = "_e-brake kinetic friction" }},
{ 0x2C, new C{ T="Float", N = "_e-brake slide speed difference" }},
{ 0x30, new C{ T="Float", N = "_AI static friction override" }},
{ 0x34, new C{ T="Float", N = "_AI kinetic friction override" }},
{ 0x38, new C{ T="Float", N = "_world space slope to stop all tr" }},
{ 0x3C, new C{ T="Float", N = "_local space slope to start tract" }},
{ 0x40, new C{ T="Float", N = "_local space slope to stop all tr" }},
{ 0x44, new C{ T="Float", N = "_local space slope to stop hit ch" }},
{ 0x481120, new C{ T="Comment", N = "_Grounded Vehicle Stabilizing Tor" }},
{ 0x481121, new C{ T="Comment", N = "_grounded speed to down torque fu" }},
{ 0x481122, new C{ T="Comment", N = "_function" }},
{ 0x481123, new C{ T="Comment", N = "_function" }},
{ 0x481124, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="Unmapped", N = "_data" }},
{ 0x601125, new C{ T="Comment", N = "_" }},
{ 0x601126, new C{ T="Comment", N = "_nearly grounded speed to down to" }},
{ 0x601127, new C{ T="Comment", N = "_function" }},
{ 0x601128, new C{ T="Comment", N = "_function" }},
{ 0x601129, new C{ T="Comment", N = "_" }},
{ 0x60, new C{ T="Unmapped", N = "_data" }},
{ 0x781130, new C{ T="Comment", N = "_" }},
{ 0x781131, new C{ T="Comment", N = "_Ground Impact Effects" }},
{ 0x78, new C{ T="TagRef", N = "_combined material effects" }},
{ 0x94, new C{ T="TagRef", N = "_visual material effects" }},
{ 0xB0, new C{ T="TagRef", N = "_sound material effects" }},
{ 0xCC1132, new C{ T="Comment", N = "_" }},
{ 0xCC, new C{ T="4Byte", N = "_flags" }},
}, S=208}},
{ 0xED4, new C{ T="Tagblock", N = "_friction points", B = new Dictionary<long, C>
{
{ 0x01133, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="mmr3Hash", N = "_marker name" }},
{ 0x4, new C{ T="mmr3Hash", N = "_region name" }},
{ 0x8, new C{ T="4Byte", N = "_runtime region index" }},
{ 0xC, new C{ T="Float", N = "_load fraction" }},
{ 0x10, new C{ T="Float", N = "_turn ratio" }},
{ 0x14, new C{ T="4Byte", N = "_flags" }},
{ 0x181134, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="mmr3Hash", N = "_looping sound marker" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_grinding object function" }},
{ 0x20, new C{ T="mmr3Hash", N = "_rolling object function" }},
{ 0x24, new C{ T="Tagblock", N = "_state configurations", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_region state" }},
{ 0x2, new C{ T="2Byte", N = "_configuration" }},
}, S=4}},
}, S=56}},
{ 0xEE81135, new C{ T="Comment", N = "_Grounded Vehicle Stabilizing Dow" }},
{ 0xEE8, new C{ T="Float", N = "_maximum speed for downForce" }},
{ 0xEEC, new C{ T="Float", N = "_angle to exclude downforce" }},
{ 0xEF01136, new C{ T="Comment", N = "_grounded speed to COM downforce " }},
{ 0xEF01137, new C{ T="Comment", N = "_function" }},
{ 0xEF01138, new C{ T="Comment", N = "_function" }},
{ 0xEF01139, new C{ T="Comment", N = "_" }},
{ 0xEF0, new C{ T="Unmapped", N = "_data" }},
{ 0xF081140, new C{ T="Comment", N = "_" }},
{ 0xF081141, new C{ T="Comment", N = "_nearly grounded speed to COM dow" }},
{ 0xF081142, new C{ T="Comment", N = "_function" }},
{ 0xF081143, new C{ T="Comment", N = "_function" }},
{ 0xF081144, new C{ T="Comment", N = "_" }},
{ 0xF08, new C{ T="Unmapped", N = "_data" }},
{ 0xF201145, new C{ T="Comment", N = "_" }},
{ 0xF201146, new C{ T="Comment", N = "_Airborne Vehicle Rotational Stab" }},
{ 0xF20, new C{ T="Float", N = "_airborne angular stabilization c.X" }},
{ 0xF24, new C{ T="Float", N = "_airborne angular stabilization c.Y" }},
{ 0xF28, new C{ T="Float", N = "_airborne angular stabilization c.Z" }},
{ 0xF2C, new C{ T="Tagblock", N = "_tricks", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_animation name" }},
{ 0x4, new C{ T="Byte", N = "_activation type" }},
{ 0x5, new C{ T="Byte", N = "_velocity preservation" }},
{ 0x6, new C{ T="Byte", N = "_flags" }},
{ 0x7, new C{ T="Unmapped", N = "_generated_pad983c" }},
{ 0x8, new C{ T="Float", N = "_camera interpolation time" }},
{ 0xC, new C{ T="Float", N = "_trick exit time" }},
{ 0x10, new C{ T="Float", N = "_trick exit camera interpolation .min" }},
{ 0x14, new C{ T="Float", N = "_trick exit camera interpolation .max" }},
{ 0x18, new C{ T="Float", N = "_trick exit displacement referenc" }},
{ 0x1C, new C{ T="Float", N = "_cooldown time" }},
{ 0x20, new C{ T="4Byte", N = "_tutorial id" }},
}, S=36}},
{ 0xF40, new C{ T="Byte", N = "_player training vehicle type" }},
{ 0xF41, new C{ T="Byte", N = "_vehicle size" }},
{ 0xF43, new C{ T="Unmapped", N = "_generated_pad9b33" }},
{ 0xF44, new C{ T="Float", N = "_complex suspension distribution " }},
{ 0xF48, new C{ T="Float", N = "_complex suspension wheel diamete" }},
{ 0xF4C, new C{ T="Float", N = "_complex suspension wheel width" }},
{ 0xF50, new C{ T="Float", N = "_minimum flipping angular velocit" }},
{ 0xF54, new C{ T="Float", N = "_maximum flipping angular velocit" }},
{ 0xF58, new C{ T="Float", N = "_upside down angle" }},
{ 0xF5C, new C{ T="Float", N = "_crouch transition time" }},
{ 0xF60, new C{ T="Float", N = "_HOOJYTSU" }},
{ 0xF64, new C{ T="Float", N = "_seat enterance acceleration scal" }},
{ 0xF68, new C{ T="Float", N = "_seat exit accelersation scale" }},
{ 0xF6C, new C{ T="mmr3Hash", N = "_blur function name" }},
{ 0xF70, new C{ T="Float", N = "_blur speed" }},
{ 0xF74, new C{ T="mmr3Hash", N = "_flip message" }},
{ 0xF781147, new C{ T="Comment", N = "_sounds and effects" }},
{ 0xF78, new C{ T="TagRef", N = "_Player vehicle sound bank" }},
{ 0xF94, new C{ T="TagRef", N = "_Surface sound material effects" }},
{ 0xFB0, new C{ T="TagRef", N = "_light suspension sound" }},
{ 0xFCC, new C{ T="Float", N = "_Light suspension sound displacem" }},
{ 0xFD0, new C{ T="TagRef", N = "_medium suspension sound" }},
{ 0xFEC, new C{ T="Float", N = "_Medium suspension sound displace" }},
{ 0xFF0, new C{ T="TagRef", N = "_hard suspension sound" }},
{ 0x100C, new C{ T="Float", N = "_Hard suspension sound displaceme" }},
{ 0x1010, new C{ T="Float", N = "_fake audio speed - speed increas" }},
{ 0x1014, new C{ T="Float", N = "_fake audio speed - boost speed i" }},
{ 0x1018, new C{ T="Float", N = "_fake audio speed - speed decreas" }},
{ 0x101C, new C{ T="Float", N = "_fake audio speed - non-boost lim" }},
{ 0x1020, new C{ T="Float", N = "_fake audio speed - max speed sca" }},
{ 0x1024, new C{ T="Tagblock", N = "_Sound RTPCs", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Attachment Index" }},
{ 0x4, new C{ T="mmr3Hash", N = "_Function" }},
{ 0x8, new C{ T="mmr3Hash", N = "_RTPC Name" }},
{ 0xC, new C{ T="4Byte", N = "_RTPC name hash value" }},
}, S=16}},
{ 0x1038, new C{ T="Tagblock", N = "_Sound Sweeteners", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Function" }},
{ 0x4, new C{ T="TagRef", N = "_sound" }},
{ 0x20, new C{ T="Float", N = "_Switch point" }},
{ 0x24, new C{ T="4Byte", N = "_Mode" }},
}, S=40}},
{ 0x104C, new C{ T="TagRef", N = "_special effect" }},
{ 0x1068, new C{ T="TagRef", N = "_driver boost damage effect or re" }},
{ 0x1084, new C{ T="TagRef", N = "_rider boost damage effect or res" }},
{ 0x10A0, new C{ T="mmr3Hash", N = "_vehicle name" }},
{ 0x10A4, new C{ T="Tagblock", N = "_AI cruise control", B = new Dictionary<long, C>
{
{ 0x01148, new C{ T="Comment", N = "_ai speed control" }},
{ 0x0, new C{ T="Float", N = "_Proportional" }},
{ 0x4, new C{ T="Float", N = "_Integral" }},
{ 0x8, new C{ T="Float", N = "_Derivative" }},
{ 0xC, new C{ T="Float", N = "_Slow down rate" }},
}, S=16}},
{ 0x10B8, new C{ T="Tagblock", N = "_UI display info", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="mmr3Hash", N = "_alt name " }},
{ 0x8, new C{ T="mmr3Hash", N = "_description" }},
{ 0xC, new C{ T="mmr3Hash", N = "_help text" }},
{ 0x10, new C{ T="mmr3Hash", N = "_icon string id" }},
{ 0x14, new C{ T="TagRef", N = "_sprite" }},
{ 0x30, new C{ T="4Byte", N = "_sprite index" }},
{ 0x34, new C{ T="TagRef", N = "_alt sprite" }},
{ 0x50, new C{ T="4Byte", N = "_alt sprite index" }},
{ 0x54, new C{ T="4Byte", N = "_damage sprite index" }},
{ 0x58, new C{ T="TagRef", N = "_ui vehicle screen reference" }},
}, S=116}},
{ 0x10CC, new C{ T="TagRef", N = "_UI Vehicle Display info" }},
{ 0x10E8, new C{ T="4Byte", N = "_Vehicle Nav Point Sequence Index" }},
{ 0x10EC1149, new C{ T="Comment", N = "_" }},
{ 0x10EC, new C{ T="Unmapped", N = "_generated_pade54b" }},
}},
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

					{ 0xC7C, new C { T = "Tagblock", B = new Dictionary<long, C> // trigger block
					{ 
					// IMPORTANT NOTE: THESE FLAG GROUPS ARE DROPBOXES
					// WE DON'T HAVE SUPPORT FOR IT YET, SO THEY'RE FLAG BLOCKS INSTEAD FOR NOW
						{ 0x4, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //INPUT
						{
							{ 7,  "All Unchecked = Right Trigger"  },
							{ 0,  "Left Trigger"  },
							{ 1,  "Melee Attack"  },
						} } },

						{ 0x6, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //BEHAVIOR
						{
							{ 7,  "All Unchecked = Spew"  },
							{ 0,  "Latch"  },
							{ 1,  "Latch-Autofire"  },
							{ 2,  "Charge"  },
							{ 3,  "Latch-Zoom"  },
							{ 4,  "Latch-RocketLauncher"  },
							{ 5,  "Spew-Charge"  },
							{ 6,  "Sword-Charge"  },
						} } },

						{ 0x8, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //PRIMARY BARREL
						{
							{ 6,  "All Unchecked = Primary Barrel"  },
							{ 7,  "All Checked = None"  },
							{ 0,  "Secondary Barrel"  },
						} } },

						{ 0xA, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //SECONDARY BARREL
						{
							{ 6,  "All Unchecked = Primary Barrel"  },
							{ 7,  "All Checked = None"  },
							{ 0,  "Secondary Barrel"  },
						} } },

						{ 0xC, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //PREDICTION
						{
							{ 7,  "All Unchecked = None"  },
							{ 0,  "Spew"  },
							{ 1,  "Charge"  },
						} } },

						{ 0x10, new C{ T="Float", N = "Autofire Time" }},
						{ 0x14, new C{ T="Float", N = "Autofire Throw" }},

						{ 0x18, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //SECONDARY ACTION
						{
							{ 7,  "All Unchecked = Fire"  },
							{ 0,  "Charge"  },
							{ 1,  "Track"  },
							{ 2,  "Fire Other"  },
						} } },

						{ 0x1C, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //PRIMARY ACTION
						{
							{ 7,  "All Unchecked = Fire"  },
							{ 0,  "Charge"  },
							{ 1,  "Track"  },
							{ 2,  "Fire Other"  },
						} } },

						{ 0x20, new C{ T="Float", N = "Charging Time" }},
						{ 0x24, new C{ T="Float", N = "Charged Time" }}, //- DOESN"T DO ANYTHING

						{ 0x28, new FlagGroup { A = 1, STR = new Dictionary<int, string>() //OVERCHARGE ACTION - DOESN"T DO ANYTHING
						{
							{ 7,  "All Unchecked = None"  },
							{ 0,  "Explode"  },
							{ 1,  "Discharge"  },
						} } },

						{ 0x30, new C{ T="TagRef"}},
						{ 0x4C, new C{ T="TagRef"}},
						{ 0x68, new C{ T="TagRef"}},

						{ 0x84, new C{ T="Float", N = "Charged Drain Rate" }}, //THIS WORKS

						{ 0x88, new C{ T="TagRef"}},
						{ 0xA4, new C{ T="TagRef"}},
						{ 0xD4, new C{ T="TagRef"}},
						{ 0xF0, new C{ T="TagRef"}},
					} } },

					{ 0xC90, new C {T = "Tagblock", B = new Dictionary<long, C> // barrel block
					{
						{ 4, new C{ T="Float", N = "Minimum Rounds per Minute"}},
						{ 8, new C{ T="Float", N = "Maximum Rounds per Minute"}},

						{ 56, new C { T = "2Byte", N = "Rounds per Burst" } },
						{ 58, new C { T = "2Byte" } }, //Relates to the one above
						
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

						{ 0xC8, new C{ T="Float", N = "Minimum Error"}},
						{ 0xCC, new C{ T="Float", N = "Minimum Error Angle"}},
						{ 0xD0, new C{ T="Float", N = "Maximum Error Angle"}},

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

						{ 0x2E8, new C { T= "Tagblock" , B = new Dictionary<long, C>
						{
							{ 0x0, new C { T = "Float", N = "Horizontal" } },
							{ 0x04, new C { T = "Float" } },
							{ 0x0C, new C { T = "Float", N = "Vertical" } },
							{ 0x10, new C { T = "Float" } },
						} } },

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
						{ 0x4, new C { T = "mmr3Hash", N = "Coating Hash" } }, // coating

						{ 0x34, new C{ T="Tagblock", B=new Dictionary<long, C> // region block
						{
							{ 0x0, new C{ T="mmr3Hash"}},
							{ 0x4, new C{ T="Byte", N="Model Region Index"}},
							{ 0x5, new C{ T="Flags"}},
							{ 0x6, new C{ T="2Byte", N="Parent Variant"}},
							{ 0x8, new C{ T="Tagblock", B=new Dictionary<long, C> // permutation block
							{
								{ 0x0, new C{ T="mmr3Hash"}},
								{ 0x4, new C{ T="Byte", N="Model Permutation Index"}},
								{ 0xC, new C{ T="Float",N="Probability"}},
								{ 0x10, new C{ T="Tagblock"}},
							}, S=44}},

						}, S=32}},



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

					{ 92, new C { T = "Tagblock", N = "Source Sidecar" } }, // SidecarPathDefinition

					{ 112, new C { T = "Float" } },
					{ 116, new C { T = "Float" } },

					{ 120, new C { T = "TagRef", N = "Model" } }, // vehicle model
					{ 148, new C { T = "TagRef", N = "Asset" } }, // aset tag ref
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

					{ 448, new C { T = "Tagblock", N = "AI Properties" } }, // object_ai_properties
					{ 468, new C { T = "Tagblock", N = "Functions" } }, // s_object_function_definition
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
					{ 0x710, new C { T = "Float", N = "Max Range" } },
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

					{ 0x850, new C { T = "TagRef", N = "Super Detonation" } },
					{ 0x86C, new C { T = "TagRef", N = "Super Detonation Damage" } },
					{ 0x888, new C { T = "TagRef", N = "Detonation Sound" } }, // SND
					{ 0x8A4, new C { T = "TagRef", N = "Super Detonation Sound" } },
					{ 0x8C0, new C { T = "TagRef", N = "Submerged Detonation Sound" } }, // SND
					{ 0x8DC, new C { T = "TagRef", N = "Super Detonation Object Types" } },

					{ 0x8FC, new C { T = "Float" } },

					{ 0x908, new C { T = "TagRef" } },
					{ 0x924, new C { T = "TagRef" } },
					{ 0x944, new C { T = "TagRef" } },
					{ 0x984, new C { T = "TagRef" } },
					{ 0x9A0, new C { T = "TagRef" } }, // JPT
					{ 0x9BC, new C { T = "TagRef" } }, // JPT

					{ 0x9DC, new C { T = "Tagblock", N = "Boarding Fields" } },

					{ 0x9F0, new C { T = "Float", N = "Boarding Detonation Time" } },

					{ 0x9F4, new C { T = "TagRef", N = "Boarding Detonation Damage" } }, //JPT
					{ 0xA10, new C { T = "TagRef", N = "Boarding Attached Detonation Damage" } }, // JPT

					{ 0xA2C, new C { T = "Float", N = "Air Gravity Scale" } },
					{ 0xA34, new C { T = "Float", N = "Air Damage Range" } },
					{ 0xA38, new C { T = "Float", N = "Water Gravity Scale" } },
					{ 0xA40, new C { T = "Float", N = "Water Damage Range" } },
					{ 0xA44, new C { T = "Float", N = "Initial Velocity" } },
					{ 0xA80, new C { T = "Float", N = "Final Velocity" } },

					{ 0xAA0, new C { T = "TagRef", N = "Material Response Reference" } },

					{ 0xABC, new C { T = "Tagblock", N = "Old Material Response Reference" } },
					{ 0xAD0, new C { T = "Tagblock", N = "Material Response" } },
					{ 0xAE4, new C { T = "Tagblock", N = "Brute Grenade" } },
					{ 0xAF8, new C { T = "Tagblock", N = "Fire Bomb Grenade" } },
					{ 0xB0C, new C { T = "Tagblock", N = "Conical Spread" } },

					{ 0xB20, new C { T = "TagRef" } },

					{ 0xB3C, new C { T = "Float" } },
					{ 0xB40, new C { T = "Float" } },
					{ 0xB44, new C { T = "Float" } },

					{ 0xB48, new C { T = "Tagblock", N = "Sound RTPCs" } },
					{ 0xB60, new C { T = "Tagblock", N = "Submunition Events" } },

					{ 0xB8C, new C { T = "Float" } },
					{ 0xB90, new C { T = "Float" } },

					{ 0xBA0, new C { T = "TagRef", N = "Charged Detonation Damage" } }, // JPT
					{ 0xBBC, new C { T = "TagRef", N = "Charged Impact Damage" } },
					{ 0xBD8, new C { T = "TagRef", N = "Charged Visual Effect" } }, // EFFE
					{ 0xBF4, new C { T = "TagRef", N = "Charged Detonation Effect" } }, // EFFE
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
			},
			{ "effe", new()
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
				}, S=140}},
			}},

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

				{ 0x28, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x0, new C{ T="mmr3Hash", N = "Hash"} },
					{ 0x4, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0x0, new C{ T="mmr3Hash", N = "Name"} },
						{ 0x4, new C{ T="2Byte", N = "Mesh Index"} },
						{ 0x6, new C{ T="2Byte", N = "Mesh Count"} },
						{ 0x8, new C{ T="mmr3Hash", N = "Clone Name"} },

					}, S=12 }},

				}, S=24 }},
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

			{ "char", new()
{
{ 0x0516, new C{ T="Comment", N = "_AnyTag" }},
{ 0x0, new C{ T="Pointer", N = "_vtable space" }},
{ 0x8517, new C{ T="Comment", N = "_internal struct" }},
{ 0x8, new C{ T="4Byte", N = "_global tag ID" }},
{ 0xC, new C{ T="Unmapped", N = "_local tag handle" }},
{ 0x10, new C{ T="4Byte", N = "_Character flags" }},
{ 0x14, new C{ T="TagRef", N = "_parent character" }},
{ 0x30, new C{ T="TagRef", N = "_unit" }},
{ 0x4C, new C{ T="TagRef", N = "_creature" }},
{ 0x68, new C{ T="TagRef", N = "_style" }},
{ 0x84, new C{ T="TagRef", N = "_major character" }},
{ 0xA0, new C{ T="TagRef", N = "_mythic skull character" }},
{ 0xBC, new C{ T="Tagblock", N = "_variants", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_variant name" }},
{ 0x4, new C{ T="2Byte", N = "_variant index" }},
{ 0x6, new C{ T="Unmapped", N = "_generated_padedbf" }},
{ 0x8, new C{ T="mmr3Hash", N = "_Style Id" }},
{ 0xC, new C{ T="Tagblock", N = "_voices", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_dialogue" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_designator" }},
{ 0x20, new C{ T="Float", N = "_weight" }},
{ 0x24, new C{ T="Tagblock", N = "_region filters", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_region name" }},
{ 0x4, new C{ T="Tagblock", N = "_permutation filters", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_permutation name" }},
}, S=4}},
}, S=24}},
}, S=56}},
}, S=32}},
{ 0xD0, new C{ T="Tagblock", N = "_voice", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_voices", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_dialogue" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_designator" }},
{ 0x20, new C{ T="Float", N = "_weight" }},
{ 0x24, new C{ T="Tagblock", N = "_region filters", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_region name" }},
{ 0x4, new C{ T="Tagblock", N = "_permutation filters", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_permutation name" }},
}, S=4}},
}, S=24}},
}, S=56}},
}, S=20}},
{ 0xE4, new C{ T="Tagblock", N = "_Development Status", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_Stage" }},
{ 0x2, new C{ T="Unmapped", N = "_padding1" }},
}, S=4}},
{ 0xF8, new C{ T="Tagblock", N = "_general properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_general flags" }},
{ 0x4, new C{ T="2Byte", N = "_type" }},
{ 0x6, new C{ T="2Byte", N = "_rank" }},
{ 0x8, new C{ T="TagRef", N = "_prototype script" }},
{ 0x24, new C{ T="Float", N = "_max leader dist" }},
{ 0x28, new C{ T="Float", N = "_absolute max leader dist" }},
{ 0x2C, new C{ T="Float", N = "_max player dialogue dist" }},
{ 0x30, new C{ T="Float", N = "_scariness" }},
{ 0x34, new C{ T="Byte", N = "_default grenade type" }},
{ 0x35, new C{ T="Unmapped", N = "_generated_pad6d7e" }},
{ 0x36, new C{ T="2Byte", N = "_behavior tree root" }},
{ 0x38, new C{ T="TagRef", N = "_Data Behavior Tree" }},
{ 0x54, new C{ T="Tagblock", N = "_disallowed weapons from trading", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_weapon" }},
}, S=28}},
{ 0x68, new C{ T="TagRef", N = "_Initial Primary Weapon " }},
{ 0x84, new C{ T="TagRef", N = "_Primary Weapon Configuration" }},
{ 0xA0, new C{ T="TagRef", N = "_Initial Secondary Weapon " }},
{ 0xBC, new C{ T="TagRef", N = "_Secondary Weapon Configuration" }},
{ 0xD8, new C{ T="TagRef", N = "_Initial Equipment " }},
{ 0xF4, new C{ T="mmr3Hash", N = "_Shield Frame Attachment" }},
{ 0xF8, new C{ T="2Byte", N = "_Token Priority" }},
{ 0xFA, new C{ T="Unmapped", N = "_generated_padba33" }},
{ 0xFC, new C{ T="Tagblock", N = "_Designer Metadata", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_meta label" }},
}, S=4}},
{ 0x110, new C{ T="Tagblock", N = "_Dialogue System Metadata", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_meta label" }},
}, S=4}},
}, S=292}},
{ 0x10C, new C{ T="Tagblock", N = "_proto spawn properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_Proto Spawn Type " }},
}, S=2}},
{ 0x120, new C{ T="Tagblock", N = "_interact properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_Interact Flags" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_padb4d2" }},
{ 0x4, new C{ T="Float", N = "_default maximum object interact " }},
}, S=8}},
{ 0x134, new C{ T="Tagblock", N = "_emotion properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_situational danger", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_highest prop class" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_pad78a8" }},
{ 0x4, new C{ T="Float", N = "_situational danger" }},
}, S=8}},
{ 0x14, new C{ T="Float", N = "_perceived danger increase half-l" }},
{ 0x18, new C{ T="Float", N = "_perceived danger decay half-life" }},
{ 0x1C, new C{ T="Float", N = "_Perceived Danger Alert Threshold" }},
{ 0x20, new C{ T="Float", N = "_Perceived Danger Combat Threshol" }},
}, S=36}},
{ 0x148, new C{ T="Tagblock", N = "_vitality properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_vitality flags" }},
{ 0x4, new C{ T="Float", N = "_normal body vitality" }},
{ 0x8, new C{ T="Float", N = "_normal shield vitality" }},
{ 0xC, new C{ T="Float", N = "_legendary body vitality" }},
{ 0x10, new C{ T="Float", N = "_legendary shield vitality" }},
{ 0x14, new C{ T="Float", N = "_body recharge fraction" }},
{ 0x18, new C{ T="Float", N = "_soft ping threshold (with shield" }},
{ 0x1C, new C{ T="Float", N = "_soft ping threshold (no shields)" }},
{ 0x20, new C{ T="Float", N = "_medium ping threshold (with shie" }},
{ 0x24, new C{ T="Float", N = "_medium ping threshold (no shield" }},
{ 0x28, new C{ T="Float", N = "_medium ping cooldown time" }},
{ 0x2C, new C{ T="Float", N = "_hard ping threshold (with shield" }},
{ 0x30, new C{ T="Float", N = "_hard ping threshold (no shields)" }},
{ 0x34, new C{ T="Float", N = "_hard ping cooldown time" }},
{ 0x38, new C{ T="Float", N = "_body recharge delay time" }},
{ 0x3C, new C{ T="Float", N = "_body recharge time" }},
{ 0x40, new C{ T="Float", N = "_shield recharge delay time" }},
{ 0x44, new C{ T="Float", N = "_shield recharge time" }},
{ 0x48, new C{ T="Float", N = "_extended shield damage threshold" }},
{ 0x4C, new C{ T="Float", N = "_extended body damage threshold" }},
{ 0x50, new C{ T="Float", N = "_runtime_body_recharge_velocity" }},
{ 0x54, new C{ T="Float", N = "_runtime_shield_recharge_velocity" }},
{ 0x58, new C{ T="TagRef", N = "_resurrect weapon" }},
{ 0x74, new C{ T="Float", N = "_player damage_scale" }},
{ 0x78, new C{ T="TagRef", N = "_collision damage override" }},
{ 0x94, new C{ T="TagRef", N = "_Knockback Collision Damage Overr" }},
{ 0xB0, new C{ T="Float", N = "_stun threshold (easy)" }},
{ 0xB4, new C{ T="Float", N = "_stun threshold (normal)" }},
{ 0xB8, new C{ T="Float", N = "_stun threshold (heroic)" }},
{ 0xBC, new C{ T="Float", N = "_stun threshold (legendary)" }},
{ 0xC0, new C{ T="Float", N = "_stun time scale (easy)" }},
{ 0xC4, new C{ T="Float", N = "_stun time scale (normal)" }},
{ 0xC8, new C{ T="Float", N = "_stun time scale (heroic)" }},
{ 0xCC, new C{ T="Float", N = "_stun time scale (legendary)" }},
{ 0xD0, new C{ T="Float", N = "_stun cooldown (easy)" }},
{ 0xD4, new C{ T="Float", N = "_stun cooldown (normal)" }},
{ 0xD8, new C{ T="Float", N = "_stun cooldown (heroic)" }},
{ 0xDC, new C{ T="Float", N = "_stun cooldown (legendary)" }},
}, S=224}},
{ 0x15C, new C{ T="Tagblock", N = "_placement properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_few upgrade chance (easy)" }},
{ 0x4, new C{ T="Float", N = "_few upgrade chance (normal)" }},
{ 0x8, new C{ T="Float", N = "_few upgrade chance (heroic)" }},
{ 0xC, new C{ T="Float", N = "_few upgrade chance (legendary)" }},
{ 0x10, new C{ T="Float", N = "_normal upgrade chance (easy)" }},
{ 0x14, new C{ T="Float", N = "_normal upgrade chance (normal)" }},
{ 0x18, new C{ T="Float", N = "_normal upgrade chance (heroic)" }},
{ 0x1C, new C{ T="Float", N = "_normal upgrade chance (legendary" }},
{ 0x20, new C{ T="Float", N = "_many upgrade chance (easy)" }},
{ 0x24, new C{ T="Float", N = "_many upgrade chance (normal)" }},
{ 0x28, new C{ T="Float", N = "_many upgrade chance (heroic)" }},
{ 0x2C, new C{ T="Float", N = "_many upgrade chance (legendary)" }},
}, S=48}},
{ 0x170, new C{ T="Tagblock", N = "_base perception properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_flags" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_pad16e5" }},
{ 0x4, new C{ T="Float", N = "_Silent Movement Speed Threshold" }},
{ 0x8, new C{ T="Float", N = "_Perception Decay Delay" }},
{ 0xC, new C{ T="Float", N = "_Moving Perception Decay Delay" }},
{ 0x10, new C{ T="Float", N = "_Perception Begin To Forget Time" }},
{ 0x14, new C{ T="Float", N = "_Perception Forget Time" }},
{ 0x18, new C{ T="Float", N = "_Combat Status Alert Cooldown" }},
{ 0x1C, new C{ T="Float", N = "_Combat Status Active Cooldown" }},
{ 0x20, new C{ T="Float", N = "_Postcombat clump state time" }},
}, S=36}},
{ 0x184, new C{ T="Tagblock", N = "_perception properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_perception_mode" }},
{ 0x2, new C{ T="2Byte", N = "_flags" }},
{ 0x4, new C{ T="Float", N = "_maximum vision distance" }},
{ 0x8, new C{ T="Float", N = "_reliable vision distance" }},
{ 0xC, new C{ T="Float", N = "_maximum peripheral vision distan" }},
{ 0x10, new C{ T="Float", N = "_reliable peripheral vision dista" }},
{ 0x14, new C{ T="Float", N = "_max peripheral vision distance a" }},
{ 0x18, new C{ T="Float", N = "_max reliable peripheral vision d" }},
{ 0x1C, new C{ T="Float", N = "_Maximum Unmistakable Distance" }},
{ 0x20, new C{ T="Float", N = "_surprise distance" }},
{ 0x24, new C{ T="Float", N = "_Min distance from last known pos" }},
{ 0x28, new C{ T="Float", N = "_Min time from last clump surpris" }},
{ 0x2C, new C{ T="Float", N = "_Min time from last seen to surpr" }},
{ 0x30, new C{ T="Float", N = "_Surprise lerp distance range.min" }},
{ 0x34, new C{ T="Float", N = "_Surprise lerp distance range.max" }},
{ 0x38, new C{ T="Float", N = "_Surprise angle range.min" }},
{ 0x3C, new C{ T="Float", N = "_Surprise angle range.max" }},
{ 0x40, new C{ T="Float", N = "_focus interior angle" }},
{ 0x44, new C{ T="Float", N = "_focus exterior angle" }},
{ 0x48, new C{ T="Float", N = "_peripheral vision angle" }},
{ 0x4C, new C{ T="Float", N = "_Vertical Exterior Up Angle" }},
{ 0x50, new C{ T="Float", N = "_Vertical Exterior Down Angle" }},
{ 0x54, new C{ T="Float", N = "_hearing distance" }},
{ 0x58, new C{ T="Float", N = "_Max Propagation Time" }},
{ 0x5C, new C{ T="Float", N = "_Partial Perception Awareness Del" }},
{ 0x60, new C{ T="Float", N = "_Full Perception Awareness Delay" }},
{ 0x64, new C{ T="Float", N = "_Unmistakable Perception Awarenes" }},
{ 0x68, new C{ T="Float", N = "_Partial Perception Acknowledgeme" }},
{ 0x6C, new C{ T="Float", N = "_Full Perception Acknowledgement " }},
{ 0x70, new C{ T="Float", N = "_Unmistakable Perception Acknowle" }},
{ 0x74, new C{ T="Float", N = "_awareness glance level" }},
{ 0x78, new C{ T="Float", N = "_identify hologram chance" }},
{ 0x7C, new C{ T="Float", N = "_hologram ignore timer.min" }},
{ 0x80, new C{ T="Float", N = "_hologram ignore timer.max" }},
{ 0x84, new C{ T="Float", N = "_hologram ignore timer shot penal" }},
{ 0x88518, new C{ T="Comment", N = "_Parital Perception/Distance dist" }},
{ 0x88519, new C{ T="Comment", N = "_mapping" }},
{ 0x88520, new C{ T="Comment", N = "_" }},
{ 0x88, new C{ T="Unmapped", N = "_data" }},
{ 0xA0521, new C{ T="Comment", N = "_" }},
{ 0xA0522, new C{ T="Comment", N = "_Normal active-camo perception" }},
{ 0xA0523, new C{ T="Comment", N = "_normal active camo perception" }},
{ 0xA0, new C{ T="Float", N = "_partial invisibility amount" }},
{ 0xA4, new C{ T="Float", N = "_partial invisibility vision dist" }},
{ 0xA8, new C{ T="Float", N = "_full invisibility amount" }},
{ 0xAC, new C{ T="Float", N = "_full invisibility vision distanc" }},
{ 0xB0524, new C{ T="Comment", N = "_" }},
{ 0xB0525, new C{ T="Comment", N = "_Legendary active-camo perception" }},
{ 0xB0526, new C{ T="Comment", N = "_legendary active camo perception" }},
{ 0xB0, new C{ T="Float", N = "_partial invisibility amount" }},
{ 0xB4, new C{ T="Float", N = "_partial invisibility vision dist" }},
{ 0xB8, new C{ T="Float", N = "_full invisibility amount" }},
{ 0xBC, new C{ T="Float", N = "_full invisibility vision distanc" }},
{ 0xC0527, new C{ T="Comment", N = "_" }},
}, S=192}},
{ 0x198, new C{ T="Tagblock", N = "_target properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_player preference" }},
}, S=4}},
{ 0x1AC, new C{ T="Tagblock", N = "_look properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Unmapped", N = "_maximum aiming deviation" }},
{ 0xC, new C{ T="Unmapped", N = "_maximum looking deviation" }},
{ 0x18, new C{ T="Unmapped", N = "_runtime aiming deviation cosines" }},
{ 0x24, new C{ T="Unmapped", N = "_runtime looking deviation cosine" }},
{ 0x30, new C{ T="Float", N = "_noncombat look delta L" }},
{ 0x34, new C{ T="Float", N = "_noncombat look delta R" }},
{ 0x38, new C{ T="Float", N = "_combat look delta L" }},
{ 0x3C, new C{ T="Float", N = "_combat look delta R" }},
{ 0x40, new C{ T="Float", N = "_noncombat idle looking.min" }},
{ 0x44, new C{ T="Float", N = "_noncombat idle looking.max" }},
{ 0x48, new C{ T="Float", N = "_noncombat idle aiming.min" }},
{ 0x4C, new C{ T="Float", N = "_noncombat idle aiming.max" }},
{ 0x50, new C{ T="Float", N = "_combat idle looking.min" }},
{ 0x54, new C{ T="Float", N = "_combat idle looking.max" }},
{ 0x58, new C{ T="Float", N = "_combat idle aiming.min" }},
{ 0x5C, new C{ T="Float", N = "_combat idle aiming.max" }},
}, S=96}},
{ 0x1C0, new C{ T="Tagblock", N = "_hopping properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Hopping Flags" }},
{ 0x4, new C{ T="Tagblock", N = "_Hopping Definition", B = new Dictionary<long, C>
{
{ 0x0528, new C{ T="Comment", N = "_Default" }},
{ 0x0529, new C{ T="Comment", N = "_Default" }},
{ 0x0, new C{ T="Float", N = "_Min hop distance" }},
{ 0x4, new C{ T="Float", N = "_Min hop distance to path end" }},
{ 0x8, new C{ T="Float", N = "_Hop wait timer min/max.min" }},
{ 0xC, new C{ T="Float", N = "_Hop wait timer min/max.max" }},
{ 0x10, new C{ T="Float", N = "_Max hop distance" }},
{ 0x14, new C{ T="Float", N = "_pad" }},
{ 0x18530, new C{ T="Comment", N = "_" }},
{ 0x18531, new C{ T="Comment", N = "_Passive" }},
{ 0x18532, new C{ T="Comment", N = "_Passive" }},
{ 0x18, new C{ T="Float", N = "_Min hop distance" }},
{ 0x1C, new C{ T="Float", N = "_Min hop distance to path end" }},
{ 0x20, new C{ T="Float", N = "_Hop wait timer min/max.min" }},
{ 0x24, new C{ T="Float", N = "_Hop wait timer min/max.max" }},
{ 0x28, new C{ T="Float", N = "_Max hop distance" }},
{ 0x2C, new C{ T="Float", N = "_pad" }},
{ 0x30533, new C{ T="Comment", N = "_" }},
{ 0x30534, new C{ T="Comment", N = "_Aggressive" }},
{ 0x30535, new C{ T="Comment", N = "_Aggressive" }},
{ 0x30, new C{ T="Float", N = "_Min hop distance" }},
{ 0x34, new C{ T="Float", N = "_Min hop distance to path end" }},
{ 0x38, new C{ T="Float", N = "_Hop wait timer min/max.min" }},
{ 0x3C, new C{ T="Float", N = "_Hop wait timer min/max.max" }},
{ 0x40, new C{ T="Float", N = "_Max hop distance" }},
{ 0x44, new C{ T="Float", N = "_pad" }},
{ 0x48536, new C{ T="Comment", N = "_" }},
}, S=72}},
}, S=24}},
{ 0x1D4, new C{ T="Tagblock", N = "_warp properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_Warp Definition", B = new Dictionary<long, C>
{
{ 0x0537, new C{ T="Comment", N = "_Default" }},
{ 0x0538, new C{ T="Comment", N = "_Default" }},
{ 0x0, new C{ T="Float", N = "_min warp distance" }},
{ 0x4, new C{ T="Float", N = "_max warp distance" }},
{ 0x8, new C{ T="Float", N = "_warp speed" }},
{ 0xC, new C{ T="Float", N = "_run up distance" }},
{ 0x10, new C{ T="Float", N = "_linear distance until cooldown" }},
{ 0x14, new C{ T="Float", N = "_cooldown duration" }},
{ 0x18, new C{ T="Float", N = "_min jump distance" }},
{ 0x1C539, new C{ T="Comment", N = "_" }},
{ 0x1C540, new C{ T="Comment", N = "_Passive" }},
{ 0x1C541, new C{ T="Comment", N = "_Passive" }},
{ 0x1C, new C{ T="Float", N = "_min warp distance" }},
{ 0x20, new C{ T="Float", N = "_max warp distance" }},
{ 0x24, new C{ T="Float", N = "_warp speed" }},
{ 0x28, new C{ T="Float", N = "_run up distance" }},
{ 0x2C, new C{ T="Float", N = "_linear distance until cooldown" }},
{ 0x30, new C{ T="Float", N = "_cooldown duration" }},
{ 0x34, new C{ T="Float", N = "_min jump distance" }},
{ 0x38542, new C{ T="Comment", N = "_" }},
{ 0x38543, new C{ T="Comment", N = "_Aggressive" }},
{ 0x38544, new C{ T="Comment", N = "_Aggressive" }},
{ 0x38, new C{ T="Float", N = "_min warp distance" }},
{ 0x3C, new C{ T="Float", N = "_max warp distance" }},
{ 0x40, new C{ T="Float", N = "_warp speed" }},
{ 0x44, new C{ T="Float", N = "_run up distance" }},
{ 0x48, new C{ T="Float", N = "_linear distance until cooldown" }},
{ 0x4C, new C{ T="Float", N = "_cooldown duration" }},
{ 0x50, new C{ T="Float", N = "_min jump distance" }},
{ 0x54545, new C{ T="Comment", N = "_" }},
}, S=84}},
{ 0x14, new C{ T="TagRef", N = "_Intro Effect" }},
{ 0x30, new C{ T="TagRef", N = "_Travel Effect" }},
{ 0x4C, new C{ T="TagRef", N = "_Outro Effect" }},
}, S=104}},
{ 0x1E8, new C{ T="Tagblock", N = "_movement properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_movement flags" }},
{ 0x4, new C{ T="Float", N = "_pathfinding radius" }},
{ 0x8, new C{ T="Float", N = "_avoidance radius" }},
{ 0xC, new C{ T="Float", N = "_destination radius" }},
{ 0x10, new C{ T="Float", N = "_Friendly Outer Radius" }},
{ 0x14, new C{ T="Float", N = "_Friendly Inner Radius" }},
{ 0x18, new C{ T="Float", N = "_Friendly Player Outer Radius" }},
{ 0x1C, new C{ T="Float", N = "_Friendly Player Inner Radius" }},
{ 0x20546, new C{ T="Comment", N = "_Danger Zone Avoidance" }},
{ 0x20, new C{ T="2Byte", N = "_obstacle leap min size" }},
{ 0x22, new C{ T="2Byte", N = "_obstacle leap max size" }},
{ 0x24, new C{ T="2Byte", N = "_obstacle ignore size" }},
{ 0x26, new C{ T="2Byte", N = "_obstacle smashable size" }},
{ 0x28547, new C{ T="Comment", N = "_clearance cache" }},
{ 0x28, new C{ T="2Byte", N = "_clearance cache bucket size" }},
{ 0x2A, new C{ T="Unmapped", N = "_generated_pad024b" }},
{ 0x2C, new C{ T="Float", N = "_max jump height" }},
{ 0x30, new C{ T="Float", N = "_max jump distance" }},
{ 0x34, new C{ T="Float", N = "_maximum leap height" }},
{ 0x38, new C{ T="Float", N = "_leap proximity fraction" }},
{ 0x3C, new C{ T="Float", N = "_maximum hoist height" }},
{ 0x40, new C{ T="Float", N = "_obstacle smash strength" }},
{ 0x44, new C{ T="4Byte", N = "_movement hints" }},
{ 0x48548, new C{ T="Comment", N = "_Throttle and inertia" }},
{ 0x48, new C{ T="Tagblock", N = "_change direction pause", B = new Dictionary<long, C>
{
{ 0x0549, new C{ T="Comment", N = "_Inertial pause settings" }},
{ 0x0, new C{ T="Float", N = "_direction change angle" }},
{ 0x4, new C{ T="4Byte", N = "_stationary change" }},
}, S=8}},
{ 0x5C, new C{ T="Float", N = "_maximum throttle" }},
{ 0x60, new C{ T="Float", N = "_minimum throttle" }},
{ 0x64, new C{ T="Float", N = "_throttle smoothing rate" }},
{ 0x68, new C{ T="Tagblock", N = "_movement throttle control", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_combat status" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_padd966" }},
{ 0x4, new C{ T="Tagblock", N = "_throttle settings", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_distance" }},
{ 0x4, new C{ T="Float", N = "_throttle scale" }},
}, S=8}},
}, S=24}},
{ 0x7C, new C{ T="Float", N = "_minimum juke throttle" }},
{ 0x80, new C{ T="Float", N = "_minimum direction change juke an" }},
{ 0x84, new C{ T="Float", N = "_non direction change juke probab" }},
{ 0x88, new C{ T="Float", N = "_non direction change juke timeou" }},
{ 0x8C, new C{ T="4Byte", N = "_minimum post juke movement ticks" }},
{ 0x90550, new C{ T="Comment", N = "_" }},
{ 0x90, new C{ T="Float", N = "_stationary turn radius" }},
{ 0x94, new C{ T="Float", N = "_move distance.min" }},
{ 0x98, new C{ T="Float", N = "_move distance.max" }},
{ 0x9C551, new C{ T="Comment", N = "_Phasing" }},
{ 0x9C, new C{ T="Float", N = "_phase chance" }},
{ 0xA0, new C{ T="Float", N = "_phase delay seconds" }},
{ 0xA4552, new C{ T="Comment", N = "_" }},
{ 0xA4553, new C{ T="Comment", N = "_Movement Facing" }},
{ 0xA4554, new C{ T="Comment", N = "_Movement Facing" }},
{ 0xA4, new C{ T="Float", N = "_Maximum Deviation Angle" }},
{ 0xA8555, new C{ T="Comment", N = "_" }},
{ 0xA8556, new C{ T="Comment", N = "_locomotion settings" }},
{ 0xA8557, new C{ T="Comment", N = "_Locomotion Settings" }},
{ 0xA8, new C{ T="Float", N = "_Sharp Turn Throttle" }},
{ 0xAC, new C{ T="Float", N = "_Sharp Turn Angle" }},
{ 0xB0, new C{ T="Float", N = "_Max Accel Time" }},
{ 0xB4, new C{ T="Float", N = "_Max Decel Time" }},
{ 0xB8558, new C{ T="Comment", N = "_" }},
{ 0xB8, new C{ T="Float", N = "_wall climb cost multiplier " }},
{ 0xBC559, new C{ T="Comment", N = "_Character flight" }},
{ 0xBC, new C{ T="Byte", N = "_Air nav firing point position" }},
{ 0xBD560, new C{ T="Comment", N = "_" }},
{ 0xBD, new C{ T="Unmapped", N = "_generated_padb596" }},
}, S=192}},
{ 0x1FC, new C{ T="Tagblock", N = "_aiming-facing properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_Flags" }},
{ 0x1, new C{ T="Byte", N = "_Default facing mode" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_padbf57" }},
{ 0x4561, new C{ T="Comment", N = "_Baseball run properties" }},
{ 0x4562, new C{ T="Comment", N = "_Baseball run properties" }},
{ 0x4, new C{ T="Float", N = "_Strafe to run rate" }},
{ 0x8, new C{ T="Float", N = "_Run to strafe rate" }},
{ 0xC, new C{ T="Float", N = "_Start distance threshold" }},
{ 0x10, new C{ T="Float", N = "_End distance threshold" }},
{ 0x14, new C{ T="Float", N = "_Path minimum length" }},
{ 0x18, new C{ T="Float", N = "_Minimum distance to target" }},
{ 0x1C563, new C{ T="Comment", N = "_" }},
}, S=28}},
{ 0x210, new C{ T="Tagblock", N = "_decelerated turns properties", B = new Dictionary<long, C>
{
{ 0x0564, new C{ T="Comment", N = "_Turn Slerp Blend Table" }},
{ 0x0565, new C{ T="Comment", N = "_Turn Slerp Blend Table Table" }},
{ 0x0566, new C{ T="Comment", N = "_Turn Slerp Blend Table" }},
{ 0x0567, new C{ T="Comment", N = "_" }},
{ 0x0, new C{ T="Unmapped", N = "_data" }},
{ 0x18568, new C{ T="Comment", N = "_" }},
{ 0x18569, new C{ T="Comment", N = "_" }},
{ 0x18570, new C{ T="Comment", N = "_Turn Anticipation Blend Table" }},
{ 0x18571, new C{ T="Comment", N = "_Turn Anticipation Blend Table" }},
{ 0x18572, new C{ T="Comment", N = "_Turn Anticipation Blend Table" }},
{ 0x18573, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x30574, new C{ T="Comment", N = "_" }},
{ 0x30575, new C{ T="Comment", N = "_" }},
{ 0x30576, new C{ T="Comment", N = "_Turn Reaction Blend Table" }},
{ 0x30577, new C{ T="Comment", N = "_Turn Reaction Blend Table" }},
{ 0x30578, new C{ T="Comment", N = "_Turn Reaction Blend Table" }},
{ 0x30579, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Unmapped", N = "_data" }},
{ 0x48580, new C{ T="Comment", N = "_" }},
{ 0x48581, new C{ T="Comment", N = "_" }},
{ 0x48, new C{ T="Float", N = "_Turn reaction cosine power" }},
{ 0x4C, new C{ T="Float", N = "_Turn reaction cosine factor" }},
}, S=80}},
{ 0x224, new C{ T="Tagblock", N = "_locomotion overrides", B = new Dictionary<long, C>
{
{ 0x0582, new C{ T="Comment", N = "_locomotion overrides" }},
{ 0x0, new C{ T="Byte", N = "_Flags" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_pad2640" }},
{ 0x4583, new C{ T="Comment", N = "_Walk speed properties" }},
{ 0x4, new C{ T="Float", N = "_walking speed" }},
{ 0x8584, new C{ T="Comment", N = "_AI Sprint speed properties" }},
{ 0x8, new C{ T="Float", N = "_AI sprinting speed" }},
{ 0xC585, new C{ T="Comment", N = "_Run speed properties" }},
{ 0xC, new C{ T="Float", N = "_running forward speed" }},
{ 0x10, new C{ T="Float", N = "_running backward speed" }},
{ 0x14, new C{ T="Float", N = "_running sideways speed" }},
{ 0x18586, new C{ T="Comment", N = "_Crouch speed properties" }},
{ 0x18, new C{ T="Float", N = "_crouching forward speed" }},
{ 0x1C, new C{ T="Float", N = "_crouching backward speed" }},
{ 0x20, new C{ T="Float", N = "_crouching sideways speed" }},
{ 0x24587, new C{ T="Comment", N = "_Acceleration/deceleration proper" }},
{ 0x24, new C{ T="Float", N = "_maximum run acceleration" }},
{ 0x28, new C{ T="Float", N = "_maximum run deceleration" }},
{ 0x2C, new C{ T="Float", N = "_maximum crouch acceleration" }},
{ 0x30, new C{ T="Float", N = "_maximum crouch deceleration" }},
{ 0x34, new C{ T="Float", N = "_minimum slip recovery accelerati" }},
{ 0x38588, new C{ T="Comment", N = "_Airborne Acceleration/Decelerati" }},
{ 0x38, new C{ T="Float", N = "_maximum airborne acceleration" }},
{ 0x3C, new C{ T="Float", N = "_maximum airborne deceleration" }},
{ 0x40589, new C{ T="Comment", N = "_Stationary turn properties old a" }},
{ 0x40, new C{ T="Float", N = "_low speed threshold" }},
{ 0x44, new C{ T="Float", N = "_high speed threshold" }},
{ 0x48, new C{ T="Float", N = "_acceleration" }},
{ 0x4C590, new C{ T="Comment", N = "_Stationary turn properties new a" }},
{ 0x4C, new C{ T="Float", N = "_stationary turn trigger range fa" }},
{ 0x50, new C{ T="Float", N = "_stationary turn max anim playbac" }},
{ 0x54591, new C{ T="Comment", N = "_Stationary turn properties new a" }},
{ 0x54, new C{ T="Float", N = "_min stationary turn angle" }},
{ 0x58, new C{ T="Float", N = "_stationary turn max speed thresh" }},
{ 0x5C, new C{ T="Float", N = "_stationary turn min yaw threshol" }},
{ 0x60, new C{ T="Float", N = "_stationary turn max speed yaw" }},
{ 0x64, new C{ T="Float", N = "_min stationary turn speed" }},
{ 0x68, new C{ T="Float", N = "_max stationary turn speed" }},
{ 0x6C, new C{ T="Float", N = "_stationary turn acceleration rat" }},
{ 0x70, new C{ T="Float", N = "_stationary turn decceleration ra" }},
{ 0x74592, new C{ T="Comment", N = "_Non stationary turn properties" }},
{ 0x74, new C{ T="Float", N = "_max angular velocity clamp" }},
{ 0x78, new C{ T="Float", N = "_turn accel/decel rate" }},
{ 0x7C593, new C{ T="Comment", N = "_Speed/acceleration scales based " }},
{ 0x7C, new C{ T="Tagblock", N = "_Mode specific scaling", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_mode name" }},
{ 0x4, new C{ T="Float", N = "_speed scale" }},
{ 0x8, new C{ T="Float", N = "_acceleration scale" }},
{ 0xC, new C{ T="Float", N = "_locomotion turn rate scale" }},
}, S=16}},
}, S=144}},
{ 0x238, new C{ T="Tagblock", N = "_movement tweak properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Dive grenade chance" }},
{ 0x4, new C{ T="Float", N = "_Brace grenade chance" }},
{ 0x8, new C{ T="Float", N = "_Dive vehicle chance" }},
{ 0xC, new C{ T="Float", N = "_Brace vehicle chance" }},
{ 0x10, new C{ T="Float", N = "_Stand Ground Chance" }},
{ 0x14, new C{ T="Float", N = "_Stand Ground Anticipation Time" }},
{ 0x18, new C{ T="Float", N = "_Brace For Grenade Time" }},
{ 0x1C, new C{ T="Float", N = "_Brace For Vehicle Impact Time" }},
{ 0x20, new C{ T="Float", N = "_Brace For Vehicle Impact Predict" }},
{ 0x24, new C{ T="Float", N = "_Brace For Vehicle Impact Velocit" }},
{ 0x28, new C{ T="2Byte", N = "_jump height" }},
{ 0x2A, new C{ T="Unmapped", N = "_generated_pad113f" }},
{ 0x2C, new C{ T="Tagblock", N = "_Full Body Animation Jump Prevent", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Animation exception name" }},
}, S=4}},
}, S=64}},
{ 0x24C, new C{ T="Tagblock", N = "_throttle styles", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_style name" }},
{ 0x4, new C{ T="Float", N = "_desired throttle" }},
{ 0x8, new C{ T="Float", N = "_acceleration time" }},
{ 0xC594, new C{ T="Comment", N = "_acceleration function" }},
{ 0xC595, new C{ T="Comment", N = "_function" }},
{ 0xC596, new C{ T="Comment", N = "_function" }},
{ 0xC597, new C{ T="Comment", N = "_" }},
{ 0xC, new C{ T="Unmapped", N = "_data" }},
{ 0x24598, new C{ T="Comment", N = "_" }},
{ 0x24, new C{ T="Float", N = "_deceleration distance" }},
{ 0x28599, new C{ T="Comment", N = "_deceleration function" }},
{ 0x28600, new C{ T="Comment", N = "_function" }},
{ 0x28601, new C{ T="Comment", N = "_function" }},
{ 0x28602, new C{ T="Comment", N = "_" }},
{ 0x28, new C{ T="Unmapped", N = "_data" }},
{ 0x40603, new C{ T="Comment", N = "_" }},
{ 0x40, new C{ T="mmr3Hash", N = "_stance" }},
}, S=68}},
{ 0x260, new C{ T="Tagblock", N = "_movement sets", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_name" }},
{ 0x4, new C{ T="Tagblock", N = "_variants", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_chance" }},
{ 0x4604, new C{ T="Comment", N = "_MAPPING" }},
{ 0x4, new C{ T="mmr3Hash", N = "_idle" }},
{ 0x8, new C{ T="mmr3Hash", N = "_alert" }},
{ 0xC, new C{ T="mmr3Hash", N = "_engage" }},
{ 0x10, new C{ T="mmr3Hash", N = "_self_preserve" }},
{ 0x14, new C{ T="mmr3Hash", N = "_search" }},
{ 0x18, new C{ T="mmr3Hash", N = "_retreat" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_panic" }},
{ 0x20, new C{ T="mmr3Hash", N = "_flank" }},
{ 0x24, new C{ T="mmr3Hash", N = "_protected" }},
{ 0x28, new C{ T="mmr3Hash", N = "_stunned" }},
{ 0x2C, new C{ T="mmr3Hash", N = "_post_combat" }},
{ 0x30, new C{ T="mmr3Hash", N = "_hurry" }},
{ 0x34, new C{ T="mmr3Hash", N = "_custom_1" }},
{ 0x38, new C{ T="mmr3Hash", N = "_custom_2" }},
{ 0x3C, new C{ T="mmr3Hash", N = "_custom_3" }},
{ 0x40, new C{ T="mmr3Hash", N = "_custom_4" }},
}, S=68}},
}, S=24}},
{ 0x274, new C{ T="Tagblock", N = "_flocking properties", B = new Dictionary<long, C>
{
{ 0x0605, new C{ T="Comment", N = "_Jaime, don't touch this" }},
{ 0x0, new C{ T="Float", N = "_deceleration distance" }},
{ 0x4, new C{ T="Float", N = "_normalized speed" }},
{ 0x8, new C{ T="Float", N = "_buffer distance" }},
{ 0xC, new C{ T="Float", N = "_throttle threshold bounds.min" }},
{ 0x10, new C{ T="Float", N = "_throttle threshold bounds.max" }},
{ 0x14, new C{ T="Float", N = "_deceleration stop time" }},
}, S=24}},
{ 0x288, new C{ T="Tagblock", N = "_swarm properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="2Byte", N = "_scatter killed count" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_pad8e1a" }},
{ 0x4, new C{ T="Float", N = "_scatter radius" }},
{ 0x8, new C{ T="Float", N = "_scatter time" }},
{ 0xC, new C{ T="Float", N = "_hound min distance" }},
{ 0x10, new C{ T="Float", N = "_hound max distance" }},
{ 0x14, new C{ T="Float", N = "_perlin offset scale" }},
{ 0x18, new C{ T="Float", N = "_offset period.min" }},
{ 0x1C, new C{ T="Float", N = "_offset period.max" }},
{ 0x20, new C{ T="Float", N = "_perlin idle movement threshold" }},
{ 0x24, new C{ T="Float", N = "_perlin combat movement threshold" }},
{ 0x28, new C{ T="Float", N = "_stuck time" }},
{ 0x2C, new C{ T="Float", N = "_stuck distance" }},
}, S=48}},
{ 0x29C, new C{ T="Tagblock", N = "_ready properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_ready time bounds.min" }},
{ 0x4, new C{ T="Float", N = "_ready time bounds.max" }},
}, S=8}},
{ 0x2B0, new C{ T="Tagblock", N = "_engage properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_flags" }},
{ 0x4, new C{ T="Float", N = "_Reposition bounds.min" }},
{ 0x8, new C{ T="Float", N = "_Reposition bounds.max" }},
{ 0xC, new C{ T="Float", N = "_Bunkering Reposition bounds.min" }},
{ 0x10, new C{ T="Float", N = "_Bunkering Reposition bounds.max" }},
{ 0x14606, new C{ T="Comment", N = "_Danger Crouch" }},
{ 0x14, new C{ T="Float", N = "_Crouch danger threshold" }},
{ 0x18, new C{ T="Float", N = "_Crouch max path distance" }},
{ 0x1C, new C{ T="Float", N = "_Stand danger threshold" }},
{ 0x20, new C{ T="Float", N = "_Fight danger move threshold" }},
{ 0x24, new C{ T="Unmapped", N = "_Fight danger move threshold cool" }},
{ 0x28, new C{ T="TagRef", N = "_override grenade projectile" }},
{ 0x44, new C{ T="Float", N = "_default combat range.min" }},
{ 0x48, new C{ T="Float", N = "_default combat range.max" }},
{ 0x4C, new C{ T="Float", N = "_default firing range.min" }},
{ 0x50, new C{ T="Float", N = "_default firing range.max" }},
{ 0x54, new C{ T="Float", N = "_Preferred engage fraction " }},
{ 0x58, new C{ T="Float", N = "_Active Shield Fire Cutoff Delay" }},
{ 0x5C, new C{ T="Float", N = "_Friendly Avoid Distance" }},
{ 0x60, new C{ T="Float", N = "_Friendly Close Avoid Distance" }},
{ 0x64, new C{ T="Float", N = "_TeamLead flocking distance" }},
{ 0x68, new C{ T="Float", N = "_Enemy avoid distance" }},
{ 0x6C, new C{ T="Float", N = "_Scary enemy avoid distance" }},
{ 0x70, new C{ T="Tagblock", N = "_Full Body Animation Exceptions f", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Animation exception name" }},
}, S=4}},
{ 0x84607, new C{ T="Comment", N = "_Flying" }},
{ 0x84, new C{ T="Float", N = "_Max angle from level" }},
}, S=136}},
{ 0x2C4, new C{ T="Tagblock", N = "_Berserk Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="TagRef", N = "_Berserk Weapon" }},
{ 0x20, new C{ T="Float", N = "_Timeout" }},
{ 0x24, new C{ T="Unmapped", N = "_Shield down chance" }},
{ 0x28, new C{ T="Float", N = "_Shield down range.min" }},
{ 0x2C, new C{ T="Float", N = "_Shield down range.max" }},
{ 0x30, new C{ T="Float", N = "_Friendly killed distance" }},
{ 0x34, new C{ T="Float", N = "_Peer killed chance" }},
{ 0x38, new C{ T="Float", N = "_Leader killed chance" }},
{ 0x3C, new C{ T="Float", N = "_Chance to play berserk anim" }},
{ 0x40, new C{ T="Float", N = "_Chance to play berserk anim when" }},
{ 0x44, new C{ T="Float", N = "_Proximity chance" }},
{ 0x48, new C{ T="Float", N = "_Proximity check cooldown" }},
{ 0x4C, new C{ T="Float", N = "_Proximity abort distance" }},
{ 0x50, new C{ T="Float", N = "_Broken kamikaze chance" }},
{ 0x54, new C{ T="Float", N = "_Surprise kamikaze chance" }},
{ 0x58, new C{ T="Tagblock", N = "_Kamikaze attachment Markers", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Attachment Marker" }},
}, S=4}},
{ 0x6C, new C{ T="Float", N = "_Broken Kamikaze stuck delay" }},
{ 0x70, new C{ T="Float", N = "_Max kamikaze time" }},
{ 0x74, new C{ T="Float", N = "_Last man trigger distance" }},
{ 0x78, new C{ T="Tagblock", N = "_Difficulty limits", B = new Dictionary<long, C>
{
{ 0x0608, new C{ T="Comment", N = "_Difficulty Limits" }},
{ 0x0, new C{ T="2Byte", N = "_max kamikaze count" }},
{ 0x2, new C{ T="2Byte", N = "_max berserk count" }},
{ 0x4, new C{ T="2Byte", N = "_min berserk count" }},
}, S=6}},
}, S=140}},
{ 0x2D8, new C{ T="Tagblock", N = "_Weapon Pickup Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Search range" }},
{ 0x4, new C{ T="Float", N = "_Minimum target range" }},
{ 0x8, new C{ T="Float", N = "_Range threshold" }},
{ 0xC, new C{ T="Float", N = "_Vehicle threshold" }},
{ 0x10, new C{ T="Float", N = "_Search delay min" }},
{ 0x14, new C{ T="Float", N = "_Search delay max" }},
{ 0x18, new C{ T="Byte", N = "_Bias" }},
{ 0x19, new C{ T="Unmapped", N = "_generated_pad5304" }},
{ 0x1C, new C{ T="Float", N = "_Danger threshold" }},
}, S=32}},
{ 0x2EC609, new C{ T="Comment", N = "_Danger Values" }},
{ 0x2EC, new C{ T="Tagblock", N = "_evasion properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Evasion danger threshold" }},
{ 0x4, new C{ T="Float", N = "_Evasion delay timer" }},
{ 0x8, new C{ T="Float", N = "_Evasion chance" }},
{ 0xC, new C{ T="Float", N = "_Evasion proximity threshold" }},
{ 0x10, new C{ T="Float", N = "_dive retreat chance" }},
}, S=20}},
{ 0x300, new C{ T="Tagblock", N = "_Dodge properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Danger threshold" }},
{ 0x4, new C{ T="Float", N = "_Cooldown" }},
{ 0x8, new C{ T="Float", N = "_Chance" }},
{ 0xC, new C{ T="Float", N = "_Stop proximity" }},
}, S=16}},
{ 0x314, new C{ T="Tagblock", N = "_Run Away From Vehicle properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Chance" }},
{ 0x4, new C{ T="Float", N = "_Minimum time to flee" }},
{ 0x8, new C{ T="Float", N = "_Time to flee" }},
{ 0xC, new C{ T="Float", N = "_Time to keep path" }},
}, S=16}},
{ 0x328, new C{ T="Tagblock", N = "_cover properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_cover flags" }},
{ 0x4, new C{ T="Float", N = "_hide behind cover time.min" }},
{ 0x8, new C{ T="Float", N = "_hide behind cover time.max" }},
{ 0xC610, new C{ T="Comment", N = "_Cover conditions" }},
{ 0xC, new C{ T="Float", N = "_Cover shield fraction" }},
{ 0x10, new C{ T="Float", N = "_Cover vitality threshold" }},
{ 0x14, new C{ T="Float", N = "_Cover danger threshold" }},
{ 0x18, new C{ T="Float", N = "_Leave cover danger threshold" }},
{ 0x1C611, new C{ T="Comment", N = "_Aggresive/Defensive cover proper" }},
{ 0x1C, new C{ T="Float", N = "_minimum defensive distance from " }},
{ 0x20, new C{ T="Float", N = "_minimum defensive distance from " }},
{ 0x24, new C{ T="Float", N = "_always defensive scary threshold" }},
{ 0x28612, new C{ T="Comment", N = "_" }},
{ 0x28613, new C{ T="Comment", N = "_Other" }},
{ 0x28, new C{ T="Float", N = "_Cover check delay" }},
{ 0x2C, new C{ T="Float", N = "_Cover pinned down check delay" }},
{ 0x30, new C{ T="Float", N = "_Emerge from cover when shield fr" }},
{ 0x34, new C{ T="Float", N = "_Body Vitality Exit Threshold" }},
{ 0x38, new C{ T="Float", N = "_Proximity self-preserve" }},
{ 0x3C, new C{ T="Float", N = "_unreachable enemy danger thresho" }},
{ 0x40, new C{ T="Float", N = "_scary target threshold" }},
{ 0x44, new C{ T="Float", N = "_Minimum enemy distance" }},
}, S=72}},
{ 0x33C, new C{ T="Tagblock", N = "_retreat properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Retreat flags" }},
{ 0x4, new C{ T="Float", N = "_Shield threshold" }},
{ 0x8, new C{ T="Float", N = "_Scary target threshold" }},
{ 0xC, new C{ T="Float", N = "_Danger threshold" }},
{ 0x10, new C{ T="Float", N = "_Proximity threshold" }},
{ 0x14, new C{ T="Float", N = "_min/max forced cower time bounds.min" }},
{ 0x18, new C{ T="Float", N = "_min/max forced cower time bounds.max" }},
{ 0x1C, new C{ T="Float", N = "_min/max cower timeout bounds.min" }},
{ 0x20, new C{ T="Float", N = "_min/max cower timeout bounds.max" }},
{ 0x24, new C{ T="Float", N = "_proximity ambush threshold" }},
{ 0x28, new C{ T="Float", N = "_awareness ambush threshold" }},
{ 0x2C, new C{ T="Float", N = "_leader dead retreat chance" }},
{ 0x30, new C{ T="Float", N = "_peer dead retreat chance" }},
{ 0x34, new C{ T="Float", N = "_second peer dead retreat chance" }},
{ 0x38, new C{ T="Float", N = "_flee timeout" }},
{ 0x3C, new C{ T="Float", N = "_zig-zag angle" }},
{ 0x40, new C{ T="Float", N = "_zig-zag period" }},
{ 0x44, new C{ T="Float", N = "_retreat grenade chance" }},
}, S=72}},
{ 0x350, new C{ T="Tagblock", N = "_search properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Search flags" }},
{ 0x4, new C{ T="Float", N = "_search time.min" }},
{ 0x8, new C{ T="Float", N = "_search time.max" }},
{ 0xC, new C{ T="Float", N = "_Search distance" }},
{ 0x10, new C{ T="Float", N = "_Max Searcher Count " }},
{ 0x14614, new C{ T="Comment", N = "_Uncover" }},
{ 0x14, new C{ T="Float", N = "_Uncover distance bounds.min" }},
{ 0x18, new C{ T="Float", N = "_Uncover distance bounds.max" }},
{ 0x1C615, new C{ T="Comment", N = "_Investigate" }},
{ 0x1C, new C{ T="Float", N = "_vocalization time.min" }},
{ 0x20, new C{ T="Float", N = "_vocalization time.max" }},
}, S=36}},
{ 0x364, new C{ T="Tagblock", N = "_pre-search properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Pre-search flags" }},
{ 0x4, new C{ T="Float", N = "_min presearch time.min" }},
{ 0x8, new C{ T="Float", N = "_min presearch time.max" }},
{ 0xC, new C{ T="Float", N = "_max presearch time.min" }},
{ 0x10, new C{ T="Float", N = "_max presearch time.max" }},
{ 0x14, new C{ T="Float", N = "_Peek Time.min" }},
{ 0x18, new C{ T="Float", N = "_Peek Time.max" }},
{ 0x1C, new C{ T="Float", N = "_Max Certainty Radius" }},
{ 0x20, new C{ T="Float", N = "_Max Presearcher Count " }},
{ 0x24, new C{ T="Float", N = "_Max Suppressing Count" }},
{ 0x28, new C{ T="Float", N = "_Max Uncover Count" }},
{ 0x2C, new C{ T="4Byte", N = "_Max Destroy Cover Count" }},
{ 0x30, new C{ T="Float", N = "_max suppress time" }},
{ 0x34, new C{ T="Float", N = "_fire to uncover chance " }},
{ 0x38, new C{ T="Float", N = "_Max destroy cover time" }},
{ 0x3C616, new C{ T="Comment", N = "_Child Weights" }},
{ 0x3C, new C{ T="Float", N = "_suppressing fire weight" }},
{ 0x40, new C{ T="Float", N = "_uncover weight" }},
{ 0x44, new C{ T="Float", N = "_leap on cover weight" }},
{ 0x48, new C{ T="Float", N = "_destroy cover weight" }},
{ 0x4C, new C{ T="Float", N = "_guard weight" }},
{ 0x50, new C{ T="Float", N = "_investigate weight" }},
{ 0x54, new C{ T="Float", N = "_search by fire weight" }},
{ 0x58617, new C{ T="Comment", N = "_" }},
}, S=88}},
{ 0x378, new C{ T="Tagblock", N = "_Recognizing Properties", B = new Dictionary<long, C>
{
{ 0x0618, new C{ T="Comment", N = "_Recognizing" }},
{ 0x0, new C{ T="Float", N = "_Recognize min time" }},
{ 0x4, new C{ T="Float", N = "_Recognize max time" }},
}, S=8}},
{ 0x38C, new C{ T="Tagblock", N = "_idle properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_idle pose delay time.min" }},
{ 0x4, new C{ T="Float", N = "_idle pose delay time.max" }},
{ 0x8, new C{ T="Float", N = "_wander delay time.min" }},
{ 0xC, new C{ T="Float", N = "_wander delay time.max" }},
}, S=16}},
{ 0x3A0, new C{ T="Tagblock", N = "_Precombat Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_Systematic Recreation Settings", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_Required Weapon Class" }},
{ 0x4, new C{ T="Float", N = "_Cooldown.min" }},
{ 0x8, new C{ T="Float", N = "_Cooldown.max" }},
{ 0xC, new C{ T="Float", N = "_Duration.min" }},
{ 0x10, new C{ T="Float", N = "_Duration.max" }},
}, S=20}},
}, S=20}},
{ 0x3B4, new C{ T="Tagblock", N = "_vocalization properties", B = new Dictionary<long, C>
{
{ 0x0619, new C{ T="Comment", N = "_Skip Fraction" }},
{ 0x0, new C{ T="Float", N = "_character skip fraction" }},
{ 0x4, new C{ T="Float", N = "_speaker weight bonus" }},
{ 0x8, new C{ T="Float", N = "_look trigger distance" }},
{ 0xC, new C{ T="Float", N = "_look cooldown time" }},
{ 0x10, new C{ T="Float", N = "_look comment time" }},
{ 0x14, new C{ T="Float", N = "_look long comment time" }},
{ 0x18, new C{ T="Float", N = "_look respond max time" }},
{ 0x1C, new C{ T="Float", N = "_look respond early out time" }},
}, S=32}},
{ 0x3C8, new C{ T="Tagblock", N = "_boarding properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_rule name" }},
{ 0x4, new C{ T="4Byte", N = "_flags" }},
{ 0x8, new C{ T="Float", N = "_Boarding Distance" }},
{ 0xC, new C{ T="Float", N = "_abort distance" }},
{ 0x10, new C{ T="Float", N = "_Minimum Entry Distance" }},
{ 0x14, new C{ T="Float", N = "_Maximum Entry Distance" }},
{ 0x18, new C{ T="Float", N = "_max speed" }},
{ 0x1C, new C{ T="Float", N = "_board time" }},
{ 0x20, new C{ T="Float", N = "_boarding timeout.min" }},
{ 0x24, new C{ T="Float", N = "_boarding timeout.max" }},
{ 0x28, new C{ T="Tagblock", N = "_vehicle specific properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_vehicle" }},
{ 0x1C, new C{ T="4Byte", N = "_flags" }},
}, S=32}},
{ 0x3C, new C{ T="Tagblock", N = "_vehicle specific pull properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_vehicle" }},
{ 0x1C, new C{ T="Float", N = "_max pull" }},
{ 0x20, new C{ T="Float", N = "_min linear acceleration" }},
{ 0x24, new C{ T="Float", N = "_pull history factor" }},
}, S=40}},
{ 0x50620, new C{ T="Comment", N = "_New boarding behavior" }},
{ 0x50, new C{ T="Float", N = "_max danger" }},
{ 0x54, new C{ T="Float", N = "_max pull" }},
{ 0x58, new C{ T="Float", N = "_min linear acceleration" }},
{ 0x5C, new C{ T="Float", N = "_pull history factor" }},
{ 0x60, new C{ T="Float", N = "_Ejection Knockback Time" }},
{ 0x64, new C{ T="Float", N = "_entry timeout" }},
{ 0x68, new C{ T="Float", N = "_Boarding Chance" }},
{ 0x6C, new C{ T="Float", N = "_Hijack Max Wait Time" }},
{ 0x70, new C{ T="Float", N = "_Min Vehicle Vitality" }},
{ 0x74, new C{ T="Float", N = "_melee cooldown.min" }},
{ 0x78, new C{ T="Float", N = "_melee cooldown.max" }},
}, S=124}},
{ 0x3DC, new C{ T="Tagblock", N = "_kungfu properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_kungfu override distance" }},
{ 0x4, new C{ T="Float", N = "_kungfu cover danger threshold" }},
{ 0x8, new C{ T="Float", N = "_Min Allowed Time" }},
{ 0xC, new C{ T="Float", N = "_Min Disallowed Time" }},
}, S=16}},
{ 0x3F0, new C{ T="Tagblock", N = "_bunker properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4621, new C{ T="Comment", N = "_DEFAULT TIMINGS" }},
{ 0x4622, new C{ T="Comment", N = "_default" }},
{ 0x4, new C{ T="Float", N = "_open time.min" }},
{ 0x8, new C{ T="Float", N = "_open time.max" }},
{ 0xC, new C{ T="Float", N = "_closed min time" }},
{ 0x10, new C{ T="Float", N = "_close danger level" }},
{ 0x14, new C{ T="Float", N = "_open chance" }},
{ 0x18, new C{ T="Float", N = "_peek chance" }},
{ 0x1C623, new C{ T="Comment", N = "_FIGHT TIMINGS" }},
{ 0x1C624, new C{ T="Comment", N = "_fight" }},
{ 0x1C, new C{ T="Float", N = "_open time.min" }},
{ 0x20, new C{ T="Float", N = "_open time.max" }},
{ 0x24, new C{ T="Float", N = "_closed min time" }},
{ 0x28, new C{ T="Float", N = "_close danger level" }},
{ 0x2C, new C{ T="Float", N = "_open chance" }},
{ 0x30, new C{ T="Float", N = "_peek chance" }},
{ 0x34625, new C{ T="Comment", N = "_COVER TIMINGS" }},
{ 0x34626, new C{ T="Comment", N = "_cover" }},
{ 0x34, new C{ T="Float", N = "_open time.min" }},
{ 0x38, new C{ T="Float", N = "_open time.max" }},
{ 0x3C, new C{ T="Float", N = "_closed min time" }},
{ 0x40, new C{ T="Float", N = "_close danger level" }},
{ 0x44, new C{ T="Float", N = "_open chance" }},
{ 0x48, new C{ T="Float", N = "_peek chance" }},
{ 0x4C627, new C{ T="Comment", N = "_GUARD TIMINGS" }},
{ 0x4C628, new C{ T="Comment", N = "_guard" }},
{ 0x4C, new C{ T="Float", N = "_open time.min" }},
{ 0x50, new C{ T="Float", N = "_open time.max" }},
{ 0x54, new C{ T="Float", N = "_closed min time" }},
{ 0x58, new C{ T="Float", N = "_close danger level" }},
{ 0x5C, new C{ T="Float", N = "_open chance" }},
{ 0x60, new C{ T="Float", N = "_peek chance" }},
}, S=100}},
{ 0x404, new C{ T="Tagblock", N = "_shield wall properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Raise Shield Delay" }},
{ 0x4, new C{ T="Float", N = "_Relocate Delay" }},
{ 0x8, new C{ T="Float", N = "_Max Leader Protect Distance" }},
{ 0xC629, new C{ T="Comment", N = "_Formation" }},
{ 0xC630, new C{ T="Comment", N = "_Formation Description" }},
{ 0xC631, new C{ T="Comment", N = "_Procedural" }},
{ 0xC, new C{ T="4Byte", N = "_Shape" }},
{ 0x10, new C{ T="Float", N = "_Yaw" }},
{ 0x14, new C{ T="Float", N = "_Spacing" }},
{ 0x18, new C{ T="4Byte", N = "_Max Rectangle Width" }},
{ 0x1C632, new C{ T="Comment", N = "_Custom" }},
{ 0x1C, new C{ T="TagRef", N = "_Formation Reference" }},
{ 0x38633, new C{ T="Comment", N = "_Common" }},
{ 0x38, new C{ T="Float", N = "_Max Speed" }},
{ 0x3C, new C{ T="Float", N = "_Max Yaw Rate" }},
{ 0x40634, new C{ T="Comment", N = "_" }},
}, S=64}},
{ 0x418, new C{ T="Tagblock", N = "_firing wall properties", B = new Dictionary<long, C>
{
{ 0x0635, new C{ T="Comment", N = "_Activation Chance" }},
{ 0x0, new C{ T="Float", N = "_Activation Chance (easy)" }},
{ 0x4, new C{ T="Float", N = "_Activation Chance (normal)" }},
{ 0x8, new C{ T="Float", N = "_Activation Chance (heroic)" }},
{ 0xC, new C{ T="Float", N = "_Activation Chance (legendary)" }},
{ 0x10636, new C{ T="Comment", N = "_" }},
{ 0x10, new C{ T="Float", N = "_Max Distance" }},
{ 0x14, new C{ T="4Byte", N = "_Min Member Count" }},
{ 0x18, new C{ T="4Byte", N = "_Max Member Count" }},
{ 0x1C, new C{ T="Float", N = "_Fire Time.min" }},
{ 0x20, new C{ T="Float", N = "_Fire Time.max" }},
{ 0x24, new C{ T="Float", N = "_Cooldown" }},
{ 0x28, new C{ T="Float", N = "_Warp In Time" }},
{ 0x2C, new C{ T="Float", N = "_Min Arrival Time Variation" }},
{ 0x30, new C{ T="Float", N = "_Max Arrival Time Variation" }},
{ 0x34, new C{ T="Float", N = "_Warp Out Time" }},
{ 0x38, new C{ T="Float", N = "_Min Departure Time Variation" }},
{ 0x3C, new C{ T="Float", N = "_Max Departure Time Variation" }},
{ 0x40, new C{ T="Float", N = "_Max Angle Between Formation and " }},
{ 0x44637, new C{ T="Comment", N = "_Formation" }},
{ 0x44638, new C{ T="Comment", N = "_Formation Description" }},
{ 0x44639, new C{ T="Comment", N = "_Procedural" }},
{ 0x44, new C{ T="4Byte", N = "_Shape" }},
{ 0x48, new C{ T="Float", N = "_Yaw" }},
{ 0x4C, new C{ T="Float", N = "_Spacing" }},
{ 0x50, new C{ T="4Byte", N = "_Max Rectangle Width" }},
{ 0x54640, new C{ T="Comment", N = "_Custom" }},
{ 0x54, new C{ T="TagRef", N = "_Formation Reference" }},
{ 0x70641, new C{ T="Comment", N = "_Common" }},
{ 0x70, new C{ T="Float", N = "_Max Speed" }},
{ 0x74, new C{ T="Float", N = "_Max Yaw Rate" }},
{ 0x78642, new C{ T="Comment", N = "_" }},
}, S=120}},
{ 0x42C, new C{ T="Tagblock", N = "_Interpose Request Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Danger Level" }},
{ 0x4, new C{ T="Float", N = "_Wait Time.min" }},
{ 0x8, new C{ T="Float", N = "_Wait Time.max" }},
{ 0xC, new C{ T="Float", N = "_Cooldown" }},
}, S=16}},
{ 0x440, new C{ T="Tagblock", N = "_Interpose Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Protect Time.min" }},
{ 0x4, new C{ T="Float", N = "_Protect Time.max" }},
{ 0x8, new C{ T="Float", N = "_Distance from Protect" }},
{ 0xC, new C{ T="mmr3Hash", N = "_Firing Style" }},
}, S=16}},
{ 0x454, new C{ T="Tagblock", N = "_bounding properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Max Wait Time" }},
{ 0x4, new C{ T="Float", N = "_Advance Time" }},
{ 0x8, new C{ T="Float", N = "_Reposition Search Range" }},
{ 0xC, new C{ T="Float", N = "_Max Destination Age" }},
{ 0x10, new C{ T="Byte", N = "_Max Cover Agents" }},
{ 0x11, new C{ T="Unmapped", N = "_generated_pada6c0" }},
}, S=20}},
{ 0x468, new C{ T="Tagblock", N = "_projectile deflection properties", B = new Dictionary<long, C>
{
{ 0x0643, new C{ T="Comment", N = "_Deflection areas" }},
{ 0x0, new C{ T="Tagblock", N = "_Target Areas", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Time to Target Window.min" }},
{ 0x4, new C{ T="Float", N = "_Time to Target Window.max" }},
{ 0x8, new C{ T="Float", N = "_Target Offset.X" }},
{ 0xC, new C{ T="Float", N = "_Target Offset.Y" }},
{ 0x10, new C{ T="Float", N = "_Target Offset.Z" }},
{ 0x14, new C{ T="Float", N = "_Target Radius" }},
{ 0x18, new C{ T="Float", N = "_Max Incoming Angle" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_Animation Name" }},
}, S=32}},
}, S=20}},
{ 0x47C, new C{ T="Tagblock", N = "_Bot Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_movement flags" }},
{ 0x4644, new C{ T="Comment", N = "_Bot info" }},
{ 0x4, new C{ T="Float", N = "_crouch time" }},
{ 0x8, new C{ T="Float", N = "_crouch Low shield threshold" }},
{ 0xC, new C{ T="Float", N = "_jump chance" }},
{ 0x10, new C{ T="Float", N = "_evade low shield threshold" }},
{ 0x14, new C{ T="Float", N = "_strafing influence vector radius" }},
{ 0x18, new C{ T="Float", N = "_strafing influence vector weight" }},
{ 0x1C, new C{ T="Float", N = "_firing point refresh distance" }},
{ 0x20, new C{ T="Float", N = "_influence rejection radius" }},
{ 0x24, new C{ T="Float", N = "_movement cooldown min" }},
{ 0x28, new C{ T="Float", N = "_movement cooldown max" }},
}, S=44}},
{ 0x490, new C{ T="Tagblock", N = "_Evasive Fight Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Engage Fraction" }},
{ 0x4, new C{ T="Float", N = "_Stationary Time.min" }},
{ 0x8, new C{ T="Float", N = "_Stationary Time.max" }},
{ 0xC, new C{ T="Float", N = "_Move Danger Threshold" }},
{ 0x10, new C{ T="Float", N = "_Shield Threshold" }},
{ 0x14, new C{ T="Float", N = "_Dive Cooldown" }},
{ 0x18, new C{ T="Float", N = "_Dive Shield Threshold" }},
{ 0x1C, new C{ T="Float", N = "_Dive Danger Threshold" }},
}, S=32}},
{ 0x4A4, new C{ T="Tagblock", N = "_Aggressive Fight Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_Flags" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_padcd38" }},
{ 0x4, new C{ T="Float", N = "_Engage Fraction" }},
{ 0x8, new C{ T="Float", N = "_Stationary Time.min" }},
{ 0xC, new C{ T="Float", N = "_Stationary Time.max" }},
{ 0x10, new C{ T="Float", N = "_Move Danger Threshold" }},
{ 0x14, new C{ T="Float", N = "_Max Speed Percent" }},
}, S=24}},
{ 0x4B8, new C{ T="Tagblock", N = "_Linear Advance Fight Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Stationary Time.min" }},
{ 0x4, new C{ T="Float", N = "_Stationary Time.max" }},
{ 0x8, new C{ T="Float", N = "_Advance Distance" }},
}, S=12}},
{ 0x4CC, new C{ T="Tagblock", N = "_Static Fight Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Stationary Time.min" }},
{ 0x4, new C{ T="Float", N = "_Stationary Time.max" }},
{ 0x8, new C{ T="Float", N = "_Danger Move Threshold" }},
}, S=12}},
{ 0x4E0, new C{ T="Tagblock", N = "_Stand Ground Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Target Distance.min" }},
{ 0x4, new C{ T="Float", N = "_Target Distance.max" }},
{ 0x8, new C{ T="Float", N = "_Ally Distance" }},
{ 0xC, new C{ T="Float", N = "_Ally Attacker Distance" }},
{ 0x10, new C{ T="Float", N = "_Behavior Linger Time" }},
}, S=20}},
{ 0x4F4, new C{ T="Tagblock", N = "_Grenade Fight Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Stationary Time.min" }},
{ 0x4, new C{ T="Float", N = "_Stationary Time.max" }},
{ 0x8, new C{ T="Float", N = "_Minimum Movement" }},
{ 0xC, new C{ T="Float", N = "_Ideal Fraction" }},
}, S=16}},
{ 0x508, new C{ T="Tagblock", N = "_Cover Sequence Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Shield Start Threshold" }},
{ 0x4, new C{ T="Float", N = "_Shield End Threshold" }},
{ 0x8, new C{ T="Float", N = "_Danger Start Threshold" }},
{ 0xC, new C{ T="Float", N = "_Danger End Threshold" }},
{ 0x10, new C{ T="Float", N = "_Minimum Hide Duration" }},
}, S=20}},
{ 0x51C, new C{ T="Tagblock", N = "_Cover Move Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="Float", N = "_Cover Failed Duration" }},
}, S=8}},
{ 0x530, new C{ T="Tagblock", N = "_Cover Hide Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Minimum Hide Duration" }},
}, S=4}},
{ 0x544, new C{ T="Tagblock", N = "_Long Range Attack Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="Float", N = "_Potshot duration" }},
{ 0x8, new C{ T="Float", N = "_Peek duration" }},
{ 0xC, new C{ T="Float", N = "_Time between potshots" }},
}, S=16}},
{ 0x558, new C{ T="Tagblock", N = "_Hoist Attack Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Max Danger Threshold" }},
{ 0x4, new C{ T="Float", N = "_Hoist Attack Chance" }},
}, S=8}},
{ 0x56C, new C{ T="Tagblock", N = "_Hunker Down Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Danger Start Threshold" }},
{ 0x4, new C{ T="Float", N = "_Distance Abort Threshold" }},
{ 0x8, new C{ T="Float", N = "_Shield Health Threshold" }},
{ 0xC, new C{ T="Float", N = "_Hunker Time.min" }},
{ 0x10, new C{ T="Float", N = "_Hunker Time.max" }},
}, S=20}},
{ 0x580, new C{ T="Tagblock", N = "_Shield Raise Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Danger Threshold" }},
}, S=4}},
{ 0x594, new C{ T="Tagblock", N = "_Vehicle Aggressive Fight Propert", B = new Dictionary<long, C>
{
{ 0x0645, new C{ T="Comment", N = "_Fight Properties" }},
{ 0x0, new C{ T="Float", N = "_Engage Fraction" }},
{ 0x4, new C{ T="Float", N = "_Max Speed Percent" }},
{ 0x8, new C{ T="4Byte", N = "_Flags" }},
{ 0xC, new C{ T="Float", N = "_Max Danger to Boost" }},
}, S=16}},
{ 0x5A8, new C{ T="Tagblock", N = "_Vehicle Defensive Fight Properti", B = new Dictionary<long, C>
{
{ 0x0646, new C{ T="Comment", N = "_Fight Properties" }},
{ 0x0, new C{ T="Float", N = "_Engage Fraction" }},
{ 0x4, new C{ T="Float", N = "_Max Speed Percent" }},
}, S=8}},
{ 0x5BC, new C{ T="Tagblock", N = "_engineer properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_death height" }},
{ 0x4, new C{ T="Float", N = "_death rise time" }},
{ 0x8, new C{ T="Float", N = "_death detonation time" }},
{ 0xC, new C{ T="Float", N = "_shield boost radius max" }},
{ 0x10, new C{ T="Float", N = "_shield boost period" }},
{ 0x14, new C{ T="mmr3Hash", N = "_shield boost damage section name" }},
{ 0x18647, new C{ T="Comment", N = "_Detonation Thresholds" }},
{ 0x18, new C{ T="Float", N = "_detonation shield threshold" }},
{ 0x1C, new C{ T="Float", N = "_detonation body vitality" }},
{ 0x20, new C{ T="Float", N = "_proximity radius" }},
{ 0x24, new C{ T="Float", N = "_proximity detonation chance" }},
{ 0x28, new C{ T="TagRef", N = "_proximity equipment" }},
}, S=68}},
{ 0x5D0, new C{ T="Tagblock", N = "_inspect properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_stop distance" }},
{ 0x4, new C{ T="Float", N = "_inspect time.min" }},
{ 0x8, new C{ T="Float", N = "_inspect time.max" }},
{ 0xC, new C{ T="Float", N = "_search range.min" }},
{ 0x10, new C{ T="Float", N = "_search range.max" }},
}, S=20}},
{ 0x5E4, new C{ T="Tagblock", N = "_vehicle entrance properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Max Distance From Actor" }},
{ 0x4, new C{ T="Float", N = "_Minimum Distance From Player" }},
{ 0x8, new C{ T="Float", N = "_Minimum Distance From Vehicle" }},
{ 0xC, new C{ T="Float", N = "_Perceived Danger Threshold" }},
{ 0x10648, new C{ T="Comment", N = "_Total Health Percentage" }},
{ 0x10, new C{ T="Float", N = "_Minimum Health to Enter" }},
{ 0x14, new C{ T="Float", N = "_Health Percentage to Exit" }},
{ 0x18649, new C{ T="Comment", N = "_Remaining Damage Sections Percen" }},
{ 0x18, new C{ T="Float", N = "_Threshold to Enter" }},
{ 0x1C, new C{ T="Float", N = "_Threshold to Exit" }},
{ 0x20, new C{ T="Float", N = "_Chance To Exit Vehicle" }},
{ 0x24, new C{ T="Float", N = "_Vehicle Exit Impulse Timer" }},
}, S=40}},
{ 0x5F8, new C{ T="Tagblock", N = "_test-only weapons properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_weapon" }},
{ 0x1C650, new C{ T="Comment", N = "_Weapon drop when killed" }},
{ 0x1C, new C{ T="Float", N = "_drop weapon loaded.min" }},
{ 0x20, new C{ T="Float", N = "_drop weapon loaded.max" }},
{ 0x24, new C{ T="Unmapped", N = "_drop weapon ammo" }},
{ 0x28, new C{ T="TagRef", N = "_weapon melee damage" }},
{ 0x44651, new C{ T="Comment", N = "_Accuracy" }},
{ 0x44, new C{ T="Float", N = "_normal accuracy bounds.min" }},
{ 0x48, new C{ T="Float", N = "_normal accuracy bounds.max" }},
{ 0x4C, new C{ T="Float", N = "_normal accuracy time" }},
{ 0x50, new C{ T="Float", N = "_heroic accuracy bounds.min" }},
{ 0x54, new C{ T="Float", N = "_heroic accuracy bounds.max" }},
{ 0x58, new C{ T="Float", N = "_heroic accuracy time" }},
{ 0x5C, new C{ T="Float", N = "_legendary accuracy bounds.min" }},
{ 0x60, new C{ T="Float", N = "_legendary accuracy bounds.max" }},
{ 0x64, new C{ T="Float", N = "_legendary accuracy time" }},
{ 0x68, new C{ T="Tagblock", N = "_weapon preferences", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_Tier Category" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_padb31f" }},
{ 0x4, new C{ T="4Byte", N = "_Weight" }},
{ 0x8, new C{ T="4Byte", N = "_Multi-Tier Multiplier" }},
}, S=12}},
{ 0x7C, new C{ T="Tagblock", N = "_weapon modes", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_mode name" }},
{ 0x4, new C{ T="4Byte", N = "_weapons flags" }},
{ 0x8652, new C{ T="Comment", N = "_Combat ranges" }},
{ 0x8, new C{ T="Float", N = "_maximum firing range" }},
{ 0xC, new C{ T="Float", N = "_minimum firing range" }},
{ 0x10, new C{ T="Float", N = "_normal combat range.min" }},
{ 0x14, new C{ T="Float", N = "_normal combat range.max" }},
{ 0x18, new C{ T="Float", N = "_bombardment range" }},
{ 0x1C, new C{ T="Float", N = "_Max special target distance" }},
{ 0x20, new C{ T="Float", N = "_Max extreme target distance" }},
{ 0x24, new C{ T="Byte", N = "_Lower bound option" }},
{ 0x25, new C{ T="Unmapped", N = "_generated_pad6ff4" }},
{ 0x28653, new C{ T="Comment", N = "_Ballistic Firing" }},
{ 0x28, new C{ T="Float", N = "_Ballistic firing bounds.min" }},
{ 0x2C, new C{ T="Float", N = "_Ballistic firing bounds.max" }},
{ 0x30, new C{ T="Float", N = "_Ballistic fraction bounds.min" }},
{ 0x34, new C{ T="Float", N = "_Ballistic fraction bounds.max" }},
{ 0x38654, new C{ T="Comment", N = "_Use Preferences" }},
{ 0x38, new C{ T="Float", N = "_Favored Ranges.X" }},
{ 0x3C, new C{ T="Float", N = "_Favored Ranges.Y" }},
{ 0x40, new C{ T="Float", N = "_Favored Ranges.Z" }},
{ 0x44, new C{ T="Float", N = "_Range Ratings.X" }},
{ 0x48, new C{ T="Float", N = "_Range Ratings.Y" }},
{ 0x4C, new C{ T="Float", N = "_Range Ratings.Z" }},
{ 0x50, new C{ T="Float", N = "_anti-vehicle rating " }},
{ 0x54, new C{ T="Float", N = "_Scariness Threshold" }},
{ 0x58655, new C{ T="Comment", N = "_Behavior" }},
{ 0x58, new C{ T="Float", N = "_first burst delay time.min" }},
{ 0x5C, new C{ T="Float", N = "_first burst delay time.max" }},
{ 0x60, new C{ T="Float", N = "_death fire-wildly chance" }},
{ 0x64, new C{ T="Float", N = "_death fire-wildly time" }},
{ 0x68, new C{ T="Float", N = "_heat vent min percent" }},
{ 0x6C, new C{ T="Float", N = "_ammo reload min percent" }},
{ 0x70, new C{ T="Float", N = "_reload chance if not empty" }},
{ 0x74, new C{ T="Float", N = "_heat vent cooldown" }},
{ 0x78, new C{ T="Float", N = "_custom stand gun offset.X" }},
{ 0x7C, new C{ T="Float", N = "_custom stand gun offset.Y" }},
{ 0x80, new C{ T="Float", N = "_custom stand gun offset.Z" }},
{ 0x84, new C{ T="Float", N = "_custom crouch gun offset.X" }},
{ 0x88, new C{ T="Float", N = "_custom crouch gun offset.Y" }},
{ 0x8C, new C{ T="Float", N = "_custom crouch gun offset.Z" }},
{ 0x90, new C{ T="4Byte", N = "_Blocked Shot Count" }},
{ 0x94, new C{ T="Float", N = "_Max Pre-Fire Time (Normal)" }},
{ 0x98, new C{ T="Float", N = "_Max Pre-Fire Time (Heroic)" }},
{ 0x9C, new C{ T="Float", N = "_Max Pre-Fire Time (Legendary)" }},
{ 0xA0, new C{ T="Tagblock", N = "_selection properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Initiators" }},
{ 0x4, new C{ T="4Byte", N = "_Required Conditions" }},
{ 0x8, new C{ T="Float", N = "_Cluster Radius" }},
{ 0xC, new C{ T="4Byte", N = "_Min Cluster Count" }},
{ 0x10, new C{ T="Float", N = "_Target Range Bounds.min" }},
{ 0x14, new C{ T="Float", N = "_Target Range Bounds.max" }},
{ 0x18, new C{ T="Float", N = "_Danger Bounds.min" }},
{ 0x1C, new C{ T="Float", N = "_Danger Bounds.max" }},
{ 0x20, new C{ T="Float", N = "_Activation Chance" }},
{ 0x24, new C{ T="Float", N = "_Activation Check Delay" }},
{ 0x28, new C{ T="Float", N = "_Projectile Error" }},
{ 0x2C, new C{ T="Float", N = "_Lifetime.min" }},
{ 0x30, new C{ T="Float", N = "_Lifetime.max" }},
{ 0x34, new C{ T="4Byte", N = "_Max Burst Count" }},
{ 0x38, new C{ T="Byte", N = "_Trigger Behavior" }},
{ 0x39, new C{ T="Unmapped", N = "_generated_padabe7" }},
}, S=60}},
{ 0xB4, new C{ T="Byte", N = "_Threat Level" }},
{ 0xB5, new C{ T="Unmapped", N = "_generated_pad89d5" }},
{ 0xB8, new C{ T="Float", N = "_Effectiveness Token Close Player" }},
{ 0xBC, new C{ T="Tagblock", N = "_Firing Program", B = new Dictionary<long, C>
{
{ 0x0656, new C{ T="Comment", N = "_Precision" }},
{ 0x0657, new C{ T="Comment", N = "_Target Position" }},
{ 0x0, new C{ T="Unmapped", N = "_Tracking" }},
{ 0x4, new C{ T="Float", N = "_Matching Fraction.min" }},
{ 0x8, new C{ T="Float", N = "_Matching Fraction.max" }},
{ 0xC, new C{ T="Float", N = "_Matching Spring Force.min" }},
{ 0x10, new C{ T="Float", N = "_Matching Spring Force.max" }},
{ 0x14, new C{ T="Unmapped", N = "_Leading" }},
{ 0x18658, new C{ T="Comment", N = "_" }},
{ 0x18659, new C{ T="Comment", N = "_Aiming Error" }},
{ 0x18, new C{ T="Float", N = "_Error Distance.min" }},
{ 0x1C, new C{ T="Float", N = "_Error Distance.max" }},
{ 0x20, new C{ T="Float", N = "_Error Angle.min" }},
{ 0x24, new C{ T="Float", N = "_Error Angle.max" }},
{ 0x28, new C{ T="Float", N = "_Error Max Angle.min" }},
{ 0x2C, new C{ T="Float", N = "_Error Max Angle.max" }},
{ 0x30660, new C{ T="Comment", N = "_" }},
{ 0x30661, new C{ T="Comment", N = "_Linear Burst Geo." }},
{ 0x30, new C{ T="Byte", N = "_Burst Line Style" }},
{ 0x31, new C{ T="Unmapped", N = "_generated_paddb86" }},
{ 0x34, new C{ T="Float", N = "_Burst Line Angle.min" }},
{ 0x38, new C{ T="Float", N = "_Burst Line Angle.max" }},
{ 0x3C, new C{ T="Float", N = "_Burst Origin Radius.min" }},
{ 0x40, new C{ T="Float", N = "_Burst Origin Radius.max" }},
{ 0x44, new C{ T="Float", N = "_Burst Origin Angle.min" }},
{ 0x48, new C{ T="Float", N = "_Burst Origin Angle.max" }},
{ 0x4C, new C{ T="Float", N = "_Burst Return Length Min.min" }},
{ 0x50, new C{ T="Float", N = "_Burst Return Length Min.max" }},
{ 0x54, new C{ T="Float", N = "_Burst Return Length Max.min" }},
{ 0x58, new C{ T="Float", N = "_Burst Return Length Max.max" }},
{ 0x5C, new C{ T="Float", N = "_Burst Return Angle.min" }},
{ 0x60, new C{ T="Float", N = "_Burst Return Angle.max" }},
{ 0x64, new C{ T="Float", N = "_Burst Maximum Angular Vel..min" }},
{ 0x68, new C{ T="Float", N = "_Burst Maximum Angular Vel..max" }},
{ 0x6C, new C{ T="Float", N = "_Burst Maximum Error Angle.min" }},
{ 0x70, new C{ T="Float", N = "_Burst Maximum Error Angle.max" }},
{ 0x74662, new C{ T="Comment", N = "_" }},
{ 0x74663, new C{ T="Comment", N = "_Hostility" }},
{ 0x74, new C{ T="Float", N = "_Rate of Fire.min" }},
{ 0x78, new C{ T="Float", N = "_Rate of Fire.max" }},
{ 0x7C, new C{ T="Float", N = "_Weapon Damage Mod..min" }},
{ 0x80, new C{ T="Float", N = "_Weapon Damage Mod..max" }},
{ 0x84664, new C{ T="Comment", N = "_Burst Pattern" }},
{ 0x84, new C{ T="Float", N = "_Duration Min.min" }},
{ 0x88, new C{ T="Float", N = "_Duration Min.max" }},
{ 0x8C, new C{ T="Float", N = "_Duration Max.min" }},
{ 0x90, new C{ T="Float", N = "_Duration Max.max" }},
{ 0x94, new C{ T="Float", N = "_Separation Min.min" }},
{ 0x98, new C{ T="Float", N = "_Separation Min.max" }},
{ 0x9C, new C{ T="Float", N = "_Separation Max.min" }},
{ 0xA0, new C{ T="Float", N = "_Separation Max.max" }},
{ 0xA4665, new C{ T="Comment", N = "_" }},
{ 0xA4666, new C{ T="Comment", N = "_Burst Noise" }},
{ 0xA4, new C{ T="Unmapped", N = "_Amount" }},
{ 0xA8, new C{ T="Float", N = "_Min Period.min" }},
{ 0xAC, new C{ T="Float", N = "_Min Period.max" }},
{ 0xB0, new C{ T="Float", N = "_Max Period.min" }},
{ 0xB4, new C{ T="Float", N = "_Max Period.max" }},
{ 0xB8667, new C{ T="Comment", N = "_" }},
}, S=184}},
{ 0xD0668, new C{ T="Comment", N = "_Unit Target Preference" }},
{ 0xD0, new C{ T="Tagblock", N = "_Unit Target Preference", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_Category" }},
{ 0x1, new C{ T="Unmapped", N = "_generated_pad0052" }},
{ 0x4, new C{ T="Float", N = "_Preference" }},
{ 0x8669, new C{ T="Comment", N = "_Vehicle Velocity Targeting Prefe" }},
{ 0x8, new C{ T="Tagblock", N = "_Vehicle Velocity Based Targeting", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Vehicle Velocity Override Prefer" }},
{ 0x4, new C{ T="Float", N = "_Velocity Threshold" }},
}, S=8}},
}, S=28}},
{ 0xE4, new C{ T="Tagblock", N = "_Prefire Beam", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_initial error angle" }},
{ 0x4670, new C{ T="Comment", N = "_Radius Decay" }},
{ 0x4671, new C{ T="Comment", N = "_radius decay function" }},
{ 0x4672, new C{ T="Comment", N = "_radius decay function" }},
{ 0x4673, new C{ T="Comment", N = "_" }},
{ 0x4, new C{ T="Unmapped", N = "_data" }},
{ 0x1C674, new C{ T="Comment", N = "_" }},
{ 0x1C675, new C{ T="Comment", N = "_Beam Spin" }},
{ 0x1C676, new C{ T="Comment", N = "_beam spin function" }},
{ 0x1C677, new C{ T="Comment", N = "_beam spin function" }},
{ 0x1C678, new C{ T="Comment", N = "_" }},
{ 0x1C, new C{ T="Unmapped", N = "_data" }},
{ 0x34679, new C{ T="Comment", N = "_" }},
{ 0x34, new C{ T="TagRef", N = "_beam effect" }},
{ 0x50, new C{ T="TagRef", N = "_full screen effect" }},
{ 0x6C680, new C{ T="Comment", N = "_Marker overrides" }},
{ 0x6C, new C{ T="mmr3Hash", N = "_start marker" }},
{ 0x70, new C{ T="mmr3Hash", N = "_fallback marker" }},
}, S=116}},
}, S=248}},
}, S=144}},
{ 0x60C, new C{ T="Tagblock", N = "_firing styles", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_style name" }},
{ 0x4, new C{ T="Float", N = "_precision" }},
{ 0x8, new C{ T="Float", N = "_hostility" }},
{ 0xC, new C{ T="Unmapped", N = "_Precision Range" }},
{ 0x10, new C{ T="Unmapped", N = "_Hostility Range" }},
}, S=20}},
{ 0x620, new C{ T="Tagblock", N = "_grenades properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_grenades flags" }},
{ 0x4, new C{ T="Byte", N = "_grenade type" }},
{ 0x5, new C{ T="Unmapped", N = "_generated_padcce0" }},
{ 0x6, new C{ T="2Byte", N = "_trajectory type" }},
{ 0x8, new C{ T="2Byte", N = "_minimum enemy count" }},
{ 0xA, new C{ T="Unmapped", N = "_generated_pade6c1" }},
{ 0xC, new C{ T="Float", N = "_enemy radius" }},
{ 0x10681, new C{ T="Comment", N = "_throw error" }},
{ 0x10, new C{ T="Float", N = "_throw error (easy)" }},
{ 0x14, new C{ T="Float", N = "_throw error (normal)" }},
{ 0x18, new C{ T="Float", N = "_throw error (heroic)" }},
{ 0x1C, new C{ T="Float", N = "_throw error (legendary)" }},
{ 0x20682, new C{ T="Comment", N = "_" }},
{ 0x20683, new C{ T="Comment", N = "_Damage Modifier" }},
{ 0x20, new C{ T="Float", N = "_Damage Modifier (easy)" }},
{ 0x24, new C{ T="Float", N = "_Damage Modifier (normal)" }},
{ 0x28, new C{ T="Float", N = "_Damage Modifier (heroic)" }},
{ 0x2C, new C{ T="Float", N = "_Damage Modifier (legendary)" }},
{ 0x30684, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Float", N = "_grenade ideal velocity" }},
{ 0x34, new C{ T="Float", N = "_grenade velocity" }},
{ 0x38, new C{ T="Float", N = "_grenade ranges.min" }},
{ 0x3C, new C{ T="Float", N = "_grenade ranges.max" }},
{ 0x40, new C{ T="Float", N = "_collateral damage radius" }},
{ 0x44, new C{ T="Float", N = "_grenade chance" }},
{ 0x48, new C{ T="Float", N = "_Active Shield Modifier" }},
{ 0x4C, new C{ T="Float", N = "_grenade throw delay" }},
{ 0x50, new C{ T="Float", N = "_Global grenade delay" }},
{ 0x54, new C{ T="Float", N = "_grenade uncover chance" }},
{ 0x58, new C{ T="Float", N = "_anti-vehicle grenade chance" }},
{ 0x5C685, new C{ T="Comment", N = "_Grenade drop when killed" }},
{ 0x5C, new C{ T="Unmapped", N = "_grenade count" }},
{ 0x60, new C{ T="Float", N = "_dont drop grenades chance" }},
}, S=100}},
{ 0x634, new C{ T="Tagblock", N = "_vehicle properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_unit" }},
{ 0x1C, new C{ T="TagRef", N = "_style" }},
{ 0x38, new C{ T="TagRef", N = "_Behavior Tree" }},
{ 0x54, new C{ T="TagRef", N = "_Nondriver Behavior Tree" }},
{ 0x70686, new C{ T="Comment", N = "_Flying Avoidance" }},
{ 0x70, new C{ T="Float", N = "_lookahead_time" }},
{ 0x74, new C{ T="Float", N = "_roll change magnitude" }},
{ 0x78, new C{ T="Float", N = "_roll decay multiplier" }},
{ 0x7C, new C{ T="Float", N = "_throttle grace period" }},
{ 0x80, new C{ T="Float", N = "_minimum throttle" }},
{ 0x84687, new C{ T="Comment", N = "_Vehicle flags" }},
{ 0x84, new C{ T="4Byte", N = "_vehicle flags" }},
{ 0x88688, new C{ T="Comment", N = "_Hover Perturbation New" }},
{ 0x88, new C{ T="Float", N = "_hover deceleration distance" }},
{ 0x8C, new C{ T="Float", N = "_hover offset distance" }},
{ 0x90689, new C{ T="Comment", N = "_Hover Perturbation Fallback" }},
{ 0x90, new C{ T="Float", N = "_hover allow perturbation speed" }},
{ 0x94, new C{ T="Float", N = "_hover random x-axis period" }},
{ 0x98, new C{ T="Float", N = "_hover random y-axis period" }},
{ 0x9C, new C{ T="Float", N = "_hover random z-axis period" }},
{ 0xA0, new C{ T="Float", N = "_hover random radius" }},
{ 0xA4, new C{ T="Float", N = "_hover anchor approach speed limi" }},
{ 0xA8, new C{ T="Float", N = "_hover anchor throttle scale dist.min" }},
{ 0xAC, new C{ T="Float", N = "_hover anchor throttle scale dist.max" }},
{ 0xB0, new C{ T="Float", N = "_hover anchor xy-throttle scale.min" }},
{ 0xB4, new C{ T="Float", N = "_hover anchor xy-throttle scale.max" }},
{ 0xB8, new C{ T="Float", N = "_hover anchor z-throttle scale.min" }},
{ 0xBC, new C{ T="Float", N = "_hover anchor z-throttle scale.max" }},
{ 0xC0, new C{ T="Float", N = "_hover throttle min z" }},
{ 0xC4690, new C{ T="Comment", N = "_Pathfinding" }},
{ 0xC4, new C{ T="Float", N = "_ai pathfinding radius" }},
{ 0xC8, new C{ T="Float", N = "_ai avoidance radius" }},
{ 0xCC, new C{ T="Float", N = "_ai destination radius" }},
{ 0xD0, new C{ T="Float", N = "_ai deceleration distance" }},
{ 0xD4, new C{ T="Float", N = "_roughly, the time it would take " }},
{ 0xD8691, new C{ T="Comment", N = "_Turning" }},
{ 0xD8, new C{ T="Float", N = "_ai turning radius" }},
{ 0xDC692, new C{ T="Comment", N = "_Steering" }},
{ 0xDC, new C{ T="Float", N = "_ai banshee steering maximum" }},
{ 0xE0, new C{ T="Float", N = "_ai max steering angle" }},
{ 0xE4, new C{ T="Float", N = "_ai max steering delta" }},
{ 0xE8, new C{ T="Float", N = "_ai oversteering scale" }},
{ 0xEC, new C{ T="Float", N = "_ai sideslip distance (combat)" }},
{ 0xF0, new C{ T="Float", N = "_ai sideslip distance (non-combat" }},
{ 0xF4, new C{ T="Float", N = "_ai avoidance distance" }},
{ 0xF8, new C{ T="Float", N = "_ai min urgency" }},
{ 0xFC, new C{ T="Float", N = "_destination behind angle" }},
{ 0x100, new C{ T="Float", N = "_skid scale" }},
{ 0x104, new C{ T="Float", N = "_aiming velocity maximum" }},
{ 0x108, new C{ T="Float", N = "_aiming acceleration maximum" }},
{ 0x10C693, new C{ T="Comment", N = "_Throttle" }},
{ 0x10C, new C{ T="Float", N = "_ai throttle maximum" }},
{ 0x110, new C{ T="Float", N = "_ai reverse throttle maximum" }},
{ 0x114, new C{ T="Float", N = "_ai goal min throttle scale" }},
{ 0x118, new C{ T="Float", N = "_ai turn min throttle scale" }},
{ 0x11C, new C{ T="Float", N = "_ai direction min throttle scale" }},
{ 0x120, new C{ T="Float", N = "_ai skid min throttle scale" }},
{ 0x124, new C{ T="Float", N = "_skid attentuation max angle" }},
{ 0x128, new C{ T="Float", N = "_ai acceleration scale" }},
{ 0x12C, new C{ T="Float", N = "_ai throttle blend" }},
{ 0x130, new C{ T="Float", N = "_theoretical max speed" }},
{ 0x134, new C{ T="Float", N = "_error scale" }},
{ 0x138, new C{ T="Float", N = "_Throttle Ramp Step" }},
{ 0x13C694, new C{ T="Comment", N = "_Speed" }},
{ 0x13C, new C{ T="Float", N = "_Target Speed" }},
{ 0x140695, new C{ T="Comment", N = "_Boost For Locomotion - Direction" }},
{ 0x140, new C{ T="Float", N = "_ai boost forward facing angle" }},
{ 0x144, new C{ T="Float", N = "_ai min throttle to boost" }},
{ 0x148696, new C{ T="Comment", N = "_Combat" }},
{ 0x148, new C{ T="Float", N = "_ai allowable aim deviation angle" }},
{ 0x14C, new C{ T="Float", N = "_Melee Cooldown" }},
{ 0x150697, new C{ T="Comment", N = "_Behavior" }},
{ 0x150, new C{ T="Float", N = "_ai charge tight angle distance" }},
{ 0x154, new C{ T="Float", N = "_ai charge tight angle" }},
{ 0x158, new C{ T="Float", N = "_ai charge repeat timeout" }},
{ 0x15C, new C{ T="Float", N = "_ai charge look-ahead time" }},
{ 0x160, new C{ T="Float", N = "_ai charge consider distance" }},
{ 0x164, new C{ T="Float", N = "_ai charge abort distance" }},
{ 0x168, new C{ T="Float", N = "_ai charge abort close distance" }},
{ 0x16C, new C{ T="Float", N = "_ai charge max out of area distan" }},
{ 0x170, new C{ T="Float", N = "_vehicle ram timeout" }},
{ 0x174, new C{ T="Float", N = "_ram paralysis time" }},
{ 0x178, new C{ T="Float", N = "_ai cover damage threshold" }},
{ 0x17C, new C{ T="Float", N = "_ai cover shield damage threshold" }},
{ 0x180, new C{ T="Float", N = "_ai cover min distance" }},
{ 0x184, new C{ T="Float", N = "_ai cover time" }},
{ 0x188, new C{ T="Float", N = "_ai cover min boost distance" }},
{ 0x18C, new C{ T="Float", N = "_turtling recent damage threshold" }},
{ 0x190, new C{ T="Float", N = "_turtling min time" }},
{ 0x194, new C{ T="Float", N = "_turtling timeout" }},
{ 0x198, new C{ T="Tagblock", N = "_Vehicle Strafing Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_strafe min distance" }},
{ 0x4, new C{ T="Float", N = "_strafe abort distance" }},
{ 0x8, new C{ T="Float", N = "_defensive strafe abort distance" }},
}, S=12}},
{ 0x1AC, new C{ T="Tagblock", N = "_Vehicle Flying Holding Pattern P", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_holding pattern elevation" }},
{ 0x4, new C{ T="Float", N = "_holding pattern speed percentage" }},
{ 0x8, new C{ T="Float", N = "_holding pattern radius" }},
{ 0xC, new C{ T="Float", N = "_max turn angle percentage" }},
}, S=16}},
{ 0x1C0, new C{ T="Tagblock", N = "_Vehicle Flying Preengage Propert", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_pre engage time.min" }},
{ 0x4, new C{ T="Float", N = "_pre engage time.max" }},
{ 0x8, new C{ T="Float", N = "_pre engage elevation" }},
{ 0xC, new C{ T="Float", N = "_pre engage radius" }},
}, S=16}},
{ 0x1D4, new C{ T="Tagblock", N = "_Vehicle Flying Suppressed Proper", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_suppressed time.min" }},
{ 0x4, new C{ T="Float", N = "_suppressed time.max" }},
{ 0x8, new C{ T="Float", N = "_suppressed damage threshold" }},
{ 0xC, new C{ T="Float", N = "_suppressed retreat distance" }},
{ 0x10, new C{ T="Float", N = "_suppressed elevation" }},
{ 0x14, new C{ T="Float", N = "_suppressed radius" }},
}, S=24}},
{ 0x1E8, new C{ T="Tagblock", N = "_Vehicle Flying Aggressive Engage", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Unmapped", N = "_aggressive loops" }},
}, S=4}},
{ 0x1FC, new C{ T="Tagblock", N = "_Vehicle Flying Defensive Engage ", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_defensive time.min" }},
{ 0x4, new C{ T="Float", N = "_defensive time.max" }},
}, S=8}},
{ 0x210, new C{ T="Tagblock", N = "_Vehicle Flying Fallback Properti", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_combat elevation.min" }},
{ 0x4, new C{ T="Float", N = "_combat elevation.max" }},
{ 0x8, new C{ T="Float", N = "_fallback minimum distance" }},
{ 0xC, new C{ T="Float", N = "_flyby distance" }},
}, S=16}},
{ 0x224, new C{ T="2Byte", N = "_obstacle ignore size" }},
{ 0x226, new C{ T="Unmapped", N = "_generated_pad354a" }},
}, S=552}},
{ 0x648, new C{ T="Tagblock", N = "_flying movement properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_vehicle" }},
{ 0x1C698, new C{ T="Comment", N = "_Default Values" }},
{ 0x1C699, new C{ T="Comment", N = "_Vector Weights" }},
{ 0x1C, new C{ T="Float", N = "_facing" }},
{ 0x20, new C{ T="Float", N = "_perturbation" }},
{ 0x24, new C{ T="Float", N = "_volume avoidance" }},
{ 0x28, new C{ T="Float", N = "_volume perturbation" }},
{ 0x2C, new C{ T="Float", N = "_volume cover" }},
{ 0x30, new C{ T="Float", N = "_flocking" }},
{ 0x34, new C{ T="Float", N = "_target" }},
{ 0x38, new C{ T="Float", N = "_target tail" }},
{ 0x3C700, new C{ T="Comment", N = "_Area Selection" }},
{ 0x3C, new C{ T="Float", N = "_area reselect time.min" }},
{ 0x40, new C{ T="Float", N = "_area reselect time.max" }},
{ 0x44701, new C{ T="Comment", N = "_Idle" }},
{ 0x44, new C{ T="Float", N = "_idle time.min" }},
{ 0x48, new C{ T="Float", N = "_idle time.max" }},
{ 0x4C702, new C{ T="Comment", N = "_Cover" }},
{ 0x4C, new C{ T="Float", N = "_unsafe cover reselect time.min" }},
{ 0x50, new C{ T="Float", N = "_unsafe cover reselect time.max" }},
{ 0x54, new C{ T="Float", N = "_cover heading reselect time.min" }},
{ 0x58, new C{ T="Float", N = "_cover heading reselect time.max" }},
{ 0x5C, new C{ T="Float", N = "_max cover search distance" }},
{ 0x60, new C{ T="Float", N = "_max cover impulse distance" }},
{ 0x64, new C{ T="Float", N = "_spline cooldown time.min" }},
{ 0x68, new C{ T="Float", N = "_spline cooldown time.max" }},
{ 0x6C703, new C{ T="Comment", N = "_Volume Avoidance" }},
{ 0x6C, new C{ T="Float", N = "_volume influence distance" }},
{ 0x70, new C{ T="Float", N = "_volume perturbation phase" }},
{ 0x74, new C{ T="Float", N = "_volume bounding distance" }},
{ 0x78704, new C{ T="Comment", N = "_Approach" }},
{ 0x78, new C{ T="Float", N = "_volume approach distance" }},
{ 0x7C, new C{ T="Float", N = "_volume break off distance" }},
{ 0x80, new C{ T="Float", N = "_minimum approach distance" }},
{ 0x84, new C{ T="Float", N = "_collision avoidance range.min" }},
{ 0x88, new C{ T="Float", N = "_collision avoidance range.max" }},
{ 0x8C705, new C{ T="Comment", N = "_Evasion" }},
{ 0x8C, new C{ T="Float", N = "_evade time.min" }},
{ 0x90, new C{ T="Float", N = "_evade time.max" }},
{ 0x94, new C{ T="Float", N = "_evade body damage threshold" }},
{ 0x98, new C{ T="Float", N = "_evade shield damage threshold" }},
{ 0x9C, new C{ T="Float", N = "_bogey retreat time" }},
{ 0xA0, new C{ T="Float", N = "_bogey retreat distance" }},
{ 0xA4706, new C{ T="Comment", N = "_Flocking" }},
{ 0xA4, new C{ T="Float", N = "_flock radius.min" }},
{ 0xA8, new C{ T="Float", N = "_flock radius.max" }},
{ 0xAC, new C{ T="Float", N = "_forward follow angle" }},
{ 0xB0, new C{ T="Float", N = "_behind follow angle" }},
{ 0xB4707, new C{ T="Comment", N = "_Tailing" }},
{ 0xB4, new C{ T="Float", N = "_min tailing time" }},
{ 0xB8, new C{ T="Float", N = "_tailing radius.min" }},
{ 0xBC, new C{ T="Float", N = "_tailing radius.max" }},
{ 0xC0, new C{ T="Float", N = "_tailing cone angle" }},
{ 0xC4708, new C{ T="Comment", N = "_Strafing" }},
{ 0xC4, new C{ T="Float", N = "_volume strafe distance" }},
{ 0xC8, new C{ T="Float", N = "_strafe min distance" }},
{ 0xCC, new C{ T="Float", N = "_strafe above distance" }},
{ 0xD0, new C{ T="Float", N = "_strafe abort distance" }},
{ 0xD4, new C{ T="Float", N = "_strafe timeout" }},
{ 0xD8709, new C{ T="Comment", N = "_Attitude Control" }},
{ 0xD8, new C{ T="Float", N = "_max descend angle" }},
{ 0xDC, new C{ T="Float", N = "_max ascend angle" }},
{ 0xE0710, new C{ T="Comment", N = "_Shooting" }},
{ 0xE0, new C{ T="Float", N = "_shooting cone angle" }},
{ 0xE4711, new C{ T="Comment", N = "_Evasive Maneuvers" }},
{ 0xE4, new C{ T="Float", N = "_missile dodge change" }},
{ 0xE8, new C{ T="Float", N = "_ideal missile dodge distance" }},
{ 0xEC, new C{ T="Float", N = "_Dodge Timeout.min" }},
{ 0xF0, new C{ T="Float", N = "_Dodge Timeout.max" }},
}, S=244}},
{ 0x65C, new C{ T="Tagblock", N = "_equipment definitions", B = new Dictionary<long, C>
{
{ 0x0712, new C{ T="Comment", N = "_Equipment" }},
{ 0x0, new C{ T="TagRef", N = "_equipment" }},
{ 0x1C, new C{ T="4Byte", N = "_flags" }},
{ 0x20, new C{ T="Float", N = "_relative drop chance" }},
{ 0x24, new C{ T="mmr3Hash", N = "_Animation" }},
{ 0x28, new C{ T="Tagblock", N = "_equipment use", B = new Dictionary<long, C>
{
{ 0x0713, new C{ T="Comment", N = "_Equipment Use" }},
{ 0x0, new C{ T="Tagblock", N = "_Use Conditions", B = new Dictionary<long, C>
{
{ 0x0714, new C{ T="Comment", N = "_Use Conditions" }},
{ 0x0, new C{ T="2Byte", N = "_use when" }},
{ 0x2, new C{ T="Unmapped", N = "_generated_padd2ce" }},
{ 0x4, new C{ T="Float", N = "_health/shield use threshold" }},
}, S=8}},
{ 0x14, new C{ T="2Byte", N = "_use how" }},
{ 0x16, new C{ T="Unmapped", N = "_generated_padd6c8" }},
{ 0x18715, new C{ T="Comment", N = "_Skip Fraction" }},
{ 0x18, new C{ T="Float", N = "_easy/normal" }},
{ 0x1C, new C{ T="Float", N = "_legendary" }},
{ 0x20, new C{ T="Float", N = "_Chance Per Second" }},
}, S=36}},
}, S=60}},
{ 0x670, new C{ T="Tagblock", N = "_stimuli responses", B = new Dictionary<long, C>
{
{ 0x0716, new C{ T="Comment", N = "_Stimulus Response" }},
{ 0x0, new C{ T="mmr3Hash", N = "_stimulus name" }},
{ 0x4, new C{ T="TagRef", N = "_override character" }},
{ 0x20, new C{ T="Unmapped", N = "_Resolved Stimulus" }},
}, S=40}},
{ 0x684, new C{ T="Tagblock", N = "_campaign metagame bucket", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Byte", N = "_flags" }},
{ 0x1, new C{ T="Byte", N = "_type" }},
{ 0x2, new C{ T="Byte", N = "_class" }},
{ 0x3, new C{ T="Unmapped", N = "_generated_pad6695" }},
{ 0x4, new C{ T="2Byte", N = "_point count" }},
}, S=6}},
{ 0x698, new C{ T="Tagblock", N = "_activity objects", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="mmr3Hash", N = "_activity name" }},
{ 0x4, new C{ T="TagRef", N = "_crate" }},
{ 0x20717, new C{ T="Comment", N = "_" }},
{ 0x20, new C{ T="mmr3Hash", N = "_crate marker name" }},
{ 0x24718, new C{ T="Comment", N = "_" }},
{ 0x24, new C{ T="mmr3Hash", N = "_unit marker name" }},
}, S=40}},
{ 0x6AC, new C{ T="Tagblock", N = "_pain screen properties", B = new Dictionary<long, C>
{
{ 0x0719, new C{ T="Comment", N = "_Pain Reactions" }},
{ 0x0, new C{ T="Float", N = "_pain screen duration" }},
{ 0x4, new C{ T="Float", N = "_pain screen region fade out dura" }},
{ 0x8, new C{ T="Float", N = "_pain screen region fade out weig" }},
{ 0xC, new C{ T="Float", N = "_pain screen angle tolerance" }},
{ 0x10, new C{ T="Float", N = "_pain screen angle randomness" }},
{ 0x14720, new C{ T="Comment", N = "_Defensive Reactions" }},
{ 0x14, new C{ T="Float", N = "_defensive screen duration" }},
{ 0x18, new C{ T="Float", N = "_defensive screen scrub fallback " }},
}, S=28}},
{ 0x6C0, new C{ T="Tagblock", N = "_Melee animation scaling properti", B = new Dictionary<long, C>
{
{ 0x0721, new C{ T="Comment", N = "_Easy Scaling" }},
{ 0x0, new C{ T="Float", N = "_Easy Windup Scaling" }},
{ 0x4, new C{ T="Float", N = "_Easy Recovery Scaling" }},
{ 0x8722, new C{ T="Comment", N = "_Normal Scaling" }},
{ 0x8, new C{ T="Float", N = "_Normal Windup Scaling" }},
{ 0xC, new C{ T="Float", N = "_Normal Recovery Scaling" }},
{ 0x10723, new C{ T="Comment", N = "_Heroic Scaling" }},
{ 0x10, new C{ T="Float", N = "_Heroic Windup Scaling" }},
{ 0x14, new C{ T="Float", N = "_Heroic Recovery Scaling" }},
{ 0x18724, new C{ T="Comment", N = "_Legendary Scaling" }},
{ 0x18, new C{ T="Float", N = "_Legendary Windup Scaling" }},
{ 0x1C, new C{ T="Float", N = "_Legendary Recovery Scaling" }},
}, S=32}},
{ 0x6D4, new C{ T="Tagblock", N = "_Close Quarters Defense Blast", B = new Dictionary<long, C>
{
{ 0x0725, new C{ T="Comment", N = "_Close Quarters Defense Blast Par" }},
{ 0x0726, new C{ T="Comment", N = "_Activation Chance" }},
{ 0x0, new C{ T="Float", N = "_Activation Chance (easy)" }},
{ 0x4, new C{ T="Float", N = "_Activation Chance (normal)" }},
{ 0x8, new C{ T="Float", N = "_Activation Chance (heroic)" }},
{ 0xC, new C{ T="Float", N = "_Activation Chance (legendary)" }},
{ 0x10727, new C{ T="Comment", N = "_" }},
{ 0x10, new C{ T="Float", N = "_cooldown" }},
{ 0x14, new C{ T="Float", N = "_attack range.min" }},
{ 0x18, new C{ T="Float", N = "_attack range.max" }},
{ 0x1C, new C{ T="mmr3Hash", N = "_marker" }},
{ 0x20, new C{ T="TagRef", N = "_damage effect" }},
{ 0x3C, new C{ T="TagRef", N = "_effect" }},
{ 0x58728, new C{ T="Comment", N = "_" }},
}, S=88}},
{ 0x6E8, new C{ T="Tagblock", N = "_bishop properties", B = new Dictionary<long, C>
{
{ 0x0729, new C{ T="Comment", N = "_Repair" }},
{ 0x0, new C{ T="TagRef", N = "_repair beam effect" }},
{ 0x1C730, new C{ T="Comment", N = "_Protect Allies" }},
{ 0x1C, new C{ T="Float", N = "_Min Follow Distance " }},
{ 0x20731, new C{ T="Comment", N = "_Resurrect Allies" }},
{ 0x20, new C{ T="Float", N = "_Resurrection Initiation Delay Ti" }},
{ 0x24, new C{ T="Float", N = "_Resurrection Charge Up Time (sec" }},
{ 0x28, new C{ T="Float", N = "_Resurrection Give Up Time (secon" }},
{ 0x2C, new C{ T="TagRef", N = "_Resurrection Ground Effect " }},
{ 0x48, new C{ T="TagRef", N = "_Res. Area Scan Start Effect " }},
{ 0x64, new C{ T="TagRef", N = "_Res. Area Scan Success Effect " }},
{ 0x80, new C{ T="TagRef", N = "_Res. Area Scan Failure Effect " }},
{ 0x9C732, new C{ T="Comment", N = "_Shard Spawning" }},
{ 0x9C, new C{ T="TagRef", N = "_Shard Spawn Ground Effect " }},
{ 0xB8, new C{ T="Float", N = "_Shard Creation Delay (seconds) " }},
{ 0xBC, new C{ T="Float", N = "_Spawn Delay (seconds) .min" }},
{ 0xC0, new C{ T="Float", N = "_Spawn Delay (seconds) .max" }},
{ 0xC4733, new C{ T="Comment", N = "_Healing Allies" }},
{ 0xC4, new C{ T="Float", N = "_Healing Time" }},
}, S=200}},
{ 0x6FC, new C{ T="Tagblock", N = "_Healing Properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Healing delay time" }},
{ 0x4, new C{ T="Float", N = "_Healing standby time" }},
}, S=8}},
{ 0x710, new C{ T="Tagblock", N = "_packmaster properties", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="TagRef", N = "_Pack Character Ancestor " }},
{ 0x1C, new C{ T="Float", N = "_command radius" }},
{ 0x20, new C{ T="4Byte", N = "_group fire min count" }},
{ 0x24, new C{ T="4Byte", N = "_group fire max count" }},
{ 0x28734, new C{ T="Comment", N = "_group fire chance" }},
{ 0x28, new C{ T="Float", N = "_group fire chance (easy)" }},
{ 0x2C, new C{ T="Float", N = "_group fire chance (normal)" }},
{ 0x30, new C{ T="Float", N = "_group fire chance (heroic)" }},
{ 0x34, new C{ T="Float", N = "_group fire chance (legendary)" }},
{ 0x38735, new C{ T="Comment", N = "_" }},
{ 0x38, new C{ T="Float", N = "_group fire burst time" }},
{ 0x3C, new C{ T="Float", N = "_group fire cooldown time" }},
{ 0x40, new C{ T="4Byte", N = "_group fire burst count" }},
{ 0x44, new C{ T="Float", N = "_group fire burst delay" }},
{ 0x48, new C{ T="Float", N = "_group fire target range" }},
{ 0x4C, new C{ T="Float", N = "_lion charge max range" }},
{ 0x50, new C{ T="Float", N = "_lion charge chance" }},
{ 0x54, new C{ T="Float", N = "_lion charge melee range" }},
{ 0x58, new C{ T="Float", N = "_lion charge fire duration" }},
{ 0x5C, new C{ T="Float", N = "_lion charge throttle override" }},
{ 0x60, new C{ T="Float", N = "_lion charge abort range" }},
{ 0x64, new C{ T="Float", N = "_pack charge distance" }},
{ 0x68, new C{ T="Float", N = "_pack charge cooldown" }},
}, S=108}},
{ 0x724, new C{ T="Tagblock", N = "_Dynamic Task", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Agent Filter Flags " }},
{ 0x4, new C{ T="4Byte", N = "_Task Generation Flags " }},
{ 0x8, new C{ T="Float", N = "_Protect Request Shield Level " }},
{ 0xC, new C{ T="Float", N = "_Protect Request Body Level " }},
{ 0x10, new C{ T="Float", N = "_Resurrection Req. Chance " }},
{ 0x14, new C{ T="Float", N = "_Shield Task Priority " }},
{ 0x18, new C{ T="Float", N = "_Protect Task Priorities (Min Hea.min" }},
{ 0x1C, new C{ T="Float", N = "_Protect Task Priorities (Min Hea.max" }},
{ 0x20, new C{ T="Float", N = "_Resurrection Task Priority " }},
{ 0x24, new C{ T="Float", N = "_Shield During Resurrection Task " }},
{ 0x28, new C{ T="TagRef", N = "_Shield Crate Override " }},
{ 0x44, new C{ T="Float", N = "_Heal Request Shield Level " }},
{ 0x48, new C{ T="Float", N = "_Heal Request Body Level " }},
}, S=76}},
{ 0x738, new C{ T="Tagblock", N = "_Rush Attack", B = new Dictionary<long, C>
{
{ 0x0736, new C{ T="Comment", N = "_Rush Attack" }},
{ 0x0737, new C{ T="Comment", N = "_Generic Advance Attack" }},
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="Float", N = "_Range.min" }},
{ 0x8, new C{ T="Float", N = "_Range.max" }},
{ 0xC, new C{ T="Float", N = "_Max scariness" }},
{ 0x10, new C{ T="Byte", N = "_Threat Level" }},
{ 0x11, new C{ T="Unmapped", N = "_generated_pad8a75" }},
{ 0x14738, new C{ T="Comment", N = "_Activation Chance" }},
{ 0x14, new C{ T="Float", N = "_Activation Chance (easy)" }},
{ 0x18, new C{ T="Float", N = "_Activation Chance (normal)" }},
{ 0x1C, new C{ T="Float", N = "_Activation Chance (heroic)" }},
{ 0x20, new C{ T="Float", N = "_Activation Chance (legendary)" }},
{ 0x24739, new C{ T="Comment", N = "_" }},
{ 0x24, new C{ T="Float", N = "_Attack timeout" }},
{ 0x28, new C{ T="Float", N = "_Melee range" }},
{ 0x2C, new C{ T="Float", N = "_Shield down threshold" }},
{ 0x30, new C{ T="Float", N = "_Abort danger threshold" }},
{ 0x34, new C{ T="Float", N = "_Abort distance" }},
{ 0x38, new C{ T="Float", N = "_Outside area range" }},
{ 0x3C, new C{ T="Float", N = "_Berserk abort distance" }},
{ 0x40, new C{ T="Float", N = "_Cooldown" }},
{ 0x44, new C{ T="Float", N = "_Tell animation chance" }},
{ 0x48, new C{ T="Float", N = "_Tell Animation Cooldown" }},
{ 0x4C, new C{ T="mmr3Hash", N = "_Throttle style" }},
{ 0x50740, new C{ T="Comment", N = "_Slow Advance" }},
{ 0x50741, new C{ T="Comment", N = "_Slow Advance" }},
{ 0x50, new C{ T="Float", N = "_Slow range" }},
{ 0x54, new C{ T="mmr3Hash", N = "_Slow throttle style" }},
{ 0x58, new C{ T="Float", N = "_Exit range" }},
{ 0x5C742, new C{ T="Comment", N = "_Rush specific settings" }},
{ 0x5C, new C{ T="4Byte", N = "_Rush flags" }},
{ 0x60, new C{ T="Float", N = "_Leader Abandoned Chance" }},
{ 0x64, new C{ T="Float", N = "_Berserk chance" }},
{ 0x68, new C{ T="Float", N = "_Berserk recheck cooldown" }},
{ 0x6C, new C{ T="Float", N = "_Min abort time" }},
{ 0x70, new C{ T="Float", N = "_Min abort dist travelled" }},
{ 0x74, new C{ T="Float", N = "_Vertical Range Scalar" }},
{ 0x78, new C{ T="Float", N = "_Zig-zag chance" }},
{ 0x7C, new C{ T="Float", N = "_Zig-zag angle" }},
{ 0x80, new C{ T="Float", N = "_Zig-zag period" }},
{ 0x84, new C{ T="Float", N = "_Tackle angle" }},
{ 0x88, new C{ T="Float", N = "_Tackle wait" }},
{ 0x8C, new C{ T="Tagblock", N = "_Combo Definitions", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Combo Flags" }},
{ 0x4, new C{ T="Float", N = "_Weight Modifier" }},
{ 0x8, new C{ T="Tagblock", N = "_Attacks", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Direction Flags" }},
{ 0x4, new C{ T="mmr3Hash", N = "_Animation Name" }},
{ 0x8, new C{ T="TagRef", N = "_Damage Override" }},
}, S=36}},
}, S=28}},
{ 0xA0, new C{ T="mmr3Hash", N = "_Taunt Animation Name" }},
}, S=164}},
{ 0x74C, new C{ T="Tagblock", N = "_Leap Attack", B = new Dictionary<long, C>
{
{ 0x0743, new C{ T="Comment", N = "_Leap Attack" }},
{ 0x0744, new C{ T="Comment", N = "_Generic Advance Attack" }},
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="Float", N = "_Range.min" }},
{ 0x8, new C{ T="Float", N = "_Range.max" }},
{ 0xC, new C{ T="Float", N = "_Max scariness" }},
{ 0x10, new C{ T="Byte", N = "_Threat Level" }},
{ 0x11, new C{ T="Unmapped", N = "_generated_pad8a75" }},
{ 0x14745, new C{ T="Comment", N = "_Activation Chance" }},
{ 0x14, new C{ T="Float", N = "_Activation Chance (easy)" }},
{ 0x18, new C{ T="Float", N = "_Activation Chance (normal)" }},
{ 0x1C, new C{ T="Float", N = "_Activation Chance (heroic)" }},
{ 0x20, new C{ T="Float", N = "_Activation Chance (legendary)" }},
{ 0x24746, new C{ T="Comment", N = "_" }},
{ 0x24, new C{ T="Float", N = "_Attack timeout" }},
{ 0x28, new C{ T="Float", N = "_Melee range" }},
{ 0x2C, new C{ T="Float", N = "_Shield down threshold" }},
{ 0x30, new C{ T="Float", N = "_Abort danger threshold" }},
{ 0x34, new C{ T="Float", N = "_Abort distance" }},
{ 0x38, new C{ T="Float", N = "_Outside area range" }},
{ 0x3C, new C{ T="Float", N = "_Berserk abort distance" }},
{ 0x40, new C{ T="Float", N = "_Cooldown" }},
{ 0x44, new C{ T="Float", N = "_Tell animation chance" }},
{ 0x48, new C{ T="Float", N = "_Tell Animation Cooldown" }},
{ 0x4C, new C{ T="mmr3Hash", N = "_Throttle style" }},
{ 0x50747, new C{ T="Comment", N = "_Slow Advance" }},
{ 0x50748, new C{ T="Comment", N = "_Slow Advance" }},
{ 0x50, new C{ T="Float", N = "_Slow range" }},
{ 0x54, new C{ T="mmr3Hash", N = "_Slow throttle style" }},
{ 0x58, new C{ T="Float", N = "_Exit range" }},
{ 0x5C749, new C{ T="Comment", N = "_Leap specific settings" }},
{ 0x5C, new C{ T="4Byte", N = "_Leap Flags" }},
{ 0x60, new C{ T="Float", N = "_Ideal leap speed" }},
{ 0x64, new C{ T="Float", N = "_Max leap speed" }},
{ 0x68, new C{ T="Float", N = "_Ballistic fraction" }},
{ 0x6C, new C{ T="Float", N = "_Prediction percent" }},
{ 0x70, new C{ T="Float", N = "_Shortfall" }},
{ 0x74, new C{ T="Float", N = "_Swoop accel rate" }},
{ 0x78, new C{ T="Float", N = "_Swoop accel time" }},
{ 0x7C, new C{ T="Float", N = "_Swoop max deviation" }},
{ 0x80, new C{ T="Float", N = "_Swoop prediction" }},
}, S=132}},
{ 0x760, new C{ T="Tagblock", N = "_Teleport Attack", B = new Dictionary<long, C>
{
{ 0x0750, new C{ T="Comment", N = "_Teleport attack" }},
{ 0x0751, new C{ T="Comment", N = "_Generic Advance Attack" }},
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="Float", N = "_Range.min" }},
{ 0x8, new C{ T="Float", N = "_Range.max" }},
{ 0xC, new C{ T="Float", N = "_Max scariness" }},
{ 0x10, new C{ T="Byte", N = "_Threat Level" }},
{ 0x11, new C{ T="Unmapped", N = "_generated_pad8a75" }},
{ 0x14752, new C{ T="Comment", N = "_Activation Chance" }},
{ 0x14, new C{ T="Float", N = "_Activation Chance (easy)" }},
{ 0x18, new C{ T="Float", N = "_Activation Chance (normal)" }},
{ 0x1C, new C{ T="Float", N = "_Activation Chance (heroic)" }},
{ 0x20, new C{ T="Float", N = "_Activation Chance (legendary)" }},
{ 0x24753, new C{ T="Comment", N = "_" }},
{ 0x24, new C{ T="Float", N = "_Attack timeout" }},
{ 0x28, new C{ T="Float", N = "_Melee range" }},
{ 0x2C, new C{ T="Float", N = "_Shield down threshold" }},
{ 0x30, new C{ T="Float", N = "_Abort danger threshold" }},
{ 0x34, new C{ T="Float", N = "_Abort distance" }},
{ 0x38, new C{ T="Float", N = "_Outside area range" }},
{ 0x3C, new C{ T="Float", N = "_Berserk abort distance" }},
{ 0x40, new C{ T="Float", N = "_Cooldown" }},
{ 0x44, new C{ T="Float", N = "_Tell animation chance" }},
{ 0x48, new C{ T="Float", N = "_Tell Animation Cooldown" }},
{ 0x4C, new C{ T="mmr3Hash", N = "_Throttle style" }},
{ 0x50754, new C{ T="Comment", N = "_Slow Advance" }},
{ 0x50755, new C{ T="Comment", N = "_Slow Advance" }},
{ 0x50, new C{ T="Float", N = "_Slow range" }},
{ 0x54, new C{ T="mmr3Hash", N = "_Slow throttle style" }},
{ 0x58, new C{ T="Float", N = "_Exit range" }},
{ 0x5C756, new C{ T="Comment", N = "_Teleport specific settings" }},
{ 0x5C, new C{ T="Float", N = "_Destination distance" }},
{ 0x60, new C{ T="Float", N = "_Side step distance" }},
{ 0x64, new C{ T="Float", N = "_Forward distance" }},
{ 0x68, new C{ T="Float", N = "_Tracking distance" }},
{ 0x6C, new C{ T="Float", N = "_Runup time" }},
{ 0x70, new C{ T="Float", N = "_Position pause" }},
{ 0x74, new C{ T="Float", N = "_Escape Cooldown" }},
{ 0x78, new C{ T="Float", N = "_Lock Distance" }},
{ 0x7C, new C{ T="TagRef", N = "_Launch effect" }},
{ 0x98, new C{ T="TagRef", N = "_Land effect" }},
}, S=180}},
{ 0x774, new C{ T="Tagblock", N = "_Jink and Shoot", B = new Dictionary<long, C>
{
{ 0x0757, new C{ T="Comment", N = "_Jink and Shoot" }},
{ 0x0, new C{ T="Float", N = "_Start distance.min" }},
{ 0x4, new C{ T="Float", N = "_Start distance.max" }},
{ 0x8, new C{ T="Float", N = "_Start max offset angle" }},
{ 0xC, new C{ T="Float", N = "_Min start teleport distance" }},
{ 0x10, new C{ T="4Byte", N = "_Number of intermediate teleports" }},
{ 0x14, new C{ T="Float", N = "_Max offset angle" }},
{ 0x18, new C{ T="Float", N = "_Side step distance.min" }},
{ 0x1C, new C{ T="Float", N = "_Side step distance.max" }},
{ 0x20, new C{ T="Float", N = "_Forward distance.min" }},
{ 0x24, new C{ T="Float", N = "_Forward distance.max" }},
{ 0x28, new C{ T="TagRef", N = "_Launch effect" }},
{ 0x44, new C{ T="TagRef", N = "_Land effect" }},
{ 0x60, new C{ T="mmr3Hash", N = "_Effect marker" }},
{ 0x64, new C{ T="TagRef", N = "_Jink and Shoot Weapon" }},
}, S=128}},
{ 0x788, new C{ T="Tagblock", N = "_Teleport and Tackle", B = new Dictionary<long, C>
{
{ 0x0758, new C{ T="Comment", N = "_Teleport and Tackle" }},
{ 0x0, new C{ T="Float", N = "_Destination distance.min" }},
{ 0x4, new C{ T="Float", N = "_Destination distance.max" }},
{ 0x8, new C{ T="Float", N = "_Max offset angle" }},
{ 0xC, new C{ T="Float", N = "_Duration to Align" }},
{ 0x10, new C{ T="Float", N = "_Min teleport distance" }},
{ 0x14, new C{ T="TagRef", N = "_Launch effect" }},
{ 0x30, new C{ T="TagRef", N = "_Land effect" }},
{ 0x4C, new C{ T="mmr3Hash", N = "_Effect marker" }},
{ 0x50, new C{ T="TagRef", N = "_Teleport and Tackle Weapon" }},
}, S=108}},
{ 0x79C, new C{ T="Tagblock", N = "_Teleport and Taunt Animation", B = new Dictionary<long, C>
{
{ 0x0759, new C{ T="Comment", N = "_Teleport and Taunt Behavior" }},
{ 0x0, new C{ T="mmr3Hash", N = "_Custom Animation Name" }},
{ 0x4, new C{ T="Float", N = "_Taunt Frequency" }},
{ 0x8, new C{ T="Float", N = "_Taunt chance" }},
{ 0xC, new C{ T="Float", N = "_Destination distance.min" }},
{ 0x10, new C{ T="Float", N = "_Destination distance.max" }},
{ 0x14, new C{ T="Float", N = "_Max offset angle" }},
{ 0x18, new C{ T="Float", N = "_Min teleport distance" }},
{ 0x1C, new C{ T="TagRef", N = "_Launch effect" }},
{ 0x38, new C{ T="TagRef", N = "_Land effect" }},
{ 0x54, new C{ T="mmr3Hash", N = "_Effect marker" }},
}, S=88}},
{ 0x7B0, new C{ T="Tagblock", N = "_Desperation Rush", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Properties" }},
{ 0x4, new C{ T="Float", N = "_Range" }},
}, S=8}},
{ 0x7C4, new C{ T="Tagblock", N = "_Proximity Melee", B = new Dictionary<long, C>
{
{ 0x0760, new C{ T="Comment", N = "_Proximity Melee Impulse" }},
{ 0x0, new C{ T="Float", N = "_Range" }},
{ 0x4, new C{ T="Float", N = "_Cooldown" }},
{ 0x8, new C{ T="Float", N = "_Outside area range" }},
}, S=12}},
{ 0x7D8, new C{ T="Tagblock", N = "_Dive Bomb Kamikaze", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Ally target chance" }},
{ 0x4, new C{ T="Float", N = "_Enemy target chance" }},
{ 0x8, new C{ T="Float", N = "_Travel distance.min" }},
{ 0xC, new C{ T="Float", N = "_Travel distance.max" }},
{ 0x10, new C{ T="Float", N = "_Travel time before exploding" }},
{ 0x14, new C{ T="TagRef", N = "_Impact detonation projectile" }},
{ 0x30, new C{ T="Float", N = "_Target error range.min" }},
{ 0x34, new C{ T="Float", N = "_Target error range.max" }},
{ 0x38, new C{ T="Byte", N = "_Behavior flags" }},
{ 0x39, new C{ T="Unmapped", N = "_generated_pad94ce" }},
{ 0x3C, new C{ T="Float", N = "_Kamikaze below health percentage" }},
{ 0x40, new C{ T="Float", N = "_Chance to kamikaze at low health" }},
{ 0x44, new C{ T="Float", N = "_Chance to kamikaze when last man" }},
{ 0x48, new C{ T="Float", N = "_a Pause between Kamikaze anim an" }},
}, S=76}},
{ 0x7EC, new C{ T="Tagblock", N = "_Post Combat", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Shoot Corpse Chance" }},
{ 0x4, new C{ T="Float", N = "_Postcombat Phase 1 Time.min" }},
{ 0x8, new C{ T="Float", N = "_Postcombat Phase 1 Time.max" }},
}, S=12}},
{ 0x800, new C{ T="Tagblock", N = "_Pack Stalk", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Min Wander Distance " }},
{ 0x4, new C{ T="Float", N = "_Outside Area Border " }},
{ 0x8, new C{ T="Float", N = "_Position Update Delay .min" }},
{ 0xC, new C{ T="Float", N = "_Position Update Delay .max" }},
{ 0x10, new C{ T="Float", N = "_Throttle In Cover " }},
}, S=20}},
{ 0x814, new C{ T="Tagblock", N = "_Fight Circle", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Strafe Time .min" }},
{ 0x4, new C{ T="Float", N = "_Strafe Time .max" }},
{ 0x8, new C{ T="Float", N = "_Extra Firing Time .min" }},
{ 0xC, new C{ T="Float", N = "_Extra Firing Time .max" }},
{ 0x10, new C{ T="Float", N = "_Target Patience Time " }},
{ 0x14, new C{ T="Float", N = "_Max Angle From ThreatAxis " }},
{ 0x18, new C{ T="Float", N = "_Nearby Inner Angle " }},
{ 0x1C, new C{ T="Float", N = "_Nearby Outer Angle " }},
{ 0x20, new C{ T="mmr3Hash", N = "_strafe throttle style " }},
{ 0x24, new C{ T="mmr3Hash", N = "_move throttle style " }},
}, S=40}},
{ 0x828, new C{ T="Tagblock", N = "_Hamstring", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Flank Angle " }},
{ 0x4, new C{ T="Float", N = "_Flank Distance " }},
{ 0x8, new C{ T="Float", N = "_Outer Engage Distance " }},
{ 0xC, new C{ T="Float", N = "_Hamstring Delay " }},
{ 0x10, new C{ T="Float", N = "_Initiate chance " }},
{ 0x14, new C{ T="Float", N = "_Max Rush Time " }},
{ 0x18, new C{ T="Float", N = "_Melee Attack Time Min " }},
{ 0x1C, new C{ T="Float", N = "_Melee Attack Time Max " }},
{ 0x20, new C{ T="Float", N = "_Max Charge Range" }},
}, S=36}},
{ 0x83C, new C{ T="Tagblock", N = "_Forerunner", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_forerunner flags" }},
{ 0x4, new C{ T="Float", N = "_order minion charge chance " }},
{ 0x8, new C{ T="Float", N = "_order minion charge radius " }},
{ 0xC, new C{ T="Float", N = "_minion charge min time " }},
{ 0x10, new C{ T="Float", N = "_minion charge max time " }},
{ 0x14, new C{ T="Float", N = "_Phase To Position distance bound.min" }},
{ 0x18, new C{ T="Float", N = "_Phase To Position distance bound.max" }},
{ 0x1C, new C{ T="Float", N = "_Phase To Position Probability ra.min" }},
{ 0x20, new C{ T="Float", N = "_Phase To Position Probability ra.max" }},
}, S=36}},
{ 0x850, new C{ T="Tagblock", N = "_Gravity Jump", B = new Dictionary<long, C>
{
{ 0x0761, new C{ T="Comment", N = "_Activation Chance" }},
{ 0x0, new C{ T="Float", N = "_Activation Chance (easy)" }},
{ 0x4, new C{ T="Float", N = "_Activation Chance (normal)" }},
{ 0x8, new C{ T="Float", N = "_Activation Chance (heroic)" }},
{ 0xC, new C{ T="Float", N = "_Activation Chance (legendary)" }},
{ 0x10762, new C{ T="Comment", N = "_" }},
{ 0x10, new C{ T="Float", N = "_Float Time " }},
{ 0x14, new C{ T="Float", N = "_Float Gravity Scale" }},
{ 0x18763, new C{ T="Comment", N = "_Descent Gravity Scale Function" }},
{ 0x18764, new C{ T="Comment", N = "_descendGravityScaleFunction" }},
{ 0x18765, new C{ T="Comment", N = "_" }},
{ 0x18, new C{ T="Unmapped", N = "_data" }},
{ 0x30766, new C{ T="Comment", N = "_" }},
{ 0x30, new C{ T="Float", N = "_Slow descend time " }},
{ 0x34, new C{ T="Float", N = "_Jump Target Height " }},
{ 0x38, new C{ T="Float", N = "_Cooldown " }},
{ 0x3C, new C{ T="Float", N = "_Trigger Distance " }},
}, S=64}},
{ 0x864, new C{ T="Tagblock", N = "_Co-op Modifiers", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Body Vitality Scale for 2-3 Play" }},
{ 0x4, new C{ T="Float", N = "_Body Vitality Scale for 4 Player" }},
{ 0x8, new C{ T="Float", N = "_Shield Vitality Scale for 2-3 Pl" }},
{ 0xC, new C{ T="Float", N = "_Shield Vitality Scale for 4 Play" }},
}, S=16}},
{ 0x878, new C{ T="Tagblock", N = "_Multi Target Tracked Attack", B = new Dictionary<long, C>
{
{ 0x0767, new C{ T="Comment", N = "_activation chance" }},
{ 0x0, new C{ T="Float", N = "_activation chance (easy)" }},
{ 0x4, new C{ T="Float", N = "_activation chance (normal)" }},
{ 0x8, new C{ T="Float", N = "_activation chance (heroic)" }},
{ 0xC, new C{ T="Float", N = "_activation chance (legendary)" }},
{ 0x10768, new C{ T="Comment", N = "_" }},
{ 0x10, new C{ T="Float", N = "_targeting angle" }},
{ 0x14, new C{ T="Float", N = "_activation range.min" }},
{ 0x18, new C{ T="Float", N = "_activation range.max" }},
{ 0x1C, new C{ T="Byte", N = "_min targets in range" }},
{ 0x1D, new C{ T="Byte", N = "_bursts" }},
{ 0x1E, new C{ T="Byte", N = "_trigger" }},
{ 0x1F, new C{ T="Unmapped", N = "_generated_pade74f" }},
{ 0x20, new C{ T="Float", N = "_cooldown" }},
}, S=36}},
{ 0x88C, new C{ T="Tagblock", N = "_Drop Objects", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="4Byte", N = "_Number of Rolls" }},
{ 0x4, new C{ T="Float", N = "_Min Item Launch Velocity" }},
{ 0x8, new C{ T="Float", N = "_Max Item Launch Velocity" }},
{ 0xC, new C{ T="Float", N = "_Max Trajectory Radius" }},
{ 0x10, new C{ T="Tagblock", N = "_Drop Objects", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Float", N = "_Easy_Normal Drop Chance" }},
{ 0x4, new C{ T="Float", N = "_Heroic Drop Chance" }},
{ 0x8, new C{ T="Float", N = "_Legendary Drop Chance" }},
{ 0xC, new C{ T="TagRef", N = "_Drop Object" }},
}, S=40}},
}, S=36}},
{ 0x8A0, new C{ T="Tagblock", N = "_Custom Scripting", B = new Dictionary<long, C>
{
{ 0x0, new C{ T="Tagblock", N = "_Scripted Threshold", B = new Dictionary<long, C>
{
{ 0x0769, new C{ T="Comment", N = "_Scripted Thresholds" }},
{ 0x0, new C{ T="4Byte", N = "_Flags" }},
{ 0x4, new C{ T="2Byte", N = "_Tracked Attribute" }},
{ 0x6, new C{ T="2Byte", N = "_Trigger When" }},
{ 0x8, new C{ T="Float", N = "_Threshold Value" }},
{ 0xC, new C{ T="Float", N = "_Recheck Buffer" }},
{ 0x10, new C{ T="Float", N = "_Recheck Cooldown" }},
{ 0x14, new C{ T="String", N = "_Function" }},
}, S=276}},
}, S=20}},
{ 0x8B4, new C{ T="Unmapped", N = "_generated_pad539d" }},
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

				{ 92, new C { T = "Tagblock", N = "Source Sidecar" } }, // SidecarPathDefinition

				{ 112, new C { T = "mmr3Hash", N = "Default Model Variant" } }, // default variant
				{ 116, new C { T = "Float", N = "Default Style ID" } },

				{ 120, new C { T = "TagRef", N = "Model" } }, // vehicle model 
				{ 148, new C { T = "TagRef", N = "Asset" } }, // aset tag ref
				{ 176, new C { T = "TagRef" } },
				{ 204, new C { T = "TagRef" } },

				{ 232, new C { T = "4Byte" } },

				{ 240, new C { T = "TagRef" } },

				{ 276, new C { T = "Float" } },

				{ 288, new C { T = "TagRef" } },
				{ 316, new C { T = "TagRef", N = "Material Effects" } }, // foot tag ref
				{ 344, new C { T = "TagRef", N = "Visual Material Effects" } }, // vemd tag ref
				{ 372, new C { T = "TagRef", N = "Sound Material Effects" } }, // smed tag ref
				{ 400, new C { T = "TagRef" } },

				{ 432, new C { T = "Float" } },

				{ 448, new C { T = "Tagblock", N = "AI Properties" } }, // object_ai_properties
				{ 468, new C { T = "Tagblock", N = "Functions" } }, // s_object_function_definition
				{ 488, new C { T = "4Byte" } },
				{ 492, new C { T = "Tagblock", N = "Runtime Interpolator Functions" } }, // ObjectRuntimeInterpolatorFunctionsBlock
				{ 512, new C { T = "Tagblock", N = "Function Switches" } }, // ObjectFunctionSwitchDefinition
				{ 532, new C { T = "Tagblock", N = "Functions Forwarded To Parent" } }, // i343::Objects::ObjectFunctionForwarding
				{ 552, new C { T = "4Byte" } },
				{ 556, new C { T = "Tagblock", N = "Ammo Refill Variant Flags" } }, // i343::Objects::AmmoRefillVariant
				{ 576, new C { T = "4Byte", N = "Variant Name" } },
				{ 0x248,new C{T = "Tagblock", N = "Attachments",B = new Dictionary<long, C>  // object_attachment_definition
				{
					{ 4, new C{ T="TagRef", N = "Type"}}, // effe
					{ 32, new C{ T="TagRef", N = "Tag Graph Output"}}, // effe
					{ 64, new C{ T="Tagblock", N = "Tag Graph Float Params"}},
					{ 84, new C{ T="TagRef", N = "Override Type"}}, //
					{ 112, new C{ T="Tagblock", N = "Variant Names"}}
				},S = 148}},

				{ 604, new C { T = "Tagblock", N = "Indirect Lighting Data" } }, // object_indirect_lighting_settings_definition
				{ 624, new C { T = "Tagblock", N = "Hull Surfaces" } }, // s_water_physics_hull_surface_definition
				{ 644, new C { T = "Tagblock", N = "Jetwash" } }, // s_jetwash_definition
				{ 664, new C { T = "Tagblock", N = "Widgets" } }, // object_definition_widget
				{ 684, new C { T = "Tagblock", N = "Change Colors" } }, // object_change_color_definition
				{ 704, new C { T = "Tagblock", N = "Multiplayer Object" } }, // s_multiplayer_object_properties_definition
				{ 724, new C { T = "Tagblock", N = "Forge Data" } }, // i343::Objects::ForgeObjectEntryDefinition

				{ 744, new C { T = "TagRef" } },
				{ 772, new C { T = "TagRef" } },

				{ 800, new C { T = "Tagblock", N = "Spawn Effects" } }, // s_object_spawn_effects
				{ 820, new C { T = "Tagblock", N = "Model Dissolve Data" } }, // ModelDissolveDataBlock


				{ 0x348, new C { T = "String", N = "Class Name" } },
				{ 0x45C, new C { T = "TagRef", N = "Prototype Script" } },

				{ 0x484, new C { T = "Tagblock", N = "Designer Metadata" } },
				{ 0x498, new C { T = "Tagblock", N = "Object Sound RTPCs" } },
				{ 0x4AC, new C { T = "Tagblock", N = "Object Sound Sweeteners" } },
				{ 0x4C8, new C { T = "Tagblock", N = "Object Function Smoothing" } },
				{ 0x4E0, new C { T = "Float" } },

				{ 0x4E4, new C { T = "TagRef" } },
				{ 0x500, new C { T = "Tagblock", N = "Tracking Info" } },
				{ 0x514, new C { T = "TagRef", N = "Property Based Effects" } },

				{ 0x530, new C { T = "Tagblock", N = "Interactions" } },
				{ 0x548, new C { T = "Tagblock", N = "Data Driven Scripted Sequence Action Defenitions" } },
				{ 0x55C, new C { T = "TagRef", N = "Channels" } },
				{ 0x578, new C { T = "Tagblock", N = "Table Entries" } },
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

				{ 0x5E8, new C { T = "Tagblock", N = "Leg Grounding Settings" } },

				{ 0x5FC, new C { T = "Float" } },
				{ 0x600, new C { T = "Float" } },
				{ 0x604, new C { T = "Float" } },

				{ 0x60C, new C { T = "Tagblock", N = "Object Node Graphs" } },
				{ 0x620, new C { T = "Tagblock", N = "Parent Animation Matching" } },
				{ 0x638, new C { T = "Tagblock", N = "Model Variant Switching Table" } },

				{ 0x64C, new C { T = "Float" } },

				{ 0x650, new C { T = "Tagblock", N = "Location Sensor" } },
				{ 0x664, new C { T = "Tagblock", N = "Shroud Generator" } },
				{ 0x678, new C { T = "Tagblock", N = "Power Component" } },
				{ 0x68C, new C { T = "Tagblock", N = "Self Destruct Handler" } },
				{ 0x6A0, new C { T = "Tagblock", N = "Indirect Lighting Component" } },

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

				{ 0x6C4, new C { T = "TagRef", N="Hologram Unit Reference" } },

				{ 0x6E0, new C { T = "Tagblock", N = "Campaign Metagame Bucket" } }, // metagame bucket
				{ 0x6F4, new C { T = "Tagblock" } },
				{ 0x720, new C { T = "Tagblock", B = new Dictionary<long, C> // camera tracks
				{
					{ 0x0, new C{ T="TagRef", N="3rdperson cam"}},
					{ 0x1C, new C{ T="TagRef", N = "Screen Effects"}},

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

				{ 0xDA4, new C { T = "Float", N="Stationary Turning Threshold" } },

				{ 0xDAC, new C { T = "TagRef" } },

				{ 0xDD0, new C { T = "Float", N = "Jump Velocity" } },

				{ 0xDD4, new C { T = "Tagblock", N = "Tricks" } }, //s_unit_trick_definition

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

				{ 0x1038, new C { T = "Float", N = "Bank Angle" } },
				{ 0x103C, new C { T = "Float", N = "Bank Apply Time" } },
				{ 0x1040, new C { T = "Float", N = "Bank Decay Time" } },
				{ 0x1044, new C { T = "Float", N = "Pitch Ratio" } },
				{ 0x1048, new C { T = "Float", N = "Max Velocity" } },
				{ 0x104C, new C { T = "Float", N = "Max Sidestep Velocity" } },
				{ 0x1050, new C { T = "Float", N = "Acceleration" } },
				{ 0x1054, new C { T = "Float", N = "Deceleation" } },
				{ 0x1058, new C { T = "Float", N = "Angular Velocity Maximum" } },
				{ 0x105C, new C { T = "Float", N = "Angular Acceleration Maximum" } },
				{ 0x1060, new C { T = "Float", N = "Crouch Velocity Modifier" } },

				{ 0x1064, new FlagGroup { A = 4, STR = new Dictionary<int, string>()
				{
					{ 0,  "Use World Up"  },
				} } },

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

				{ 0x2C, new C { T="TagRef", N = "Activation Effect"} },
				{ 0x50, new C { T="TagRef", N = "Active Malleable Properties Modifier"} },
				{ 0x6C, new C { T="TagRef", N = "Timed Malleable Properties Modifier"} },

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

				{ 0x98, new C { T="TagRef", N = "Rope Effect"} },
				{ 0xB8, new C { T="TagRef", N = "Pulling Effect"} },
				{ 0xD4, new C { T="TagRef", N = "Reeling Effect"} },
				{ 0xF0, new C { T="TagRef", N = "Deactivation Effect"} },
				{ 0x10C, new C { T="TagRef", N = "Projectile"} },
				{ 0x12C, new C { T="TagRef", N = "Object"} },

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

				{ 0x380, new C { T = "Float", N = "Bash Activation Hold Time" } },
				{ 0x384, new C { T = "Float", N = "Bash Max Aim Angle" } },
				{ 0x388, new C { T = "Float", N = "Bash Aim Lock Rate of Change" } },
				{ 0x38C, new C { T = "Float", N = "Bash Peak Velocity" } },
				{ 0x390, new C { T = "Float", N = "Bash Charge Up Duration" } },
				{ 0x3AC, new C { T = "Float", N = "Bash Acceleration Duration" } },

				{ 0x400, new C { T="TagRef", N = "BashCharge Up Effect"} },
				{ 0x41C, new C { T="TagRef", N = "Bash Launch Effect"} },
				{ 0x438, new C { T="TagRef", N = "Bash Active Effect"} },
				{ 0x454, new C { T="TagRef", N = "Bash Completion Effect"} },

				{ 0x470, new C { T="TagRef", N = "Owner Damage"} },
				{ 0x48C, new C { T="TagRef", N = "AOE Damage"} },

				{ 0x4B4, new C { T = "Float", N = "Miss Cooldown Delay" } },
			}},

			{"eqip",new()
			{
				{ 0x10, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x14, new C { T = "Float" } },
				{ 0x1C, new C { T = "Float" } },
				{ 0x20, new C { T = "Float" } },
				{ 0x24, new C { T = "Float" } },
				{ 0x28, new C { T = "Float" } },
				{ 0x2C, new C { T = "Float" } },
				{ 0x30, new C { T = "Float" } },
				{ 0x34, new C { T = "Float" } },
				{ 0x38, new C { T = "Float" } },
				{ 0x3C, new C { T = "Float" } },
				{ 0x40, new C { T = "Float" } },

				{ 0x44, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x48, new C { T = "Float" } },
				{ 0x4C, new C { T = "Float" } },
				{ 0x50, new C { T = "Float" } },
				{ 0x54, new C { T = "Float" } },
				{ 0x58, new C { T = "Float" } },

				{ 0x5C, new C{ T="Tagblock"}}, //SidecarPathDefinition
				
				{ 0x78, new C { T="TagRef", N = "Model" } },
				{ 0x94, new C { T="TagRef" } },
				{ 0xB0, new C { T="TagRef" } },
				{ 0xCC, new C { T="TagRef" } },
				{ 0xF0, new C { T="TagRef", N = "Collision Damage" } },

				{ 0x10C, new C{ T="Tagblock"}}, //s_object_early_mover_obb_definition

				{ 0x120, new C { T="TagRef" } },
				{ 0x13C, new C { T="TagRef", N = "Material Effect" } },
				{ 0x158, new C { T="TagRef" } },
				{ 0x174, new C { T="TagRef" } },
				{ 0x190, new C { T="TagRef", N = "Sound" } },

				{ 0x1B0, new C { T = "Float" } },

				{ 0x1C0, new C { T="Tagblock" } }, //object_ai_properties
				{ 0x1D4, new C { T="Tagblock" } }, //s_object_function_definition

				{ 0x1E4, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x1EC, new C{ T="Tagblock"}}, //ObjectRuntimeInterpolatorFunctionsBlock
				{ 0x200, new C{ T="Tagblock"}}, //ObjectFunctionSwitchDefinition
				{ 0x214, new C{ T="Tagblock"}}, //i343::Objects::ObjectFunctionForwarding
				{ 0x22C, new C{ T="Tagblock"}}, //i343::Objects::AmmoRefillVariant

				{ 0x240, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x248, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x4, new C { T="TagRef" } },
					{ 0x20, new C { T="TagRef" } },

					{ 0x40, new C{ T="Tagblock"}}, //TagGraph::TagGraphFloatParam
					
					{ 0x54, new C { T="TagRef" } },

					{ 0x70, new C{ T="Tagblock"}}, //i343::Objects::AttachmentVariantName
					{ 0x94, new C{ T="Tagblock"}}, //i343::Objects::AttachmentVariantName
					{ 0xA8, new C{ T="Tagblock"}}, //object_change_color_function
				} } }, //object_attachment_definition
				
				{ 0x25C, new C{ T="Tagblock"}}, //object_indirect_lighting_settings_definition
				{ 0x270, new C{ T="Tagblock"}}, //s_water_physics_hull_surface_definition
				{ 0x284, new C{ T="Tagblock"}}, //s_jetwash_definition
				{ 0x298, new C{ T="Tagblock"}}, //object_definition_widget
				{ 0x2AC, new C{ T="Tagblock"}}, //object_change_color_definition
				{ 0x2C0, new C{ T="Tagblock"}}, //s_multiplayer_object_properties_definition
				{ 0x2D4, new C{ T="Tagblock"}}, //i343::Objects::ForgeObjectEntryDefinition

				{ 0x2E4, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x2E8, new C { T="TagRef" } },
				{ 0x304, new C { T="TagRef" } },

				{ 0x320, new C{ T="Tagblock"}}, //s_object_spawn_effects
				{ 0x334, new C{ T="Tagblock"}}, //ModelDissolveDataBlock
				{ 0x448, new C{ T="Tagblock"}}, //HsReferencesBlock

				{ 0x45C, new C { T="TagRef" } },

				{ 0x478, new C { T = "Float" } },
				{ 0x47C, new C { T = "Float" } },
				{ 0x480, new C { T = "Float" } },

				{ 0x484, new C{ T="Tagblock"}}, //s_object_meta_label
				{ 0x498, new C{ T="Tagblock"}}, //SoundRTPCBlockDefinition
				{ 0x4AC, new C{ T="Tagblock"}}, //SoundSweetenerBlockDefinition
				{ 0x4C8, new C{ T="Tagblock"}}, //i343::Objects::ComputeFunctionSmoothingBlockDefinition

				{ 0x4E0, new C { T = "Float" } },

				{ 0x4E4, new C { T="TagRef" } },

				{ 0x500, new C{ T="Tagblock"}}, //i343::SpartanTracking::ObjectDefinition

				{ 0x514, new C { T="TagRef" } },

				{ 0x530, new C{ T="Tagblock"}}, //InteractionOpportunityDefinition
				{ 0x548, new C{ T="Tagblock"}}, //ScriptedSequenceActionDefinition

				{ 0x55C, new C { T="TagRef" } },

				{ 0x578, new C{ T="Tagblock"}}, //AnimChannelEntry
				{ 0x58C, new C{ T="Tagblock"}}, //AnimSetTableEntry

				{ 0x5A0, new C { T = "Float" } },
				{ 0x5A4, new C { T = "Float" } },
				{ 0x5A8, new C { T = "Float" } },
				{ 0x5AC, new C { T = "Float" } },
				{ 0x5B0, new C { T = "Float" } },
				{ 0x5B4, new C { T = "Float" } },
				{ 0x5B8, new C { T = "Float" } },
				{ 0x5BC, new C { T = "Float" } },
				{ 0x5C0, new C { T = "Float" } },

				{ 0x5C4, new C{ T="Tagblock"}}, //AnimationDefinition.Anim Set Nodegraph Metadata

				{ 0x5E4, new C { T = "Float" } },

				{ 0x5E8, new C{ T="Tagblock"}}, //LegGroundingSettings

				{ 0x5FC, new C { T = "Float" } },
				{ 0x600, new C { T = "Float" } },
				{ 0x604, new C { T = "Float" } },
				{ 0x608, new C { T = "Float" } },

				{ 0x60C, new C{ T="Tagblock"}}, //i343::Objects::ObjectNodeGraphDefinition
				{ 0x620, new C{ T="Tagblock"}}, //i343::Objects::AnimationMatchingTableEntry
				{ 0x638, new C{ T="Tagblock"}}, //i343::Objects::ModelVariantSwappingTableEntry
				{ 0x650, new C{ T="Tagblock"}}, //i343::Items::LocationSensorDefinition
				{ 0x664, new C{ T="Tagblock"}}, //i343::Items::ShroudGeneratorDefinition
				{ 0x678, new C{ T="Tagblock"}}, //i343::Objects::PowerComponentDefinition
				{ 0x68C, new C{ T="Tagblock"}}, //i343::Objects::SelfDestructHandlerDefinition
				{ 0x6A0, new C{ T="Tagblock"}}, //i343::Objects::IndirectLightingComponentDefinition

				{ 0x6BC, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x730, new C{ T="Tagblock"}}, //i343::Items::VariantHUDMessages
				{ 0x744, new C{ T="Tagblock"}}, //PredictedBitmapsBlock

				{ 0x758, new C { T="TagRef", N = "Damage Effect" } },
				{ 0x77C, new C { T="TagRef", N = "Effect" } },
				{ 0x798, new C { T="TagRef" } },

				{ 0x7B4, new C { T = "Float" } },
				{ 0x7B8, new C { T = "Float" } },
				{ 0x7BC, new C { T = "Float" } },
				{ 0x7C0, new C { T = "Float" } },
				{ 0x7C4, new C { T = "Float" } },
				{ 0x7C8, new C { T = "Float" } },
				{ 0x7CC, new C { T = "Float" } },

				{ 0x7D8, new C { T="TagRef", N = "Grounded Friction" } },
				{ 0x7F4, new C { T="TagRef" } },
				{ 0x850, new C { T="TagRef" } },

				{ 0x86C, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x874, new C { T = "Float" } },
				{ 0x878, new C { T = "Float" } },
				{ 0x87C, new C { T = "Float" } },
				{ 0x880, new C { T = "Float" } },
				{ 0x884, new C { T = "Float" } },
				{ 0x888, new C { T = "Float" } },

				{ 0x88C, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
				} } },

				{ 0x894, new C { T = "Float" } },
				{ 0x898, new C { T = "Float" } },
				{ 0x89C, new C { T = "Float" } },
				{ 0x8A0, new C { T = "Float" } },
				{ 0x8A4, new C { T = "Float" } },
				{ 0x8AC, new C { T = "Float" } },

				{ 0x8B0, new C{ T="Tagblock"}}, //OptionalUnitCameraBlock
				{ 0x8C4, new C{ T="Tagblock", B= new Dictionary<long, C>
				{
					{ 0x0, new C { T = "Float" } },
					{ 0x4, new C { T = "Float" } },
					{ 0x8, new C { T = "Float" } },
					{ 0xC, new C { T = "Float" } },
					{ 0x10, new C { T = "Float" } },
					{ 0x14, new C { T = "Float" } },
					{ 0x18, new C { T = "Float" } },
					{ 0x1C, new C { T = "Float", N = "Activation Cost" } },
					{ 0x20, new C { T = "Float" } },
					{ 0x24, new C { T = "Float" } },
					{ 0x28, new C { T = "Float" } },
					{ 0x2C, new C { T = "Float" } },

					{ 0x30, new C{ T="Tagblock"}}, //function_definition_data

					{ 0x44, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
					} } },

					{ 0x48, new C { T = "Float" } },

					{ 0x4C, new C{ T="Tagblock"}}, //EquipmentAbilityDeactivationOverrideSettings
					
					{ 0x50, new C { T = "Float" } },
					{ 0x54, new C { T = "Float" } },

					{ 0x58, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
					} } },

					{ 0x5C, new C { T = "Float" } },
					{ 0x60, new C { T = "Float" } },
					{ 0x64, new C { T = "Float" } },
					{ 0x68, new C { T = "Float" } },

					{ 0x6C, new C{ T="Tagblock"}}, //ControlStateUIInfo
					
					{ 0x7C, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
					} } },

					{ 0x88, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
							{ 0x0, new C { T = "Float", N = "Spawn X" } },
							{ 0x4, new C { T = "Float", N = "Spawn Y" } },
							{ 0x8, new C { T = "Float", N = "Spawn Z" } },
							{ 0xC, new C { T = "Float" } },
							{ 0x10, new C { T = "Float" } },
							{ 0x14, new C { T = "Float" } },
							{ 0x18, new C { T = "Float" } },
							{ 0x1C, new C { T = "Float", N = "Spawn Velocity" } },
							{ 0x20, new C { T = "Float" } },
							{ 0x24, new C { T = "Float" } },
							{ 0x28, new C { T = "Float" } },
							{ 0x2C, new C { T = "Float" } },
							{ 0x30, new C { T = "Float" } },

							{ 0x34, new C { T="TagRef" } },
							{ 0x64, new C { T="TagRef", N = "Projectile" } },
							{ 0x80, new C { T="TagRef" } },
							{ 0xB0, new C { T="TagRef", N = "Bitmap" } },
							{ 0xD0, new C { T="TagRef", N = "Bitmap" } },
							{ 0xF4, new C { T="TagRef" } },

					} } },//EquipmentAbilityTypeSpawner


					{ 0x9C, new C{ T="Tagblock"}}, //EquipmentAbilityTypeProximityMine
					{ 0xB0, new C{ T="Tagblock"}}, //EquipmentAbilityTypeMotionTrackerNoise
					{ 0xC4, new C{ T="Tagblock"}}, //EquipmentAbilityTypeTreeOfLife
					{ 0xD8, new C{ T="Tagblock"}}, //EquipmentAbilityTypeRepulsorField
					{ 0xEC, new C{ T="Tagblock"}}, //EquipmentAbilityTypeDaddy
					{ 0x100, new C{ T="Tagblock"}}, //EquipmentAbilityTypeAmmoPack
					
					{ 0x114, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0xC, new C{ T="Tagblock", B= new Dictionary<long, C>
						{
							{ 0x0, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
							{
								{ 0, "Unknown Function"  },
							} } },

							{ 0x08, new C { T = "Float" } },
							{ 0x0C, new C { T = "Float" } },
							{ 0x10, new C { T = "Float" } },
							{ 0x14, new C { T = "Float" } },
							{ 0x18, new C { T = "Float" } },
							{ 0x1C, new C { T = "Float" } },

							{ 0x20, new C { T="TagRef" } },
							{ 0x3C, new C { T="TagRef", N = "Generic Modifier Definition Tag" } },
						} } },

						{ 0x20, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
						{
							{ 0, "Unknown Function"  },
						} } },
					} } }, //EquipmentAbilityTypeHealthPack
					
					{ 0x128, new C{ T="Tagblock"}}, //EquipmentAbilityTypeJetPack
					{ 0x13C, new C{ T="Tagblock"}}, //EquipmentAbilityTypeHologram
					{ 0x150, new C{ T="Tagblock"}}, //EquipmentAbilityTypeSpecialWeapon
					{ 0x164, new C{ T="Tagblock"}}, //EquipmentAbilityTypeSpecialMove
					{ 0x178, new C{ T="Tagblock"}}, //EquipmentAbilityTypeEngineerShields
					{ 0x18C, new C{ T="Tagblock"}}, //EquipmentAbilityTypeAutoTurret
					{ 0x1A0, new C{ T="Tagblock"}}, //EquipmentAbilityTypeShieldProjector
					{ 0x1B4, new C{ T="Tagblock"}}, //EquipmentAbilityTypeProjectileCollector
					{ 0x1C8, new C{ T="Tagblock"}}, //EquipmentAbilityTypeActiveShield
					{ 0x1DC, new C{ T="Tagblock"}}, //EquipmentAbilityTypeFortificationShield
					{ 0x1F0, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0x0, new C { T="TagRef" } },
						{ 0x1C, new C { T="TagRef", N = "Generic Modifier Definition Tag" } },
					} } }, //EquipmentAbilityTypeGenericPowerup
					
					{ 0x204, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0x0, new C { T="TagRef", N = "Generic Modifier Definition Tag" } },
					} } }, //EquipmentAbilityTypeMalleableProperties
					
					{ 0x218, new C{ T="Tagblock", B= new Dictionary<long, C>
					{
						{ 0x0, new C { T="TagRef", N = "Spartan Tracking Ping Definition" } },
						{ 0x1C, new C { T="TagRef" } },
					} } }, //EquipmentAbilityTypeSpartanCustomTracking
					
					{ 0x22C, new C{ T="Tagblock"}}, //EquipmentAbilityTypeTeleporter
					{ 0x240, new C{ T="Tagblock"}}, //EquipmentAbilityTypeFrameAbilityItem
					{ 0x254, new C{ T="Tagblock"}}, //EquipmentAbilityTypeEquipmentRecharger
					{ 0x268, new C{ T="Tagblock"}}, //EquipmentAbilityTypeKnockback
					{ 0x27C, new C{ T="Tagblock"}}, //EquipmentAbilityTypeActiveCamo

					{ 0x290, new C { T="TagRef" } },
					{ 0x2AC, new C { T="TagRef", N = "Effect" } },
					{ 0x2C8, new C { T="TagRef", N = "Effect" } },
					{ 0x2E4, new C { T="TagRef" } },

					{ 0x330, new C { T = "Float" } },
					{ 0x334, new C { T = "Float" } },


				} } }, //EquipmentAbility

				{ 0x8D8, new C { T="TagRef" } },

				{ 0x8F4, new C{ T="Tagblock"}}, //Interface::UIItemInfo

				{ 0x908, new C { T="TagRef", N = "Sound" } },
				{ 0x924, new C { T="TagRef" } },
				{ 0x940, new C { T="TagRef" } },
				{ 0x95C, new C { T="TagRef" } },

				{ 0x978, new C{ T="Tagblock"}}, //SoundRTPCBlockDefinition
				{ 0x98C, new C{ T="Tagblock"}}, //SoundSweetenerBlockDefinition
				{ 0x9A0, new C{ T="Tagblock"}}, //i343::Equipment::ObjectFunctionRemapping
			}},

			{"bloc",new()
			{
				{ 0x10, new FlagGroup {A = 4, STR = new Dictionary<int, string>()

					{
							{ 0, "Does Not Cast Shadow." },
				} } },

				{ 0x78, new C { T = "TagRef", N = "Model" } },
				{ 0x13C, new C { T = "TagRef", N = "Material Effects" } },

				{ 0x248, new C { T = "Tagblock", B= new Dictionary<long, C>
				{
					{ 0x04, new C { T = "TagRef" } },
					{ 0x20, new C { T = "TagRef" } },
				} } }, // Object_attachment_definition
			}},

			{"scen",new()
			{
				{ 0x10, new FlagGroup {A = 4, STR = new Dictionary<int, string>()

					{
							{ 0, "Does Not Cast Shadow." },
				} } },

				{ 0x78, new C { T = "TagRef", N = "Model" } },
				{ 0x13C, new C { T = "TagRef", N = "Material Effects" } },

				{ 0x248, new C { T = "Tagblock", B= new Dictionary<long, C>
				{
					{ 0x04, new C { T = "TagRef" } },
					{ 0x20, new C { T = "TagRef" } },
				} } }, // Object_attachment_definition
			}},

			{"bost",new()
			{
				{ 0x10, new C { T = "TagRef", N = "Collision Damage" } },

				{ 0x2C, new FlagGroup {A = 4, STR = new Dictionary<int, string>()

					{
							{ 0, "Constant boost while active" }, // Don't know what the actual name is, but this is what it does
				} } },
				{ 0x30, new C { T = "Float", N = "Boost Speed" } }, //Doesn't seem to work for the banshee, works on ghost though.
				{ 0x34, new C { T = "Float" } },
				{ 0x38, new C { T = "Float" } },
				{ 0x3C, new C { T = "Float", N = "Boost Cost" } }, //Works on banshee and ghost.			
				{ 0x40, new C { T = "Float" } },
				{ 0x44, new C { T = "Float", N = "Boost Recharge Rate" } },
				{ 0x48, new C { T = "Float" } },
				{ 0x4dC, new C { T = "Float" } },
				{ 0x74, new C { T = "Float", N = "Power Scale" } },
				{ 0x78, new C { T = "Float" } },
				{ 0x7C, new C { T = "Float" } },
				{ 0x80, new C { T = "Float" } },
				{ 0x84, new C { T = "Float" } },
				{ 0x88, new C { T = "Float" } },
				{ 0x8C, new C { T = "Float" } },
			}},

			{"saev",new()
			{
				{ 0x18, new C { T = "Float", N = "Recharge Cost" } },
				{ 0x20, new C { T = "Float", N = "Recharge Duration" } },
				{ 0x2C, new C { T = "TagRef" } },
				{ 0xA0, new C { T = "Float" } },
				{ 0x198, new C { T = "Float", N = "Thrust Power" } },
			}},

			{"sasp",new()
			{
				{ 0x14, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Third Person on Activation" },
				} } },

				{ 0x2C, new C { T = "TagRef" } },
				{ 0x50, new C { T = "TagRef" } },

				{ 0xC8, new C { T = "Float", N = "Sprint Speed" } },
				{ 0xD0, new C { T = "Float", N = "Sprint Acceleration" } },
			}},

			{"gmpm",new() // Extremely small tag, but contains modifiers for abilities.
			{
				{ 0x10, new C { T = "Tagblock", B= new Dictionary<long, C>
				{
					{ 0x8, new C { T = "Float", N = "Modifier" } }, // Only value that seems to do anything.
				} } },
			}},

			{"valu",new() // Mapped some values, Not useful rn. 
			{
				{ 0x70, new C { T = "Float"  } },
				{ 0x74, new C { T = "TagRef" } },
				{ 0xA0, new C { T = "Float"  } },
				{ 0xC8, new C { T = "Float"  } },

				{ 0x10, new C { T = "Tagblock", B= new Dictionary<long, C> // no idea if these change anything useful
				{
				{ 0x10, new C { T = "Float"  } },
				{ 0x14, new C { T = "TagRef" } },
				{ 0x40, new C { T = "Float"  } },
				{ 0x4C, new C { T = "TagRef" } },
				{ 0x68, new C { T = "Float"  } },
				{ 0x78, new C { T = "Float"  } },
				{ 0xE0, new C { T = "Float"  } },
				{ 0x148, new C { T = "Float"  } },
				{ 0x1B0, new C { T = "Float"  } },
				{ 0x218, new C { T = "Float"  } },
				{ 0x280, new C { T = "Float"  } },
				{ 0x2DC, new C { T = "Float"  } },
				{ 0x2E8, new C { T = "Float"  } },
				{ 0x350, new C { T = "Float"  } },
				{ 0x3A8, new C { T = "Float"  } },
				{ 0x3B8, new C { T = "Float"  } },
				{ 0x420, new C { T = "Float"  } },
				{ 0x488, new C { T = "Float"  } },
				{ 0x4E0, new C { T = "Float"  } },
				{ 0x4f0, new C { T = "Float"  } },
				{ 0x558, new C { T = "Float"  } },
				{ 0x5C0, new C { T = "Float"  } },
				{ 0x618, new C { T = "Float"  } },
				{ 0x6EC, new C { T = "Float"  } },

				{ 0xEB0, new C { T = "Tagblock", B= new Dictionary<long, C>
				{
				{ 0xF9C, new C { T = "TagRef" } }, // material

				} } } } } },
				{ 0x90, new C { T = "TagRef" } },
				{ 0xAC, new C { T = "TagRef" } },

			}},


			{"vcdd",new() // Vehicle Configuration
			{
				{ 0x10, new C { T = "TagRef" } },      // Vehi
				{ 0x34, new C { T = "TagRef" } },      // Sofd
				{ 0x70, new C { T = "Tagblock"} },     // i343::Vehicle::WeaponConfigurationGroup
				{ 0xA8, new C { T = "Float" } },       // No clue What this does
				{ 0xE8, new C { T = "TagRef" } },      // Sbnk


			}},

			{"weat",new() // Weapon Attachment
			{
				{ 0x18, new C { T = "Tagblock"} },     // i343::Weapon::AttachmentRegionPermutationDefinition
				{ 0x58, new C { T = "Tagblock"} },     // i343::Weapon::AccuracyPropertyModifiersDefinition
				{ 0x80, new C { T = "Tagblock"} },	   // i343::Weapon::UnitPropertyModifiersDefinition
				{ 0xD8, new C { T = "Tagblock"} },     // i343::Weapon::RecoilGunkickPropertyModifiersDefinition
				{ 0x100, new C { T = "Tagblock"} },    // i343::Weapon::HeatPropertyModifiersDefinition
				{ 0x128, new C { T = "TagRef"} },	   // Bloc
				{ 0x14C, new C { T = "Float" } },       // No clue What this does
				{ 0x150, new C { T = "Float" } },       // No clue What this does
				{ 0x164, new C { T = "Float" } },       // No clue What this does
				{ 0x168, new C { T = "Float" } },       // No clue What this does
				{ 0x16C, new C { T = "Float" } },       // No clue What this does
				{ 0x170, new C { T = "Float" } },       // No clue What this does

			}},

			{"bitm",new() // Bitmap Mapped only a few things
			{
				{ 0x58, new C { T = "Tagblock"} },      // BitmapGroupSequence1
				{ 0xC0, new C { T = "Tagblock"} },      // BitmapGroupSequence1
				{ 0xF8, new C { T = "Tagblock"} },      // BitmapDataResource
				{ 0x110, new C { T = "Tagblock"} },     // BitmapDataResource.pixels
				{ 0x140, new C { T = "Tagblock"} },     // StreamingBitmapData
			
			}},

			{"ant!",new() // Antenna Mapped what I could find 
			{
				{ 0x14, new C { T = "TagRef"} },	   // Bitm
				{ 0x30, new C { T = "TagRef"} },	   // Pphy
				{ 0x4C, new C { T = "Float" } },       // No clue What this does
				{ 0x50, new C { T = "Float" } },       // No clue What this does
				{ 0x54, new C { T = "Float" } },       // No clue What this does
				{ 0x58, new C { T = "Float" } },       // No clue What this does
				{ 0x5C, new C { T = "Float" } },       // No clue What this does
				{ 0x60, new C { T = "Float" } },       // No clue What this does
				{ 0x64, new C { T = "Float" } },       // No clue What this does
				{ 0x68, new C { T = "Float" } },       // No clue What this does
			}},

			{"uihg",new() // User interface Hud Globals some stuff 
			{
				{ 0xA8, new C { T = "Tagblock"} },	   // NavPointPresentationTagBlock
				{ 0xC8, new C { T = "TagRef"} },	   // bitm
				{ 0x13C, new C { T = "TagRef"} },
				{ 0x16C, new C { T = "TagRef"} },
				{ 0x198, new C { T = "TagRef"} },
			}},

			{"cfxs",new() // Camera_fx_settings some stuff here
			{
				{ 0x18, new C { T = "Float" } },       // No clue What this does
				{ 0x1C, new C { T = "Float" } },       // No clue What this does
				{ 0x34, new C { T = "Float" } },       // No clue What this does
				{ 0x3C, new C { T = "Float" } },       // No clue What this does
				{ 0x60, new C { T = "TagRef"} },
				{ 0x68, new C { T = "TagRef"} },
				{ 0x130, new C { T = "TagRef"} },	   // TagRef EFEX
				{ 0x138, new C { T = "TagRef"} },
				{ 0x150, new C { T = "TagRef"} },	   // TagRef Bitm
				{ 0x16C, new C { T = "TagRef"} },	   //
				{ 0x278, new C { T = "TagRef"} },	   //
				


			}},

			{"mwsy",new()
			{
				{ 0x10, new C { T = "Tagblock" } }, // MaterialRegion
				
				{ 0x20, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Unknown Function" }, // Looks like a flag block. That or a combo box.
				} } },

				{ 0x30, new C { T = "Tagblock",B = new Dictionary<long, C> // CoatingMaterialSetReference
				{
					{ 0x0, new C { T = "TagRef", N = "Coating Material Set" } },
				}, S=28 } },

				{ 0x40, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Unknown Function" }, //Most likely a combo box
				} } },

				{ 0x50, new C { T = "Tagblock",B = new Dictionary<long, C> //MaterialStyle
				{
					{ 0x0, new C { T = "mmr3Hash", N = "Coating Hash" } },
					{ 0x4, new C { T = "TagRef", N = "Material Palette" } },

					{ 0x1C, new C { T = "mmr3Hash", N = "MaterialStyle.palette" } },

					{ 0x20, new C { T = "Float", N = "Global Damage" } },
					{ 0x24, new C { T = "Float", N = "Hero Damage" } },
					{ 0x28, new C { T = "Float", N = "Global Emissive" } },
					{ 0x2C, new C { T = "Float", N = "Emissive Amount" } },
					{ 0x30, new C { T = "Float", N = "Scratch Amount" } },
					{ 0x34, new C { T = "Float", N = "Grime Type" } },
					{ 0x38, new C { T = "Float", N = "Grime Amount" } },

					{ 0x3C, new C { T = "Tagblock" } }, // MaterialStyleRegion

					{ 0x4C, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
					{
						{ 0, "Region Flags - Unknown Function" },
					} } },

				}, S=92 } },

				{ 0x60, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Unknown Function" },
				} } },

				{ 0x74, new C { T = "TagRef", N = "Material Visor Swatch" } },
				
				{ 0x94, new C { T = "Tagblock",B = new Dictionary<long, C> // PreloadSwatchEntry
				{
					{ 0x0, new C { T = "TagRef", N = "Material Swatch" } },
				}, S=28 } },

				{ 0xA4, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Unknown Function" },
				} } },
			}},

			{"mwpl",new() // This seems useless in my opinion
			{
				{ 0x10, new C { T = "Tagblock",B = new Dictionary<long, C> // MaterialSwatchEntry
				{
					{ 0x4, new C { T = "TagRef" } },
				}, S=56 } },

				{ 0x20, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Combo box type" },
				} } },
			}},
			
			{"mwsw",new()
			{
				{ 0x10, new C { T = "Float" } },
				{ 0x14, new C { T = "Float" } },
				{ 0x18, new C { T = "Float" } },
				{ 0x1C, new C { T = "Float" } },

				{ 0x20, new C { T = "TagRef" } },

				{ 0x3C, new C { T = "Float" } },
				{ 0x40, new C { T = "Float" } },

				{ 0x44, new C { T = "TagRef" } },

				{ 0x60, new C { T = "Float" } },
				{ 0x64, new C { T = "Float" } },
				{ 0x68, new C { T = "Float" } },
				{ 0x6C, new C { T = "Float" } },
				{ 0x70, new C { T = "Float" } },
				{ 0x74, new C { T = "Float" } },
				{ 0x78, new C { T = "Float" } },
				{ 0x7C, new C { T = "Float" } },
				{ 0x80, new C { T = "Float" } },
				{ 0x84, new C { T = "Float" } },
				{ 0x88, new C { T = "Float" } },
				{ 0x8C, new C { T = "Float" } },
				{ 0x90, new C { T = "Float" } },

				{ 0x94, new C { T = "Tagblock",B = new Dictionary<long, C> // MaterialColorVariants
				{
					{ 0x0, new C { T = "mmr3Hash" } },

					{ 0x4, new C { T = "Float" } },
					{ 0x8, new C { T = "Float" } },
					{ 0xC, new C { T = "Float" } },
					{ 0x10, new C { T = "Float" } },
					{ 0x14, new C { T = "Float" } },
					{ 0x18, new C { T = "Float" } },
					{ 0x1C, new C { T = "Float" } },
					{ 0x20, new C { T = "Float" } },
					{ 0x24, new C { T = "Float" } },
					{ 0x28, new C { T = "Float" } },
					{ 0x2C, new C { T = "Float" } },

				}, S=48 } },

				{ 0xA4, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Combo box type" },
				} } },
			}},
			
			{"mwvs",new()
			{
				{ 0x10, new C { T = "Tagblock",B = new Dictionary<long, C> // MaterialVisorPatternReference
				{
					{ 0x0, new C { T = "mmr3Hash" } },

					{ 0x4, new C { T = "TagRef" } },
				}, S=32 } },

				{ 0x24, new C { T = "Tagblock",B = new Dictionary<long, C> // MaterialColorVariants
				{
					{ 0x0, new C { T = "mmr3Hash" } },

					{ 0x4, new C { T = "Float" } },
					{ 0x8, new C { T = "Float" } },
					{ 0xC, new C { T = "Float" } },
					{ 0x10, new C { T = "Float" } },
					{ 0x14, new C { T = "Float" } },
					{ 0x18, new C { T = "Float" } },
					{ 0x1C, new C { T = "Float" } },
					{ 0x20, new C { T = "Float" } },
					{ 0x24, new C { T = "Float" } },
					{ 0x28, new C { T = "Float" } },
					{ 0x2C, new C { T = "Float" } },

				}, S=48 } },

				{ 0x30, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Unknown Function" },
				} } },

				{ 0x34, new FlagGroup {A = 4, STR = new Dictionary<int, string>()
				{
					{ 0, "Unknown Function" },
				} } },
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
