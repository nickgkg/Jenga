using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour {

	public GameObject block;
	public GameObject[] blocks;

	// Use this for initialization
	void Start () {
		blocks = new GameObject[54];
		int height = 0;
		float xSize = 3f;
		float ySize = 0.6f;
		float zSize = 1f;
		int dim = 0;//number of blocks on a layer
		for (;height < 10;height++){
			for (dim = -1;dim < 2; dim++){
				GameObject cubeSpawn = (GameObject)Instantiate(block, new Vector3((float)(dim*(height%2)),(float)(height*0.6),(float)(dim*(height+1)%2)), Quaternion.Euler(0,90*(height%2),0));
				cubeSpawn.transform.localScale -= new Vector3((Random.Range(0,10)/1000f),(Random.Range(0,10)/500f),(Random.Range(0,10)/1000f));
				blocks[dim+1+height*3] = cubeSpawn;
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
