using System.Collections.Generic;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace DesirePaths
{
    public class DesirePaths : Mod
    {
        public DesirePaths( ModContentPack content ) : base( content )
        {
            // initialize settings
            Settings = GetSettings<Settings>();

#if DEBUG
            Harmony.DEBUG = true;
#endif

            var harmony = new Harmony( "Fluffy.DesirePaths" );
            harmony.PatchAll();
        }

        public static Settings Settings { get; private set; }

        public override void DoSettingsWindowContents( Rect inRect )
        {
            base.DoSettingsWindowContents( inRect );
            GetSettings<Settings>().DoWindowContents( inRect );
        }

        public override string SettingsCategory() => I18n.DesirePaths;
    }
}