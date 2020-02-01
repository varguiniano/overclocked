using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasLookAt : MonoBehaviour {
	public GameObject cam;
	
	// Use this for initialization
	void Start () {
		cam=GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (cam.transform);
	}

}
