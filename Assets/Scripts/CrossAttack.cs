using UnityEngine;

public class CrossAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mOwer;
    public GameObject mWeapon;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DoAttack(int dir)
    {
        transform.rotation = Direction.Rotation(dir);
		Debug.LogFormat("cross atack");
		mWeapon.GetComponent<CrossWeapon>().playAttack();
    }

    public void CancelAttack()
    {
        //mSword.GetComponent<Animator>().SetInteger("Attack", 0);
    }


    public void afterAttack()
    {
		mOwer.SendMessage("afterAttack");
    }

	public void beforeAttack()
    {
		mOwer.SendMessage("beforeAttack");
    }

}
