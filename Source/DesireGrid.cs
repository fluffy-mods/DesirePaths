// DesireGrid.cs
// Copyright Karel Kroeze, 2020-2020

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DesirePaths {
    public class DesireGrid: MapComponent, ICellBoolGiver {
        private float[] walkGrid;
        private float[] stoneGrid;
        private TerrainDef[] originalTerrain;
        private readonly CellBoolDrawer pathsDrawer;

        public static bool drawPaths;

        public DesireGrid(Map map) : base(map) {
            int n = map.cellIndices.NumGridCells;
            walkGrid = new float[n];
            stoneGrid = new float[n];
            originalTerrain = new TerrainDef[n];

            for (int i = 0; i < n; i++) {
                walkGrid[i] = 0;
                stoneGrid[i] = 0;
            }

            pathsDrawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 2610, .5f);
        }

        public bool IsPackable(TerrainDef terrain) {
            DefModExtension_PackedTerrain settings = terrain.GetModExtension<DefModExtension_PackedTerrain>();
            if (settings != null) {
                return !settings.disabled
                    && terrain != settings.packedTerrain;
            }
            return terrain != TerrainDefOf.PackedDirt && terrain.takeFootprints;
        }

        public TerrainDef PackedTerrain(TerrainDef terrain) {
            return terrain.GetModExtension<DefModExtension_PackedTerrain>()?.packedTerrain ?? TerrainDefOf.PackedDirt;
        }

        public override void MapComponentTick() {
            base.MapComponentTick();
            if (GenTicks.TicksGame % 20 == 0) {
                DoWalkGridTick();
            }

            if (GenTicks.TicksGame % (GenDate.TicksPerDay / 12) == 0) {
                DoUpdateTick();
            }

            if (GenTicks.TicksGame % GenDate.TicksPerHour == 0) {
                DoPathDrawerUpdate();
            }
        }

        public override void MapComponentUpdate() {
            if (drawPaths) {
                pathsDrawer.MarkForDraw();
            }

            pathsDrawer.CellBoolDrawerUpdate();
        }

        public void DoWalkGridTick() {
            foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned.Where(p => !p.Dead && !p.Downed && p.Awake())) {
                float weight = 1f;
                if (pawn.RaceProps?.Animal ?? false) {
                    weight *= 1 / 4f;
                }

                weight *= pawn.BodySize;
                if (!pawn.pather.MovingNow) {
                    weight *= 1 / 10f;
                }

                int index = map.cellIndices.CellToIndex(pawn.Position);
                walkGrid[index] += weight;
                stoneGrid[index] += weight;

                if (DesirePaths.Settings.includeAdjacent) {
                    weight *= DesirePaths.Settings.adjacentFactor;
                    foreach (IntVec3 adjacent in GenAdjFast.AdjacentCells8Way(pawn.Position)) {
                        if (adjacent.InBounds(map)) {
                            index = map.cellIndices.CellToIndex(adjacent);
                            walkGrid[index] += weight;
                            stoneGrid[index] += weight;
                        }
                    }
                }

                // also remove snow where we're walking, directly on the snowgrid
                map.snowGrid.AddDepth(pawn.Position, -.02f * DesirePaths.Settings.snowClearFactor * pawn.BodySize);

#if DEBUG
                map.debugDrawer.FlashCell(pawn.Position, weight);
#endif
            }
        }

        public void DoUpdateTick() {
            for (int i = 0; i < map.cellIndices.NumGridCells; i++) {
                if (walkGrid[i] > DesirePaths.Settings.pathCreateThreshold) {
                    TryCreatePath(i);
                }

                walkGrid[i] *= DesirePaths.Settings.pathDegradeFactor;
                if (walkGrid[i] < DesirePaths.Settings.pathDegradeThreshold) {
                    TryRemovePath(i);
                }

                if (stoneGrid[i] > DesirePaths.Settings.stoneSmoothThreshold) {
                    TrySmooth(i);
                }
            }

        }

        public void DoPathDrawerUpdate() {
            // walkGrid updated, update paths drawer
            float max = walkGrid.Max();
            walkThreshold = max * .05f;
            walkMax = max * .8f;
            pathsDrawer.SetDirty();
        }

        public void TryCreatePath(int index) {
            IntVec3 cell = map.cellIndices.IndexToCell(index);
            TerrainDef terrain = cell.GetTerrain(map);

            if (IsPackable(terrain)) {
                originalTerrain[index] = terrain;
                map.terrainGrid.SetTerrain(cell, PackedTerrain(terrain));
            }
        }


        public void TryRemovePath(int index) {
            if (originalTerrain[index] == null) {
                return;
            }

            IntVec3 cell = map.cellIndices.IndexToCell(index);
            TerrainDef terrain = cell.GetTerrain(map);
            if (terrain != PackedTerrain(originalTerrain[index])) {
                return;
            }

            map.terrainGrid.SetTerrain(cell, originalTerrain[index]);
            originalTerrain[index] = null;
        }

        public void TrySmooth(int index) {
            IntVec3 cell = map.cellIndices.IndexToCell(index);
            TerrainDef terrain = cell.GetTerrain(map);
            if (terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone)) {
                map.terrainGrid.SetTerrain(cell, terrain.smoothedTerrain);
            }
        }

        public void DebugDraw() {
            Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
            Log.Message($"walk: min: {walkGrid.Min()}, max: {walkGrid.Max()}, avg: {walkGrid.Average()}");
            foreach (IntVec3 cell in map.AllCells) {
                map.debugDrawer.FlashCell(cell, walkGrid[map.cellIndices.CellToIndex(cell)]);
            }
        }

