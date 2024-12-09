using System.Text;
using AOCHelper;

namespace AOC2024;

public class Day09(int year) : AOC_DayBase(year, 9)
{
    private class MemoryBlock
    {
        public bool IsFile { get; init; }

        public int FileId { get; init; }

        public int Size { get; set; }
    }

    protected override void SolveA_Internal(string input)
    {
        var diskMap = Parse(input);

        // PrintMemoryMap(diskMap);

        while (true)
        {
            var movedAnyBlock = false;

            // find an empty block & try to move any file block
            for (var i = 0; i < diskMap.Count; i++)
            {
                if (diskMap[i].IsFile) continue;
                var emptyBlock = diskMap[i];
                
                for (var j = diskMap.Count - 1; j > i; j--)
                {
                    if (!diskMap[j].IsFile) continue;
                    var dataBlock = diskMap[j];
                    
                    var blocksToMove = Math.Min(emptyBlock.Size, dataBlock.Size);
                    
                    diskMap.Insert(j + 1, new MemoryBlock
                    {
                        IsFile = false,
                        FileId = -1,
                        Size = blocksToMove,
                    });
                    
                    dataBlock.Size -= blocksToMove;
                    if (dataBlock.Size == 0) diskMap.RemoveAt(j);

                    emptyBlock.Size -= blocksToMove;
                    if (emptyBlock.Size == 0) diskMap.RemoveAt(i);

                    diskMap.Insert(i, new MemoryBlock
                    {
                        FileId = dataBlock.FileId,
                        IsFile = true,
                        Size = blocksToMove,
                    });

                    movedAnyBlock = true;
                    break;
                }

                if (movedAnyBlock) break;
            }

            if (movedAnyBlock)
            {
                // PrintMemoryMap(diskMap);
            }
            else
            {
                break;
            }
        }

        // PrintMemoryMap(diskMap);

        var checksum = CalculateChecksum(diskMap);
        AOC_Logger.Display("Checksum: " + checksum);
    }
    
    protected override void SolveB_Internal(string input)
    {
        var diskMap = Parse(input);

        // PrintMemoryMap(diskMap);
        
        var movedFiles = new HashSet<int>();
        
        while (true)
        {
            var movedAnyBlock = false;

            // find an empty block & try to move any file block
            for (var i = 0; i < diskMap.Count; i++)
            {
                if (diskMap[i].IsFile) continue;
                
                var emptyBlock = diskMap[i];

                for (var j = diskMap.Count - 1; j > i; j--)
                {
                    if (!diskMap[j].IsFile) continue;
                    
                    var dataBlock = diskMap[j];

                    if (movedFiles.Contains(dataBlock.FileId)) continue;
                    
                    // block does not fit!
                    if (dataBlock.Size > emptyBlock.Size) continue;

                    movedFiles.Add(dataBlock.FileId);
                    
                    var blocksToMove = dataBlock.Size;
                   
                    diskMap.Insert(j + 1, new MemoryBlock
                    {
                        IsFile = false,
                        FileId = -1,
                        Size = blocksToMove,
                    });
                    
                    diskMap.RemoveAt(j);

                    emptyBlock.Size -= blocksToMove;
                    if (emptyBlock.Size == 0) diskMap.RemoveAt(i);

                    diskMap.Insert(i, dataBlock);

                    movedAnyBlock = true;
                    break;
                }

                if (movedAnyBlock) break;
            }

            if (movedAnyBlock)
            {
                // PrintMemoryMap(diskMap);
            }
            else
            {
                break;
            }
        }
        
        // PrintMemoryMap(diskMap);
        
        var checksum = CalculateChecksum(diskMap);
        AOC_Logger.Display("Checksum: " + checksum);
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
            var c = block.IsFile ? (char) (block.FileId + '0') : '.';

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

            diskMap.Add(new MemoryBlock
            {
                FileId = id,
                IsFile = isFile,
                Size = size,
            });

            isFile = !isFile;
        }

        return diskMap;
    }

    protected override string TestInput => """
                                           2333133121414131402
                                           """;
}