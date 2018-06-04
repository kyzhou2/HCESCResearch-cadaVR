using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using Normal.UI;
using System;

public class annotation : MonoBehaviour {
	public Transform drawLineFrom;
	public Transform drawLineTo;
	public float lineWidth = 0.001f;
	public Color lineColor = Color.black;
	protected LineRenderer line;
	protected Transform headset;
	public bool alwaysFaceHeadset = false;
	public GameObject pin;
	public GameObject keyboard;
	public KeyboardDisplay keyboardDisplay;
	public VRTK_ControllerEvents vrtk;
	public GameObject UIContainer;
	public GameObject text;
	public GameObject expandBtn, editBtn, deleteBtn, closeBtn, moveBtn;
	public int modelID;



	void OnEnable(){
		SetLine();
		headset = VRTK_DeviceFinder.HeadsetTransform();
		if (drawLineTo == null && transform.parent != null)
		{
			drawLineTo = transform.parent;
		}

	}

	protected virtual void Update()
	{
		DrawLine();
		if (alwaysFaceHeadset)
		{
			transform.LookAt(headset);
		}
	}

	protected virtual void SetLine()
	{
		line = transform.Find("Line").GetComponent<LineRenderer>();
		line.material = Resources.Load("TooltipLine") as Material;
		line.material.color = lineColor;
		#if UNITY_5_5_OR_NEWER
		line.startColor = lineColor;
		line.endColor = lineColor;
		line.startWidth = lineWidth;
		line.endWidth = lineWidth;
		#else
		line.SetColors(lineColor, lineColor);
		line.SetWidth(lineWidth, lineWidth);
		#endif
		if (drawLineFrom == null)
		{
			drawLineFrom = transform;
		}
	}

	public void expand(){
		UIContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200f, 200f);
		text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (190f, 176f);
		text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, 81f);
		expandBtn.SetActive (false);
		editBtn.SetActive (true);
		closeBtn.SetActive (true);
		deleteBtn.SetActive (true);
		moveBtn.SetActive (true);
	}

	public void close(){
		UIContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f, 30f);
		text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f, 30f);
		text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, 15f);
		expandBtn.SetActive (true);
		editBtn.SetActive (false);
		closeBtn.SetActive (false);
		deleteBtn.SetActive (false);
		moveBtn.SetActive (false);
	}

	public void delete(){
		Destroy (pin);
		Destroy (gameObject);
	}

	public void edit(){
		keyboard.SetActive (true);
		keyboardDisplay._text = text.GetComponent<Text>();
	}

	public void move(){
		

	}

	public annotationSerializer serialize(){
		annotationSerializer ret = new annotationSerializer ();
		ret.pinX = pin.transform.localPosition.x;
		ret.pinY = pin.transform.localPosition.y;
		ret.pinZ = pin.transform.localPosition.z;
		ret.flagX = transform.localPosition.x;
		ret.flagY = transform.localPosition.y;
		ret.flagZ = transform.localPosition.z;
		ret.flagRotX = transform.localRotation.x;
		ret.flagRotY = transform.localRotation.y;
		ret.flagRotZ = transform.localRotation.z;
		ret.flagRotW = transform.localRotation.w;
		ret.text = text.GetComponent<Text> ().text;
		ret.modelID = modelID;

		return ret;
	}

	// Use this for initialization
	void Start () {
		vrtk.TriggerClicked += new ControllerInteractionEventHandler(TriggerClicked);
	}

	private void TriggerClicked(object sender, ControllerInteractionEventArgs e)
	{


	}

	protected virtual void DrawLine()
	{
		if (drawLineTo != null)
		{
			line.SetPosition(0, drawLineFrom.position);
			line.SetPosition(1, drawLineTo.position);
		}
	}
}

[Serializable] public class annotationSerializer{
	public float pinX, pinY, pinZ;
	public float flagX, flagRotX, flagY, flagRotY, flagZ, flagRotZ, flagRotW;
	public string text;
	public int modelID;
}

