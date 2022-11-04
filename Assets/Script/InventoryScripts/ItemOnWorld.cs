using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory PlayerInventory;

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    private void AddNewItem()
    {
        if (!PlayerInventory.itemsList.Contains(thisItem))
        {
            for(int i = 0; i < PlayerInventory.itemsList.Count; i++)
            {
                if (PlayerInventory.itemsList[i]==null)
                {
                    PlayerInventory.itemsList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += thisItem.itemTemplateNumber; 
        }

        InventoryManager.RefreshItem();
    }
}
