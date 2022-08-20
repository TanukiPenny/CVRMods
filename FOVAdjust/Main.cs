using System.Collections;
using ABI_RC.Core.Player;
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
    public static readonly MelonLogger.Instance Log = new(BuildShit.Name, System.ConsoleColor.DarkYellow);
    public static HarmonyLib.Harmony MyHarmony = new("FOVAdjust");
    public static Camera MainCamera;
    private static Camera _uiCamera;
    private static readonly int UIlayer = 1 << 30;
    private static readonly int UINum = 30;
    private static GameObject _mainUi;
    private static MelonPreferences_Category _fovAdjust;
    public static MelonPreferences_Entry<float> FOV;
        
    public override void OnApplicationStart()
    {
        _fovAdjust = MelonPreferences.CreateCategory("FOVAdjust", "FOVAdjust");
        FOV = _fovAdjust.CreateEntry("fov", 60f, "FOV",
            "Field of view that your game will use");
        MelonCoroutines.Start(WaitForUi());
        Log.Msg("FOVAdjust Loaded");
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            SetFOV(FOV.Value);
        }
    }

    private static IEnumerator WaitForUi()
    {
        while (GameObject.Find("Cohtml") == null) yield return null;
        SetUp();
    }

    private static void SetUp()
    {
        _mainUi = GameObject.Find("Cohtml");
        MainCamera = Camera.main;
        _uiCamera = CamTools.SetupCamera("FOVAdjust", MainCamera, UIlayer);
        SetLayers();
        SetFOV(FOV.Value);
        Log.Msg("Setup Complete!");
    }

    private static void SetLayers()
    {
        _mainUi.transform.Find("CohtmlWorldView").gameObject.layer = UINum;
        _mainUi.transform.Find("QuickMenu").gameObject.layer = UINum;
        MainCamera.transform.Find("CohtmlHud").gameObject.layer = UINum;
        Log.Msg("Layers Set!");
    }

    public static void SetFOV(float fov)
    {
        if (!MainCamera) return;
        CVR_DesktopCameraController.defaultFov = fov;
        CVR_DesktopCameraController.UpdateFov();
        Log.Msg("FOV Changed");
    }
}