using UnityEngine;
using Verse;

namespace DesirePaths
{
    public class Settings : ModSettings
    {
        public int pathCreateThreshold = 120;
        public int pathDegradeThreshold = 80;
        public float pathDegradeFactor = .9f;
        public float snowClearFactor = .5f;
        public int stoneSmoothThreshold = 1000;

        public void DoWindowContents(Rect canvas)
        {
            var options = new Listing_Standard();
            options.Begin(canvas);
            options.Label( I18n.PathCreationThreshold( pathCreateThreshold, 120 ) );
            pathCreateThreshold = (int) options.Slider( pathCreateThreshold, 50, 1000 );
            options.Gap();

            options.Label( I18n.PathDegradeThreshold( pathDegradeThreshold, 80 ) );
            pathDegradeThreshold = (int) options.Slider( pathDegradeThreshold, 0, pathCreateThreshold - 10 );
            options.Gap();

            options.Label( I18n.PathDegradeFactor( 1 - pathDegradeFactor, .1f ) );
            pathDegradeFactor = options.Slider( pathDegradeFactor, .5f, .99f );
            options.Gap();

            options.Label( I18n.SnowClearFactor( snowClearFactor, .5f ) );
            snowClearFactor = options.Slider( snowClearFactor, 0f, 1f );
            options.Gap();

            options.Label( I18n.StoneSmoothThreshold( stoneSmoothThreshold, 2500 ) );
            stoneSmoothThreshold = (int) options.Slider( stoneSmoothThreshold, 100, 10000 );
            options.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref pathCreateThreshold, "pathCreateThreshold", 120);
            Scribe_Values.Look(ref pathDegradeThreshold, "pathDegradeThreshold", 80);
            Scribe_Values.Look(ref pathDegradeFactor, "pathDegradeFactor", .9f);
            Scribe_Values.Look(ref snowClearFactor, "snowClearFactor", .5f);
            Scribe_Values.Look(ref stoneSmoothThreshold, "stoneSmoothThreshold", 2500);
        }
    }
}