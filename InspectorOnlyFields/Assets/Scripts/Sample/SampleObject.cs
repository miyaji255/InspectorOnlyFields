using InspectorOnlyFields;
using UnityEngine;

namespace Sample
{
    public class SampleObject : MonoBehaviour
    {
        [InspectorOnly]
        public GameObject GameObject = new GameObject();

        // Start is called before the first frame update
        void Start() {
            GameObject = new GameObject();
        }

        // Update is called once per frame
        void Update() {

        }
    }

    public class Sample
    {
        [InspectorOnly]
        public GameObject GameObject = new GameObject();

        // Start is called before the first frame update
        void Start()
        {
            GameObject = new GameObject();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

