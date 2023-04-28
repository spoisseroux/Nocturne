using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSoundSystem : MonoBehaviour    
{
    private ParticleSystem particleSystem;
    private AudioSource audioSource;

    public AudioClip[] BornSounds;

    private int currentNumberOfParticles;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var amount = Mathf.Abs(currentNumberOfParticles - particleSystem.particleCount);

        if (particleSystem.particleCount > currentNumberOfParticles) {
            StartCoroutine(PlaySound(BornSounds[Random.Range(0, BornSounds.Length)], amount));
        }

        currentNumberOfParticles = particleSystem.particleCount;
    }

    private IEnumerator PlaySound(AudioClip clip, int amount) {
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(0.05f); //dont overlap
    }
}
