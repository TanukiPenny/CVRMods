using ABI.CCK.Components;
using ABI_RC.Core.InteractionSystem;
using MelonLoader;

namespace FOVAdjust;

public class Patches
{
    public static void PatchShit()
    {
        Main.Log.Msg("Init Patches!");
        Main.MyHarmony.Patch
        (
            typeof(CVRWorld).GetMethod("SetupWorldRules"),
            null,
            typeof(Patches).GetMethod("SetupWorldRulesPatch").ToNewHarmonyMethod()
        );
    }

    private static void SetupWorldRulesPatch()
    {
        Main.SetFOV(Main.FOV.Value);
        Main.MainCamera.cullingMask &= ~(1 << 30);
        Main.Log.Msg("Setup World!");
    }
}