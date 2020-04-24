// DesireGrid.cs
// Copyright Karel Kroeze, 2020-2020

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DesirePaths
{
    public class DesireGrid : MapComponent
    {
        private float[]      walkGrid;
        private TerrainDef[] originalTerrain;

        public DesireGrid( Map map ) : base( map )
        {
            var n = map.cellIndices.NumGridCells;
            walkGrid = new float[n];
            for ( var i = 0; i < n; i++ ) walkGrid[i] = 0;

            originalTerrain = new TerrainDef[n];
        }

        public bool IsPackable( TerrainDef terrain )
        {
            return terrain.takeFootprints;
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            if ( GenTicks.TicksGame % 10                          == 0 ) DoPheromoneTick();
            if ( GenTicks.TicksGame % ( GenDate.TicksPerDay / 12 ) == 0 ) DoUpdateTick();
        }

        public void DoPheromoneTick()
        {
            foreach ( var pawn in map.mapPawns.AllPawnsSpawned.Where( p => !p.Dead && !p.Downed && p.Awake() ) )
            {
                var weight = 1f;
                if ( pawn.RaceProps?.Animal ?? false )
                    weight *= 1 / 4f;
                weight *= pawn.BodySize;
                if ( !pawn.pather.MovingNow )
                    weight *= 1 / 10f;
                walkGrid[map.cellIndices.CellToIndex( pawn.PositionHeld )] += weight;

                // also remove snow where we're walking
                map.snowGrid.AddDepth( pawn.Position, -.1f );

#if DEBUG
                map.debugDrawer.FlashCell( pawn.Position, walkGrid[map.cellIndices.CellToIndex(pawn.Position)] / 500 );
#endif
            }
        }

        public void DoUpdateTick()
        {
            for ( var i = 0; i < walkGrid.Length; i++ )
            {
                if ( walkGrid[i] > DesirePaths.Settings.pathCreateThreshold )
                    TryCreatePath( i );
                walkGrid[i] *= DesirePaths.Settings.pathDegradeFactor;
                if ( walkGrid[i] < DesirePaths.Settings.pathDegradeThreshold )
                    TryRemovePath( i );
            }
        }

        public void TryCreatePath( int index )
        {
            var cell    = map.cellIndices.IndexToCell( index );
            var terrain = cell.GetTerrain( map );
            if ( terrain == TerrainDefOf.PackedDirt || !IsPackable( terrain ) )
                return;

            originalTerrain[index] = terrain;
            map.terrainGrid.SetTerrain( cell, TerrainDefOf.PackedDirt );
        }

        public void TryRemovePath( int index )
        {
            if ( originalTerrain[index] == null )
                return;

            var cell    = map.cellIndices.IndexToCell( index );
            var terrain = cell.GetTerrain( map );
            if ( terrain != TerrainDefOf.PackedDirt )
                return;

            map.terrainGrid.SetTerrain( cell, originalTerrain[index] );
            originalTerrain[index] = null;
        }

        public void DebugDraw()
        {
            Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
            Log.Message( $"walk: min: {walkGrid.Min()}, max: {walkGrid.Max()}, avg: {walkGrid.Average()}" );
            foreach ( var cell in map.AllCells )
                map.debugDrawer.FlashCell( cell, walkGrid[map.cellIndices.CellToIndex( cell )] );
        }

#if DEBUG
        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();

            var pos = UI.MouseCell();
            Widgets.Label( new Rect( 10, Screen.height * 1 / 3f, 300, 300 ),
                           $"x: {pos.x}, y: {pos.z}\n"                               +
                           $"walk: {walkGrid[map.cellIndices.CellToIndex( pos )]}\n" +
                           $"pathCost: {pos.GetTerrain( map ).pathCost}\n" );

            if ( KeyBindingDefOf.Accept.KeyDownEvent )
                DebugDraw();
        }
#endif

        private static Dictionary<ushort, TerrainDef> terrainsByHash =
            DefDatabase<TerrainDef>.AllDefsListForReading.ToDictionary( d => d.shortHash, d => d );
        public override void ExposeData()
        {
            base.ExposeData();

            if ( Scribe.mode == LoadSaveMode.LoadingVars )
            {
                walkGrid        = new float[map.cellIndices.NumGridCells];
                originalTerrain = new TerrainDef[map.cellIndices.NumGridCells];
            }

            MapExposeUtility.ExposeUshort(
                map, ( cell ) => (ushort) ( walkGrid[map.cellIndices.CellToIndex( cell )] * 100 ),
                ( cell, val ) => walkGrid[map.cellIndices.CellToIndex( cell )] = val / 100f,
                "walkGrid" );

            MapExposeUtility.ExposeUshort(
                map, ( cell ) => originalTerrain[map.cellIndices.CellToIndex( cell )]?.shortHash ?? 0,
                ( cell, val ) =>
                {
                    if ( val != 0 && terrainsByHash.TryGetValue( val, out TerrainDef terrain ) )
                        originalTerrain[map.cellIndices.CellToIndex( cell )] = terrain;

                }, "originalTerrain" );
        }
    }
}