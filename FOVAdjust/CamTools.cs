using UnityEngine;

namespace FOVAdjust;

public class CamTools
{
    public static Camera SetupCamera(string name, Camera mainCam, int layer)
    {
        var gameobj = new GameObject(name);
        var cam = gameobj.AddComponent<Camera>();
        cam.CopyFrom(mainCam);
        gameobj.transform.parent = mainCam.transform;
        gameobj.transform.localPosition = Vector3.zero;
        gameobj.transform.localRotation = Quaternion.identity;
        gameobj.transform.localScale = Vector3.one;
        cam.clearFlags = CameraClearFlags.Depth;
        cam.cullingMask = layer;
        return cam;
    }
}