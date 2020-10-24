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
    public Dictionary<Transform, int> IndexesTransforms;
    private HashSet<string> words;
    public Text ScoreText;
    public Text Record;

    public Dictionary<string, int> PointsLetters = new Dictionary<string, int>() 
    {
        { "A", 1 },
        { "B", 3 },
        { "C", 3 },
        { "D", 2 },
        { "E", 1 },
        { "F", 4 },
        { "G", 2 },
        { "H", 4 },
        { "I", 1 },
        { "J", 8 },
        { "K", 5 },
        { "L", 1 },
        { "M", 3 },
        { "N", 1 },
        { "O", 1 },
        { "P", 3 },
        { "Q", 10 },
        { "R", 1 },
        { "S", 1 },
        { "T", 1 },
        { "U", 1 },
        { "V", 4 },
        { "W", 4 },
        { "X", 8 },
        { "Y", 4 },
        { "Z", 10 },
        { "А", 1 },
        { "Б", 3 },
        { "В", 2 },
        { "Г", 3 },
        { "Д", 2 },
        { "Е", 1 },
        { "Ж", 5 },
        { "З", 5 },
        { "И", 1 },
        { "Й", 4 },
        { "К", 2 },
        { "Л", 2 },
        { "М", 2 },
        { "Н", 1 },
        { "О", 1 },
        { "П", 2 },
        { "Р", 2 },
        { "С", 2 },
        { "Т", 2 },
        { "У", 3 },
        { "Ф", 10 },
        { "Х", 5 },
        { "Ц", 5 },
        { "Ч", 5 },
        { "Ш", 10 },
        { "Щ", 10 },
        { "Ъ", 10 },
        { "Ы", 5 },
        { "Ь", 5 },
        { "Э", 8 },
        { "Ю", 10 },
        { "Я", 4 }
    };

    public Dictionary<int, string> OccupiedCells;
    public HashSet<int> FreeCells;
    public HashSet<int> IndexesLetters;

    public Sprite SpriteDefault;
    public Sprite SpriteSelection;
    public Sprite SpriteBlock;
    public float Dist { get; set; }
    public Vector3 StartPos { get; set; }
    private HashSet<int> lineX = new HashSet<int>();
    private HashSet<int> lineY = new HashSet<int>();
    [SerializeField] TextAsset file;
    [SerializeField] public Blocks Blocks;
    [SerializeField] public Saving Saving;

    void Start()
    {
        FreeCells = new HashSet<int>();
        for (var i = 0; i < 100; i++)
            FreeCells.Add(i);
        //IndexesLetters = new HashSet<int>();
        IndexesTransforms = new Dictionary<Transform, int>();
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
            IndexesTransforms[fieldCell] = i;
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

    public void SetRecord()
    {
        Record.text = ScoreText.text;
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
        FindWords(IndexesLetters, lineX, 'x');
        FindWords(IndexesLetters, lineY, 'y');

        lineX = new HashSet<int>();
        lineY = new HashSet<int>();
    }

    public void DeleteLetter(int index)
    {
        var fieldCell = transform.GetChild(index);
        var letter = fieldCell.GetChild(0).GetComponent<Text>().text;
        UpdateScore(PointsLetters[letter]);
        FreeCells.Add(index);
        OccupiedCells.Remove(index);
        AnimationsController.Instance.AnimateLetter(fieldCell);
        fieldCell.GetComponent<Image>().sprite = SpriteDefault;
        fieldCell.Find("Text").GetComponent<Text>().text = "";
        fieldCell.GetChild(1).GetComponent<Text>().text = "";
    }

    public bool IsGameOver()
    {
        var length = Config.FieldSize;
        for (var i = 0; i < Blocks.numberBlocks; i++)
        {
            var block = Blocks.transform.GetChild(i);
            var pattern = block.GetComponent<BlockBehaviour>().pattern;
            if (pattern.Count == 0) return false;

            foreach (var freeCell in FreeCells)
            {
                foreach (var inCell in pattern)
                {
                    var contains = true;
                    foreach (var outCell in pattern)
                    {
                        var freeCellVector = new Vector2(freeCell % length, freeCell / length);
                        var vector = new Vector2(outCell % 3, outCell / 3)
                            - new Vector2(inCell % 3, inCell / 3);
                        var outFieldCell = vector.y * length + vector.x + freeCell;
                        if (freeCellVector.x + vector.x < 0 
                            || freeCellVector.x + vector.x > length - 1
                            || freeCellVector.y + vector.y < 0
                            || freeCellVector.y + vector.y > length - 1
                            || !FreeCells.Contains((int)outFieldCell)) 
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
        if (int.Parse(Record.text) < int.Parse(ScoreText.text))
        {
            SetRecord();
        }
        Saving.Save();
    }

    public void UpdateCheckItems(int index)
    {
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
