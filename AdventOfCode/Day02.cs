namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly Game[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath).Select(l => Game.Parse(l)).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int redLimit = 12;
        int greenLimit = 13;
        int blueLimit = 15;

        int result = 0;
        foreach (Game game in _input)
        {
            if (game.Rounds.All(r => r.Red <= redLimit && r.Green <= greenLimit && r.Blue <= blueLimit))
            {
                result += game.Index;
            }
        }
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private record Game(int Index, Round[] Rounds)
    {
        public static Game Parse(string input)
        {
            string[] parts = input.Split(":");
            int index = int.Parse(parts[0].Split(" ")[1]);
            Round[] rounds = parts[1].Split(";").Select(roundInput => Round.Parse(roundInput.Split(","))).ToArray();
            return new Game(index, rounds);
        }
    }

    private record Round(int Red, int Green, int Blue)
    {
        public static Round Parse(string[] round)
        {
            int red = 0, green = 0, blue = 0;
            foreach (string input in round)
            {
                string[] parts = input.Trim().Split(" ");
                int count = int.Parse(parts[0]);
                switch (parts[1])
                {
                    case "red":
                        red = count;
                        break;
                    case "green":
                        green = count;
                        break;
                    case "blue":
                        blue = count;
                        break;
                }
            }
            return new Round(red, green, blue);
        }
    }
}
