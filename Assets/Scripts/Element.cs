using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
	public int mX=0;
    public int mY=0;

	public void SetPos(int x, int y)
    {
        mX = x;
        mY = y;
    }

	public enum Type {
		None = 0,
        Hero = 1,
        Orc = 2,
		Rock = 3,
		Gate = 4,
	}

	public abstract Type ElementType();
}
