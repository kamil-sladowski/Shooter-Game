using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

/**
 * Allow fundamentals enemy's operations, like looking for and attack player.
 * 
 */ 

public class Enemy : MonoBehaviour {
	public float walkSpeed = 5.0f;
	public float attackDistance = 2.0f;
	public float attackStrenght = 5.0f;
	public float attackDelay = 1.0f;
    public float turnSpeed = 360.0f;
    public GameObject corpse;
    public UnityEvent enemyKilledEvent;

    private float hp = 500.0f;
    private AICharacterControl aiContol;
    private Patrol patrolScript;

	private float timer = 0;
	private string currentState;
	private Animator anim;
	private AnimatorStateInfo stateInfo;
    private Transform player;

    private bool seePlayer = false;
    public float maxTimeOfChasing = 10.0f;
    private float ChasingCounter;

    void Awake() {
        GameOverMenager.countOfEnemies++;
    }

    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        patrolScript = GetComponent<Patrol>();
        aiContol = GetComponent<AICharacterControl>();
        anim = GetComponent<Animator>();
        anim.SetBool("Prepare to walk", true);

        anim = GetComponent<Animator>();
        anim.SetBool("land", true);
    }

    void Update(){
        if (seePlayer){
            rotateToPlayer(player.transform.position);
            transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
            ChasingCounter += Time.deltaTime;

            if (ChasingCounter > maxTimeOfChasing) {
                setVsibilityPlayer(false);
                ChasingCounter = 0.0f;
            }
        }

    }



	void OnTriggerStay(Collider col){
		if (col.tag.Equals ("Player")) {
            float distace = Vector3.Distance(transform.position, col.transform.position);
			if(distace > attackDistance){
                setVsibilityPlayer(true);
            }
			else if(timer <=0){
				col.SendMessage("hurt", attackStrenght);
				timer = attackDelay;
			}
			if(timer > 0){
				timer -= Time.deltaTime;
			}
		}
	}


	
    void rotateToPlayer(Vector3 playerPosition) {
        Vector3 direction = playerPosition - transform.position;
        direction.y = 0;
        direction.Normalize();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
    }


	void hurt(float demage){ 
		hp -= Mathf.Abs(demage);
        setVsibilityPlayer(true);

        if (hp >= (-demage))
			ScoreMenager.score += Mathf.Abs(demage);
		else
			ScoreMenager.score += Mathf.Abs(hp);

        if (hp <= 0){
            enemyKilledEvent.Invoke();
            Instantiate(corpse, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}

    private void setVsibilityPlayer(bool see) {
        seePlayer = see;
        patrolScript.enabled = !see;
            aiContol.target = player.transform;
        aiContol.enabled = see;
    }

	private void animationSet(string animationToPlay){
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if (currentState == "") {
			currentState = animationToPlay;
			if (stateInfo.IsName ("Base Layer.run") && currentState != "run") {
				anim.SetBool ("runToIdle0", true);
			}

			string state = "idle0To" + currentState.Substring(0, 1).ToUpper() + currentState.Substring(1);
			anim.SetBool(state, true);
			currentState = "";
		}
	}
}
