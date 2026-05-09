using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;

namespace HexRailway
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "hex.railways";
        private const string PluginName = "HexRailways";
        private const string PluginVersion = "1.0.0";
        private ConfigEntry<bool> _pluginEnabled;

        internal static Plugin Instance { get; private set; }
        internal static Harmony HarmonyInstance { get; private set; }
        internal bool IsPluginEnabled => _pluginEnabled.Value;

        private void Awake()
        {
            Instance = this;
            _pluginEnabled = Config.Bind("General", "Enabled", true, "Enable or disable the HexRailways plugin.");
            _pluginEnabled.SettingChanged += OnPluginEnabled;

            HarmonyInstance = new Harmony(PluginGuid);
            HarmonyInstance.PatchAll();
        }

        private void OnDestroy()
        {
            if(_pluginEnabled != null)
            {
                _pluginEnabled.SettingChanged -= OnPluginEnabled;
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
