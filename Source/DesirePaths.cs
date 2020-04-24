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
        }

        public static Settings Settings { get; private set; }

        public override void DoSettingsWindowContents( Rect inRect )
        {
            base.DoSettingsWindowContents( inRect );
            GetSettings<Settings>().DoWindowContents( inRect );
        }

        public override string SettingsCategory()
        {
            return "Desire Paths";
        }
    }
}