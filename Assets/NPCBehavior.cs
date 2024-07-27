using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    private MovementBehavior movementBehavior;
    private OrderBehavior orderBehavior;
    private TableBehavior tableBehavior;
    private Transform exitPoint;
    private string chairTag;
    private float sitDuration;

    public void Initialize(Transform exitPoint, string chairTag, float sitDuration)
    {
        this.exitPoint = exitPoint;
        this.chairTag = chairTag;
        this.sitDuration = sitDuration;

        movementBehavior = GetComponent<MovementBehavior>();
        orderBehavior = GetComponent<OrderBehavior>();
        tableBehavior = GetComponent<TableBehavior>();

        // NPC'yi başlat ve oturma ve kalkma noktalarını ayarla
        movementBehavior.Initialize(exitPoint, chairTag, sitDuration);
    }

    private void Update()
    {
        if (movementBehavior != null && orderBehavior != null && tableBehavior != null)
        {
            // NPC'nin oturma ve sipariş işlemlerini yönet
            if (!movementBehavior.IsSitting() && movementBehavior.HasReachedDestination())
            {
                orderBehavior.SetSittingStatus(true);
                tableBehavior.LookAtClosestTable();
            }
        }
    }
}
