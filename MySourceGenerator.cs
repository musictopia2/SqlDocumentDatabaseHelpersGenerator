namespace SqlDocumentDatabaseHelpersGenerator;
[Generator] //this is important so it knows this class is a generator which will generate code for a class using it.
#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting
public class MySourceGenerator : IIncrementalGenerator
#pragma warning restore RS1036 // Specify analyzer banned API enforcement setting
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //step 1
        IncrementalValuesProvider<ClassDeclarationSyntax> declares1 = context.SyntaxProvider.CreateSyntaxProvider(
            (s, _) => IsSyntaxTarget(s),
            (t, _) => GetTarget(t))
            .Where(m => m != null)!;


        //step 2
        var declares2 = context.CompilationProvider.Combine(declares1.Collect());

        //step 3
        var declares3 = declares2.SelectMany(static (x, _) =>
        {
            ImmutableHashSet<ClassDeclarationSyntax> start = [.. x.Right];
            return GetResults(start, x.Left);
        });
        //step 4
        var declares4 = declares3.Collect();
        //final step
        context.RegisterSourceOutput(declares4, Execute);
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
    private static ImmutableHashSet<ResultsModel> GetResults(
        ImmutableHashSet<ClassDeclarationSyntax> classes,
        Compilation compilation
        )
    {
        ParserClass parses = new(classes, compilation);
        BasicList<ResultsModel> output = parses.GetResults();
        return [.. output];
    }
    private void Execute(SourceProductionContext context, ImmutableArray<ResultsModel> list)
    {
        EmitClass emit = new(list, context);
        emit.Emit();
    }
}