using System.Collections;
using ABI_RC.Core.InteractionSystem;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace CVRConsoleViewer;

public class Patches
{
    public static void PatchShit()
    {
        Main.Log.Msg("Init Patches!");
        Main.MyHarmony.Patch
        (
            typeof(CVR_MenuManager).GetMethod("ToggleQuickMenu"),
            null,
            typeof(Patches).GetMethod("ToggleQmPatch").ToNewHarmonyMethod()
        );
    }

    public static void ToggleQmPatch(bool __0)
    {
        MelonCoroutines.Start(ToggleDelay(__0));
    }

    private static IEnumerator ToggleDelay(bool aaa)
    {
        yield return new WaitForSeconds(.2f);
        if(UI._consolePrefab == null) yield break;
        UI._consolePrefab.SetActive(aaa);
        UI.ResetOffsets();
    }
}