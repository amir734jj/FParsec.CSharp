﻿namespace FParsec.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.FSharp.Core;
    using static Primitives;

    public class Operators<TTerm, TAfterString> : IEnumerable<Operator<TTerm, TAfterString, Unit>> {
        private readonly HashSet<Operator<TTerm, TAfterString, Unit>> operators = new HashSet<Operator<TTerm, TAfterString, Unit>>();

        private static readonly FSharpFunc<CharStream<Unit>, Reply<TAfterString>> stop = preturn<TAfterString, Unit>(default);

        #region AddInfix()

        public Operators<TTerm, TAfterString> AddInfix(
            string operatorString,
            int precedence,
            Func<TTerm, TTerm, TTerm> map)
            => AddInfix(operatorString, precedence, Associativity.Left, stop, map);

        public Operators<TTerm, TAfterString> AddInfix(string operatorString,
            int precedence,
            Associativity associativity,
            Func<TTerm, TTerm, TTerm> map)
            => AddInfix(operatorString, precedence, associativity, stop, map);

        public Operators<TTerm, TAfterString> AddInfix(
            string operatorString,
            int precedence,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterStringParser,
            Func<TTerm, TTerm, TTerm> map)
            => AddInfix(operatorString, precedence, Associativity.Left, afterStringParser, map);

        public Operators<TTerm, TAfterString> AddInfix(
            string operatorString,
            int precedence,
            Associativity associativity,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterStringParser,
            Func<TTerm, TTerm, TTerm> map) {
            _ = operators.Add(new InfixOperator<TTerm, TAfterString, Unit>(
                operatorString,
                afterStringParser,
                precedence,
                associativity,
                map.ToFSharpFunc()));
            return this;
        }

        #endregion AddInfix()

        #region AddPrefix()

        public Operators<TTerm, TAfterString> AddPrefix(
            string operatorString,
            int precedence,
            Func<TTerm, TTerm> map)
            => AddPrefix(operatorString, precedence, false, stop, map);

        public Operators<TTerm, TAfterString> AddPrefix(
            string operatorString,
            int precedence,
            bool isAssociative,
            Func<TTerm, TTerm> map)
            => AddPrefix(operatorString, precedence, isAssociative, stop, map);

        public Operators<TTerm, TAfterString> AddPrefix(
            string operatorString,
            int precedence,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterStringParser,
            Func<TTerm, TTerm> map)
            => AddPrefix(operatorString, precedence, false, afterStringParser, map);

        public Operators<TTerm, TAfterString> AddPrefix(
            string operatorString,
            int precedence,
            bool isAssociative,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterStringParser,
            Func<TTerm, TTerm> map) {
            _ = operators.Add(new PrefixOperator<TTerm, TAfterString, Unit>(
                operatorString,
                afterStringParser,
                precedence,
                isAssociative,
                map.ToFSharpFunc()));
            return this;
        }

        #endregion AddPrefix()

        #region AddPostfix()

        public Operators<TTerm, TAfterString> AddPostfix(string operatorString, int precedence, Func<TTerm, TTerm> map)
            => AddPostfix(operatorString, precedence, false, stop, map);

        public Operators<TTerm, TAfterString> AddPostfix(string operatorString, int precedence, bool isAssociative, Func<TTerm, TTerm> map)
            => AddPostfix(operatorString, precedence, isAssociative, stop, map);

        public Operators<TTerm, TAfterString> AddPostfix(
            string operatorString,
            int precedence,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterStringParser,
            Func<TTerm, TTerm> map)
            => AddPostfix(operatorString, precedence, false, afterStringParser, map);

        public Operators<TTerm, TAfterString> AddPostfix(
            string operatorString,
            int precedence,
            bool isAssociative,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterStringParser,
            Func<TTerm, TTerm> map) {
            _ = operators.Add(new PostfixOperator<TTerm, TAfterString, Unit>(
                operatorString,
                afterStringParser,
                precedence,
                isAssociative,
                map.ToFSharpFunc()));
            return this;
        }

        #endregion AddPostfix()

        #region AddTernary()

        public Operators<TTerm, TAfterString> AddTernary(
            string leftString,
            string rightString,
            int precedence,
            Associativity associativity,
            Func<TTerm, TTerm, TTerm, TTerm> map)
            => AddTernary(leftString, stop, rightString, stop, precedence, associativity, map);

        public Operators<TTerm, TAfterString> AddTernary(
            string leftString,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterLeftStringParser,
            string rightString,
            FSharpFunc<CharStream<Unit>, Reply<TAfterString>> afterRightStringParser,
            int precedence,
            Associativity associativity,
            Func<TTerm, TTerm, TTerm, TTerm> map) {
            _ = operators.Add(new TernaryOperator<TTerm, TAfterString, Unit>(
                leftString,
                afterLeftStringParser,
                rightString,
                afterRightStringParser,
                precedence,
                associativity,
                map.ToFSharpFunc()));
            return this;
        }

        #endregion AddTernary()

        public IEnumerator<Operator<TTerm, TAfterString, Unit>> GetEnumerator() => operators.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => operators.GetEnumerator();
    }
}
