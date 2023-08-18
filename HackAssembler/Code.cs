namespace HackAssembler;

public abstract class Code
{
    protected readonly string Symbol;

    protected Code(string symbol)
    {
        Symbol = symbol;
    }

    public abstract string ToBinary();
}

public class Dest : Code
{
    public Dest(string symbol = "null") : base(symbol) {}

    public override string ToBinary()
    {
        return new Dictionary<string, string>()
        {
            { "null", "000" },
            { "A", "100" },
            { "D", "010" },
            { "M", "001" },
            { "AD", "110" },
            { "DA", "110" },
            { "AM", "101" },
            { "MA", "101" },
            { "DM", "011" },
            { "MD", "011" },
            { "ADM", "111" },
            { "AMD", "111" },
            { "DAM", "111" },
            { "DMA", "111" },
            { "MAD", "111" },
            { "MDA", "111" },
        }[Symbol];
    }
}

public class Comp : Code
{
    public Comp(string symbol) : base(symbol) {}

    public override string ToBinary()
    {
        return new Dictionary<string, string>()
        {
            { "0", "0101010" },
            { "1", "0111111" },
            { "-1", "0111010" },
            { "A", "0110000" },
            { "D", "0001100" },
            { "M", "1110000" },
            { "!A", "0110001" },
            { "!D", "0001101" },
            { "!M", "1110001" },
            { "-A", "0110011" },
            { "-D", "0001111" },
            { "-M", "1110011" },
            { "A+1", "0110111" },
            { "D+1", "0011111" },
            { "M+1", "1110111" },
            { "A-1", "0110010" },
            { "D-1", "0001110" },
            { "M-1", "1110010" },
            { "D+A", "0000010" },
            { "A+D", "0000010" },
            { "D+M", "1000010" },
            { "M+D", "1000010" },
            { "D-A", "0010011" },
            { "D-M", "1010011" },
            { "A-D", "0000111" },
            { "M-D", "1000111" },
            { "D&A", "0000000" },
            { "D&M", "1000000" },
            { "D|A", "0010101" },
            { "D|M", "1010101" }
        }[Symbol];
    }
}

public class Jump : Code
{
    public Jump(string symbol = "null") : base(symbol) {}

    public override string ToBinary()
    {
        return new Dictionary<string, string>()
        {
            { "null", "000" },
            { "JGT", "001" },
            { "JEQ", "010" },
            { "JGE", "011" },
            { "JLT", "100" },
            { "JNE", "101" },
            { "JLE", "110" },
            { "JMP", "111" }
        }[Symbol];
    }
}