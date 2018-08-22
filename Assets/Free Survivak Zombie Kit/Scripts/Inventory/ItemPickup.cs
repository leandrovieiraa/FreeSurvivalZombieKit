using UnityEngine;

public class ItemPickup : MonoBehaviour {

	public Item item;   // Item to put in the inventory if picked up

    void OnTriggerStay(Collider playerCollider)
    {
        if (playerCollider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player found item: " + item.name);

            if (Input.GetKeyDown(KeyCode.P))
            {
                // Open inventory before pick up iten for prevent null objects bugs
                playerCollider.gameObject.GetComponent<PlayerController>().inventory.SetActive(true);

                // Pick up item
                PickUp();
            }
        }
    }

	// Pick up the item
	void PickUp ()
	{
		Debug.Log("Pick up " + item.name);
		Inventory.instance.Add(item);	// Add to inventory

		Destroy(gameObject);	// Destroy item from scene
	}

}
