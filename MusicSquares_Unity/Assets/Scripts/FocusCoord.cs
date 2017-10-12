using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCoord : Singleton<FocusCoord> 
{
	public CoordinateDelegate SignalCoordChange;
	public GenericEventDelegate SignalShiftRight;
	public GenericEventDelegate SignalShiftLeft;
	public GenericEventDelegate SignalShiftForward;
	public GenericEventDelegate SignalShiftBackward;

	private int _x = 0;
	private int _z = 0;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow)) Right();
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) Left();
		else if (Input.GetKeyDown(KeyCode.UpArrow)) Forward();
		else if (Input.GetKeyDown(KeyCode.DownArrow)) Backward();
	}

	public int GetX()
	{
		return _x;
	}

	public int GetZ()
	{
		return _z;
	}

	public void Right()
	{
		SetCoord(_x + 1, _z);
		if (SignalShiftRight != null)
		{
			SignalShiftRight();
		}
	}

	public void Left()
	{
		SetCoord(_x - 1, _z);
		if (SignalShiftLeft != null)
		{
			SignalShiftLeft();
		}
	}

	public void Forward()
	{
		SetCoord(_x, _z + 1);
		if (SignalShiftForward != null)
		{
			SignalShiftForward();
		}
	}

	public void Backward()
	{
		SetCoord(_x, _z - 1);
		if (SignalShiftLeft != null)
		{
			SignalShiftLeft();
		}
	}

	public void SetCoord(int x, int z)
	{
		_x = x;
		_z = z;

		if (SignalCoordChange != null)
		{
			SignalCoordChange(x, z);
		}
	}
}
