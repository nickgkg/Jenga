using UnityEngine;
using System.Collections;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class CameraControl : MonoBehaviour {
	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	public GameObject armLocation = null;
	public GameObject arm = null;
	
	// A rotation that compensates for the Myo armband's orientation parallel to the ground, i.e. yaw.
	// Once set, the direction the Myo armband is facing becomes "forward" within the program.
	// Set by making the fingers spread pose or pressing "r".
	private Quaternion _antiYaw = Quaternion.identity;
	
	// A reference angle representing how the armband is rotated about the wearer's arm, i.e. roll.
	// Set by making the fingers spread pose or pressing "r".
	private float _referenceRoll = 0.0f;
	private float theta = 0;	
	private float height = 2.5f;
	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	private float radius = 10;
	private float armDistance = 0;//min is 0
	private float initAccX = 0;
	private ThalmicMyo thalmicMyo;
	UpdateAnimation a;
	void Start () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		initAccX = thalmicMyo.gyroscope.x;
		a = (UpdateAnimation)arm.GetComponent("UpdateAnimation");
		print (a.gameObject.name);
		Update();
	}

	// Update is called once per frame.
	void Update ()
	{
		// Access the ThalmicMyo component attached to the Myo object.

		// Update references when the pose becomes fingers spread or the q key is pressed.
		int moveDirection = 0;
		if (thalmicMyo.pose == Pose.WaveIn && Input.GetKey ("space")) {
			moveDirection = 1;
			//print (1);
			
			//ExtendUnlockAndNotifyUserAction(thalmicMyo);
		}
		
		if (thalmicMyo.pose == Pose.WaveOut && Input.GetKey ("space")){
			moveDirection = -1;
			//ExtendUnlockAndNotifyUserAction(thalmicMyo);
		}

		if (Input.GetKey ("h")) {
			initAccX = thalmicMyo.accelerometer.x;
		}

		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
		}

		if(thalmicMyo.pose == Pose.FingersSpread){
			print ("hello");
			a.animIndex = 1;
		}else if(thalmicMyo.pose == Pose.Fist){
			a.animIndex = 2;
		}else{
			a.animIndex = 0;
		}

		//if (Input.GetKeyDown ("r")) {
		//	moveDirection = 1;
		//}


		if (Input.GetKey ("w")) {
			armDistance+=0.1f;
		}
		if (Input.GetKey ("s")) {
			armDistance-=0.1f;
		}

		if (Input.GetKey ("e")) {
			height+=0.1f;
		}
		if (Input.GetKey ("q")) {
			height-=0.1f;
		}

		// Update references. This anchors the joint on-screen such that it faces forward away
		// from the viewer when the Myo armband is oriented the way it is when these references are taken.
		if (moveDirection != 0) {
			theta += -1*moveDirection*0.025f;
		}
		float adjAccX = thalmicMyo.gyroscope.x - initAccX;
		//if (adjAccX > 10 || adjAccX < -10) {
			armDistance += adjAccX/200;
		//}

		if (armDistance > 8) {
			armDistance = 8;
		} else if (armDistance < -3) {
			armDistance = -3;
		}
		
		transform.localPosition = new Vector3((float)(radius*Mathf.Cos(theta)),height+1,(float)(radius*Mathf.Sin(theta)));
		transform.localRotation = Quaternion.Euler(15f, -1*theta*180/Mathf.PI-90, 0f);

		armLocation.transform.localPosition = new Vector3((float)((radius-armDistance+3)*Mathf.Cos(theta)),height-1.5f,(float)((radius-armDistance+3)*Mathf.Sin(theta)));
		armLocation.transform.localRotation = Quaternion.Euler(-1*myo.transform.rotation.eulerAngles.z-45, -1*theta*180/Mathf.PI+180, myo.transform.rotation.eulerAngles.x);
	}
	


	
	// Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
	// recognized.
	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;
		
		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}
		
		myo.NotifyUserAction ();
	}
}