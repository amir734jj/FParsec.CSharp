using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using FParsec.CSharp;
using static FParsec.CSharp.CharParsersCS;
using static FParsec.CSharp.PrimitivesCS;
using static FParsec.CharParsers;
using static FParsec.Primitives;
using System;
using System.Runtime.CompilerServices;
using static FParsec.CSharp.Parser1;
using static FParsec.CSharp.Parser2;

namespace FParsec.CSharp {
    public class Parser1<T> : FSharpFunc<CharStream<Unit>, Reply<T>> {
        public readonly FSharpFunc<CharStream<Unit>, Reply<T>> p;
        public Parser1(FSharpFunc<CharStream<Unit>, Reply<T>> p) => this.p = p;
        public override Reply<T> Invoke(CharStream<Unit> chars) => p.Invoke(chars);
    }

    public static class Parser1 {
        public static Parser1<T> Box1<T>(this FSharpFunc<CharStream<Unit>, Reply<T>> p) => new Parser1<T>(p);
        public static FSharpFunc<CharStream<Unit>, Reply<T>> Unbox1<T>(this Parser1<T> p) => p.p;
        public static Parser1<char> AnyChar1 = anyChar<Unit>().Box1();
        public static Parser1<char> CharP1(char c) => pchar<Unit>(c).Box1();
        public static Parser1<(T1, T2)> And1<T1, T2>(this Parser1<T1> p1, Parser1<T2> p2) => op_DotGreaterGreaterDot(p1, p2).Map(x => x.ToValueTuple()).Box1();
    }

    public class Parser2<T> {
        public readonly FSharpFunc<CharStream<Unit>, Reply<T>> p;
        public Parser2(FSharpFunc<CharStream<Unit>, Reply<T>> p) => this.p = p;
        public static implicit operator Parser2<T>(FSharpFunc<CharStream<Unit>, Reply<T>> p) => new Parser2<T>(p);
        public static implicit operator FSharpFunc<CharStream<Unit>, Reply<T>>(Parser2<T> p) => p.p;
    }

    public static class Parser2 {
        public static Parser2<T> Box2<T>(this FSharpFunc<CharStream<Unit>, Reply<T>> p) => new Parser2<T>(p);
        public static FSharpFunc<CharStream<Unit>, Reply<T>> Unbox2<T>(this Parser2<T> p) => p.p;
        public static Parser2<char> AnyChar2 = anyChar<Unit>();
        public static Parser2<char> CharP2(char c) => pchar<Unit>(c);
        public static Parser2<(T1, T2)> And2<T1, T2>(this Parser2<T1> p1, Parser2<T2> p2) => op_DotGreaterGreaterDot<T1, Unit, T2>(p1, p2).Map(x => x.ToValueTuple());
    }

    class Parser1Tests {
        void FromFParsecParser() { Parser1<char> p = anyChar<Unit>().Box1(); }
        void ToFParsecParser() { FSharpFunc<CharStream<Unit>, Reply<char>> p = AnyChar1; }
        void PassToFParsec() { var p = many(AnyChar1); }
        void Combine() { var p = Parser1.CharP1('x').And1(AnyChar1); }
        void CombineFParsecParsers() { var p = pchar<Unit>('x').Box1().And1(anyChar<Unit>().Box1()); }
        void CombineDefinedOnFParsecType() { var p = CharP1('x').And(AnyChar1); }
    }

    class Parser2Tests {
        void FromFParsecParser() { Parser2<char> p = anyChar<Unit>(); }
        void ToFParsecParser() { FSharpFunc<CharStream<Unit>, Reply<char>> p = AnyChar2; }
        void PassToFParsec() { var p = many<char, Unit>(AnyChar2); }
        void Combine() { var p = CharP2('x').And2(AnyChar2); }
        void CombineFParsecParsers() { var p = pchar<Unit>('x').Box2().And2(anyChar<Unit>().Box2()); }
        void CombineDefinedOnFParsecType() { var p = CharP2('x').Unbox2().And(AnyChar2.Unbox2()); }
    }
}
