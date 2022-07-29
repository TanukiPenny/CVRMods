using System.IO;
using System.Reflection;
using UnityEngine;

namespace CVRConsoleViewer;

internal static class BundleManager
{
    public static GameObject _consolePrefab;

    public static void Init()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CVRConsoleViewer.console");
        using var memoryStream = new MemoryStream((int)stream!.Length);
        stream.CopyTo(memoryStream);
        var bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray(), 0);
        _consolePrefab = bundle.LoadAsset<GameObject>("console.prefab");
        _consolePrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
    }
}