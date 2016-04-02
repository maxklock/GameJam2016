using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BulletsProps : MonoBehaviour {

    public float maxLifeTime = 2;
    private float rLifeTime = 0;

    private int id;

    public void Init(int playerId)
    {
        this.id = playerId;

    }
    
	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
        rLifeTime += Time.deltaTime;

        if(rLifeTime> maxLifeTime)
        {
            GameObject.Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && (int)collision.gameObject.GetComponent<Player>().Id != id)
        {
            collision.gameObject.GetComponent<Player>().ResetPosition();
        }
    }
}
