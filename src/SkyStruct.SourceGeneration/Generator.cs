using System.Text;
using Microsoft.CodeAnalysis;

namespace SkyStruct.SourceGeneration;

[Generator]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // additional files
        var textFiles = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".skystruct"));
        
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("Verify.g.cs", """
                                         namespace Generated
                                         {
                                             public static class GeneratorCheck
                                             {
                                                 public const string Status = "Compiler lib loaded";
                                             }
                                         }
                                         """);
        });

        //var namesAndContents = textFiles.Select((text, ct) =>
        //{
        //    
        //    //Stupid, should just come as stream, figure out if slow
        //    var byteArray = Encoding.UTF8.GetBytes(text.GetText(ct)!.ToString());
        //    var fileStream = new MemoryStream(byteArray);
        //    
        //    var lexer = new Lexer.Lexer(new StreamReader(fileStream!), ct);
        //    var tokens = lexer.Tokenize().ToList();

        //    var parser = new Parser.Parser(tokens);
        //    var ast = parser.Parse();

        //    var codeGenerator = new CodeGenerator.Builder(ast, "HolyNamespace");
        //    var code = codeGenerator.BuildFile();

        //    //Assure namespace and filename are uppercase
        //    var name = Path.GetFileNameWithoutExtension(text.Path);
        //    name = char.ToUpper(name[0]) + name.Substring(1);
        //    
        //    return (
        //        name: name,
        //        content: code 
        //    );

        //});


        //// per-file output
        //context.RegisterSourceOutput(namesAndContents, (spc, item) =>
        //{
        //    spc.AddSource($"Skystruct.{item.name}.g.cs", $$"""
        //                                                   namespace {{item.name}};
        //                                                   
        //                                                   {{item.content}}
        //                                                   
        //                                                   """);
        //});
    }
}