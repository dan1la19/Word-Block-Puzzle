﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BlockBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private FieldBehaviour fieldBehaviour;

    private void Start()
    {
        fieldBehaviour = transform.parent.parent.
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
            var blockCell = gameObject.transform.GetChild(i);
            var letter = blockCell.Find("Shell/Text").gameObject.GetComponent<Text>().text;
            if (letter != "")
            {
                var pos = GetPosition(blockCell.position.x, blockCell.position.y);
                Debug.Log($"{Math.Round(pos.x, Config.Rounding)} {Math.Round(pos.y, Config.Rounding)}");
                var fieldCell = fieldBehaviour.GetFieldCell(Math.Round(pos.x, Config.Rounding), Math.Round(pos.y, Config.Rounding));
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
                var fieldCell = fieldBehaviour.GetFieldCell(Math.Round(pos.x, Config.Rounding), Math.Round(pos.y, Config.Rounding));
                fieldCell.Find("Text").GetComponent<Text>().text = letter;
                fieldBehaviour.UpdateCheckItems(fieldCell);
            }
        }
        Destroy(gameObject);
        fieldBehaviour.UpdateScore(countLetters);
        fieldBehaviour.DeleteWords();

        transform.parent.GetComponent<Blocks>().NewBlock();
    }

    private Vector2 GetPosition(float x, float y)
    {
        x -= fieldBehaviour.StartPos.x;
        y -= fieldBehaviour.StartPos.y;
        x = (float)Math.Floor(x / fieldBehaviour.Dist) * fieldBehaviour.Dist;
        y = (float)Math.Floor(y / fieldBehaviour.Dist) * fieldBehaviour.Dist;
        x += fieldBehaviour.StartPos.x;
        y += fieldBehaviour.StartPos.y;
        return new Vector2(x, y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Post();
    }
}
