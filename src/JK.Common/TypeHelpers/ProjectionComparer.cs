﻿using System;
using System.Collections.Generic;

namespace JK.Common.TypeHelpers
{
    // found at: https://stackoverflow.com/a/21021391
    public static class ProjectionComparer<TSource>
    {
        public static IEqualityComparer<TSource> CompareBy<TValue>(
            Func<TSource, TValue> selector)
        {
            return CompareBy(selector, EqualityComparer<TValue>.Default);
        }
        
        public static IEqualityComparer<TSource> CompareBy<TValue>(Func<TSource, TValue> selector, IEqualityComparer<TValue> comparer)
        {
            return new ComparerImpl<TValue>(selector, comparer);
        }

        private sealed class ComparerImpl<TValue> : IEqualityComparer<TSource>
        {
            private readonly Func<TSource, TValue> _selector;
            private readonly IEqualityComparer<TValue> _comparer;

            public ComparerImpl(Func<TSource, TValue> selector, IEqualityComparer<TValue> comparer)
            {
                if (selector is null) throw new ArgumentNullException("selector");
                if (comparer is null) throw new ArgumentNullException("comparer");
                _selector = selector;
                _comparer = comparer;
            }

            bool IEqualityComparer<TSource>.Equals(TSource x, TSource y)
            {
                if (x.Equals(default(TSource)) && y.Equals(default(TSource))) return true;
                if (x.Equals(default(TSource)) || y.Equals(default(TSource))) return false;
                return _comparer.Equals(_selector(x), _selector(y));
            }

            int IEqualityComparer<TSource>.GetHashCode(TSource obj)
            {
                return obj.Equals(default(TSource)) ? 0 : _comparer.GetHashCode(_selector(obj));
            }
        }
    }
}
