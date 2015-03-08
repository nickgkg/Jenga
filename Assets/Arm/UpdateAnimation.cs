using UnityEngine;
using System.Collections;

public class UpdateAnimation : MonoBehaviour {
	int lastAnim = -1;
	public int animIndex = -1;
	public float speed = 1;
	Animation a;
	void Start () {
		a = (Animation)this.GetComponent("Animation");
		a.wrapMode = WrapMode.ClampForever;
	}
	
	// Update is called once per frame
	void Update () {
		if(animIndex == 0 || lastAnim+animIndex==3){
			if(lastAnim == 1){
				print ("hmm");
				a["Point"].speed = -speed;
				a.Play ("Point");
				a["Point"].time= a["Point"].time < a["Point"].length ? a["Point"].time:  a["Point"].length;
			}else if(lastAnim == 2){
				a["grab"].speed = -speed;
				a.Play ("grab");
				a["grab"].time= a["grab"].time < a["grab"].length ? a["grab"].time:  a["grab"].length;
			}
			lastAnim = 0;
		}else if(animIndex == 1 && a["grab"].time<0.01){
			a["Point"].speed = speed;
			a.Play ("Point");
			lastAnim = 1;
		}else if(animIndex == 2 && a["Point"].time<0.01){
			a["grab"].speed = speed;
			a.Play ("grab");
			lastAnim = 2;
		}
	}
}
