using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellBehaviour : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var block = eventData.pointerDrag.GetComponent<BlockBehaviour>();
        if (block != null)
        {
            Debug.Log("drop cell");
            block.Place(this);
        }
    }
}
