using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrid : MonoBehaviour {
	[SerializeField] private Block _blockPrefab;
	[SerializeField] private int _width = 3;
	[SerializeField] private int _depth = 3;

	private List<List<Block>> _blockList = new List<List<Block>>();

	void Awake()
	{
		for (int z = 0; z < _depth; z++)
		{
			List<Block> row = new List<Block>();
			for (int x = 0; x < _width; x++)
			{
				Block block = Instantiate(_blockPrefab);
				block.SetX(x);
				block.SetZ(z);
				Vector3 blockSize = block.GetSize();
				Vector3 blockPos = new Vector3();
				blockPos.x = blockSize.x * x;
				blockPos.y = -blockSize.y / 2f;
				blockPos.z = blockSize.z * z;
				block.transform.position = blockPos;
				block.transform.SetParent(transform, true);
				row.Add(block);
			}
			_blockList.Add(row);
		}
	}
}
