namespace SqlDocumentDatabaseHelpersGenerator;
internal static class SourceContextExtensions
{
    public static void RaiseCustomException(this SourceProductionContext context, string information)
    {
        context.ReportDiagnostic(Diagnostic.Create(ExceptionDisplay(information), Location.None));
    }
    private static DiagnosticDescriptor ExceptionDisplay(string information) => new("First",
       "Failed",
       information,
       "Failed",
       DiagnosticSeverity.Error,
       true
       );
}