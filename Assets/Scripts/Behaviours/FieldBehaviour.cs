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
    public Text ScoreText;

    public HashSet<int> indexesLetters;
    
    public Sprite SpriteDefault;
    public Sprite SpriteSelection;
    public float Dist { get; set; }
    public Vector3 StartPos { get; set; }
    public string FileName;
    private HashSet<int> lineX = new HashSet<int>();
    private HashSet<int> lineY = new HashSet<int>();

    void Start()
    {
        indexesLetters = new HashSet<int>();
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

    private void FindWords(HashSet<int> indexesLetters, HashSet<int> line, char flag)
    {
        foreach (var c in line)
        {
            var indexes = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                indexes.Add(flag == 'x' ? i * 10 + c : c * 10 + i);
            }

            var letters = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var letter = transform.GetChild(indexes[i]).Find("Text").GetComponent<Text>().text;
                if (letter == "")
                {
                    letter = " ";
                }
                letters.Append(letter);
            }

            for (var length = 2; length <= 10; length++)
            {
                for (var startIndex = 0; startIndex <= 10 - length; startIndex++)
                {
                    if (words.Contains(letters.ToString(startIndex, length)))
                    {
                        for (var k = startIndex; k < startIndex + length; k++)
                        {
                            indexesLetters.Add(indexes[k]);
                            transform.GetChild(indexes[k]).GetComponent<Image>().sprite = SpriteSelection;
                        }
                    }
                }
            }
        }
    }

    public void HighlightedWords()
    {
        //Debug.Log($"lineX {lineX.Count} lineY {lineY.Count}");
        FindWords(indexesLetters, lineX, 'x');
        FindWords(indexesLetters, lineY, 'y');

        lineX = new HashSet<int>();
        lineY = new HashSet<int>();
    }

    public void DeleteLetter(Transform fieldCell)
    {
        AnimationsController.Instance.AnimateLetter(fieldCell);
        fieldCell.GetComponent<Image>().sprite = SpriteDefault;
        fieldCell.Find("Text").GetComponent<Text>().text = "";
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
