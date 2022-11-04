using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerEquipSlot : MonoBehaviour
{
    public int slotID;
    public Item slotItem;
    public Image slotImage;
    private bool showText;
    public GameObject itemInSlot;
    private string slotInfo;


    private GUIStyle style;

    public void Awake()
    {
        showText = false;
        style = new GUIStyle("box")
        {
            fontSize = 28
        };
    }

    public void Show()
    {
        showText = !showText;
    }


    public void OnGUI()
    {
        if (showText)
            GUI.Box(new Rect(Screen.width * 0.7f, 10, Screen.width * 0.2f, Screen.height * 0.2f), slotInfo, style);
    }

    public void SetupSlot(Item item)
    {

        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = item.itemImage;
        slotInfo = item.itemInfo;
    }

}
