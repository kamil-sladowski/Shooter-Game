using UnityEngine;
using System.Collections;

/**
 * Script clarifies weapon mechanics for automatics weapons.
 */ 

public class RifleShoot : Shoot {
	public GameObject bulletInjury;
	public Rigidbody bulletShell;
	public string headDemage;
	public string legDemage;
	public string chestDemage;


	new void fire(){
		base.fire ();
		ejectBulletShell();	
	}

	public override void inflictsDemage(){
		float demage = -1.0f;

		if (Physics.Raycast (cameraTransform.position, fwd, out hit, range)) {
			if (hit.transform.tag != "Item") {
				if (hit.distance <= range) {
					if (hit.collider.tag == "Enemy" || hit.collider.tag == "Head demage" 
					    || hit.collider.tag == "Chest demage" || hit.collider.tag == "Leg demage") {
					

						if (hit.collider.tag == "Head demage")
							demage = pointInjurePart (headDemage);
						
						else if (hit.collider.tag == "Chest demage") 
							demage = pointInjurePart (chestDemage);
						
						else if (hit.collider.tag == "Leg demage") 
							demage = pointInjurePart (legDemage);
						
						substractHealthPoints (demage, hit.transform.gameObject);
					} else {
						DrawBulletHoles (bulletHole);
						Debug.Log ("We hit wall");
					}
				}
			}
		}
	}


	public void ejectBulletShell(){
        Debug.Log("ejectBulletShell");
		Rigidbody clone;
		clone = Instantiate (bulletShell, ejector.transform.position, ejector.transform.rotation) as Rigidbody;
		clone.velocity = transform.right * ejectForce;
		clone.velocity = transform.right * ejectForce;
	}
}
