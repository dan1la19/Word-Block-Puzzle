using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using SaveSystem;

public class BlockBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    public List<int> pattern;

    [SerializeField] FieldBehaviour FieldBehaviour;
    [SerializeField] Sprite Sprite;
    [SerializeField] Saving Saving;

    private void Start()
    {
        Saving = transform.parent.parent.GetComponent<Saving>();
        FieldBehaviour = transform.parent.parent.
            Find("Field").GetComponent<FieldBehaviour>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(targetPos.x, targetPos.y, startPos.z);
    }

    public void Post()
    {
        for (var i = 0; i < 9; i++)
        {
            var blockCell = transform.GetChild(i);
            var letter = blockCell.Find("Shell/Text").GetComponent<Text>().text;
            if (letter != "")
            {
                var pos = GetPosition(blockCell.position.x, blockCell.position.y);
                var fieldCell = FieldBehaviour.GetFieldCell(Math.Round(pos.x, Config.Rounding), Math.Round(pos.y, Config.Rounding));
                var text = fieldCell != null ? fieldCell.Find("Text").GetComponent<Text>().text : null;
                if (text == null || text != "" && text != " " && letter != " ")
                {
                    transform.position = startPos;
                    return;
                }
            }
        }

        var countLetters = 0;
        for (var i = 0; i < 9; i++)
        {
            var blockCell = gameObject.transform.GetChild(i);
            var letter = blockCell.Find("Shell/Text").gameObject.GetComponent<Text>().text;
            if (letter != "")
            {
                countLetters++;
                var pos = GetPosition(blockCell.position.x, blockCell.position.y);
                var fieldCell = FieldBehaviour.
                    GetFieldCell(Math.Round(pos.x, Config.Rounding), Math.Round(pos.y, Config.Rounding));

                var index = FieldBehaviour.IndexesTransforms[fieldCell];
                if (letter != " ")
                {
                    fieldCell.GetComponent<Image>().sprite = Sprite;
                    fieldCell.Find("Text").GetComponent<Text>().text = letter;
                    fieldCell.GetChild(1).GetComponent<Text>().text = FieldBehaviour.PointsLetters[letter].ToString();
                    FieldBehaviour.OccupiedCells.Add(index, letter);
                }
                FieldBehaviour.FreeCells.Remove(index);
                FieldBehaviour.UpdateCheckItems(index);
            }
        }
        Destroy(gameObject);
        FieldBehaviour.UpdateScore(countLetters);
        FieldBehaviour.HighlightedWords();
        transform.parent.GetComponent<Blocks>().NewBlocks();
        if (FieldBehaviour.IsGameOver())
        {
            FieldBehaviour.SetRecord();
            Debug.Log("Game Over");
        }
        Saving.Save();
    }

    private Vector2 GetPosition(float x, float y)
    {
        x -= FieldBehaviour.StartPos.x;
        y -= FieldBehaviour.StartPos.y;
        x = (float)Math.Floor(x / FieldBehaviour.Dist) * FieldBehaviour.Dist;
        y = (float)Math.Floor(y / FieldBehaviour.Dist) * FieldBehaviour.Dist;
        x += FieldBehaviour.StartPos.x;
        y += FieldBehaviour.StartPos.y;
        return new Vector2(x, y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Post();
    }
}
