using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Item item;
    public TMP_Text text;
    public Inventory PlayerInventory;
    Button buyButton;
    CharacterStats m_characterStatas;

    public void Awake()
    {
        text =transform.GetChild(1).GetComponent<TMP_Text>();
        buyButton = GetComponentInChildren<Button>();
        buyButton.onClick.AddListener(AddNewItem);
    }


    private void Start()
    {
        m_characterStatas = GameManager.Instance.GetGameObject("Zombie(Clone)").GetComponent<CharacterStats>();
        text.text = "Gold:" + item.itemValue.ToString();
    }

    private void AddNewItem()
    {
        if (m_characterStatas.Gold < item.itemValue)
            return;
        if (!PlayerInventory.itemsList.Contains(item))
        {
            for (int i = 0; i < PlayerInventory.itemsList.Count; i++)
            {
                if (PlayerInventory.itemsList[i] == null)
                {
                    PlayerInventory.itemsList[i] = item;
                    break;
                }
            }
        }
        else
        {
            item.itemHeld += item.itemTemplateNumber;
        }
        m_characterStatas.Gold -= item.itemValue;
        InventoryManager.RefreshItem();
    }

}
