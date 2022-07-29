using System.Collections;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace QMPet;

public class UI
{
    
    public static GameObject QMPetFront, QMPetBack, qm;
    private static GameObject VRCat, Container, QMPet, Character;
    private static RawImage QMPetFront_Img, QMPetBack_Img;
    private static RectTransform QMPetFront_Rect, QMPetBack_Rect;
    
    public static IEnumerator WaitForQm()
    {
        while (GameObject.Find("Cohtml/QuickMenu") == null) yield return null;
        PreparePet();
        AssignImgs();
    }
    
    public static void PreparePet()
    {
        qm = GameObject.Find("Cohtml/QuickMenu");
        QMPet = Object.Instantiate();
        QMPet.name = "QMPet";
        QMPetFront = QMPet.transform.FindChild("Front").gameObject;
        QMPetFront_Img = QMPetFront.GetComponent<RawImage>();
        QMPetFront_Rect = QMPetFront.GetComponent<RectTransform>();
        QMPetBack = QMPet.transform.FindChild("Back").gameObject;
        QMPetBack_Img = QMPetBack.GetComponent<RawImage>();
        QMPetBack_Rect = QMPetBack.GetComponent<RectTransform>();
        
        QMPetFront.transform.localPosition = new Vector3(220, 25, 0);
        QMPetBack.transform.localPosition = new Vector3(220, 25, 0);
        QMPetFront_Rect.pivot = new Vector2(1, 0);
        QMPetBack_Rect.pivot = new Vector2(0, 0);

        QMPetBack_Rect.sizeDelta = new Vector2(150, 150);

        QMPetFront.transform.localScale = new Vector3(Main.scale.Value, Main.scale.Value, Main.scale.Value);
        QMPetBack.transform.localScale = new Vector3(Main.scale.Value, Main.scale.Value, Main.scale.Value);

        QMPetFront_Img.texture = null;
        QMPetBack_Img.texture = null;
    }
    
    private static void AssignImgs()
    {
        if (Main.FrontLoaded)
        {
            QMPetFront_Img.texture = ResourceManager.GetTexture("QMPet.FrontTexture");
        }
        else
        {
            QMPetFront_Img.color = Color.clear;
        }
        if (Main.BackLoaded)
        {
            QMPetBack_Img.texture = ResourceManager.GetTexture("QMPet.BackTexture");
        }
        else
        {
            QMPetBack_Img.color = Color.clear;
        }
    }
}