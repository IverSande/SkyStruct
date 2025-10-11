namespace SkyStruct.Parser;

public abstract class AstNode;

public class TypeNode : AstNode
{
    public required string Name { get; set; }
    public List<PropertyNode> Properties { get; set; } = []; 
    public string Parent { get; set; } = null!;
}

public class PropertyNode : AstNode
{
    public required string Name { get; set; }
    public required string DataType { get; set; }
}
