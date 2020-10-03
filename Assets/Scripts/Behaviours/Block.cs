using System;

public class Block
{
	private int row;

	public Cell[,] Cells;

	public Block()
	{
		Cells = new Cell[3, 3];
	}

	public void AddLine(string line)
	{
		for (var i = 0; i < line.Length; i++)
		{
			Cells[row, i] = new Cell(line[i]);
		}

		row++;
	}
}
