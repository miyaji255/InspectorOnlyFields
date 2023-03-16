using Microsoft.CodeAnalysis;

namespace InspectorOnlyAnalyzer
{
    public class InspectorOnlyAttributesGenerator
    {
        public static readonly string InspectorOnlyAttributeName = "InspectorOnlyFields.InspectorOnlyAttribute";
        public static readonly string[] InspectorOnlyAttributeNameArray = InspectorOnlyAttributeName.Split('.');
    }
}
