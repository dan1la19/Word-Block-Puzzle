using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
	private int row;
	public string [,] Letters;
	public List<int> pattern;

	public Block()
	{
		pattern = new List<int>();
		Letters = new string[3, 3];
	}

	public void AddLine(string line)
	{
		for (var i = 0; i < line.Length; i++)
		{
			Letters[row, i] = line[i].ToString();
		}
		row++;
	}
}
