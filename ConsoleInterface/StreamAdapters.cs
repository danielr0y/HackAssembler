namespace ConsoleInterface;

using HackAssembler;

public class StreamReaderToReadable : Readable
{
    private readonly StreamReader _reader;
    public StreamReaderToReadable(StreamReader reader)
    {
        _reader = reader;
    }

    public bool HasMoreLines()
    {
        return _reader.Peek() > 0;
    }

    public string ReadLine()
    {
        return _reader.ReadLine() ?? "";
    }

    public void Rewind()
    {
        _reader.DiscardBufferedData();
        _reader.BaseStream.Seek(0, SeekOrigin.Begin);
    }
}

public class StreamWriterToWritable : Writable
{
    private readonly StreamWriter _writer;

    public StreamWriterToWritable(StreamWriter writer)
    {
        _writer = writer;
    }

    public void WriteLine(string line)
    {
        _writer.WriteLine(line);
    }
}