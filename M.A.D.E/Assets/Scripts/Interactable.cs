using UnityEngine;

/// <summary>
/// Egy központi interfész minden olyan játékelem számára, amellyel a játékos 
/// kapcsolatba léphet (pl. ládák, ajtók, érmék, italok).
/// Biztosítja az egységes kezelést az interakciós rendszer és a UI visszajelzések számára.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Végrehajtja a tárgyhoz rendelt specifikus interakciót (pl. kinyílik, ad egy érmét).
    /// Ez a metódus hívódik meg, amikor a játékos megnyomja az interakciós gombot.
    /// </summary>
    void Interact();

    /// <summary>
    /// Visszaad egy leíró szöveget az objektumról, ami a UI-on jelenhet meg.
    /// Segít a játékosnak azonosítani az objektumot vagy a végrehajtható műveletet.
    /// </summary>
    /// <returns>A tárgy interakciós leírása (pl. "Kincsesláda").</returns>
    string GetDescription();

    /// <summary>
    /// Megvizsgálja, hogy az objektum jelenleg interaktálható állapotban van-e.
    /// Ezt használja a rendszer annak eldöntésére, hogy megjelenítse-e az interakciós feliratot.
    /// </summary>
    /// <returns>Igaz, ha az objektummal még lehet interaktálni (pl. a láda még nincs kinyitva).</returns>
    bool CanInteract();
}