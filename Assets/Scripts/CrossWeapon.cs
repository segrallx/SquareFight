using UnityEngine;


public class CrossWeapon : MonoBehaviour
{
    public CrossAttack mWrapper;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void afterAttack()
    {
        mWrapper.afterAttack();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogFormat("weapon coll {0}", other.gameObject.tag);
        if (other.gameObject.tag != "Hero")
        {
            return;
        }
		//GetComponent<Animation>().Stop();
    }


}
