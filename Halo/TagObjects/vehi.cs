using System.Collections.Generic;

namespace Assembly69.Halo.TagObjects
{
    public class Vehi
    {
        public struct C
        {
            public string T { get; set; }
            public Dictionary<long, C> B { get; set; }

            public long S { get; set; } // length of tagblock
        }

        public static Dictionary<long, C> VehicleTag = new Dictionary<long, C>
        {
            { 0, new C{ T="Pointer"}},
            { 8, new C{ T="4Byte"}}, // datnum
            { 12, new C{ T="4Byte"}}, // tagID

            { 40, new C{ T="Float"}},
            { 44, new C{ T="Float"}},
            { 48, new C{ T="Float"}},
            { 52, new C{ T="Float"}},
            { 56, new C{ T="Float"}},
            { 60, new C{ T="Float"}},
            { 64, new C{ T="Float"}},
            { 76, new C{ T="Float"}},
            { 80, new C{ T="Float"}},
            { 84, new C{ T="Float"}},

            { 92, new C{ T="Tagblock"}}, // SidecarPathDefinition

            { 112, new C{ T="Float"}},
            { 116, new C{ T="Float"}},

            { 120, new C{ T="TagRef"}}, // vehicle model
            { 148, new C{ T="TagRef"}}, // aset tag ref
            { 176, new C{ T="TagRef"}},
            { 204, new C{ T="TagRef"}},

            { 232, new C{ T="4Byte"}},

            { 240, new C{ T="TagRef"}},

            { 276, new C{ T="Float"}},

            { 288, new C{ T="TagRef"}},
            { 316, new C{ T="TagRef"}}, // foot tag ref
            { 344, new C{ T="TagRef"}}, // vemd tag ref
            { 372, new C{ T="TagRef"}}, // smed tag ref
            { 400, new C{ T="TagRef"}},

            { 432, new C{ T="Float"}},

            { 448, new C{ T="Tagblock"}}, // object_ai_properties
            { 468, new C{ T="Tagblock"}}, // s_object_function_definition
            { 488, new C{ T="4Byte"}},
            { 492, new C{ T="Tagblock"}}, // ObjectRuntimeInterpolatorFunctionsBlock
            { 512, new C{ T="Tagblock"}}, // ObjectFunctionSwitchDefinition
            { 532, new C{ T="Tagblock"}}, // i343::Objects::ObjectFunctionForwarding
            { 552, new C{ T="4Byte"}},
            { 556, new C{ T="Tagblock"}}, // i343::Objects::AmmoRefillVariant
            { 576, new C{ T="4Byte"}},
            { 0x248, new C{ T="Tagblock", B=new Dictionary<long, C> // object_attachment_definition
            {
                { 4, new C{ T="TagRef"}}, // effe
                { 32, new C{ T="TagRef"}}, // effe
                { 64, new C{ T="Tagblock"}},
                { 84, new C{ T="TagRef"}}, //
                { 112, new C{ T="Tagblock"}}
            }, S=148}},
            { 604, new C{ T="Tagblock"}}, // object_indirect_lighting_settings_definition
            { 624, new C{ T="Tagblock"}}, // s_water_physics_hull_surface_definition
            { 644, new C{ T="Tagblock"}}, // s_jetwash_definition
            { 664, new C{ T="Tagblock"}}, // object_definition_widget
            { 684, new C{ T="Tagblock"}}, // object_change_color_definition
            { 704, new C{ T="Tagblock"}}, // s_multiplayer_object_properties_definition
            { 724, new C{ T="Tagblock"}}, // i343::Objects::ForgeObjectEntryDefinition

            { 744, new C{ T="TagRef"}},
            { 772, new C{ T="TagRef"}},

            { 800, new C{ T="Tagblock"}}, // s_object_spawn_effects
            { 820, new C{ T="Tagblock"}}, // ModelDissolveDataBlock

            { 840, new C{ T="String"}}, // (vehicle_partial_emp)

            { 0x448, new C{ T="Tagblock"}}, // HsReferencesBlock
            { 0x45C, new C{ T="TagRef"}},

            { 0x480, new C{ T="Float"}},

            { 0x484, new C{ T="Tagblock"}}, // metalabel somethiong ?? recheck this one
            { 0x498, new C{ T="Tagblock"}}, // SoundRTPCBlockDefinition
            { 0x4AC, new C{ T="Tagblock"}}, // SoundSweetenerBlockDefinition

            { 0x4C0, new C{ T="Pointer"}},

            { 0x4C8, new C{ T="Tagblock"}}, // i343::Objects::ComputeFunctionSmoothingBlockDefinition

            { 0x4DC, new C{ T="Float"}},
            { 0x4E0, new C{ T="Float"}},

            { 0x4E4, new C{ T="TagRef"}},
            { 0x500, new C{ T="Tagblock"}}, // i343::SpartanTracking::ObjectDefinition

            { 0x514, new C{ T="TagRef"}},
            { 0x530, new C{ T="Tagblock"}}, // InteractionOpportunityDefinition

            { 0x544, new C{ T="Float"}},
            { 0x548, new C{ T="Tagblock"}}, // ScriptedSequenceActionDefinition
            { 0x55C, new C{ T="TagRef"}},
            { 0x578, new C{ T="Tagblock"}}, // AnimChannelEntry
            { 0x58C, new C{ T="Tagblock"}}, // AnimSetTableEntry

            { 0x5A0, new C{ T="Float"}},
            { 0x5A4, new C{ T="Float"}},
            { 0x5A8, new C{ T="Float"}},
            { 0x5AC, new C{ T="Float"}},
            { 0x5B0, new C{ T="Float"}},
            { 0x5B4, new C{ T="Float"}},
            { 0x5B8, new C{ T="Float"}},
            { 0x5BC, new C{ T="Float"}},
            { 0x5C0, new C{ T="Float"}},
            { 0x5CC, new C{ T="4Byte"}},
            { 0x5DC, new C{ T="Float"}},
            { 0x5E0, new C{ T="Float"}},
            { 0x5E4, new C{ T="Float"}},

            { 0x5E8, new C{ T="Tagblock"}}, // LegGroundingSettings

            { 0x5FC, new C{ T="Float"}},
            { 0x600, new C{ T="Float"}},
            { 0x604, new C{ T="Float"}},
            { 0x608, new C{ T="Float"}},

            { 0x60C, new C{ T="Tagblock"}}, // i343::Objects::ObjectNodeGraphDefinition
            { 0x620, new C{ T="Tagblock"}}, // i343::Objects::AnimationMatchingTableEntry

            { 0x634, new C{ T="Float"}},

            { 0x638, new C{ T="Tagblock"}}, // i343::Objects::ModelVariantSwappingTableEntry

            { 0x63C, new C{ T="Float"}},

            { 0x650, new C{ T="Tagblock"}}, // i343::Items::LocationSensorDefinition
            { 0x664, new C{ T="Tagblock"}}, // i343::Items::ShroudGeneratorDefinition
            { 0x678, new C{ T="Tagblock"}}, // i343::Objects::PowerComponentDefinition
            { 0x68C, new C{ T="Tagblock"}}, // i343::Objects::SelfDestructHandlerDefinition
            { 0x6A0, new C{ T="Tagblock"}}, // i343::Objects::IndirectLightingComponentDefinition

            { 0x6B4, new C{ T="Float"}},
            { 0x6B8, new C{ T="4Byte"}},
            { 0x6BC, new C{ T="4Byte"}},

            { 0x6C4, new C{ T="TagRef"}},
            { 0x6E0, new C{ T="Tagblock"}}, // s_campaign_metagame_bucket
            { 0x6F4, new C{ T="Tagblock"}}, // s_unit_screen_effect_definition

            { 0x70C, new C{ T="Float"}},
            { 0x720, new C{ T="Tagblock"}}, // s_unit_camera_track
            { 0x768, new C{ T="Tagblock"}}, // s_unit_camera_acceleration
            { 0x77C, new C{ T="Float"}},
            { 0x790, new C{ T="Tagblock"}}, // s_unit_camera_track
            { 0x7D8, new C{ T="Tagblock"}}, // s_unit_camera_acceleration

            { 0x7F0, new C{ T="TagRef"}},

            { 0x818, new C{ T="Float"}},
            { 0x81C, new C{ T="Float"}},
            { 0x830, new C{ T="Float"}},

            { 0x870, new C{ T="TagRef"}},

            { 0x898, new C{ T="Float"}},

            { 0x8BC, new C{ T="Tagblock"}}, // WeaponSpecificMarkers
            { 0x8D0, new C{ T="TagRef"}},
            { 0x908, new C{ T="TagRef"}},
            { 0x924, new C{ T="TagRef"}},
            { 0x940, new C{ T="TagRef"}},
            { 0x95C, new C{ T="TagRef"}},
            { 0x978, new C{ T="TagRef"}},
            { 0x994, new C{ T="TagRef"}},
            { 0x9B0, new C{ T="TagRef"}},

            { 0x9D0, new C{ T="Float"}},

            { 0x9E0, new C{ T="Tagblock"}}, // HudUnitSoundDefinitions
            { 0x9F4, new C{ T="Tagblock"}}, // dialogue_variant_definition

            { 0xA30, new C{ T="Float"}},
            { 0xA34, new C{ T="Float"}},
            { 0xA38, new C{ T="Float"}},

            { 0xA64, new C{ T="Tagblock"}}, // powered_seat_definition
            { 0xA78, new C{ T="Tagblock", B=new Dictionary<long, C> // unit_initial_weapon
            {
                { 0, new C{ T="TagRef"}}, // weap
                { 40, new C{ T="Float"}},
                { 48, new C{ T="Float"}},
                { 148, new C{ T="TagRef"}}, //
                { 188, new C{ T="Tagblock"}}
            }, S=212}},
            { 0xA8C, new C{ T="Tagblock"}}, // s_target_tracking_parameters
            { 0xAA0, new C{ T="Tagblock"}}, // unit_seat

            { 0xAB4, new C{ T="Float"}},
            { 0xAB8, new C{ T="Float"}},
            { 0xABC, new C{ T="Float"}},

            { 0xAC8, new C{ T="TagRef"}},
            { 0xAE4, new C{ T="Tagblock"}}, // i343::Objects::PowerComponentDefinition
            { 0xAF8, new C{ T="TagRef"}},
            { 0xB14, new C{ T="TagRef"}},

            { 0xB38, new C{ T="Float"}},
            { 0xB3C, new C{ T="Float"}},
            { 0xB40, new C{ T="Float"}},
            { 0xB44, new C{ T="Float"}},
            { 0xB48, new C{ T="Float"}},
            { 0xB4C, new C{ T="Float"}},

            { 0xB54, new C{ T="Pointer"}},
            { 0xB5C, new C{ T="Pointer"}},

            { 0xB7C, new C{ T="TagRef"}},
            { 0xB98, new C{ T="TagRef"}},

            { 0xBB4, new C{ T="Tagblock"}}, // ExitAndDetachVariant

            { 0xBCC, new C{ T="Float"}},

            { 0xBE4, new C{ T="TagRef"}},

            { 0xC00, new C{ T="Float"}},
            { 0xC04, new C{ T="Float"}},
            { 0xC08, new C{ T="Float"}},
            { 0xC0C, new C{ T="Float"}},
            { 0xC10, new C{ T="Float"}},
            { 0xC18, new C{ T="Float"}},
            { 0xC1C, new C{ T="Float"}},
            { 0xC28, new C{ T="Float"}},
            { 0xC30, new C{ T="Float"}},
            { 0xC34, new C{ T="Float"}},
            { 0xC3C, new C{ T="Float"}},
            { 0xC40, new C{ T="Float"}},
            { 0xC5C, new C{ T="Float"}},
            { 0xC64, new C{ T="Float"}},
            { 0xC68, new C{ T="Float"}},
            { 0xC70, new C{ T="Float"}},
            { 0xC74, new C{ T="Float"}},
            { 0xC7C, new C{ T="Float"}},
            { 0xC80, new C{ T="Float"}},
            { 0xC84, new C{ T="Float"}},
            { 0xC88, new C{ T="Float"}},
            { 0xC8C, new C{ T="Float"}},
            { 0xC94, new C{ T="Float"}},
            { 0xC98, new C{ T="Float"}},
            { 0xCA0, new C{ T="Float"}},
            { 0xCAC, new C{ T="Float"}},
            { 0xCB8, new C{ T="Float"}},
            { 0xCBC, new C{ T="Float"}},
            { 0xCC4, new C{ T="Float"}},
            { 0xCC8, new C{ T="Float"}},
            { 0xCD0, new C{ T="Float"}},
            { 0xCD4, new C{ T="Float"}},
            { 0xCDC, new C{ T="Float"}},
            { 0xCE0, new C{ T="Float"}},
            { 0xCE8, new C{ T="Float"}},
            { 0xCF4, new C{ T="Float"}},
            { 0xCF8, new C{ T="Float"}},
            { 0xD00, new C{ T="Float"}},
            { 0xD04, new C{ T="Float"}},
            { 0xD0C, new C{ T="Float"}},
            { 0xD10, new C{ T="Float"}},
            { 0xD18, new C{ T="Float"}},
            { 0xD1C, new C{ T="Float"}},
            { 0xD24, new C{ T="Float"}},

            { 0xD28, new C{ T="TagRef"}},
            { 0xD44, new C{ T="TagRef"}}, // EFFECT TAG REF
            { 0xD64, new C{ T="TagRef"}},
            { 0xD80, new C{ T="TagRef"}}, // VEHI TAG REF

            { 0xDA0, new C{ T="Tagblock"}}, // s_vehicle_human_tank_definition
            { 0xDB4, new C{ T="Tagblock"}}, // s_vehicle_human_jeep_definition
            { 0xDC8, new C{ T="Tagblock"}}, // s_vehicle_human_plane_definition
            { 0xDDC, new C{ T="Tagblock"}}, // s_vehicle_alien_scout_definition
            { 0xDF0, new C{ T="Tagblock"}}, // s_vehicle_alien_fighter_definition
            { 0xE04, new C{ T="Tagblock"}}, // s_vehicle_turret_definition
            { 0xE18, new C{ T="Tagblock"}}, // s_vehicle_vtol_definition
            { 0xE2C, new C{ T="Tagblock"}}, // s_vehicle_chopper_definition
            { 0xE40, new C{ T="Tagblock"}}, // s_vehicle_guardian_definition
            { 0xE54, new C{ T="Tagblock"}}, // s_vehicle_jackal_glider_definition
            { 0xE68, new C{ T="Tagblock"}}, // s_vehicle_space_fighter_definition
            { 0xE7C, new C{ T="Tagblock"}}, // s_vehicle_revenant_definition

            { 0xE90, new C{ T="Float"}},
            { 0xE94, new C{ T="4Byte"}},

            { 0xE98, new C{ T="Tagblock"}}, // i343::Vehicles::AntiGravityPointConfiguration
            { 0xEAC, new C{ T="Tagblock"}}, // i343::Vehicles::AntiGravityPointDefinition
            { 0xEC0, new C{ T="Tagblock"}}, // i343::Vehicles::FrictionPointConfiguration
            { 0xED4, new C{ T="Tagblock"}}, // i343::Vehicles::FrictionPointDefinition

            { 0xEE8, new C{ T="Float"}},
            { 0xEEC, new C{ T="Float"}},

            { 0xEF0, new C{ T="Pointer"}},
            { 0xEF8, new C{ T="Pointer"}},
            { 0xF08, new C{ T="Pointer"}},
            { 0xF10, new C{ T="Pointer"}},

            { 0xF2C, new C{ T="Tagblock"}}, // s_unit_trick_definition
            { 0xF40, new C{ T="Float"}},
            { 0xF50, new C{ T="Float"}},
            { 0xF54, new C{ T="Float"}},
            { 0xF58, new C{ T="Float"}},
            { 0xF60, new C{ T="Float"}},
            { 0xF74, new C{ T="4Byte"}},

            { 0xF78, new C{ T="TagRef"}},
            { 0xF94, new C{ T="TagRef"}},
            { 0xFB0, new C{ T="TagRef"}},
            { 0xFD0, new C{ T="TagRef"}},
            { 0xFF0, new C{ T="TagRef"}},
            { 0x1024, new C{ T="Tagblock"}}, // SoundRTPCBlockDefinition
            { 0x1038, new C{ T="Tagblock"}}, // SoundSweetenerBlockDefinition

            { 0x104C, new C{ T="TagRef"}},
            { 0x1068, new C{ T="TagRef"}},
            { 0x1084, new C{ T="TagRef"}},

            { 0x10A4, new C{ T="Tagblock"}}, // s_vehicleAiCruiseControl
            { 0x10B8, new C{ T="Tagblock"}}, // Interface::UIItemInfo

            { 0x10CC, new C{ T="TagRef"}},
            { 0x10EC, new C{ T="Float"}},
        };

