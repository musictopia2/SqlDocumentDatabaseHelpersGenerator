namespace SqlDocumentDatabaseHelpersGenerator;
[Generator] //this is important so it knows this class is a generator which will generate code for a class using it.
#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting
public class MySourceGenerator : IIncrementalGenerator
#pragma warning restore RS1036 // Specify analyzer banned API enforcement setting
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> declares = context.SyntaxProvider.CreateSyntaxProvider(
            (s, _) => IsSyntaxTarget(s),
            (t, _) => GetTarget(t))
            .Where(m => m != null)!;
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilation
            = context.CompilationProvider.Combine(declares.Collect());
        context.RegisterSourceOutput(compilation, (spc, source) =>
        {
            Execute(source.Item1, source.Item2, spc);
        });
    }
    private bool IsSyntaxTarget(SyntaxNode syntax)
    {
        bool rets = syntax is ClassDeclarationSyntax;
        return rets;
    }
    private ClassDeclarationSyntax? GetTarget(GeneratorSyntaxContext context)
    {
        var ourClass = context.GetClassNode(); //can use the sematic model at this stage
        var symbol = context.GetClassSymbol(ourClass);
        //we have to make sure only one class is allowed.
        bool rets = symbol.Implements("ISourceGeneratedDataAccess");
        if (rets == false)
        {
            return null;
        }
        return ourClass; //decided to not do the extras anymore.
    }
    private void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> list, SourceProductionContext context)
    {
        //at this point, we have a list of classes.  its already been filtered.
        var others = list.Distinct();
        ParserClass parses = new(others, compilation);
        var results = parses.GetResults();
        EmitClass emit = new(results, context);
        emit.Emit();
    }
}