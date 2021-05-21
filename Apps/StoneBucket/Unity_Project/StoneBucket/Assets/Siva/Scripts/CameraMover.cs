using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
	
	Vector3 initPos;
	
	IEnumerator Start(){
		yield return null;
		//zoom in until B label is barealy in view.
		Transform zoomPoint = GameObject.Find("prop_hoverPadB").transform.Find("LabelSign");
		yield return StartCoroutine(zoomInUntilOutOfViewX(zoomPoint.position-0.2f*Vector3.left));
		initPos = transform.position;
	}

	public IEnumerator zoomOutUntiInView(Vector3 target){
		while(GetComponent<Camera>().WorldToScreenPoint(target).y>Screen.height){
			transform.Translate(Vector3.back*(Time.deltaTime));
			yield return null;
		}
	}
	
	public IEnumerator zoomInUntilOutOfView(Vector3 target){
		while(GetComponent<Camera>().WorldToScreenPoint(target).y<Screen.height){
			if(transform.position.y<=initPos.y)
				break;
			transform.Translate(Vector3.forward*(Time.deltaTime));
			yield return null;
		}
	}

	public IEnumerator zoomInUntilOutOfViewX(Vector3 target){
		while(GetComponent<Camera>().WorldToScreenPoint(target).x>0){
			transform.Translate(Vector3.forward*(Time.deltaTime));
			yield return null;
		}
	}

}
