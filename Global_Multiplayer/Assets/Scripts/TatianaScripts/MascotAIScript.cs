using UnityEngine;
using UnityEngine.AI;

public class MascotAIScript : MonoBehaviour
{
    GameObject Player;

    NavMeshAgent Agent;

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask PlayerLayer;

    //PatrolPoints
    Vector3 DesPoint;
    bool WalkPoint;
    [SerializeField] float WalkRange;

    //StateChange
    [SerializeField] float SightRange;
    bool PlayerInSight;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player"); //GamObject.FindTag
    }

    private void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);

        if (!PlayerInSight)
        {
            Patrol();
        }


        if (PlayerInSight)
        {
            Chase();
        }
    }

    void Chase()
    {
        Agent.SetDestination(Player.transform.position);
    }

    void Patrol()
    {
        if (!WalkPoint)
        {
            SearchForDestination();
        }

        if (WalkPoint)
        {
            Agent.SetDestination(DesPoint);
        }

        if (Vector3.Distance(transform.position, DesPoint) < 10)
        {
            WalkPoint = false;
        }
    }

    void SearchForDestination()
    {
        float z = Random.Range(-WalkRange, WalkRange);
        float x = Random.Range(-WalkRange, WalkRange);

        DesPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(DesPoint, Vector3.down, GroundLayer))
        {
            WalkPoint = true;
        }
    }
}
