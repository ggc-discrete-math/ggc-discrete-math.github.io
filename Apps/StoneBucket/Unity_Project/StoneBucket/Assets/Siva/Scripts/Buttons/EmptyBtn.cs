using UnityEngine;
using System.Collections;

public class EmptyBtn : GotoBtn {
	
	public override void onClick(){
		base.onClick();
		btnText.text = "Empty? " + btnText.text;
	}
	
	public void turnOff(){
		transform.parent.gameObject.SetActive(false);
	}
	
	public void turnOn(){
		transform.parent.gameObject.SetActive(true);
		//check if Empty already added.
		if(!btnText.text.StartsWith("Empty"))
			btnText.text = "Empty? " + btnText.text;
	}

}
