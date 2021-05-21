using UnityEngine;
using System.Collections;

public class ArmRota : MonoBehaviour {
	
	public float speed = 10f;
	public Vector3 axis = Vector3.up;
	
	private float targetRot;
	private bool complete;
	
	void Start(){
		targetRot = getCurrentRot();
		complete = true;
	}
	
	void Update () {
		if(complete==false){
			float currentRot = getCurrentRot();
			float angle = Time.deltaTime*speed;
			if(Mathf.Approximately(currentRot, targetRot)){
				GetComponent<AudioSource>().Stop();
				complete = true;
			}
			else if(currentRot<targetRot){
				if(currentRot+angle>targetRot)
					angle = targetRot - currentRot;
				transform.Rotate(angle*axis);
			}
			else{
				if(currentRot-angle<targetRot)
					angle = currentRot - targetRot;
				transform.Rotate(-angle*axis);
			}
		}
	}
	
	public void rotateTo(float degrees){
		GetComponent<AudioSource>().Play();
		complete=false;
		this.targetRot = degrees;
	}
	
	public bool inMotion(){
		return !complete;
	}
		
	private float getCurrentRot(){
		return Vector3.Dot(transform.localRotation.eulerAngles, axis);
	}
}
