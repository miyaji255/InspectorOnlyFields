using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Dena.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis;
using Xunit;

namespace InspectorOnlyAnalyzer.Test
{
    public class InspectorOnlyFieldAnalyzerTest
    {
        [Fact]
        public async Task EmptySourceAsync()
        {
            var analyzer = new InspectorOnlyFieldsAnalyzer();
            var source = """
                public class Foo
                {
                    public int Field;
                }

                """;
            source = source.AddAttributes();

            var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

            Assert.Empty(diagnostics.ValidDiagnostics());
        }

        [Theory]
        [MemberData(nameof(GetGoodSources))]
        public async Task GoodSourceAsync(string source)
        {
            var analyzer = new InspectorOnlyFieldsAnalyzer();
            source = source.AddAttributes();

            var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

            Assert.Empty(diagnostics.ValidDiagnostics());
        }

        [Theory]
        [MemberData(nameof(GetBadSources))]
        public async Task BadSourceAsync(string source, DiagnosticData[] expectedDiagnositcs)
        {
            var analyzer = new InspectorOnlyFieldsAnalyzer();
            source = source.AddAttributes();

            var actualDiagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

            actualDiagnostics = actualDiagnostics
                .ValidDiagnostics()
                .OrderBy(d => d.Location.SourceSpan.Start)
                .ThenBy(d => d.Id)
                .ThenBy(d => d.GetMessage())
                .ToImmutableArray();

            Assert.Equal(expectedDiagnositcs.Length, actualDiagnostics.Length);
            for (var i = 0; i < expectedDiagnositcs.Length; i++)
            {
                var expected = expectedDiagnositcs[i];
                var actual = actualDiagnostics[i];

                Assert.Equal(expected.Id, actual.Id);
                if (expected.Message is not null)
                    Assert.Equal(expected.Message, actual.GetMessage());
                Assert.Equal(expected.Severity, actual.Severity);
                Assert.Equal(expected.StartPosition, actual.Location.GetLineSpan().StartLinePosition);
                Assert.Equal(expected.EndPosition, actual.Location.GetLineSpan().EndLinePosition);
            }
        }

        public static object[][] GetGoodSources()
        {
            #region good test sources
            var sources = new string[] {
                // Field に対して使うとき
                """
                using InspectorOnlyFields;
                using UnityEngine;

                public class Foo
                {
                    [InspectorOnly]
                    public int Field1;

                    [SerializeField]
                    [InspectorOnly]
                    private int Field2;

                    private void StartUp()
                    {
                        var temp = Field1 * 2;
                        temp = Field2 * 2;
                    }
                }

                """,
                // Field に対して使わないとき
                """
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    public int Field1 = 1;

                    [SerializeField]
                    private int Field2 = 1;

                    private void StartUp()
                    {
                        var temp = Field1 * 2;
                        temp = Field2 * 2;
                        Field1 = temp;
                        Field2 = temp;
                        (Field1, (temp, Field1)) = (3, (4, 5));
                        (Field1, (temp, Field2)) = (3, (4, 5));
                    }
                }

                """,
                // Property に対して使うとき
                """
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    [field: SerializeField]
                    [field: InspectorOnly]
                    public int Field1 { get; set; }

                    [field: SerializeField]
                    [field: InspectorOnly]
                    private int Field2 { get; set; }

                    private void StartUp()
                    {
                        var temp = Field1 * 2;
                        temp = Field2 * 2;
                    }
                }

                """,
                // Property に対して使わないとき
                """
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    [field: SerializeField]
                    public int Field1 { get; set; } = 1;

                    [field: SerializeField]
                    private int Field2 { get; set; } = 1;

                    private void StartUp()
                    {
                        var temp = Field1 * 2;
                        temp = Field2 * 2;
                        Field1 = temp;
                        Field2 = temp;
                        (Field1, (temp, Field1)) = (3, (4, 5));
                        (Field1, (temp, Field2)) = (3, (4, 5));
                    }
                }

                """,
            };
            #endregion
            return sources.Select(s => new string[] { s }).ToArray();
        }

