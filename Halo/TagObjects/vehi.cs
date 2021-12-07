using System.Collections.Generic;

namespace Assembly69.Halo.TagObjects
{
    public class vehi
    {
        public struct c
        {
            public string T { get; set; }
            public Dictionary<long, c> B { get; set; }

            public long S { get; set; } // length of tagblock
        }

        public static Dictionary<long, c> VehicleTag = new Dictionary<long, c>
        {
            { 0, new c{ T="Pointer"}},
            { 8, new c{ T="4Byte"}}, // datnum
            { 12, new c{ T="4Byte"}}, // tagID

            { 40, new c{ T="Float"}},
            { 44, new c{ T="Float"}},
            { 48, new c{ T="Float"}},
            { 52, new c{ T="Float"}},
            { 56, new c{ T="Float"}},
            { 60, new c{ T="Float"}},
            { 64, new c{ T="Float"}},
            { 76, new c{ T="Float"}},
            { 80, new c{ T="Float"}},
            { 84, new c{ T="Float"}},

            { 92, new c{ T="Tagblock"}}, // SidecarPathDefinition

            { 112, new c{ T="Float"}},
            { 116, new c{ T="Float"}},

            { 120, new c{ T="TagRef"}}, // vehicle model
            { 148, new c{ T="TagRef"}}, // aset tag ref
            { 176, new c{ T="TagRef"}},
            { 204, new c{ T="TagRef"}},

            { 232, new c{ T="4Byte"}},

            { 240, new c{ T="TagRef"}},

            { 276, new c{ T="Float"}},

            { 288, new c{ T="TagRef"}},
            { 316, new c{ T="TagRef"}}, // foot tag ref
            { 344, new c{ T="TagRef"}}, // vemd tag ref
            { 372, new c{ T="TagRef"}}, // smed tag ref
            { 400, new c{ T="TagRef"}},

            { 432, new c{ T="Float"}},

            { 448, new c{ T="Tagblock"}}, // object_ai_properties
            { 468, new c{ T="Tagblock"}}, // s_object_function_definition
            { 488, new c{ T="4Byte"}},
            { 492, new c{ T="Tagblock"}}, // ObjectRuntimeInterpolatorFunctionsBlock
            { 512, new c{ T="Tagblock"}}, // ObjectFunctionSwitchDefinition
            { 532, new c{ T="Tagblock"}}, // i343::Objects::ObjectFunctionForwarding
            { 552, new c{ T="4Byte"}},
            { 556, new c{ T="Tagblock"}}, // i343::Objects::AmmoRefillVariant
            { 576, new c{ T="4Byte"}},
            { 0x248, new c{ T="Tagblock", B=new Dictionary<long, c> // object_attachment_definition
            {
                { 4, new c{ T="TagRef"}}, // effe
                { 32, new c{ T="TagRef"}}, // effe
                { 64, new c{ T="Tagblock"}},
                { 84, new c{ T="TagRef"}}, //
                { 112, new c{ T="Tagblock"}}
            }, S=148}},
            { 604, new c{ T="Tagblock"}}, // object_indirect_lighting_settings_definition
            { 624, new c{ T="Tagblock"}}, // s_water_physics_hull_surface_definition
            { 644, new c{ T="Tagblock"}}, // s_jetwash_definition
            { 664, new c{ T="Tagblock"}}, // object_definition_widget
            { 684, new c{ T="Tagblock"}}, // object_change_color_definition
            { 704, new c{ T="Tagblock"}}, // s_multiplayer_object_properties_definition
            { 724, new c{ T="Tagblock"}}, // i343::Objects::ForgeObjectEntryDefinition

            { 744, new c{ T="TagRef"}},
            { 772, new c{ T="TagRef"}},

            { 800, new c{ T="Tagblock"}}, // s_object_spawn_effects
            { 820, new c{ T="Tagblock"}}, // ModelDissolveDataBlock

            { 840, new c{ T="String"}}, // (vehicle_partial_emp)

            { 0x448, new c{ T="Tagblock"}}, // HsReferencesBlock
            { 0x45C, new c{ T="TagRef"}},

            { 0x480, new c{ T="Float"}},

            { 0x484, new c{ T="Tagblock"}}, // metalabel somethiong ?? recheck this one
            { 0x498, new c{ T="Tagblock"}}, // SoundRTPCBlockDefinition
            { 0x4AC, new c{ T="Tagblock"}}, // SoundSweetenerBlockDefinition

            { 0x4C0, new c{ T="Pointer"}},

            { 0x4C8, new c{ T="Tagblock"}}, // i343::Objects::ComputeFunctionSmoothingBlockDefinition

            { 0x4DC, new c{ T="Float"}},
            { 0x4E0, new c{ T="Float"}},

            { 0x4E4, new c{ T="TagRef"}},
            { 0x500, new c{ T="Tagblock"}}, // i343::SpartanTracking::ObjectDefinition

            { 0x514, new c{ T="TagRef"}},
            { 0x530, new c{ T="Tagblock"}}, // InteractionOpportunityDefinition

            { 0x544, new c{ T="Float"}},
            { 0x548, new c{ T="Tagblock"}}, // ScriptedSequenceActionDefinition
            { 0x55C, new c{ T="TagRef"}},
            { 0x578, new c{ T="Tagblock"}}, // AnimChannelEntry
            { 0x58C, new c{ T="Tagblock"}}, // AnimSetTableEntry

            { 0x5A0, new c{ T="Float"}},
            { 0x5A4, new c{ T="Float"}},
            { 0x5A8, new c{ T="Float"}},
            { 0x5AC, new c{ T="Float"}},
            { 0x5B0, new c{ T="Float"}},
            { 0x5B4, new c{ T="Float"}},
            { 0x5B8, new c{ T="Float"}},
            { 0x5BC, new c{ T="Float"}},
            { 0x5C0, new c{ T="Float"}},
            { 0x5CC, new c{ T="4Byte"}},
            { 0x5DC, new c{ T="Float"}},
            { 0x5E0, new c{ T="Float"}},
            { 0x5E4, new c{ T="Float"}},

            { 0x5E8, new c{ T="Tagblock"}}, // LegGroundingSettings

            { 0x5FC, new c{ T="Float"}},
            { 0x600, new c{ T="Float"}},
            { 0x604, new c{ T="Float"}},
            { 0x608, new c{ T="Float"}},

            { 0x60C, new c{ T="Tagblock"}}, // i343::Objects::ObjectNodeGraphDefinition
            { 0x620, new c{ T="Tagblock"}}, // i343::Objects::AnimationMatchingTableEntry

            { 0x634, new c{ T="Float"}},

            { 0x638, new c{ T="Tagblock"}}, // i343::Objects::ModelVariantSwappingTableEntry

            { 0x63C, new c{ T="Float"}},

            { 0x650, new c{ T="Tagblock"}}, // i343::Items::LocationSensorDefinition
            { 0x664, new c{ T="Tagblock"}}, // i343::Items::ShroudGeneratorDefinition
            { 0x678, new c{ T="Tagblock"}}, // i343::Objects::PowerComponentDefinition
            { 0x68C, new c{ T="Tagblock"}}, // i343::Objects::SelfDestructHandlerDefinition
            { 0x6A0, new c{ T="Tagblock"}}, // i343::Objects::IndirectLightingComponentDefinition

            { 0x6B4, new c{ T="Float"}},
            { 0x6B8, new c{ T="4Byte"}},
            { 0x6BC, new c{ T="4Byte"}},

            { 0x6C4, new c{ T="TagRef"}},
            { 0x6E0, new c{ T="Tagblock"}}, // s_campaign_metagame_bucket
            { 0x6F4, new c{ T="Tagblock"}}, // s_unit_screen_effect_definition

            { 0x70C, new c{ T="Float"}},
            { 0x720, new c{ T="Tagblock"}}, // s_unit_camera_track
            { 0x768, new c{ T="Tagblock"}}, // s_unit_camera_acceleration
            { 0x77C, new c{ T="Float"}},
            { 0x790, new c{ T="Tagblock"}}, // s_unit_camera_track
            { 0x7D8, new c{ T="Tagblock"}}, // s_unit_camera_acceleration

            { 0x7F0, new c{ T="TagRef"}},

            { 0x818, new c{ T="Float"}},
            { 0x81C, new c{ T="Float"}},
            { 0x830, new c{ T="Float"}},

            { 0x870, new c{ T="TagRef"}},

            { 0x898, new c{ T="Float"}},

            { 0x8BC, new c{ T="Tagblock"}}, // WeaponSpecificMarkers
            { 0x8D0, new c{ T="TagRef"}},
            { 0x908, new c{ T="TagRef"}},
            { 0x924, new c{ T="TagRef"}},
            { 0x940, new c{ T="TagRef"}},
            { 0x95C, new c{ T="TagRef"}},
            { 0x978, new c{ T="TagRef"}},
            { 0x994, new c{ T="TagRef"}},
            { 0x9B0, new c{ T="TagRef"}},

            { 0x9D0, new c{ T="Float"}},

            { 0x9E0, new c{ T="Tagblock"}}, // HudUnitSoundDefinitions
            { 0x9F4, new c{ T="Tagblock"}}, // dialogue_variant_definition

            { 0xA30, new c{ T="Float"}},
            { 0xA34, new c{ T="Float"}},
            { 0xA38, new c{ T="Float"}},

            { 0xA64, new c{ T="Tagblock"}}, // powered_seat_definition
            { 0xA78, new c{ T="Tagblock", B=new Dictionary<long, c> // unit_initial_weapon
            {
                { 0, new c{ T="TagRef"}}, // weap
                { 40, new c{ T="Float"}},
                { 48, new c{ T="Float"}},
                { 148, new c{ T="TagRef"}}, //
                { 188, new c{ T="Tagblock"}}
            }, S=212}},
            { 0xA8C, new c{ T="Tagblock"}}, // s_target_tracking_parameters
            { 0xAA0, new c{ T="Tagblock"}}, // unit_seat

            { 0xAB4, new c{ T="Float"}},
            { 0xAB8, new c{ T="Float"}},
            { 0xABC, new c{ T="Float"}},

            { 0xAC8, new c{ T="TagRef"}},
            { 0xAE4, new c{ T="Tagblock"}}, // i343::Objects::PowerComponentDefinition
            { 0xAF8, new c{ T="TagRef"}},
            { 0xB14, new c{ T="TagRef"}},

            { 0xB38, new c{ T="Float"}},
            { 0xB3C, new c{ T="Float"}},
            { 0xB40, new c{ T="Float"}},
            { 0xB44, new c{ T="Float"}},
            { 0xB48, new c{ T="Float"}},
            { 0xB4C, new c{ T="Float"}},

            { 0xB54, new c{ T="Pointer"}},
            { 0xB5C, new c{ T="Pointer"}},

            { 0xB7C, new c{ T="TagRef"}},
            { 0xB98, new c{ T="TagRef"}},

            { 0xBB4, new c{ T="Tagblock"}}, // ExitAndDetachVariant

            { 0xBCC, new c{ T="Float"}},

            { 0xBE4, new c{ T="TagRef"}},

            { 0xC00, new c{ T="Float"}},
            { 0xC04, new c{ T="Float"}},
            { 0xC08, new c{ T="Float"}},
            { 0xC0C, new c{ T="Float"}},
            { 0xC10, new c{ T="Float"}},
            { 0xC18, new c{ T="Float"}},
            { 0xC1C, new c{ T="Float"}},
            { 0xC28, new c{ T="Float"}},
            { 0xC30, new c{ T="Float"}},
            { 0xC34, new c{ T="Float"}},
            { 0xC3C, new c{ T="Float"}},
            { 0xC40, new c{ T="Float"}},
            { 0xC5C, new c{ T="Float"}},
            { 0xC64, new c{ T="Float"}},
            { 0xC68, new c{ T="Float"}},
            { 0xC70, new c{ T="Float"}},
            { 0xC74, new c{ T="Float"}},
            { 0xC7C, new c{ T="Float"}},
            { 0xC80, new c{ T="Float"}},
            { 0xC84, new c{ T="Float"}},
            { 0xC88, new c{ T="Float"}},
            { 0xC8C, new c{ T="Float"}},
            { 0xC94, new c{ T="Float"}},
            { 0xC98, new c{ T="Float"}},
            { 0xCA0, new c{ T="Float"}},
            { 0xCAC, new c{ T="Float"}},
            { 0xCB8, new c{ T="Float"}},
            { 0xCBC, new c{ T="Float"}},
            { 0xCC4, new c{ T="Float"}},
            { 0xCC8, new c{ T="Float"}},
            { 0xCD0, new c{ T="Float"}},
            { 0xCD4, new c{ T="Float"}},
            { 0xCDC, new c{ T="Float"}},
            { 0xCE0, new c{ T="Float"}},
            { 0xCE8, new c{ T="Float"}},
            { 0xCF4, new c{ T="Float"}},
            { 0xCF8, new c{ T="Float"}},
            { 0xD00, new c{ T="Float"}},
            { 0xD04, new c{ T="Float"}},
            { 0xD0C, new c{ T="Float"}},
            { 0xD10, new c{ T="Float"}},
            { 0xD18, new c{ T="Float"}},
            { 0xD1C, new c{ T="Float"}},
            { 0xD24, new c{ T="Float"}},

            { 0xD28, new c{ T="TagRef"}},
            { 0xD44, new c{ T="TagRef"}}, // EFFECT TAG REF
            { 0xD64, new c{ T="TagRef"}},
            { 0xD80, new c{ T="TagRef"}}, // VEHI TAG REF

            { 0xDA0, new c{ T="Tagblock"}}, // s_vehicle_human_tank_definition
            { 0xDB4, new c{ T="Tagblock"}}, // s_vehicle_human_jeep_definition
            { 0xDC8, new c{ T="Tagblock"}}, // s_vehicle_human_plane_definition
            { 0xDDC, new c{ T="Tagblock"}}, // s_vehicle_alien_scout_definition
            { 0xDF0, new c{ T="Tagblock"}}, // s_vehicle_alien_fighter_definition
            { 0xE04, new c{ T="Tagblock"}}, // s_vehicle_turret_definition
            { 0xE18, new c{ T="Tagblock"}}, // s_vehicle_vtol_definition
            { 0xE2C, new c{ T="Tagblock"}}, // s_vehicle_chopper_definition
            { 0xE40, new c{ T="Tagblock"}}, // s_vehicle_guardian_definition
            { 0xE54, new c{ T="Tagblock"}}, // s_vehicle_jackal_glider_definition
            { 0xE68, new c{ T="Tagblock"}}, // s_vehicle_space_fighter_definition
            { 0xE7C, new c{ T="Tagblock"}}, // s_vehicle_revenant_definition

            { 0xE90, new c{ T="Float"}},
            { 0xE94, new c{ T="4Byte"}},

            { 0xE98, new c{ T="Tagblock"}}, // i343::Vehicles::AntiGravityPointConfiguration
            { 0xEAC, new c{ T="Tagblock"}}, // i343::Vehicles::AntiGravityPointDefinition
            { 0xEC0, new c{ T="Tagblock"}}, // i343::Vehicles::FrictionPointConfiguration
            { 0xED4, new c{ T="Tagblock"}}, // i343::Vehicles::FrictionPointDefinition

            { 0xEE8, new c{ T="Float"}},
            { 0xEEC, new c{ T="Float"}},

            { 0xEF0, new c{ T="Pointer"}},
            { 0xEF8, new c{ T="Pointer"}},
            { 0xF08, new c{ T="Pointer"}},
            { 0xF10, new c{ T="Pointer"}},

            { 0xF2C, new c{ T="Tagblock"}}, // s_unit_trick_definition
            { 0xF40, new c{ T="Float"}},
            { 0xF50, new c{ T="Float"}},
            { 0xF54, new c{ T="Float"}},
            { 0xF58, new c{ T="Float"}},
            { 0xF60, new c{ T="Float"}},
            { 0xF74, new c{ T="4Byte"}},

            { 0xF78, new c{ T="TagRef"}},
            { 0xF94, new c{ T="TagRef"}},
            { 0xFB0, new c{ T="TagRef"}},
            { 0xFD0, new c{ T="TagRef"}},
            { 0xFF0, new c{ T="TagRef"}},
            { 0x1024, new c{ T="Tagblock"}}, // SoundRTPCBlockDefinition
            { 0x1038, new c{ T="Tagblock"}}, // SoundSweetenerBlockDefinition

            { 0x104C, new c{ T="TagRef"}},
            { 0x1068, new c{ T="TagRef"}},
            { 0x1084, new c{ T="TagRef"}},

            { 0x10A4, new c{ T="Tagblock"}}, // s_vehicleAiCruiseControl
            { 0x10B8, new c{ T="Tagblock"}}, // Interface::UIItemInfo

            { 0x10CC, new c{ T="TagRef"}},
            { 0x10EC, new c{ T="Float"}},
        };

