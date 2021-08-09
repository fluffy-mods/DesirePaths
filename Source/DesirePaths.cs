using HarmonyLib;
using UnityEngine;
using Verse;

namespace DesirePaths {
    public class DesirePaths: Mod {
        public DesirePaths(ModContentPack content) : base(content) {
            // initialize settings
            Settings = GetSettings<Settings>();

#if DEBUG
            Harmony.DEBUG = true;
#endif

            Harmony harmony = new Harmony( "Fluffy.DesirePaths" );
            harmony.PatchAll();
        }

        public static Settings Settings { get; private set; }

        public override void DoSettingsWindowContents(Rect inRect) {
            base.DoSettingsWindowContents(inRect);
            GetSettings<Settings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory() {
            return I18n.DesirePaths;
        }
    }
}
