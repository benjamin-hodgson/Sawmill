using System;

namespace Sawmill
{
    /// <summary>
    /// A <see cref="System.Action{T}"/> whose first argument is a <see cref="System.Span{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    public delegate void SpanAction<T>(Span<T> span);

    /// <summary>
    /// A <see cref="System.Action{T, U}"/> whose first argument is a <see cref="System.Span{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <param name="arg">An additional argument</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    /// <typeparam name="U">The type of the additional argument</typeparam>
    public delegate void SpanAction<T, in U>(Span<T> span, U arg);
    

    /// <summary>
    /// A <see cref="System.Func{T, R}"/> whose first argument is a <see cref="System.Span{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    /// <typeparam name="R">The return type</typeparam>
    public delegate R SpanFunc<T, out R>(Span<T> span);

    /// <summary>
    /// A <see cref="System.Func{T, U, R}"/> whose first argument is a <see cref="System.Span{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <param name="arg">An additional argument</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    /// <typeparam name="R">The return type</typeparam>
    /// <typeparam name="U">The type of the additional argument</typeparam>
    public delegate R SpanFunc<T, in U, out R>(Span<T> span, U arg);


    /// <summary>
    /// A <see cref="System.Action{T}"/> whose first argument is a <see cref="System.ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    public delegate void ReadOnlySpanAction<T>(ReadOnlySpan<T> span);
    
    /// <summary>
    /// A <see cref="System.Action{T, U}"/> whose first argument is a <see cref="System.ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <param name="arg">An additional argument</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    /// <typeparam name="U">The type of the additional argument</typeparam>
    public delegate void ReadOnlySpanAction<T, in U>(ReadOnlySpan<T> span, U arg);
    

    /// <summary>
    /// A <see cref="System.Func{T, R}"/> whose first argument is a <see cref="System.ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    /// <typeparam name="R">The return type</typeparam>
    public delegate R ReadOnlySpanFunc<T, out R>(ReadOnlySpan<T> span);
    
    /// <summary>
    /// A <see cref="System.Func{T, U, R}"/> whose first argument is a <see cref="System.ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span</param>
    /// <param name="arg">An additional argument</param>
    /// <typeparam name="T">The type of elements in the span</typeparam>
    /// <typeparam name="R">The return type</typeparam>
    /// <typeparam name="U">The type of the additional argument</typeparam>
    public delegate R ReadOnlySpanFunc<T, in U, out R>(ReadOnlySpan<T> span, U arg);
}
