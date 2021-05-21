using UnityEngine;
using System.Collections;

public class WheelsRotation : MonoBehaviour
{
	public float relativeSpeed = 100f;
	
	private UnityEngine.AI.NavMeshAgent nav;
	private Transform[] wheels = new Transform[4];
	
	
	void Awake ()
	{
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		wheels[0] = GameObject.Find("vehicle_rcLand_wheel_frontLeft").transform;
		wheels[1] = GameObject.Find("vehicle_rcLand_wheel_frontRight").transform;
		wheels[2] = GameObject.Find("vehicle_rcLand_wheel_rearLeft").transform;
		wheels[3] = GameObject.Find("vehicle_rcLand_wheel_rearRight").transform;
	}
	
	
	void Update ()
	{
		float navSpeed = nav.velocity.magnitude;
		foreach(Transform wheel in wheels)
		{
			wheel.Rotate(Vector3.right * relativeSpeed * navSpeed * Time.deltaTime);
		}
	}
}
