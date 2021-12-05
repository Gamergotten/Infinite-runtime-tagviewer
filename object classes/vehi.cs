using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembly69.object_classes
{
    static class vehi
    {
        public static Dictionary<long, string> VehicleTag = new Dictionary<long, string>
        {
            { 0, "Pointer" },
            { 8, "4Byte" }, // datnum
            { 12, "4Byte" }, // tagID

            { 40, "Float" },
            { 44, "Float" },
            { 48, "Float" },
            { 52, "Float" },
            { 56, "Float" },
            { 60, "Float" },
            { 64, "Float" },
            { 76, "Float" },
            { 80, "Float" },
            { 84, "Float" },

            { 92, "Tagblock" }, // SidecarPathDefinition

            { 112, "Float" },
            { 116, "Float" },

            { 120, "TagRef" }, // vehicle model
            { 148, "TagRef" }, // aset tag ref
            { 176, "TagRef" }, 
            { 204, "TagRef" },

            { 232, "4Byte" },

            { 240, "TagRef" },

            { 276, "Float" },

            { 288, "TagRef" },
            { 316, "TagRef" }, // foot tag ref
            { 344, "TagRef" }, // vemd tag ref
            { 372, "TagRef" }, // smed tag ref
            { 400, "TagRef" },

            { 432, "Float" },

            { 448, "Tagblock" }, // object_ai_properties
            { 468, "Tagblock" }, // s_object_function_definition
            { 488, "4Byte" },
            { 492, "Tagblock" }, // ObjectRuntimeInterpolatorFunctionsBlock
            { 512, "Tagblock" }, // ObjectFunctionSwitchDefinition
            { 532, "Tagblock" }, // i343::Objects::ObjectFunctionForwarding
            { 552, "4Byte" },
            { 556, "Tagblock" }, // i343::Objects::AmmoRefillVariant
            { 576, "4Byte" },
            { 584, "Tagblock" }, // object_attachment_definition
            { 604, "Tagblock" }, // object_indirect_lighting_settings_definition
            { 624, "Tagblock" }, // s_water_physics_hull_surface_definition
            { 644, "Tagblock" }, // s_jetwash_definition
            { 664, "Tagblock" }, // object_definition_widget
            { 684, "Tagblock" }, // object_change_color_definition
            { 704, "Tagblock" }, // s_multiplayer_object_properties_definition
            { 724, "Tagblock" }, // i343::Objects::ForgeObjectEntryDefinition

            { 744, "TagRef" },
            { 772, "TagRef" },

            { 800, "Tagblock" }, // s_object_spawn_effects
            { 820, "Tagblock" }, // ModelDissolveDataBlock

            { 840, "String" }, // (vehicle_partial_emp)

            { 0x448, "Tagblock" }, // HsReferencesBlock
            { 0x45C, "TagRef" },

            { 0x480, "Float" },

            { 0x484, "Tagblock" }, // metalabel somethiong ?? recheck this one
            { 0x498, "Tagblock" }, // SoundRTPCBlockDefinition
            { 0x4AC, "Tagblock" }, // SoundSweetenerBlockDefinition

            { 0x4C0, "Pointer" },

            { 0x4C8, "Tagblock" }, // i343::Objects::ComputeFunctionSmoothingBlockDefinition

            { 0x4DC, "Float" },
            { 0x4E0, "Float" },

            { 0x4E4, "TagRef" },
            { 0x500, "Tagblock" }, // i343::SpartanTracking::ObjectDefinition

            { 0x514, "TagRef" },
            { 0x530, "Tagblock" }, // InteractionOpportunityDefinition

            { 0x544, "Float" },
            { 0x548, "Tagblock" }, // ScriptedSequenceActionDefinition
            { 0x55C, "TagRef" },
            { 0x578, "Tagblock" }, // AnimChannelEntry
            { 0x58C, "Tagblock" }, // AnimSetTableEntry

            { 0x5A0, "Float" },
            { 0x5A4, "Float" },
            { 0x5A8, "Float" },
            { 0x5AC, "Float" },
            { 0x5B0, "Float" },
            { 0x5B4, "Float" },
            { 0x5B8, "Float" },
            { 0x5BC, "Float" },
            { 0x5C0, "Float" },
            { 0x5CC, "4Byte" },
            { 0x5DC, "Float" },
            { 0x5E0, "Float" },
            { 0x5E4, "Float" },

            { 0x5E8, "Tagblock" }, // LegGroundingSettings

            { 0x5FC, "Float" },
            { 0x600, "Float" },
            { 0x604, "Float" },
            { 0x608, "Float" },

            { 0x60C, "Tagblock" }, // i343::Objects::ObjectNodeGraphDefinition
            { 0x620, "Tagblock" }, // i343::Objects::AnimationMatchingTableEntry

            { 0x634, "Float" },

            { 0x638, "Tagblock" }, // i343::Objects::ModelVariantSwappingTableEntry

            { 0x63C, "Float" },

            { 0x650, "Tagblock" }, // i343::Items::LocationSensorDefinition
            { 0x664, "Tagblock" }, // i343::Items::ShroudGeneratorDefinition
            { 0x678, "Tagblock" }, // i343::Objects::PowerComponentDefinition
            { 0x68C, "Tagblock" }, // i343::Objects::SelfDestructHandlerDefinition
            { 0x6A0, "Tagblock" }, // i343::Objects::IndirectLightingComponentDefinition

            { 0x6B4, "Float" },
            { 0x6B8, "4Byte" },
            { 0x6BC, "4Byte" },

            { 0x6C4, "TagRef" },
            { 0x6E0, "Tagblock" }, // s_campaign_metagame_bucket
            { 0x6F4, "Tagblock" }, // s_unit_screen_effect_definition

            { 0x70C, "Float" },
            { 0x720, "Tagblock" }, // s_unit_camera_track
            { 0x768, "Tagblock" }, // s_unit_camera_acceleration
            { 0x77C, "Float" },
            { 0x790, "Tagblock" }, // s_unit_camera_track
            { 0x7D8, "Tagblock" }, // s_unit_camera_acceleration

            { 0x7F0, "TagRef" },

            { 0x818, "Float" },
            { 0x81C, "Float" },
            { 0x830, "Float" },

            { 0x870, "TagRef" },

            { 0x898, "Float" },

            { 0x8BC, "Tagblock" }, // WeaponSpecificMarkers
            { 0x8D0, "TagRef" },
            { 0x908, "TagRef" },
            { 0x924, "TagRef" },
            { 0x940, "TagRef" },
            { 0x95C, "TagRef" },
            { 0x978, "TagRef" },
            { 0x994, "TagRef" },
            { 0x9B0, "TagRef" },

            { 0x9D0, "Float" },

            { 0x9E0, "Tagblock" }, // HudUnitSoundDefinitions
            { 0x9F4, "Tagblock" }, // dialogue_variant_definition

            { 0xA30, "Float" },
            { 0xA34, "Float" },
            { 0xA38, "Float" },

            { 0xA64, "Tagblock" }, // powered_seat_definition
            { 0xA78, "Tagblock" }, // unit_initial_weapon
            { 0xA8C, "Tagblock" }, // s_target_tracking_parameters
            { 0xAA0, "Tagblock" }, // unit_seat

            { 0xAB4, "Float" },
            { 0xAB8, "Float" },
            { 0xABC, "Float" },

            { 0xAC8, "TagRef" },
            { 0xAE4, "Tagblock" }, // i343::Objects::PowerComponentDefinition
            { 0xAF8, "TagRef" },
            { 0xB14, "TagRef" },

            { 0xB38, "Float" },
            { 0xB3C, "Float" },
            { 0xB40, "Float" },
            { 0xB44, "Float" },
            { 0xB48, "Float" },
            { 0xB4C, "Float" },

            { 0xB54, "Pointer" },
            { 0xB5C, "Pointer" },

            { 0xB7C, "TagRef" },
            { 0xB98, "TagRef" },

            { 0xBB4, "Tagblock" }, // ExitAndDetachVariant

            { 0xBCC, "Float" },

            { 0xBE4, "TagRef" },

            { 0xC00, "Float" },
            { 0xC04, "Float" },
            { 0xC08, "Float" },
            { 0xC0C, "Float" },
            { 0xC10, "Float" },
            { 0xC18, "Float" },
            { 0xC1C, "Float" },
            { 0xC28, "Float" },
            { 0xC30, "Float" },
            { 0xC34, "Float" },
            { 0xC3C, "Float" },
            { 0xC40, "Float" },
            { 0xC5C, "Float" },
            { 0xC64, "Float" },
            { 0xC68, "Float" },
            { 0xC70, "Float" },
            { 0xC74, "Float" },
            { 0xC7C, "Float" },
            { 0xC80, "Float" },
            { 0xC84, "Float" },
            { 0xC88, "Float" },
            { 0xC8C, "Float" },
            { 0xC94, "Float" },
            { 0xC98, "Float" },
            { 0xCA0, "Float" },
            { 0xCAC, "Float" },
            { 0xCB8, "Float" },
            { 0xCBC, "Float" },
            { 0xCC4, "Float" },
            { 0xCC8, "Float" },
            { 0xCD0, "Float" },
            { 0xCD4, "Float" },
            { 0xCDC, "Float" },
            { 0xCE0, "Float" },
            { 0xCE8, "Float" },
            { 0xCF4, "Float" },
            { 0xCF8, "Float" },
            { 0xD00, "Float" },
            { 0xD04, "Float" },
            { 0xD0C, "Float" },
            { 0xD10, "Float" },
            { 0xD18, "Float" },
            { 0xD1C, "Float" },
            { 0xD24, "Float" },

            { 0xD28, "TagRef" },
            { 0xD44, "TagRef" }, // EFFECT TAG REF
            { 0xD64, "TagRef" },
            { 0xD80, "TagRef" }, // VEHI TAG REF

            { 0xDA0, "Tagblock" }, // s_vehicle_human_tank_definition
            { 0xDB4, "Tagblock" }, // s_vehicle_human_jeep_definition
            { 0xDC8, "Tagblock" }, // s_vehicle_human_plane_definition
            { 0xDDC, "Tagblock" }, // s_vehicle_alien_scout_definition
            { 0xDF0, "Tagblock" }, // s_vehicle_alien_fighter_definition
            { 0xE04, "Tagblock" }, // s_vehicle_turret_definition
            { 0xE18, "Tagblock" }, // s_vehicle_vtol_definition
            { 0xE2C, "Tagblock" }, // s_vehicle_chopper_definition
            { 0xE40, "Tagblock" }, // s_vehicle_guardian_definition
            { 0xE54, "Tagblock" }, // s_vehicle_jackal_glider_definition
            { 0xE68, "Tagblock" }, // s_vehicle_space_fighter_definition
            { 0xE7C, "Tagblock" }, // s_vehicle_revenant_definition

            { 0xE90, "Float" },
            { 0xE94, "4Byte" },

            { 0xE98, "Tagblock" }, // i343::Vehicles::AntiGravityPointConfiguration
            { 0xEAC, "Tagblock" }, // i343::Vehicles::AntiGravityPointDefinition
            { 0xEC0, "Tagblock" }, // i343::Vehicles::FrictionPointConfiguration
            { 0xED4, "Tagblock" }, // i343::Vehicles::FrictionPointDefinition

            { 0xEE8, "Float" },
            { 0xEEC, "Float" },

            { 0xEF0, "Pointer" },
            { 0xEF8, "Pointer" },
            { 0xF08, "Pointer" },
            { 0xF10, "Pointer" },

            { 0xF2C, "Tagblock" }, // s_unit_trick_definition
            { 0xF40, "Float" },
            { 0xF50, "Float" },
            { 0xF54, "Float" },
            { 0xF58, "Float" },
            { 0xF60, "Float" },
            { 0xF74, "4Byte" },

            { 0xF78, "TagRef" },
            { 0xF94, "TagRef" },
            { 0xFB0, "TagRef" },
            { 0xFD0, "TagRef" },
            { 0xFF0, "TagRef" },
            { 0x1024, "Tagblock" }, // SoundRTPCBlockDefinition
            { 0x1038, "Tagblock" }, // SoundSweetenerBlockDefinition

            { 0x104C, "TagRef" },
            { 0x1068, "TagRef" },
            { 0x1084, "TagRef" },

            { 0x10A4, "Tagblock" }, // s_vehicleAiCruiseControl
            { 0x10B8, "Tagblock" }, // Interface::UIItemInfo

            { 0x10CC, "TagRef" },
            { 0x10EC, "Float" },

        };

        public static Dictionary<long, string> WeaponTag = new Dictionary<long, string>
        {
            { 0, "Pointer" },
            { 8, "4Byte" }, // datnum
            { 12, "4Byte" }, // tagID

            { 0x05C, "Tagblock" },
            { 0x078, "TagRef" }, // HLMT
            { 0x094, "TagRef" },
            { 0x0B0, "TagRef" },
            { 0x0CC, "TagRef" },
            { 0x0F0, "TagRef" },

            { 0x10C, "Tagblock" },

            { 0x120, "TagRef" },
            { 0x13C, "TagRef" }, // FOOT
            { 0x158, "TagRef" }, // VMED
            { 0x174, "TagRef" }, // SMED
            { 0x190, "TagRef" },

            { 0x1C0, "Tagblock" },
            { 0x1D4, "Tagblock" },
            { 0x1EC, "Tagblock" },
            { 0x200, "Tagblock" },
            { 0x214, "Tagblock" },
            { 0x22C, "Tagblock" },
            { 0x248, "Tagblock" },
            { 0x25C, "Tagblock" },
            { 0x270, "Tagblock" },
            { 0x284, "Tagblock" },
            { 0x298, "Tagblock" },
            { 0x2AC, "Tagblock" },
            { 0x2C0, "Tagblock" },
            { 0x2D4, "Tagblock" },

            { 0x2E8, "TagRef" },
            { 0x304, "TagRef" },

            { 0x320, "Tagblock" },
            { 0x334, "Tagblock" },
            { 0x448, "Tagblock" },

            { 0x45C, "TagRef" },

            { 0x484, "Tagblock" },
            { 0x498, "Tagblock" },
            { 0x4AC, "Tagblock" },
            { 0x4C8, "Tagblock" },

            { 0x4E4, "TagRef" },

            { 0x500, "Tagblock" },
            { 0x514, "TagRef" },

            { 0x530, "Tagblock" },
            { 0x548, "Tagblock" },

            { 0x55C, "TagRef" },

            { 0x578, "Tagblock" },
            { 0x58C, "Tagblock" },
            { 0x5E8, "Tagblock" },
            { 0x60C, "Tagblock" },
            { 0x620, "Tagblock" },
            { 0x638, "Tagblock" },
            { 0x650, "Tagblock" },
            { 0x664, "Tagblock" },
            { 0x678, "Tagblock" },
            { 0x68C, "Tagblock" },
            { 0x6A0, "Tagblock" },
            { 0x730, "Tagblock" },
            { 0x744, "Tagblock" },

            { 0x758, "TagRef" },
            { 0x77C, "TagRef" },
            { 0x798, "TagRef" },
            { 0x7D8, "TagRef" },
            { 0x7F4, "TagRef" },
            { 0x850, "TagRef" }, // WEAP
            { 0x88C, "TagRef" },
            { 0x8A8, "TagRef" },
            { 0x924, "TagRef" },
            { 0x940, "TagRef" },
            { 0x95C, "TagRef" },
            { 0x978, "TagRef" },

            { 0x994, "Tagblock" },

            { 0x9A8, "TagRef" },
            { 0x9D4, "TagRef" },

            { 0x9F0, "Tagblock" },
            { 0xA80, "Tagblock" },
            { 0xA94, "Tagblock" },

            { 0xAF0, "TagRef" },
            { 0xB0C, "TagRef" },

            { 0xB6C, "Tagblock" },
            { 0xB88, "Tagblock" },
            { 0xBA4, "Tagblock" },

            { 0xBB8, "TagRef" },
            { 0xBE4, "TagRef" },
            { 0xC00, "TagRef" },

            { 0xC1C, "Tagblock" },

            { 0xC30, "TagRef" }, // BITM

            { 0xC4C, "Tagblock" },

            { 0xC60, "TagRef" },
            { 0xC7C, "Tagblock" },
            { 0xC90, "Tagblock" },

            { 0xCBC, "TagRef" },
            { 0xCD8, "TagRef" },
            { 0xCF4, "TagRef" },
            { 0xD10, "TagRef" },

            { 0xDB4, "Tagblock" },
            { 0xDC8, "TagRef" },
            { 0xDE8, "Tagblock" },

            { 0xE00, "TagRef" },
            { 0xE1C, "TagRef" },
            { 0xE38, "TagRef" },
            { 0xE54, "TagRef" },
            { 0xE70, "TagRef" },
            { 0xE8C, "TagRef" },
            { 0xEA8, "TagRef" },
            { 0xEC4, "TagRef" },

            { 0xF80, "Tagblock" },
            { 0xFBC, "Tagblock" },

            { 0x1074, "TagRef" },
            { 0x109C, "TagRef" },
            { 0x10BC, "Tagblock" },
            { 0x10D0, "TagRef" },
            { 0x10EC, "TagRef" },
            { 0x1108, "TagRef" },

            { 0x1128, "Tagblock" },
            { 0x113C, "Tagblock" },

        };

    }
}
