using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    private Vector3 startPos;
    private bool isDragable = true;
    private CanvasGroup group;

    private void Start()
    {
        @group = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDragable) return;
        @group.blocksRaycasts = false;
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(targetPos.x, targetPos.y, startPos.z);
        transform.position = transform.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        transform.position = startPos;
        @group.blocksRaycasts = true;
    }

    public void Place(CellBehaviour cell)
    {
        transform.position = cell.transform.position;
        isDragable = false;
    }
}
