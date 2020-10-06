using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public GameObject mResUsed;    //当前使用的底图.
    public GameObject mResUnExplored;    //没有被探索的底图.
    public GameObject mNpc;

    public bool mExplored = false; // 是否探索

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetUsed()
    {
        mResUsed.SetActive(true);
        Debug.LogFormat("this");
    }

    public void SetNull()
    {



        mResUsed.SetActive(false);
    }

    // 设置为探索过.
    public void SetExplored()
    {
        mResUnExplored.SetActive(false);
        mExplored = true;
        if (mNpc != null)
        {
            mNpc.SetActive(true);
        }
    }

    public bool GetExplored()
    {
        return mExplored;
    }


}
