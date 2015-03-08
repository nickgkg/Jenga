using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyTrigger : MonoBehaviour {
	ArrayList h = new ArrayList();
	GameObject arm;
	UpdateAnimation a;
	
	void Start(){
		arm = GameObject.Find("Arm");
		a = (UpdateAnimation)arm.GetComponent("UpdateAnimation");
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		h.Add(collisionInfo.gameObject.name);
		if(h.Contains("Bone_005_L_046_R") && h.Contains("Bone_005_L_018_R") && a.animIndex==2){
			GameObject temp = GameObject.Find("ArmLocation");
			Hold h2 = (Hold)temp.GetComponent("Hold");
			h2.setHeld(this.gameObject);
		}
	}
	void OnCollisionExit(Collision collisionInfo)
	{
		h.Remove(collisionInfo.gameObject.name);
	}
}
