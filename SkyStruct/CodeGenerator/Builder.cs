using System.Text;
using SkyStruct.Parser;

namespace SkyStruct.CodeGenerator;

public class Builder
{
    private readonly StringBuilder _builder;
    private readonly IEnumerable<AstNode> _astNodes;
    private readonly string _namespace;
    
    public Builder(IEnumerable<AstNode> nodes, string @namespace)
    {
        _builder = new StringBuilder();
        _astNodes = nodes;
        _namespace = @namespace;
    }

    public string BuildFile()
    {
        _builder.BuildNamespace(_namespace);
        foreach (var node in _astNodes.Where(node => node is TypeNode))
        {
            _builder.BuildTypeNode((TypeNode)node);
        }

        return _builder.ToString();
    }
    
}