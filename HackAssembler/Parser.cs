namespace HackAssembler;

public interface Readable
{
    bool HasMoreLines();
    string ReadLine();
    void Rewind();
}

public class Parser : AssemblyParser
{
    enum InstructionType { A_INSTRUCTION, C_INSTRUCTION, L_INSTRUCTION, SKIPPABLE }

    private readonly Readable _source;

    public Parser(Readable source)
    {
        _source = source;
    }

    public Instruction CurrentInstruction { get; private set; }

    public void Rewind()
    {
        _source.Rewind();
    }

    public bool HasMoreLines()
    {
        return _source.HasMoreLines();
    }

    public void Advance()
    {
        do
        {
            string textInstruction = _source.ReadLine();
            CurrentInstruction = ParseInstruction(textInstruction);
        } while (CurrentInstruction is Skippable_Instruction);
    }

    private Instruction ParseInstruction(string instruction)
    {
        var textInstruction = instruction.TrimStart().Split(' ', 2)[0];

        switch (DetermineInstructionType(textInstruction))
        {
            case InstructionType.L_INSTRUCTION:
            {
                var symbol = textInstruction.Substring(1, textInstruction.Length - 2);
                return new L_Instruction(textInstruction, symbol);
            }
            case InstructionType.A_INSTRUCTION:
            {
                var symbol = textInstruction.Substring(1);

                if (Int16.TryParse(symbol, out Int16 constant))
                {
                    return new A_Instruction_Constant(textInstruction, constant);
                }

                return new A_Instruction_Symbolic(textInstruction, symbol);
            }
            case InstructionType.C_INSTRUCTION:
            {
                string comp = textInstruction;
                Dest? dest = null;
                Jump? jump = null;

                if (textInstruction.Contains('='))
                {
                    var destRest = textInstruction.Split('=', 2);
                    dest = new Dest(destRest[0]);
                    comp = destRest[1];
                }

                if (textInstruction.Contains(';'))
                {
                    var compJump = textInstruction.Split(';', 2);
                    comp = compJump[0];
                    jump = new Jump(compJump[1]);
                }

                return new C_Instruction(textInstruction, new Comp(comp), dest ?? new Dest(), jump ?? new Jump());
            }
            case InstructionType.SKIPPABLE:
            default:
            {
                return new Skippable_Instruction(textInstruction);
            }
        }
    }

    private InstructionType DetermineInstructionType(string textInstruction)
    {
        var instructionTypes = new Dictionary<char, InstructionType>()
        {
            { '@', InstructionType.A_INSTRUCTION },
            { '(', InstructionType.L_INSTRUCTION },
            { 'C', InstructionType.C_INSTRUCTION },
            { '/', InstructionType.SKIPPABLE },
        };

        char firstChar;
        try
        {
            firstChar = textInstruction.ToCharArray()[0];
            if (! instructionTypes.ContainsKey(firstChar))
            {
                firstChar = 'C';
            }
        }
        catch (IndexOutOfRangeException e)
        {
            return InstructionType.SKIPPABLE;
        }

        return instructionTypes[firstChar];
    }
}