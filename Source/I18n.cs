// I18n.cs
// Copyright Karel Kroeze, -2020

using Verse;

namespace DesirePaths
{
    public static class I18n
    {
        private static string Translate( this string key, params NamedArgument[] args )
        {
            return TranslatorFormattedStringExtensions.Translate( Key( key ), args ).Resolve();
        }

        private static string Key( string key )
        {
            return $"Fluffy.DesirePaths.{key}";
        }

        public static string DesirePaths = TranslatorFormattedStringExtensions.Translate( "Fluffy.DesirePaths" );

        public static string DrawPaths = "DrawPaths".Translate();

        public static string PathCreationThreshold( int threshold, int @default ) =>
            "Settings.PathCreationThreshold".Translate( threshold, @default );

        public static string PathDegradeThreshold( int threshold, int @default ) =>
            "Settings.PathDegradeThreshold".Translate( threshold, @default );

        public static string StoneSmoothThreshold( int threshold, int @default ) =>
            "Settings.StoneSmoothThreshold".Translate( threshold, @default );

        public static string PathDegradeFactor( float factor, float @default ) =>
            "Settings.PathDegradeFactor".Translate( factor.ToString( "P0" ), @default.ToString( "P0" ) );

        public static string SnowClearFactor( float factor, float @default ) =>
            "Settings.SnowClearFactor".Translate( factor.ToString( "P0" ), @default.ToString( "P0" ) );

    }
}