using System;

namespace Framework.Core.FrameDI
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute { }
}