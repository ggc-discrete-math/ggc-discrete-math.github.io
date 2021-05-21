using UnityEngine;
using System.Collections;

public class PadBtn : ButtonAction {
	
	private string[] padNames = {"A", "B", "C", "D"};
	private int currentNameIndex = 0;
	
	private TextMesh btnText;	
	
	void Start(){
		btnText = transform.parent.Find("Text").GetComponent<TextMesh>();
	}
	
	public override void onClick(){
		currentNameIndex = (currentNameIndex + 1) % padNames.Length;
		btnText.text = padNames[currentNameIndex];
	}
	
	public int getPadIndex(){
		return System.Array.IndexOf<string>(padNames, getPadName());
	}
	
	public string getPadName(){
		return btnText.text;
	}
}
