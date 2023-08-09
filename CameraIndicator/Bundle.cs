using System.IO;
using System.Reflection;
using UnityEngine;

namespace CameraIndicator
{
    public static class Bundle
    {
        public static GameObject CamObject;
    
        public static void Init()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("cameraindicator");
            using var memoryStream = new MemoryStream((int)stream!.Length);
            stream.CopyTo(memoryStream);
            var bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray(), 0);
            CamObject = bundle.LoadAsset<GameObject>("Assets/CameraIndicator/Cam.prefab");
            CamObject.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }
    }
}
