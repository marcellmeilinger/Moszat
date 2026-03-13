using UnityEngine;

/// <summary>
/// Egy központi interfész minden olyan játékelem számára, amellyel a játékos 
/// kapcsolatba léphet (pl. ládák, ajtók, érmék, italok).
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Végrehajtja a tárgyhoz rendelt specifikus interakciót (pl. kinyílik, ad egy érmét).
    /// </summary>
    void Interact();

    /// <summary>
    /// Visszaad egy leíró szöveget az objektumról, ami UI-on jelenhet meg (pl. "Nyomd meg az E-t a nyitáshoz").
    /// </summary>
    /// <returns>A tárgy interakciós leírása.</returns>
    string GetDescription();
}
