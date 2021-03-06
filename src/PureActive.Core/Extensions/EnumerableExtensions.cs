﻿// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="EnumerableExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;

namespace PureActive.Core.Extensions
{
    /// <summary>
    /// Class EnumerableExtensions.
    /// </summary>
    /// <autogeneratedoc />
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Maximums the length of the string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        public static int MaxStringLength<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.Select(item => item.ToString().Length).Concat(new[] {0}).Max() ?? 0;
        }
    }
}