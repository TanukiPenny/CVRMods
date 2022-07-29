using System.Collections;
using MelonLoader;
using UnityEngine;

namespace FOVAdjust;

public static class BuildShit
{
    public const string Name = "FOVAdjust";
    public const string Author = "Penny";
    public const string Version = "1.0.0";
    public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";
    public const string Description = "A standalone mod to adjust you in game FOV!";
}
    
public class Main : MelonMod
{
    private static readonly MelonLogger.Instance Log = new(BuildShit.Name, System.ConsoleColor.DarkYellow);

    private static Camera _mainCamera, _uiCamera;
    private static readonly int UIlayer = 1 << 30;
    private static GameObject _mainUi;
        
    public override void OnApplicationStart()
    {
        MelonCoroutines.Start(WaitForUi());
        Log.Msg("FOVAdjust Loaded");
    }
    
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (!_mainCamera) return;
        _mainCamera.cullingMask &= ~(1 << 31);
    }

    private static IEnumerator WaitForUi()
    {
        while (GameObject.Find("Cohtml") == null) yield return null;
        SetUp();
    }

    private static void SetUp()
    {
        _mainUi = GameObject.Find("Cohtml");
        _mainCamera = Camera.main;
        _uiCamera = CamTools.SetupCamera("FOVAdjust", _mainCamera, UIlayer);
        SetLayers();
        SetFOV(90);
        Log.Msg("Setup Complete!");
    }

    private static void SetLayers()
    {
        _mainUi.transform.Find("CohtmlWorldView").gameObject.layer = UIlayer;
        _mainUi.transform.Find("QuickMenu").gameObject.layer = UIlayer;
        _mainCamera.transform.Find("CohtmlHud").gameObject.layer = UIlayer;
    }

    private static void SetFOV(int fov)
    {
        _mainCamera.fieldOfView = fov;
    }
}