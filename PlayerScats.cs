using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/**
 * Control current player's health and energy.
 */

public class PlayerScats : MonoBehaviour {
	public float maxHealth = 100;
	public float currentHealth = 100;
	public float maxArmor = 100;
	public float currentArmor = 100;
	public float maxEnergy = 100;
	public float currentEnergy = 100;
	public float walkSpeed = 10.0f;
	public float runSpeed = 20.0f;
	
	public Texture2D healthTexture;
	public Texture2D armorTexture;
	public Texture2D energyTexture;
	public Texture2D hitTexture;
    public UnityEvent playerDeadEvent;
	public UnityEvent playerVisibility;

	private bool hit = false;
	private float opacity = 0.0f;
	private float canHeal = 0.0f;
	private float canRegenerate = 0.0f;
	private CharacterController chCont;
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsC;
	private Vector3 lastPosition;
	private float barWidth;
	private float barHeight;
    private float GROUNDLEVEL;
    


    void Awake(){
		barHeight = Screen.height * 0.02f;
		barWidth = barHeight * 10.0f;
		chCont = GetComponent<CharacterController>();
		lastPosition = transform.position;
		chCont = GetComponent<CharacterController>();
		fpsC = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ();
        GROUNDLEVEL = transform.position.y;
    }

    

	void OnGUI(){
		GUI.DrawTexture (new Rect (Screen.width - barWidth - 10,
		                          Screen.height - barHeight - 60,
		                          currentHealth * barWidth / maxHealth,
		                          barHeight), 
		                 healthTexture);
		GUI.DrawTexture (new Rect (Screen.width - barWidth - 10,
		                          Screen.height - barHeight - 40,
		                          currentArmor * barWidth / maxArmor,
		                          barHeight), 
		                 armorTexture);
		GUI.DrawTexture (new Rect (Screen.width - barWidth - 10,
		                          Screen.height - barHeight - 20,
		                          currentEnergy * barWidth / maxEnergy,
		                          barHeight), 
		                 energyTexture);

		
		if(hit) {
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, opacity);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), hitTexture);
			StartCoroutine("waitAndChangeOpacity");
		}
		
		if (opacity <= 0) {
			hit = false;
		}

	}


    void Update(){
        if (canHeal > 0.0f)
            canHeal -= Time.deltaTime;

        if (canRegenerate > 0.0f)
            canRegenerate -= Time.deltaTime;


        if (canHeal <= 0.0f && currentHealth < maxHealth){
            regenerate(ref currentHealth, maxHealth, 0.0005f);
        }
        if (canRegenerate <= 0.0f && currentEnergy < maxEnergy){
            regenerate(ref currentEnergy, maxEnergy, 0.003f);
        }

        if (transform.position.y > GROUNDLEVEL + 3.5f){
            playerVisibility.Invoke();
        }
    }

    IEnumerator waitAndChangeOpacity()
	{
		yield return new WaitForEndOfFrame();
		opacity -= 0.05f;
	}

	void hurt(float demage){
		hit = true;
		opacity = 1.0f;
		if (currentArmor > 0) {
			currentArmor -=demage;
		} 
		else currentHealth -= demage;
	
		if (currentHealth < maxHealth)
			canHeal = 5.0f;

        if (currentHealth <= 0){
            playerDeadEvent.Invoke();
        }

        currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
	}
	


	void regenerate(ref float currentStat, float maxStat, float regenerationSpeed){
		currentStat += maxStat * regenerationSpeed;
		Mathf.Clamp(currentStat, 0, maxStat);
	}

	void FixedUpdate(){ 
		float speed = walkSpeed;
 

        if (Input.GetKey (KeyCode.LeftShift) && chCont.isGrounded && lastPosition != transform.position && currentEnergy > 0)
        {
            lastPosition = transform.position;
            speed = runSpeed;
			currentEnergy -= 1;
			currentEnergy = Mathf.Clamp (currentEnergy, 0, maxEnergy);
			canRegenerate = 4.0f;
		}

		if (currentEnergy > 0) {
			fpsC.CanRun = true;
		} else {
			fpsC.CanRun = false;
		}
	}
}
