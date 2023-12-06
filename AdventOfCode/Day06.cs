namespace AdventOfCode;

public class Day06 : BaseDay
{
    private Race[] _races;

    public Day06()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        string[] times = lines[0].Replace("Time:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] distances = lines[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);

        _races = new Race[times.Length];
        for (int i = 0; i < times.Length; ++i)
        {
            _races[i] = new Race(int.Parse(times[i]), int.Parse(distances[i]));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int result = _races.Aggregate(1, (result, race) => result * GetWaysToWin(race));
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private int GetWaysToWin(Race race)
    {
        int waysToWin = 0;
        for (int i = 0; i < race.Time; ++i)
        {
            int distance = (race.Time - i) * i;
            waysToWin += distance > race.Distance ? 1 : 0;
        }
        return waysToWin;
    }

    private record Race(int Time, int Distance);
}
