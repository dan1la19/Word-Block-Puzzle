using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

    public void Place(CellBehaviour cell)
    {
        transform.position = cell.transform.position;
        startPos = transform.position;
        isDragable = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("drop");
        transform.position = startPos;
        if(isDragable) @group.blocksRaycasts = true;
    }
}