#if DEBUG
        public override void MapComponentOnGUI() {
            base.MapComponentOnGUI();

            IntVec3 pos = UI.MouseCell();
            int index = map.cellIndices.CellToIndex(pos);
            TerrainDef terrain = pos.GetTerrain(map);
            Widgets.Label(new Rect(10, Screen.height * 1 / 3f, 300, 300),
                           $"x: {pos.x}, y: {pos.z}\n" +
                           $"walk: {walkGrid[index]}\n" +
                           $"pathCost: {terrain.pathCost}\n" +
                           $"enabled: {(IsPackable(terrain) ? "yes" : "no")}\n" +
                           $"original: {originalTerrain[index]}\n" +
                           $"packed: {PackedTerrain(terrain).defName}");

            if (KeyBindingDefOf.Accept.KeyDownEvent) {
                DebugDraw();
            }
        }
#endif

        private static readonly Dictionary<ushort, TerrainDef> terrainsByHash =
            DefDatabase<TerrainDef>.AllDefsListForReading.ToDictionary(d => d.shortHash, d => d);

        public Color Color => GenUI.MouseoverColor;

        public override void ExposeData() {
            base.ExposeData();

            if (Scribe.mode == LoadSaveMode.LoadingVars) {
                walkGrid = new float[map.cellIndices.NumGridCells];
                stoneGrid = new float[map.cellIndices.NumGridCells];
                originalTerrain = new TerrainDef[map.cellIndices.NumGridCells];
            }

            MapExposeUtility.ExposeUshort(
                map, (cell) => (ushort) (walkGrid[map.cellIndices.CellToIndex(cell)] * 100),
                (cell, val) => walkGrid[map.cellIndices.CellToIndex(cell)] = val / 100f,
                "walkGrid");

            MapExposeUtility.ExposeUshort(
                map, (cell) => (ushort) (stoneGrid[map.cellIndices.CellToIndex(cell)] * 100),
                (cell, val) => stoneGrid[map.cellIndices.CellToIndex(cell)] = val / 100f,
                "stoneGrid");

            MapExposeUtility.ExposeUshort(
                map, (cell) => originalTerrain[map.cellIndices.CellToIndex(cell)]?.shortHash ?? 0,
                (cell, val) => {
                    if (val != 0 && terrainsByHash.TryGetValue(val, out TerrainDef terrain)) {
                        originalTerrain[map.cellIndices.CellToIndex(cell)] = terrain;
                    }
                }, "originalTerrain");
        }

        private float walkThreshold = float.MaxValue;
        private float walkMax = float.MaxValue;
        public bool GetCellBool(int index) {
            return walkGrid[index] > walkThreshold;
        }

        public Color GetCellExtraColor(int index) {
            Color color = Color.white;
            color.a = Mathf.Clamp01(walkGrid[index] / walkMax);
            return color;
        }
    }
}
