using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrid : MonoBehaviour {
	[SerializeField] private Block _blockPrefab;
	[SerializeField] private int _xExtant = 1;
	[SerializeField] private int _zExtant = 1;

	private List<List<Block>> _grid = new List<List<Block>>();

	private int _xCenter = 0;
	private int _zCenter = 0;
	private Block _focusedBlock;

	void Awake()
	{
		InitGrid();
		FocusCoord.instance.SignalCoordChange += OnFocusCoordChange;
	}

	void OnDestroy()
	{
		if (FocusCoord.DoesExist())
		{
			FocusCoord.instance.SignalCoordChange -= OnFocusCoordChange;
		}
	}

	void InitGrid()
	{
		_xCenter = 0;
		_zCenter = 0;

		int xOrigin = _xCenter - _xExtant;
		int zOrigin = _zCenter - _zExtant;

		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
		{
			List<Block> row = new List<Block>();
			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
			{
				Block block = InstantiateBlockAtCoord(xIndex + xOrigin, zIndex + zOrigin);
				row.Add(block);
			}
			_grid.Add(row);
		}

		UpdateFocusedBlock();
	}

	void OnFocusCoordChange(int newFocusX, int newFocusZ)
	{
		int focusChangeX = newFocusX - _xCenter;
		int focusChangeZ = newFocusZ - _zCenter;

		_xCenter = newFocusX;
		_zCenter = newFocusZ;

		List<Block> deadBlocks = new List<Block>();
		List<List<Block>> tempGrid = new List<List<Block>>();

		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
		{
			List<Block> tempRow = new List<Block>();
			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
			{
				Block block = _grid[zIndex][xIndex];

				int newBlockX = block.GetX();
				int newBlockZ = block.GetZ();

				if (newBlockX < GetMinX())
				{
					newBlockX = GetMaxX();
				}
				else if (newBlockX > GetMaxX())
				{
					newBlockX = GetMinX();
				}

				if (newBlockZ < GetMinZ())
				{
					newBlockZ = GetMaxZ();
				}
				else if (newBlockZ > GetMaxZ())
				{
					newBlockZ = GetMinZ();
				}

				if (newBlockX != block.GetX() || newBlockZ != block.GetZ())
				{
					block.ResetBlock();
					block.SetCoord(newBlockX, newBlockZ);
					deadBlocks.Add(block);
				}
				else
				{
					tempRow.Add(block);
				}
			}
			tempGrid.Add(tempRow);
		}

		// you need to add the deadblocks back
		// but you should do this within the above for loop
		// use arrays instead of lists, so you can assign exact indices
		// (for example, assign the deadblock when the deadblock is made, to the END of the array [or beginning])

		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
		{
			List<Block> row = _grid[zIndex];
			row.Clear();
			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
			{
				Block block = tempGrid[zIndex][xIndex];
				row.Add(block);
			}
		}

		UpdateFocusedBlock();
	}

	void UpdateFocusedBlock()
	{
		if (_focusedBlock != null)
		{
			_focusedBlock.Unfocus();
		}

		int xIndex = _xCenter - GetMinX();
		int zIndex = _zCenter - GetMinZ();

		Block focusedBlock = _grid[zIndex][xIndex];
		focusedBlock.Focus();
		Debug.Log(focusedBlock.GetX() + ", " + focusedBlock.GetZ());
		_focusedBlock = focusedBlock;
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

	bool IsCoordInGrid(int x, int z)
	{
		return x >= GetMinX() && x <= GetMaxX() && z >= GetMinZ() && z <= GetMaxZ();
	}

//
//	void ShiftBlockBy(Block block, int xShift, int zShift)
//	{
//		if (xShift >= GetWidth() || zShift >= GetDepth())
//		{
//			block.ResetBlock();
//		}
//
//		int xCoord = block.GetX() + xShift;
//		int zCoord = block.GetZ() + zShift;
//
//		block.SetCoord(xCoord, zCoord);
//	}
//
//	void ShiftBy(int xShift, int zShift)
//	{
//		if (xShift == 0 && zShift == 0) return;
//
//		List<List<Block>> tempList = new List<List<Block>>();
//
//		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
//		{
//			List<Block> tempRow = new List<Block>();
//			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
//			{
//				Block block = _blockList[zIndex][xIndex];
//
//				if (xShift >= GetWidth() || zShift >= GetDepth())
//				{
//					ShiftBlockBy(block, xShift, zShift);
//				}
//
//				tempRow.Add(block);
//			}
//			tempList.Add(tempRow);
//		}
//
//		for (int zIndex = 0; zIndex < GetDepth(); zIndex++)
//		{
//			List<Block> row = _blockList[zIndex];
//			row.Clear();
//			for (int xIndex = 0; xIndex < GetWidth(); xIndex++)
//			{
//				Block block = tempList[zIndex][xIndex];
//				row.Add(block);
//			}
//		}
//
//		_xCenter += xShift;
//		_zCenter += zShift;
//	}

	Block InstantiateBlockAtCoord(int x, int z)
	{
		Block block = Instantiate(_blockPrefab);
		block.transform.SetParent(transform);
		block.SetCoord(x, z);
		return block;
	}
}
