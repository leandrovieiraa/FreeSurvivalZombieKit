using Collect.Containers;
using Collect.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private StandardContainer inventory;
    public GameObject itemObject;

    void OnTriggerStay(Collider playerCollider)
    {
        if (playerCollider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player found this item");

            if (Input.GetKeyDown(KeyCode.P))
            {
                inventory = GameObject.Find("Canvas").transform.Find("Inventory").GetComponent<StandardContainer>();
                GameObject slotHolder = inventory.gameObject.transform.Find("Slots").gameObject;
                List<GameObject> slots = new List<GameObject>();

                foreach (object obj in slotHolder.transform)
                {
                    Transform child = (Transform)obj;
                    slots.Add(child.gameObject);
                }
                
                foreach (GameObject slot in slots)
                {
                    if(slot.transform.childCount == 0)
                    {
                        GameObject item = Instantiate(itemObject, slot.transform, false);
                        Debug.Log("Pickup this item: " + itemObject.name);
                        Destroy(this.gameObject);
                        break;                             
                    }
                }                               
            }          
        }
    }
}
