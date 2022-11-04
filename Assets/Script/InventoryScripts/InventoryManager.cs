using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Inventory itemTemplate;
    public Inventory myBag;
    public Inventory myEquip;

    public GameObject emptySlot;
    public GameObject playerEquipSlot;

    public GameObject slotGrid;
    public GameObject equipGrid;

    public List<GameObject> backpackageSlots = new();
    public List<GameObject> equipSlots = new();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        RefreshItem();
        RefreshEquip();
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < Instance.slotGrid.transform.childCount; i++)
        {
            if (Instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for(int i =0; i < Instance.myBag.itemsList.Count; i++)
        {
            Instance.backpackageSlots.Add(Instance.emptySlot);
            Instance.backpackageSlots[i] = Instantiate(Instance.emptySlot,Instance.slotGrid.transform);
            Instance.backpackageSlots[i].GetComponent<Slot>().slotID = i;
            Instance.backpackageSlots[i].GetComponent<Slot>().SetupSlot(Instance.myBag.itemsList[i]);
        }
    }

    public static void RefreshEquip()
    {
        List<Vector3> slotPosition = new();
        for (int i = 0; i < Instance.equipGrid.transform.childCount; i++)
        {
            if (Instance.equipGrid.transform.childCount == 0)
                break;
            slotPosition.Add(Instance.equipGrid.transform.GetChild(i).position);
            Destroy(Instance.equipGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < Instance.myEquip.itemsList.Count; i++)
        {
            Instance.equipSlots.Add(Instance.playerEquipSlot);
            Instance.equipSlots[i] = Instantiate(Instance.playerEquipSlot, slotPosition[i],Quaternion.identity ,Instance.equipGrid.transform);
            Instance.equipSlots[i].GetComponent<Slot>().slotID = i;
            Instance.equipSlots[i].GetComponent<Slot>().SetupSlot(Instance.myEquip.itemsList[i]);
        }
    }

}
