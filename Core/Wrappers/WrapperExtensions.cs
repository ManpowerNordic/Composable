using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Void.Wrappers
{
    ///<summary/>
    public static class WrapperExtensions
    {
        /// <summary>
        /// Given a sequence of <see cref="IWrapper{T}"/> returns a sequence containing the wrapped T values.
        /// </summary>
        public static IEnumerable<T> Unwrap<T>(this IEnumerable<IWrapper<T>> wrapper)
        {
            Contract.Requires(wrapper != null);
            return wrapper.Select(wrapping => wrapping.Wrapped);
        }
    }
}