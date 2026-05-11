using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using HexRailway.Core;
using HexRailway.Core.Localization;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.IO;
using UnityEngine;

namespace HexRailway
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "hex.railways";
        private const string PluginName = "HexRailways";
        private const string PluginVersion = "1.0.0";
        private ConfigEntry<bool> _pluginEnabled;
        private AssetBundle _assetBundle;

        internal static Plugin Instance { get; private set; }
        internal static Harmony HarmonyInstance { get; private set; }
        internal bool IsPluginEnabled => _pluginEnabled.Value;
        internal AssetBundle AssetBundle => _assetBundle;

        private void Awake()
        {
            Instance = this;

            _pluginEnabled = Config.Bind("General", "Enabled", true, "Enable or disable the HexRailways plugin.");
            _pluginEnabled.SettingChanged += OnPluginEnabled;

            HarmonyInstance = new Harmony(PluginGuid);
            HarmonyInstance.PatchAll();

            LocalizationManager.OnLocalizationAdded += LocalizationRegistrar.AddLocalization;

            string modPath = Path.GetDirectoryName(Info.Location);
            string assetBundlePath = Path.Combine(modPath, "hexrailways");

            _assetBundle = AssetUtils.LoadAssetBundle(assetBundlePath);

            if (_assetBundle == null)
            {
                Jotunn.Logger.LogError("Failed to load HexRailways asset bundle.");
                return;
            }

            Jotunn.Logger.LogInfo("HexRailways asset bundle loaded.");

            PrefabManager.OnVanillaPrefabsAvailable += HexRailwaysRegistrar.RegisterItems;

            Jotunn.Logger.LogInfo($"{PluginName} v{PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= HexRailwaysRegistrar.RegisterItems;
            LocalizationManager.OnLocalizationAdded -= LocalizationRegistrar.AddLocalization;

            if (_pluginEnabled != null)
            {
                _pluginEnabled.SettingChanged -= OnPluginEnabled;
            }

            if(_assetBundle != null)
            {
                _assetBundle.Unload(true);
                _assetBundle = null;
            }

            HarmonyInstance?.UnpatchSelf();
            HarmonyInstance = null;
            Instance = null;
        }

        private void OnPluginEnabled(object sender, EventArgs e)
        {
            Jotunn.Logger.LogInfo($"HexRailways plugin enabled: {IsPluginEnabled}");
        }
    }
}
