using UnityEngine;
using System.Collections;

public class TargetBob : MonoBehaviour {

	float targetY;
	Vector3 startPos;

	// Use this for initialization
	void Start () {

		startPos = transform.position;

	
	}
	
	// Update is called once per frame
	void Update () {


		if (transform.position.y < startPos.y + 0.2) 
		{
			targetY = startPos.y + 2;
		} 

		if (transform.position.y > startPos.y + 1.8)
		{
			targetY = startPos.y;
		}

		if (transform.position.y > targetY) 
		{
			transform.position = new Vector3 (startPos.x, transform.position.y - 0.1f, startPos.z);	
		}
		if (transform.position.y < targetY) 
		{
			transform.position = new Vector3 (startPos.x, transform.position.y + 0.1f, startPos.z);	
		}




	
	}
}
