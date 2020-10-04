using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private CanvasGroup group;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = false;
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(targetPos.x, targetPos.y, startPos.z);
    }

    public void Place(CellBehaviour cell)
    {
        transform.position = cell.transform.position;
        //startPos = transform.position;

        for (var i = 0; i < 9; i++)
        {
            var child = gameObject.transform.GetChild(i);
            var letter = child.Find("Shell/Text").gameObject.GetComponent<Text>().text;
            if (letter != null)
            {
                child.Find("Shell").gameObject.SetActive(false);
            }
        }

        Destroy(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("drop");
        transform.position = startPos;
        group.blocksRaycasts = true;
    }
}
