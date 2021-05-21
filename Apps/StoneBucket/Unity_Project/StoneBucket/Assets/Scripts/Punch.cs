using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour
{
	private Animator anim;
	private int hitState;
	private int doPunchBool;
	
	
	void Awake ()
	{
		anim = GetComponent<Animator>();
		hitState = Animator.StringToHash("Base Layer.Hit");
		doPunchBool = Animator.StringToHash("DoPunch");
	}
	
	
	void OnTriggerExit (Collider other)
	{
		anim.SetBool(doPunchBool, true);
	}
	
	void Update ()
	{
		if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hitState)
			anim.SetBool(doPunchBool, false);
	}
}
