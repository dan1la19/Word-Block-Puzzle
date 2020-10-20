using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SyllablesBehaviour
{
    public static Syllables SyllablesOf1 = new Syllables();
    public static Syllables SyllablesOf2 = new Syllables();
    public static Syllables SyllablesOf3 = new Syllables();

    public static void ParseLine(string line)
    {
        for (var i = 0; i < line.Length - 2; i++)
        {
            SyllablesOf1.AddSyllable(line[i].ToString());
            SyllablesOf2.AddSyllable(line.Substring(i, 2));
            SyllablesOf3.AddSyllable(line.Substring(i, 3));
        }
        Debug.Log(line);
        SyllablesOf1.AddSyllable(line[line.Length - 2].ToString());
        SyllablesOf1.AddSyllable(line[line.Length - 1].ToString());
        SyllablesOf2.AddSyllable(line.Substring(line.Length - 2, 2));
    }

    public static void CalculateProbabilitys()
    {
        SyllablesOf1.CalculateProbability();
        SyllablesOf2.CalculateProbability();
        SyllablesOf3.CalculateProbability();
    }
}
