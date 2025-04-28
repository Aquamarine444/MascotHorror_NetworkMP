using UnityEngine;
using UnityEngine.AI;

public class SpiritMovement : MonoBehaviour
{
    //private Animator anim;

    [Header("Roaming:")]
    private NavMeshAgent navAgent;

    private NavMeshHit navHit;

    private Vector3 currentDestination;

    [SerializeField]
    private float maxwalkDistance = 50f;

    [Header("FindPlayer")]
    private bool moveToPlayer;



    [SerializeField]
    private GameObject Player;

    void Start()
    {

    }

    void Update()
    {
        //Activates animations for sprite
        //AnimateSpirits();

        CheckIfReachedDestination(); //Used for roaming

    }

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        SetNewDestination();
    }

    //General roaming around area
    void SetNewDestination()
    {
        while (true)
        {
            NavMesh.SamplePosition(((Random.insideUnitSphere * maxwalkDistance) + transform.position), out navHit, maxwalkDistance, -1);

            if (currentDestination != navHit.position)
            {
                currentDestination = navHit.position;
                navAgent.SetDestination(currentDestination);
                break;
            }
        }
    }

    //Used in Roaming script
    void CheckIfReachedDestination()
    {
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    if (moveToPlayer)
                    {
                        if (navAgent.stoppingDistance == 0f)
                        {
                            Player.tag = "Spirit";
                            moveToPlayer = false;
                        }
                    }
                    else
                    {
                        SetNewDestination();
                    }
                }
            }
        }
    }
    /* void AnimateSpirits()
   {
       //Use when spirit as animations to control
     if (navAgent.velocity.magnitude > 0)
       {
           anim.SetBool("Walk", true);
       }
       else
       {
           anim.SetBool("Walk", false);
       }
   }*/

    private void OnTriggerEnter(Collider other) //Find player
    {
            if (other.CompareTag("Player"))
            {
                moveToPlayer = true;
                navAgent.SetDestination(other.transform.position);
            }
    }

    private void OnTriggerExit(Collider other) //Lost Player
    {
        if (other.CompareTag("Player"))
        {
            moveToPlayer = false;
            SetNewDestination();
        }
    }
}