        public static Dictionary<long, C> WeaponTag = new Dictionary<long, C>
        {
            { 0, new C{ T="Pointer"}},
            { 8, new C{ T="4Byte"}}, // datnum
            { 12, new C{ T="4Byte"}}, // tagID

            { 0x05C, new C{ T="Tagblock"}},
            { 0x078, new C{ T="TagRef"}}, // HLMT
            { 0x094, new C{ T="TagRef"}},
            { 0x0B0, new C{ T="TagRef"}},
            { 0x0CC, new C{ T="TagRef"}},
            { 0x0F0, new C{ T="TagRef"}},

            { 0x10C, new C{ T="Tagblock"}},

            { 0x120, new C{ T="TagRef"}},
            { 0x13C, new C{ T="TagRef"}}, // FOOT
            { 0x158, new C{ T="TagRef"}}, // VMED
            { 0x174, new C{ T="TagRef"}}, // SMED
            { 0x190, new C{ T="TagRef"}},

            { 0x1C0, new C{ T="Tagblock"}},
            { 0x1D4, new C{ T="Tagblock"}},
            { 0x1EC, new C{ T="Tagblock"}},
            { 0x200, new C{ T="Tagblock"}},
            { 0x214, new C{ T="Tagblock"}},
            { 0x22C, new C{ T="Tagblock"}},
            { 0x248, new C{ T="Tagblock", B=new Dictionary<long, C> // attachment block
            {
                { 4, new C{ T="TagRef"}}, // effe
                { 32, new C{ T="TagRef"}}, // effe
                { 64, new C{ T="Tagblock"}},
                { 84, new C{ T="TagRef"}}, //
                { 112, new C{ T="Tagblock"}}
            }, S=148}},

            { 0x25C, new C{ T="Tagblock"}},
            { 0x270, new C{ T="Tagblock"}},
            { 0x284, new C{ T="Tagblock"}},
            { 0x298, new C{ T="Tagblock"}},
            { 0x2AC, new C{ T="Tagblock"}},
            { 0x2C0, new C{ T="Tagblock"}},
            { 0x2D4, new C{ T="Tagblock"}},

            { 0x2E8, new C{ T="TagRef"}},
            { 0x304, new C{ T="TagRef"}},

            { 0x320, new C{ T="Tagblock"}},
            { 0x334, new C{ T="Tagblock"}},
            { 0x448, new C{ T="Tagblock"}},

            { 0x45C, new C{ T="TagRef"}},

            { 0x484, new C{ T="Tagblock"}},
            { 0x498, new C{ T="Tagblock"}},
            { 0x4AC, new C{ T="Tagblock"}},
            { 0x4C8, new C{ T="Tagblock"}},

            { 0x4E4, new C{ T="TagRef"}},

            { 0x500, new C{ T="Tagblock"}},
            { 0x514, new C{ T="TagRef"}},

            { 0x530, new C{ T="Tagblock"}},
            { 0x548, new C{ T="Tagblock"}},

            { 0x55C, new C{ T="TagRef"}},

            { 0x578, new C{ T="Tagblock"}},
            { 0x58C, new C{ T="Tagblock"}},
            { 0x5E8, new C{ T="Tagblock"}},
            { 0x60C, new C{ T="Tagblock"}},
            { 0x620, new C{ T="Tagblock"}},
            { 0x638, new C{ T="Tagblock"}},
            { 0x650, new C{ T="Tagblock"}},
            { 0x664, new C{ T="Tagblock"}},
            { 0x678, new C{ T="Tagblock"}},
            { 0x68C, new C{ T="Tagblock"}},
            { 0x6A0, new C{ T="Tagblock"}},
            { 0x730, new C{ T="Tagblock"}},
            { 0x744, new C{ T="Tagblock"}},

            { 0x758, new C{ T="TagRef"}},
            { 0x77C, new C{ T="TagRef"}},
            { 0x798, new C{ T="TagRef"}},
            { 0x7D8, new C{ T="TagRef"}},
            { 0x7F4, new C{ T="TagRef"}},
            { 0x850, new C{ T="TagRef"}}, // WEAP
            { 0x88C, new C{ T="TagRef"}},
            { 0x8A8, new C{ T="TagRef"}},
            { 0x924, new C{ T="TagRef"}},
            { 0x940, new C{ T="TagRef"}},
            { 0x95C, new C{ T="TagRef"}},
            { 0x978, new C{ T="TagRef"}},

            { 0x994, new C{ T="Tagblock"}},

            { 0x9A8, new C{ T="TagRef"}},
            { 0x9D4, new C{ T="TagRef"}},

            { 0x9F0, new C{ T="Tagblock"}},
            { 0xA80, new C{ T="Tagblock"}},
            { 0xA94, new C{ T="Tagblock"}},

            { 0xAF0, new C{ T="TagRef"}},
            { 0xB0C, new C{ T="TagRef"}},

            { 0xB6C, new C{ T="Tagblock"}},
            { 0xB88, new C{ T="Tagblock"}},
            { 0xBA4, new C{ T="Tagblock"}},

            { 0xBB8, new C{ T="TagRef"}},
            { 0xBE4, new C{ T="TagRef"}},
            { 0xC00, new C{ T="TagRef"}},

            { 0xC1C, new C{ T="Tagblock"}},

            { 0xC30, new C{ T="TagRef"}}, // BITM

            { 0xC4C, new C{ T="Tagblock"}},

            { 0xC60, new C{ T="TagRef"}},
            { 0xC7C, new C{ T="Tagblock"}},
            { 0xC90, new C{ T="Tagblock", B=new Dictionary<long, C> // barrel block
            {
                { 4, new C{ T="Float"}},
                { 8, new C{ T="Float"}},

                { 60, new C{ T="Float"}},
                { 64, new C{ T="Float"}},

                { 76, new C{ T="Float"}},
                { 80, new C{ T="Float"}},

                { 92, new C{ T="Float"}},

                { 0x64, new C{ T="4Byte"}},
                { 0x68, new C{ T="Float"}},

                { 0x84, new C{ T="Tagblock"}},
                { 0x98, new C{ T="Tagblock"}},
                { 0xB8, new C{ T="Float"}},
                { 0xBC, new C{ T="Float"}},
                { 0xFC, new C{ T="Tagblock"}},
                { 0x110, new C{ T="Tagblock"}},
                { 0x124, new C{ T="Tagblock"}},
                { 0x138, new C{ T="Tagblock"}},
                { 0x150, new C{ T="Tagblock"}},
                { 0x164, new C{ T="Tagblock"}},
                { 0x178, new C{ T="Tagblock"}},
                { 0x18C, new C{ T="Tagblock"}},
                { 0x1A4, new C{ T="Tagblock"}},

                { 0x1B8, new C{ T="TagRef"}}, // PROJ
                { 0x1D4, new C{ T="TagRef"}}, // PROJ

                { 0x1F4, new C{ T="Tagblock"}},
                { 0x208, new C{ T="TagRef"}},
                { 0x224, new C{ T="TagRef"}},
                { 0x240, new C{ T="TagRef"}},

                { 0x25C, new C{ T="Float"}},
                { 0x260, new C{ T="Float"}},
                { 0x264, new C{ T="Float"}},
                { 0x268, new C{ T="Float"}},
                { 0x26C, new C{ T="Float"}},
                { 0x270, new C{ T="Float"}},
                { 0x274, new C{ T="Float"}}
            }, S=848}},

            { 0xCBC, new C{ T="TagRef"}},
            { 0xCD8, new C{ T="TagRef"}},
            { 0xCF4, new C{ T="TagRef"}},
            { 0xD10, new C{ T="TagRef"}},

            { 0xDB4, new C{ T="Tagblock"}},
            { 0xDC8, new C{ T="TagRef"}},
            { 0xDEC, new C{ T="Tagblock"}},

            { 0xE00, new C{ T="TagRef"}},
            { 0xE1C, new C{ T="TagRef"}},
            { 0xE38, new C{ T="TagRef"}},
            { 0xE54, new C{ T="TagRef"}},
            { 0xE70, new C{ T="TagRef"}},
            { 0xE8C, new C{ T="TagRef"}},
            { 0xEA8, new C{ T="TagRef"}},
            { 0xEC4, new C{ T="TagRef"}},

            { 0xF80, new C{ T="Tagblock"}},
            { 0xFBC, new C{ T="Tagblock"}},

            { 0x1074, new C{ T="TagRef"}},
            { 0x109C, new C{ T="TagRef"}},
            { 0x10BC, new C{ T="Tagblock"}},
            { 0x10D0, new C{ T="TagRef"}},
            { 0x10EC, new C{ T="TagRef"}},
            { 0x1108, new C{ T="TagRef"}},

            { 0x1128, new C{ T="Tagblock"}},
            { 0x113C, new C{ T="Tagblock"}}
        };

