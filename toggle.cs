using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggle : MonoBehaviour {

	public void tog(){
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
