using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Collect.Containers;
using Collect.Items;

namespace Collect.Sample {

    public class Bandit : MonoBehaviour, ContainerDelegate {

        public GameObject HeadObject;
        public GameObject ChestObject;
        public GameObject LegObject;
        public GameObject ContainerObject;
        public GameObject SayingsObject;
        public float LabelDelay = 3.0f;

        public string[] ItemAddedSayings;
        public string[] ItemRemovedSayings;

        private Container equipment;
        private Text sayingsLabel;

        public void Start() {
            equipment = ContainerObject.GetComponent<Container>();
            sayingsLabel = SayingsObject.GetComponent<Text>();

            equipment.ItemWasAdded += ItemWasAdded;
            equipment.ItemWasRemoved += ItemWasRemoved;

            HeadObject.SetActive(false);
            ChestObject.SetActive(false);
            LegObject.SetActive(false);
            HideLabel();
        }

        public void OnDestroy() {
            equipment.ItemWasAdded -= ItemWasAdded;
            equipment.ItemWasRemoved -= ItemWasRemoved;
        }

        public void ItemWasAdded(GameObject item) {
            DraggableItemType itemType = item.GetComponent<DraggableItemType>();

            if (itemType.ItemType == ItemType.Head) {
                HeadObject.SetActive(true);
            } else if (itemType.ItemType == ItemType.Chest) {
                ChestObject.SetActive(true);
            } else if (itemType.ItemType == ItemType.Legs) {
                LegObject.SetActive(true);
            }

            saySomething(ItemAddedSayings);
        }

        public void ItemWasRemoved(GameObject item) {
            DraggableItemType itemType = item.GetComponent<DraggableItemType>();

            if (itemType.ItemType == ItemType.Head) {
                HeadObject.SetActive(false);
            } else if (itemType.ItemType == ItemType.Chest) {
                ChestObject.SetActive(false);
            } else if (itemType.ItemType == ItemType.Legs) {
                LegObject.SetActive(false);
            }

            saySomething(ItemRemovedSayings);
        }

        public void HideLabel() {
            sayingsLabel.text = "";
        }

        private void saySomething(string[] sayingsList) {

            CancelInvoke("HideLabel");

            string saying = sayingsList[(int) Random.Range(0, sayingsList.Length)];
            sayingsLabel.text = saying;

            Invoke("HideLabel", LabelDelay);
        }
    }
}
