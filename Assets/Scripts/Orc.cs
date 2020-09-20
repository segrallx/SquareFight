using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Element
{
    public bool mPosChange;

	protected int mRound; //执行回合
	protected State mState = State.Idle;

	public enum State
    {
        Idle,
		Move,
		Attack,
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
		AIUpdate();

		if (mPosChange)
        {
            mPosChange = false;
            transform.position = new Vector3(mX * 1, mY * 1, 0);
        }

    }

	public override Type ElementType()
	{
		return Element.Type.Orc;
	}

	public virtual void AIUpdate(){}
}
