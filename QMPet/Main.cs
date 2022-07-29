using System;
using MelonLoader;

namespace QMPet;

public static class BuildShit
{
    public const string Name = "QMPet";
    public const string Author = "Penny";
    public const string Version = "1.0.0";
    public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";
    public const string Description = "A standalone mod to add a pet to ur QM!";
}

public class Main : MelonMod
{
    public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);
    public static HarmonyLib.Harmony MyHarmony = new("QMPet");
    private static MelonPreferences_Category qmPetCategory;
    public static MelonPreferences_Entry<float> scale;
    public static bool FrontLoaded = false, BackLoaded = false;
    
    public override void OnApplicationStart()
    {
        qmPetCategory = MelonPreferences.CreateCategory("QMPet", "QMPet");
        scale = qmPetCategory.CreateEntry("scale", 1.3f, "Pet Scale");

        CheckFolder();
        LoadImgs();
    }
    
    public override void OnPreferencesSaved()
    {
        if (UI.QMPetBack != null)
        {
            UI.QMPetFront.transform.localScale = new Vector3(scale.Value, scale.Value, scale.Value);
            UI.QMPetBack.transform.localScale = new Vector3(scale.Value, scale.Value, scale.Value);
        }
    }
    
    private void CheckFolder()
    {
        if (!Directory.Exists("UserData/QMPet"))
        {
            Directory.CreateDirectory("UserData/QMPet");
        }
    }

    private void LoadImgs()
    {
        if (File.Exists("UserData/QMPet/front.png"))
        {
            ResourceManager.LoadTexture("QMPet", "FrontTexture", File.ReadAllBytes("UserData/QMPet/front.png"));
            FrontLoaded = true;
        }
        else
        {
            Log.Msg("No front image loaded!");
        }
        if (File.Exists("UserData/QMPet/back.png"))
        {
            ResourceManager.LoadTexture("QMPet", "BackTexture", File.ReadAllBytes("UserData/QMPet/back.png"));
            BackLoaded = true;
        }
        else
        {
            Log.Msg("No back image loaded!");
        }
    }
}