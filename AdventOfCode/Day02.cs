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
        int result = _input.Sum(game => game.Rounds.All(r => r.IsPossible()) ? game.Index : 0);
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(_input.Sum(game => game.MinPower()).ToString());
    }

    private record Game(int Index, Round[] Rounds)
    {
        public long MinPower() => Rounds.Max(r => r.Red) * Rounds.Max(r => r.Green) * Rounds.Max(r => r.Blue);  

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
        public bool IsPossible() => Red <= 12 && Green <= 13 && Blue <= 14;

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
