using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.dgl;
public readonly struct Board
{
    private static Random Random = new();
    private readonly Cell[,] _cells;
    public int Width => _cells.GetLength(0);
    public int Height => _cells.GetLength(1);
    public Board(int width, int height, float density = 0)
    {
        _cells = new Cell[width, height];
        foreach((int x, int y) in Coords)
        {
            _cells[x, y] = (density > 0 && Random.NextDouble() < density ? new((byte)Random.Next(0, 255), 
                                                                               (byte)Random.Next(0, 255), 
                                                                               (byte)Random.Next(0, 255)) 
                                                                         : new(0, 0, 0));
        }
    }
    public Board(Board previous)
    {
        _cells = new Cell[previous.Width, previous.Height];
        foreach((int x, int y) in Coords)
        {
            _cells[x, y] = new(previous[x, y], previous.NeighborsOf(x, y));
        }
    }
    public bool InBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
    public Cell this[int x, int y] => InBounds(x, y) ? _cells[x, y] : throw new Exception($"{x} and/or {y} is not in the range of board {ToString()}!");
    public IEnumerable<Cell> NeighborsOf(int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int xx = x + i, yy = y + j;
                if (i == 0 && j == 0 || !InBounds(xx, yy))
                    continue;
                yield return _cells[xx, yy];
            }
        }
    }
    public IEnumerable<(int x, int y)> Coords
    {
        get
        {
            for(int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                    yield return (x, y);
            }
        }
    }
    public IEnumerable<Cell> Cells
    {
        get
        {
            foreach ((int x, int y) in Coords)
                yield return this[x, y];
        }
    }
    public IEnumerable<Cell> Row(int y)
    {
        for(int x = 0; x < Width; x++) yield return this[x, y];
    }
    public IEnumerable<IEnumerable<Cell>> Rows
    {
        get
        {
            for (int y = 0; y < Height; y++)
                yield return Row(y);
        }
    }
    public override string ToString()
    {
        return $"Board {Width}x{Height}";
    }
    public byte[] BmpBytes
    {
        get
        {
            // header info from https://en.wikipedia.org/wiki/BMP_file_format#Example_1
            int bmpHeaderSize = 14, 
                dibHeaderSize = 40, 
                imageSize = Height * (Width * 3 + (Width * 3) % 4), 
                totalSize = bmpHeaderSize + dibHeaderSize + imageSize;
            byte[] result = new byte[totalSize];
            int index = 0;
            void write(params byte[] bs)
            {
                foreach (byte b in bs)
                {
                    if (index >= result.Length)
                        throw new Exception($"Attempting to write {b} at index {index}, but result.Length = {result.Length}!");
                    result[index] = b;
                    index++;
                }
            }
            void writeInt(int n, int bytes)
            {
                int ct = 0;
                foreach(byte b in n.InBytesMsb())
                {
                    if (ct >= bytes)
                        break;
                    write(b);
                    ct++;
                }
                pad(bytes - ct);
            }
            void pad(int n)
            {
                for (int _ = 0; _ < n; _++)
                    write(0);
            }
            // BMP header
            //  0
            foreach (char c in "BM")
                write((byte)c);
            //  2
            writeInt(totalSize, 4);
            //  6
            pad(4);
            //  10
            writeInt(bmpHeaderSize + dibHeaderSize, 4);
            // DIB header
            //  14
            writeInt(dibHeaderSize, 4);
            //  18
            writeInt(Width, 4);
            //  22
            writeInt(Height, 4);
            //  26
            writeInt(1, 2);
            //  28
            writeInt(24, 2);
            //  30
            pad(4);
            //  34
            writeInt(imageSize, 4);
            //  38
            writeInt(2835, 4);
            //  42
            writeInt(2835, 4);
            //  46
            pad(8);
            //  54
            foreach (IEnumerable<Cell> row in Rows)
            {
                int remainder = 0;
                foreach(Cell c in row)
                {
                    write(c.R);
                    write(c.G);
                    write(c.B);
                    remainder = (remainder + 3) % 4;
                }
                pad(remainder);
            }
            return result;
        }
    }
}