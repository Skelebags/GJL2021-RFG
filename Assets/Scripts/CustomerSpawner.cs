using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public RectTransform spawnpointStart;
    public RectTransform spawnpointEnd;

    public GameObject mapButton;

    [SerializeField]
    private int day_offset = 5;


    [SerializeField]
    private float slide_speed = 0.4f;

    private GameManager manager;
    private GameObject[] customers;
    private GameObject curr_customer = null;
    private int remaining;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        customers = Resources.LoadAll<GameObject>("Customers");
        
        foreach(GameObject customer in customers)
        {
            Debug.Log(customer.name);
        }

        remaining = (manager.GetPlayer().day * 2) + day_offset;
    }

    // Update is called once per frame
    void Update()
    {
        if(remaining > 0)
        {
            if (curr_customer == null)
            {
                curr_customer = Instantiate(customers[Random.Range(0, customers.Length)], spawnpointStart.position, Quaternion.identity, gameObject.transform);

            }
        }
        else
        {
            mapButton.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (curr_customer != null && !curr_customer.GetComponent<Customer>().hasBeenServed)
        {
            FindObjectOfType<AudioManager>().Play("Footstep", 1f);
            curr_customer.GetComponent<RectTransform>().position = Vector2.Lerp(curr_customer.GetComponent<RectTransform>().position, spawnpointEnd.position, slide_speed);
            if ((curr_customer.GetComponent<RectTransform>().position - spawnpointEnd.position).magnitude <= 0.5f)
            {
                curr_customer.GetComponent<RectTransform>().position = spawnpointEnd.position;
            }
        }
        else if(curr_customer != null && curr_customer.GetComponent<Customer>().hasBeenServed)
        {
            curr_customer.GetComponent<RectTransform>().position = Vector2.Lerp(curr_customer.GetComponent<RectTransform>().position, spawnpointStart.position, slide_speed);
            if ((curr_customer.GetComponent<RectTransform>().position - spawnpointStart.position).magnitude <= 0.5f)
            {
                Destroy(curr_customer);
                remaining--;
            }
        }
    }
}
