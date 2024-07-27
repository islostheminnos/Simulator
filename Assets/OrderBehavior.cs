using UnityEngine;

public class OrderBehavior : MonoBehaviour
{
    public GameObject[] placeableOrders;
    public GameObject orderEmote;
    private GameObject currentOrder;
    private bool isSitting = false;

    public void SetSittingStatus(bool status)
    {
        if (isSitting != status) // Durum değiştiğinde sadece çağır
        {
            isSitting = status;
            if (isSitting && currentOrder == null)
            {
                RandomOrderSelector(); // NPC oturduğunda sipariş oluştur
            }
        }
    }

    public void RandomOrderSelector()
    {
        if (isSitting && currentOrder == null)
        {
            int randomIndex = UnityEngine.Random.Range(0, placeableOrders.Length);

            // Yeni sipariş objesini currentOrder olarak sakla
            if (orderEmote != null)
            {
                currentOrder = Instantiate(placeableOrders[randomIndex], orderEmote.transform);
            }

            // En yakın masayı bul ve sipariş adını ekle
            GameObject closestTable = GetComponent<TableBehavior>().FindClosestTable();
            if (closestTable != null)
            {
                TableManager tableManager = closestTable.GetComponent<TableManager>();
                if (tableManager != null)
                {
                    tableManager.AddOrderName(currentOrder.name);
                }
            }
        }
    }

    public void OrderDestroy()
    {
        if (currentOrder != null)
        {
            Destroy(currentOrder);
        }
    }

    public void OnOrderDelivered()
    {
        if (currentOrder != null)
        {
            Destroy(currentOrder);
            currentOrder = null;
        }
    }
}
