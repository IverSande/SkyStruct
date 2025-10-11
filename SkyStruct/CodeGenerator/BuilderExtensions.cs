using System.Text;
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
        builder.Append("public class " + type.Name + "{").AppendLine();
        foreach (var property in type.Properties)
        {
            builder.Append("public " + property.DataType + " " + property.Name + " { get; set; }").AppendLine();
        }
        builder.Append('}').AppendLine();

        return builder;
    } 
}