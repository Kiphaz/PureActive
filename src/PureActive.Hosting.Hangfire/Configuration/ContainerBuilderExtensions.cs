﻿// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-05-2018
// ***********************************************************************
// <copyright file="ContainerBuilderExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Autofac;
using PureActive.Core.Abstractions.Queue;
using PureActive.Queue.Hangfire.Queue;

namespace PureActive.Hosting.Hangfire.Configuration
{
    /// <summary>
    /// Extension methods for building the IOC container on application start.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers the operation runner.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterJobQueueClient(this ContainerBuilder builder)
        {
            builder.RegisterType<JobQueueClient>().As<IJobQueueClient>().InstancePerLifetimeScope();
        }

        ///// <summary>
        ///// Registers the json serialization.
        ///// </summary>
        ///// <param name="builder">The builder.</param>
        ///// <param name="typeMaps">The type maps.</param>
        ///// <autogeneratedoc />
        //public static void RegisterJsonSerialization(this ContainerBuilder builder, ITypeMapCollection typeMaps)
        //{
        //    builder.RegisterType<JsonSettingsProvider>().As<IJsonSettingsProvider>().InstancePerLifetimeScope();
        //    builder.RegisterType<ModelSerializer>().As<IJsonSerializer>().InstancePerLifetimeScope();
        //    builder.RegisterInstance(typeMaps).As<ITypeMapCollection>();
        //}
    }
}