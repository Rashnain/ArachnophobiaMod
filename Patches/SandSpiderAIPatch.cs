using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ArachnophobiaMod.Patches;

[HarmonyPatch]
internal class SandSpiderAIPatch
{
    private static Dictionary<SandSpiderAI, float> _timeSinceMovingLegs;

    [HarmonyPatch(typeof(SandSpiderAI), "Start")]
    [HarmonyPrefix]
    public static void Start(SandSpiderAI __instance)
    {
        _timeSinceMovingLegs ??= new Dictionary<SandSpiderAI, float>();
        _timeSinceMovingLegs.Add(__instance, Random.Range(0.1f, 0.9f));

        __instance.gameObject.transform.Find("MeshContainer/MeshRenderer").gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        Transform spiderHead = __instance.gameObject.transform.Find("MeshContainer/AnimContainer/Armature/Head");
        spiderHead.Find("LeftFang").gameObject.GetComponent<MeshRenderer>().enabled = false;
        spiderHead.Find("RightFang").gameObject.GetComponent<MeshRenderer>().enabled = false;

        GameObject cat = Object.Instantiate(Plugin.Cat, __instance.gameObject.transform.Find("MeshContainer/ScanNode"));
        cat.transform.localScale = new Vector3(5f, 2.5f, 5f);
        cat.transform.localPosition = new Vector3(0f, 1f, -0.5f);

        Material spiderMat = spiderHead.gameObject.GetComponentInChildren<MeshRenderer>().material;
        Material catMat = cat.GetComponent<MeshRenderer>().material;
        catMat.shader = spiderMat.shader;
        catMat.renderQueue = spiderMat.renderQueue;

        __instance.enemyType.stunSFX = Plugin.CatStunSFX;
        __instance.hitSpiderSFX = Plugin.CatHitSFX;
        __instance.dieSFX = Plugin.CatDieSFX;
        __instance.attackSFX = Plugin.CatAttackSFX;
        __instance.spoolPlayerSFX = Plugin.SpoolPlayerSFX;
    }

    [HarmonyPatch(typeof(SandSpiderAI), "Update")]
    [HarmonyPostfix]
    public static void Update(SandSpiderAI __instance)
    {
        _timeSinceMovingLegs[__instance] += Time.deltaTime;
    }

    [HarmonyPatch(typeof(SandSpiderAI), "MoveLegsProcedurally")]
    [HarmonyPrefix]
    public static bool MoveLegsProcedurally(SandSpiderAI __instance)
    {
        if (_timeSinceMovingLegs[__instance] <= 0.2f)
            return false;
        _timeSinceMovingLegs[__instance] = 0f;
        return true;
    }

    [HarmonyPatch(typeof(SandSpiderAI), "KillEnemy")]
    [HarmonyPostfix]
    public static void KillEnemy(SandSpiderAI __instance, object[] __args)
    {
        Transform cat = __instance.gameObject.transform.Find("MeshContainer/ScanNode/Cat(Clone)").transform;
        Vector3 angles = cat.localEulerAngles;
        cat.localEulerAngles = new Vector3(angles.x, angles.y, -90f);
        cat.localPosition = new Vector3(0f, 0f, 0f);
    }
}