using Microsoft.CodeAnalysis;

namespace InspectorOnlyAnalyzer
{
    [Generator]
    public partial class InspectorOnlyAttributesGenerator : ISourceGenerator
    {
        public static readonly string InspectorOnlyAttributeName = "InspectorOnlyFields.InspectorOnlyAttribute";
        public static readonly string[] InspectorOnlyAttributeNameArray = InspectorOnlyAttributeName.Split('.');

        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context) {
            context.AddSource("InspectorOnlyAttribute.cs", _ioaSource);
            //context.AddSource("IgnoreInspectorOnlyAttribute.cs", _ignoreSource);
        }
    }
}
