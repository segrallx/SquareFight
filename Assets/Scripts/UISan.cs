using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISan : MonoBehaviour
{
    public GameObject mHpVal;
    public GameObject mUIDead;
    public GameObject mUIWin;

    private static UISan __instance = null;
    public static UISan Instance()
    {
        return __instance;
    }

    //private bool mIsDead = false;
    private bool mIsWin = false;

    void Awake()
    {
        __instance = this;
		mUIDead.SetActive(false);
		mUIWin.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // if (mIsDead)
        // {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        // }

		if (mIsWin)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                Next();
            }
        }

    }

    public void SetHP(int val)
    {
        mHpVal.GetComponent<UnityEngine.UI.Text>().text = string.Format("HP:{0}", val);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


    public void Next()
    {
        SceneManager.LoadScene(0);
    }


    public void ShowDead()
    {
        mUIDead.SetActive(true);
        //mIsDead = true;
    }

    public void ShowWin()
    {
        mUIWin.SetActive(true);
        mIsWin = true;
    }


}
