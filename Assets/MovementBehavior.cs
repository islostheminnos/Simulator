using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovementBehavior : MonoBehaviour
{
    private Animator aiAnim;
    private NavMeshAgent agent;
    private Transform exitPoint;
    private bool isSitting = false;
    private float minSitDuration;
    private OrderBehavior orderBehavior;
    private TableBehavior tableBehavior;
    private Transform chair;

    public void Initialize(Transform exitPoint, string chairTag, float sitDuration)
    {
        this.exitPoint = exitPoint;
        this.minSitDuration = sitDuration;

        aiAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        aiAnim.SetTrigger("isWalking");

        // En yakın sandalyeyi bul ve oraya git
        chair = FindClosestChair(chairTag);
        if (chair != null)
        {
            chair.GetComponent<Chair>().isTargeted = true;
            agent.destination = chair.position;
        }
    }

    private void Start()
    {
        orderBehavior = GetComponent<OrderBehavior>();
        tableBehavior = GetComponent<TableBehavior>();
    }

    private void Update()
    {
        if (!isSitting && agent.remainingDistance < 0.1f)
        {
            StartCoroutine(SitAndLeave());
        }
    }

    private IEnumerator SitAndLeave()
    {
        isSitting = true;
        orderBehavior.SetSittingStatus(true); // OrderBehavior'a oturduğumuzu bildir

        aiAnim.ResetTrigger("isWalking");
        aiAnim.SetTrigger("isSitting");
        transform.LookAt(chair);

        // En yakın masayı bul ve ona bak
        tableBehavior.LookAtClosestTable();

        // Oturma süresi kadar bekle
        yield return new WaitForSeconds(minSitDuration);

        aiAnim.ResetTrigger("isSitting");
        aiAnim.SetTrigger("isWalking");
        agent.destination = exitPoint.position;
        isSitting = false;
        orderBehavior.SetSittingStatus(false); // OrderBehavior'a oturma işleminin bittiğini bildir
        orderBehavior.OrderDestroy();

        // Exit point'e yürüdükten sonra yok olmayı bekle
        while (agent.remainingDistance > 0.1f || agent.pathPending)
        {
            yield return null;
        }

        // Exit point'e ulaştığında yok ol
        Destroy(gameObject);
    }

    private Transform FindClosestChair(string chairTag)
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag(chairTag);
        float closestDistance = Mathf.Infinity;
        Transform closestChair = null;

        foreach (GameObject chair in chairs)
        {
            Chair chairComponent = chair.GetComponent<Chair>();
            if (!chairComponent.isTargeted)
            {
                float distanceToChair = Vector3.Distance(transform.position, chair.transform.position);
                if (distanceToChair < closestDistance)
                {
                    closestDistance = distanceToChair;
                    closestChair = chair.transform;
                }
            }
        }

        return closestChair;
    }

    public bool IsSitting()
    {
        return isSitting;
    }

    public bool HasReachedDestination()
    {
        return agent.remainingDistance < 0.1f;
    }
}
