using UnityEngine;

/// <summary>
/// A karakter (játékos vagy ellenfél) hanghatásainak lejátszásáért felelős osztály.
/// </summary>
public class CharacterSounds : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Sound Effects")]
    public AudioClip attackSound;
    public AudioClip takeHitSound;
    public AudioClip deathSound;
    public AudioClip runSound; 

    /// <summary>
    /// Inicializálja az AudioSource komponenst a kezdéskor.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    /// <summary>
    /// Támadás hangjának lejátszása.
    /// </summary>
    public void PlayAttackSound()
    {
        if (attackSound != null) audioSource.PlayOneShot(attackSound);
    }

    /// <summary>
    /// Sebződés hangjának lejátszása.
    /// </summary>
    public void PlayHitSound()
    {
        if (takeHitSound != null) audioSource.PlayOneShot(takeHitSound);
    }

    /// <summary>
    /// Halál hangjának lejátszása.
    /// </summary>
    public void PlayDeathSound()
    {
        if (deathSound != null) audioSource.PlayOneShot(deathSound);
    }

    /// <summary>
    /// Futás hangjának lejátszása.
    /// </summary>
    public void PlayRunSound()
    {
        if (runSound != null) audioSource.PlayOneShot(runSound);
    }
}