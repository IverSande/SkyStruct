namespace SkyStruct.Parser;

public abstract class AstNode;

public class TypeNode : AstNode
{
    public string Name { get; set; }
    public List<PropertyNode> Properties { get; set; } = []; 
    public string? InheritedType { get; set; }
}

public class PropertyNode : AstNode
{
    public string Name { get; set; }
    public string DataType { get; set; }
    public List<Constraint> Constraints { get; set; } = [];
}

public enum Constraint
{
    Required,
    Optional,
    List,
    //More constraints    
}
