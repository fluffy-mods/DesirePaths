using UnityEngine;
using Verse;

namespace DesirePaths
{
	public class Settings : ModSettings
	{
        public int pathDegradeThreshold;
        public int pathCreateThreshold;
        public float pathDegradeFactor = .9f;

        public void DoWindowContents( Rect canvas )
        {
            var options = new Listing_Standard();
            options.Begin( canvas );
            options.Label( $"Path creation threshold (current: {pathCreateThreshold}, default: 120, lower creates paths faster)" );
            pathCreateThreshold = (int) options.Slider( pathCreateThreshold, 50, 1000 );
            options.Gap();

            options.Label( $"Path degrade threshold (current: {pathDegradeThreshold}, default: 80, higher removes paths faster)" );
            pathDegradeThreshold = (int) options.Slider( pathDegradeThreshold, 0, pathCreateThreshold - 10 );
            options.Gap();

            options.Label( $"Path degrade factor (current: {pathDegradeFactor}, default: .9, lower degrades paths faster)" );
            pathDegradeFactor = options.Slider( pathDegradeFactor, .5f, .99f );
            options.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look( ref pathCreateThreshold, "pathCreateThreshold", 120 );
            Scribe_Values.Look( ref pathDegradeThreshold, "pathDegradeThreshold", 80 );
            Scribe_Values.Look( ref pathDegradeFactor, "pathDegradeFactor", .9f );
        }
	}
}