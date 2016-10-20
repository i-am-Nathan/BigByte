// file:	Assets\UnityTestTools\Assertions\Editor\PropertyResolver.cs
//
// summary:	Implements the property resolver class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   (Serializable) a property resolver. </summary>
    ///
 

    [Serializable]
    public class PropertyResolver
    {
        /// <summary>   Gets or sets a list of names of the excluded fields. </summary>
        ///
        /// <value> A list of names of the excluded fields. </value>

        public string[] ExcludedFieldNames { get; set; }

        /// <summary>   Gets or sets a list of types of the excluded. </summary>
        ///
        /// <value> A list of types of the excluded. </value>

        public Type[] ExcludedTypes { get; set; }

        /// <summary>   Gets or sets a list of types of the allowed. </summary>
        ///
        /// <value> A list of types of the allowed. </value>

        public Type[] AllowedTypes { get; set; }

        /// <summary>   Default constructor. </summary>
        ///
     

        public PropertyResolver()
        {
            ExcludedFieldNames = new string[] { };
            ExcludedTypes = new Type[] { };
            AllowedTypes = new Type[] { };
        }

        /// <summary>   Gets fields and properties under path. </summary>
        ///
     
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="go">           The go. </param>
        /// <param name="propertPath">  Full pathname of the propert file. </param>
        ///
        /// <returns>   The fields and properties under path. </returns>

        public IList<string> GetFieldsAndPropertiesUnderPath(GameObject go, string propertPath)
        {
            propertPath = propertPath.Trim();
            if (!PropertyPathIsValid(propertPath))
            {
                throw new ArgumentException("Incorrect property path: " + propertPath);
            }

            var idx = propertPath.LastIndexOf('.');

            if (idx < 0)
            {
                var components = GetFieldsAndPropertiesFromGameObject(go, 2, null);
                return components;
            }

            var propertyToSearch = propertPath;
            Type type;
            if (MemberResolver.TryGetMemberType(go, propertyToSearch, out type))
            {
                idx = propertPath.Length - 1;
            }
            else
            {
                propertyToSearch = propertPath.Substring(0, idx);
                if (!MemberResolver.TryGetMemberType(go, propertyToSearch, out type))
                {
                    var components = GetFieldsAndPropertiesFromGameObject(go, 2, null);
                    return components.Where(s => s.StartsWith(propertPath.Substring(idx + 1))).ToArray();
                }
            }

            var resultList = new List<string>();
            var path = "";
            if (propertyToSearch.EndsWith("."))
                propertyToSearch = propertyToSearch.Substring(0, propertyToSearch.Length - 1);
            foreach (var c in propertyToSearch)
            {
                if (c == '.')
                    resultList.Add(path);
                path += c;
            }
            resultList.Add(path);
            foreach (var prop in type.GetProperties().Where(info => info.GetIndexParameters().Length == 0))
            {
                if (prop.Name.StartsWith(propertPath.Substring(idx + 1)))
                    resultList.Add(propertyToSearch + "." + prop.Name);
            }
            foreach (var prop in type.GetFields())
            {
                if (prop.Name.StartsWith(propertPath.Substring(idx + 1)))
                    resultList.Add(propertyToSearch + "." + prop.Name);
            }
            return resultList.ToArray();
        }

        /// <summary>   Property path is valid. </summary>
        ///
     
        ///
        /// <param name="propertPath">  Full pathname of the propert file. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        internal bool PropertyPathIsValid(string propertPath)
        {
            if (propertPath.StartsWith("."))
                return false;
            if (propertPath.IndexOf("..") >= 0)
                return false;
            if (Regex.IsMatch(propertPath, @"\s"))
                return false;
            return true;
        }

        /// <summary>   Gets fields and properties from game object. </summary>
        ///
     
        ///
        /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside
        ///                                                 the required range. </exception>
        ///
        /// <param name="gameObject">       The game object. </param>
        /// <param name="depthOfSearch">    Depth of the search. </param>
        /// <param name="extendPath">       Full pathname of the extend file. </param>
        ///
        /// <returns>   The fields and properties from game object. </returns>

        public IList<string> GetFieldsAndPropertiesFromGameObject(GameObject gameObject, int depthOfSearch, string extendPath)
        {
            if (depthOfSearch < 1) throw new ArgumentOutOfRangeException("depthOfSearch has to be greater than 0");

            var goVals = GetPropertiesAndFieldsFromType(typeof(GameObject),
                                                        depthOfSearch - 1).Select(s => "gameObject." + s);

            var result = new List<string>();
            if (AllowedTypes == null || !AllowedTypes.Any() || AllowedTypes.Contains(typeof(GameObject)))
                result.Add("gameObject");
            result.AddRange(goVals);

            foreach (var componentType in GetAllComponents(gameObject))
            {
                if (AllowedTypes == null || !AllowedTypes.Any() || AllowedTypes.Any(t => t.IsAssignableFrom(componentType)))
                    result.Add(componentType.Name);

                if (depthOfSearch > 1)
                {
                    var vals = GetPropertiesAndFieldsFromType(componentType, depthOfSearch - 1);
                    var valsFullName = vals.Select(s => componentType.Name + "." + s);
                    result.AddRange(valsFullName);
                }
            }

            if (!string.IsNullOrEmpty(extendPath))
            {
                var memberResolver = new MemberResolver(gameObject, extendPath);
                var pathType = memberResolver.GetMemberType();
                var vals = GetPropertiesAndFieldsFromType(pathType, depthOfSearch - 1);
                var valsFullName = vals.Select(s => extendPath + "." + s);
                result.AddRange(valsFullName);
            }

            return result;
        }

        /// <summary>   Gets properties and fields from type. </summary>
        ///
     
        ///
        /// <param name="type">     The type. </param>
        /// <param name="level">    The level. </param>
        ///
        /// <returns>   An array of string. </returns>

        private string[] GetPropertiesAndFieldsFromType(Type type, int level)
        {
            level--;

            var result = new List<string>();
            var fields = new List<MemberInfo>();
            fields.AddRange(type.GetFields().Where(f => !Attribute.IsDefined(f, typeof(ObsoleteAttribute))).ToArray());
            fields.AddRange(type.GetProperties().Where(info => info.GetIndexParameters().Length == 0 && !Attribute.IsDefined(info, typeof(ObsoleteAttribute))).ToArray());

            foreach (var member in fields)
            {
                var memberType = GetMemberFieldType(member);
                var memberTypeName = memberType.Name;

                if (AllowedTypes == null
                    || !AllowedTypes.Any()
                    || (AllowedTypes.Any(t => t.IsAssignableFrom(memberType)) && !ExcludedFieldNames.Contains(memberTypeName)))
                {
                    result.Add(member.Name);
                }

                if (level > 0 && IsTypeOrNameNotExcluded(memberType, memberTypeName))
                {
                    var vals = GetPropertiesAndFieldsFromType(memberType, level);
                    var valsFullName = vals.Select(s => member.Name + "." + s);
                    result.AddRange(valsFullName);
                }
            }
            return result.ToArray();
        }

        /// <summary>   Gets member field type. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="info"> The information. </param>
        ///
        /// <returns>   The member field type. </returns>

        private Type GetMemberFieldType(MemberInfo info)
        {
            if (info.MemberType == MemberTypes.Property)
                return (info as PropertyInfo).PropertyType;
            if (info.MemberType == MemberTypes.Field)
                return (info as FieldInfo).FieldType;
            throw new Exception("Only properties and fields are allowed");
        }

        /// <summary>   Gets all components. </summary>
        ///
     
        ///
        /// <param name="gameObject">   The game object. </param>
        ///
        /// <returns>   An array of type. </returns>

        internal Type[] GetAllComponents(GameObject gameObject)
        {
            var result = new List<Type>();
            var components = gameObject.GetComponents(typeof(Component));
            foreach (var component in components)
            {
                var componentType = component.GetType();
                if (IsTypeOrNameNotExcluded(componentType, null))
                {
                    result.Add(componentType);
                }
            }
            return result.ToArray();
        }

        /// <summary>   Query if 'memberType' is type or name not excluded. </summary>
        ///
     
        ///
        /// <param name="memberType">       Type of the member. </param>
        /// <param name="memberTypeName">   Name of the member type. </param>
        ///
        /// <returns>   True if type or name not excluded, false if not. </returns>

        private bool IsTypeOrNameNotExcluded(Type memberType, string memberTypeName)
        {
            return !ExcludedTypes.Contains(memberType) && !ExcludedFieldNames.Contains(memberTypeName);
        }
    }
}
