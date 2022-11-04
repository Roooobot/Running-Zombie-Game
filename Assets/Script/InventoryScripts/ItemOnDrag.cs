using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    public Transform originalParent;
    public Inventory myBag;
    public Inventory myEquip;

    private int currentItemID;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != -1)
        {
            return;
        }
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = new Vector3(eventData.position.x, eventData.position.y, 0.0f);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != -1)
        {
            return;
        }
        transform.position = new Vector3(eventData.position.x, eventData.position.y, 0.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != -1)
        {
            return;
        }
        if (originalParent.parent.name == "Grid")
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")
                {
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
                    if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.parent.name == "Grid" )
                    {
                        var temp = myBag.itemsList[currentItemID];
                        myBag.itemsList[currentItemID] = myBag.itemsList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                        myBag.itemsList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;
                    }
                    else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.parent.name == "EquipGrid")
                    {
                        var temp = myEquip.itemsList[eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInParent<Slot>().slotID];
                        myEquip.itemsList[eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInParent<Slot>().slotID] = myBag.itemsList[currentItemID];
                        myBag.itemsList[currentItemID] = temp;
                    }
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
                {
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    if (eventData.pointerCurrentRaycast.gameObject.transform.parent.name == "Grid")
                    {
                        myBag.itemsList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemsList[currentItemID];
                        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID != currentItemID)
                            myBag.itemsList[currentItemID] = null;
                    }
                    if (eventData.pointerCurrentRaycast.gameObject.transform.parent.name == "EquipGrid")
                    {
                        myEquip.itemsList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemsList[currentItemID];
                        myBag.itemsList[currentItemID] = null;
                    }
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                transform.SetParent(originalParent);
                transform.position = originalParent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject == null)
            {
                Vector3 vector3 = FindObjectOfType<PlayerCharacter>().gameObject.transform.position;
                vector3.x += 1;
                Instantiate(myBag.itemsList[currentItemID].itemPrefab, vector3, Quaternion.identity);
                myBag.itemsList[currentItemID] = null;
                InventoryManager.RefreshItem();
                return;
            }
        }
        if (originalParent.parent.name == "EquipGrid")
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
                {
                    if (eventData.pointerCurrentRaycast.gameObject.transform.parent.name == "Grid")
                    {
                        transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                        transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                        myBag.itemsList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myEquip.itemsList[currentItemID];
                        myEquip.itemsList[currentItemID] = null;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }
                }
            }
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.parent.parent.SetAsLastSibling();
    }
}
