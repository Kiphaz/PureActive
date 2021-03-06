﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using PureActive.Core.Abstractions.Queue;

namespace PureActive.Queue.Hangfire.Queue
{
    /// <summary>
    ///     A client to manage jobs in a job queue.
    /// </summary>
    public class JobQueueClient : IJobQueueClient
    {
        /// <summary>
        ///     The background job client.
        /// </summary>
        private readonly IBackgroundJobClient _backgroundJobClient;

        /// <summary>
        ///     The monitoring API.
        /// </summary>
        private readonly IMonitoringApi _monitoringApi;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public JobQueueClient(
            IBackgroundJobClient backgroundJobClient,
            JobStorage jobStorage)
        {
            _backgroundJobClient = backgroundJobClient;
            _monitoringApi = jobStorage.GetMonitoringApi();
        }

        /// <summary>
        ///     Enqueues a job.
        /// </summary>
        /// <typeparam name="TInterface">An interface containing the method to run.</typeparam>
        /// <param name="job">
        ///     A call to a method on the given interface.
        ///     All parameters to the method must be serializable.
        /// </param>
        /// <returns>The job ID.</returns>
        public Task<string> EnqueueAsync<TInterface>(Expression<Func<TInterface, Task>> job)
        {
            // TODO: Make this actually asynchronous once HangFire supports it.

            var jobId = _backgroundJobClient.Enqueue(job);

            return Task.FromResult(jobId);
        }

        /// <summary>
        ///     Returns the status of the job with the given ID.
        /// </summary>
        public Task<JobStatus> GetJobStatusAsync(string jobId) => GetJobStatusAsync(_monitoringApi, jobId);

        /// <summary>
        ///     Returns the status of the job with the given ID.
        /// </summary>
        public static Task<JobStatus> GetJobStatusAsync(IMonitoringApi monitoringApi, string jobId)
        {
            // TODO: Make this actually asynchronous once HangFire supports it.

            var jobDetails = monitoringApi.JobDetails(jobId)
                .History
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefault();

            var jobState = GetJobState(jobDetails);
            var enteredState = jobDetails?.CreatedAt ?? DateTime.MinValue;

            return Task.FromResult(new JobStatus(jobState, enteredState));
        }

        /// <summary>
        /// Waits for the Job to Complete
        /// </summary>
        /// <param name="jobId">JobID</param>
        /// <param name="timeout">Time out in milliseconds</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<JobStatus> WaitForJobToComplete(string jobId, int timeout, CancellationToken cancellationToken) =>
            WaitForJobToComplete(_monitoringApi, jobId, timeout, cancellationToken);


        /// <summary>
        /// Waits for the Job to Complete
        /// </summary>
        /// <param name="monitoringApi"></param>
        /// <param name="jobId">JobID</param>
        /// <param name="timeout">Time out in milliseconds</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<JobStatus> WaitForJobToComplete(IMonitoringApi monitoringApi, string jobId, int timeout, CancellationToken cancellationToken)
        {
            var jobStatus = GetJobStatusAsync(monitoringApi, jobId).Result;

            // If job status is in a final state
            if (jobStatus.IsFinalState)
                return jobStatus;

            var secs = timeout / 1000;

            if (secs == 0)
                secs = 1;
            else if (timeout == -1)
                secs = int.MaxValue;

            // Poll for Completed Job
            while (!jobStatus.IsFinalState && secs-- > 0)
            {
                if (cancellationToken.IsCancellationRequested)
                    return jobStatus;

                // Wait 1 sec
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

                // Try again
                jobStatus = await GetJobStatusAsync(monitoringApi, jobId);
            }

            return jobStatus;
        }


   
        /// <summary>
        ///     Returns the job state for the given job.
        /// </summary>
        private static JobState GetJobState(StateHistoryDto jobDetails)
        {
            var stateName = jobDetails?.StateName;

            if (stateName == EnqueuedState.StateName)
                return JobState.NotStarted;

            if (stateName == AwaitingState.StateName)
                return JobState.Awaiting;

            if (stateName == SucceededState.StateName)
                return JobState.Succeeded;

            if (stateName == FailedState.StateName)
                return JobState.Failed;

            if (stateName == ScheduledState.StateName)
                return JobState.Scheduled;

            if (stateName == DeletedState.StateName)
                return JobState.Deleted;

            return stateName == ProcessingState.StateName ? JobState.InProgress : JobState.Unknown;
        }
    }
}