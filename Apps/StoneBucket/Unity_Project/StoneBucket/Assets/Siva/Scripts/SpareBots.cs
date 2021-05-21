using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpareBots : MonoBehaviour {
	private List<BombBot> bots;
	private Vector3 dropPoint;
	//private GameObject bombBot = null;
	//public GameObject bomBotPrefab;

	//public int totalBombs = 20;

	void Start () {
		bots = new List<BombBot>();

		GameObject[] bs = GameObject.FindGameObjectsWithTag("Bomb");
		foreach(GameObject b in bs)
			bots.Add (b.GetComponent<BombBot>());

		dropPoint = transform.Find("DropPoint").position;
	}


	public void addBot(BombBot b){
		b.turnOn();
		bots.Add(b);
	}
	
	public BombBot removeBot(){
		if(bots.Count==0)
			return null;

		//should take top bot
		BombBot bestBot = bots[0];
		float maxY = bestBot.transform.position.y;
		foreach(BombBot b in bots){
			if(b.transform.position.y>maxY){
				maxY = b.transform.position.y;
				bestBot = b;
			}
		}
		bots.Remove(bestBot);
		bestBot.turnOff();
		return bestBot;
	}

	public Vector3 getDropPoint(){
		return dropPoint;
	}
	
	public int botCount(){
		return bots.Count;
	}

	/*
	public void removeAllBots(){
		foreach(BombBot b in bots){
			Destroy(b.gameObject);
		}
		bots.Clear();
	}
	*/

	/*
	void Update(){

		if(bots.Count<totalBombs){
			if(bombBot==null || bombBot.rigidbody.velocity.magnitude<0.25){
				bombBot = GameObject.Instantiate(bomBotPrefab, dropPoint,Quaternion.identity) as GameObject;
				addBot(bombBot.GetComponent<BombBot>());
				print (bots.Count);
			}
		}
	}
	*/


}
