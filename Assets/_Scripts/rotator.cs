using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class rotator : MonoBehaviour {

	[SerializeField] private VRTK_ControllerEvents vrtkLeft, vrtkRight;
	[SerializeField] private float spinSpeed = 0f;
	[SerializeField] private Transform t;


	// Use this for initialization
	void Start () {
		
	}


	void Update(){
		if(vrtkRight.gripPressed) t.Rotate (new Vector3 (0f, spinSpeed * Time.deltaTime, 0f));
		else if(vrtkLeft.gripPressed) t.Rotate (new Vector3 (0f, -1.0f * spinSpeed * Time.deltaTime, 0f));


	}

}
