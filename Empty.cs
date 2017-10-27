using System;
using System.Dynamic;
using System.Linq;
using ImpromptuInterface;

namespace DynamicObjects
{
    public class Empty<T> : DynamicObject where T : class
    {
        public static T Instance => new Empty<T>().ActLike<T>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var type = typeof(T).GetProperties().FirstOrDefault(m => m.Name == binder.Name)?.PropertyType;
            result = TryInit(type);
            return true;
        }

        //String return empty String
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var type = args.Length == 1 ? args[0].GetType() : typeof(T).GetMethods().FirstOrDefault(m => m.Name == binder.Name && m.ReturnType.Name != "Void")?.ReturnType;
            result = TryInit(type);
            return true;
        }

        private object TryInit(Type type)
        {
            if (type == null)
                return null;

            if (type.IsValueType)
                return Activator.CreateInstance(type);
            if (type.Name == "String")
                return string.Empty;
            if (TypeHaveEmptyConstructor(type))
                return Activator.CreateInstance(type);
            return null;
        }

        private bool TypeHaveEmptyConstructor(Type type)
        {
            var contructors = type.GetConstructors();//.Any(c => c.CustomAttributes.Any());
            foreach (var c in contructors)
            {
                var r = c.GetParameters();
                if (r.Length == 0)
                    return true;
            }
            return false;
        }
    }
}
