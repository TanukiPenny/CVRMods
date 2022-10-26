using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ABI_RC.Core.InteractionSystem;
using cohtml;
using MelonLoader;
using UnityEngine;

namespace ConsoleViewer
{
    public static class BuildShit
    {
        public const string Name = "ConsoleViewer";
        public const string Author = "Penny, Davi";
        public const string Version = "2.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";
        public const string Description = "A standalone mod to see your MelonLoader logs in game!";
    }

    public class ConsoleViewer : MelonMod
    {
        public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);
        private static MelonPreferences_Category _consoleViewer;
        private static string CurrTime => DateTime.Now.AddMilliseconds(-1.0).ToString("HH:mm:ss.fff");

        public override void OnApplicationStart()
        {
            HookCallbacks();
        }

        private static int a = 0;
        
        public override void OnUpdate()
        {
            if (!Input.GetKeyDown(KeyCode.P)) return;
            Log.Msg(a);
            a++;
        }

        private static void HookCallbacks()
        {
            MelonLogger.MsgCallbackHandler += OnLog;
            //MelonLogger.WarningCallbackHandler += (callingMod, logText) => OnLog(true, callingMod, logText);
            //MelonLogger.ErrorCallbackHandler += (callingMod, logText) => OnLog(false, callingMod, logText);
        }

        private static void OnLog(ConsoleColor melonColor, ConsoleColor txtColor, string callingMod, string content)
        {
            AddLine($"[<span style=\"color:{HexStrings[ConsoleColor.Green]};\">{CurrTime}</span>] [<span style=\"color:{HexStrings[melonColor]};\">{callingMod}</span>] {content}<br/>");
        }
        
        private static void OnLog(bool isWarn, string callingMod, string content)
        {
            
        }

        private static List<string> _logs = new();

        private static void AddLine(string line)
        {
            if (_logs.Count > 50)
            {
                _logs.RemoveAt(0);
            }
            _logs.Add(line);
            try
            {
                CVR_MenuManager.Instance.quickMenu.View.TriggerEvent("CVUI-ConsoleTextUpdate", _logs);
            }
            catch
            {
                // ignored
            }
        }

        private static readonly Hashtable HexStrings = new()
        {
            {ConsoleColor.Black, "#000000"},
            {ConsoleColor.DarkBlue, "#00008b"},
            {ConsoleColor.DarkGreen, "#008b00"},
            {ConsoleColor.DarkCyan, "#008b8b"},
            {ConsoleColor.DarkRed, "#8b0000"},
            {ConsoleColor.DarkMagenta, "#8b008b"},
            {ConsoleColor.DarkYellow, "#8b8b00"},
            {ConsoleColor.Gray, "#c0c0c0"},
            {ConsoleColor.DarkGray, "#8b8b8b"},
            {ConsoleColor.Blue, "#0000ff"},
            {ConsoleColor.Green, "#00ff00"},
            {ConsoleColor.Cyan, "#00ffff"},
            {ConsoleColor.Red, "#ff0000"},
            {ConsoleColor.Magenta, "#ff00ff"},
            {ConsoleColor.Yellow, "#ffff00"},
            {ConsoleColor.White, "#ffffff"}
        };
    }
}