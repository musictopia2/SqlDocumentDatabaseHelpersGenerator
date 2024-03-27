namespace SqlDocumentDatabaseHelpersGenerator;
internal class ResultsModel
{
    public string DatabaseName { get; set; } = "";
    public string CollectionName { get; set; } = "";
    public EnumSourceGeneratedSerializeOptions SerializeOption { get; set; }
    public string ClassName { get; set; } = "";
    public string MainNamespace { get; set; } = "";
    public EnumDocumentCategory DocumentCategory { get; set; } = EnumDocumentCategory.None; //not given yet.
    public EnumSimpleCategory SimpleCategory { get; set; } = EnumSimpleCategory.None;
    public string SqlitePath { get; set; } = ""; //may be given but may not be given.
    public string DefaultValue { get; set; } = ""; //if a default value is given, use it.  sometimes not even needed.
    public INamedTypeSymbol? GenericSymbol { get; set; }
    public bool Nullable { get; set; } //may need to know if this is nullable (?)
    public bool UsedPartial { get; set; } //so if the proper interfaces was implemented but did not use partial, then can eventually raise an error and not allow compilation
}