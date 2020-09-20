using UnityEngine;

public class OrcSpawner : MonoBehaviour
{
    // public GameObject[] mOrcs;
    // private float mSpawnTime;
    // private int mOrcCnt;
    // private int idx = 0;

    // void Start()
    // {
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (mOrcCnt >= 10)
    //     {
    //         return;
    //     }

    //     mSpawnTime += Time.deltaTime;
    //     if (mSpawnTime > 0.1)
    //     {
    //         mSpawnTime = 0;
    //         SpawnOrc();
    //         mOrcCnt++;
    //     }

    // }


    // void SpawnOrc()
    // {
    //     if (!Floors.Instance().HasFreePos())
    //     {
    //         return;
    //     }
	// 	var hero = Hero.Instance();

    //     var posInt = Floors.Instance().PopRandomFreePos(Floors.FloorState.Orc);
    //     Debug.LogFormat("spawn pos {0}", posInt);
    //     var posFloat = new Vector3(posInt.x * Floors.FloorSize, posInt.y * Floors.FloorSize);
    //     //var mOrc = mOrcs[idx % mOrcs.Length];
	// 	var mOrc = mOrcs[1];
    //     var obj = Instantiate(mOrc, posFloat, Quaternion.identity, transform);
    //     obj.GetComponent<Orc>().SetPos(posInt.x, posInt.y);
	// 	Floors.Instance().GetFloorObj(posInt.x, posInt.y).mNpc = obj;
	// 	obj.SetActive(false);

    //     idx += 1;
    // }

    // // void Reset()
    // // {
    // // 	mOrcCnt=0;
    // // }




}
