﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace FunctionalWay.Options
{
    public class Some<T>
    {
        public T Value { get; }
        public Some(T value)
        {
            Value = value;
        }
    }

    public class None {}
    public class Option<T> : IEquatable<Option<T>>
    {

        private readonly T _value;
        private readonly bool _isSome;
        private bool _isNone => !_isSome;

        private Option(T value)
        {
            _value = value;
            _isSome = true;
        }

        public bool IsSome => _isSome;
        public bool IsNone => _isNone;
        
        public Option()
        {
            _isSome = false;
        }
        
        public R Match<R>(Func<R> None, Func<T, R> Some)
        {
            return _isSome ? Some(_value) : None();
        }

        public Unit Match(Action None, Action<T> Some) 
            => Match(None.ToFunc(), Some.ToFunc());

        public async Task<R> Match<R>(Func<R> None, Func<T, Task<R>> Some)
        {
            if (_isSome) 
                return await Some(_value);
            
            return None();
        }

        public async Task MatchAsync(Action None, Func<T, Task> Some)
        {
            if (_isSome)
            {
                await Some(_value);
                return;
            }

            None();
        } 
        
        public Option<R> Map<R>(Func<T, R> func)
        {
            return _isSome ? F.Some(func(_value)) : F.None;
        }

        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value); 
        public static implicit operator Option<T>(None _) => new Option<T>();
        
        public bool Equals(Option<T> other)
        {
            return _isSome == other._isSome &&
                   (_isNone || _value.Equals(other._value));
        }

        public bool Equals(None none) => _isNone;
        
        public override bool Equals(object obj)
        {
            return Equals((Option<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(_value) * 397) ^ _isSome.GetHashCode();
            }
        }
        

        public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
        public static bool operator !=(Option<T> @this, Option<T> other) => !@this.Equals(other);

        public override string ToString() => _isSome ? $"Some({_value})" : "None";

    }


}