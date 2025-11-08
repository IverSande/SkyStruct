namespace SkyStruct.Lsp.LspTypes;

internal abstract record LspAny
{
    public LspNull? Null { get; init; }
    public LspBoolean? Boolean { get; init; }
    public LspString? String { get; init; }
    public LspObject? Object { get; init; }
    public LspArray? Array { get; init; }
    public LspInteger? Integer { get; init; }
    public LspDecimal? Decimal { get; init; }
    public LspUnsignedInteger? UnsignedInteger { get; init; }
}

internal record LspString(string Value);
internal record LspObject(Dictionary<string, LspAny> Value);
internal record LspArray(IEnumerable<LspAny> Value);
internal record LspInteger(int Value);
internal record LspUnsignedInteger(uint Value);
internal record LspDecimal(decimal Value);
internal record LspBoolean(bool Value);
internal record LspNull();