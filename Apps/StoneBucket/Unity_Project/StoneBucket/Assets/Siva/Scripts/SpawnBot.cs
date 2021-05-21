using UnityEngine;
using System.Collections;

public class SpawnBot : MonoBehaviour {
	public GameObject bombBotPrefab;
	private GameObject bombBot;
	
	private Animator anim;
	private int hitState;
	private int doPunchBool;
	
	private GameObject fist;
	
	private bool returnBot;
	
	
	void Start () {
		anim = GetComponent<Animator>();
		hitState = Animator.StringToHash("Base Layer.Hit");
		doPunchBool = Animator.StringToHash("DoPunch");
		fist = GameObject.Find("prop_batteringRam_fist");
		bombBot = null;
		returnBot = false;
	}
	
	void Update () {
		if(bombBot!=null && 
			anim.GetCurrentAnimatorStateInfo(0).nameHash == hitState){
			
			//Anumation has started
			//Don't loop animation.
			anim.SetBool(doPunchBool, false);
			
			if(returnBot && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.75f){
				Destroy(bombBot);
				bombBot=null;
				GetComponent<AudioSource>().Stop();
			}
			else if(!returnBot && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.25f){
				bombBot.GetComponent<Renderer>().enabled = true;
				bombBot=null;
				GetComponent<AudioSource>().Stop();
			}
		}
	}
	
	public GameObject newBombBot(){
		GetComponent<AudioSource>().Play();
		returnBot = false;
		//build new bombot
		bombBot = GameObject.Instantiate(bombBotPrefab, 
				transform.position+1.7f*transform.forward + 2.2f*Vector3.up,
				transform.rotation) as GameObject;
		//start animation.
		anim.SetBool(doPunchBool, true);
		//build bombbot
		bombBot.transform.parent = fist.transform;
		bombBot.name = "BombBot";
		bombBot.GetComponent<Renderer>().enabled = false;
		return bombBot;
	}
	
	public void returnBombBot(BombBot b){
		GetComponent<AudioSource>().Play();
		returnBot = true;
		bombBot = b.gameObject;
		bombBot.transform.parent = fist.transform;
		anim.SetBool(doPunchBool, true);
	}
	
	public bool isAnimating(){
		//not working.
		return anim.GetCurrentAnimatorStateInfo(0).nameHash == hitState ||
			anim.GetBool(doPunchBool);
	}
}
