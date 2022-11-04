using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class EquipOnDrag : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler
{
    public Transform originalParent;
    public Inventory myBag;
    private int currentItemID;


    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.parent.parent.SetAsLastSibling();
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<PlayerEquipSlot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = new Vector3(eventData.position.x, eventData.position.y, 0.0f);
        
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(eventData.position.x, eventData.position.y, 0.0f);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if(eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                myBag.itemsList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemsList[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID != currentItemID)
                    myBag.itemsList[currentItemID] = null;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        return;
    }
}
