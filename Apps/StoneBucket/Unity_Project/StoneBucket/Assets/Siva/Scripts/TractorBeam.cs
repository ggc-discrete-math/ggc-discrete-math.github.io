using UnityEngine;
using System.Collections;

public class TractorBeam : MonoBehaviour {
	
	private LineRenderer line;
	private Transform target;
	private Vector3 startPos;
	private Vector3 destination;
	private float startTime;
	
	void Start () {
		line = GetComponent<LineRenderer>();
		line.enabled = false;
		target = null;
	}
	
	void Update () {
		if(target!=null){
			
			//float target
			float delta = Time.time-startTime;
			target.position = Vector3.Lerp(startPos, destination, delta);
			
			//update target position on line
			line.SetPosition(1, target.position);

			//turn off lifting.
			if(delta>1){
				target = null;
				line.enabled = false;
				GetComponent<AudioSource>().Stop();
			}
		}
	}
	
	public void lift(Transform target, Vector3 destination){
		this.target = target;
		this.destination = destination;
		this.startTime = Time.time;
		this.startPos = target.position;
		
		line.enabled = true;
		
		//set start at tractorbeam base
		line.SetPosition(0, transform.position);
		line.SetPosition(1, target.position);
		GetComponent<AudioSource>().Play();
	}
	
	public bool isLifting(){
		return target!=null;
	}
	
	
}
