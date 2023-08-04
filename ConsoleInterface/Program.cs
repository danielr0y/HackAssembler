using ConsoleInterface;
using HackAssembler;

try
{
    string filename = args[0];
    string filenameWithoutExtension = filename.Split(".", 2)[0];
    string outputFileName = $"{filenameWithoutExtension}.hack";

    using FileStream infs = new FileStream(filename, FileMode.Open, FileAccess.Read);
    using StreamReader sr = new StreamReader(infs);

    using FileStream outfs = new FileStream(outputFileName, FileMode.Create);
    using StreamWriter sw = new StreamWriter(outfs);

    var assembler = new Assembler(new Parser(new StreamReaderToReadable(sr)));

    assembler.Translate(new StreamWriterToWritable(sw));
}
catch (Exception e)
{
    Console.WriteLine(e);
}