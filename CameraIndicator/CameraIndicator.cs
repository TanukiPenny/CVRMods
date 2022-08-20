using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ABI_RC.Core.Player;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraIndicator;

public static class BuildShit
{
    public const string Name = "CameraIndicator";
    public const string Author = "Penny";
    public const string Version = "1.0.0";
    public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";
    public const string Description = "-----------------------------------------";
}
    
public class CameraIndicator : MelonMod
{
    public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);
    public static List<CVRPlayerEntity> CvrPlayerEntities = new();
    public static List<GameObject> CameraObjects = new();

    public override void OnApplicationStart()
    {
        Bundle.Init();
    }

    public override void OnUpdate()
    {
        foreach (var cameraObject in CameraObjects)
        {
            var player = CvrPlayerEntities.FirstOrDefault(player => player.Username == cameraObject.name);
            if (player.PuppetMaster.PlayerAvatarMovementDataInput.CameraEnabled)
            {
                cameraObject.SetActive(true);
            }
            else
            {
                cameraObject.SetActive(false);
                continue;
            }
            cameraObject.transform.position = player.PuppetMaster.PlayerAvatarMovementDataInput.CameraPosition;
            cameraObject.transform.rotation = Quaternion.Euler(player.PuppetMaster.PlayerAvatarMovementDataInput.CameraRotation);
        }
    }
}