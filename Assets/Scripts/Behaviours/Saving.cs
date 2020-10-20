using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saving : MonoBehaviour
{
    [SerializeField] FieldBehaviour FieldBehaviour;
    public Data Data;

    public void Start()
    {
        Data = LoadData();
        Unload();
    }

    public void Save()
    {
        var crypt = new Cryptography();
        UpdateData();
        var encrypted = crypt.Encrypt(Data);
        EasySave.Save("Save", encrypted);
    }

    private void Unload()
    {
        FieldBehaviour.ScoreText.text = Data.ScoreText;

        //for (var i = 0; i < 100; i++)
        //{
        //    var fieldCell = FieldBehaviour.transform.GetChild(i);
        //    fieldCell.GetChild(0).GetComponent<Text>().text
        //        = Data.FieldCells[i].GetChild(0).GetComponent<Text>().text;
        //    fieldCell.GetComponent<Image>().sprite
        //        = Data.FieldCells[i].GetComponent<Image>().sprite;
        //}
            
    }

    private void UpdateData()
    {
        Data.ScoreText = FieldBehaviour.ScoreText.text;

        //for (var i = 0; i < 100; i++)
        //{
        //    var fieldCell = FieldBehaviour.transform.GetChild(i);
        //    Data.FieldCells[i].GetChild(0).GetComponent<Text>().text 
        //        = fieldCell.GetChild(0).GetComponent<Text>().text;
        //    Data.FieldCells[i].GetComponent<Image>().sprite 
        //        = fieldCell.GetComponent<Image>().sprite;
        //}
    }

    public Data LoadData()
    {
        if (EasySave.HasKey<string>("Save"))
        {
            var crypt = new Cryptography();
            return crypt.Decrypt<Data>(EasySave.Load<string>("Save"));
        }
        return new Data();
    }
}
