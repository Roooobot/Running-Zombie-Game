using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在编译器的右键的creat菜单里添加新的选项
[CreateAssetMenu(fileName = "New Iem",menuName ="Inventory/New Item")]


public class Item :ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public int itemTemplateNumber = 1;              //item的单位数量
    public int itemHeld;                                         //item当前的数量
    public int itemValue;
    [TextArea ]
    public string itemInfo;
    public bool Equip;
    public int damage;
}
