using System.Text;
using SkyStruct.Parser;

namespace SkyStruct.CodeGenerator;

public static class BuilderExtensions
{
    extension(StringBuilder builder)
    {
        public StringBuilder BuildNamespace(string @namespace) => 
            builder
                .Append("namespace " + @namespace + ";")
                .AppendLine();

        public StringBuilder BuildTypeNode(TypeNode type)
        {
            builder.Append("public class " + type.Name);
            if (type.InheritedType is not null)
            {
                builder.Append(" : " + type.InheritedType);
            }
            builder.AppendLine().Append('{').AppendLine();
        
            foreach (var property in type.Properties)
            {
                builder.Append("  public " + property.ToDotNetDataType() + " " + property.Name + " { get; set; }").AppendLine();
            }
            builder.Append('}').AppendLine();

            return builder;
        }
    }

    private static string ToDotNetDataType(this PropertyNode type) => 
        ToConstrainedDotNetDataType(type.DataType, type.Constraints);

    extension(string type)
    {
        private string ToConstrainedDotNetDataType(List<Constraint> constraints)
        {
            var dotnetType = type.ToDotNetDataType(constraints.FirstOrDefault(c => c == Constraint.Decimal));
        
            if (constraints.Contains(Constraint.List))
                dotnetType = dotnetType.AddList();
            if (constraints.Contains(Constraint.Required))
                dotnetType = dotnetType.AddRequired();
            if (constraints.Contains(Constraint.Optional))
                dotnetType = dotnetType.AddOptional();

            return dotnetType;
        }

        private string AddList() =>
            "List<" + type + ">";

        private string AddRequired() =>
            "required" + type;

        private string AddOptional() =>
            type + "?";

        private string ToDotNetDataType(Constraint? constraint = null) => (type, constraint) switch
        {
            ("Text", _) => "string",
            ("Number", Constraint.Decimal) => "decimal",
            ("Number", _) => "long",
            ("Bool", _) => "bool",
            _ => type
        };
    }
}