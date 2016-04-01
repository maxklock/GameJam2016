using UnityEngine;
using System.Collections;

public class VulcanoLight : MonoBehaviour {

    Light pointLight;

    float rf = 0;

	// Use this for initialization
	void Start () {
        pointLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Random.value>0.8)
            rf += Random.value / 2;

        pointLight.intensity = 6 + Mathf.Sin(Time.realtimeSinceStartup+rf)*3;


    }
}
