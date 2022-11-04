using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڱ��������Ҽ���creat�˵�������µ�ѡ��
[CreateAssetMenu(fileName = "New Iem",menuName ="Inventory/New Item")]


public class Item :ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public int itemTemplateNumber = 1;              //item�ĵ�λ����
    public int itemHeld;                                         //item��ǰ������
    public int itemValue;
    [TextArea ]
    public string itemInfo;
    public bool Equip;
    public int damage;
}
