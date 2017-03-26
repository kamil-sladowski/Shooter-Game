// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/**
 * In this script enemy choose next patrol' point,
 * and continue walk in that direction, 
 */

public class Patrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Vector3 firstPosition;
    private float timer = 0.0f;
    private float timeDelay = 4.0f;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        firstPosition = transform.position;
        agent.autoBraking = false;

        goToNextPoint();
    }

    void Update(){
        if (timer >= timeDelay){
            if (Vector3.Distance(transform.position, firstPosition) < 5.5f){
                goToNextPoint();
            }
            timer = 0.0f;
            firstPosition = transform.position;
        }
        else{
            timer += Time.deltaTime;
        }
        
        if (agent.remainingDistance < 1.0f)  
            goToNextPoint();
    }

    void goToNextPoint(){
        if (points.Length == 0)
            return;

      
        destPoint = Random.Range(0, points.Length);
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }
}