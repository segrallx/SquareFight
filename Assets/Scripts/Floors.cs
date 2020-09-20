using System.Collections.Generic;
using UnityEngine;

public class Floors : MonoBehaviour
{

    public int XSizeMax;
    public int XSizeMin;
    public int YSizeMax;
    public int YSizeMin;

    private Element.Type[,] mElement;
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
        YSizeMax = bound.MaxY;
        YSizeMin = bound.MinY;

        Debug.LogFormat("Floors XSizeMin:{0} XSizeMax:{1} YSizeMin:{2} YSizeMax:{3} ",
                        XSizeMin, XSizeMax, YSizeMin, YSizeMax);
        mElement = new Element.Type[XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1];
        Debug.LogFormat("Floors Init {0} {1}", XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1);
        //mFloorObj = new Floor[XSizeMax - XSizeMin + 1, YSizeMax - YSizeMin + 1];
    }


    public void SetElementType(int x, int y, Element.Type state)
    {
        mElement[x - XSizeMin, y - YSizeMin] = state;
    }

    public Element.Type GetElementType(int x, int y)
    {
        return mElement[x - XSizeMin, y - YSizeMin];
    }

    public void SetFloorNull(int x, int y)
    {
        //GetFloorObj(x, y).SetNull();
        mElement[x - XSizeMin, y - YSizeMin] = Element.Type.None;
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

    public void ChangeFloor(int oldX, int oldY, int x, int y, Element.Type state)
    {
        Debug.LogFormat("set floor state {0}:{1} {2}:{3}", oldX, oldY, x, y);
        mElement[oldX - XSizeMin, oldY - YSizeMin] = Element.Type.None;
        mElement[x - XSizeMin, y - YSizeMin] = state;

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
    public Vector2Int PopRandomFreePos(Element.Type fs)
    {
        var rnd = new System.Random();
        var idx = rnd.Next(mFreeFloorList.Count);
        var ret = mFreeFloorList[idx];
        mFreeFloorList.RemoveAt(idx);
        SetElementType(ret.x, ret.y, fs);
        //GetFloorObj(ret.x, ret.y).SetUsed();
        return ret;
    }


}
