// file:	Assets\DownloadedContent\AllStarCharacterLibrary\Scripts\RootMotionOff.cs
//
// summary:	Implements the root motion off class

using UnityEngine;
using System.Collections;

/// <summary>   A root motion off. </summary>
///
/// <remarks>    . </remarks>

public class RootMotionOff : StateMachineBehaviour 
{
    /// <summary>   The collider test time. </summary>
	public float ColliderTestTime;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    /// <summary>   Executes the state enter action. </summary>
    ///
 
    ///
    /// <param name="animator">     The animator. </param>
    /// <param name="stateInfo">    Information describing the state. </param>
    /// <param name="layerIndex">   Zero-based index of the layer. </param>

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		animator.applyRootMotion = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    /// <summary>   Executes the state exit action. </summary>
    ///
 
    ///
    /// <param name="animator">     The animator. </param>
    /// <param name="stateInfo">    Information describing the state. </param>
    /// <param name="layerIndex">   Zero-based index of the layer. </param>

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		animator.applyRootMotion = true;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
