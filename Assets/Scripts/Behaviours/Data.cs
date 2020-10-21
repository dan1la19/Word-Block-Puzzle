using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;

public class Data
{
    public string ScoreText;
    public List<int> indexes;
    public List<string> letters;
    public List<int> IndexesLetters;
    public List<List<int>> Patterns;
    public string Record;

    public Data()
    {
        Patterns = new List<List<int>>();
        Record = "0";
        ScoreText = "0";
        indexes = new List<int>();
        letters = new List<string>();
        IndexesLetters = new List<int>();
    }
    
    public void AddBlock()
    {

    }

    public List<Block> GetBlocks()
    {
        return new List<Block>();
    }

    public void SetOccupiedCells(Dictionary<int, string> occupiedCells)
    {
        indexes = new List<int>();
        letters = new List<string>();
        foreach (var cell in occupiedCells.Keys)
        {
            indexes.Add(cell);
            letters.Add(occupiedCells[cell]);
        }
    }

    public Dictionary<int, string> GetOccupiedCells()
    {
        var occupiedCells = new Dictionary<int, string>();
        if (indexes.Count == 0) return occupiedCells;
        for (var i = 0; i < indexes.Count; i++)
        {
            occupiedCells[indexes[i]] = letters[i];
        }
        return occupiedCells;
    }
}
