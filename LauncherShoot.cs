using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Script prepared especially for missile laucher.
 */

public class LauncherShoot : Shoot {
	public Animator animLaucher;
	public float timedTarget = 1.0f;
    public Texture2D trackingBar;
    public GameObject Launcher;
	public Rigidbody stringerMissile;
	public AudioSource lockSound;

    public float trackingTime = 4.0f;
	private float trackingTimer = 0.0f;

    public Text LockTarget;
	public Text EjectMissile;

    private bool isTracking = false;
    private bool lockedOn;
    
	private GameObject enemyTarget;
	private float barWidth;
	private float barHeight;


    new void OnGUI(){
		base.OnGUI();
        GUI.DrawTexture (new Rect (Screen.width*0.5f - barWidth+180,
		                           Screen.height*0.5f - barHeight + 200,
		                           barWidth,
		                           -(trackingTimer * barHeight / trackingTime)*0.3f), 
		                 trackingBar);
	}
    
    void OnEnable()
    {
        LockTarget.enabled = true;
        crossHairImage.enabled = true;
    }
    
    void Awake(){
		barWidth = Screen.width * 0.014f;
		barHeight = barWidth * 10.0f;
    }
		
	void Start(){
		Cursor.visible = false;
		animDemage = demagePoints.GetComponent<Animator> ();
		demagePoints = demagePoints.GetComponent<Text> ();
		currentAmmo = magazineSize;
		weaponAnim = GetComponent<Animator>();
    }



	protected override void fire(){
		if (currentAmmo > 0 && !isReloading) {
			LockTarget.enabled = true;
			if (Input.GetButton ("LockOn") && shotDelayCounter <=0 ) {
				fwd = transform.TransformDirection (Vector3.forward) * range;
				cameraTransform = transform.parent.GetComponent<Transform>();
				inflictsDemage();
				tracking ();
				locking ();
				ejectMissile();
			}
			else
				substractTrackingTimer();

			if (shotDelayCounter > 0) 
				shotDelayCounter -= Time.deltaTime;
		}
		
		if ((Input.GetButtonDown ("LockOn") && currentAmmo == 0) || (Input.GetButtonDown ("Reload") && currentAmmo < magazineSize)) {
			if (totalAmmo > 0) 
				isReloading = true;
		}
	}

	
	public override void inflictsDemage(){
		if (Physics.Raycast (cameraTransform.position, fwd, out hit, range)) {
			if (!(hit.transform.tag == "Item")){
				if(hit.distance <= range){
					if (hit.transform.tag == "Enemy" || hit.transform.tag == "EnemySpaceshipEngine") {
						enemyTarget = hit.collider.gameObject;
						if (!lockedOn) {
							isTracking = true;							
						}
					
					}else  {
						disableLaucher();
						substractTrackingTimer();
						DrawBulletHoles(bulletHole);
						Debug.Log ("We hit wall");
					}
				}
			}
		}
	}

	void tracking(){
		if (isTracking) {
			animLaucher.SetBool ("track", true);
			animLaucher.SetBool ("greenLock", false);
			trackingTimer += Time.deltaTime*2.5f; 
		
		}
	}
	void locking(){
		if (trackingTimer >= trackingTime) {
			isTracking = false;
			lockedOn = true;
			LockTarget.enabled = false;
			
			//trackingSound.enabled = false;
			lockSound.enabled = true;
			animLaucher.SetBool ("greenLock", true);

			//stringerLaucher.animation.Play();
		}
	}

	void ejectMissile(){
		if(lockedOn && enemyTarget != null){
			EjectMissile.enabled = true;
			if(Input.GetButtonDown ("Fire1")){
				EjectMissile.enabled = false;
				trackingTimer = 0.0f;
                Rigidbody clone = Instantiate(stringerMissile, ejector.transform.position, ejector.transform.rotation) as Rigidbody;	
				clone.velocity = transform.forward * ejectForce;
				currentAmmo -= 1;
                gameObject.GetComponent<Animation> ().Play();  
                //reloadWeapon.SetTrigger("shoot");
                disableLaucher();
				shotDelayCounter = shootDelay;
			}
		}
	}

	void disableLaucher(){
		lockedOn = false;
		isTracking = false;

		lockSound.enabled = false;
		//trackingSound.enabled = false;
		animLaucher.SetBool("greenLock", false);
		animLaucher.SetBool("track", false);
	}

	void substractTrackingTimer(){
		trackingTimer -= Time.deltaTime*0.5f; 
		trackingTimer  = Mathf.Clamp (trackingTimer , 0, trackingTime);
	}

    public GameObject getEnemyTarget() {
        return enemyTarget;
    }
    public void setEnemyTarget(GameObject target) {
        enemyTarget = target;
    }
    
    void OnDisable() {
        if(LockTarget !=null)
            LockTarget.enabled = false;
        if (crossHairImage != null)
            crossHairImage.enabled = false;
    }
    
}
