using System.Text;
using AOCHelper;

namespace AOC2024;

public class Day09(int year) : AOC_DayBase(year, 9)
{
    private class MemoryBlock
    {
        public bool IsFile => FileId != -1;

        public int FileId { get; set; }

        public int Size { get; set; }
    }

    protected override void SolveA_Internal(string input)
    {
        var diskMap = Parse(input);

        while (true)
        {
            var emptyBlockIdx = diskMap.FindIndex(block => !block.IsFile);

            if (emptyBlockIdx == -1) break;

            var dataBlockIdx = diskMap.FindLastIndex(
                diskMap.Count - 1,
                diskMap.Count - 1 - emptyBlockIdx,
                (block) => block.IsFile);

            if (dataBlockIdx == -1) break;

            var emptyBlock = diskMap[emptyBlockIdx];
            var dataBlock = diskMap[dataBlockIdx];

            var blocksToMove = Math.Min(emptyBlock.Size, dataBlock.Size);

            MoveMemory(diskMap, dataBlockIdx, emptyBlockIdx, blocksToMove);
        }

        var checksum = CalculateChecksum(diskMap);
        AOC_Logger.Display("Checksum: " + checksum);
    }

    protected override void SolveB_Internal(string input)
    {
        var diskMap = Parse(input);

        var filesToMove = new List<MemoryBlock>();
        for (var i = diskMap.Count - 1; i >= 0; i--)
        {
            if (diskMap[i].IsFile) filesToMove.Add(diskMap[i]);
        }

        foreach (var fileToMove in filesToMove)
        {
            var blockIdx = diskMap.IndexOf(fileToMove);

            var emptyIdx = diskMap
                .FindIndex(
                    0,
                    blockIdx,
                    block => !block.IsFile && block.Size >= fileToMove.Size);

            if (emptyIdx == -1) continue;

            MoveMemory(diskMap, blockIdx, emptyIdx, fileToMove.Size);
        }

        var checksum = CalculateChecksum(diskMap);
        AOC_Logger.Display("Checksum: " + checksum);
    }

    private static void MoveMemory(
        List<MemoryBlock> memory,
        int sourceIdx,
        int targetIdx,
        int size)
    {
        var sourceBlock = memory[sourceIdx];
        var targetBlock = memory[targetIdx];

        var prevIsEmpty = sourceIdx > 0 && !memory[sourceIdx - 1].IsFile;
        var nextIsEmpty = sourceIdx < memory.Count - 1 && !memory[sourceIdx + 1].IsFile;

        var blockIsFullyMoved = size == sourceBlock.Size;

        if (blockIsFullyMoved && prevIsEmpty && nextIsEmpty)
        {
            memory[sourceIdx - 1].Size += size + memory[sourceIdx + 1].Size;
            memory.RemoveAt(sourceIdx + 1);
        }
        else if (nextIsEmpty)
        {
            memory[sourceIdx + 1].Size += size;
        }
        else if (prevIsEmpty)
        {
            memory[sourceIdx - 1].Size += size;
        }
        else
        {
            memory.Insert(sourceIdx + 1, new MemoryBlock
            {
                FileId = -1,
                Size = size,
            });
        }

        sourceBlock.Size -= size;
        if (sourceBlock.Size == 0) memory.RemoveAt(sourceIdx);

        if (targetBlock.Size == size)
        {
            targetBlock.FileId = sourceBlock.FileId;
        }
        else
        {
            targetBlock.Size -= size;
            if (targetBlock.Size == 0) memory.RemoveAt(targetIdx);

            memory.Insert(targetIdx, new MemoryBlock
            {
                FileId = sourceBlock.FileId,
                Size = size,
            });
        }
    }

    private static long CalculateChecksum(List<MemoryBlock> diskMap)
    {
        var checksum = 0L;
        var pos = 0;

        for (var i = 0; i < diskMap.Count; i++)
        {
            var block = diskMap[i];

            if (block.IsFile)
            {
                for (var j = 0; j < block.Size; j++)
                {
                    checksum += pos * block.FileId;
                    pos++;
                }
            }
            else
            {
                pos += block.Size;
            }
        }

        return checksum;
    }

    private static void PrintMemoryMap(List<MemoryBlock> memory)
    {
        var sb = new StringBuilder();

        foreach (var block in memory)
        {
            var c = block.IsFile ? (char)(block.FileId + '0') : '.';

            for (var i = 0; i < block.Size; i++)
            {
                sb.Append(c);
            }
        }

        AOC_Logger.Display(sb.ToString());
    }

    private static List<MemoryBlock> Parse(string input)
    {
        var diskMap = new List<MemoryBlock>();

        var isFile = true;
        var fileIdx = 0;

        foreach (var c in input)
        {
            var size = c - '0';

            var id = isFile ? fileIdx++ : -1;

            if (isFile || size > 0)
            {
                diskMap.Add(new MemoryBlock
                {
                    FileId = id,
                    Size = size,
                });
            }

            isFile = !isFile;
        }

        return diskMap;
    }

    protected override string TestInput => """
                                           2333133121414131402
                                           """;
}