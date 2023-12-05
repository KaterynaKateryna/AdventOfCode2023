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

        for (int i = 0; i < _maps.Length; ++i)
        {
            _maps[i] = _maps[i].OrderBy(m => m.SourceRangeStart).ToArray();
        }
    }

    public override ValueTask<string> Solve_1()
    {
        return new (_seeds.Select(s => FindLocation(s)).Min().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        Range[] seeds = new Range[_seeds.Length / 2];
        for (int i = 0; i < _seeds.Length; i += 2)
        {
            seeds[i / 2] = new Range(_seeds[i], _seeds[i + 1]);
        }

        long location = seeds.Select(s => FindLocationRanges(s)).SelectMany(l => l).Min(r => r.Start);

        return new(location.ToString());
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

    private List<Range> FindLocationRanges(Range range)
    {
        List<Range> ranges = [ range ];
        for (int i = 0; i < _maps.Length; ++i)
        {
            List<Range> rangesRes = new List<Range>();
            foreach(Range r in ranges)
            {
                var dest = GetDestinationRanges(r, _maps[i]);
                rangesRes.AddRange(dest);
            }
            ranges = rangesRes;
        }
        return ranges;
    }

    private long GetDestination(long source, Map[] maps)
    {
        foreach (Map map in maps)
        {
            if (source >= map.SourceRangeStart && source < (map.SourceRangeStart + map.RangeLength))
            { 
                long diff = source - map.SourceRangeStart;
                return map.DestinationRangeStart + diff;
            }
        }
        return source;
    }

    private List<Range> GetDestinationRanges(Range range, Map[] maps)
    {
        List<Range> result = new List<Range>();
        long i = range.Start;
        long length = range.RangeLength;

        foreach (Map map in maps)
        {
            if (i < map.SourceRangeStart && length > 0)
            {
                Range r = new Range(i, map.SourceRangeStart - i > length ? length : map.SourceRangeStart - i);
                result.Add(r);
                i += r.RangeLength;
                length -= r.RangeLength;
            }

            if (i >= map.SourceRangeStart && i < (map.SourceRangeStart + map.RangeLength) && length > 0)
            {
                long diff = i - map.SourceRangeStart;
                long overlap = length > (map.SourceRangeStart + map.RangeLength - i) ? (map.SourceRangeStart + map.RangeLength - i) : length;
                Range r = new Range(map.DestinationRangeStart + diff, overlap);
                result.Add(r);
                i += r.RangeLength;
                length -= r.RangeLength;
            }
        }

        if (i < range.Start + range.RangeLength)
        {
            Range r = new Range(i, range.Start + range.RangeLength - i);
            result.Add(r);
        }

        return result;
    }

    private record Map(long DestinationRangeStart, long SourceRangeStart, long RangeLength);

    private record Range(long Start, long RangeLength);
}
