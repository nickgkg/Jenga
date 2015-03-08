using UnityEngine;
using System.Collections;

public class Hold : MonoBehaviour {
	
	GameObject held = null;
	public GameObject arm;
	UpdateAnimation a;


	void Start(){
		a = (UpdateAnimation)arm.GetComponent("UpdateAnimation");
	}

	public void setHeld(GameObject g){
		held = g;
		held.transform.parent = arm.transform.parent; 
		((Rigidbody)held.GetComponent("Rigidbody")).isKinematic = true;
	}

	void Update () {
		if(a.animIndex!=2){
			if(held != null){
				((Rigidbody)held.GetComponent("Rigidbody")).isKinematic = false;
				held.transform.parent = null;
			}
			held = null;
		}
	}
}
