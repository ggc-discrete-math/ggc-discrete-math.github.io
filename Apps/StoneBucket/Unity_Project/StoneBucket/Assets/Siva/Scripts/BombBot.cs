using UnityEngine;
using System.Collections;

public class BombBot : MonoBehaviour {

	void FixedUpdate(){
		/*
		if(transform.position.y <=1){
			//float at y=1
			//rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
		}
		*/
	}
	
	public void turnOn(){
		enabled = true;
		if(gameObject.GetComponent<Rigidbody>()==null)
			gameObject.AddComponent<Rigidbody>();
		GetComponent<Collider>().enabled = true;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		GetComponent<Rigidbody>().drag = 3f;
		GetComponent<Rigidbody>().angularDrag = 0.3f;
		GetComponent<Rigidbody>().AddTorque(10*Vector3.right);
	}
	
	public void turnOff(){
		enabled = false;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}
	
	void OnCollisionEnter(Collision c){
		
		if(GetComponent<Rigidbody>()!=null){
			if(c.collider.name.StartsWith("prop_robotArm"))
				return;
			GetComponent<AudioSource>().volume = c.relativeVelocity.magnitude;
			GetComponent<AudioSource>().Play();
		}
	}
}
