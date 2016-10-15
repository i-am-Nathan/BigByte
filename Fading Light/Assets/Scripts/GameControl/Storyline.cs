using UnityEngine;
using System.Collections;

public abstract class Storyline : MonoBehaviour {

    /// <summary>
    /// Nexts this instance.
    /// </summary>
    public abstract void Next();

    /// <summary>
    /// Dialogues the complete.
    /// </summary>
    public abstract void DialogueComplete();

    /// <summary>
    /// Disables the mole man.
    /// </summary>
    public abstract void DisableMoleMan();

    /// <summary>
    /// Enables the mole man.
    /// </summary>
    public abstract void EnableMoleMan();

    /// <summary>
    /// Deletes the first mole man from list.
    /// </summary>
    public abstract void NextMoleMan();

}
