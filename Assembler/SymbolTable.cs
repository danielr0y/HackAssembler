namespace Assembler;

public class SymbolTable
{
    private readonly Dictionary<string, Int16> _table;

    public SymbolTable(Dictionary<string,Int16> defaultValues)
    {
        _table = defaultValues;
    }

    public Int16 GetAddress(string symbol)
    {
        return _table[symbol];
    }

    public bool Contains(string symbol)
    {
        return _table.ContainsKey(symbol);
    }

    public void AddEntry(string symbol, Int16 address)
    {
        _table.Add(symbol, address);
    }
}