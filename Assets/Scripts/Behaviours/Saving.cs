using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Saving : MonoBehaviour
{
    [SerializeField] FieldBehaviour FieldBehaviour;
    public Data Data;

    public void Start()
    {
        Data = Load();
        Unload();
    }


    private void Unload()
    {
        FieldBehaviour.ScoreText.text = Data.ScoreText;
        FieldBehaviour.OccupiedCells = Data.GetOccupiedCells();
        FieldBehaviour.IndexesLetters = new HashSet<int>();
        FieldBehaviour.Record.text = Data.Record;

        foreach (var i in Data.IndexesLetters) 
        {
            FieldBehaviour.IndexesLetters.Add(i);
        }

        foreach (var index in FieldBehaviour.OccupiedCells.Keys)
        {
            var fieldCell = FieldBehaviour.transform.GetChild(index);
            fieldCell.GetChild(0).GetComponent<Text>().text =
                FieldBehaviour.OccupiedCells[index];
            fieldCell.GetChild(1).GetComponent<Text>().text =
                FieldBehaviour.PointsLetters[FieldBehaviour.OccupiedCells[index]].ToString();
            var image = FieldBehaviour.transform.GetChild(index).GetComponent<Image>();
            if (FieldBehaviour.IndexesLetters.Contains(index))
                image.sprite = FieldBehaviour.SpriteSelection;
            else
                image.sprite = FieldBehaviour.SpriteBlock;
        }

        foreach(var block in Data.GetBlocks())
        {
            FieldBehaviour.Blocks.CreateBlock(block);
        }
    }

    private void Download()
    {
        Data.ScoreText = FieldBehaviour.ScoreText.text;
        Data.Record = FieldBehaviour.Record.text;
        Data.SetOccupiedCells(FieldBehaviour.OccupiedCells);
        Data.IndexesLetters = FieldBehaviour.IndexesLetters.ToList();

        Data.Patterns = new List<List<int>>();
        for (var i = 0; i < FieldBehaviour.Blocks.transform.childCount; i++)
        {
            var pattern = FieldBehaviour.Blocks.transform.
                GetChild(i).GetComponent<BlockBehaviour>().pattern;
            Data.Patterns.Add(pattern);
        } 
        
    }

    public void Save()
    {
        var crypt = new Cryptography();
        Download();
        var encrypted = crypt.Encrypt(Data);
        EasySave.Save("Save", encrypted);
    }

    public Data Load()
    {
        if (EasySave.HasKey<string>("Save"))
        {
            var crypt = new Cryptography();
            return crypt.Decrypt<Data>(EasySave.Load<string>("Save"));
        }
        return new Data();
    }
}
