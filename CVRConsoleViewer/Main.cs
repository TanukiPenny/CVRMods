using System;
using MelonLoader;
using UnityEngine;

namespace CVRConsoleViewer;

public static class BuildShit
{
    public const string Name = "CVRConsoleViewer";
    public const string Author = "Penny, Davi";
    public const string Version = "1.0.0";
    public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";
    public const string Description = "A standalone mod to see your MelonLoader logs in game!";
}
    
public class Main : MelonMod
{
    public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);
    public static HarmonyLib.Harmony MyHarmony = new("CVRConsoleViewer");
    private static MelonPreferences_Category _cvrConsoleViewer;
    private static MelonPreferences_Entry<int> _fontSize;
    public static MelonPreferences_Entry<int> MaxLines;
    public static MelonPreferences_Entry<int> MaxChars;
    public static MelonPreferences_Entry<bool> TimeStamp;
    public static MelonPreferences_Entry<bool> AutoElastic;
    
    public override void OnApplicationStart()
    {
        BundleManager.Init();
        ConsoleManager.AttachTrackers();
        _cvrConsoleViewer = MelonPreferences.CreateCategory("CVRConsoleViewer", "CVRConsoleViewer");
        _fontSize = _cvrConsoleViewer.CreateEntry("fontSize", 20, "Font Size",
            "Font size of the text in your console tab");
        MaxLines = _cvrConsoleViewer.CreateEntry("maxLines", 200, "Max Displayed Lines",
            "Defines the limit in which your console starts discarding old lines");
        MaxChars = _cvrConsoleViewer.CreateEntry("maxChars", 1000, "Max Characters per Log",
            "Defines the limit of characters per log, printing only part of it if length is greater (will break if way too high because TextMesh limits)");
        TimeStamp = _cvrConsoleViewer.CreateEntry("timeStamp", true, "Time Stamp",
            "Sets whether logs show time stamps or not");
        AutoElastic = _cvrConsoleViewer.CreateEntry("autoElastic", true, "Elastic on new log",
            "Sets whether logs automatically scrolls down to the bottom");
        MelonCoroutines.Start(UI.WaitForQm());
        Log.Msg("CVRConsoleViewer Loaded");
    }

    public override void OnPreferencesSaved()
    {
        if (UI.Text == null)
            return;
        UI.Text.fontSize = _fontSize.Value;
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (!Camera.main) return;
        Camera.main.cullingMask &= ~(1 << 15);
    }
}