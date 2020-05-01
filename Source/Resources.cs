// Resources.cs
// Copyright Karel Kroeze, -2020

using UnityEngine;
using Verse;

namespace DesirePaths
{
    [StaticConstructorOnStartup]
    public static class Resources
    {
        public static Texture2D DrawPathsIcon;

        static Resources()
        {
            DrawPathsIcon = ContentFinder<Texture2D>.Get("UI/Icons/DrawPaths");
        }
    }
}