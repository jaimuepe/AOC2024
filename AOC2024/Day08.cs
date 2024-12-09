using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day08(int year) : AOC_DayBase(year, 8)
{
    protected override void SolveA_Internal(string input)
    { 
        var antennasByFrequency = Parse(
            input, 
            out var width,
            out var height);

        var antinodeLocations = new HashSet<Vector2i>();

        foreach (var (_, antennas) in antennasByFrequency)
        {
            for (var i = 0; i < antennas.Count; i++)
            {
                for (var j = 0; j < antennas.Count; j++)
                {
                    if (i == j) continue;
                    
                    // get vector between antennas
                    var diff = antennas[j] - antennas[i];
                    
                    var p1 = antennas[i] - diff;
                    if (AOC_Utils.IsInsideBounds(p1, width, height))
                    {
                        antinodeLocations.Add(p1);
                    }

                    var p2 = antennas[j] + diff;
                    if (AOC_Utils.IsInsideBounds(p2, width, height))
                    {
                        antinodeLocations.Add(p2);
                    }
                }
            }
        }
        
        AOC_Logger.Display("Antinode unique locations: " + antinodeLocations.Count);
    }

    protected override void SolveB_Internal(string input)
    {
        var antennasByFrequency = Parse(
            input, 
            out var width,
            out var height);

        var antinodeLocations = new HashSet<Vector2i>();

        foreach (var (_, antennas) in antennasByFrequency)
        {
            for (var i = 0; i < antennas.Count; i++)
            {
                for (var j = 0; j < antennas.Count; j++)
                {
                    if (i == j) continue;
                    
                    // get vector between antennas
                    var diff = antennas[j] - antennas[i];

                    // the antennas themselves create an antinode too
                    var resonancePos = antennas[i];
                    while (AOC_Utils.IsInsideBounds(resonancePos, width, height))
                    {
                        antinodeLocations.Add(resonancePos);
                        resonancePos -= diff;
                    }
                    
                    resonancePos = antennas[j];
                    while (AOC_Utils.IsInsideBounds(resonancePos, width, height))
                    {
                        antinodeLocations.Add(resonancePos);
                        resonancePos += diff;
                    }
                }
            }
        }
        
        AOC_Logger.Display("Antinode unique locations: " + antinodeLocations.Count);
    }

    private static Dictionary<char, List<Vector2i>> Parse(
        string input, 
        out int width, 
        out int height)
    {
        var antennasByFrequency = new Dictionary<char, List<Vector2i>>();

        var lines = AOC_Utils
            .SplitLines(input)
            .ToList();

        height = lines.Count;
        width = lines[0].Length;
        
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var c = lines[i][j];
                if (c == '.') continue;

                if (!antennasByFrequency.TryGetValue(c, out var value))
                {
                    value = [];
                    antennasByFrequency[c] = value;
                }

                value.Add(new Vector2i(j, i));
            }
        }

        return antennasByFrequency;
    }

    protected override string TestInput => """
                                           ............
                                           ........0...
                                           .....0......
                                           .......0....
                                           ....0.......
                                           ......A.....
                                           ............
                                           ............
                                           ........A...
                                           .........A..
                                           ............
                                           ............
                                           """;
}
