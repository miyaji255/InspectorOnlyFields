namespace InspectorOnlyAnalyzer
{
    partial class InspectorOnlyAttributesGenerator
    {
        private const string _ioaSource = """
            #pragma warning disable IDE0079
            #pragma warning disable IDE0005
            using System;

            namespace InspectorOnlyFields
            {
                [AttributeUsage(AttributeTargets.Field)]
                internal sealed class InspectorOnlyAttribute : Attribute { }
            }
            """;

        private const string _ignoreSource = """
            #pragma warning disable IDE0079
            #pragma warning disable IDE0005
            using System;

            namespace InspectorOnlyFields
            {
                [AttributeUsage(AttributeTargets.Field)]
                internal sealed class IgnoreInspectorOnlyAttribute : Attribute { }
            }
            """;
    }
}
