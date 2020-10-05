using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldBehaviour : MonoBehaviour
{
    private Dictionary<double, Dictionary<double, Transform>> fieldCells;
    public float dist { get; set; }
    public Vector3 startPos { get; set; }

    void Start()
    {
        dist = transform.GetChild(90).position.x - transform.GetChild(91).position.x;
        startPos = transform.GetChild(90).position;// - new Vector3(dist / 2, dist / 2);

        fieldCells = new Dictionary<double, Dictionary<double, Transform>>();
        for (var i = 0; i < 100; i++)
        {
            var fieldCell = transform.GetChild(i);
            var x = Math.Round(fieldCell.position.x/* - dist / 2*/, 6);
            var y = Math.Round(fieldCell.position.y /*- dist / 2*/, 6);

            if (!fieldCells.ContainsKey(x))
            {
                fieldCells[x] = new Dictionary<double, Transform>();
            }
            fieldCells[x][y] = fieldCell;
        }
    }

    public Transform GetFieldCell(double x, double y)
    {
        if (fieldCells.ContainsKey(x) && fieldCells[x].ContainsKey(y)) 
            return fieldCells[x][y];
        return null;
    }
}
