using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ABI_RC.Core.Networking;
using ABI_RC.Core.Player;
using HarmonyLib;
using TMPro;
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

public static class Patches
{
    public static void PlayerJoin(CVRPlayerEntity p)
    {
        try
        {
            CvrPlayerEntities.Add(p);
            var tempobj = Object.Instantiate(Bundle.camObject, Base.transform);
            var tempcamobject = new CameraObject(tempobj,tempobj.transform.GetChild(0).gameObject,
                tempobj.transform.GetChild(1).GetChild(0).gameObject,
                tempobj.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>());
            tempcamobject.CamTran.SetActive(false);
            tempcamobject.CamTran.name = p.Username;
            tempcamobject.NameText.text = p.Username;
            CameraObjects.Add(tempcamobject);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

    public static void PlayerLeave(CVRPlayerEntity p)
    {
        try
        {
            var player = CvrPlayerEntities.FirstOrDefault(playerEntity => p.Username == playerEntity.Username);
            if (player == null) return;
            var tempcamobject = CameraObjects.FirstOrDefault(tempcamobject => p.Username == tempcamobject.CamTran.name);
            CameraObjects.Remove(tempcamobject);
            CvrPlayerEntities.Remove(player);
            Object.Destroy(tempcamobject.CamTran);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}