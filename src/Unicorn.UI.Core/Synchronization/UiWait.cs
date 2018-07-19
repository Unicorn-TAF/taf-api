﻿using System;
using System.Globalization;
using System.Threading;
using Unicorn.Core.Logging;
using Unicorn.Core.Utility.Synchronization;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    public class UiWait<T> : DefaultWait<T> where T : IControl
    {
        private string attribute;
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="clock">The clock to use when measuring the timeout.</param>
        public UiWait(T input) : base(input) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="clock">The clock to use when measuring the timeout.</param>
        public UiWait(T input, string attribute, string value) : base(input)
        {
            this.attribute = attribute;
            this.value = value;
        }

        /// <summary>
        /// Repeatedly applies this instance's input value to the given function until one of the following
        /// occurs:
        /// <para>
        /// <list type="bullet">
        /// <item>the function returns neither null nor false</item>
        /// <item>the function throws an exception that is not in the list of ignored exception types</item>
        /// <item>the timeout expires</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
        /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
        /// <returns>The delegate's return value.</returns>
        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException("Can only wait on an object or boolean response, tried to use type: " + resultType.ToString(), "condition");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Waiting for {input} {condition.Method.Name} during {this.Timeout} with polling interval {this.PollingInterval}");

            Exception lastException = null;
            var endTime = this.clock.LaterBy(this.Timeout);
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    var result = condition(this.input);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"\twait is successful [Wait time = {endTime - startTime}]");
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"\twait is successful [Wait time = {endTime - startTime}]");
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!this.IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // Check the timeout after evaluating the function to ensure conditions
                // with a zero timeout can succeed.
                if (!this.clock.IsNowBefore(endTime))
                {
                    var timeoutMessage = string.IsNullOrEmpty(this.Message) ?
                        string.Format("{0} expired after {1} seconds", condition, Timeout.TotalSeconds) :
                        string.Format(CultureInfo.InvariantCulture, "Timed out after {0} seconds: {1}", this.Timeout.TotalSeconds, this.Message);

                    throw new TimeoutException(timeoutMessage, lastException);
                }

                Thread.Sleep(this.PollingInterval);
            }
        }

        /// <summary>
        /// Repeatedly applies this instance's input value to the given function until one of the following
        /// occurs:
        /// <para>
        /// <list type="bullet">
        /// <item>the function returns neither null nor false</item>
        /// <item>the function throws an exception that is not in the list of ignored exception types</item>
        /// <item>the timeout expires</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
        /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
        /// <returns>The delegate's return value.</returns>
        public TResult UntilAttribute<TResult>(Func<T, string, string, TResult> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException("Can only wait on an object or boolean response, tried to use type: " + resultType.ToString(), "condition");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Waiting for {input} '{this.attribute}' {condition.Method.Name} '{this.value}' during {this.Timeout} with polling interval {this.PollingInterval}");

            Exception lastException = null;
            var endTime = this.clock.LaterBy(this.Timeout);
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    var result = condition(this.input, this.attribute, this.value);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"\twait is successful [Wait time = {endTime - startTime}]");
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"\twait is successful [Wait time = {endTime - startTime}]");
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!this.IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // Check the timeout after evaluating the function to ensure conditions
                // with a zero timeout can succeed.
                if (!this.clock.IsNowBefore(endTime))
                {
                    var timeoutMessage = string.IsNullOrEmpty(this.Message) ?
                        string.Format("{0} expired after {1} seconds", condition, Timeout.TotalSeconds) :
                        string.Format(CultureInfo.InvariantCulture, "Timed out after {0} seconds: {1}", this.Timeout.TotalSeconds, this.Message);

                    throw new TimeoutException(timeoutMessage, lastException);
                }

                Thread.Sleep(this.PollingInterval);
            }
        }
    }
}
