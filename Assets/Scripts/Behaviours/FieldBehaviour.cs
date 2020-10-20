using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FieldBehaviour : MonoBehaviour
{
    private Dictionary<double, Dictionary<double, Transform>> fieldCells;
    public Dictionary<Transform, int> indexesTransforms;
    private HashSet<string> words;
    public Text ScoreText;

    public HashSet<int> freeCells;
    public HashSet<int> indexesLetters;

    public Sprite SpriteDefault;
    public Sprite SpriteSelection;
    public float Dist { get; set; }
    public Vector3 StartPos { get; set; }
    private HashSet<int> lineX = new HashSet<int>();
    private HashSet<int> lineY = new HashSet<int>();
    [SerializeField] TextAsset file;
    [SerializeField] Blocks Blocks;

    void Start()
    {
        freeCells = new HashSet<int>();
        for (var i = 0; i < 100; i++)
            freeCells.Add(i);
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
        var words = new HashSet<string>();
        foreach (var line in file.text.Split('\n'))
        {
            var word = line.Remove(line.Length - 1);
            words.Add(word);
            SyllablesBehaviour.ParseLine(word);
        }
        SyllablesBehaviour.CalculateProbabilitys();
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
        FindWords(indexesLetters, lineX, 'x');
        FindWords(indexesLetters, lineY, 'y');

        lineX = new HashSet<int>();
        lineY = new HashSet<int>();
    }

    public void DeleteLetter(int index)
    {
        var fieldCell = transform.GetChild(index);
        freeCells.Add(index);
        AnimationsController.Instance.AnimateLetter(fieldCell);
        fieldCell.GetComponent<Image>().sprite = SpriteDefault;
        fieldCell.Find("Text").GetComponent<Text>().text = "";
    }

    public bool isGameOver()
    {
        for (var i = 0; i < Blocks.numberBlocks; i++)
        {
            var block = Blocks.transform.GetChild(i);
            var pattern = block.GetComponent<BlockBehaviour>().pattern;
            if (pattern.Count == 0) return false;

            foreach (var freeCell in freeCells)
            {
                foreach (var inCell in pattern)
                {
                    var contains = true;
                    foreach (var outCell in pattern)
                    {
                        var freeCellVector = new Vector2(freeCell % 10, freeCell / 10);
                        var vector = new Vector2(outCell % 3, outCell / 3)
                            - new Vector2(inCell % 3, inCell / 3);
                        var outFieldCell = vector.y * 10 + vector.x + freeCell;
                        if (freeCellVector.x + vector.x < 0 
                            || freeCellVector.x + vector.x > 9
                            || freeCellVector.y + vector.y < 0
                            || freeCellVector.y + vector.y > 9
                            || !freeCells.Contains((int)outFieldCell)) 
                        {
                            contains = false;
                            break;
                        }
                    }
                    if (contains) return false;
                }
            }
        }
        return true;
    }

    public void UpdateScore(int points)
    {
        ScoreText.text = (int.Parse(ScoreText.text) + points).ToString();
    }

    public void UpdateCheckItems(Transform fieldCell)
    {
        var index = indexesTransforms[fieldCell];
        freeCells.Remove(index);
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
