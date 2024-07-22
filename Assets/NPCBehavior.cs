using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    
    private Animator aiAnim;
    private NavMeshAgent agent;
    private Transform chair;
    private Transform exitPoint;


    private float minSitDuration = 90.0f; // En az oturma süresi (1.5 dakika)
    private float maxAdditionalSitDuration = 15.0f; // Ek rastgele oturma süresi (0-1 dakika)
    private bool isSitting = false;
    public string tableTag = "Table"; // Masaların tag'ı


    public void Initialize(Transform chair, Transform exitPoint, float sitDuration)
    {
        this.chair = chair;
        this.exitPoint = exitPoint;

        aiAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        aiAnim.SetTrigger("isWalking");
        agent.destination = chair.position;
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
        aiAnim.ResetTrigger("isWalking");
        aiAnim.SetTrigger("isSitting");
        transform.LookAt(chair);

        // En yakın masayı bul ve ona bak
        FindClosestTable();

        // 1.5 dakika bekle
        yield return new WaitForSeconds(minSitDuration);

        // Rastgele bir süre bekle (0-60 saniye arasında)
        float additionalWait = Random.Range(0.0f, maxAdditionalSitDuration);
        yield return new WaitForSeconds(additionalWait);

        aiAnim.ResetTrigger("isSitting");
        aiAnim.SetTrigger("isWalking");
        agent.destination = exitPoint.position;

        // Exit point'e yürüdükten sonra yok olmayı bekle
        while (agent.remainingDistance > 0.1f || agent.pathPending)
        {
            yield return null;
        }

        // Exit point'e ulaştığında yok ol
        Destroy(gameObject);
    }

    private void FindClosestTable()
    {
        GameObject[] tables = GameObject.FindGameObjectsWithTag(tableTag);
        float closestDistance = Mathf.Infinity;
        Transform closestTable = null;

        foreach (GameObject table in tables)
        {
            float distanceToTable = Vector3.Distance(transform.position, table.transform.position);
            if (distanceToTable < closestDistance)
            {
                closestDistance = distanceToTable;
                closestTable = table.transform;
            }
        }

        if (closestTable != null)
        {
            transform.LookAt(closestTable);
        }
    }

    private void OnDestroy()
    {
        if (chair != null)
        {
            chair.GetComponent<Chair>().isTargeted = false;
        }
    }

    
}
