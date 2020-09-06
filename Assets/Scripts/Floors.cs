using System.Collections.Generic;
using UnityEngine;

public class Floors : MonoBehaviour
{

	public enum FloorState {
		None = 0,
		Hero = 1,
		Orc = 2,
	}

    public const float FloorSize = 1f;

    private const int xsize = 8;
    private const int ysize = 5;

    public static int XSizeMax = xsize - 1;
    public static int XSizeMin = -xsize;

    public static int YSizeMax = ysize - 1;
    public static int YSizeMin = -ysize + 1;

    public GameObject mFloor001;

    private static FloorState[,] mFloorState =
		new FloorState[XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1];
	private static Floor[,] mFloorObj =
		new Floor[XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1];
    private static List<Vector2Int> mFreeFloorList = new List<Vector2Int>();


    private static Floors __instance = null;

    public static Floors Instance()
    {
        return __instance;
    }


    void setFloorRaw(int x, int y, FloorState state)
    {
        mFloorState[x - XSizeMin, y - YSizeMin] = state;
    }

	public Floor GetFloorObj(int x, int y)
    {
        return mFloorObj[x - XSizeMin, y - YSizeMin];
    }

    public FloorState GetFloorState(int x, int y)
    {
        return mFloorState[x - XSizeMin, y - YSizeMin];
    }

	public void SetFloorExplored(int x, int y)
    {
		if(!CheckFloorValid(x,y)) {
			return;
		}
		mFloorObj[x - XSizeMin, y - YSizeMin].SetExplored();
    }

	public bool CheckFloorExplored(int x, int y)
    {
		if(!CheckFloorValid(x,y)) {
			return false;
		}
		return mFloorObj[x - XSizeMin, y - YSizeMin].mExplored;
    }


    public void SetFloorNull(int x, int y)
    {
		GetFloorObj(x, y).SetNull();
		mFloorState[x - XSizeMin, y - YSizeMin]= FloorState.None;
		mFreeFloorList.Add(new Vector2Int(x, y));
    }



	public bool CheckFloorValid(int x, int y)
	{
		if (x < XSizeMin || x> XSizeMax) {
			return false;
		}

		if (y < YSizeMin || y> YSizeMax) {
			return false;
		}

		return true;
	}

    public void ChangeFloor(int oldX, int oldY, int x, int y, FloorState state)
    {
        //Debug.LogFormat("set floor state {0}:{1} {2}:{3}", oldX , oldY, x, y);
        mFloorState[oldX - XSizeMin, oldY - YSizeMin] = FloorState.None;
        mFloorState[x - XSizeMin, y - YSizeMin] = state;

		GetFloorObj(oldX, oldY).SetNull();
		GetFloorObj(x, y).SetUsed();

        mFreeFloorList.Add(new Vector2Int(oldX, oldY));
        mFreeFloorList.Remove(new Vector2Int(x, y));
    }


    void Awake()
    {
        __instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
		initMap();
    }

    // Update is called once per frame
    void Update()
    {
    }


	// 初始化地图.
    void initMap()
    {
		var hero = Hero.Instance();
        for (var x = XSizeMin; x <= XSizeMax; x++)
        {
            for (var y = YSizeMin; y <= YSizeMax; y++)
            {
                var pos = new Vector3(x * FloorSize, y * FloorSize);
                var obj = Instantiate(mFloor001, pos, Quaternion.identity, transform);
				var floor = obj.GetComponent<Floor>();
				mFloorObj[x - XSizeMin, y - YSizeMin] = floor;

                if (x == hero.mX && y == hero.mY)
                {
					floor.SetUsed();
					floor.SetExplored();
                    setFloorRaw(x, y, FloorState.Hero);
                }
                else
                {
					floor.SetNull();
                    setFloorRaw(x, y, 0);
                    mFreeFloorList.Add(new Vector2Int(x, y));
                }
            }
        }

		hero.ExploreCrossPos();
    }

	// 检查是否还有空位
    public bool HasFreePos()
    {
        return mFreeFloorList.Count > 0;
    }


    // 返回一个当前的空位
    public Vector2Int PopRandomFreePos(FloorState fs)
    {
        var rnd = new System.Random();
        var idx = rnd.Next(mFreeFloorList.Count);
        var ret = mFreeFloorList[idx];
        mFreeFloorList.RemoveAt(idx);
        setFloorRaw(ret.x, ret.y, fs);
		GetFloorObj(ret.x, ret.y).SetUsed();
        return ret;
    }


}
