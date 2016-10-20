/*
 * Copyright (c) 2016 Made With Mosnter Love (Pty) Ltd
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included 
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR 
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = System.Object;

namespace MonsterLove.StateMachine
{
    /// <summary>   A state machine runner. </summary>
    ///
 

	public class StateMachineRunner : MonoBehaviour
	{
        /// <summary>   List of state machines. </summary>
		private List<IStateMachine> stateMachineList = new List<IStateMachine>();

        /// <summary>
        /// Creates a stateMachine token object which is used to managed to the state of a monobehaviour.
        /// </summary>
        ///
     
        ///
        /// <typeparam name="T">    An Enum listing different state transitions. </typeparam>
        /// <param name="component">    The component whose state will be managed. </param>
        ///
        /// <returns>   A StateMachine&lt;T&gt; </returns>

		public StateMachine<T> Initialize<T>(MonoBehaviour component) where T : struct, IConvertible, IComparable
		{
			var fsm = new StateMachine<T>(this, component);

			stateMachineList.Add(fsm);

			return fsm;
		}

        /// <summary>
        /// Creates a stateMachine token object which is used to managed to the state of a monobehaviour.
        /// Will automatically transition the startState.
        /// </summary>
        ///
     
        ///
        /// <typeparam name="T">    An Enum listing different state transitions. </typeparam>
        /// <param name="component">    The component whose state will be managed. </param>
        /// <param name="startState">   The default start state. </param>
        ///
        /// <returns>   A StateMachine&lt;T&gt; </returns>

		public StateMachine<T> Initialize<T>(MonoBehaviour component, T startState) where T : struct, IConvertible, IComparable
		{
			var fsm = Initialize<T>(component);

			fsm.ChangeState(startState);

			return fsm;
		}

        /// <summary>   Fixed update. </summary>
        ///
     

		void FixedUpdate()
		{
			for (int i = 0; i < stateMachineList.Count; i++)
			{
				var fsm = stateMachineList[i];
				if(!fsm.IsInTransition && fsm.Component.enabled) fsm.CurrentStateMap.FixedUpdate();
			}
		}

        /// <summary>   Updates this object. </summary>
        ///
     

		void Update()
		{
			for (int i = 0; i < stateMachineList.Count; i++)
			{
				var fsm = stateMachineList[i];
				if (!fsm.IsInTransition && fsm.Component.enabled)
				{
					fsm.CurrentStateMap.Update();
				}
			}
		}

        /// <summary>   Late update. </summary>
        ///
     

		void LateUpdate()
		{
			for (int i = 0; i < stateMachineList.Count; i++)
			{
				var fsm = stateMachineList[i];
				if (!fsm.IsInTransition && fsm.Component.enabled)
				{
					fsm.CurrentStateMap.LateUpdate();
				}
			}
		}

		//void OnCollisionEnter(Collision collision)
		//{
		//	if(currentState != null && !IsInTransition)
		//	{
		//		currentState.OnCollisionEnter(collision);
		//	}
		//}

        /// <summary>   Executes the nothing operation. </summary>
        ///
     

		public static void DoNothing()
		{
		}

        /// <summary>   Executes the nothing collider operation. </summary>
        ///
     
        ///
        /// <param name="other">    The other. </param>

		public static void DoNothingCollider(Collider other)
		{
		}

        /// <summary>   Executes the nothing collision operation. </summary>
        ///
     
        ///
        /// <param name="other">    The other. </param>

		public static void DoNothingCollision(Collision other)
		{
		}

        /// <summary>   Executes the nothing coroutine operation. </summary>
        ///
     
        ///
        /// <returns>   An IEnumerator. </returns>

		public static IEnumerator DoNothingCoroutine()
		{
			yield break;
		}
	}

    /// <summary>   A state mapping. </summary>
    ///
 

	public class StateMapping
	{
        /// <summary>   The state. </summary>
		public object state;

        /// <summary>   True if this object has enter routine. </summary>
		public bool hasEnterRoutine;
        /// <summary>   The enter call. </summary>
		public Action EnterCall = StateMachineRunner.DoNothing;
        /// <summary>   The enter routine. </summary>
		public Func<IEnumerator> EnterRoutine = StateMachineRunner.DoNothingCoroutine;

        /// <summary>   True if this object has exit routine. </summary>
		public bool hasExitRoutine;
        /// <summary>   The exit call. </summary>
		public Action ExitCall = StateMachineRunner.DoNothing;
        /// <summary>   The exit routine. </summary>
		public Func<IEnumerator> ExitRoutine = StateMachineRunner.DoNothingCoroutine;

        /// <summary>   The finally. </summary>
		public Action Finally = StateMachineRunner.DoNothing;
        /// <summary>   The update. </summary>
		public Action Update = StateMachineRunner.DoNothing;
        /// <summary>   The late update. </summary>
		public Action LateUpdate = StateMachineRunner.DoNothing;
        /// <summary>   The fixed update. </summary>
		public Action FixedUpdate = StateMachineRunner.DoNothing;
        /// <summary>   The on collision enter. </summary>
		public Action<Collision> OnCollisionEnter = StateMachineRunner.DoNothingCollision;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="state">    The state. </param>

		public StateMapping(object state)
		{
			this.state = state;
		}

	}
}


