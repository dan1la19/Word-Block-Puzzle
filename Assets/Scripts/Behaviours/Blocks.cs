using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class Blocks : MonoBehaviour
{
	public string FileName;

	public Blocks()
	{
	}

	public List<Block> GetBlocks()
	{
		var file = new FileStream(FileName, FileMode.Open);
		var reader = new StreamReader(file);
		var blocks = new List<Block>();
		var block = new Block();

		while (!reader.EndOfStream)
		{
			var line = reader.ReadLine();
			if (line == "")
			{
				blocks.Add(block);
				block = new Block();
				continue;
			}
			block.AddLine(reader.ReadLine());
		}
		return blocks;
	}
}
