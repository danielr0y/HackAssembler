namespace Assembler;

public abstract class Instruction
{
    private readonly string _textInstruction;

    protected Instruction(string textInstruction)
    {
        _textInstruction = textInstruction;
    }
}

public interface ExecutableInstruction
{
    string ToBinary();
}

public class Skippable_Instruction : Instruction
{
    public Skippable_Instruction(string textInstruction) : base(textInstruction) {}
}

public class L_Instruction : Instruction
{
    public L_Instruction(string textInstruction, string symbol) : base(textInstruction)
    {
        Symbol = symbol;
    }

    public string Symbol { get; }
}

public class A_Instruction_Constant : Instruction, ExecutableInstruction
{
    private readonly Int16 _constant;

    public A_Instruction_Constant(string textInstruction, Int16 constant) : base(textInstruction)
    {
        _constant = constant;
    }

    public string ToBinary() => Convert.ToString(_constant, 2).PadLeft(16, '0');
}

public class A_Instruction_Symbolic : Instruction, ExecutableInstruction
{
    public A_Instruction_Symbolic(string textInstruction, string symbol) : base(textInstruction)
    {
        Symbol = symbol;
    }

    public string Symbol { get; }
    private Int16 Address { get; set; }

    public void ResolveSymbols(SymbolTable symbolTable)
    {
        Address = symbolTable.GetAddress(Symbol);
    }

    public string ToBinary() => Convert.ToString(Address ,2).PadLeft(16, '0');
}

public class C_Instruction : Instruction, ExecutableInstruction
{
    private readonly Comp _comp;
    private readonly Dest _dest;
    private readonly Jump _jump;

    public C_Instruction(string textInstruction, Comp comp, Dest dest, Jump jump) : base(textInstruction)
    {
        _comp = comp;
        _dest = dest;
        _jump = jump;
    }

    public string ToBinary() => $"111{_comp.ToBinary()}{_dest.ToBinary()}{_jump.ToBinary()}";
}