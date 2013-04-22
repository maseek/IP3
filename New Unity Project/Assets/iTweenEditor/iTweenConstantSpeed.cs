/*
 * Developed by Felipe Teixeira
 * E-mail: felipetnh@gmail.com
 * 07/June/2012
**/

using UnityEngine;
using System.Collections.Generic;

public class iTweenConstantSpeed : MonoBehaviour {
	
	iTweenPath path;
	Vector3[] position;
	List<Vector3> nodes = new List<Vector3>(){Vector3.zero};
	public int amount = 100;
	public float distance = 2;
	public string pathName;
	
	void Start(){
		path = this.gameObject.GetComponent("iTweenPath") as iTweenPath;
		position = iTweenPath.GetPath(pathName);
		nodes[0] = position[0];
		int atual = 0;
		for( int i = 0; i < amount; i++ ){
			if( Vector3.Distance(nodes[atual],iTween.PointOnPath(position,(float)i/amount)) > distance ){
				nodes.Add( iTween.PointOnPath(position,(float)i/amount) );
				atual++;
			}
		}
		nodes.Add(position[position.Length-1]);
		
		path.nodes = nodes;
		//path.nodes.Clear();
		path.nodeCount = nodes.Count;
		//path.nodes.AddRange(nodes.ToArray());
		
		//this.enabled = false;
	}
}
