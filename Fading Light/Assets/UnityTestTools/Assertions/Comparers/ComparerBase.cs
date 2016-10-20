// file:	Assets\UnityTestTools\Assertions\Comparers\ComparerBase.cs
//
// summary:	Implements the comparer base class

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace UnityTest
{
    /// <summary>   A comparer base. </summary>
    ///
 

    public abstract class ComparerBase : ActionBase
    {
        /// <summary>   Values that represent compare to types. </summary>
        ///
     

        public enum CompareToType
        {
            /// <summary>   An enum constant representing the compare to object option. </summary>
            CompareToObject,
            /// <summary>   An enum constant representing the compare to constant value option. </summary>
            CompareToConstantValue,
            /// <summary>   An enum constant representing the compare to null option. </summary>
            CompareToNull
        }

        /// <summary>   Type of the compare to. </summary>
        public CompareToType compareToType = CompareToType.CompareToObject;

        /// <summary>   The other. </summary>
        public GameObject other;
        /// <summary>   The object other value. </summary>
        protected object m_ObjOtherVal;
        /// <summary>   Full pathname of the other property file. </summary>
        public string otherPropertyPath = "";
        /// <summary>   The member resolver b. </summary>
        private MemberResolver m_MemberResolverB;

        /// <summary>
        /// Compares this object object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="a">    Object to be compared. </param>
        /// <param name="b">    Object to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected abstract bool Compare(object a, object b);

        /// <summary>
        /// Compares this object object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="objValue"> The object to compare to this object. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(object objValue)
        {
            if (compareToType == CompareToType.CompareToConstantValue)
            {
                m_ObjOtherVal = ConstValue;
            }
            else if (compareToType == CompareToType.CompareToNull)
            {
                m_ObjOtherVal = null;
            }
            else
            {
                if (other == null)
                    m_ObjOtherVal = null;
                else
                {
                    if (m_MemberResolverB == null)
                        m_MemberResolverB = new MemberResolver(other, otherPropertyPath);
                    m_ObjOtherVal = m_MemberResolverB.GetValue(UseCache);
                }
            }
            return Compare(objValue, m_ObjOtherVal);
        }

        /// <summary>   Gets accepatble types for b. </summary>
        ///
     
        ///
        /// <returns>   An array of type. </returns>

        public virtual Type[] GetAccepatbleTypesForB()
        {
            return null;
        }

        #region Const value

        /// <summary>   Gets or sets the constant value. </summary>
        ///
        /// <value> The constant value. </value>

        public virtual object ConstValue { get; set; }

        /// <summary>   Gets default constant value. </summary>
        ///
     
        ///
        /// <returns>   The default constant value. </returns>

        public virtual object GetDefaultConstValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>   Gets failure message. </summary>
        ///
     
        ///
        /// <returns>   The failure message. </returns>

        public override string GetFailureMessage()
        {
            var message = GetType().Name + " assertion failed.\n" + go.name + "." + thisPropertyPath + " " + compareToType;
            switch (compareToType)
            {
                case CompareToType.CompareToObject:
                    message += " (" + other + ")." + otherPropertyPath + " failed.";
                    break;
                case CompareToType.CompareToConstantValue:
                    message += " " + ConstValue + " failed.";
                    break;
                case CompareToType.CompareToNull:
                    message += " failed.";
                    break;
            }
            message += " Expected: " + m_ObjOtherVal + " Actual: " + m_ObjVal;
            return message;
        }
    }

    /// <summary>   (Serializable) a comparer base generic. </summary>
    ///
 
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>

    [Serializable]
    public abstract class ComparerBaseGeneric<T> : ComparerBaseGeneric<T, T>
    {
    }

    /// <summary>   (Serializable) a comparer base generic. </summary>
    ///
 
    ///
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>

    [Serializable]
    public abstract class ComparerBaseGeneric<T1, T2> : ComparerBase
    {
        /// <summary>   The constant value generic. </summary>
        public T2 constantValueGeneric = default(T2);

        /// <summary>   Gets or sets the constant value. </summary>
        ///
        /// <value> The constant value. </value>

        public override Object ConstValue
        {
            get
            {
                return constantValueGeneric;
            }
            set
            {
                constantValueGeneric = (T2)value;
            }
        }

        /// <summary>   Gets default constant value. </summary>
        ///
     
        ///
        /// <returns>   The default constant value. </returns>

        public override Object GetDefaultConstValue()
        {
            return default(T2);
        }

        /// <summary>   Query if 'type' is value type. </summary>
        ///
     
        ///
        /// <param name="type"> The type. </param>
        ///
        /// <returns>   True if value type, false if not. </returns>

        static bool IsValueType(Type type)
        {
#if !UNITY_METRO
            return type.IsValueType;
#else
            return false;
#endif
        }

        /// <summary>   Compares two T1 objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="a">    T 1 to be compared. </param>
        /// <param name="b">    T 2 to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(object a, object b)
        {
            var type = typeof(T2);
            if (b == null && IsValueType(type))
            {
                throw new ArgumentException("Null was passed to a value-type argument");
            }
            return Compare((T1)a, (T2)b);
        }

        /// <summary>   Compares two T1 objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <param name="a">    T 1 to be compared. </param>
        /// <param name="b">    T 2 to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected abstract bool Compare(T1 a, T2 b);

        /// <summary>   Gets accepatble types for a. </summary>
        ///
     
        ///
        /// <returns>   An array of type. </returns>

        public override Type[] GetAccepatbleTypesForA()
        {
            return new[] { typeof(T1) };
        }

        /// <summary>   Gets accepatble types for b. </summary>
        ///
     
        ///
        /// <returns>   An array of type. </returns>

        public override Type[] GetAccepatbleTypesForB()
        {
            return new[] {typeof(T2)};
        }

        /// <summary>   Gets a value indicating whether this object use cache. </summary>
        ///
        /// <value> True if use cache, false if not. </value>

        protected override bool UseCache { get { return true; } }
    }
}
