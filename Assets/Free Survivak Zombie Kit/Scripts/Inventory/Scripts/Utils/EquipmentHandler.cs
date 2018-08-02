using Collect.Items;
using Collect.Slots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    public GameObject defaultArms;
    private Vector3 fixArmsPosition;
    private Quaternion fixArmsRotation;

    public GameObject weaponHolder;
    public GameObject lastWeaponEquiped;

    public GameObject slotHolder;
    public List<GameObject> slots;

    public bool weaponEquipped = false;

    void Start ()
    {
        if (slots.Count <= 0)
            GetAllSlots();
	}

	void Update ()
    {
        if (slots.Count > 0)
            SearchForActiveSlots();
	}

    void GetAllSlots()
    {
        foreach (object obj in slotHolder.transform)
        {
            Transform child = (Transform)obj;
            slots.Add(child.gameObject);
        }
    }

    void SearchForActiveSlots()
    {
        foreach (GameObject slot in slots)
        {
            if (slot.transform.childCount == 0 && slot.GetComponent<SlotWithType>().Type == ItemType.Hands)
            {            
                foreach (object obj in weaponHolder.transform)
                {
                    Transform child = (Transform)obj;
                    child.gameObject.SetActive(false);
                }

                if(lastWeaponEquiped != null)
                {
                    Debug.Log("Weapon Unequiped: " + lastWeaponEquiped.name);
                    lastWeaponEquiped = null;
                }
                             
                if (defaultArms.activeSelf == false && weaponEquipped == true)
                {           
                    weaponEquipped = false;
                    defaultArms.SetActive(true);                   
                }                    
            }             
            else
            {
                foreach (object obj in slot.transform)
                {             
                    Transform child = (Transform)obj;

                    if (child.GetComponent<DraggableItemType>().ItemType == ItemType.Hands)
                    {
                       
                        if (defaultArms.activeSelf)
                            defaultArms.SetActive(false);

                        GameObject weapon = weaponHolder.transform.Find(child.GetComponent<ItemMapping>().itemName).gameObject;
                        if (weapon.activeSelf == false && weaponEquipped == false)
                        {                  
                            Debug.Log("Weapon Equiped: " + weapon.name);
                            weaponEquipped = true;
                            lastWeaponEquiped = weapon;
                            weapon.SetActive(true);                
                        }                       
                    }
                }              
            }        
        }
    }
}
