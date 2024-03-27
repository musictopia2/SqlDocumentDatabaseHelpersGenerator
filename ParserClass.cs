namespace SqlDocumentDatabaseHelpersGenerator;
internal class ParserClass(IEnumerable<ClassDeclarationSyntax> list, Compilation compilation)
{
    public BasicList<ResultsModel> GetResults()
    {
        BasicList<ResultsModel> output = [];
        foreach (var item in list)
        {
            ResultsModel results = GetResult(item);
            output.Add(results);
        }
        return output;
    }
    private string GetCaptureValue(PropertyDeclarationSyntax syntax)
    {
        var member = syntax.DescendantNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault();
        if (member is not null)
        {
            return member.ToString();
        }
        var firsts = syntax.DescendantNodes().OfType<LiteralExpressionSyntax>().FirstOrDefault();
        if (firsts is null)
        {
            return "null";
        }
        if (firsts!.Token.Value is null)
        {
            return "null";
        }
        return $"""
            "{firsts.Token.Value}"
            """;
    }
    private string GetDefaultValue(PropertyDeclarationSyntax syntax, EnumSimpleCategory category)
    {
        if (category == EnumSimpleCategory.CustomEnum)
        {
            var options = syntax.DescendantNodes().OfType<IdentifierNameSyntax>().Last();
            string value = options.Identifier.ValueText;
            return value;
        }
        if (category == EnumSimpleCategory.DateOnly || category == EnumSimpleCategory.DateTime)
        {
            var implicitCreation = syntax.DescendantNodes().OfType<ImplicitObjectCreationExpressionSyntax>().FirstOrDefault();
            if (implicitCreation is not null)
            {
                return implicitCreation.ToString(); //i think.
            }
            var oldCreation = syntax.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
            if (oldCreation is not null)
            {
                return oldCreation.ToString();
            }
            return "default"; //try default
        }
        return GetCaptureValue(syntax)!;
    }
    private ResultsModel GetResult(ClassDeclarationSyntax classDeclaration)
    {
        ResultsModel output = new();
        SemanticModel compilationSemanticModel = classDeclaration.GetSemanticModel(compilation);
        INamedTypeSymbol symbol = compilationSemanticModel.GetDeclaredSymbol(classDeclaration)!;
        var temp = symbol.AllInterfaces.Single(x => x.Name == "ISourceGeneratedDataAccess");
        var firstArgument = (INamedTypeSymbol)temp!.TypeArguments[0];
        if (firstArgument.Name == "Nullable")
        {
            output.Nullable = true;
            var usedWithNull = (INamedTypeSymbol)firstArgument.TypeArguments[0];
            output.GenericSymbol = usedWithNull;
        }
        else
        {
            output.GenericSymbol = firstArgument;
        }
        if (symbol.Implements("IPlainDataAccess") || symbol.Implements("ISimpleDataAccess"))
        {
            output.DocumentCategory = EnumDocumentCategory.SimpleTypesAccess;
        }
        else if (symbol.Implements("IListDataAccess"))
        {
            output.DocumentCategory = EnumDocumentCategory.ListAccess;
        }
        else if (symbol.Implements("IObjectDataAccess"))
        {
            output.DocumentCategory = EnumDocumentCategory.ObjectAccess;
        }
        if (output.DocumentCategory == EnumDocumentCategory.SimpleTypesAccess)
        {
            ITypeSymbol genericSymbol = output.GenericSymbol;
            if (genericSymbol.TypeKind == TypeKind.Enum)
            {
                output.SimpleCategory = EnumSimpleCategory.StandardEnum;
            }
            else if (genericSymbol.Name.StartsWith("Enum"))
            {
                output.SimpleCategory = EnumSimpleCategory.CustomEnum;
            }
            else if (genericSymbol.Name == "Int32")
            {
                output.SimpleCategory = EnumSimpleCategory.Int;
            }
            else if (genericSymbol.Name == "Boolean")
            {
                output.SimpleCategory = EnumSimpleCategory.Bool;
            }
            else if (genericSymbol.Name == "Decimal")
            {
                output.SimpleCategory = EnumSimpleCategory.Decimal;
            }
            else if (genericSymbol.Name == "Double")
            {
                output.SimpleCategory = EnumSimpleCategory.Double;
            }
            else if (genericSymbol.Name == "Float")
            {
                output.SimpleCategory = EnumSimpleCategory.Float;
            }
            else if (genericSymbol.Name == "DateOnly")
            {
                output.SimpleCategory = EnumSimpleCategory.DateOnly;
            }
            else if (genericSymbol.Name == "DateTime")
            {
                output.SimpleCategory = EnumSimpleCategory.DateTime;
            }
            else if (genericSymbol.Name == "String")
            {
                output.SimpleCategory = EnumSimpleCategory.String;
            }
            else
            {
                output.SimpleCategory = EnumSimpleCategory.None; //can show errors showing not supported.
            }
        }
        output.MainNamespace = symbol.ContainingNamespace.ToDisplayString();
        output.ClassName = symbol.Name;
        output.UsedPartial = classDeclaration.IsPartial(); //this is very common.
        var list = classDeclaration.Members.OfType<PropertyDeclarationSyntax>().ToBasicList();
        foreach (var item in list)
        {
            IPropertySymbol other = compilationSemanticModel.GetDeclaredSymbol(item)!;
            if (other.ExplicitInterfaceImplementations.Count() == 1)
            {
                var fins = other.ExplicitInterfaceImplementations.Single();
                if (fins.Name == "DatabaseName")
                {
                    output.DatabaseName = GetCaptureValue(item)!;
                }
                else if (fins.Name == "CollectionName")
                {
                    output.CollectionName = GetCaptureValue(item)!;
                }
                else if (fins.Name == "SerializationOptions")
                {
                    var options = item.DescendantNodes().OfType<IdentifierNameSyntax>().Last();
                    string value = options.Identifier.ValueText;
                    if (value == "Old")
                    {
                        output.SerializeOption = EnumSourceGeneratedSerializeOptions.Old;
                    }
                    else if (value == "RegisterSingleInstance")
                    {
                        output.SerializeOption = EnumSourceGeneratedSerializeOptions.RegisterSingleInstance;
                    }
                    else if (value == "ForceEntireSystem")
                    {
                        output.SerializeOption = EnumSourceGeneratedSerializeOptions.ForceEntireSystem;
                    }
                }
                else if (fins.Name == "Path")
                {
                    output.SqlitePath = GetCaptureValue(item)!;

                }
                else if (fins.Name == "DefaultValue")
                {
                    output.DefaultValue = GetDefaultValue(item, output.SimpleCategory);
                }
            }
        }
        if (output.SqlitePath == "")
        {
            output.SqlitePath = """
                            ""
                            """;
        }
        if (output.DefaultValue == "" && output.SimpleCategory != EnumSimpleCategory.CustomEnum)
        {
            output.DefaultValue = "default";
        }
        else if (output.DefaultValue == "")
        {
            output.DefaultValue = """
                ""
                """;
        }
        return output;
    }
}