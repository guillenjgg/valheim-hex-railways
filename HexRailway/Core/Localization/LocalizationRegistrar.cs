using Jotunn.Managers;
using System.Collections.Generic;

namespace HexRailway.Core.Localization
{
    internal static class LocalizationRegistrar
    {
        internal const string TokenPrefix = "$hex_railways_";
        internal const string KeyPrefix = "hex_railways_";

        internal static void AddLocalization()
        {
            var localization = LocalizationManager.Instance.GetLocalization();

            localization.AddTranslation("English", new Dictionary<string, string>
            {
                {$"{KeyPrefix}wood_rail_section", "Wood Rail Section" },
                {$"{KeyPrefix}wood_rail_section_desc", "A wooden railway section for basic mine cart tracks." },
                {$"{KeyPrefix}item_flint_nails", "Flint Nails" },
                {$"{KeyPrefix}item_flint_nails_desc", "Nails made of flint for railway construction." }
            });
        }
    }
}
