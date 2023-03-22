#pragma warning disable RS2008 // アナライザー リリース追跡を有効にする
using System.Collections.Immutable;
using System.Linq;
using InspectorOnlyAnalyzer.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace InspectorOnlyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InspectorOnlyFieldsAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor _referenceRule = new(
                id: "InspOnly001",
                title: "a",
                messageFormat: new LocalizableResourceString(nameof(Resources.MessageFormat001), Resources.ResourceManager, typeof(Resources)),
                category: "InspectorUtilAnalyzerCorrectness",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true
            );

        private static readonly DiagnosticDescriptor _declareRule = new(
                id: "InspOnly002",
                title: "a",
                messageFormat: new LocalizableResourceString(nameof(Resources.MessageFormat002), Resources.ResourceManager, typeof(Resources)),
                category: "InspectorUtilAnalyzerCorrectness",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true
            );

        private static readonly DiagnosticDescriptor _declareWithoutNonSerializedRule = new(
                id: "InspOnly003",
                title: "a",
                messageFormat: new LocalizableResourceString(nameof(Resources.MessageFormat003), Resources.ResourceManager, typeof(Resources)),
                category: "InspectorUtilAnalyzerCorrectness",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true
            );

        private static readonly DiagnosticDescriptor _containingTypeRule = new(
                id: "InspOnly004",
                title: "a",
                messageFormat: new LocalizableResourceString(nameof(Resources.MessageFormat004), Resources.ResourceManager, typeof(Resources)),
                category: "InspectorUtilAnalyzerCorrectness",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true
            );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
                _referenceRule,
                _declareRule,
                _declareWithoutNonSerializedRule,
                _containingTypeRule
            );

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterOperationAction(AnalyzeSymbol, OperationKind.FieldReference, OperationKind.PropertyReference, OperationKind.FieldInitializer, OperationKind.PropertyInitializer);

            context.RegisterSymbolAction(AnalyzeDeclare, SymbolKind.Field, SymbolKind.Property);
        }

        #region assignment
        private void AnalyzeSymbol(OperationAnalysisContext context)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            if (context.Operation.Kind == OperationKind.FieldReference)
            {
                if (!(context.Operation is IFieldReferenceOperation fieldReference)
                    || !VerifyFieldReference(fieldReference.Field))
                    return;
            }
            else if (context.Operation.Kind == OperationKind.PropertyReference)
            {
                if (!(context.Operation is IPropertyReferenceOperation propertyReference)
                    || !TryGetAutoPropertyField(propertyReference.Property, out var field)
                    || !VerifyFieldReference(field))
                    return;
            }
            else if (context.Operation.Kind == OperationKind.FieldInitializer)
            {
                if (!(context.Operation is IFieldInitializerOperation fieldInitializer)
                    || !VerifyFieldReference(fieldInitializer.InitializedFields[0]))
                    return;

                var diagnostic = Diagnostic.Create(_referenceRule, context.Operation.Syntax.GetLocation(), fieldInitializer.InitializedFields[0].ToDisplayParts().LastOrDefault().ToString());
                context.ReportDiagnostic(diagnostic);
            }
            else if (context.Operation.Kind == OperationKind.PropertyInitializer)
            {
                if (!(context.Operation is IPropertyInitializerOperation propertyInitializer)
                    || !TryGetAutoPropertyField(propertyInitializer.InitializedProperties[0], out var field)
                    || !VerifyFieldReference(field))
                    return;

                var diagnostic = Diagnostic.Create(_referenceRule, context.Operation.Syntax.GetLocation(), propertyInitializer.InitializedProperties[0].ToDisplayParts().LastOrDefault().ToString());
                context.ReportDiagnostic(diagnostic);
            }

            if (context.Operation.Parent is IAssignmentOperation assignmentOperation && assignmentOperation.Target.Syntax.Span == context.Operation.Syntax.Span
             || context.Operation.Parent is ITupleOperation tupleOperation && VerifyTuple(tupleOperation))
            {
                var memberReference = (IMemberReferenceOperation)context.Operation;
                var diagnostic = Diagnostic.Create(_referenceRule, context.Operation.Syntax.GetLocation(), memberReference.Member.ToDisplayParts().LastOrDefault().ToString());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool TryGetAutoPropertyField(IPropertySymbol property, out IFieldSymbol field)
        {
            field = property.ContainingType.GetMembers()
                .OfType<IFieldSymbol>()
                .FirstOrDefault(f => SymbolEqualityComparer.Default.Equals(f.AssociatedSymbol, property));

            return field != null;
        }

        private bool VerifyTuple(ITupleOperation tupleOperation)
        {
            switch (tupleOperation.Parent)
            {
                case IAssignmentOperation assignmentOperation:
                    return tupleOperation.Syntax.Span == assignmentOperation.Target.Syntax.Span;
                case ITupleOperation parent:
                    return VerifyTuple(parent);
                default:
                    return false;
            }
        }

        private bool VerifyFieldReference(IFieldSymbol fieldSymbol)
        {
            // public で定義されているとき
            if (fieldSymbol.DeclaredAccessibility == Accessibility.Public)
                return fieldSymbol.GetAttributes().Any(a => VerifySymbolFullName(a.AttributeClass, InspectorOnlyAttributesGenerator.InspectorOnlyAttributeNameArray))
                    && VerifyContainingType(fieldSymbol.ContainingType);
            else
                return fieldSymbol.GetAttributes().Any(a => VerifySymbolFullName(a.AttributeClass, InspectorOnlyAttributesGenerator.InspectorOnlyAttributeNameArray))
                    && fieldSymbol.GetAttributes().Any(a => VerifySymbolFullName(a.AttributeClass, "UnityEngine", "SerializeField"))
                    && VerifyContainingType(fieldSymbol.ContainingType);
        }

        private bool VerifySymbolFullName(INamespaceOrTypeSymbol? symbol, params string[] names)
        {
            for (var i = names.Length - 1; i >= 0; i--)
            {
                if (names[i] != symbol?.Name)
                    return false;

                symbol = symbol.ContainingNamespace;
            }
            return symbol?.Name == "";
        }

        private bool VerifyContainingType(INamedTypeSymbol typeSymbol)
        {
            return !typeSymbol.IsAbstract
                && !typeSymbol.IsStatic
                && (VerifySymbolFullName(typeSymbol.BaseType, "UnityEngine", "MonoBehaviour")
                    || typeSymbol.GetAttributes().Any(a => VerifySymbolFullName(a.AttributeClass, "System", "SerializableAttribute"))
                    || VerifyBaseType(typeSymbol));
        }

        private bool VerifyBaseType(INamedTypeSymbol typeSymbol)
        {
            return VerifySymbolFullName(typeSymbol.BaseType, "UnityEngine", "Object")
                || typeSymbol.BaseType is not null && VerifyBaseType(typeSymbol.BaseType);
        }
        #endregion

        #region declare
        private void AnalyzeDeclare(SymbolAnalysisContext context)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            IFieldSymbol fieldSymbol;
            if (context.Symbol.Kind == SymbolKind.Property)
            {
                if (!TryGetAutoPropertyField((IPropertySymbol)context.Symbol, out fieldSymbol))
                    return;
            }
            else if (context.Symbol.Kind == SymbolKind.Field)
            {
                fieldSymbol = (IFieldSymbol)context.Symbol;
            }
            else
            {
                return;
            }

            var serializeFieldAttribute = new string[] { "UnityEngine", "SerializeField" };
            var nonSerializedAttribute = new string[] { "System", "NonSerializedAttribute" };

            var hasInspectorOnly = false;
            var hasSerializeField = false;
            var hasNonSerialized = false;
            var isInvalidContainingType = false;
            foreach (var attribute in fieldSymbol.GetAttributes())
            {
                if (!hasInspectorOnly && VerifySymbolFullName(attribute.AttributeClass, InspectorOnlyAttributesGenerator.InspectorOnlyAttributeNameArray))
                    hasInspectorOnly = true;
                if (!hasSerializeField && VerifySymbolFullName(attribute.AttributeClass, serializeFieldAttribute))
                    hasSerializeField = true;
                if (!hasNonSerialized && VerifySymbolFullName(attribute.AttributeClass, nonSerializedAttribute))
                    hasNonSerialized = true;
                if (!isInvalidContainingType && !VerifyContainingType(fieldSymbol.ContainingType))
                    isInvalidContainingType = true;
            }

            if (!hasInspectorOnly)
                return;

            if (hasNonSerialized)
            {
                var diagnostic = Diagnostic.Create(_declareWithoutNonSerializedRule, context.Symbol.Locations[0]);
                context.ReportDiagnostic(diagnostic);
            }

            if (fieldSymbol.DeclaredAccessibility != Accessibility.Public && !hasSerializeField)
            {
                var diagnostic = Diagnostic.Create(_declareRule, context.Symbol.Locations[0]);
                context.ReportDiagnostic(diagnostic);
            }

            if (isInvalidContainingType)
            {
                var diagnostic = Diagnostic.Create(_containingTypeRule, context.Symbol.Locations[0]);
                context.ReportDiagnostic(diagnostic);
            }
        }
        #endregion
    }
}
