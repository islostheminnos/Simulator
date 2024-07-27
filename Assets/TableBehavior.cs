using UnityEngine;

public class TableBehavior : MonoBehaviour
{
    public string tableTag = "Table";

    public GameObject FindClosestTable()
    {
        GameObject[] tables = GameObject.FindGameObjectsWithTag(tableTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestTable = null;

        foreach (GameObject table in tables)
        {
            float distanceToTable = Vector3.Distance(transform.position, table.transform.position);
            if (distanceToTable < closestDistance)
            {
                closestDistance = distanceToTable;
                closestTable = table;
            }
        }

        if (closestTable == null || closestTable.GetComponent<TableManager>() == null)
        {
            Debug.LogError("TableManager component not found on closest table.");
        }

        return closestTable;
    }

    public void LookAtClosestTable()
    {
        GameObject closestTable = FindClosestTable();
        if (closestTable != null)
        {
            transform.LookAt(closestTable.transform);
        }
    }
}
