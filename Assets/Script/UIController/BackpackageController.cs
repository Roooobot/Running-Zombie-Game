using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackageController : MonoBehaviour
{
    private PlayerCharacter character;
    public GameObject m_MyBag;
    public GameObject m_MyEquip;
    public GameObject Shop;

    public bool m_ShowMouse = false;

    private void OnEnable()
    {
        foreach (var item in FindObjectsOfType<GameObject>())
        {
            if (item.name == "BackPackage")
            {
                m_MyBag= item;
            }
            if (item.name == "PlayerEquip")
            {
                m_MyEquip = item;
            }
            if (item.name == "Shop")
            {
                Shop = item;
            }

        }
        character=GetComponentInParent<PlayerCharacter>();
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        OpenMyBag();
        OpenMyEquip();
        OpenShop();
        ShowMouse();
    }

    

    public void Init()
    {
        m_MyBag.SetActive(false);
        m_MyEquip.SetActive(false);
        Shop.SetActive(false);
    }

    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_MyBag.SetActive(!m_MyBag.activeSelf);
        }   
        
    }
    void OpenMyEquip()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            m_MyEquip.SetActive(!m_MyEquip.activeSelf);
        }
    }

    private void OpenShop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (var collider in colliders)
                if (collider.CompareTag("ShopNPC"))
                {
                    Shop.SetActive(!Shop.activeSelf);
                    break;
                }
        }
    }
    void ShowMouse()
    {
        m_ShowMouse = m_MyBag.activeSelf || m_MyEquip.activeSelf;
        if (m_ShowMouse||character.showMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

}
