using UnityEngine;
using System.Collections;
using Collect.Utils;

namespace Collect.Containers {

    [AddComponentMenu("Collect/Containers/Container Parent")]
    public class ContainerParent : MonoBehaviour {

        public GameObject ContainerPrefab;

        private Container container;
        public Container Container {
            get { return container; }
            set { container = value; }
        }

        public void Start() {
            if (ContainerPrefab == null) {
                throw new MissingReferenceException("Container Parent must have a Container Prefab set.");
            }

            GameObject containerObject = GameObject.Instantiate(ContainerPrefab);
            containerObject.transform.SetParent(CanvasHelper.GetCanvas().transform);
            container = containerObject.GetComponent<Container>();

            if (container == null) {
                throw new MissingComponentException("Container Prefab must have `Container` component attached.");
            }

            //  don't show by default
            container.Close();
        }
    }
}