        public static Dictionary<long, c> WeaponTag = new Dictionary<long, c>
        {
            { 0, new c{ T="Pointer"}},
            { 8, new c{ T="4Byte"}}, // datnum
            { 12, new c{ T="4Byte"}}, // tagID

            { 0x05C, new c{ T="Tagblock"}},
            { 0x078, new c{ T="TagRef"}}, // HLMT
            { 0x094, new c{ T="TagRef"}},
            { 0x0B0, new c{ T="TagRef"}},
            { 0x0CC, new c{ T="TagRef"}},
            { 0x0F0, new c{ T="TagRef"}},

            { 0x10C, new c{ T="Tagblock"}},

            { 0x120, new c{ T="TagRef"}},
            { 0x13C, new c{ T="TagRef"}}, // FOOT
            { 0x158, new c{ T="TagRef"}}, // VMED
            { 0x174, new c{ T="TagRef"}}, // SMED
            { 0x190, new c{ T="TagRef"}},

            { 0x1C0, new c{ T="Tagblock"}},
            { 0x1D4, new c{ T="Tagblock"}},
            { 0x1EC, new c{ T="Tagblock"}},
            { 0x200, new c{ T="Tagblock"}},
            { 0x214, new c{ T="Tagblock"}},
            { 0x22C, new c{ T="Tagblock"}},
            { 0x248, new c{ T="Tagblock", B=new Dictionary<long, c> // attachment block
            {
                { 4, new c{ T="TagRef"}}, // effe
                { 32, new c{ T="TagRef"}}, // effe
                { 64, new c{ T="Tagblock"}},
                { 84, new c{ T="TagRef"}}, //
                { 112, new c{ T="Tagblock"}}
            }, S=148}},

            { 0x25C, new c{ T="Tagblock"}},
            { 0x270, new c{ T="Tagblock"}},
            { 0x284, new c{ T="Tagblock"}},
            { 0x298, new c{ T="Tagblock"}},
            { 0x2AC, new c{ T="Tagblock"}},
            { 0x2C0, new c{ T="Tagblock"}},
            { 0x2D4, new c{ T="Tagblock"}},

            { 0x2E8, new c{ T="TagRef"}},
            { 0x304, new c{ T="TagRef"}},

            { 0x320, new c{ T="Tagblock"}},
            { 0x334, new c{ T="Tagblock"}},
            { 0x448, new c{ T="Tagblock"}},

            { 0x45C, new c{ T="TagRef"}},

            { 0x484, new c{ T="Tagblock"}},
            { 0x498, new c{ T="Tagblock"}},
            { 0x4AC, new c{ T="Tagblock"}},
            { 0x4C8, new c{ T="Tagblock"}},

            { 0x4E4, new c{ T="TagRef"}},

            { 0x500, new c{ T="Tagblock"}},
            { 0x514, new c{ T="TagRef"}},

            { 0x530, new c{ T="Tagblock"}},
            { 0x548, new c{ T="Tagblock"}},

            { 0x55C, new c{ T="TagRef"}},

            { 0x578, new c{ T="Tagblock"}},
            { 0x58C, new c{ T="Tagblock"}},
            { 0x5E8, new c{ T="Tagblock"}},
            { 0x60C, new c{ T="Tagblock"}},
            { 0x620, new c{ T="Tagblock"}},
            { 0x638, new c{ T="Tagblock"}},
            { 0x650, new c{ T="Tagblock"}},
            { 0x664, new c{ T="Tagblock"}},
            { 0x678, new c{ T="Tagblock"}},
            { 0x68C, new c{ T="Tagblock"}},
            { 0x6A0, new c{ T="Tagblock"}},
            { 0x730, new c{ T="Tagblock"}},
            { 0x744, new c{ T="Tagblock"}},

            { 0x758, new c{ T="TagRef"}},
            { 0x77C, new c{ T="TagRef"}},
            { 0x798, new c{ T="TagRef"}},
            { 0x7D8, new c{ T="TagRef"}},
            { 0x7F4, new c{ T="TagRef"}},
            { 0x850, new c{ T="TagRef"}}, // WEAP
            { 0x88C, new c{ T="TagRef"}},
            { 0x8A8, new c{ T="TagRef"}},
            { 0x924, new c{ T="TagRef"}},
            { 0x940, new c{ T="TagRef"}},
            { 0x95C, new c{ T="TagRef"}},
            { 0x978, new c{ T="TagRef"}},

            { 0x994, new c{ T="Tagblock"}},

            { 0x9A8, new c{ T="TagRef"}},
            { 0x9D4, new c{ T="TagRef"}},

            { 0x9F0, new c{ T="Tagblock"}},
            { 0xA80, new c{ T="Tagblock"}},
            { 0xA94, new c{ T="Tagblock"}},

            { 0xAF0, new c{ T="TagRef"}},
            { 0xB0C, new c{ T="TagRef"}},

            { 0xB6C, new c{ T="Tagblock"}},
            { 0xB88, new c{ T="Tagblock"}},
            { 0xBA4, new c{ T="Tagblock"}},

            { 0xBB8, new c{ T="TagRef"}},
            { 0xBE4, new c{ T="TagRef"}},
            { 0xC00, new c{ T="TagRef"}},

            { 0xC1C, new c{ T="Tagblock"}},

            { 0xC30, new c{ T="TagRef"}}, // BITM

            { 0xC4C, new c{ T="Tagblock"}},

            { 0xC60, new c{ T="TagRef"}},
            { 0xC7C, new c{ T="Tagblock"}},
            { 0xC90, new c{ T="Tagblock", B=new Dictionary<long, c> // barrel block
            {
                { 4, new c{ T="Float"}},
                { 8, new c{ T="Float"}},

                { 60, new c{ T="Float"}},
                { 64, new c{ T="Float"}},

                { 76, new c{ T="Float"}},
                { 80, new c{ T="Float"}},

                { 92, new c{ T="Float"}},

                { 0x64, new c{ T="4Byte"}},
                { 0x68, new c{ T="Float"}},

                { 0x84, new c{ T="Tagblock"}},
                { 0x98, new c{ T="Tagblock"}},
                { 0xB8, new c{ T="Float"}},
                { 0xBC, new c{ T="Float"}},
                { 0xFC, new c{ T="Tagblock"}},
                { 0x110, new c{ T="Tagblock"}},
                { 0x124, new c{ T="Tagblock"}},
                { 0x138, new c{ T="Tagblock"}},
                { 0x150, new c{ T="Tagblock"}},
                { 0x164, new c{ T="Tagblock"}},
                { 0x178, new c{ T="Tagblock"}},
                { 0x18C, new c{ T="Tagblock"}},
                { 0x1A4, new c{ T="Tagblock"}},

                { 0x1B8, new c{ T="TagRef"}}, // PROJ
                { 0x1D4, new c{ T="TagRef"}}, // PROJ

                { 0x1F4, new c{ T="Tagblock"}},
                { 0x208, new c{ T="TagRef"}},
                { 0x224, new c{ T="TagRef"}},
                { 0x240, new c{ T="TagRef"}},

                { 0x25C, new c{ T="Float"}},
                { 0x260, new c{ T="Float"}},
                { 0x264, new c{ T="Float"}},
                { 0x268, new c{ T="Float"}},
                { 0x26C, new c{ T="Float"}},
                { 0x270, new c{ T="Float"}},
                { 0x274, new c{ T="Float"}}
            }, S=848}},

            { 0xCBC, new c{ T="TagRef"}},
            { 0xCD8, new c{ T="TagRef"}},
            { 0xCF4, new c{ T="TagRef"}},
            { 0xD10, new c{ T="TagRef"}},

            { 0xDB4, new c{ T="Tagblock"}},
            { 0xDC8, new c{ T="TagRef"}},
            { 0xDEC, new c{ T="Tagblock"}},

            { 0xE00, new c{ T="TagRef"}},
            { 0xE1C, new c{ T="TagRef"}},
            { 0xE38, new c{ T="TagRef"}},
            { 0xE54, new c{ T="TagRef"}},
            { 0xE70, new c{ T="TagRef"}},
            { 0xE8C, new c{ T="TagRef"}},
            { 0xEA8, new c{ T="TagRef"}},
            { 0xEC4, new c{ T="TagRef"}},

            { 0xF80, new c{ T="Tagblock"}},
            { 0xFBC, new c{ T="Tagblock"}},

            { 0x1074, new c{ T="TagRef"}},
            { 0x109C, new c{ T="TagRef"}},
            { 0x10BC, new c{ T="Tagblock"}},
            { 0x10D0, new c{ T="TagRef"}},
            { 0x10EC, new c{ T="TagRef"}},
            { 0x1108, new c{ T="TagRef"}},

            { 0x1128, new c{ T="Tagblock"}},
            { 0x113C, new c{ T="Tagblock"}}
        };

