namespace SqlDocumentDatabaseHelpersGenerator;
internal static class WriterExtensions
{
    public static ICodeBlock PopulateConstructor(this ICodeBlock w, ResultsModel result)
    {
        w.WriteLine(w =>
        {
            w.Write("public ")
            .Write(result.ClassName)
            .Write("()");
        })
        .WriteCodeBlock(w =>
        {
            w.WriteLine(w =>
            {
                w.Write("_context = new(")
                .Write(result.DatabaseName)
                .Write(", ")
                .Write(result.CollectionName)
                .Write(", @")
                .Write(result.SqlitePath)
                .Write(");");
            });
            if (result.SerializeOption == EnumSourceGeneratedSerializeOptions.ForceEntireSystem)
            {
                w.WriteLine("global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.SystemTextJsonStrings.RequireCustomSerialization = true;");
            }
        });
        return w;
    }
    public static ICodeBlock PopulateObjectExists(this ICodeBlock w)
    {
        w.WriteLine("private protected async Task<bool> ObjectExists()")
            .WriteCodeBlock(w =>
            {
                w.WriteLine("string data = await _context!.GetDocumentAsync();")
                .WriteLine("if (string.IsNullOrWhiteSpace(data))")
                .WriteCodeBlock(w =>
                {
                    w.WriteLine("return false;");
                })
                .WriteLine("_text = data;")
                .WriteLine("return true;");
                ;
            });
        return w;
    }
    public static ICodeBlock PopulateGetDocuments(this ICodeBlock w, ResultsModel result)
    {
        if (result.DocumentCategory == EnumDocumentCategory.ObjectAccess)
        {
            w.WriteLine(w =>
            {
                w.Write("private protected async Task<")
                .Write(result.GenericSymbol!.Name)
                .Write("> GetDocumentsAsync()");
            })
            .WriteCodeBlock(w =>
            {
                w.WriteLine("if (_text is null)")
                .WriteCodeBlock(w =>
                {
                    w.WriteLine(w =>
                    {
                        w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                        .AppendDoubleQuote("No text was found.  Try to call ObjectExists first and do an action when the object does not exist")
                        .Write(");");
                    });
                });
                if (result.SerializeOption == EnumSourceGeneratedSerializeOptions.RegisterSingleInstance)
                {
                    w.WriteLine(w =>
                    {
                        w.Write("if (global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">.MasterContext is null)");
                    })
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Requires custom serialization to be aot compatible")
                            .Write(");");
                        });
                    })
                    .WriteLine(w =>
                    {
                        w.Write(result.GenericSymbol!.Name)
                        .Write("? output = default;");
                    })
                    .WriteLine("await Task.Run(() =>")
                    .WriteLambaBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("output = global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<")
                            .Write(result.GenericSymbol!.Name)
                            .Write(">.MasterContext.Deserialize(_text);");
                        });
                    })
                    .WriteLine("if (output is null)")
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Failed to deserialize because cannot be null")
                            .Write(");");
                        });
                    });
                }
                else
                {
                    w.WriteLine(w =>
                     {
                         w.Write(result.GenericSymbol!.Name)
                         .Write(" output;");
                     })
                    .WriteLine(w =>
                    {
                        w.Write("output = await global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.SystemTextJsonStrings.DeserializeObjectAsync<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">(_text);");
                    });
                }
                w.WriteLine("return output;");
            });
        }
        else if (result.DocumentCategory == EnumDocumentCategory.ListAccess)
        {
            w.WriteLine(w =>
            {
                w.Write("private protected async Task<global::CommonBasicLibraries.CollectionClasses.BasicList<")
                .Write(result.GenericSymbol!.Name)
                .Write(">> GetDocumentsAsync()");
            })
            .WriteCodeBlock(w =>
            {
                w.WriteLine("string data = await _context!.GetDocumentAsync();")
                .WriteLine("if (string.IsNullOrWhiteSpace(data))")
                .WriteCodeBlock(w =>
                {
                    w.WriteLine("return [];");
                });
                if (result.SerializeOption == EnumSourceGeneratedSerializeOptions.RegisterSingleInstance)
                {
                    w.WriteLine(w =>
                    {
                        w.Write("if (global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<global::CommonBasicLibraries.CollectionClasses.BasicList<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">>.MasterContext is null)");
                    })
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Requires custom serialization to be aot compatible")
                            .Write(");");
                        });
                    })
                    .WriteLine(w =>
                    {
                        w.Write("global::CommonBasicLibraries.CollectionClasses.BasicList<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">? output = default;");
                    })
                    .WriteLine("await Task.Run(() =>")
                    .WriteLambaBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("output = global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<global::CommonBasicLibraries.CollectionClasses.BasicList<")
                            .Write(result.GenericSymbol!.Name)
                            .Write(">>.MasterContext.Deserialize(data);");
                        });
                    })
                    .WriteLine("if (output is null)")
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Failed to deserialize because cannot be null")
                            .Write(");");
                        });
                    });
                }
                else
                {
                    w.WriteLine(w =>
                    {
                        w.Write("global::CommonBasicLibraries.CollectionClasses.BasicList<")
                        .Write(result.GenericSymbol!.Name)
                        .Write("> output;");
                    })
                    .WriteLine(w =>
                    {
                        w.Write("output = await global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.SystemTextJsonStrings.DeserializeObjectAsync<global::CommonBasicLibraries.CollectionClasses.BasicList<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">>(data);");
                    });
                }
                w.WriteLine("return output;");
            });
        }

        return w;
    }
    public static ICodeBlock PopulateUpsertRecords(this ICodeBlock w, ResultsModel result)
    {
        if (result.DocumentCategory == EnumDocumentCategory.ObjectAccess)
        {
            w.WriteLine(w =>
            {
                w.Write("private protected async Task UpsertRecordsAsync(")
                .Write(result.GenericSymbol!.Name)
                .Write(" payLoad)");
            })
            .WriteCodeBlock(w =>
            {
                if (result.SerializeOption == EnumSourceGeneratedSerializeOptions.RegisterSingleInstance)
                {
                    w.WriteLine(w =>
                    {
                        w.Write("if (global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">.MasterContext is null)");
                    })
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Requires custom serialization to be aot compatible")
                            .Write(");");
                        });
                    })
                    .WriteLine(w =>
                    {
                        w.Write("string content = ")
                        .AppendDoubleQuote("")
                        .Write(";");
                    })
                    .WriteLine("await Task.Run(() =>")
                    .WriteLambaBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("content = global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<")
                            .Write(result.GenericSymbol!.Name)
                            .Write(">.MasterContext.Serialize(payLoad);");
                        });
                    })
                    .WriteLine(w =>
                    {
                        w.Write("if (content == ")
                        .AppendDoubleQuote("")
                        .Write(")");
                    })
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Content cannot be blank")
                            .Write(");");
                        });
                    });
                }
                else
                {
                    w.WriteLine("string content = await global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.SystemTextJsonStrings.SerializeObjectAsync(payLoad);");
                }
                w.WriteLine("await _context!.UpsertDocumentAsync(content);");
            });

        }
        else if (result.DocumentCategory == EnumDocumentCategory.ListAccess)
        {

            w.WriteLine(w =>
            {
                w.Write("private protected async Task UpsertRecordsAsync(global::CommonBasicLibraries.CollectionClasses.BasicList<")
                .Write(result.GenericSymbol!.Name)
                .Write("> list)");
            }).WriteCodeBlock(w =>
            {
                if (result.SerializeOption == EnumSourceGeneratedSerializeOptions.RegisterSingleInstance)
                {
                    w.WriteLine(w =>
                    {
                        w.Write("if (global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<global::CommonBasicLibraries.CollectionClasses.BasicList<")
                        .Write(result.GenericSymbol!.Name)
                        .Write(">>.MasterContext is null)");
                    });
                    w.WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Requires custom serialization to be aot compatible")
                            .Write(");");
                        });
                    })
                    .WriteLine(w =>
                    {
                        w.Write("string content = ")
                        .AppendDoubleQuote("")
                        .Write(";");
                    })
                    .WriteLine("await Task.Run(() =>")
                    .WriteLambaBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("content = global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.CustomSerializeHelpers<global::CommonBasicLibraries.CollectionClasses.BasicList<")
                            .Write(result.GenericSymbol!.Name)
                            .Write(">>.MasterContext.Serialize(list);");
                        });
                    })
                    .WriteLine(w =>
                    {
                        w.Write("if (content == ")
                        .AppendDoubleQuote("")
                        .Write(")");
                    })
                    .WriteCodeBlock(w =>
                    {
                        w.WriteLine(w =>
                        {
                            w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                            .AppendDoubleQuote("Content cannot be blank")
                            .Write(");");
                        });
                    });
                }
                else
                {
                    w.WriteLine("string content = await global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.SystemTextJsonStrings.SerializeObjectAsync(list);");
                }
                w.WriteLine("await _context!.UpsertDocumentAsync(content);");
            });
        }
        return w;
    }
    private static string GetSimpleType(this EnumSimpleCategory category, ITypeSymbol genericSymbol)
    {
        if (category == EnumSimpleCategory.CustomEnum || category == EnumSimpleCategory.StandardEnum)
        {
            return genericSymbol.Name;
        }
        if (category == EnumSimpleCategory.Int)
        {
            return "int";
        }
        if (category == EnumSimpleCategory.String)
        {
            return "string";
        }
        if (category == EnumSimpleCategory.Bool)
        {
            return "bool";
        }
        if (category == EnumSimpleCategory.Char)
        {
            return "char";
        }
        if (category == EnumSimpleCategory.DateOnly)
        {
            return "DateOnly";
        }
        if (category == EnumSimpleCategory.DateTime)
        {
            return "DateTime";
        }
        if (category == EnumSimpleCategory.Decimal)
        {
            return "decimal";
        }
        if (category == EnumSimpleCategory.Double)
        {
            return "double";
        }
        if (category == EnumSimpleCategory.Float)
        {
            return "float";
        }
        return "";
    }
    public static ICodeBlock PopulateGetData(this ICodeBlock w, ResultsModel result)
    {
        string simpleType;
        simpleType = result.SimpleCategory.GetSimpleType(result.GenericSymbol!);
        w.WriteLine(w =>
        {
            w.Write("private protected async Task<")
            .Write(simpleType);
            if (result.Nullable)
            {
                w.Write("?");
            }
            w.Write("> GetDataAsync()");
        })
        .WriteCodeBlock(w =>
        {
            w.WriteLine("string data = await _context!.GetDocumentAsync();")
            .WriteLine("if (string.IsNullOrWhiteSpace(data))")
            .WriteCodeBlock(w =>
            {
                if (result.SimpleCategory != EnumSimpleCategory.CustomEnum)
                {
                    w.WriteLine(w =>
                    {
                        w.Write("await SaveDataAsync(")
                        .Write(result.DefaultValue)
                        .Write(");");
                    })
                    .WriteLine(w =>
                    {
                        w.Write("return ")
                        .Write(result.DefaultValue)
                        .Write(";");
                    });
                }
                else
                {
                    w.WriteLine(w =>
                    {
                        w.Write("await _context.UpsertDocumentAsync(")
                            .Write(result.DefaultValue)
                            .Write(");");
                    })
                    .WriteLine(w =>
                    {
                        w.Write("return ")
                        .Write(result.GenericSymbol!.Name)
                        .Write(".FromName(")
                        .Write(result.DefaultValue)
                        .Write(");");
                    });
                }
            });
            if (result.SimpleCategory == EnumSimpleCategory.String)
            {
                w.WriteLine("return data;");
            }
            else if (result.SimpleCategory != EnumSimpleCategory.CustomEnum && result.SimpleCategory != EnumSimpleCategory.StandardEnum)
            {
                w.WriteLine(w =>
                {
                    w.Write("bool rets = ")
                    .Write(simpleType)
                    .Write(".TryParse(data, out ")
                    .Write(simpleType)
                    .Write(" output);");
                })
                .WriteLine("if (rets == false)")
                .WriteCodeBlock(w =>
                {
                    w.WriteLine(w =>
                    {
                        w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                        .AppendDoubleQuote(w =>
                        {
                            w.Write("Failed to parse ")
                            .Write(result.GenericSymbol!.Name);
                        }).Write(");");
                    });
                });
                w.WriteLine("return output;");
            }
            else if (result.SimpleCategory == EnumSimpleCategory.StandardEnum)
            {
                w.WriteLine(w =>
                {
                    w.Write("bool rets = Enum.TryParse(data, out ")
                    .Write(result.GenericSymbol!.Name)
                    .Write(" output);");
                })
                .WriteLine("if (rets == false)")
                .WriteCodeBlock(w =>
                {
                    w.WriteLine(w =>
                    {
                        w.Write("throw new global::CommonBasicLibraries.BasicDataSettingsAndProcesses.CustomBasicException(")
                        .AppendDoubleQuote(w =>
                        {
                            w.Write("Failed to parse ")
                            .Write(result.GenericSymbol!.Name);
                        }).Write(");");
                    });
                });
                w.WriteLine("return output;");
            }
            else if (result.SimpleCategory == EnumSimpleCategory.CustomEnum)
            {
                w.WriteLine(w =>
                {
                    w.Write(result.GenericSymbol!.Name)
                    .Write(" output = ")
                    .Write(result.GenericSymbol!.Name)
                    .Write(".FromName(data);");
                });
                w.WriteLine("return output;");
            }
        });
        return w;
    }
    public static ICodeBlock PopulateSaveData(this ICodeBlock w, ResultsModel result)
    {
        string simpleType;
        simpleType = result.SimpleCategory.GetSimpleType(result.GenericSymbol!);
        w.WriteLine(w =>
        {
            w.Write("protected async Task SaveDataAsync(")
            .Write(simpleType);
            if (result.Nullable)
            {
                w.Write("?");
            }
            w.Write(" payLoad)");
        })
        .WriteCodeBlock(w =>
        {
            if (result.SimpleCategory != EnumSimpleCategory.CustomEnum)
            {
                w.WriteLine("string content = payLoad!.ToString()!;");
            }
            else
            {
                w.WriteLine("string content = payLoad.Name;");
            }
            w.WriteLine("await _context!.UpsertDocumentAsync(content);");
        });
        return w;
    }
}