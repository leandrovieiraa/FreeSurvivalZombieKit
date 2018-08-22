using UnityEngine;

/* An Item that can be consumed. So far that just means regaining health */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class Consumable : Item {

	public int healthGain;		// How much Health?
    public int hungerGain;		// How much Hunger?
    public int thirstyGain;      // How much Thirsty?

    // This is called when pressed in the inventory
    public override void Use()
	{
        VitalsController vitals = GameObject.FindGameObjectWithTag("Player").GetComponent<VitalsController>();

        if (healthGain > 0)
            vitals.Increase(healthGain, "health");
                
        if (hungerGain > 0)
            vitals.Increase(hungerGain, "hunger");

        if (thirstyGain > 0)
            vitals.Increase(thirstyGain, "thirst");

        Debug.Log(name + " consumed!");

		RemoveFromInventory();	// Remove the item after use
	}

}
