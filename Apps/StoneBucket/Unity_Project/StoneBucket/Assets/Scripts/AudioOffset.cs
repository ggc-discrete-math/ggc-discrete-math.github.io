using UnityEngine;
using System.Collections;

public class AudioOffset : MonoBehaviour
{
	IEnumerator Start ()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		yield return new WaitForSeconds(Random.Range(0f, 1.5f));
		GetComponent<AudioSource>().Play();
	}
}
