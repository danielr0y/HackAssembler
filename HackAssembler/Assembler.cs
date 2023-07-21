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

/**
 * Hack assembly programs are allowed to use symbolic labels before the symbols are defined.
 * This implementation employs a two-pass strategy to support this requirement
 * where labels are found and their line numbers added to the symbol table during the first pass
 * and instructions are translated during the second.
 */
public class Assembler
{
    private readonly AssemblyParser _parser;
    private readonly SymbolTable _symbolTable;
    private Int16 _nextAvailableAddress;

    public Assembler(AssemblyParser parser)
    {
        _parser = parser;
        _symbolTable = new SymbolTable(new Dictionary<string, Int16>()
        {
            { "R0", 0 },
            { "R1", 1 },
            { "R2", 2 },
            { "R3", 3 },
            { "R4", 4 },
            { "R5", 5 },
            { "R6", 6 },
            { "R7", 7 },
            { "R8", 8 },
            { "R9", 9 },
            { "R10", 10 },
            { "R11", 11 },
            { "R12", 12 },
            { "R13", 13 },
            { "R14", 14 },
            { "R15", 15 },
            { "SP", 0 },
            { "LCL", 1 },
            { "ARG", 2 },
            { "THIS", 3 },
            { "THAT", 4 },
            { "SCREEN", 16384 },
            { "KBD", 24576 },
        });
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
