using ABI_RC.Core.InteractionSystem;
using HarmonyLib;
using MelonLoader;

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
        if(UI._consolePrefab == null) return;
        UI._consolePrefab.SetActive(__0);
        UI.ResetOffsets();
    }
}