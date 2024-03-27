namespace SqlDocumentDatabaseHelpersGenerator;
internal static class SourceBuilderExtensions
{
    public static void DocumentDatabaseStub(this SourceCodeStringBuilder builder, Action<ICodeBlock> action, string mainNameSpace, string className)
    {
        builder.WriteLine("#nullable enable")
            .WriteLine(w =>
         {
             w.Write("namespace ")
             .Write(mainNameSpace)
             .Write(";");

         }).WriteLine(w =>
         {
             w.Write("public partial class ")
             .Write(className);
         })
         .WriteCodeBlock(w =>
         {
             w.WriteLine("private readonly global::DocumentDbLibrary.DocumentContext _context;");
             action.Invoke(w);
         });
    }
}