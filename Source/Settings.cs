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
            options.Label($"Path creation threshold (current: {pathCreateThreshold}, default: 120, lower creates paths faster)");
            pathCreateThreshold = (int)options.Slider(pathCreateThreshold, 50, 1000);
            options.Gap();

            options.Label($"Path degrade threshold (current: {pathDegradeThreshold}, default: 80, higher removes paths faster)");
            pathDegradeThreshold = (int)options.Slider(pathDegradeThreshold, 0, pathCreateThreshold - 10);
            options.Gap();

            options.Label($"Path degrade factor (current: {pathDegradeFactor:N2}, default: .9, lower degrades paths faster)");
            pathDegradeFactor = options.Slider(pathDegradeFactor, .5f, .99f);
            options.Gap();

            options.Label($"Snow path creation speed (current: {snowClearFactor:P}, default: 50%, higher removes snow quicker)");
            snowClearFactor = options.Slider(snowClearFactor, 0f, 1f);
            options.Gap();

            options.Label($"How long does it take for rough stone to become smoothed with use? (current: {stoneSmoothThreshold}, default: 1000, higher takes longer)");
            stoneSmoothThreshold = (int)options.Slider(stoneSmoothThreshold, 100, 10000);
            options.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref pathCreateThreshold, "pathCreateThreshold", 120);
            Scribe_Values.Look(ref pathDegradeThreshold, "pathDegradeThreshold", 80);
            Scribe_Values.Look(ref pathDegradeFactor, "pathDegradeFactor", .9f);
            Scribe_Values.Look(ref snowClearFactor, "snowClearFactor", .5f);
            Scribe_Values.Look(ref stoneSmoothThreshold, "stoneSmoothThreshold", 1000);
        }
    }
}