        public static Dictionary<long, C> HlmtTag = new Dictionary<long, C>
        {
            { 0x10, new C{ T="TagRef"}}, // mode
            { 0x2C, new C{ T="TagRef"}}, // COLL
            { 0x48, new C{ T="TagRef"}}, // JMAD
            { 0x64, new C{ T="TagRef"}}, // PHMO

            { 0xAC, new C{ T="Tagblock"}},
            { 0xF4, new C{ T="Tagblock", B=new Dictionary<long, C> // object variant
            {
                { 0x34, new C{ T="Tagblock"}},
                { 0x48, new C{ T="Tagblock", B=new Dictionary<long, C> // object block
                {
                    { 12, new C{ T="TagRef"}}, //
                    { 40, new C{ T="TagRef"}}, //
                }, S=72}},
                { 0x5C, new C{ T="Tagblock"}},
                { 0x70, new C{ T="Tagblock"}},
                { 0x104, new C{ T="TagRef"}},
                { 0x120, new C{ T="TagRef"}},
                { 0x13C, new C{ T="TagRef"}},
                { 0x158, new C{ T="TagRef"}},
                { 0x174, new C{ T="Tagblock"}},

                //{ 0x1BC, new c{ T="Tagblock"}},
                //{ 0x1D0, new c{ T="Tagblock"}},
                //{ 0x1E4, new c{ T="Tagblock"}},
                //{ 0x1F8, new c{ T="Tagblock"}},
                //{ 0x28C, new c{ T="TagRef"}},
                //{ 0x2A8, new c{ T="TagRef"}},
                //{ 0x2C4, new c{ T="TagRef"}},
                //{ 0x2E0, new c{ T="TagRef"}},
                //{ 0x2FC, new c{ T="Tagblock"}},
            }, S=392}}, // 784
            { 0x108, new C{ T="Tagblock"}},
            { 0x11C, new C{ T="Tagblock"}},
            { 0x130, new C{ T="Tagblock"}},
            { 0x150, new C{ T="TagRef"}},

            { 0x16C, new C{ T="Tagblock"}},
            { 0x180, new C{ T="Tagblock"}},
            { 0x194, new C{ T="Tagblock"}},

            { 0x1E8, new C{ T="TagRef"}},

            { 0x204, new C{ T="Tagblock"}},
            { 0x218, new C{ T="Tagblock"}},
            { 0x22C, new C{ T="Tagblock"}},
            { 0x240, new C{ T="Tagblock"}},
            { 0x258, new C{ T="Tagblock"}},
            { 0x26C, new C{ T="Tagblock"}},

            { 0x280, new C{ T="TagRef"}},
            { 0x29C, new C{ T="TagRef"}},

            { 0x2B8, new C{ T="Tagblock"}},
            { 0x2CC, new C{ T="Tagblock"}},
            { 0x2E0, new C{ T="Tagblock"}},
        };

