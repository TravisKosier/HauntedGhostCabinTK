using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    private bool seesPlayer;
    public GameObject player;
    public float dist;
    public GameObject visionCone;
    public Transform soundSource;
    public bool contact;

    public enum EnemyStates
    {
        PATROL,
        CHASE,
        DISTRACTED
    }
    public EnemyStates enemyState;

    int m_CurrentWaypointIndex;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other != null)
        {
            if (other.gameObject.CompareTag("Distraction") && other.transform.parent.gameObject.GetComponent<ObjectMover>().GetAudible())
            {
                contact = true;
                soundSource = other.gameObject.transform;
                enemyState = EnemyStates.DISTRACTED;
            }
        } 
    }

    void Update()
    {
        switch (enemyState) //Alert or not alert
        {
            case EnemyStates.PATROL:
                {
                    seesPlayer = visionCone.GetComponent<Detection>().getSeesPlayer(); //Is player seen?
                    if (!seesPlayer) //If no, keep patrolling
                    {
                        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                        {
                            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                        }
                    }
                    else //If yes, start pursuing towards player
                    {
                        enemyState = EnemyStates.CHASE;
                    }
                }
                break;
            case EnemyStates.CHASE:
                {
                    seesPlayer = visionCone.GetComponent<Detection>().getSeesPlayer(); //Is player seen?
                    if (seesPlayer) //If yes, keep pursuing
                    {
                        dist = Vector3.Distance(this.transform.position, player.transform.position);
                        if (dist > 2)
                        {
                            navMeshAgent.SetDestination(player.transform.position); //Move towards player
                        }
                        else
                        {
                            navMeshAgent.SetDestination(this.transform.position);
                        }
                    }
                    else //If no, stop pursuing, start patrolling
                    {
                        enemyState = EnemyStates.PATROL;
                    }
                }
                break;
            case EnemyStates.DISTRACTED:
                {
                    seesPlayer = visionCone.GetComponent<Detection>().getSeesPlayer(); //Is player seen?
                    if (!seesPlayer) //If no, keep moving to source of sound
                    {
                        navMeshAgent.SetDestination(soundSource.position); //Move towards sound
                        dist = Vector3.Distance(this.transform.position, soundSource.position); //Check if enemy has reached source of noise
                        if (dist > 4) //If very close to source of sound
                        {
                            enemyState = EnemyStates.PATROL; //Return to patrolling
                            soundSource = null;
                        }
                    }
                    else //If yes, start pursuing towards player
                    {
                        enemyState = EnemyStates.CHASE;
                    }
                }
                break;
        }         
    }
}
