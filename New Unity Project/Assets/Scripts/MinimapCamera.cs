using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {
	public Texture2D minimapBorder;
	public Camera minimapCamera;
	// Use this for initialization
	void Start () {
		this.camera.pixelRect = new Rect( Screen.width - 420, Screen.height - 330, 380, 230);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if(minimapCamera.enabled) {
			GUI.DrawTexture(new Rect( Screen.width - 425, 95, 390, 240), minimapBorder);   	
		}
	}
}
