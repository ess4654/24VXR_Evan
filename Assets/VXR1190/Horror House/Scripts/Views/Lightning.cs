using UnityEngine;

namespace HorrorHouse.Views
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private Light lightingFlash;
        [SerializeField] private AudioSource thunderSound;
        [SerializeField] private AudioClip[] thunderAudioClips;

        private ParticleSystem lighting;
        private bool on;

        private void Awake()
        {
            lighting = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!on && lighting.particleCount > 0)
            {
                on = true;
                if (thunderSound && thunderAudioClips.Length > 0)
                    thunderSound.PlayOneShot(thunderAudioClips[Random.Range(0, thunderAudioClips.Length)]);
            }
            else if (on && lighting.particleCount == 0)
            {
                on = false;
                if (lightingFlash)
                    lightingFlash.gameObject.SetActive(false);
            }

            if (on)
                OnLighting();
        }

        private void OnLighting()
        {
            if (lightingFlash && Random.value < .2f)
                lightingFlash.gameObject.SetActive(!lightingFlash.gameObject.activeInHierarchy);
        }
    }
}