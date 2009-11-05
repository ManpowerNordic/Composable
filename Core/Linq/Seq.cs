﻿using System.Collections.Generic;
using System.Linq;

namespace Void.Linq
{
    public static class Seq
    {
        public static IEnumerable<T> Create<T>(params T[] values)
        {
            return values;
        }

        public static IEnumerable<int> From(int start)
        {
            while(start < int.MaxValue)
            {
                yield return start++;
            }
        }

        public static IEnumerable<int> By(this int me, int stepsize)
        {
            while(me < int.MaxValue)
            {
                yield return me;
                me += stepsize;
            }
        }

        public static IEnumerable<int> To(this int me, int guard)
        {
            return From(me).Take(guard - me + 1);
        }
    }
}