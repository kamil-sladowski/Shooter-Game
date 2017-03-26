using UnityEngine;
using System.Collections;

public class TakeAmmo : MonoBehaviour {
	public float ammunition = 25.0f;
	public byte gunType = 1;
	private GunInventory gunInfo;


	void Update () {
		transform.Rotate (new Vector3 (0, 1, 0));

	}
	void OnTriggerEnter(Collider col){

		if (col.tag.Equals("Player")) {
			GunInventory inventory = col.GetComponent<GunInventory>();

			if(  (inventory.haveGuns[gunType])){ 
				Debug.Log("take ammo: can get ammo");
				inventory.SendMessage("addAmmo", new Vector2(ammunition, gunType));
				Destroy(gameObject);
				}
		}
	}
}
