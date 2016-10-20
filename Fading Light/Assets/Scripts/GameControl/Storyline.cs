// file:	Assets\Scripts\GameControl\Storyline.cs
//
// summary:	Implements the storyline class

using UnityEngine;
using System.Collections;

/// <summary>   A storyline. </summary>
///
/// <remarks>    . </remarks>

public abstract class Storyline : MonoBehaviour {

    /// <summary>   Nexts this instance. </summary>
    ///
 

    public abstract void Next();

    /// <summary>   Dialogues the complete. </summary>
    ///
 

    public abstract void DialogueComplete();

    /// <summary>   Disables the mole man. </summary>
    ///
 

    public abstract void StartText();

    /// <summary>   Enables the mole man. </summary>
    ///
 

    public abstract void EnableMoleMan();

    /// <summary>   Deletes the first mole man from list. </summary>
    ///
 

    public abstract void NextMoleMan();

    /// <summary>   Character damage enabled. </summary>
    ///
 
    ///
    /// <param name="enabled">  True to enable, false to disable. </param>

    public abstract void CharacterDamageEnabled(bool enabled);

}
