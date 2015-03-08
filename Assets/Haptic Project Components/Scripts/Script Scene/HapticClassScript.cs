using UnityEngine;
using System.Collections;

public class HapticClassScript : MonoBehaviour {

	public string HapticMode;
	public int ModeIndex;

	public string TouchableFace;

	public GameObject myHapticCamera;
	public GameObject workSpaceObj;
	public GameObject hapticCursor;
	
	//SetHapticWorkSpace Values
	public float[] myWorkSpacePosition = new float[3];
	public float[] myWorkSpaceSize = new float[3];

	public float maxPenetration;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
