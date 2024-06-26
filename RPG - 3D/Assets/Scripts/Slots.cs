using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slots : MonoBehaviour, IDropHandler
{
    [System.Serializable]
    public enum SlotsType
    {
        inventory,
        Helmet,
        Armor,
        Shield
    }

    public SlotsType SlotType;

    public GameObject item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            Dragitem.ItemBeginDragged.GetComponent<Dragitem>().SetParent(transform, this);
        }
    }
}
