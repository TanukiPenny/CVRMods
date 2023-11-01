using System;
using System.Collections.Generic;
using System.Linq;
using ABI_RC.Core.Player;
using ABI_RC.Systems.GameEventSystem;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraIndicator
{
    public static class BuildShit
    {
        public const string Name = "CameraIndicator";
        public const string Author = "Penny";
        public const string Version = "1.0.7";
        public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";

        public const string Description =
            "A mod that gives you a indicator of where other players cameras are postioned";
    }

    public class CameraIndicator : MelonMod
    {
        public static MelonPreferences_Category cat;
        private const string catagory = "CameraIndicator";
        public static MelonPreferences_Entry<bool> showIndicators;

        public static readonly MelonLogger.Instance Log = new(BuildShit.Name, System.Drawing.Color.FromArgb(128, 128, 0));
        public static readonly List<CameraObject> CameraObjects = new();
        public static GameObject Base;
        private static bool _initialized;

        public override void OnInitializeMelon()
        {
            Bundle.Init();

            cat = MelonPreferences.CreateCategory(catagory, "CameraIndicator");
            showIndicators = MelonPreferences.CreateEntry(catagory, nameof(showIndicators), true, "Show Camera Indicators (Defaults to On every launch)");
            showIndicators.Value = true; //Defaults to on every game launch

            CVRGameEventSystem.World.OnLoad.AddListener(worldGuid => {

                if (!_initialized) {
                    CVRPlayerManager.Instance.OnPlayerEntityCreated += PlayerJoin;
                    CVRPlayerManager.Instance.OnPlayerEntityRecycled += PlayerLeave;
                    _initialized = true;
                }

                CameraObjects.Clear();
                Base = new GameObject("CameraIndicator");
            });
        }

        public override void OnLateUpdate()
        {
            foreach (var cameraObject in CameraObjects)
            {
                var player = cameraObject.PlayerEntity;
                if (player.PlayerObject == null) continue;
                if (player.PuppetMaster.PlayerAvatarMovementDataInput.CameraEnabled && showIndicators.Value)
                {
                    cameraObject.CamTran.SetActive(true);
                }
                else
                {
                    cameraObject.CamTran.SetActive(false);
                    continue;
                }

                cameraObject.CamTran.transform.position = Vector3.Lerp(cameraObject.CamTran.transform.position,
                    player.PuppetMaster.PlayerAvatarMovementDataInput.CameraPosition, 20f * Time.deltaTime);
                cameraObject.CamRot.transform.rotation = Quaternion.Lerp(cameraObject.CamRot.transform.rotation,
                    Quaternion.Euler(player.PuppetMaster.PlayerAvatarMovementDataInput.CameraRotation +
                                     new Vector3(90f, 0, 0)), 20f * Time.deltaTime);
                cameraObject.NameTag.transform.LookAt(Camera.main.transform);
            }
        }

        private static void PlayerJoin(CVRPlayerEntity p)
        {
            try
            {
                var camGameObject = Object.Instantiate(Bundle.CamObject, Base.transform);
                camGameObject.transform.GetChild(0).gameObject.layer = 5; //Hide from CVR Camera by changing object to UI layer
                var camObject = new CameraObject(p, camGameObject,camGameObject.transform.GetChild(0).gameObject,
                    camGameObject.transform.GetChild(1).GetChild(0).gameObject,
                    camGameObject.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>());
                camObject.CamTran.SetActive(false);
                camObject.CamTran.name = p.Username;
                camObject.NameText.text = p.Username;
                CameraObjects.Add(camObject);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static void PlayerLeave(CVRPlayerEntity p)
        {
            try
            {
                var camObject = CameraObjects.FirstOrDefault(camObject => p.Username == camObject.PlayerEntity.Username);
                if (!CameraObjects.Remove(camObject)) return;
                Object.Destroy(camObject?.CamTran);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
