using UnityEngine;
using System.Collections;

public class ButtonAction : MonoBehaviour {
	
	private Color origColor;
	
	public void setColor(Color c){
		GetComponent<Renderer>().material.color = c;
		origColor = c;
	}
	
	public void highLight(){
		GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	public void unHighLight(){
		GetComponent<Renderer>().material.color = origColor;
	}
	
	public void onClickDown(){
		highLight();
	}
	
	public void onClickUp(){
		unHighLight();
		onClick();
	}
	
	public virtual void onClick(){
	}
}
