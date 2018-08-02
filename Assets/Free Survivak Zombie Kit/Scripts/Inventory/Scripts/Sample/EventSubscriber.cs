using UnityEngine;
using System.Collections;

using Collect.Events;
using Collect.Slots;
using UnityEngine.EventSystems;

public class EventSubscriber : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //  Listening to the `ItemDidDrop` event
        ItemEventManager.OnItemDidInvalidDrop += ItemDidInvalidDrop;
	}

    public void ItemDidInvalidDrop(GameObject item, Slot slot, PointerEventData data) {
        Debug.Log(item.name);
        Debug.Log(slot.Item.name);
        Debug.Log(slot.name);
        Debug.Log(data.position);
    }
}
