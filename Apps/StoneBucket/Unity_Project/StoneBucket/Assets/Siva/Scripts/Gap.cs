using UnityEngine;
using System.Collections;

public class Gap : MonoBehaviour {

	private float startTime;
	private static float gapTime = 0.25f;
	private bool stretching = false;
	private bool shrinking = false;
	
	void Start(){
		GetComponent<Renderer>().enabled = false;
	}
	
	void Update () {
		if(stretching || shrinking){
			float percentDone = (Time.time - startTime)/gapTime;
			Vector3 scale = transform.localScale;
			if(stretching)
				scale.z = Mathf.Lerp(0, 0.3f, percentDone);
			else
				scale.z = Mathf.Lerp(0.3f, 0, percentDone);
			transform.localScale = scale;
			if(percentDone>=1){
				if(shrinking)
					GetComponent<Renderer>().enabled = false;
				stretching = false;
				shrinking = false;
			}
		}
		
	
	}
	
	public void stretch(){
		GetComponent<Renderer>().enabled = true;
		startTime = Time.time;
		stretching = true;
	}
	
	public void shrink(){
		startTime = Time.time;
		shrinking = true;
	}
	
	public bool isStretching(){
		return stretching;
	}
	
	public bool isShrinking(){
		return shrinking;
	}
}
