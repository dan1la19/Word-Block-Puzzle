using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockGenerator
{
	private static List<List<int>> templates = new List<List<int>>()
	{
		new List<int>() { 1 },
		new List<int>() { 1, 4 },
		new List<int>() { 3, 4 },
		new List<int>() { 3, 4, 5 },
		new List<int>() { 1, 4, 7 },
		new List<int>() { 1, 4, 5 },
		new List<int>() { 0, 1, 4 },
		new List<int>() { 1, 2, 4 },
		new List<int>() { 1, 4, 3 },
		new List<int>() { 0, 1, 3, 4 },
	};

	//private static List<List<char>> letters = new List<List<char>>()
	//{
	//	new List<char>(){ 'о', 'и', 'а', 'ы', 'ю', 'я', 'э', 'е', 'у', 'е' },
	//	new List<char>(){ 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л',
	//		'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ъ' }
	//};

	private static List<int> probabilityPatterns = new List<int>() { 33, 13, 13, 10, 10, 3, 3, 3, 3, 6 };
	//private static List<int> probabilityLetters = new List<int>() { 66, 33 };

	public static Block GenerateBlock()
    {
		var block = new Block();
		//var rnd = new System.Random();
		var pattern = GetRandomElement(templates, probabilityPatterns);
		var i = 0;
		string line;
		if (pattern.Count == 1) 
			line = SyllablesBehaviour.SyllablesOf1.GetSyllable();
		else if (pattern.Count == 2) 
			line = SyllablesBehaviour.SyllablesOf2.GetSyllable();
		else if (pattern[0] == 3 || pattern[2] == 7) 
			line = SyllablesBehaviour.SyllablesOf3.GetSyllable();
		else if (pattern.Count == 4) 
			line = SyllablesBehaviour.SyllablesOf2.GetSyllable() 
				+ SyllablesBehaviour.SyllablesOf2.GetSyllable();
		else line = SyllablesBehaviour.SyllablesOf2.GetSyllable()
				+ SyllablesBehaviour.SyllablesOf1.GetSyllable();

		foreach (var cell in pattern)
		{
			//var listLetter = GetRandomElement(letters, probabilityLetters);
			//var index = rnd.Next(0, listLetter.Count - 1);
			//block.Letters[i / 3, i % 3] = listLetter[index].ToString();
			try
			{
				block.Letters[cell / 3, cell % 3] = line[i].ToString();
			}
			catch
			{
				Debug.Log($"i = {i}, line = {line}.");
			}
			i++;
		}
		return block;
    }

	private static T GetRandomElement<T>(List<T> list, List<int> probabilities)
	{
		var number = new System.Random().Next(0, 100);
		var ranges = new List<int>();
		var startRange = 0;
		for (var i = 0; i < probabilities.Count - 1; i++)
		{
			ranges.Add(startRange + probabilities[i]);
			startRange += probabilities[i];
		}
		ranges.Add(100);
		for (var i = 0; i < ranges.Count; i++)
		{
			if (number <= ranges[i])
			{
				return list[i];
			}
		}
		return default;
	}
}
