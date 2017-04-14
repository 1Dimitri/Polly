﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly
{
    public partial class Policy<TResult>
    {
        private readonly Func<Func<CancellationToken, Task<TResult>>, Context, CancellationToken, bool, Task<TResult>> _asyncExecutionPolicy;

        internal Policy(
            Func<Func<CancellationToken, Task<TResult>>, Context, CancellationToken, bool, Task<TResult>> asyncExecutionPolicy,
            IEnumerable<ExceptionPredicate> exceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> resultPredicates)
        {
            if (asyncExecutionPolicy == null) throw new ArgumentNullException("asyncExecutionPolicy");

            _asyncExecutionPolicy = asyncExecutionPolicy;
            _exceptionPredicates = exceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            _resultPredicates = resultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync(Func<Task<TResult>> action)
        {
            return ExecuteAsync(ct => action(), new Context(), CancellationToken.None, false);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync(Func<Task<TResult>> action, bool continueOnCapturedContext)
        {
            return ExecuteAsync(ct => action(), new Context(), CancellationToken.None, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken)
        {
            return ExecuteAsync(action, new Context(), cancellationToken, false);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.InvalidOperationException">Please use asynchronous-defined policies when calling asynchronous ExecuteAsync (and similar) methods.</exception>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsync(action, new Context(), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Task<TResult>> action)
        {
            return ExecuteAndCaptureAsync(ct => action(), new Context(), CancellationToken.None, false);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<Task<TResult>> action, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(ct => action(), new Context(), CancellationToken.None, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(), cancellationToken, false);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.InvalidOperationException">Please use asynchronous-defined policies when calling asynchronous ExecuteAsync (and similar) methods.</exception>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(), cancellationToken, continueOnCapturedContext);
        }
    }

}
