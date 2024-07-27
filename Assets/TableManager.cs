using UnityEngine;

public class TableManager : MonoBehaviour
{
    public string[] orderNames;

    public void AddOrderName(string orderName)
    {
        // Mevcut orderNames array'ini genişletip yeni orderName'i ekle
        string[] newOrderNames = new string[orderNames.Length + 1];
        orderNames.CopyTo(newOrderNames, 0);
        newOrderNames[orderNames.Length] = orderName;
        orderNames = newOrderNames;
    }
}
