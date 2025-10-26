using System.Text;
using Microsoft.Extensions.Primitives;
using SkyStruct.Parser;

namespace SkyStruct.CodeGenerator;

public static class BuilderExtensions
{
    public static StringBuilder BuildNamespace(this StringBuilder builder, string @namespace) => 
        builder
            .Append("namespace " + @namespace + ";")
            .AppendLine();

    public static StringBuilder BuildTypeNode(this StringBuilder builder, TypeNode type)
    {
        builder.Append("public class " + type.Name);
        if (type.InheritedType is not null)
        {
            builder.Append(" : " + type.InheritedType);
        }
        builder.AppendLine().Append('{').AppendLine();
        
        foreach (var property in type.Properties)
        {
            builder.Append("  public " + property.DataType.ToDotNetDataType() + " " + property.Name + " { get; set; }").AppendLine();
        }
        builder.Append('}').AppendLine();

        return builder;
    }

    private static string ToDotNetDataType(this string type) => type switch
    {
        "Text" => "string",
        "Number" => "int",
        "Decimal" => "decimal",
        _ => type
    };
}