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
	private Quaternion initQuat;
	
	// A reference angle representing how the armband is rotated about the wearer's arm, i.e. roll.
	// Set by making the fingers spread pose or pressing "r".
	private float _referenceRoll = 0.0f;
	public static float theta = 0;	
	private float height = 2.5f;
	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	private float radius = 10;
	private float armDistance = 0;//min is 0
	private float armShift = 0;//min is 0 (left/right)
	private float armHeight = 0;
	private float initGyrZ = 0;
	private float initGyrX = 0;
	private float initGyrY = 0;
	private float camHeight = 3.5f;
	private ThalmicMyo thalmicMyo;
	UpdateAnimation a;

	private float initRotY = 0;
	void Start () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		initGyrZ = thalmicMyo.gyroscope.z;
		initGyrX = thalmicMyo.gyroscope.x;
		initGyrY = thalmicMyo.gyroscope.y;
		initQuat = thalmicMyo.transform.rotation;
		a = (UpdateAnimation)arm.GetComponent("UpdateAnimation");
		print (a.gameObject.name);
		initRotY = myo.transform.rotation.eulerAngles.y;
		Update();
	}

	// Update is called once per frame.
	void Update ()
	{
		// Access the ThalmicMyo component attached to the Myo object.

		// Update references when the pose becomes fingers spread or the q key is pressed.
		int moveDirection = 0;
		if (thalmicMyo.pose == Pose.WaveIn && Input.GetKey ("space") || Input.GetKey("a")) {
			if(Input.GetKey(KeyCode.LeftShift))
				armShift-=0.3f;
			else
				moveDirection = 1;
			//print (1);
			
			//ExtendUnlockAndNotifyUserAction(thalmicMyo);
		}
		
		if (thalmicMyo.pose == Pose.WaveOut && Input.GetKey ("space") || Input.GetKey("d")){
			if(Input.GetKey(KeyCode.LeftShift))
				armShift+=0.3f;
			else
				moveDirection = -1;
			//ExtendUnlockAndNotifyUserAction(thalmicMyo);
		}

		if (Input.GetKey ("h")) {
			initQuat = thalmicMyo.transform.rotation;
			initGyrZ = thalmicMyo.gyroscope.z;
			initGyrX = thalmicMyo.gyroscope.x;
			initGyrY = thalmicMyo.gyroscope.y;
		}

		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
		}

		if(thalmicMyo.pose == Pose.FingersSpread || Input.GetMouseButton(1)){
			a.animIndex = 1;
		}else if(thalmicMyo.pose == Pose.Fist || Input.GetMouseButton(0)){
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
			armHeight+=0.1f;
		}
		if (Input.GetKey ("q")) {
			armHeight-=0.1f;
		}

		// Update references. This anchors the joint on-screen such that it faces forward away
		// from the viewer when the Myo armband is oriented the way it is when these references are taken.
		if (moveDirection != 0) {
			theta += -1*moveDirection*0.025f;
		}

		if (thalmicMyo.pose == Pose.DoubleTap){
			Object.Destroy(GameObject.Find("Hub"));
			Application.LoadLevel (0);
		}
		if (!Input.GetKey ("space")) {
			if(false){
				Quaternion armPos = thalmicMyo.transform.rotation;
				armHeight += (armPos.x-initQuat.x);
				armShift += (armPos.y-initQuat.y);
				armDistance += (armPos.z-initQuat.z)/5;
			}else{

				float adjGyrZ = thalmicMyo.gyroscope.z - initGyrZ;
				float adjGyrX = thalmicMyo.gyroscope.x - initGyrX;
				float adjGyrY = thalmicMyo.gyroscope.y - initGyrY;
				if (adjGyrZ > 10 || adjGyrZ < -10) {
					if (adjGyrZ > 80){
						adjGyrZ /= 3;
					}
					armDistance -= adjGyrZ/300;
				} if ((adjGyrX > 10 || adjGyrX < -10)  && !(a.animIndex ==2 && Hold.held!=null)){
					armHeight -= adjGyrX / 200;
				} if (adjGyrY > 10 || adjGyrY < -10) {//left and right
					armShift -= adjGyrY / 100;
				}
				//Quaternion armPos = thalmicMyo.transform.rotation;
				//armDistance += (armPos.z-initQuat.z)/5;
			}
		}
		if (armDistance > 10) {
			armDistance = 10f;
		} else if (armDistance < -2) {
			armDistance = -2f;
		}
		if (armShift > 35) {
			armShift = 35f;
		} else if (armShift < -35) {
			armShift = -35f;
		}
		if (armHeight > 8) {
			armHeight = 8;
		} else if (armHeight < 0) {
			armHeight = 0;
		}
		
		//camera transforms
		transform.localPosition = new Vector3((float)(radius*Mathf.Cos(theta))
		                                      ,camHeight+armHeight
		                                      ,(float)(radius*Mathf.Sin(theta)));
		transform.localRotation = Quaternion.Euler(15f+0.2f*armHeight
		                                           ,-1*theta*180/Mathf.PI-90
		                                           , 0f);
		//print (armShift);
		//arm transforms
		armLocation.transform.localPosition = new Vector3((float)((radius-armDistance+3)*Mathf.Cos(theta))
		                                                  ,camHeight+armHeight-3
		                                                  ,(float)((radius-armDistance+3)*Mathf.Sin(theta)));
		armLocation.transform.localRotation = Quaternion.Euler(-10,
		                                                       -1*theta*180/Mathf.PI+180+armShift,
		                                                       0);
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