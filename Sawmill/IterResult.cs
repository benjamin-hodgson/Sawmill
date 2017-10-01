using System;

namespace Sawmill
{
    /// <summary>
    /// Factory methods for <see cref="IterResult{T}"/>,
    /// for use with <see cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>.
    /// </summary>
    public static class IterResult
    {
        /// <summary>
        /// Creates an <see cref="IterResult{T}"/> representing the end of iteration.
        /// </summary>
        /// <typeparam name="T">The type of the rewritable object being passed between iterations.</typeparam>
        /// <returns>An <see cref="IterResult{T}"/> representing the end of iteration.</returns>
        public static IterResult<T> Done<T>()
            => new IterResult<T>();
        /// <summary>
        /// Creates an <see cref="IterResult{T}"/> representing iteration which is not yet complete.
        /// </summary>
        /// <typeparam name="T">The type of the rewritable object being passed between iterations.</typeparam>
        /// <param name="result">The value with which to continue iteration.</param>
        /// <returns>An <see cref="IterResult{T}"/> representing iteration which is not yet complete.</returns>
        public static IterResult<T> Continue<T>(T result)
            => new IterResult<T>(result);
    }

    /// <summary>
    /// An object representing the result of a single iteration of
    /// <see cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>.
    /// <para>
    /// Either iteration is not complete, in which case the <see cref="IterResult{T}"/>
    /// contains a new value with which to continue iteration,
    /// or iteration is complete, in which case the <see cref="IterResult{T}"/> contains nothing.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the rewritable object being passed between iterations.</typeparam>
    public struct IterResult<T>
    {
        /// <summary>
        /// True if iteration is not complete.
        /// </summary>
        /// <returns>True if iteration is not complete.</returns>
        public bool Continue { get; }
        private readonly T _result;
        /// <summary>
        /// Gets the result of the loop of iteration
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if iteration is complete (ie <see cref="Continue"/> is false and there is no result).
        /// </exception>
        /// <returns>The result of the loop of iteration</returns>
        public T Result
        {
            get
            {
                if (!Continue)
                {
                    throw new InvalidOperationException("No result");
                }
                return _result;
            }
        }

        /// <summary>
        /// Creates an <see cref="IterResult{T}"/> representing iteration which is not yet complete.
        /// </summary>
        /// <param name="result"></param>
        public IterResult(T result)
        {
            Continue = true;
            _result = result;
        }
    }
}