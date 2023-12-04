namespace AdventOfCode;

public class Day04 : BaseDay
{
    private Card[] _input;

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath).Select(Card.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = 0;
        foreach (Card card in _input) 
        {
            int matches = card.WinningNumbers.Intersect(card.YourNumbers).Count();
            int score = (int)Math.Pow(2, matches-1);
            sum += score;
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private record Card(int[] WinningNumbers, int[] YourNumbers)
    {
        public static Card Parse(string line)
        {
            string[] parts = line.Split(':')[1].Split('|');
            int[] winning = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            int[] yours = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            return new Card(winning, yours);
        }
    }
}
