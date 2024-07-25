using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;
    public string chairTag = "Chair";
    public float minSpawnDelay = 0.0f;
    public float maxSpawnDelay = 120.0f;
    public float sitDuration = 90.0f; // En az oturma süresi

    private void Start()
    {
        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC()
    {
        while (true)
        {
            // Rastgele bir spawn delay süresi bekle (0-120 saniye arasında)
            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
            SpawnNewNPC();
        }
    }

    private void SpawnNewNPC()
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag(chairTag);
        foreach (GameObject chair in chairs)
        {
            Chair chairScript = chair.GetComponent<Chair>();
            if (chairScript != null && !chairScript.isTargeted)
            {
                GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

                // Referansları atama
                NPCBehavior npcBehavior = npc.GetComponent<NPCBehavior>();
                

                if (npcBehavior != null)
                {
                    npcBehavior.Initialize(chair.transform, exitPoint, sitDuration);

                    // Referansları birbirine atama
                  
                    
                }
                else
                {
                    Debug.LogError("NPC prefab does not contain NPCBehavior or OrderManager component.");
                }

                chairScript.isTargeted = true;
                break;
            }
        }
    }
}
