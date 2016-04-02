using UnityEngine;
using System.Collections;

public class hubbo : MonoBehaviour {

    public Animator animator;

    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("running", true);
        }
	}
}
