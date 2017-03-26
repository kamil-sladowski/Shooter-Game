using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * It allow player to climb the ladder.
 */

public class LadderClimbing : MonoBehaviour {
	private Transform target;
	private bool isClimbing = false;
	private float climbSpeed = 28.0f;
	

	void OnTriggerStay(Collider col) {
		if (col.gameObject.tag.Equals("Player")) {
			if (!isClimbing) {
				target = col.transform;
				if (target) {
					target.GetComponent<Rigidbody> ().useGravity = false;
				}
				isClimbing = true;
			} 
			else{
				isClimbing = false;
				target.GetComponent<Rigidbody> ().useGravity = true;
				target = null;
			}
		}

		if (isClimbing) {
			if (target) 
				target.GetComponent<Rigidbody>().useGravity = false;
            
            if (Input.GetKey("w")){
                target.GetComponent<Rigidbody>().useGravity = false;
                target.Translate(Vector3.up * Time.deltaTime * climbSpeed);
            }
		}
	}



	void OnTriggerExit(Collider col) {
		if(target) {
			target.GetComponent<Rigidbody> ().useGravity = true;
			target.Translate(Vector3.forward * Time.deltaTime * 10);		
		}
		isClimbing = false;
		target = null;
	}
}
