using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Blocks : MonoBehaviour
{
	private List<Block> blocks;
	private int index;

	[SerializeField] TextAsset file;
	public GameObject Block;
	public GameObject Canvas;

	private void Start()
	{
		blocks = GetBlocks();
		Debug.Log(blocks.Count);
		for (var i = 0; i < 3; i++)
		{
			CreateBlock(blocks[i]);
			index++;
		}
		//index++;
		//NewBlocks();
	}

	private List<Block> GetBlocks()
	{
		var blocks = new List<Block>();
		var block = new Block();
		foreach (var line in file.text.Split('\n'))
		{
			if (line == "") continue;
			var word = line.Remove(line.Length - 1);
			if (word == "*")
			{
				blocks.Add(block);
				block = new Block();
				continue;
			}
			block.AddLine(word);
		}
		return blocks;
	}

	private void SetParameters(GameObject newBlock, Block block)
	{
		for (var i = 0; i < 9; i++) 
		{
			var letter = block.Letters[i / 3, i % 3];
			var blockCell = newBlock.transform.GetChild(i);
			if (letter is null || letter == ""/* || letter == " "*/)
			{
				blockCell.Find("Shell").gameObject.SetActive(false);
			}
			else
			{
				blockCell.Find("Shell/Text").gameObject.GetComponent<Text>().text = letter;
			}
		}
	}

	public void NewBlocks()
	{
		index--;
		if (index == 0)
		{
			for (var i = 0; i < 3; i++)
			{
				CreateBlock(BlockGenerator.GenerateBlock());
				index++;
			}
		}
		//CreateBlock(blocks[index]);
		//index++;
	}

	private void CreateBlock(Block block)
	{
		//TODO Block
		var newBlock = Instantiate(Block, Block.transform.position, Quaternion.identity);
		SetParameters(newBlock, block);
		newBlock.transform.SetParent(Canvas.transform, false);
	}
	
	public void Click()
	{
		var block = new Block();
		block.AddLine(" ");
		CreateBlock(block);
	}
}
