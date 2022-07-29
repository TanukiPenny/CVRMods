using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ABI_RC.Core.InteractionSystem;
using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Object = UnityEngine.Object;

namespace CVRConsoleViewer;

public static class UI
{
    public static GameObject qm, _consolePrefab, UiCameraObj;
    public static TextMeshProUGUI Text;
    private static ScrollRect _scrollRect;
    public static Camera UiCamera;
    

    public static IEnumerator WaitForQm()
    {
        while (GameObject.Find("Cohtml/QuickMenu") == null) yield return null;
        BuildMenu();
    }
    

    private static void BuildMenu()
    {
        Patches.PatchShit();
        qm = GameObject.Find("Cohtml/QuickMenu");
        _consolePrefab = Object.Instantiate(BundleManager._consolePrefab, qm.transform);
        _consolePrefab.transform.localPosition = new Vector3(-0.72f, 0, 0);
        _consolePrefab.transform.localScale = new Vector3(0.0013f, 0.0013f, 0.0013f);
        
        _scrollRect = _consolePrefab.GetComponentInChildren<ScrollRect>(true);
        Text = _consolePrefab.transform.Find("Scroll View/Viewport/Content/")
                         .GetComponentInChildren<TextMeshProUGUI>(true);
        SetLayerRecursively(_consolePrefab, 31);
        foreach (var i in ConsoleManager.Cached)
            AppendText(i);
        MelonPreferences.Save();
        SetupCamera();
        Main.Log.Msg("Setup Complete!");
    }

    private static void SetupCamera()
    {
        var mainCam = Camera.main;
        var gameobj = new GameObject("CVRConsoleViewer");
        var cam = gameobj.AddComponent<Camera>();
        cam.CopyFrom(mainCam);
        if (XRDevice.isPresent && Environment.GetCommandLineArgs().Contains("-vr"))
        {
            gameobj.transform.parent = mainCam.transform.parent;
        }
        else
        {
            gameobj.transform.parent = mainCam.transform;
        }
        gameobj.transform.localPosition = Vector3.zero;
        gameobj.transform.localRotation = Quaternion.identity;
        gameobj.transform.localScale = Vector3.one;
        UiCameraObj = gameobj;
        UiCamera = cam;
        UiCamera.clearFlags = CameraClearFlags.Depth;
        UiCamera.cullingMask = 1 << 31;
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    #region Text Appending
    private static int _lineNum;
    public static void AppendText(string txt)
    {
        if (_lineNum >= Main.MaxLines.Value)
            Text.text = GetReducedStr(Text.text, Main.MaxLines.Value);
        else
            _lineNum++;
        Text.text += txt;
    }
    private static string GetReducedStr(string content, int nthIndex)
    {
        var index = 0;
        nthIndex = content.Count(occ => occ == '\n') - nthIndex + 1;
        
        if (nthIndex < 0)
            return content;
        
        for (; nthIndex != 0; nthIndex--)
            index = content.IndexOf('\n', index) + 1;
        
        return content.Substring(index);
    }
    #endregion
    
    #region ResetOffsets
    private static bool _fired;
    public static void ResetOffsets()
    {
        if (!_fired) MelonCoroutines.Start(ResetOffsetsCoroutine());
    }
    private static IEnumerator ResetOffsetsCoroutine()
    {
        _scrollRect.movementType = ScrollRect.MovementType.Elastic;
        
        yield return new WaitForSeconds(.5f);
        
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        _fired = false;
    }
    #endregion
}