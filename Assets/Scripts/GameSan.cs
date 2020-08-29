using UnityEngine;

public class GameSan : MonoBehaviour
{

    int mRound=0; // 回合数

    private static GameSan __instance = null;
    public static GameSan Instance()
    {
        return __instance;
    }


    void Awake()
    {
        __instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddRound()
    {
        mRound += 1;
    }

	public int Round()
    {
        return mRound;
    }


}