        public static Dictionary<long, C> ProjectileTag = new Dictionary<long, C>
        {
            { 0, new C{ T="Pointer"}},
            { 8, new C{ T="4Byte"}}, // datnum
            { 12, new C{ T="4Byte"}}, // tagID

            { 40, new C{ T="Float"}},
            { 44, new C{ T="Float"}},
            { 48, new C{ T="Float"}},
            { 52, new C{ T="Float"}},
            { 56, new C{ T="Float"}},
            { 60, new C{ T="Float"}},
            { 64, new C{ T="Float"}},
            { 76, new C{ T="Float"}},
            { 80, new C{ T="Float"}},
            { 84, new C{ T="Float"}},

            { 92, new C{ T="Tagblock"}}, // SidecarPathDefinition

            { 112, new C{ T="Float"}},
            { 116, new C{ T="Float"}},

            { 120, new C{ T="TagRef"}}, // vehicle model
            { 148, new C{ T="TagRef"}}, // aset tag ref
            { 176, new C{ T="TagRef"}},
            { 204, new C{ T="TagRef"}},

            { 232, new C{ T="4Byte"}},

            { 240, new C{ T="TagRef"}},

            { 276, new C{ T="Float"}},

            { 288, new C{ T="TagRef"}},
            { 316, new C{ T="TagRef"}}, // foot tag ref
            { 344, new C{ T="TagRef"}}, // vemd tag ref
            { 372, new C{ T="TagRef"}}, // smed tag ref
            { 400, new C{ T="TagRef"}},

            { 432, new C{ T="Float"}},

            { 448, new C{ T="Tagblock"}}, // object_ai_properties
            { 468, new C{ T="Tagblock"}}, // s_object_function_definition
            { 488, new C{ T="4Byte"}},
            { 492, new C{ T="Tagblock"}}, // ObjectRuntimeInterpolatorFunctionsBlock
            { 512, new C{ T="Tagblock"}}, // ObjectFunctionSwitchDefinition
            { 532, new C{ T="Tagblock"}}, // i343::Objects::ObjectFunctionForwarding
            { 552, new C{ T="4Byte"}},
            { 556, new C{ T="Tagblock"}}, // i343::Objects::AmmoRefillVariant
            { 576, new C{ T="4Byte"}},
            { 0x248, new C{ T="Tagblock", B=new Dictionary<long, C> // object_attachment_definition
            {
                { 4, new C{ T="TagRef"}}, // effe
                { 32, new C{ T="TagRef"}}, // effe
                { 64, new C{ T="Tagblock"}},
                { 84, new C{ T="TagRef"}}, //
                { 112, new C{ T="Tagblock"}}
            }, S=148}},
            { 604, new C{ T="Tagblock"}}, // object_indirect_lighting_settings_definition
            { 624, new C{ T="Tagblock"}}, // s_water_physics_hull_surface_definition
            { 644, new C{ T="Tagblock"}}, // s_jetwash_definition
            { 664, new C{ T="Tagblock"}}, // object_definition_widget
            { 684, new C{ T="Tagblock"}}, // object_change_color_definition
            { 704, new C{ T="Tagblock"}}, // s_multiplayer_object_properties_definition
            { 724, new C{ T="Tagblock"}}, // i343::Objects::ForgeObjectEntryDefinition

            { 744, new C{ T="TagRef"}},
            { 772, new C{ T="TagRef"}},

            { 800, new C{ T="Tagblock"}}, // s_object_spawn_effects
            { 820, new C{ T="Tagblock"}}, // ModelDissolveDataBlock

            { 0x448, new C{ T="Tagblock"}},
            { 0x45C, new C{ T="TagRef"}},
            { 0x484, new C{ T="Tagblock"}},
            { 0x4C8, new C{ T="Tagblock"}},
            { 0x4E4, new C{ T="TagRef"}},
            { 0x500, new C{ T="Tagblock"}},
            { 0x514, new C{ T="TagRef"}},
            { 0x530, new C{ T="Tagblock"}},
            { 0x548, new C{ T="Tagblock"}},
            { 0x55C, new C{ T="TagRef"}},

            { 0x578, new C{ T="Tagblock"}},
            { 0x58C, new C{ T="Tagblock"}},

            { 0x5A0, new C{ T="Float"}},
            { 0x5A4, new C{ T="Float"}},
            { 0x5A8, new C{ T="Float"}},
            { 0x5AC, new C{ T="Float"}},
            { 0x5B0, new C{ T="Float"}},
            { 0x5B4, new C{ T="Float"}},
            { 0x5B8, new C{ T="Float"}},
            { 0x5BC, new C{ T="Float"}},
            { 0x5C0, new C{ T="Float"}},
            { 0x5E0, new C{ T="Float"}},

            { 0x5E8, new C{ T="Tagblock"}},
            { 0x60C, new C{ T="Tagblock"}},
            { 0x620, new C{ T="Tagblock"}},
            { 0x638, new C{ T="Tagblock"}},
            { 0x64C, new C{ T="Float"}},
            { 0x650, new C{ T="Tagblock"}},
            { 0x664, new C{ T="Tagblock"}},
            { 0x678, new C{ T="Tagblock"}},
            { 0x688, new C{ T="Tagblock"}},
            { 0x6A0, new C{ T="Tagblock"}},

            { 0x6B8, new C{ T="TagRef"}}, //PROJ

            { 0x6D4, new C{ T="Float"}},
            { 0x6F8, new C{ T="Float"}},
            { 0x704, new C{ T="Float"}},
            { 0x708, new C{ T="Float"}},
            { 0x710, new C{ T="Float"}},
            { 0x720, new C{ T="Float"}},
            { 0x72C, new C{ T="Float"}},

            { 0x738, new C{ T="TagRef"}}, // EFFE
            { 0x754, new C{ T="TagRef"}}, // EEFFE
            { 0x770, new C{ T="TagRef"}},
            { 0x78C, new C{ T="TagRef"}}, // JPT
            { 0x7A8, new C{ T="TagRef"}}, // JPT
            { 0x7C4, new C{ T="TagRef"}},
            { 0x7E0, new C{ T="TagRef"}},
            { 0x800, new C{ T="TagRef"}},
            { 0x81C, new C{ T="TagRef"}},

            { 0x83C, new C{ T="Tagblock"}},

            { 0x850, new C{ T="TagRef"}},
            { 0x86C, new C{ T="TagRef"}},
            { 0x888, new C{ T="TagRef"}}, // SND
            { 0x8A4, new C{ T="TagRef"}},
            { 0x8C0, new C{ T="TagRef"}}, // SND
            { 0x8DC, new C{ T="TagRef"}},

            { 0x8FC, new C{ T="Float"}},

            { 0x908, new C{ T="TagRef"}},
            { 0x924, new C{ T="TagRef"}},
            { 0x944, new C{ T="TagRef"}},
            { 0x984, new C{ T="TagRef"}},
            { 0x9A0, new C{ T="TagRef"}}, // JPT
            { 0x9BC, new C{ T="TagRef"}}, // JPT

            { 0x9DC, new C{ T="Tagblock"}},

            { 0x9F0, new C{ T="Float"}},

            { 0x9F4, new C{ T="TagRef"}}, //JPT
            { 0xA10, new C{ T="TagRef"}}, // JPT

            { 0xA2C, new C{ T="Float"}},
            { 0xA34, new C{ T="Float"}},
            { 0xA38, new C{ T="Float"}},
            { 0xA40, new C{ T="Float"}},
            { 0xA44, new C{ T="Float"}},
            { 0xA80, new C{ T="Float"}},

            { 0xAA0, new C{ T="TagRef"}},

            { 0xABC, new C{ T="Tagblock"}},
            { 0xAD0, new C{ T="Tagblock"}},
            { 0xAE4, new C{ T="Tagblock"}},
            { 0xAF8, new C{ T="Tagblock"}},
            { 0xB0C, new C{ T="Tagblock"}},

            { 0xB20, new C{ T="TagRef"}},

            { 0xB3C, new C{ T="Float"}},
            { 0xB40, new C{ T="Float"}},
            { 0xB44, new C{ T="Float"}},

            { 0xB48, new C{ T="Tagblock"}},
            { 0xB60, new C{ T="Tagblock"}},

            { 0xB8C, new C{ T="Float"}},
            { 0xB90, new C{ T="Float"}},

            { 0xBA0, new C{ T="TagRef"}}, // JPT
            { 0xBBC, new C{ T="TagRef"}},
            { 0xBDC, new C{ T="TagRef"}}, // EFFE
            { 0xBF5, new C{ T="TagRef"}}, // EFFE
        };
    }
}