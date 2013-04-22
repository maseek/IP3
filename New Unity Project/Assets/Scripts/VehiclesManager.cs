using UnityEngine;
using System.Collections;

public class VehiclesManager : MonoBehaviour {
	public GameObject carPrefab;
	public GameObject busPrefab;
	public GameObject vanPrefab;
	private float carY = 0.3374233f;
	private float busY = 0.08400619f;
	private float vanY = 0.08400619f;
	
	public GameObject row1;
	public GameObject row2;
	public GameObject row3;
	public GameObject row4;
	public GameObject col1;
	public GameObject col2;
	
	private float nextSpawnRow = 0;
	private float nextSpawnCol = 0;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextSpawnRow) {
			nextSpawnRow = Time.time + Random.Range(1.0f, 2.0f);
			GameObject spawn = new GameObject();
			Vector3 spawnPoint = Vector3.zero;
			float yPos = 0;
			Quaternion spawnRotation = Quaternion.identity;
			float offset = 2.75f * Random.Range(-1, 2);
			int spawnPick = Random.Range(0, 21);
			if(spawnPick < 17) {
				spawn = carPrefab;
				yPos = carY;
			}
			else if(spawnPick < 19) {
				spawn = busPrefab;
				yPos = busY;
			}
			else {
				spawn = vanPrefab;
				yPos = vanY;
			}
			switch(Random.Range(0, 4)) {
			case 0:
				spawnPoint = new Vector3(row1.transform.position.x, yPos, row1.transform.position.z+offset);
				spawnRotation = carPrefab.transform.rotation;
				break;
			case 1:
				spawnPoint = new Vector3(row2.transform.position.x, yPos, row2.transform.position.z+offset);
				spawnRotation = carPrefab.transform.rotation;
				spawnRotation *= Quaternion.Euler(0, 180, 0);
				break;
			case 2:
				spawnPoint = new Vector3(row3.transform.position.x, yPos, row3.transform.position.z+offset);
				spawnRotation = carPrefab.transform.rotation;
				break;
			case 3:
				spawnPoint = new Vector3(row4.transform.position.x, yPos+row4.transform.position.y, row4.transform.position.z+offset);
				spawnRotation = carPrefab.transform.rotation;
				spawnRotation *= Quaternion.Euler(0, 180, 0);
				break;
			}
			Instantiate(spawn, spawnPoint, spawnRotation);
		}
		if(Time.time > nextSpawnCol) {
			nextSpawnCol = Time.time + Random.Range(2.0f, 4.0f);
			GameObject spawn = new GameObject();
			Vector3 spawnPoint = Vector3.zero;
			Quaternion spawnRotation = Quaternion.identity;
			float yPos = 0;
			float offset = 2.75f * Random.Range(-1, 1);
			int spawnPick = Random.Range(0, 11);
			if(spawnPick < 9) {
				spawn = carPrefab;
				yPos = carY;
			}
			/*else if(spawnPick < 9) {
				spawn = busPrefab;
				yPos = busY;
			}*/
			else {
				spawn = vanPrefab;
				yPos = vanY;
			}
			switch(Random.Range(0, 2)) {
			case 0:
				spawnPoint = new Vector3(col1.transform.position.x+offset, yPos, col1.transform.position.z);
				spawnRotation = carPrefab.transform.rotation;
				spawnRotation *= Quaternion.Euler(0, 90, 0);
				break;
			case 1:
				spawnPoint = new Vector3(col2.transform.position.x+offset, yPos, col2.transform.position.z);
				spawnRotation = carPrefab.transform.rotation;
				spawnRotation *= Quaternion.Euler(0, 270, 0);
				break;
			}
			Instantiate(spawn, spawnPoint, spawnRotation);
		}
	}
}