        public static Dictionary<long, c> HLMTTag = new Dictionary<long, c>
        {
            { 0x10, new c{ T="TagRef"}}, // mode
            { 0x2C, new c{ T="TagRef"}}, // COLL
            { 0x48, new c{ T="TagRef"}}, // JMAD
            { 0x64, new c{ T="TagRef"}}, // PHMO

            { 0xAC, new c{ T="Tagblock"}},
            { 0xF4, new c{ T="Tagblock", B=new Dictionary<long, c> // object variant
            {
                { 0x34, new c{ T="Tagblock"}},
                { 0x48, new c{ T="Tagblock", B=new Dictionary<long, c> // object block
                {
                    { 12, new c{ T="TagRef"}}, //
                    { 40, new c{ T="TagRef"}}, //
                }, S=72}},
                { 0x5C, new c{ T="Tagblock"}},
                { 0x70, new c{ T="Tagblock"}},
                { 0x104, new c{ T="TagRef"}},
                { 0x120, new c{ T="TagRef"}},
                { 0x13C, new c{ T="TagRef"}},
                { 0x158, new c{ T="TagRef"}},
                { 0x174, new c{ T="Tagblock"}},

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
            { 0x108, new c{ T="Tagblock"}},
            { 0x11C, new c{ T="Tagblock"}},
            { 0x130, new c{ T="Tagblock"}},
            { 0x150, new c{ T="TagRef"}},

            { 0x16C, new c{ T="Tagblock"}},
            { 0x180, new c{ T="Tagblock"}},
            { 0x194, new c{ T="Tagblock"}},

            { 0x1E8, new c{ T="TagRef"}},

            { 0x204, new c{ T="Tagblock"}},
            { 0x218, new c{ T="Tagblock"}},
            { 0x22C, new c{ T="Tagblock"}},
            { 0x240, new c{ T="Tagblock"}},
            { 0x258, new c{ T="Tagblock"}},
            { 0x26C, new c{ T="Tagblock"}},

            { 0x280, new c{ T="TagRef"}},
            { 0x29C, new c{ T="TagRef"}},

            { 0x2B8, new c{ T="Tagblock"}},
            { 0x2CC, new c{ T="Tagblock"}},
            { 0x2E0, new c{ T="Tagblock"}},
        };

        public static Dictionary<long, c> projectileTag = new Dictionary<long, c>
        {
            { 0, new c{ T="Pointer"}},
            { 8, new c{ T="4Byte"}}, // datnum
            { 12, new c{ T="4Byte"}}, // tagID

            { 40, new c{ T="Float"}},
            { 44, new c{ T="Float"}},
            { 48, new c{ T="Float"}},
            { 52, new c{ T="Float"}},
            { 56, new c{ T="Float"}},
            { 60, new c{ T="Float"}},
            { 64, new c{ T="Float"}},
            { 76, new c{ T="Float"}},
            { 80, new c{ T="Float"}},
            { 84, new c{ T="Float"}},

            { 92, new c{ T="Tagblock"}}, // SidecarPathDefinition

            { 112, new c{ T="Float"}},
            { 116, new c{ T="Float"}},

            { 120, new c{ T="TagRef"}}, // vehicle model
            { 148, new c{ T="TagRef"}}, // aset tag ref
            { 176, new c{ T="TagRef"}},
            { 204, new c{ T="TagRef"}},

            { 232, new c{ T="4Byte"}},

            { 240, new c{ T="TagRef"}},

            { 276, new c{ T="Float"}},

            { 288, new c{ T="TagRef"}},
            { 316, new c{ T="TagRef"}}, // foot tag ref
            { 344, new c{ T="TagRef"}}, // vemd tag ref
            { 372, new c{ T="TagRef"}}, // smed tag ref
            { 400, new c{ T="TagRef"}},

            { 432, new c{ T="Float"}},

            { 448, new c{ T="Tagblock"}}, // object_ai_properties
            { 468, new c{ T="Tagblock"}}, // s_object_function_definition
            { 488, new c{ T="4Byte"}},
            { 492, new c{ T="Tagblock"}}, // ObjectRuntimeInterpolatorFunctionsBlock
            { 512, new c{ T="Tagblock"}}, // ObjectFunctionSwitchDefinition
            { 532, new c{ T="Tagblock"}}, // i343::Objects::ObjectFunctionForwarding
            { 552, new c{ T="4Byte"}},
            { 556, new c{ T="Tagblock"}}, // i343::Objects::AmmoRefillVariant
            { 576, new c{ T="4Byte"}},
            { 0x248, new c{ T="Tagblock", B=new Dictionary<long, c> // object_attachment_definition
            {
                { 4, new c{ T="TagRef"}}, // effe
                { 32, new c{ T="TagRef"}}, // effe
                { 64, new c{ T="Tagblock"}},
                { 84, new c{ T="TagRef"}}, //
                { 112, new c{ T="Tagblock"}}
            }, S=148}},
            { 604, new c{ T="Tagblock"}}, // object_indirect_lighting_settings_definition
            { 624, new c{ T="Tagblock"}}, // s_water_physics_hull_surface_definition
            { 644, new c{ T="Tagblock"}}, // s_jetwash_definition
            { 664, new c{ T="Tagblock"}}, // object_definition_widget
            { 684, new c{ T="Tagblock"}}, // object_change_color_definition
            { 704, new c{ T="Tagblock"}}, // s_multiplayer_object_properties_definition
            { 724, new c{ T="Tagblock"}}, // i343::Objects::ForgeObjectEntryDefinition

            { 744, new c{ T="TagRef"}},
            { 772, new c{ T="TagRef"}},

            { 800, new c{ T="Tagblock"}}, // s_object_spawn_effects
            { 820, new c{ T="Tagblock"}}, // ModelDissolveDataBlock

            { 0x448, new c{ T="Tagblock"}},
            { 0x45C, new c{ T="TagRef"}},
            { 0x484, new c{ T="Tagblock"}},
            { 0x4C8, new c{ T="Tagblock"}},
            { 0x4E4, new c{ T="TagRef"}},
            { 0x500, new c{ T="Tagblock"}},
            { 0x514, new c{ T="TagRef"}},
            { 0x530, new c{ T="Tagblock"}},
            { 0x548, new c{ T="Tagblock"}},
            { 0x55C, new c{ T="TagRef"}},

            { 0x578, new c{ T="Tagblock"}},
            { 0x58C, new c{ T="Tagblock"}},

            { 0x5A0, new c{ T="Float"}},
            { 0x5A4, new c{ T="Float"}},
            { 0x5A8, new c{ T="Float"}},
            { 0x5AC, new c{ T="Float"}},
            { 0x5B0, new c{ T="Float"}},
            { 0x5B4, new c{ T="Float"}},
            { 0x5B8, new c{ T="Float"}},
            { 0x5BC, new c{ T="Float"}},
            { 0x5C0, new c{ T="Float"}},
            { 0x5E0, new c{ T="Float"}},

            { 0x5E8, new c{ T="Tagblock"}},
            { 0x60C, new c{ T="Tagblock"}},
            { 0x620, new c{ T="Tagblock"}},
            { 0x638, new c{ T="Tagblock"}},
            { 0x64C, new c{ T="Float"}},
            { 0x650, new c{ T="Tagblock"}},
            { 0x664, new c{ T="Tagblock"}},
            { 0x678, new c{ T="Tagblock"}},
            { 0x688, new c{ T="Tagblock"}},
            { 0x6A0, new c{ T="Tagblock"}},

            { 0x6B8, new c{ T="TagRef"}}, //PROJ

            { 0x6D4, new c{ T="Float"}},
            { 0x6F8, new c{ T="Float"}},
            { 0x704, new c{ T="Float"}},
            { 0x708, new c{ T="Float"}},
            { 0x710, new c{ T="Float"}},
            { 0x720, new c{ T="Float"}},
            { 0x72C, new c{ T="Float"}},

            { 0x738, new c{ T="TagRef"}}, // EFFE
            { 0x754, new c{ T="TagRef"}}, // EEFFE
            { 0x770, new c{ T="TagRef"}},
            { 0x78C, new c{ T="TagRef"}}, // JPT
            { 0x7A8, new c{ T="TagRef"}}, // JPT
            { 0x7C4, new c{ T="TagRef"}},
            { 0x7E0, new c{ T="TagRef"}},
            { 0x800, new c{ T="TagRef"}},
            { 0x81C, new c{ T="TagRef"}},

            { 0x83C, new c{ T="Tagblock"}},

            { 0x850, new c{ T="TagRef"}},
            { 0x86C, new c{ T="TagRef"}},
            { 0x888, new c{ T="TagRef"}}, // SND
            { 0x8A4, new c{ T="TagRef"}},
            { 0x8C0, new c{ T="TagRef"}}, // SND
            { 0x8DC, new c{ T="TagRef"}},

            { 0x8FC, new c{ T="Float"}},

            { 0x908, new c{ T="TagRef"}},
            { 0x924, new c{ T="TagRef"}},
            { 0x944, new c{ T="TagRef"}},
            { 0x984, new c{ T="TagRef"}},
            { 0x9A0, new c{ T="TagRef"}}, // JPT
            { 0x9BC, new c{ T="TagRef"}}, // JPT

            { 0x9DC, new c{ T="Tagblock"}},

            { 0x9F0, new c{ T="Float"}},

            { 0x9F4, new c{ T="TagRef"}}, //JPT
            { 0xA10, new c{ T="TagRef"}}, // JPT

            { 0xA2C, new c{ T="Float"}},
            { 0xA34, new c{ T="Float"}},
            { 0xA38, new c{ T="Float"}},
            { 0xA40, new c{ T="Float"}},
            { 0xA44, new c{ T="Float"}},
            { 0xA80, new c{ T="Float"}},

            { 0xAA0, new c{ T="TagRef"}},

            { 0xABC, new c{ T="Tagblock"}},
            { 0xAD0, new c{ T="Tagblock"}},
            { 0xAE4, new c{ T="Tagblock"}},
            { 0xAF8, new c{ T="Tagblock"}},
            { 0xB0C, new c{ T="Tagblock"}},

            { 0xB20, new c{ T="TagRef"}},

            { 0xB3C, new c{ T="Float"}},
            { 0xB40, new c{ T="Float"}},
            { 0xB44, new c{ T="Float"}},

            { 0xB48, new c{ T="Tagblock"}},
            { 0xB60, new c{ T="Tagblock"}},

            { 0xB8C, new c{ T="Float"}},
            { 0xB90, new c{ T="Float"}},

            { 0xBA0, new c{ T="TagRef"}}, // JPT
            { 0xBBC, new c{ T="TagRef"}},
            { 0xBDC, new c{ T="TagRef"}}, // EFFE
            { 0xBF5, new c{ T="TagRef"}}, // EFFE
        };
    }
}