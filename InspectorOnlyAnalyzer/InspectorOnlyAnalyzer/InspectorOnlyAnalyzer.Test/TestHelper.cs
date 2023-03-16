using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InspectorOnlyAnalyzer.Test
{
    internal static class TestHelper
    {
        public static string AddAttributes(this string source)
        {
            const string inspectorOnly = """
                namespace InspectorOnlyFields
                {
                    using System;

                    [AttributeUsage(AttributeTargets.Field)]
                    public class InspectorOnlyAttribute : Attribute { }
                }

                """;
            const string serializeField = """
                namespace UnityEngine
                {
                    using System;

                    [AttributeUsage(AttributeTargets.Field)]
                    public class SerializeField : Attribute
                    {
                    }
                }

                """;
            return string.Concat(source, inspectorOnly, serializeField);
        }

        public static IEnumerable<Diagnostic> ValidDiagnostics(this IEnumerable<Diagnostic> diagnostics)
            => diagnostics.Where(d => d.Id.StartsWith("IOF") || d.DefaultSeverity == DiagnosticSeverity.Error);
    }

    public record class DiagnosticData(string Id, string? Message, DiagnosticSeverity Severity, LinePosition StartPosition, LinePosition EndPosition)
    {
        public static readonly string Message01 = " へ代入することは InspectorOnly 属性により禁止されています";
        public static readonly string Message02 = "InspectorOnly 属性はシリアライズ可能なフィールドに付与する必要があります";
        public static readonly string Message03 = "InspectorOnly 属性は NonSerialized 属性と同時に使用することができません";

        public DiagnosticData(string Id, DiagnosticSeverity Severity, LinePosition StartPosition, LinePosition EndPosition)
            : this(Id, null, Severity, StartPosition, EndPosition) { }

        public DiagnosticData(string Id, DiagnosticSeverity Severity, (int, int) StartPosition, (int, int) EndPosition)
            : this(Id, Severity, new LinePosition(StartPosition.Item1, StartPosition.Item2), new LinePosition(EndPosition.Item1, EndPosition.Item2)) { }

        public DiagnosticData(string Id, DiagnosticSeverity Severity, (int, int, int) Position)
            : this(Id, Severity, new LinePosition(Position.Item1, Position.Item2), new LinePosition(Position.Item1, Position.Item3)) { }
    }
}
