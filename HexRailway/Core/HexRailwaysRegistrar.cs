using HexRailway.Core.Localization;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System;
using System.Runtime.CompilerServices;

namespace HexRailway.Core
{
    internal static class HexRailwaysRegistrar
    {
        private static bool _registered;
        private const string PlaceVfx = "vfx_Place_wood_beam";
        private const string PlaceSfx = "sfx_build_hammer_wood";
        private const string DestroyedSfx = "sfx_wood_destroyed";
        private const string SawDustVfx = "vfx_SawDust";

        internal static void RegisterItems()
        {
            if (_registered || Plugin.Instance == null || Plugin.Instance.AssetBundle == null)
            {
                return;
            }

            RegisterRecipePrefabs();
            var registeredCount = RegisterWoodRailSection();

            _registered = true;
            PrefabManager.OnVanillaPrefabsAvailable -= RegisterItems;

            Jotunn.Logger.LogInfo($"HexRailways registered {registeredCount} custom piece(s).");
        }

        

        private static int RegisterWoodRailSection()
        {
            var customPiece = new CustomPiece(
                Plugin.Instance.AssetBundle,
                HexRailwayPrefabNames.WoodRailSection,
                fixReference: true,
                CreateWoodRailSectionConfig());

            ConfigurePrefab(customPiece.PiecePrefab);

            return PieceManager.Instance.AddPiece(customPiece) ? 1 : 0;
        }

        private static PieceConfig CreateWoodRailSectionConfig()
        {
            var config = new PieceConfig
            {
                Name = $"{LocalizationRegistrar.TokenPrefix}wood_rail_section",
                Description = $"{LocalizationRegistrar.TokenPrefix}wood_rail_section_desc",
                PieceTable = PieceTables.Hammer,
                Category = HexRailwayPrefabNames.HexRailwayCategory
            };

            config.AddRequirement("Wood", 4, true);
            config.AddRequirement("hex_railway_flint_nails", 4, true);
            config.AddRequirement("Stone", 2, true);

            return config;
        }

        private static void ConfigurePrefab(UnityEngine.GameObject prefab)
        {
            var piece = prefab.GetComponent<Piece>();
            var wearNTear = prefab.GetComponent<WearNTear>();

            if (piece == null)
            {
                Jotunn.Logger.LogError($"Piece component is NULL on {prefab.name}");
            }
            else
            {
                // Force no crafting station requirement because the Unity prefab may still carry a serialized reference.
                piece.m_craftingStation = null;
                SetPlaceEffects(piece);
            }

            if (wearNTear == null)
            {
                Jotunn.Logger.LogError($"WearNTear component is NULL on {prefab.name}");
            }
            else
            {
                SetWearNTearEffects(wearNTear);
            }
        }

        private static void SetPlaceEffects(Piece piece)
        {
            piece.m_placeEffect.m_effectPrefabs = new[]
            {
                CreateEffect(PlaceVfx),
                CreateEffect(PlaceSfx)
            };
        }

        private static void SetWearNTearEffects(WearNTear wearNTear)
        {
            wearNTear.m_destroyedEffect.m_effectPrefabs = new[]
            {
                CreateEffect(DestroyedSfx),
                CreateEffect(SawDustVfx)
            };

            wearNTear.m_hitEffect.m_effectPrefabs = new[]
            {
                CreateEffect(SawDustVfx)
            };
        }

        private static EffectList.EffectData CreateEffect(string prefabName)
        {
            var prefab = PrefabManager.Instance.GetPrefab(prefabName);

            if (prefab == null)
            {
                Jotunn.Logger.LogWarning($"Effect prefab not found: {prefabName}");
            }

            return new EffectList.EffectData
            {
                m_prefab = prefab,
                m_enabled = prefab != null,
                m_variant = -1
            };
        }

        // We'll make this generic later to create all nail recipes
        private static void AddFlintNails()
        {
            ItemConfig flintNails = new ItemConfig();
            flintNails.Name = $"{LocalizationRegistrar.TokenPrefix}item_flint_nails";
            flintNails.Description = $"{LocalizationRegistrar.TokenPrefix}item_flint_nails_desc";
            flintNails.CraftingStation = CraftingStations.Workbench;
            flintNails.AddRequirement("Flint", 2);
            flintNails.AddRequirement("Wood", 1);
            flintNails.Weight = 0.1f;
            flintNails.Amount = 10;
            flintNails.StackSize = 200;

            CustomItem flintNailsItem = new CustomItem("hex_railway_flint_nails", "BronzeNails", flintNails);
            var addNails = ItemManager.Instance.AddItem(flintNailsItem);

            if (addNails)
            {
                Jotunn.Logger.LogInfo("Flint nails item registered successfully.");
            }
            else
            {
                Jotunn.Logger.LogError("Failed to register flint nails item.");
            }
        }

        private static void RegisterRecipePrefabs()
        {
            AddFlintNails();
        }
    }
}