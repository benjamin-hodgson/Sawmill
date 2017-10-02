using System;
using System.Collections.Generic;

namespace Sawmill
{
    /// <summary>
    /// Utility methods for <see cref="IEnumerator{T}"/>.
    /// </summary>
    public static class EnumeratorExtensions
    {
        /// <summary>
        /// Advances <paramref name="enumerator"/> by one and return the new current value.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next value of <paramref name="enumerator"/></returns>
        public static T Draw1<T>(this IEnumerator<T> enumerator)
        {
            var hasNext = enumerator.MoveNext();
            if (!hasNext)
            {
                throw new InvalidOperationException("Reached end of enumeration");
            }
            return enumerator.Current;
        }

        /// <summary>
        /// Advances <paramref name="enumerator"/> by an arbitrary number of items and return the new current value.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <param name="count">How many items to draw from <paramref name="enumerator"/></param>
        /// <returns>An array containing the next <paramref name="count"/> items of <paramref name="enumerator"/></returns>
        public static T[] Draw<T>(this IEnumerator<T> enumerator, int count)
        {
            var result = new T[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = enumerator.Draw1();
            }
            return result;
        }

        /// <summary>
        /// Advances <paramref name="enumerator"/> by two and return the two values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next two values of <paramref name="enumerator"/></returns>
        public static (T, T) Draw2<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1()
            );

        /// <summary>
        /// Advances <paramref name="enumerator"/> by three and return the three values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next three values of <paramref name="enumerator"/></returns>
        public static (T, T, T) Draw3<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1());

        /// <summary>
        /// Advances <paramref name="enumerator"/> by four and return the four values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next four values of <paramref name="enumerator"/></returns>
        public static (T, T, T, T) Draw4<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1()
            );

        /// <summary>
        /// Advances <paramref name="enumerator"/> by five and return the five values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next five values of <paramref name="enumerator"/></returns>
        public static (T, T, T, T, T) Draw5<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1()
            );

        /// <summary>
        /// Advances <paramref name="enumerator"/> by six and return the six values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next six values of <paramref name="enumerator"/></returns>
        public static (T, T, T, T, T, T) Draw6<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1()
            );

        /// <summary>
        /// Advances <paramref name="enumerator"/> by seven and return the seven values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next seven values of <paramref name="enumerator"/></returns>
        public static (T, T, T, T, T, T, T) Draw7<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1()
            );

        /// <summary>
        /// Advances <paramref name="enumerator"/> by eight and return the eight values.
        /// Throws an exception if there are not enough items left in the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException"><paramref name="enumerator"/> ran out of items</exception>
        /// <param name="enumerator">The enumerator</param>
        /// <returns>The next eight values of <paramref name="enumerator"/></returns>
        public static (T, T, T, T, T, T, T, T) Draw8<T>(this IEnumerator<T> enumerator)
            => (
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1(),
                enumerator.Draw1()
            );
    }
}