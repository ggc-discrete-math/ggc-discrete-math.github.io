using UnityEngine;
using System.Collections;

public class StepArrow : MonoBehaviour {
	
	private float moveSpeed = 0.5f;
	
	public IEnumerator switchToStep(GameObject cmdBlock){
		while(!Mathf.Approximately(cmdBlock.transform.position.y, transform.position.y)){
			float deltaY = cmdBlock.transform.position.y - transform.position.y;
			float y;
			if(deltaY>0)
				y = Mathf.Min(moveSpeed*Time.deltaTime, deltaY);
			else
				y = Mathf.Max(-moveSpeed*Time.deltaTime, deltaY);
			
			transform.Translate(0, y, 0);
			yield return null;
		}
		transform.parent = cmdBlock.transform;
	}
	
	public GameObject getCurrentCmdBlock(){
		return transform.parent.gameObject;
	}
}
