using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Table : MonoBehaviour
{
    Animator aiAnim;
    NavMeshAgent agent;
    public float thresholdDistance = 0.1f;
    public string chairTag = "Chair"; // Sandalyelerin tag'ı
    public string tableTag = "Table"; // Masaların tag'ı
    private Transform closestChair;
    private Transform closestTable;

    void Start()
    {
        aiAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        aiAnim.SetTrigger("isWalking");

        // En yakın sandalyeyi bul
        FindClosestChair();
    }

    // Update is called once per frame
    void Update()
    {
        // Eğer agent'in bir hedefi varsa ona doğru ilerle
        if (closestChair != null)
        {
            float distance = Vector3.Distance(transform.position, closestChair.position);
            if (distance < thresholdDistance)
            {
                aiAnim.ResetTrigger("isWalking");
                aiAnim.SetTrigger("isSitting");
                FindClosestTable(); // En yakın masayı bul ve karakterin ona bakmasını sağla
                if (closestTable != null)
                {
                    transform.LookAt(closestTable);
                }
                agent.destination = transform.position; // Hedefe ulaştıktan sonra hareketi durdur

                // Sandalyeyi tekrar kullanılabilir hale getir
                closestChair.GetComponent<Chair>().isTargeted = false;
                closestChair = null;
            }
        }
    }

    void FindClosestChair()
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag(chairTag);
        float closestDistance = Mathf.Infinity;

        foreach (GameObject chair in chairs)
        {
            Chair chairScript = chair.GetComponent<Chair>();
            if (chairScript != null && !chairScript.isTargeted) // Sandalyenin hedeflenmediğinden emin ol
            {
                float distanceToChair = Vector3.Distance(transform.position, chair.transform.position);
                if (distanceToChair < closestDistance)
                {
                    closestDistance = distanceToChair;
                    closestChair = chair.transform;
                }
            }
        }

        if (closestChair != null)
        {
            agent.destination = closestChair.position; // En yakın sandalyeyi hedef olarak ayarla
            closestChair.GetComponent<Chair>().isTargeted = true; // Sandalyeyi hedeflenmiş olarak işaretle
        }
    }

    void FindClosestTable()
    {
        GameObject[] tables = GameObject.FindGameObjectsWithTag(tableTag);
        float closestDistance = Mathf.Infinity;

        foreach (GameObject table in tables)
        {
            float distanceToTable = Vector3.Distance(transform.position, table.transform.position);
            if (distanceToTable < closestDistance)
            {
                closestDistance = distanceToTable;
                closestTable = table.transform;
            }
        }
    }
}
