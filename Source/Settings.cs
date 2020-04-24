using UnityEngine;
using Verse;

namespace DesirePaths
{
	public class Settings : ModSettings
	{
        public int pathDegradeThreshold;
        public int pathCreateThreshold;
        public float pathDegradeFactor = .9f;
        public string pathDegradeThresholdBuffer;
        public string pathCreateThresholdBuffer;
        public string pathDegradeFactorBuffer;

		public void DoWindowContents(Rect canvas)
		{
			var options = new Listing_Standard();
			options.Begin(canvas);
            options.TextFieldNumericLabeled( "Path creation threshold (default: 120, lower creates paths faster)", ref pathCreateThreshold, ref pathCreateThresholdBuffer, 50, 1000 );
            options.TextFieldNumericLabeled( "Path degrade threshold (default: 80, higher removes paths faster)", ref pathDegradeThreshold, ref pathDegradeThresholdBuffer, 0, pathCreateThreshold - 10 );
            options.TextFieldNumericLabeled( "Path degrade factor (default: .9, lower degrades paths faster)", ref pathDegradeFactor, ref pathDegradeFactorBuffer, .6f, .99f );
			options.End();
		}
		
		public override void ExposeData()
        {
            Scribe_Values.Look( ref pathCreateThreshold, "pathCreateThreshold", 120 );
            Scribe_Values.Look( ref pathDegradeThreshold, "pathDegradeThreshold", 80 );
        }
	}
}