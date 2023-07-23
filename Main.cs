using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CyclopsDecals.Patches;
using BepInEx;
using BepInEx.Logging;

namespace CyclopsDecals
{
    [BepInPlugin("EldritchCarMaker.CyclopsDecals", "Cyclops decals", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        private static ManualLogSource _laggr;
        public enum Level//lazy fix
        {
            Info,
            Debug,
            Warn,
            Error
        }
        public static void Log(Level level, object obj)
        {
            _laggr.Log(levelConvert[level], obj);
        }
        private static Dictionary<Level, LogLevel> levelConvert = new Dictionary<Level, LogLevel>()
        {
            { Level.Info, LogLevel.Info },
            { Level.Error, LogLevel.Error },
            { Level.Warn, LogLevel.Warning },
            { Level.Debug, LogLevel.Debug }
        };

        public void Awake()
        {
            _laggr = Logger;
            Log(Level.Info, "Loading CyclopsDecals");

            var assetsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

            foreach (var file in Directory.GetFiles(assetsPath))
            {
                CyclopsPatch.decalFiles.Add(Path.Combine(assetsPath, file));
            }
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "CyclopsDecalsHarmonyID");

            Log(Level.Info, "Loaded CyclopsDecals");
        }
    }
}
