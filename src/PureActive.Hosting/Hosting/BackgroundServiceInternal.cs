﻿// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="BackgroundServiceInternal.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;

namespace PureActive.Hosting.Hosting
{
    /// <summary>
    /// Class BackgroundServiceInternal.
    /// Implements the <see cref="Hosting.HostedServiceInternal{T}" />
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Hosting.HostedServiceInternal{T}" />
    /// <seealso cref="IDisposable" />
    /// <autogeneratedoc />
    public abstract class BackgroundServiceInternal<T> : HostedServiceInternal<T>, IDisposable
    {
        /// <summary>
        /// The stopping CTS
        /// </summary>
        /// <autogeneratedoc />
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        /// <summary>
        /// The executing task
        /// </summary>
        /// <autogeneratedoc />
        private Task _executingTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundServiceInternal{T}"/> class.
        /// </summary>
        /// <param name="commonServices">The common services.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        /// <param name="serviceHost">The service host.</param>
        /// <autogeneratedoc />
        protected BackgroundServiceInternal(ICommonServices commonServices, IApplicationLifetime applicationLifetime,
            ServiceHost serviceHost) :
            base(commonServices, applicationLifetime, serviceHost)
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <autogeneratedoc />
        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The
        /// implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when
        /// <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is
        /// called.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>Task.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (ServiceHostStatus != ServiceHostStatus.Stopped)
            {
                Logger?.LogDebug("{ServiceHost}:Started Called with {ServiceHostStatus}", ServiceHost,
                    ServiceHostStatus);
            }

            ServiceHostStatus = ServiceHostStatus.StartPending;
            _executingTask = ExecuteAsync(_stoppingCts.Token);
            ServiceHostStatus = ServiceHostStatus.Running;

            _executingTask.ContinueWith(t =>
            {
                if (_executingTask.IsCompleted)
                {
                    ServiceHostStatus = ServiceHostStatus.Stopped;
                }
            }, cancellationToken);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>Task.</returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StopPending;

            if (_executingTask == null)
            {
                ServiceHostStatus = ServiceHostStatus.Stopped;

                return;
            }
      
            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
                ServiceHostStatus = ServiceHostStatus.Stopped;
            }
        }
    }
}