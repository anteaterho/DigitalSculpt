using UnityEngine;
using System.Collections;
using Vectrosity;

public class CursorCircle : MonoBehaviour {

	SculptVertices sv;
	public TextAsset vectorObj;
	public Material lineMaterial;


	// Use this for initialization
	void Start () {
		sv = GameObject.Find ("Main Camera").GetComponent<SculptVertices> ();
//		var circlePoints = VectorLine.BytesToVector3Array (vectorObj.bytes);
//		VectorLine line = new VectorLine ("Circle", circlePoints, lineMaterial, 2.0f);
//
//		VectorManager.ObjectSetup (gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = sv.MousePos;
		this.transform.LookAt (Camera.main.transform.position);
//		this.transform.position = Camera.main.ViewportToWorldPoint (Input.mousePosition);
	}
}
