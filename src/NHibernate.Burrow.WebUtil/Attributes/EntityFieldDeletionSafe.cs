using System;
using NHibernate.Burrow.WebUtil.Impl;

namespace NHibernate.Burrow.WebUtil.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EntityFieldDeletionSafe : StatefulField
    {
        public EntityFieldDeletionSafe() : base(typeof (GetEntityVSFInterceptor).AssemblyQualifiedName) {}
    }
}