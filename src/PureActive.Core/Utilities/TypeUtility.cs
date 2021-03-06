// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 11-01-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="TypeUtility.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace PureActive.Core.Utilities
{
    /// <summary>
    /// Class TypeUtility.
    /// </summary>
    /// <autogeneratedoc />
    public static class TypeUtility
    {
        /// <summary>
        /// High-level method that enumerates all Types in the loaded assembly,
        /// and places their class names and properties in a hashtable.  At deserialization
        /// time, this Hashtable (containing Property Names and Types).
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>Hashtable.</returns>
        public static Hashtable GetProperties(IEnumerable<Type> types)
        {
            Hashtable properties = new Hashtable();

            foreach (var type in types)
            {
                if (type == null)
                    continue;

                Hashtable entry = GetProperties(type);

                if (entry != null && !string.IsNullOrEmpty(type.FullName))
                {
                    properties.Add(type.FullName, entry);
                }
            }

            return properties;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Hashtable.</returns>
        /// <autogeneratedoc />
        public static Hashtable GetProperties(Type type)
        {
            Hashtable properties = new Hashtable();

            // Type dump
            //Debug.Print("Name: " + t.Name);
            //Debug.Print("    IsClass: " + t.IsClass);
            //Debug.Print("    IsArray: " + t.IsArray);
            //Debug.Print("    IsEnum: " + t.IsEnum);
            //Debug.Print("    IsAbstract: " + t.IsAbstract);
            //Debug.Print("");

            // If it's a class, then it's something we care about
            if (type.IsClass)
            {
                MethodInfo[] methods = type.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    //Debug.Print("            Name: " + method.Name);
                    //Debug.Print("            IsVirtual: " + method.IsVirtual);
                    //Debug.Print("            IsStatic: " + method.IsStatic);
                    //Debug.Print("            IsPublic: " + method.IsPublic);
                    //Debug.Print("            IsFinal: " + method.IsFinal);
                    //Debug.Print("            IsAbstract: " + method.IsAbstract);
                    //Debug.Print("            MemberType: " + method.MemberType);
                    //Debug.Print("            DeclaringType: " + method.DeclaringType);
                    //Debug.Print("            ReturnType: " + method.ReturnType);

                    // If the Name.StartsWith "get_" and/or "set_",
                    // and it's not Abstract && not Virtual
                    // then it's a Property to save
                    if (method.Name.StartsWith("get_") &&
                        method.IsAbstract == false &&
                        method.IsVirtual == false)
                    {
                        // Ignore abstract and virtual objects
                        if (method.IsAbstract ||
                            method.IsVirtual ||
                            method.ReturnType.IsAbstract)
                        {
                            continue;
                        }

                        // Ignore delegates and MethodInfos
                        if (method.ReturnType == typeof(Delegate) ||
                            method.ReturnType == typeof(MulticastDelegate) ||
                            method.ReturnType == typeof(MethodInfo))
                        {
                            continue;
                        }

                        // Same for DeclaringType
                        if (method.DeclaringType == typeof(Delegate) ||
                            method.DeclaringType == typeof(MulticastDelegate))
                        {
                            continue;
                        }

                        // Don't need these types either
                        if (method.Name.StartsWith("System.Globalization"))
                        {
                            continue;
                        }

                        properties.Add(method.Name.Substring(4), method.ReturnType);
                    }
                }

                return properties;
            }

            return null;
        }
    }
}