using UnityEngine;
using System.Collections;

public class takeAmmo : MonoBehaviour {


	public float ammunition = 25.0f;
	public byte gunType = 1;
	private GunInventory gunInfo;


	void Update () {
		transform.Rotate (new Vector3 (0, 1, 0));

	}
	void OnTriggerEnter(Collider col){

		if (col.tag.Equals("Player")) {
			Debug.Log("Znalazles paczke z amunicja");
			GunInventory inventory = col.GetComponent<GunInventory>();
			//Transform gun = col.transform.Find("FirstPersonCharacter/My_Glock");

			if(  (inventory.haveGuns[gunType])){ //(inventory.GetComponent<Shoot>().canGetAmmo())&&
				Debug.Log("take ammo: can get ammo");
				//gun.SendMessage("addAmmo", new Vector2(ammunition, gunType));
				inventory.SendMessage("addAmmo", new Vector2(ammunition, gunType));
				Destroy(gameObject);
				}
	
		}
	}
}