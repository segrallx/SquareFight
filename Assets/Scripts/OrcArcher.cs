using UnityEngine;

public class OrcArcher : Orc
{
    static int mi = 0;

    private int[][] m4Dir = new int[4][] {
        new int[]{0,1},
        new int[]{0,-1},
        new int[]{1,0},
        new int[]{-1,0},
    };

    public CrossAttack mTooth;
	public bool mAttacking = false;
    private int mAttackRound = -1; // 上一次攻击的回合

    // Start is called before the first frame update
    void Start()
    {
    }

	void initAnimator()
	{
		var animator = GetComponent<Animator>();
		for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
		{
			var animation = animator.runtimeAnimatorController.animationClips[i];
			AnimationEvent evt = new AnimationEvent();
			evt.time = 1f * animation.length;
			evt.functionName = "animationOver";
			evt.stringParameter = animation.name;
			animation.AddEvent(evt);
		}
		Debug.LogFormat("add animation event");
	}

	void initAnimation()
	{
		var animation = GetComponent<Animation>();
		AnimationEvent evt = new AnimationEvent();
		evt.time = 1f * animation.clip.length;
		evt.functionName = "animationOver";
		evt.stringParameter = animation.name;
		animation.clip.AddEvent(evt);
		animation.Play();
		Debug.LogFormat("add animation event");
	}


    // Start is called before the first frame update
    void Awake()
    {
		if (mi == 0)
        {
			//initAnimator();
			initAnimation();
            mi = 1;
        }
    }

    void animationOver(string name)
    {
        Debug.LogFormat("animation over {0}", name);
        tryAction();
    }


    // Update is called once per frame
    public override void AIUpdate()
    {
        //tryAction();
    }

    bool hasRound()
    {
        //Debug.LogFormat("round {0} round to {1}", mRound, GameSan.Instance().Round());
        if (mRound >= GameSan.Instance().Round())
        {
            return false;
        }
        return true;
    }

    bool hasAttackRound()
    {
        //Debug.LogFormat("round {0} round to {1}", mRound, GameSan.Instance().Round());
        if (mAttackRound != GameSan.Instance().Round())
        {
            return true;
        }
        return false;
    }


    void addRound()
    {
        mRound += 1;
    }

    void setAttackRound()
    {
        mAttackRound = GameSan.Instance().Round();
        mRound = GameSan.Instance().Round();
    }


    void tryAction()
    {
        if (mAttacking)
        {
            return;
        }

        //Debug.LogFormat("try action");
        if (hasAttackRound() && tryAttack())
        {
            setAttackRound();
            return;
        }
        else if (hasRound() && tryMove())
        {
            addRound();
            return;
        }
        else
        {
            // doIdle();
            // if (hasRound())
            // {
            //     addRound();
            // }
        }
    }


    // 选一个离玩家最近的方向移动.
    bool tryMove()
    {
        float distance = 100000;
        Vector2Int posRet = Vector2Int.zero;
        bool match = false;
        var floor = Floors.Instance();
        var hero = Hero.Instance();
        var cur = new Vector2Int(mX, mY);
        var posHero = new Vector2Int(hero.mX, hero.mY);

        if (Vector2Int.Distance(cur, posHero) <= 1)
        {
            return false;
        }

        for (var i = 0; i < 4; i++)
        {
            var dir = m4Dir[i];
            var mX2 = mX + dir[0];
            var mY2 = mY + dir[1];

            if (floor.CheckFloorValid(mX2, mY2) &&
                floor.GetElementType(mX2, mY2) == Element.Type.None)
            {
                var pos1 = new Vector2Int(mX2, mY2);
                var distanceTmp = Vector2Int.Distance(pos1, posHero);
                if (distanceTmp < distance)
                {
                    distance = distanceTmp;
                    posRet = pos1;
                    match = true;
                }
            }
        }

        if (match)
        {
            Floors.Instance().ChangeFloor(mX, mY, posRet.x, posRet.y, Element.Type.Orc);
            Debug.LogFormat("do move from {0}:{1} to {2}:{3}", mX, mY, posRet.x, posRet.y);
            mX = posRet.x;
            mY = posRet.y;
            mPosChange = true;
            doMove();
            return true;
        }

        return false;
    }

    void doIdle()
    {
        //mState = State.Idle;
    }

    void doMove()
    {
        //mState = State.Move;
    }


    bool tryAttack()
    {
        var hero = Hero.Instance();
        var posHero = new Vector2Int(hero.mX, hero.mY);
        var cur = new Vector2Int(mX, mY);
        int dir = 0;
        bool match = false;

        if ((mX == hero.mX || mY == hero.mY) && Vector2Int.Distance(cur, posHero) <= 3)
        {
            dir = Direction.Dir(cur, posHero);
            match = true;
        }

        if (!match)
        {
            return false;
        }

        mTooth.DoAttack(dir);

        return match;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogFormat("silly trigger on  {0}", other.tag);
        if (other.tag == "HeroWeapon")
        {
            GoDead();
        }
    }

    void GoDead()
    {
        Destroy(gameObject);
        Floors.Instance().SetFloorNull(mX, mY);
    }

    void afterAttack()
    {
		Debug.LogFormat("archer afterAttack ");
		mAttacking = false;
    }

	void beforeAttack()
    {
		Debug.LogFormat("archer beforeAttack ");
        mAttacking = true;
    }

}
