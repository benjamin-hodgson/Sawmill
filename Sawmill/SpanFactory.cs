using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Sawmill
{
    // todo: implement directly in IL? might need different builds for each target framework
    internal static class SpanFactory<T>
    {
        private delegate Span<T> SpanCtor(ref T value, int length);
        private static readonly SpanCtor? _spanCtor;
        public static bool CanCreate => _spanCtor != null;

        static SpanFactory()
        {
            var ctor = typeof(Span<T>)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .SingleOrDefault(c => c.GetParameters().Length == 2 && c.GetParameters()[0].ParameterType.IsByRef);

            if (ctor == null)
            {
                return;
            }

            var method = new DynamicMethod("", typeof(Span<T>), new[] { typeof(T).MakeByRefType(), typeof(int) });

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            _spanCtor = (SpanCtor)method.CreateDelegate(typeof(SpanCtor));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> Create(ref T value, int length)
            => _spanCtor!(ref value, length);
    }
}