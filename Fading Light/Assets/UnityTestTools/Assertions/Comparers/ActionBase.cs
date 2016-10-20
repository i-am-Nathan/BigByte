// file:	Assets\UnityTestTools\Assertions\Comparers\ActionBase.cs
//
// summary:	Implements the action base class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   An action base. </summary>
    ///
 

    public abstract class ActionBase : ScriptableObject
    {
        /// <summary>   The go. </summary>
        public GameObject go;
        /// <summary>   The object value. </summary>
        protected object m_ObjVal;

        /// <summary>   The member resolver. </summary>
        private MemberResolver m_MemberResolver;

        /// <summary>   Full pathname of this property file. </summary>
        public string thisPropertyPath = "";

        /// <summary>   Gets accepatble types for a. </summary>
        ///
     
        ///
        /// <returns>   An array of type. </returns>

        public virtual Type[] GetAccepatbleTypesForA()
        {
            return null;
        }

        /// <summary>   Gets depth of search. </summary>
        ///
     
        ///
        /// <returns>   The depth of search. </returns>

        public virtual int GetDepthOfSearch() { return 2; }

        /// <summary>   Gets excluded field names. </summary>
        ///
     
        ///
        /// <returns>   An array of string. </returns>

        public virtual string[] GetExcludedFieldNames()
        {
            return new string[] { };
        }

        /// <summary>
        /// Compares this object object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool Compare()
        {
            if (m_MemberResolver == null)
                m_MemberResolver = new MemberResolver(go, thisPropertyPath);
            m_ObjVal = m_MemberResolver.GetValue(UseCache);
            var result = Compare(m_ObjVal);
            return result;
        }

        /// <summary>
        /// Compares this object object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="objVal">   The object to compare to this object. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected abstract bool Compare(object objVal);

        /// <summary>   Gets a value indicating whether this object use cache. </summary>
        ///
        /// <value> True if use cache, false if not. </value>

        virtual protected bool UseCache { get { return false; } }

        /// <summary>   Gets parameter type. </summary>
        ///
     
        ///
        /// <returns>   The parameter type. </returns>

        public virtual Type GetParameterType() { return typeof(object); }

        /// <summary>   Gets configuration description. </summary>
        ///
     
        ///
        /// <returns>   The configuration description. </returns>

        public virtual string GetConfigurationDescription()
        {
            string result = "";
#if !UNITY_METRO
            foreach (var prop in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                     .Where(info => info.FieldType.IsSerializable))
            {
                var value = prop.GetValue(this);
                if (value is double)
                    value = ((double)value).ToString("0.########");
                if (value is float)
                    value = ((float)value).ToString("0.########");
                result += value + " ";
            }
#endif  // if !UNITY_METRO
            return result;
        }

        /// <summary>   Gets the fields in this collection. </summary>
        ///
     
        ///
        /// <param name="type"> The type. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process the fields in this collection.
        /// </returns>

        IEnumerable<FieldInfo> GetFields(Type type)
        {
#if !UNITY_METRO
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
#else
            return null;
#endif
        }

        /// <summary>   Creates a copy. </summary>
        ///
     
        ///
        /// <param name="oldGameObject">    The old game object. </param>
        /// <param name="newGameObject">    The new game object. </param>
        ///
        /// <returns>   The new copy. </returns>

        public ActionBase CreateCopy(GameObject oldGameObject, GameObject newGameObject)
        {
#if !UNITY_METRO
            var newObj = CreateInstance(GetType()) as ActionBase;
#else
            var newObj = (ActionBase) this.MemberwiseClone();
#endif
            var fields = GetFields(GetType());
            foreach (var field in fields)
            {
                var value = field.GetValue(this);
                if (value is GameObject)
                {
                    if (value as GameObject == oldGameObject)
                        value = newGameObject;
                }
                field.SetValue(newObj, value);
            }
            return newObj;
        }

        /// <summary>   Fails the given assertion. </summary>
        ///
     
        ///
        /// <param name="assertion">    The assertion. </param>

        public virtual void Fail(AssertionComponent assertion)
        {
            Debug.LogException(new AssertionException(assertion), assertion.GetFailureReferenceObject());
        }

        /// <summary>   Gets failure message. </summary>
        ///
     
        ///
        /// <returns>   The failure message. </returns>

        public virtual string GetFailureMessage()
        {
            return GetType().Name + " assertion failed.\n(" + go + ")." + thisPropertyPath + " failed. Value: " + m_ObjVal;
        }
    }

    /// <summary>   An action base generic. </summary>
    ///
 
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>

    public abstract class ActionBaseGeneric<T> : ActionBase
    {
        /// <summary>   Compares this T object to another to determine their relative ordering. </summary>
        ///
     
        ///
        /// <param name="objVal">   The t to compare to this object. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(object objVal)
        {
            return Compare((T)objVal);
        }

        /// <summary>
        /// Compares this T object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="objVal">   The t to compare to this object. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected abstract bool Compare(T objVal);

        /// <summary>   Gets accepatble types for a. </summary>
        ///
     
        ///
        /// <returns>   An array of type. </returns>

        public override Type[] GetAccepatbleTypesForA()
        {
            return new[] { typeof(T) };
        }

        /// <summary>   Gets parameter type. </summary>
        ///
     
        ///
        /// <returns>   The parameter type. </returns>

        public override Type GetParameterType()
        {
            return typeof(T);
        }

        /// <summary>   Gets a value indicating whether this object use cache. </summary>
        ///
        /// <value> True if use cache, false if not. </value>

        protected override bool UseCache { get { return true; } }
    }
}
