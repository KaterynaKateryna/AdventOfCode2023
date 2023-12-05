namespace AdventOfCode;

public class Day05 : BaseDay
{
    private long[] _seeds;
    private Map[][] _maps;

    public Day05()
    {
        string input = File.ReadAllText(InputFilePath);
        string[] parts = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);

        _seeds = parts[0].Replace("seeds: ", "").Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        _maps = new Map[parts.Length - 1][];
        for (int i = 0; i < parts.Length - 1; ++i)
        {
            string[] lines = parts[i + 1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Map[] map = new Map[lines.Length - 1];
            for(int j = 1; j < lines.Length; ++j)
            {
                long[] numbers = lines[j].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                map[j - 1] = new Map(numbers[0], numbers[1], numbers[2]);
            }

            _maps[i] = map;
        }
    }

    public override ValueTask<string> Solve_1()
    {
        return new (_seeds.Select(s => FindLocation(s)).Min().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private long FindLocation(long seed)
    {
        long value = seed;
        for(int i = 0; i < _maps.Length; ++i)
        {
            value = GetDestination(value, _maps[i]);
        }
        return value;
    }

    private long GetDestination(long source, Map[] maps)
    {
        foreach (Map map in maps)
        {
            if (source >= map.SourceLengthStart && source < (map.SourceLengthStart + map.RangeLength))
            { 
                long diff = source - map.SourceLengthStart;
                return map.DestinationRangeStart + diff;
            }
        }
        return source;
    }

    private record Map(long DestinationRangeStart, long SourceLengthStart, long RangeLength);
}
