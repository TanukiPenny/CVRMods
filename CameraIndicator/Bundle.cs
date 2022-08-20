using System.IO;
using System.Reflection;
using UnityEngine;

namespace CameraIndicator;

public class Bundle
{
    public static GameObject camObject;
    
    public static void Init()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CameraIndicator.cameraindicator");
        using var memoryStream = new MemoryStream((int)stream!.Length);
        stream.CopyTo(memoryStream);
        var bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray(), 0);
        camObject = bundle.LoadAsset<GameObject>("Cam.prefab");
        camObject.hideFlags |= HideFlags.DontUnloadUnusedAsset;
    }
}