// file:	Assets\UnityTestTools\Assertions\AssertionComponent.cs
//
// summary:	Implements the assertion component class

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace UnityTest
{
    /// <summary>   (Serializable) an assertion component. </summary>
    ///
 

    [Serializable]
    public class AssertionComponent : MonoBehaviour, IAssertionComponentConfigurator
    {
        /// <summary>   The check after time. </summary>
        [SerializeField] public float checkAfterTime = 1f;
        /// <summary>   The repeat check time. </summary>
        [SerializeField] public bool repeatCheckTime = true;
        /// <summary>   The repeat every time. </summary>
        [SerializeField] public float repeatEveryTime = 1f;
        /// <summary>   The check after frames. </summary>
        [SerializeField] public int checkAfterFrames = 1;
        /// <summary>   The repeat check frame. </summary>
        [SerializeField] public bool repeatCheckFrame = true;
        /// <summary>   The repeat every frame. </summary>
        [SerializeField] public int repeatEveryFrame = 1;

        /// <summary>   Gets or sets a value indicating whether this object has failed. </summary>
        ///
        /// <value> True if this object has failed, false if not. </value>

        [SerializeField] public bool hasFailed;

        /// <summary>   The check methods. </summary>
        [SerializeField] public CheckMethod checkMethods = CheckMethod.Start;

        /// <summary>   Gets or sets the action base. </summary>
        ///
        /// <value> The m action base. </value>

        [SerializeField] private ActionBase m_ActionBase;

        /// <summary>   The checks performed. </summary>
        [SerializeField] public int checksPerformed = 0;

        /// <summary>   The check on frame. </summary>
        private int m_CheckOnFrame;

        /// <summary>   Full pathname of the created in file. </summary>
        private string m_CreatedInFilePath = "";
        /// <summary>   The created in file line. </summary>
        private int m_CreatedInFileLine = -1;

        /// <summary>   Gets or sets the action. </summary>
        ///
        /// <value> The action. </value>

        public ActionBase Action
        {
            get { return m_ActionBase; }
            set
            {
                m_ActionBase = value;
                m_ActionBase.go = gameObject;
            }
        }

        /// <summary>   Gets failure reference object. </summary>
        ///
     
        ///
        /// <returns>   The failure reference object. </returns>

        public Object GetFailureReferenceObject()
        {
            #if UNITY_EDITOR
            if (!string.IsNullOrEmpty(m_CreatedInFilePath))
            {
                return UnityEditor.AssetDatabase.LoadAssetAtPath(m_CreatedInFilePath, typeof(Object));
            }
            #endif
            return this;
        }

        /// <summary>   Gets creation location. </summary>
        ///
     
        ///
        /// <returns>   The creation location. </returns>

        public string GetCreationLocation()
        {
            if (!string.IsNullOrEmpty(m_CreatedInFilePath))
            {
                var idx = m_CreatedInFilePath.LastIndexOf("\\") + 1;
                return string.Format("{0}, line {1} ({2})", m_CreatedInFilePath.Substring(idx), m_CreatedInFileLine, m_CreatedInFilePath);
            }
            return "";
        }

        /// <summary>   Awakes this object. </summary>
        ///
     

        public void Awake()
        {
            if (!Debug.isDebugBuild)
                Destroy(this);
            OnComponentCopy();
        }

        /// <summary>   Executes the validate action. </summary>
        ///
     

        public void OnValidate()
        {
            if (Application.isEditor)
                OnComponentCopy();
        }

        /// <summary>   Executes the component copy action. </summary>
        ///
     

        private void OnComponentCopy()
        {
            if (m_ActionBase == null) return;
            var oldActionList = Resources.FindObjectsOfTypeAll(typeof(AssertionComponent)).Where(o => ((AssertionComponent)o).m_ActionBase == m_ActionBase && o != this);

            // if it's not a copy but a new component don't do anything
            if (!oldActionList.Any()) return;
            if (oldActionList.Count() > 1)
                Debug.LogWarning("More than one refence to comparer found. This shouldn't happen");

            var oldAction = oldActionList.First() as AssertionComponent;
            m_ActionBase = oldAction.m_ActionBase.CreateCopy(oldAction.gameObject, gameObject);
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        public void Start()
        {
            CheckAssertionFor(CheckMethod.Start);

            if (IsCheckMethodSelected(CheckMethod.AfterPeriodOfTime))
            {
                StartCoroutine("CheckPeriodically");
            }
            if (IsCheckMethodSelected(CheckMethod.Update))
            {
                m_CheckOnFrame = Time.frameCount + checkAfterFrames;
            }
        }

        /// <summary>   Check periodically. </summary>
        ///
     
        ///
        /// <returns>   An IEnumerator. </returns>

        public IEnumerator CheckPeriodically()
        {
            yield return new WaitForSeconds(checkAfterTime);
            CheckAssertionFor(CheckMethod.AfterPeriodOfTime);
            while (repeatCheckTime)
            {
                yield return new WaitForSeconds(repeatEveryTime);
                CheckAssertionFor(CheckMethod.AfterPeriodOfTime);
            }
        }

        /// <summary>   Determine if we should check on frame. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool ShouldCheckOnFrame()
        {
            if (Time.frameCount > m_CheckOnFrame)
            {
                if (repeatCheckFrame)
                    m_CheckOnFrame += repeatEveryFrame;
                else
                    m_CheckOnFrame = Int32.MaxValue;
                return true;
            }
            return false;
        }

        /// <summary>   Executes the disable action. </summary>
        ///
     

        public void OnDisable()
        {
            CheckAssertionFor(CheckMethod.OnDisable);
        }

        /// <summary>   Executes the enable action. </summary>
        ///
     

        public void OnEnable()
        {
            CheckAssertionFor(CheckMethod.OnEnable);
        }

        /// <summary>   Executes the destroy action. </summary>
        ///
     

        public void OnDestroy()
        {
            CheckAssertionFor(CheckMethod.OnDestroy);
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        public void Update()
        {
            if (IsCheckMethodSelected(CheckMethod.Update) && ShouldCheckOnFrame())
            {
                CheckAssertionFor(CheckMethod.Update);
            }
        }

        /// <summary>   Fixed update. </summary>
        ///
     

        public void FixedUpdate()
        {
            CheckAssertionFor(CheckMethod.FixedUpdate);
        }

        /// <summary>   Late update. </summary>
        ///
     

        public void LateUpdate()
        {
            CheckAssertionFor(CheckMethod.LateUpdate);
        }

        /// <summary>   Executes the controller collider hit action. </summary>
        ///
     

        public void OnControllerColliderHit()
        {
            CheckAssertionFor(CheckMethod.OnControllerColliderHit);
        }

        /// <summary>   Executes the particle collision action. </summary>
        ///
     

        public void OnParticleCollision()
        {
            CheckAssertionFor(CheckMethod.OnParticleCollision);
        }

        /// <summary>   Executes the joint break action. </summary>
        ///
     

        public void OnJointBreak()
        {
            CheckAssertionFor(CheckMethod.OnJointBreak);
        }

        /// <summary>   Executes the became invisible action. </summary>
        ///
     

        public void OnBecameInvisible()
        {
            CheckAssertionFor(CheckMethod.OnBecameInvisible);
        }

        /// <summary>   Executes the became visible action. </summary>
        ///
     

        public void OnBecameVisible()
        {
            CheckAssertionFor(CheckMethod.OnBecameVisible);
        }

        /// <summary>   Executes the trigger enter action. </summary>
        ///
     

        public void OnTriggerEnter()
        {
            CheckAssertionFor(CheckMethod.OnTriggerEnter);
        }

        /// <summary>   Executes the trigger exit action. </summary>
        ///
     

        public void OnTriggerExit()
        {
            CheckAssertionFor(CheckMethod.OnTriggerExit);
        }

        /// <summary>   Executes the trigger stay action. </summary>
        ///
     

        public void OnTriggerStay()
        {
            CheckAssertionFor(CheckMethod.OnTriggerStay);
        }

        /// <summary>   Executes the collision enter action. </summary>
        ///
     

        public void OnCollisionEnter()
        {
            CheckAssertionFor(CheckMethod.OnCollisionEnter);
        }

        /// <summary>   Executes the collision exit action. </summary>
        ///
     

        public void OnCollisionExit()
        {
            CheckAssertionFor(CheckMethod.OnCollisionExit);
        }

        /// <summary>   Executes the collision stay action. </summary>
        ///
     

        public void OnCollisionStay()
        {
            CheckAssertionFor(CheckMethod.OnCollisionStay);
        }

        /// <summary>   Executes the trigger enter 2D action. </summary>
        ///
     

        public void OnTriggerEnter2D()
        {
            CheckAssertionFor(CheckMethod.OnTriggerEnter2D);
        }

        /// <summary>   Executes the trigger exit 2D action. </summary>
        ///
     

        public void OnTriggerExit2D()
        {
            CheckAssertionFor(CheckMethod.OnTriggerExit2D);
        }

        /// <summary>   Executes the trigger stay 2D action. </summary>
        ///
     

        public void OnTriggerStay2D()
        {
            CheckAssertionFor(CheckMethod.OnTriggerStay2D);
        }

        /// <summary>   Executes the collision enter 2D action. </summary>
        ///
     

        public void OnCollisionEnter2D()
        {
            CheckAssertionFor(CheckMethod.OnCollisionEnter2D);
        }

        /// <summary>   Executes the collision exit 2D action. </summary>
        ///
     

        public void OnCollisionExit2D()
        {
            CheckAssertionFor(CheckMethod.OnCollisionExit2D);
        }

        /// <summary>   Executes the collision stay 2D action. </summary>
        ///
     

        public void OnCollisionStay2D()
        {
            CheckAssertionFor(CheckMethod.OnCollisionStay2D);
        }

        /// <summary>   Check assertion for. </summary>
        ///
     
        ///
        /// <param name="checkMethod">  The check method. </param>

        private void CheckAssertionFor(CheckMethod checkMethod)
        {
            if (IsCheckMethodSelected(checkMethod))
            {
                Assertions.CheckAssertions(this);
            }
        }

        /// <summary>   Query if 'method' is check method selected. </summary>
        ///
     
        ///
        /// <param name="method">   The method. </param>
        ///
        /// <returns>   True if check method selected, false if not. </returns>

        public bool IsCheckMethodSelected(CheckMethod method)
        {
            return method == (checkMethods & method);
        }


        #region Assertion Component create methods

        /// <summary>   Creates a new T. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        ///
        /// <returns>   A T. </returns>

        public static T Create<T>(CheckMethod checkOnMethods, GameObject gameObject, string propertyPath) where T : ActionBase
        {
            IAssertionComponentConfigurator configurator;
            return Create<T>(out configurator, checkOnMethods, gameObject, propertyPath);
        }

        /// <summary>   Creates a new T. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="configurator">     [out] The configurator. </param>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        ///
        /// <returns>   A T. </returns>

        public static T Create<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath) where T : ActionBase
        {
            return CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
        }

        /// <summary>   Creates a new T. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        /// <param name="gameObject2">      The second game object. </param>
        /// <param name="propertyPath2">    The second property path. </param>
        ///
        /// <returns>   A T. </returns>

        public static T Create<T>(CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, GameObject gameObject2, string propertyPath2) where T : ComparerBase
        {
            IAssertionComponentConfigurator configurator;
            return Create<T>(out configurator, checkOnMethods, gameObject, propertyPath, gameObject2, propertyPath2);
        }

        /// <summary>   Creates a new T. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="configurator">     [out] The configurator. </param>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        /// <param name="gameObject2">      The second game object. </param>
        /// <param name="propertyPath2">    The second property path. </param>
        ///
        /// <returns>   A T. </returns>

        public static T Create<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, GameObject gameObject2, string propertyPath2) where T : ComparerBase
        {
            var comparer = CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
            comparer.compareToType = ComparerBase.CompareToType.CompareToObject;
            comparer.other = gameObject2;
            comparer.otherPropertyPath = propertyPath2;
            return comparer;
        }

        /// <summary>   Creates a new T. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        /// <param name="constValue">       The constant value. </param>
        ///
        /// <returns>   A T. </returns>

        public static T Create<T>(CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, object constValue) where T : ComparerBase
        {
            IAssertionComponentConfigurator configurator;
            return Create<T>(out configurator, checkOnMethods, gameObject, propertyPath, constValue);
        }

        /// <summary>   Creates a new T. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="configurator">     [out] The configurator. </param>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        /// <param name="constValue">       The constant value. </param>
        ///
        /// <returns>   A T. </returns>

        public static T Create<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, object constValue) where T : ComparerBase
        {
            var comparer = CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
            if (constValue == null)
            {
                comparer.compareToType = ComparerBase.CompareToType.CompareToNull;
                return comparer;
            }
            comparer.compareToType = ComparerBase.CompareToType.CompareToConstantValue;
            comparer.ConstValue = constValue;
            return comparer;
        }

        /// <summary>   Creates assertion component. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="configurator">     [out] The configurator. </param>
        /// <param name="checkOnMethods">   The check on methods. </param>
        /// <param name="gameObject">       The game object. </param>
        /// <param name="propertyPath">     Full pathname of the property file. </param>
        ///
        /// <returns>   The new assertion component. </returns>

        private static T CreateAssertionComponent<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath) where T : ActionBase
        {
            var ac = gameObject.AddComponent<AssertionComponent>();
            ac.checkMethods = checkOnMethods;
            var comparer = ScriptableObject.CreateInstance<T>();
            ac.Action = comparer;
            ac.Action.go = gameObject;
            ac.Action.thisPropertyPath = propertyPath;
            configurator = ac;

#if !UNITY_METRO
            var stackTrace = new StackTrace(true);
            var thisFileName = stackTrace.GetFrame(0).GetFileName();
            for (int i = 1; i < stackTrace.FrameCount; i++)
            {
                var stackFrame = stackTrace.GetFrame(i);
                if (stackFrame.GetFileName() != thisFileName)
                {
                    string filePath = stackFrame.GetFileName().Substring(Application.dataPath.Length - "Assets".Length);
                    ac.m_CreatedInFilePath = filePath;
                    ac.m_CreatedInFileLine = stackFrame.GetFileLineNumber();
                    break;
                }
            }
#endif  // if !UNITY_METRO
            return comparer;
        }

        #endregion

        #region AssertionComponentConfigurator

        /// <summary>
        /// If the assertion is evaluated in Update, after how many frame should the evaluation start.
        /// Deafult is 1 (first frame)
        /// </summary>
        ///
        /// <value> The update check start on frame. </value>

        public int UpdateCheckStartOnFrame { set { checkAfterFrames = value; } }

        /// <summary>
        /// If the assertion is evaluated in Update and UpdateCheckRepeat is true, how many frame should
        /// pass between evaluations.
        /// </summary>
        ///
        /// <value> The update check repeat frequency. </value>

        public int UpdateCheckRepeatFrequency { set { repeatEveryFrame = value; } }

        /// <summary>
        /// If the assertion is evaluated in Update, should the evaluation be repeated after
        /// UpdateCheckRepeatFrequency frames.
        /// </summary>
        ///
        /// <value> True if update check repeat, false if not. </value>

        public bool UpdateCheckRepeat { set { repeatCheckFrame = value; } }

        /// <summary>
        /// If the assertion is evaluated after a period of time, after how many seconds the first
        /// evaluation should be done.
        /// </summary>
        ///
        /// <value> The time check start after. </value>

        public float TimeCheckStartAfter { set { checkAfterTime = value; } }

        /// <summary>
        /// If the assertion is evaluated after a period of time and TimeCheckRepeat is true, after how
        /// many seconds should the next evaluation happen.
        /// </summary>
        ///
        /// <value> The time check repeat frequency. </value>

        public float TimeCheckRepeatFrequency { set { repeatEveryTime = value; } }

        /// <summary>
        /// If the assertion is evaluated after a period, should the evaluation happen again after
        /// TimeCheckRepeatFrequency seconds.
        /// </summary>
        ///
        /// <value> True if time check repeat, false if not. </value>

        public bool TimeCheckRepeat { set { repeatCheckTime = value; } }

        /// <summary>   Gets the component. </summary>
        ///
        /// <value> The component. </value>

        public AssertionComponent Component { get { return this; } }
        #endregion
    }

    /// <summary>   Interface for assertion component configurator. </summary>
    ///
 

    public interface IAssertionComponentConfigurator
    {
        /// <summary>
        /// If the assertion is evaluated in Update, after how many frame should the evaluation start.
        /// Deafult is 1 (first frame)
        /// </summary>
        ///
        /// <value> The update check start on frame. </value>

        int UpdateCheckStartOnFrame { set; }

        /// <summary>
        /// If the assertion is evaluated in Update and UpdateCheckRepeat is true, how many frame should
        /// pass between evaluations.
        /// </summary>
        ///
        /// <value> The update check repeat frequency. </value>

        int UpdateCheckRepeatFrequency { set; }

        /// <summary>
        /// If the assertion is evaluated in Update, should the evaluation be repeated after
        /// UpdateCheckRepeatFrequency frames.
        /// </summary>
        ///
        /// <value> True if update check repeat, false if not. </value>

        bool UpdateCheckRepeat { set; }

        /// <summary>
        /// If the assertion is evaluated after a period of time, after how many seconds the first
        /// evaluation should be done.
        /// </summary>
        ///
        /// <value> The time check start after. </value>

        float TimeCheckStartAfter { set; }

        /// <summary>
        /// If the assertion is evaluated after a period of time and TimeCheckRepeat is true, after how
        /// many seconds should the next evaluation happen.
        /// </summary>
        ///
        /// <value> The time check repeat frequency. </value>

        float TimeCheckRepeatFrequency { set; }

        /// <summary>
        /// If the assertion is evaluated after a period, should the evaluation happen again after
        /// TimeCheckRepeatFrequency seconds.
        /// </summary>
        ///
        /// <value> True if time check repeat, false if not. </value>

        bool TimeCheckRepeat { set; }

        /// <summary>   Gets the component. </summary>
        ///
        /// <value> The component. </value>

        AssertionComponent Component { get; }
    }
}