        public static object[][] GetBadSources()
        {
            #region bad test sources
            var sources = new (string, DiagnosticData[])[] {
                // Field に代入するとき
                ("""
                using System;
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    [InspectorOnly]
                    public int Field1 = 1;

                    [SerializeField]
                    [InspectorOnly]
                    private int Field2 = 1;

                    private void StartUp()
                    {
                        Field1 = 1;
                        Field2 = 2;
                        var temp = 0;
                        (Field1, (Field2, temp)) = (5, (6, 7));
                    }
                }

                """,
                new DiagnosticData[] {
                    new("IOF001", DiagnosticSeverity.Warning, (7, 22, 25)),
                    new("IOF001", DiagnosticSeverity.Warning, (11, 23, 26)),
                    new("IOF001", DiagnosticSeverity.Warning, (15, 8, 14)),
                    new("IOF001", DiagnosticSeverity.Warning, (16, 8, 14)),
                    new("IOF001", DiagnosticSeverity.Warning, (18, 9, 15)),
                    new("IOF001", DiagnosticSeverity.Warning, (18, 18, 24)),
                }),
                // Field の宣言が間違えているとき
                ("""
                using System;
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    [NonSerialized]
                    [InspectorOnly]
                    public int Field1;

                    [InspectorOnly]
                    private int Field2;

                    [SerializeField]
                    [NonSerialized]
                    [InspectorOnly]
                    private int Field3;
                }

                """,
                new DiagnosticData[] {
                    new("IOF003", DiagnosticSeverity.Warning, (8, 15, 21)),
                    new("IOF002", DiagnosticSeverity.Warning, (11, 16, 22)),
                    new("IOF003", DiagnosticSeverity.Warning, (16, 16, 22)),
                }),
                // Property に代入するとき
                ("""
                using System;
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    [field: SerializeField]
                    [field: InspectorOnly]
                    public int Field1 { get; set; } = 1;

                    [field: SerializeField]
                    [field: InspectorOnly]
                    private int Field2 { get; set; } = 1;

                    private void StartUp()
                    {
                        Field1 = 1;
                        Field2 = 2;
                        var temp = 0;
                        (Field1, (Field2, temp)) = (5, (6, 7));
                    }
                }

                """,
                new DiagnosticData[] {
                    new("IOF001", DiagnosticSeverity.Warning, (8, 36, 39)),
                    new("IOF001", DiagnosticSeverity.Warning, (12, 37, 40)),
                    new("IOF001", DiagnosticSeverity.Warning, (16, 8, 14)),
                    new("IOF001", DiagnosticSeverity.Warning, (17, 8, 14)),
                    new("IOF001", DiagnosticSeverity.Warning, (19, 9, 15)),
                    new("IOF001", DiagnosticSeverity.Warning, (19, 18, 24)),
                }),
                // Property の宣言が間違えているとき
                ("""
                using System;
                using InspectorOnlyFields;
                using UnityEngine;
                
                public class Foo
                {
                    [field: NonSerialized]
                    [field: InspectorOnly]
                    public int Field1 { get; set; }
                
                    [field: InspectorOnly]
                    private int Field2 { get; set; }

                    [field: SerializeField]
                    [field: NonSerialized]
                    [field: InspectorOnly]
                    private int Field3 { get; set; }
                }

                """,
                new DiagnosticData[] {
                    new("IOF002", DiagnosticSeverity.Warning, (8, 15, 21)),
                    new("IOF003", DiagnosticSeverity.Warning, (8, 15, 21)),
                    new("IOF002", DiagnosticSeverity.Warning, (11, 16, 22)),
                    new("IOF003", DiagnosticSeverity.Warning, (16, 16, 22)),
                }),
            };
            #endregion
            return sources.Select(s => new object[] { s.Item1, s.Item2 }).ToArray();
        }
    }
}


