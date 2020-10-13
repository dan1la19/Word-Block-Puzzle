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
    private Text ScoreText;

    public Sprite Sprite;
    public float Dist { get; set; }
    public Vector3 StartPos { get; set; }
    public string FileName;
    private HashSet<int> lineX = new HashSet<int>();
    private HashSet<int> lineY = new HashSet<int>();

    void Start()
    {
        ScoreText = transform.parent.Find("Score").GetComponent<Text>();
        indexesTransforms = new Dictionary<Transform, int>();
        Dist = transform.GetChild(91).position.x - transform.GetChild(90).position.x;
        StartPos = transform.GetChild(90).position - new Vector3(Dist / 2, Dist / 2);
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
            var pos = fieldCell.position - new Vector3(Dist / 2, Dist / 2);
            var x = Math.Round(pos.x, Config.Rounding);
            var y = Math.Round(pos.y, Config.Rounding);

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
            var line = reader.ReadLine();
            words.Add(line);
            SyllablesBehaviour.ParseLine(line);
        }
        SyllablesBehaviour.CalculateProbabilitys();
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

            var word = letters.ToString(startIndex, length);
            if (word != "")
                Debug.Log(word);

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

        UpdateScore(indexesLetters.Count);
        foreach (var index in indexesLetters)
        {
            var fieldCell = transform.GetChild(index);
            //TODO Анимация

            fieldCell.gameObject.GetComponent<Image>().sprite = Sprite;
            fieldCell.Find("Text").GetComponent<Text>().text = "";
        }
        lineX = new HashSet<int>();
        lineY = new HashSet<int>();
    }

    public void UpdateScore(int points)
    {
        ScoreText.text = (int.Parse(ScoreText.text) + points).ToString();
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
