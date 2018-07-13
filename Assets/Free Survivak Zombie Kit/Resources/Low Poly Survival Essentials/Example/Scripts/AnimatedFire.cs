using UnityEngine;
using Random = UnityEngine.Random;

namespace BrokenVector
{
    [RequireComponent(typeof(Light))]
    public class AnimatedFire : MonoBehaviour
    {

        public float MinIntensity = 4;
        public float MaxIntensity = 6;
        public float Speed = 1;

        private new Light light;
        private float initialRandom;

        void Awake()
        {
            this.light = this.GetComponent<Light>();
        }

        void Start()
        {
            initialRandom = Random.Range(0f, 65535f);
        }

        void Update()
        {
            float noise = Mathf.PerlinNoise(initialRandom, Speed * Time.time);
            light.intensity = Mathf.Lerp(MinIntensity, MaxIntensity, noise);
        }
        
    }
}