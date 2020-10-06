using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FieldBehaviour : MonoBehaviour
{
    private Dictionary<double, Dictionary<double, Transform>> fieldCells;
    public Dictionary<Transform, int> indexesTransforms;
    private HashSet<string> words;

    public float dist { get; set; }
    public Vector3 startPos { get; set; }
    public string FileName;
    private HashSet<int> lineX = new HashSet<int>();
    private HashSet<int> lineY = new HashSet<int>();

    void Start()
    {
        indexesTransforms = new Dictionary<Transform, int>();
        dist = transform.GetChild(91).position.x - transform.GetChild(90).position.x;
        startPos = transform.GetChild(90).position - new Vector3(dist / 2, dist / 2);
        SetFieldCells();
        words = GetWords();
    }

    private void SetFieldCells()
    {
        fieldCells = new Dictionary<double, Dictionary<double, Transform>>();
        for (var i = 0; i < 100; i++)
        {
            var fieldCell = transform.GetChild(i);
            indexesTransforms[fieldCell] = i;
            var pos = fieldCell.position - new Vector3(dist / 2, dist / 2);
            var x = Math.Round(pos.x, 6);
            var y = Math.Round(pos.y, 6);

            if (!fieldCells.ContainsKey(x))
            {
                fieldCells[x] = new Dictionary<double, Transform>();
            }
            fieldCells[x][y] = fieldCell;
        }
    }

    private HashSet<string> GetWords()
    {
        var file = new FileStream(FileName, FileMode.Open);
        var reader = new StreamReader(file);
        var words = new HashSet<string>();
        while (!reader.EndOfStream)
        {
            words.Add(reader.ReadLine());
        }
        reader.Close();
        return words;
    }


    private void AddIndexesLetters(HashSet<int> indexesLetters, HashSet<int> line, char flag)
    {
        foreach (var c in line)
        {
            var letters = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var index = flag == 'x' ? i * 10 + c : c * 10 + i;
                var letter = transform.GetChild(index).Find("Text").GetComponent<Text>().text;
                if (letter == "")
                {
                    letter = " ";
                }
                letters.Append(letter);
            }

            var startIndex = 0;
            var length = 0;
            for (var i = 2; i <= 10; i++)
            {
                for (var j = 0; j <= 10 - i; j++)
                {
                    if (words.Contains(letters.ToString(j, i)) && i > length)
                    {
                        startIndex = j;
                        length = i;
                    }
                }
            }

            for (var i = startIndex; i < startIndex + length; i++)
            {
                var index = flag == 'x' ? i * 10 + c : c * 10 + i;
                indexesLetters.Add(index);
            }
        }
    }

    public void DeleteWords()
    {
        var indexesLetters = new HashSet<int>();
        AddIndexesLetters(indexesLetters, lineX, 'x');
        AddIndexesLetters(indexesLetters, lineY, 'y');

        var points = indexesLetters.Count; //Баллы за удаление слов
        foreach (var index in indexesLetters)
        {
            transform.GetChild(index).Find("Text").GetComponent<Text>().text = "";
        }
        lineX = new HashSet<int>();
        lineY = new HashSet<int>();
    }

    public void UpdateCheckItems(Transform fieldCell)
    {
        var index = indexesTransforms[fieldCell];
        lineY.Add(index / 10);
        lineX.Add(index % 10);
    }

    public Transform GetFieldCell(double x, double y)
    {
        if (fieldCells.ContainsKey(x) && fieldCells[x].ContainsKey(y)) 
            return fieldCells[x][y];
        return null;
    }
}
