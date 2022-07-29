using ABI_RC.Core.InteractionSystem;
using MelonLoader;

namespace QMPet;

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
        if(UI.qm == null) return;
        //Do shit
    }
}