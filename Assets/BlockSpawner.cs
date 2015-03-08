using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour {

	public GameObject block;
	public GameObject[] blocks;

	// Use this for initialization
	void Start () {
		blocks = new GameObject[54];
		int height = 0;
		int dim = 0;//number of blocks on a layer
		for (;height < 18;height++){
			for (;dim < 3; dim++){
				GameObject cubeSpawn = (GameObject)Instantiate(block, new Vector3((float)0,(float)(dim*0.6),(float)height), transform.rotation);
				blocks[dim+height*3] = cubeSpawn;
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
