using UnityEngine;
using System.Collections;
using Collect.Containers;
using System;

public class ContainerOpener : MonoBehaviour, ContainerDelegate {

    private ContainerParent containerParent;

	// Use this for initialization
	void Start () {
        containerParent = GetComponent<ContainerParent>();

        containerParent.Container.ItemWasAdded += ItemWasAdded;
        containerParent.Container.ItemWasRemoved += ItemWasRemoved;
	}

    public void OnDestroy() {
        containerParent.Container.ItemWasAdded -= ItemWasAdded;
        containerParent.Container.ItemWasRemoved -= ItemWasRemoved;
    }
	
	public void OnMouseDown() {
        containerParent.Container.Toggle();
    }

    public void ItemWasAdded(GameObject item) {
        Debug.Log("An item was added to this object's container: " + item.name);
    }

    public void ItemWasRemoved(GameObject item) {
        Debug.Log("An item was removed from this object's container: " + item.name);
    }
}
