using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class Blocks : MonoBehaviour
{
	public string FileName;
	public List<Block> blocks;
	public int index = 0;
	public GameObject Block;
	public GameObject Canvas;

	private void Start()
	{
		blocks = GetBlocks();
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
			if (line == "*")
			{
				blocks.Add(block);
				block = new Block();
				continue;
			}
			block.AddLine(line);
		}
		reader.Close();
		return blocks;
	}

	public void NewBlock()
	{
		if (index == blocks.Count)
		{
			Debug.Log("Blocks Ended");
			return;
		}
		Debug.Log("New Block");
		var newBlock = Instantiate(Block, Block.transform.position, Quaternion.identity) as GameObject;
		newBlock.transform.SetParent(Canvas.transform, false);
		index++;
	}
}
