using UnityEngine;
using System.Collections;

using Collect.Containers;

namespace Collect.Sample {

    public class Chest : MonoBehaviour {

        public GameObject ContainerObject;

        public void Start() {
            ContainerObject.SetActive(false);
        }

        public void OnMouseDown() {
            if (ContainerObject.active == true) {
                ContainerObject.SetActive(false);
            } else {
                ContainerObject.SetActive(true);
            }
        }
    }
}
