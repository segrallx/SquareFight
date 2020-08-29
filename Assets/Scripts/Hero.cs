﻿using UnityEngine;

public class Hero : MonoBehaviour
{
    public int mX;
    public int mY;

    public int mNextX;
    public int mNextY;

    // public static int mOldX;
    // public static int mOldY;
    private int mHP; //玩家血量
	private float mKeyHoldTime =0;
	private KeyCode mKeyHold =0;

    private static Hero __instance = null;
    public static Hero Instance()
    {
        return __instance;
    }

    public CrossAttack mSword;

    private bool OnAttack = false; //是否正处于攻击状态

    void Awake()
    {
        __instance = this;
    }

    void resetNext()
    {
        mNextX = mX;
        mNextY = mY;
    }


    // Start is called before the first frame update
    void Start()
    {
        mX = 0;
        mY = 0;

        // mOldX = 0;
        // mOldY = 0;

        mHP = 600;
        UISan.Instance().SetHP(mHP);
    }

    bool syncFloorState()
    {
        if (mX == mNextX && mY == mNextY)
        {
            return false;
        }

        //Debug.LogFormat("new pos {0}:{1}", mX , mY);

        var instance = Floors.Instance();
        if (instance.GetFloorState(mNextX, mNextY) == Floors.FloorState.None)
        {
            Floors.Instance().ChangeFloor(mX, mY, mNextX, mNextY, Floors.FloorState.Hero);
            mX = mNextX;
            mY = mNextY;
            return true;
        }
        else
        {
            mNextX = mX;
            mNextY = mY;
            return false;
        }
    }

    void posUp()
    {
        mNextX = mX;
        mNextY = mY + 1;

        //Debug.LogFormat("up pos {0}:{1}", mX , mY);
        if (mNextY > Floors.YSizeMax)
        {
            mNextY = Floors.YSizeMax;
        }
    }

    void posDown()
    {
        mNextX = mX;
        mNextY = mY - 1;

        if (mNextY < Floors.YSizeMin)
        {
            mNextY = Floors.YSizeMin;
        }
    }

    void posLeft()
    {
		mNextX = mX - 1;
        mNextY = mY;

        if (mNextX < Floors.XSizeMin)
        {
            mNextX = Floors.XSizeMin;
        }
    }

    void posRight()
    {

		mNextX = mX + 1;
        mNextY = mY;

        if (mNextX > Floors.XSizeMax)
        {
            mNextX = Floors.XSizeMax;
        }

    }

	void inputAction(KeyCode key)
	{
		switch(key)
		{
			case KeyCode.W:
				posUp();
				break;
			case KeyCode.S:
				posDown();
				break;
			case KeyCode.A:
				posLeft();
				break;
			case KeyCode.D:
				posRight();
				break;
		}
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
			mKeyHoldTime=0;
			inputAction(KeyCode.W);
        }
        else if (Input.GetKey(KeyCode.W))
        {
			mKeyHoldTime+=Time.deltaTime;
			mKeyHold=KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
			mKeyHoldTime=0;
			inputAction(KeyCode.S);
        }
		else if (Input.GetKey(KeyCode.S))
        {
			mKeyHoldTime+=Time.deltaTime;
			mKeyHold=KeyCode.S;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
			mKeyHoldTime=0;
			inputAction(KeyCode.A);
        }
		else if (Input.GetKey(KeyCode.A))
        {
			mKeyHoldTime+=Time.deltaTime;
			mKeyHold=KeyCode.A;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
			mKeyHoldTime=0;
			inputAction(KeyCode.D);
        }
		else if (Input.GetKey(KeyCode.D))
        {
			mKeyHoldTime+=Time.deltaTime;
			mKeyHold=KeyCode.D;
        }


		if(mKeyHoldTime>0.2 && Input.GetKey(mKeyHold) ) {
			mKeyHoldTime=0;
			inputAction(mKeyHold);
		}

        if (checkCanAtkNext())
        {
            doAttack();
            return;
        }

        if (syncFloorState())
        {
            doMove();
            transform.position = new Vector3(mX * 1, mY * 1, 0);
        }

    }

    // 检查下一个位置是否可以被攻击
    bool checkCanAtkNext()
    {
        if (Floors.Instance().GetFloorState(mNextX, mNextY) == Floors.FloorState.Orc)
        {
            return true;
        }
        return false;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogFormat("trigger coll {0}", other.gameObject.tag);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.LogFormat("trigger on  {0}", other.tag);
        BeAttacked();
    }

    // 玩家受到伤害
    public void BeAttacked()
    {
        mHP -= 1;
        UISan.Instance().SetHP(mHP);
        if (mHP <= 0)
        {
            UISan.Instance().ShowDead();
        }
    }

    void doAttack()
    {
        OnAttack = true;
        int dir = Direction.Dir(mX, mY, mNextX, mNextY);
        mSword.DoAttack(dir);
        resetNext();
        GameSan.Instance().AddRound();
    }

    void doMove()
    {
        mSword.CancelAttack();
        ExploreCrossPos();
        GameSan.Instance().AddRound();
    }

    // 探索周围
    public void ExploreCrossPos()
    {
        var floors = Floors.Instance();
        floors.SetFloorExplored(mX - 1, mY);
        floors.SetFloorExplored(mX + 1, mY);

        floors.SetFloorExplored(mX, mY - 1);
        floors.SetFloorExplored(mX, mY + 1);
    }


    public void afterAttack()
    {
        OnAttack = false;
    }





}
