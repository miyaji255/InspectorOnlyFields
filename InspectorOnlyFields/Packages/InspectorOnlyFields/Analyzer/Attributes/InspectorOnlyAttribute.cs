using System;

namespace InspectorOnlyFields
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InspectorOnlyAttribute : Attribute { }
}
