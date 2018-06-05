using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class model_controller : MonoBehaviour {
	[SerializeField] private float spinSpeed = 90f;
	[SerializeField] private float scaleFactor = 0.05f;
	[SerializeField] private annotation_controller ac;
	[SerializeField] private GameObject scanner, scanner_location;

	private bool mriEnable = false;


	public List<GameObject> models;
	//public List<model_serializer> ms = new List<model_serializer>();
	public int active_model = 0;

	// Use this for initialization
	void Start () {
		//load ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void rotCW () {
		transform.Rotate(new Vector3(0f, spinSpeed * Time.deltaTime, 0f));
	}

	public void rotCCW () {
		transform.Rotate(new Vector3(0f, -1.0f * spinSpeed * Time.deltaTime, 0f));
	}

	public void rotUp(){
		transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);

	}


	public void rotDown (){
		transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime * -1.0f);
	}

	public void rotReset(){
		transform.localRotation = Quaternion.identity;
	}

	public void scaleUp(){
		Vector3 t = transform.localScale;
		transform.localScale = new Vector3(t.x + scaleFactor, t.y + scaleFactor, t.z + scaleFactor);
	}

	public void scaleDown(){
		Vector3 t = transform.localScale;
		transform.localScale = new Vector3(t.x - scaleFactor, t.y - scaleFactor, t.z -scaleFactor);
	}

	public void activateModel(int i){
		Debug.Log (i);
		models [i].SetActive (true);
		models [active_model].SetActive (false);
		active_model = i;
		rotReset ();
	}

	public GameObject getActiveModel(){
		return models [active_model];
	}

	public annotation[] getAnnotations(){
		return transform.GetComponentsInChildren<annotation> (true);
	}

	public void resetModels(){
		annotation[] annots = getAnnotations ();
		foreach (annotation a in annots) {
			Destroy (a.pin);
			Destroy (a.gameObject);

		}
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		ac.save ();

	}

	public void mriTog(){
		if (mriEnable) {
			scanner.transform.SetParent (transform);
			mriEnable = false;
		} else {
			scanner.transform.SetParent (scanner_location.transform);
			scanner.transform.localPosition = new Vector3 (0f, 0f, 0f);
			scanner.transform.localRotation = new Quaternion (0f, 0f, 0f, 0f);
			scanner.transform.localScale = new Vector3 (1f, 1f, 1f);


			mriEnable = true;
		}






	}









		
}

public static class saveLoad{



}
