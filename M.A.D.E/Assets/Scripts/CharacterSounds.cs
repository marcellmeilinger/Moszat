using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Sound Effects")]
    public AudioClip attackSound;
    public AudioClip takeHitSound;
    public AudioClip deathSound;
    public AudioClip runSound; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayAttackSound()
    {
        if (attackSound != null) audioSource.PlayOneShot(attackSound);
    }

    public void PlayHitSound()
    {
        if (takeHitSound != null) audioSource.PlayOneShot(takeHitSound);
    }

    public void PlayDeathSound()
    {
        if (deathSound != null) audioSource.PlayOneShot(deathSound);
    }

    public void PlayRunSound()
    {
        if (runSound != null) audioSource.PlayOneShot(runSound);
    }
}