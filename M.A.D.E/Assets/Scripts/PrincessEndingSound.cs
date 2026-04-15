using UnityEngine;

public class PrincessEndingSound : MonoBehaviour, IInteractable
{
    [Header("Sound effects")]
    public AudioClip cheerSound;

    
    public void Interact()
    {
        if (cheerSound != null)
        {
            AudioSource.PlayClipAtPoint(cheerSound, transform.position);
        }

       
    }

    public string GetDescription()
    {
        return "Talk to Princess";
    }
}