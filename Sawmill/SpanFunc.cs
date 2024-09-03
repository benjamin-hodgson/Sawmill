using System;

namespace Sawmill;

/// <summary>
/// A <see cref="Func{T, U, R}" /> whose first argument is a <see cref="Span{T}" />.
/// </summary>
/// <param name="span">The span.</param>
/// <param name="arg">An additional argument.</param>
/// <typeparam name="T">The type of elements in the span.</typeparam>
/// <typeparam name="U">The type of the additional argument.</typeparam>
/// <typeparam name="R">The return type.</typeparam>
/// <returns>A value of type <typeparamref name="R" />.</returns>
public delegate R SpanFunc<T, in U, out R>(Span<T> span, U arg);

internal delegate R SpanFunc<T, out R>(Span<T> span);

internal delegate R ReadOnlySpanFunc<T, in U, out R>(ReadOnlySpan<T> span, U arg);

internal delegate void SpanAction<T, in U>(Span<T> span, U arg);
