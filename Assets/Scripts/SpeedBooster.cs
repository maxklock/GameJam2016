using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class SpeedBooster : MonoBehaviour {

    Vector3 _startPosition;
    Quaternion _startOrientation;

	// Use this for initialization
	void Start () {
        _startPosition = gameObject.transform.position;

    }
	
	// Update is called once per frame
	void Update () {

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, _startPosition.y + (float)Mathf.Sin(Time.time*3f)*0.4f, gameObject.transform.position.z);
        gameObject.transform.Rotate(Vector3.up, 2);
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().StartBoost();
            GameObject.Destroy(this.gameObject);

        }
    }
}
