using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Allow to collect and menage of the weapons.
 */

public class GroundWeapon : MonoBehaviour {
	public int weaponNumber;
	private bool isPlayerInTrigger;
	public Text grabWeapon;

    // Use this for initialization
    void Start(){
		grabWeapon.GetComponent<Text> ();
	}


	void OnTriggerStay(Collider col){
		if (col.tag.Equals ("Player")) {
			isPlayerInTrigger = true;
			if(Input.GetKeyDown(KeyCode.E)){
				col.SendMessage("addWeapon", weaponNumber);
				isPlayerInTrigger = false;
				Destroy(gameObject);
				grabWeapon.enabled = false;
			}
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag.Equals ("Player"))
		isPlayerInTrigger = false;

	}

	void OnGUI(){
            if (isPlayerInTrigger)
                grabWeapon.enabled = true;
            else
                grabWeapon.enabled = false; 
	}

}
