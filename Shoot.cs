using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

/**
 * It covering varying aspects to build weapon mechanics.
 * Sript cover:
 * Raycasting which will display bullet hole textures to indicate impact
 * Using Animation events to call a function at a precise moment
 * Adjusting the rate of fire based on the weapon type
 * Shooting projectiles for things like a paintball gun or rocket launchers
 * Utilizing iron sights on a weapon when right clicking
 */

public class Shoot : MonoBehaviour {
	public GameObject bulletHole;
	public GameObject ejector;
    public GameObject gunCamera;
    public float ejectForce;
	public float reloadTime = 2.0f;
	public float range;
    public bool isAutomatic = false;
    public Text demagePoints;
	public Text ammoState;
	public Text lackOfAmunitionText;
	public Image crossHairImage;

	protected float reloadTimer =0.0f;
	protected bool isReloading = false;
    protected bool isStillReloading = false;
	protected RaycastHit hit;

	public byte maxAmmo;
	public byte magazineSize;
	protected byte currentAmmo;
	protected byte totalAmmo =0;

	public float shootDelay = 0.5f;
	protected bool isZooming = false;
	protected float shotDelayCounter = 0.0f;
	protected float zoomFieldOfView = 40.0f;
	protected float defaultFieldOfView = 60.0f;
	protected int RayMask = 1 << 1 | 1 << 11; 
	protected Animator animDemage;
    protected Animator weaponAnim;
	protected Vector3 fwd;
	protected Transform cameraTransform;

   
    void OnEnable(){
        crossHairImage.enabled = true;

        if (currentAmmo == 0 && totalAmmo > 0)
            isReloading = true;
    }

    // Use this for initialization
    protected void Start(){
		Cursor.visible = false;
		animDemage = demagePoints.GetComponent<Animator> ();
		demagePoints = demagePoints.GetComponent<Text> ();
		ammoState = ammoState.GetComponent<Text> ();
		lackOfAmunitionText = lackOfAmunitionText.GetComponent<Text> ();
        weaponAnim = GetComponent<Animator>();
        currentAmmo = magazineSize;
    }


	public void OnGUI()
	{
        if(ammoState !=null)
		    ammoState.text = currentAmmo + "/" + totalAmmo;
        
        if (currentAmmo == 0 && totalAmmo == 0)
                lackOfAmunitionText.enabled = true;
        else
            lackOfAmunitionText.enabled = false;
	}


    // Update is called once per frame
    void Update () {
		fire ();
		zoom ();
		reload ();
	}
	

	protected virtual void fire(){
		if (currentAmmo > 0 && !isReloading) {
			if ((Input.GetButtonDown ("Fire1") || (Input.GetButton ("Fire1") && isAutomatic)) 
																  && shotDelayCounter <= 0) {

				shotDelayCounter = shootDelay;
				currentAmmo-=1;
				fwd = transform.TransformDirection (Vector3.forward) * range;
				cameraTransform = transform.parent.GetComponent<Transform>();
				inflictsDemage();
                gameObject.GetComponent<Animation> ().Play(); 
                weaponAnim.SetTrigger("shoot");
            }
			if (shotDelayCounter > 0)
				shotDelayCounter -= Time.deltaTime;
		}

		if ((Input.GetButtonDown ("Fire1") && currentAmmo == 0) 
                || (Input.GetButtonDown ("Reload") && currentAmmo < magazineSize)) {
			if (totalAmmo > 0) {
				isReloading = true;
            }
		}
	}

	public void reload(){ 
		if (isReloading && totalAmmo>0) {
            crossHairImage.enabled = false;
            Debug.Log("is reloading");
            if (!isStillReloading)
            {
                weaponAnim.SetTrigger("reload");
                isStillReloading = true;
            }
			reloadTimer += Time.deltaTime;
			if (reloadTimer >= reloadTime) {
				byte needAmmo = (byte)(magazineSize - currentAmmo);
				if (totalAmmo >= needAmmo) {
					totalAmmo -= needAmmo;
					currentAmmo += needAmmo;
				}
				if (totalAmmo < needAmmo) {
					currentAmmo += totalAmmo;
					totalAmmo = 0;
				}
                isReloading = false;
                crossHairImage.enabled = true;
                reloadTimer = 0.0f;
                isStillReloading = false;
                Debug.Log("Reloading finished");
            }
		}
	}


    /// <summary>
    /// Create text to show the number of inflicted injuries
    /// controlled by the camera
    /// </summary>
    public float pointInjurePart(string bodyPart){
		animDemage.SetTrigger("dam");
		float demage = int.Parse(bodyPart);
		demagePoints.text = bodyPart;
	    return demage;
	}



	public void zoom(){ 
		if (gameObject.GetComponentInParent<Camera> () as Camera) {
			Camera camera = gameObject.GetComponentInParent<Camera> ();
			if (Input.GetButton("Fire2")) {
				if(camera.fieldOfView > zoomFieldOfView ){
					camera.fieldOfView--;
				}
			}
			else if (camera.fieldOfView < defaultFieldOfView)
				camera.fieldOfView++;
			else {
				isZooming = false;
				gunCamera.SetActive(true);
			}
		}
	}


	public bool canGetAmmo(){
		if (totalAmmo == maxAmmo) {
			return false;
		}
		Debug.Log("shoot: can get ammo");
		return true;
	}

	public void substractHealthPoints(float demage, GameObject enemy){
		enemy.SendMessage("hurt", demage);
	}


    /// <param name="ammoData.x">Used to get number of amunition.</param>
    /// <param name="ammoData.y">Used to get type of gun.</param>
    public void addAmmo(Vector2 ammoData){ //ammunition, gunType
		byte ammoToAdd = (byte)ammoData.x;
		if (totalAmmo + ammoToAdd < maxAmmo)
			totalAmmo += (byte)ammoToAdd;
		else
			totalAmmo = maxAmmo;
	}

	public void DrawBulletHoles(GameObject bulletHole){
		Instantiate (bulletHole, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal)); 
	}

    
    void OnDisable(){
            crossHairImage.enabled = false;
        if (isReloading){
            weaponAnim.ResetTrigger("reload");
            isReloading = false;
            crossHairImage.enabled = true;
            reloadTimer = 0.0f;
            isStillReloading = false;
            Debug.Log("Reloading stopped");

        }
    }

    public virtual void inflictsDemage() { }

}
