using System;
using System.Dynamic;
using System.Linq;
using ImpromptuInterface;

namespace DynamicObjects
{
    public class Null<T> : DynamicObject where T : class
    {
        public static T Instance => new Null<T>().ActLike<T>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var type = typeof(T).GetProperties().FirstOrDefault(m => m.Name == binder.Name)?.PropertyType;
            result = type != null && type.IsValueType ? Activator.CreateInstance(type) : null;
            return true;
        }

        //String return null ref 
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var type = args.Length == 1 ? args[0].GetType() : typeof(T).GetMethods().FirstOrDefault(m => m.Name == binder.Name && m.ReturnType.Name != "Void")?.ReturnType;
            result = type != null && type.IsValueType ? Activator.CreateInstance(type) : null;
            return true;
        }
    }
}