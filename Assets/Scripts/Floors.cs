using System.Collections.Generic;
using UnityEngine;

public class Floors : MonoBehaviour
{
    public enum FloorState
    {
        None = 0,
        Hero = 1,
        Orc = 2,
    }

    public int XSizeMax;
    public int XSizeMin;
    public int YSizeMax;
    public int YSizeMin;

    private FloorState[,] mFloorState;
    //public Floor[,] mFloorObj;
    private List<Vector2Int> mFreeFloorList = new List<Vector2Int>();

    private static Floors __instance = null;

    public static Floors Instance()
    {
        return __instance;
    }

    void Awake()
    {
        __instance = this;
    }

    public void Init(FtDBound bound)
    {
        XSizeMax = bound.MaxX;
        XSizeMin = bound.MinX;
        YSizeMax = bound.MaxX;
        YSizeMin = bound.MinY;

        Debug.LogFormat("Floors XSizeMin:{0} XSizeMax:{1} YSizeMin:{2} YSizeMax:{3} ",
                        XSizeMin, XSizeMax, YSizeMin, YSizeMax);
        mFloorState = new FloorState[XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1];
        Debug.LogFormat("Floors Init {0} {1}", XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1);
        //mFloorObj = new Floor[XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1];
    }


    void setFloorState(int x, int y, FloorState state)
    {
        mFloorState[x - XSizeMin, y - YSizeMin] = state;
    }

    // public Floor GetFloorObj(int x, int y)
    // {
    //     Debug.LogFormat("GetFloorObj x:{0} y:{1} obj:{2}", x - XSizeMin, y - YSizeMin, mFloorObj);
    //     return mFloorObj[x - XSizeMin, y - YSizeMin];
    // }

    public FloorState GetFloorState(int x, int y)
    {
        return mFloorState[x - XSizeMin, y - YSizeMin];
    }

    public void SetFloorNull(int x, int y)
    {
        //GetFloorObj(x, y).SetNull();
        mFloorState[x - XSizeMin, y - YSizeMin] = FloorState.None;
        mFreeFloorList.Add(new Vector2Int(x, y));
    }

    public bool CheckFloorValid(int x, int y)
    {
        if (x < XSizeMin || x > XSizeMax)
        {
            return false;
        }

        if (y < YSizeMin || y > YSizeMax)
        {
            return false;
        }

        return true;
    }

    public void ChangeFloor(int oldX, int oldY, int x, int y, FloorState state)
    {
        Debug.LogFormat("set floor state {0}:{1} {2}:{3}", oldX, oldY, x, y);
        mFloorState[oldX - XSizeMin, oldY - YSizeMin] = FloorState.None;
        mFloorState[x - XSizeMin, y - YSizeMin] = state;

        // GetFloorObj(oldX, oldY).SetNull();
        // GetFloorObj(x, y).SetUsed();

        mFreeFloorList.Add(new Vector2Int(oldX, oldY));
        mFreeFloorList.Remove(new Vector2Int(x, y));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
        setFloorState(ret.x, ret.y, fs);
        //GetFloorObj(ret.x, ret.y).SetUsed();
        return ret;
    }


}
