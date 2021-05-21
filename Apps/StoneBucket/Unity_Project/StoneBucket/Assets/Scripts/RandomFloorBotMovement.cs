using UnityEngine;
using System.Collections;

public class RandomFloorBotMovement : MonoBehaviour
{
	public Vector2 minimumCoordinates;
	public Vector2 maximumCoordinates;
	public Vector2 speedVariance;
	public Vector2 newDestinationTimeVariance;
	public Vector2 newSpeedTimeVariance;
	
	
	private UnityEngine.AI.NavMeshAgent nav;
	private float newDestinationTime;
	private float destinationTimer;
	private float newSpeedTime;
	private float speedTimer;
	
	
	void Awake ()
	{
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		SetRandomDestination();
		SetRandomSpeed();
	}
	

	void FixedUpdate ()
	{
		destinationTimer += Time.deltaTime;
		speedTimer += Time.deltaTime;
		
		if(destinationTimer >= newDestinationTime)
			SetRandomDestination();
		
		if(speedTimer >= newSpeedTime)
			SetRandomSpeed();
	}
	
	
	void SetRandomDestination ()
	{
		float newX = Random.Range(minimumCoordinates.x, maximumCoordinates.x);
		float newZ = Random.Range(minimumCoordinates.y, maximumCoordinates.y);
		nav.destination = new Vector3(newX, -1f, newZ);
		SetNewDestinationTime();
	}
	
	void SetRandomSpeed ()
	{
		nav.speed = Random.Range(speedVariance.x, speedVariance.y);
		SetNewSpeedTime();
	}
	
	void SetNewDestinationTime ()
	{
		newDestinationTime = Random.Range(newDestinationTimeVariance.x, newDestinationTimeVariance.y);
		destinationTimer = 0f;
	}
	
	
	void SetNewSpeedTime ()
	{
		newSpeedTime = Random.Range(newSpeedTimeVariance.x, newSpeedTimeVariance.y);
		speedTimer = 0f;
	}
}
