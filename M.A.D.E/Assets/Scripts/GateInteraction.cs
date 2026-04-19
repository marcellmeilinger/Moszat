using UnityEngine;

/// <summary>
/// Kezeli a kapukkal való interakciót a játékos részéről.
/// Megvalósítja az IInteractable interfészt, így a PlayerInteraction rendszeren keresztül vezérelhető.
/// </summary>
public class GateInteraction : MonoBehaviour, IInteractable
{
    /// <summary>
    /// A kapu animációiért felelős komponens referenciája.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Jelzi, hogy a kapu meg lett-e már nyitva. 
    /// Az állapotot a vizuális visszajelző rendszer is figyeli.
    /// </summary>
    public bool isOpened { get; private set; }

    /// <summary>
    /// Inicializálja az Animator komponenst a kezdéskor.
    /// </summary>
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Végrehajtja a kapu nyitását. Ezt a metódust a PlayerInteraction hívja meg.
    /// </summary>
    public void Interact()
    {
        if (!isOpened)
        {
            anim.SetTrigger("Open"); 
            isOpened = true; 
            Debug.Log("A kapu kinyílt.");
        }
    }

    /// <summary>
    /// Visszaadja a tárgy rövid leírását a UI számára.
    /// </summary>
    /// <returns>A kapu megnevezése.</returns>
    public string GetDescription()
    {
        return "Gate";
    }

    /// <summary>
    /// Ellenőrzi, hogy a kapu jelenleg interaktálható-e.
    /// Ha a kapu már nyitva van, a felirat nem jelenik meg többet.
    /// </summary>
    /// <returns>Igaz, ha a kapu még zárva van.</returns>
    public bool CanInteract()
    {
        return !isOpened; // [cite: 70]
    }
}