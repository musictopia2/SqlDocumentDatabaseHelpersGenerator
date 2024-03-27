namespace SqlDocumentDatabaseHelpersGenerator;
internal class EmitClass(BasicList<ResultsModel> results, SourceProductionContext context)
{
    private string GetErrorMessage(ResultsModel result)
    {
        if (result.UsedPartial == false)
        {
            return $"The class {result.ClassName} from namespace {result.MainNamespace} requires partial classes";
        }
        if (result.DocumentCategory == EnumDocumentCategory.None)
        {
            return $"The class {result.ClassName} from namespace {result.MainNamespace} does not have a document category.  Try implementing the proper interface so it can capture it";
        }
        if (result.DocumentCategory == EnumDocumentCategory.SimpleTypesAccess)
        {
            if (result.DefaultValue == "")
            {
                return $"The class {result.ClassName} from namespace {result.MainNamespace} does not have default value for simple data access";
            }
        }
        return "";
    }
    private BasicList<string> GetErrors()
    {
        BasicList<string> output = [];
        foreach (var item in results)
        {
            string fins = GetErrorMessage(item);
            if (string.IsNullOrWhiteSpace(fins) == false)
            {
                output.Add(fins);
            }
        }
        return output;
    }
    public void Emit()
    {
        BasicList<string> errors = GetErrors();
        if (errors.Count > 0)
        {
            //don't do any source generation because of errors.
            foreach (var item in errors)
            {
                context.RaiseCustomException(item);
            }
            return;
        }
        foreach (var item in results)
        {
            WriteItem(item);
        }
    }
    private void WriteItem(ResultsModel item)
    {
        SourceCodeStringBuilder builder = new();
        builder.DocumentDatabaseStub(w =>
        {
            WriteOutClass(item, w);
        }, item.MainNamespace, item.ClassName);
        context.AddSource($"{item.ClassName}.g.cs", builder.ToString());
    }
    private void WriteOutClass(ResultsModel item, ICodeBlock w)
    {
        if (item.DocumentCategory == EnumDocumentCategory.ObjectAccess)
        {
            w.WriteLine("private string? _text;");
        }
        w.PopulateConstructor(item);
        if (item.DocumentCategory == EnumDocumentCategory.ObjectAccess)
        {
            w.PopulateObjectExists();
        }
        if (item.DocumentCategory == EnumDocumentCategory.ObjectAccess || item.DocumentCategory == EnumDocumentCategory.ListAccess)
        {
            w.PopulateGetDocuments(item)
                .PopulateUpsertRecords(item);
        }
        else if (item.DocumentCategory == EnumDocumentCategory.SimpleTypesAccess)
        {
            w.PopulateGetData(item)
                .PopulateSaveData(item);
        }
    }
}