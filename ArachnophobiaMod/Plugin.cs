using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Video;

namespace ArachnophobiaMod;

[BepInPlugin(GUID, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    private const string GUID = "com.github.rashnain.arachnophobiamod";
    private const string Name = "ArachnophobiaMod";
    private const string Version = "1.0.1";

    private static readonly Harmony Harmony = new(GUID);

    internal static GameObject Cat;
    internal static VideoClip CatVideo;
    internal static AudioClip CatStunSFX;
    internal static AudioClip CatHitSFX;
    internal static AudioClip CatDieSFX;
    internal static AudioClip CatAttackSFX;
    internal static AudioClip SpoolPlayerSFX;

    private void Awake()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromMemory(GetResourceBytes("cat"));
        Cat = assetBundle.LoadAsset<GameObject>("assets/Cat.prefab");
        CatVideo = assetBundle.LoadAsset<VideoClip>("assets/CatVideo0001-0100.mp4");
        CatStunSFX = assetBundle.LoadAsset<AudioClip>("assets/StunCat.mp3");
        CatHitSFX = assetBundle.LoadAsset<AudioClip>("assets/CatHit.mp3");
        CatDieSFX = assetBundle.LoadAsset<AudioClip>("assets/CatDie.mp3");
        CatAttackSFX = assetBundle.LoadAsset<AudioClip>("assets/CatAttack.mp3");
        SpoolPlayerSFX = assetBundle.LoadAsset<AudioClip>("assets/SpoolPlayerInWebWithoutStepSound.mp3");

        Harmony.PatchAll();
    }

    private static byte[] GetResourceBytes(string resourceName)
    {
        string name = Name + ".Resources." + resourceName;
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

        if (stream == null)
            return null;

        byte[] array = new byte[stream.Length];

        return stream.Read(array, 0, array.Length) < array.Length ? null : array;
    }
}