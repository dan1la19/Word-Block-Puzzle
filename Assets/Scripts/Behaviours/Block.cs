using System;
using UnityEngine;

public class Block
{
	private int row;

	public BlockCell[,] Cells;

	public Block()
	{
		Cells = new BlockCell[3, 3];
	}

	public void AddLine(string line)
	{
		for (var i = 0; i < line.Length; i++)
		{
			Cells[row, i] = new BlockCell(line[i]);
		}
		row++;
	}
}
