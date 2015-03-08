using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour {

	// Use this for initialization
	UpdateAnimation a;
	void Start () {
		a = (UpdateAnimation)this.GetComponent("UpdateAnimation");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(1) || a.animIndex==1){
			a.animIndex = 1;
		}else if(Input.GetMouseButton(0) || a.animIndex==2){
			a.animIndex = 2;
		}else{
			a.animIndex = 0;
		}
	}
}
