using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrid : MonoBehaviour {
	[SerializeField] private Block _blockPrefab;
	[SerializeField] private int _xExtant = 1;
	[SerializeField] private int _zExtant = 1;

	private List<List<Block>> _blockList = new List<List<Block>>();
	private int _xCenter = 0;
	private int _zCenter = 0;

	void Awake()
	{
		InitGrid();
		FocusCoord.instance.SignalCoordChange += OnCoordChange;
	}

	void OnDestroy()
	{
		if (FocusCoord.DoesExist())
		{
			FocusCoord.instance.SignalCoordChange -= OnCoordChange;
		}
	}

	void InitGrid()
	{
		_xCenter = 0;
		_zCenter = 0;

		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
		{
			List<Block> row = new List<Block>();
			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
			{
				Block block = InstantiateBlockAtCoord(xIndex + _xCenter, zIndex + _zCenter);
				row.Add(block);
			}
			_blockList.Add(row);
		}
	}

	void OnCoordChange(int x, int z)
	{
		int xDelta = x - _xCenter;
		int zDelta = z - _zCenter;

		ShiftBy(xDelta, zDelta);
	}

	int GetMinX()
	{
		return _xCenter - _xExtant;
	}

	int GetMinZ()
	{
		return _zCenter - _zExtant;
	}
		
	int GetMaxX()
	{
		return _xCenter + _xExtant;
	}

	int GetMaxZ()
	{
		return _zCenter + _zExtant;
	}

	int GetWidth()
	{
		return _xExtant * 2 + 1;
	}

	int GetDepth()
	{
		return _zExtant * 2 + 1;
	}

	void ShiftBlockBy(Block block, int xShift, int zShift)
	{
		if (xShift >= GetWidth() || zShift >= GetDepth())
		{
			block.ResetBlock();
		}

		int xCoord = block.GetX() + xShift;
		int zCoord = block.GetZ() + zShift;

		block.SetCoord(xCoord, zCoord);
	}

	void ShiftBy(int xShift, int zShift)
	{
		if (xShift == 0) return;

		List<List<Block>> tempList = new List<List<Block>>();

		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
		{
			List<Block> tempRow = new List<Block>();
			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
			{
				Block block = _blockList[zIndex][xIndex];

				if (xShift >= GetWidth() || zShift >= GetDepth())
				{
					ShiftBlockBy(block, xShift, zShift);
				}
			}
			tempList.Add(tempRow);
		}

		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
		{
			List<Block> row = _blockList[zIndex];
			row.Clear();
			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
			{
				Block block = tempList[zIndex][xIndex];
				row[xIndex] = block;
			}
		}

		_xCenter += xShift;
		_zCenter += zShift;
	}

	Block InstantiateBlockAtCoord(int x, int z)
	{
		Block block = Instantiate(_blockPrefab);
		block.transform.SetParent(transform);
		block.SetCoord(x, z);
		return block;
	}
}
