using Sentry;
using System;
using System.Collections.Generic;
using HarmonyLib;
using Logger = CyclopsDecals.Main;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CyclopsDecals.Patches
{
    [HarmonyPatch(typeof(SubRoot))]
    internal class CyclopsPatch
    {
        public static List<string> decalFiles = new List<string>();

        [HarmonyPatch(nameof(SubRoot.Awake))]
        public static void Prefix(SubRoot __instance)
        {
            if(!__instance.name.Contains("(Clone)"))
            {
                Logger.Log(Logger.Level.Info, "Skipping prefab");
                return;
            }

            Logger.Log(Logger.Level.Info, $"Adding decals to cyclops. {decalFiles.Count} decals total");
            var subname = __instance.GetComponentInChildren<SubName>(true);
            var nameText = subname.hullName;

            foreach(var decalFile in decalFiles)
            {
                var decalName = decalFile.Substring((decalFile.Length / 3) * 2);
                Logger.Log(Logger.Level.Info, $"Adding Decal {decalName}");
                var decal = Nautilus.Utility.ImageUtils.LoadTextureFromFile(decalFile);//file name includes file extension already
                var decalObj = GameObject.Instantiate(nameText.gameObject, nameText.transform.position, nameText.transform.rotation, nameText.transform.parent);
                decalObj.name = $"Decal {decalName}";
                GameObject.DestroyImmediate(decalObj.GetComponent<TextMeshProUGUI>());
                decalObj.AddComponent<Image>().sprite = Sprite.Create(decal, new Rect(0, 0, decal.width, decal.height), Vector2.zero);
                decalObj.transform.position = nameText.transform.position;
                decalObj.transform.rotation = nameText.transform.rotation;
                decalObj.transform.localScale = nameText.transform.localScale;

                decalObj.transform.position += 0.15f * decalObj.transform.up;//bottom was clipping with the cyclops
            }
        }
    }
}
