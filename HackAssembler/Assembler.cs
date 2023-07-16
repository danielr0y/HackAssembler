namespace HackAssembler;


public interface AssemblyParser
{
    bool HasMoreLines();
    void Advance();
    Instruction CurrentInstruction { get; }
    void Rewind();
}

public interface Writable
{
    void WriteLine(string line);
}

public class Assembler
{
    private readonly AssemblyParser _parser;
    private readonly SymbolTable _symbolTable;
    private Int16 _nextAvailableAddress;

    public Assembler(AssemblyParser parser, SymbolTable symbolTable)
    {
        _parser = parser;
        _symbolTable = symbolTable;
        _nextAvailableAddress = 16;
    }

    private Int16 NextAvailableAddress => _nextAvailableAddress++;

    public void Translate(Writable output)
    {
        NoteLabelLineNumbers();
        _parser.Rewind();
        ToBinary(output);
    }

    private void NoteLabelLineNumbers()
    {
        Int16 lineNumber = 0;

        while (_parser.HasMoreLines())
        {
            _parser.Advance(); // sets up the next instruction

            if (_parser.CurrentInstruction is L_Instruction label)
            {
                _symbolTable.AddEntry(label.Symbol, lineNumber);

                continue; // do not increment the line number
            }

            lineNumber++;
        }
    }

    private void ToBinary(Writable output)
    {
        while (_parser.HasMoreLines())
        {
            _parser.Advance(); // sets up the next instruction

            if (_parser.CurrentInstruction is ExecutableInstruction instruction)
            {
                if (instruction is A_Instruction_Symbolic symbolicA)
                {
                    if (!_symbolTable.Contains(symbolicA.Symbol))
                    {
                        _symbolTable.AddEntry(symbolicA.Symbol, NextAvailableAddress);
                    }

                    symbolicA.ResolveSymbols(_symbolTable);
                    instruction = symbolicA;
                }

                output.WriteLine(instruction.ToBinary());
            }
        }
    }
}