using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syllables
{
    private List<int> probabilitySyllables;
    private List<string> syllables;
    private Dictionary<string, int> repetitionSyllables;
    private int numberSyllables;

    public Syllables()
    {
        syllables = new List<string>();
        probabilitySyllables = new List<int>();
        repetitionSyllables = new Dictionary<string, int>();
    }

    public void CalculateProbability()
    {
        var range = 0;
        foreach (var syllable in repetitionSyllables.Keys)
        {
            var probability = (double)repetitionSyllables[syllable] * 1000 / numberSyllables;
            range += (int)Math.Ceiling(probability);
            probabilitySyllables.Add(range);
            syllables.Add(syllable);
        }
        repetitionSyllables.Clear();
    }

    public string GetSyllable()
    {
        var number = new System.Random().Next(0, 1000);

        for (var i = 0; i < probabilitySyllables.Count; i++)
        {
            if (number <= probabilitySyllables[i])
            {
                return syllables[i];
            }
        }
        return "";
    }

    public void AddSyllable(string syllable)
    {
        if (repetitionSyllables.ContainsKey(syllable))
        {
            repetitionSyllables[syllable]++;
        }
        else
        {
            repetitionSyllables[syllable] = 1;
        }
        numberSyllables++;
    }
}
