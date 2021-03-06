using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Normal.UI;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class annotation_controller : MonoBehaviour {

	[SerializeField] private GameObject pin;
	[SerializeField] private GameObject pin_holder, flag_holder;
	[SerializeField] private GameObject flag;
	[SerializeField] private model_controller mc;
	[SerializeField] private VRTK_ControllerEvents vrtk;
	[SerializeField] private GameObject keyboard;
	[SerializeField] private KeyboardDisplay keyboardDisplay;

	private List<annotationSerializer> annotations = new List<annotationSerializer> ();
	private bool enable = false;
	private int clickCount = 0;
	private GameObject newPin, newFlag, activeModel;

	// Use this for initialization
	void Start () {
		vrtk.TriggerClicked += new ControllerInteractionEventHandler(TriggerClicked);
		vrtk.TriggerUnclicked += new ControllerInteractionEventHandler(TriggerUnclicked);
		load ();
	}

	private void TriggerClicked(object sender, ControllerInteractionEventArgs e)
	{

		if (enable) {
			activeModel = mc.getActiveModel ();
			if (clickCount == 0) {
				newPin = Instantiate (pin, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				clickCount++;
				pin_holder.SetActive (false);
				flag_holder.SetActive (true);
			} else if (clickCount == 1) {
				newFlag = Instantiate (flag, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				//newFlag.GetComponent<VRTK_ObjectTooltip> ().drawLineTo = newPin.transform;
				newFlag.GetComponent<annotation> ().drawLineTo = newPin.transform;
				//newFlag.GetComponent<annotation> ().drawLineFrom = newFlag.transform;
				flag_holder.SetActive (false);
				keyboard.SetActive (true);
				newPin.transform.SetParent (activeModel.transform);
				newFlag.transform.SetParent (activeModel.transform);
				newFlag.GetComponent<annotation> ().pin = newPin;
				keyboardDisplay._text = newFlag.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Text>();
				newFlag.GetComponent<annotation> ().keyboardDisplay = keyboardDisplay;
				newFlag.GetComponent<annotation> ().keyboard = keyboard;
				newFlag.GetComponent<annotation> ().vrtk = vrtk;
				newFlag.GetComponent<annotation> ().modelID = mc.active_model;
				clickCount = 0;
				enable = false;
			}
		}
	}

	public void loadAnnoatation(float x, float y, float z, float rotx, float roty, float rotz, float rotw, float pinx, float piny, float pinz, int modelID, string text){
		newPin = Instantiate (pin, Vector3.back , Quaternion.identity) as GameObject;
		newFlag = Instantiate (flag, Vector3.back , Quaternion.identity) as GameObject;
		newFlag.GetComponent<annotation> ().drawLineTo = newPin.transform;
		newPin.transform.SetParent (mc.models[modelID].transform);
		newFlag.transform.SetParent (mc.models[modelID].transform);
		newPin.transform.localPosition = new Vector3 (pinx, piny, pinz);
		newFlag.transform.localPosition = new Vector3 (x, y, z);
		newFlag.transform.localRotation = new Quaternion (rotx, roty, rotz, rotw);
		newFlag.GetComponent<annotation> ().pin = newPin;
		newFlag.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Text>().text = text;
		newFlag.GetComponent<annotation> ().keyboardDisplay = keyboardDisplay;
		newFlag.GetComponent<annotation> ().keyboard = keyboard;
		newFlag.GetComponent<annotation> ().vrtk = vrtk;
		newFlag.GetComponent<annotation> ().modelID = modelID;
	}
		

	private void TriggerUnclicked(object sender, ControllerInteractionEventArgs e)
	{
		
	}



	public void addAnnotation(){
		enable = true;
		pin_holder.SetActive (true);
		keyboard.SetActive (false);
	}

	public void save(){
		
		annotation[] annots;
		annots = mc.getAnnotations();
		annotations = new List<annotationSerializer> ();

		foreach (annotation a in annots) {
			annotationSerializer asTemp;
			asTemp = a.serialize();
			annotations.Add (asTemp);
		}
			
		BinaryFormatter bf = new BinaryFormatter ();
		if (File.Exists (Application.persistentDataPath + "\\savedState.gd")) {
			File.Delete (Application.persistentDataPath + "\\savedState.gd");
		}

		FileStream file = File.Create(Application.persistentDataPath + "\\savedState.gd");

		bf.Serialize(file, annotations);
		file.Close();
	}

	public void load(){
		if(File.Exists(Application.persistentDataPath + "\\savedState.gd")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "\\savedState.gd", FileMode.Open);
			annotations = (List<annotationSerializer>) bf.Deserialize(file);
			file.Close ();

			for (int i = 0; i < annotations.Count; i++) {
				annotationSerializer a = annotations [i];
				loadAnnoatation (a.flagX, a.flagY, a.flagZ, a.flagRotX, a.flagRotY, a.flagRotZ, a.flagRotW, a.pinX, a.pinY, a.pinZ, a.modelID, a.text);

			}

		}

		//Debug.Log(ms[0].annotations[0] + "out");
	}

	public void quit(){
		Application.Quit();
	}
}
