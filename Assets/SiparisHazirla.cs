using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiparisHazirla : MonoBehaviour
{
    [SerializeField] private Camera FPcam;
    [SerializeField] private float range = 30f;
    [SerializeField] private GameObject hand;
    [SerializeField] GameObject inCupObject;
    private RaycastHit hit;
    private GameObject currentObject;
    private bool objectInHand = false;
    private List<GameObject> tepsiBardaklar = new List<GameObject>();
    private Transform tezgah;
    private Transform tepsi;
   
    void Awake()
    {
        inCupObject.SetActive(false);
        tezgah = GameObject.Find("Tezgah").transform;
        tepsi = GameObject.Find("Tepsi").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectInHand == false)
            {
                TryGrab();
            }
            else if (objectInHand == true)
            {
                TryPlace();
            }
        }
    }

    private void TryGrab()
    {
        if (Physics.Raycast(FPcam.transform.position, FPcam.transform.forward, out hit, range))
        {
            // Suluk elimizdeyken başka bir obje almayı engellemek için kontrol ekleyelim
            if (currentObject != null && currentObject.tag == "Suluk")
            {
                Debug.Log("Suluk elimdeyken başka bir obje alamazsiniz.");
                return;
            }

            if (hit.transform.tag == "Bardak" || hit.transform.tag == "Caydanlik" || (hit.transform.tag == "Suluk" && currentObject == null))
            {
                GrabObject(hit.transform.gameObject);
            }
        }
    }

    private void GrabObject(GameObject obj)
    {
        currentObject = obj;
        currentObject.transform.SetParent(hand.transform);
        currentObject.transform.position = hand.transform.position;
        objectInHand = true;
        Debug.Log($"{currentObject.name} grabbed");
    }

    private void TryPlace()
    {
        if (Physics.Raycast(FPcam.transform.position, FPcam.transform.forward, out hit, range))
        {
            Debug.Log($"Looking at {hit.transform.name} with tag {hit.transform.tag}");

            if (hit.transform.tag == "Tepsi" && currentObject.tag == "Bardak")
            {
                PlaceOnTepsi();
            }
            else if (hit.transform.tag == "Tezgah" && (currentObject.tag == "Suluk" || currentObject.tag == "Caydanlik"))
            {
                PlaceOnTezgah();
            }
            else if (hit.transform.tag == "Bardak" && currentObject.tag == "Caydanlik")
            {
                PourTea();
            }
            else if (hit.transform.name == "dem" && currentObject.tag == "Suluk")
            {
                AddWater();
            }
        }
    }

    private void PlaceOnTepsi()
    {
        currentObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentObject.transform.parent = null;
        Vector3 newPosition = tepsi.position + new Vector3(0, 0.1f, tepsiBardaklar.Count * 0.2f);
        currentObject.transform.position = newPosition;
        tepsiBardaklar.Add(currentObject);
        objectInHand = false;
        currentObject = null;
        Debug.Log("Object placed on tepsi");
    }

    private void PlaceOnTezgah()
    {
        currentObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentObject.transform.parent = null;

        if (currentObject.tag == "Suluk")
        {
            currentObject.transform.position = tezgah.position + new Vector3(-3, .4f, 0);
        }
        else if (currentObject.tag == "Caydanlik")
        {
            currentObject.transform.position = tezgah.position + new Vector3(-4, .4f, 0);
        }

        objectInHand = false;
        Debug.Log($"{currentObject.name} placed on tezgah");
        currentObject = null;
    }

    private void PourTea()
    {
        hit.transform.name = "dem";
        Debug.Log("Tea poured into bardak");
        inCupObject.SetActive(true);
        inCupObject.GetComponent<Renderer>().material.color = new Color(0,8,20);
        inCupObject.transform.localPosition = new Vector3(0,0.03f,0);
    }

    private void AddWater()
    {
        if (hit.transform != null && hit.transform.name == "dem")
        {
            Debug.Log($"Adding water to {hit.transform.name} with tag {hit.transform.tag}");
            hit.transform.name = "cay";
            inCupObject.GetComponent<Renderer>().material.color = new Color(113, 0, 0);
            inCupObject.transform.localPosition = new Vector3(0,0.05f,0);
            Debug.Log("Water added, tea is ready");
        }
        else
        {
            Debug.Log("Hit transform is null or not dem");
        }
    }
}
