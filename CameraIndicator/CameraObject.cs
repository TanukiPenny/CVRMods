using ABI_RC.Core.Player;
using UnityEngine;

namespace CameraIndicator;

public class CameraObject
{
    public GameObject CamTran { get; set; }
    public GameObject CamRot { get; set; }
    public GameObject NameTag { get; set; }
    public UnityEngine.UI.Text NameText { get; set; }
    
    public CVRPlayerEntity PlayerEntity { get; set; }

    public CameraObject(CVRPlayerEntity playerEntity, GameObject camTran, GameObject camRot, GameObject nameTag, UnityEngine.UI.Text nameText)
    {
        CamTran = camTran;
        CamRot = camRot;
        NameTag = nameTag;
        NameText = nameText;
        PlayerEntity = playerEntity;
    }
    
    
}