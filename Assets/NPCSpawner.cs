using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;
    public string chairTag = "Chair";
    public float minSpawnDelay = 0f;
    public float maxSpawnDelay = 5f;
    public float sitDuration = 10f;

    private void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    private IEnumerator SpawnNPCs()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            NPCBehavior npcBehavior = npc.GetComponent<NPCBehavior>();
            if (npcBehavior != null)
            {
                npcBehavior.Initialize(exitPoint, chairTag, sitDuration);
            }
        }
    }
}
