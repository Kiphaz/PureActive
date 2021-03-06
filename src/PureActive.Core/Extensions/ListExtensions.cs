﻿// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 10-31-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ListExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace PureActive.Core.Extensions
{
    /// <summary>
    /// Class ListExtensions.
    /// </summary>
    /// <autogeneratedoc />
    public static class ListExtensions
    {
        /// <summary>
        /// The RNG
        /// </summary>
        /// <autogeneratedoc />
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Shuffles the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <autogeneratedoc />
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                return;

            var n = list.Count;

            while (n > 1)
            {
                n--;

                var k = Rng.Next(n + 1);

                // Swap values
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        /// <autogeneratedoc />
        public static IList<T> AddItem<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns>ICollection&lt;T&gt;.</returns>
        /// <autogeneratedoc />
        public static ICollection<T> AddItem<T>(this ICollection<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Clones the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        /// <autogeneratedoc />
        public static IList<T> CloneList<T>(this IEnumerable<T> list) where T : ICloneable
        {
            return list.Select(item => (T) item.Clone()).ToList();
        }
    }
}