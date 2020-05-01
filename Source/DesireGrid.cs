// DesireGrid.cs
// Copyright Karel Kroeze, 2020-2020

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DesirePaths
{
    public class DesireGrid : MapComponent, ICellBoolGiver
    {
        private float[] walkGrid;
        private float[] stoneGrid;
        private TerrainDef[] originalTerrain;
        private CellBoolDrawer pathsDrawer;

        public static bool drawPaths = true;

        public DesireGrid(Map map) : base(map)
        {
            var n = map.cellIndices.NumGridCells;
            walkGrid = new float[n];
            stoneGrid = new float[n];
            originalTerrain = new TerrainDef[n];

            for (var i = 0; i < n; i++)
            {
                walkGrid[i] = 0;
                stoneGrid[i] = 0;
            }

            pathsDrawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 2610, .5f);
        }

        public bool IsPackable(TerrainDef terrain)
        {
            return terrain.takeFootprints;
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            if (GenTicks.TicksGame % 20 == 0) DoWalkGridTick();
            if (GenTicks.TicksGame % (GenDate.TicksPerDay / 12) == 0) DoUpdateTick();
            if (GenTicks.TicksGame % (GenDate.TicksPerHour) == 0) DoPathDrawerUpdate();
        }

        public override void MapComponentUpdate()
        {
            if (drawPaths)
                pathsDrawer.MarkForDraw();
            pathsDrawer.CellBoolDrawerUpdate();
        }

        public void DoWalkGridTick()
        {
            foreach (var pawn in map.mapPawns.AllPawnsSpawned.Where(p => !p.Dead && !p.Downed && p.Awake()))
            {
                var weight = 1f;
                if (pawn.RaceProps?.Animal ?? false)
                    weight *= 1 / 4f;
                weight *= pawn.BodySize;
                if (!pawn.pather.MovingNow)
                    weight *= 1 / 10f;
                var index = map.cellIndices.CellToIndex(pawn.Position);
                walkGrid[index] += weight;
                stoneGrid[index] += weight;

                // also remove snow where we're walking, directly on the snowgrid
                map.snowGrid.AddDepth(pawn.Position, -.02f * DesirePaths.Settings.snowClearFactor * pawn.BodySize);

#if DEBUG
                map.debugDrawer.FlashCell(pawn.Position, weight);
#endif
            }
        }

        public void DoUpdateTick()
        {
            for (var i = 0; i < map.cellIndices.NumGridCells; i++)
            {
                if (walkGrid[i] > DesirePaths.Settings.pathCreateThreshold)
                    TryCreatePath(i);
                walkGrid[i] *= DesirePaths.Settings.pathDegradeFactor;
                if (walkGrid[i] < DesirePaths.Settings.pathDegradeThreshold)
                    TryRemovePath(i);
                if (stoneGrid[i] > DesirePaths.Settings.stoneSmoothThreshold)
                    TrySmooth(i);
            }

        }

        public void DoPathDrawerUpdate()
        {
            // walkGrid updated, update paths drawer
            var max = walkGrid.Max();
            walkThreshold = max * .05f;
            walkMax = max * .8f;
            pathsDrawer.SetDirty();
        }

        public void TryCreatePath(int index)
        {
            var cell = map.cellIndices.IndexToCell(index);
            var terrain = cell.GetTerrain(map);
            if (terrain == TerrainDefOf.PackedDirt || !IsPackable(terrain))
                return;

            originalTerrain[index] = terrain;
            map.terrainGrid.SetTerrain(cell, TerrainDefOf.PackedDirt);
        }

        public void TryRemovePath(int index)
        {
            if (originalTerrain[index] == null)
                return;

            var cell = map.cellIndices.IndexToCell(index);
            var terrain = cell.GetTerrain(map);
            if (terrain != TerrainDefOf.PackedDirt)
                return;

            map.terrainGrid.SetTerrain(cell, originalTerrain[index]);
            originalTerrain[index] = null;
        }

        public void TrySmooth(int index)
        {
            var cell = map.cellIndices.IndexToCell(index);
            var terrain = cell.GetTerrain(map);
            if (terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))
                map.terrainGrid.SetTerrain(cell, terrain.smoothedTerrain);
        }

        public void DebugDraw()
        {
            Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
            Log.Message($"walk: min: {walkGrid.Min()}, max: {walkGrid.Max()}, avg: {walkGrid.Average()}");
            foreach (var cell in map.AllCells)
                map.debugDrawer.FlashCell(cell, walkGrid[map.cellIndices.CellToIndex(cell)]);
        }

#if DEBUG
        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();

            var pos = UI.MouseCell();
            Widgets.Label(new Rect(10, Screen.height * 1 / 3f, 300, 300),
                           $"x: {pos.x}, y: {pos.z}\n" +
                           $"walk: {walkGrid[map.cellIndices.CellToIndex(pos)]}\n" +
                           $"pathCost: {pos.GetTerrain(map).pathCost}\n");

            if (KeyBindingDefOf.Accept.KeyDownEvent)
                DebugDraw();
        }
#endif

        private static Dictionary<ushort, TerrainDef> terrainsByHash =
            DefDatabase<TerrainDef>.AllDefsListForReading.ToDictionary(d => d.shortHash, d => d);

        public Color Color => GenUI.MouseoverColor;

        public override void ExposeData()
        {
            base.ExposeData();

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                walkGrid = new float[map.cellIndices.NumGridCells];
                stoneGrid = new float[map.cellIndices.NumGridCells];
                originalTerrain = new TerrainDef[map.cellIndices.NumGridCells];
            }

            MapExposeUtility.ExposeUshort(
                map, (cell) => (ushort)(walkGrid[map.cellIndices.CellToIndex(cell)] * 100),
                (cell, val) => walkGrid[map.cellIndices.CellToIndex(cell)] = val / 100f,
                "walkGrid");

            MapExposeUtility.ExposeUshort(
                map, ( cell ) => (ushort) ( stoneGrid[map.cellIndices.CellToIndex( cell )] * 100 ),
                ( cell, val ) => stoneGrid[map.cellIndices.CellToIndex( cell )] = val / 100f,
                "stoneGrid");

            MapExposeUtility.ExposeUshort(
                map, (cell) => originalTerrain[map.cellIndices.CellToIndex(cell)]?.shortHash ?? 0,
                (cell, val) =>
                {
                    if (val != 0 && terrainsByHash.TryGetValue(val, out TerrainDef terrain))
                        originalTerrain[map.cellIndices.CellToIndex(cell)] = terrain;

                }, "originalTerrain");
        }

        private float walkThreshold = float.MaxValue;
        private float walkMax = float.MaxValue;
        public bool GetCellBool(int index)
        {
            return walkGrid[index] > walkThreshold;
        }

        public Color GetCellExtraColor(int index)
        {
            var color = Color.white;
            color.a = Mathf.Clamp01(walkGrid[index] / walkMax);
            return color;
        }
    }
}