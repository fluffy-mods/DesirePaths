// Copyright Karel Kroeze, 2020-2021.
// DesirePaths/DesirePaths/Patch_DoPlaySettingsGlobalControls.cs

using HarmonyLib;
using RimWorld;
using Verse;

namespace DesirePaths {
    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    public static class Patch_DoPlaySettingsGlobalControls {
        public static void Postfix(WidgetRow row, bool worldView) {
            if (!worldView) {
                row.ToggleableIcon(ref DesireGrid.drawPaths, Resources.DrawPathsIcon, I18n.DrawPaths);
            }
        }
    }
}
