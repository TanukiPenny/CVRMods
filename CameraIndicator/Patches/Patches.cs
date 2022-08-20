using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ABI_RC.Core.Networking;
using ABI_RC.Core.Player;
using HarmonyLib;
using UnityEngine;
using static CameraIndicator.CameraIndicator;
using Object = UnityEngine.Object;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace CameraIndicator.Patches;

[HarmonyPatch]
class CVRPlayerManagerJoin
{
    private static readonly MethodInfo _targetMethod = typeof(List<CVRPlayerEntity>).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
    private static readonly MethodInfo _userJoinMethod = typeof(Patches).GetMethod("PlayerJoin", BindingFlags.Static | BindingFlags.Public);
    private static readonly FieldInfo _playerEntity = typeof(CVRPlayerManager).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).Single(t => t.GetField("p") != null).GetField("p");

    static MethodInfo TargetMethod()
    {
        return typeof(CVRPlayerManager).GetMethod(nameof(CVRPlayerManager.TryCreatePlayer), BindingFlags.Instance | BindingFlags.Public);
    }
        
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new CodeMatcher(instructions)
            .MatchForward(true, new CodeMatch(OpCodes.Callvirt, _targetMethod))
            .Insert(
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldfld, _playerEntity),
                new CodeInstruction(OpCodes.Call, _userJoinMethod)
            )
            .InstructionEnumeration();

        return code;
    }
}

[HarmonyPatch]
class CVRPlayerEntityLeave
{
    private static readonly MethodInfo _userLeaveMethod = typeof(Patches).GetMethod("PlayerLeave", BindingFlags.Static | BindingFlags.Public);
        
    static MethodInfo TargetMethod()
    {
        return typeof(CVRPlayerEntity).GetMethod(nameof(CVRPlayerEntity.Recycle), BindingFlags.Instance | BindingFlags.Public);
    }
        
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new CodeMatcher(instructions)
            .Advance(1)
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, _userLeaveMethod)
            )
            .InstructionEnumeration();
            
        return code;
    }
}

[HarmonyPatch(typeof(NetworkManager))]
class NetworkManagerPatches
{
    // Token: 0x06000046 RID: 70 RVA: 0x00003DA0 File Offset: 0x00001FA0
    [HarmonyPatch("OnGameNetworkConnectionClosed")]
    [HarmonyPostfix]
    private static void OnGameNetworkClosed()
    {
        CvrPlayerEntities.Clear();
        foreach (var cameraObject in CameraObjects)
        {
            Object.Destroy(cameraObject);
        }
        CameraObjects.Clear();
    }
}

public static class Patches
{
    public static void PlayerJoin(CVRPlayerEntity p)
    {
        CvrPlayerEntities.Add(p);
        Log.Msg($"{p.Username}: Has joined");
        var tempobj = Object.Instantiate(Bundle.camObject);
        tempobj.SetActive(false);
        tempobj.name = p.Username;
        tempobj.transform.localScale = new Vector3(.03f, .03f, .03f);
        Object.Destroy(tempobj.GetComponent<BoxCollider>());
        CameraObjects.Add(tempobj);
    }

    public static void PlayerLeave(CVRPlayerEntity p)
    {
        var player = CvrPlayerEntities.FirstOrDefault(playerEntity => p.Username == playerEntity.Username);
        if (player == null) return;
        Log.Msg($"{p.Username}: Has Left");
        var tempobj = CameraObjects.FirstOrDefault(tempobj => p.Username == tempobj.name);
        CameraObjects.Remove(tempobj);
        CvrPlayerEntities.Remove(player);
        Object.Destroy(tempobj);
    }
}