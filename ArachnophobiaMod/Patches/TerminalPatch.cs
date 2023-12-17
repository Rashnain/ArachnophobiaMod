using HarmonyLib;

namespace ArachnophobiaMod.Patches;

[HarmonyPatch]
internal class TerminalPatch
{
    [HarmonyPatch(typeof(Terminal), "Awake")]
    [HarmonyPostfix]
    public static void Awake(Terminal __instance)
    {
        __instance.enemyFiles[12].displayVideo = Plugin.CatVideo;
    }
}