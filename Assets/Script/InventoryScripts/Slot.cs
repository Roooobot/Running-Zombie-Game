using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public int slotID;
    public Item slotItem;
    public string slotName;
    public Image slotImage;
    public TMP_Text slotNum;
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
        if(item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        slotImage.sprite = item.itemImage;
        slotName = item.itemName;
        if (item.itemHeld > 1)
        {
            slotNum.text = item.itemHeld.ToString();
        }
        else
            slotNum.text = "";
        slotInfo = item.itemInfo;
    }

}
