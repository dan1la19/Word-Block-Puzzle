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
	public int numberBlocks;
	public Dictionary<List<int>, Vector2> Offset = new Dictionary<List<int>, Vector2>() 
	{
		{ BlockGenerator.Templates[0], new Vector2(0, -140) },
		{ BlockGenerator.Templates[1], new Vector2(0, -190) },
		{ BlockGenerator.Templates[2], new Vector2(50, -140) },
		{ BlockGenerator.Templates[3], new Vector2(0, -140) },
		{ BlockGenerator.Templates[4], new Vector2(0, -140) },
		{ BlockGenerator.Templates[5], new Vector2(-50, -190) },
		{ BlockGenerator.Templates[6], new Vector2(50, -190) },
		{ BlockGenerator.Templates[7], new Vector2(-50, -190) },
		{ BlockGenerator.Templates[8], new Vector2(50, -190) },
		{ BlockGenerator.Templates[9], new Vector2(50, -190) },
		{ BlockGenerator.Templates[10], new Vector2(0, -140) },
		{ BlockGenerator.Templates[11], new Vector2(0, -140) },
		{ BlockGenerator.Templates[12],  new Vector2(0, -140) },
		{ BlockGenerator.Templates[13],  new Vector2(0, -140) },
	};
	[SerializeField] TextAsset file;
	[SerializeField] GameObject Block;
	[SerializeField] FieldBehaviour FieldBehaviour;

	private void Start()
	{
		blocks = GetBlocks();
		Debug.Log(blocks.Count);
		for (var i = 0; i < 3; i++)
		{
			CreateBlock(blocks[i]);
			numberBlocks++;
		}
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
			if (letter is null || letter == "" || letter == " ")
			{
				blockCell.Find("Shell").gameObject.SetActive(false);
            }
			else
			{
				blockCell.Find("Shell/Text").gameObject.GetComponent<Text>().text = letter;
				blockCell.Find("Shell/Point").gameObject.GetComponent<Text>().text = 
					FieldBehaviour.PointsLetters[letter].ToString();
			}
		}
	}

	public void NewBlocks()
	{
		numberBlocks--;
		if (numberBlocks == 0)
		{
			for (var i = 0; i < 3; i++)
			{
				CreateBlock(BlockGenerator.GenerateBlock());
				numberBlocks++;
			}
            AnimationsController.Instance.AnimateBlocks();
		}
	}

	public void CreateBlock(Block block)
	{
        var newBlock = Instantiate(Block, Block.transform.position, Quaternion.identity);
		SetParameters(newBlock, block);
		newBlock.transform.SetParent(transform, false);
		if (Offset.ContainsKey(block.Pattern))
			newBlock.GetComponent<RectTransform>().anchoredPosition = Offset[block.Pattern];
		newBlock.GetComponent<BlockBehaviour>().pattern = block.Pattern;
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
    }
	
	//public void Click()
	//{
	//	var block = new Block();
	//	block.AddLine(" ");
	//	CreateBlock(block);
	//}
}
