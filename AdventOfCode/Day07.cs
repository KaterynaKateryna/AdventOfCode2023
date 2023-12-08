namespace AdventOfCode;

public class Day07 : BaseDay
{
    private Hand[] _hands;

    public Day07() 
    {
        _hands = File.ReadAllLines(InputFilePath).Select(Hand.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        Array.Sort(_hands);

        long result = 0;
        for (int i = 0; i < _hands.Length; ++i)
        {
            result += _hands[i].Bid * (i + 1);
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private record Hand(string Cards, int Bid) : IComparable<Hand>
    {
        public HandType HandType { get; init; }

        public static Hand Parse(string line)
        {
            string[] parts = line.Split(" ");
            return new Hand(parts[0], int.Parse(parts[1]))
            { 
                HandType = GetHandType(parts[0])
            };
        }

        private static HandType GetHandType(string cards)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            foreach (char ch in cards)
            {
                dict[ch] = dict.ContainsKey(ch) ? dict[ch] + 1 : 1;
            }

            if (dict.Any(kv => kv.Value == 5))
            {
                return HandType.FiveOfAKind;
            }
            if (dict.Any(kv => kv.Value == 4))
            {
                return HandType.FourOfAKind;
            }
            if (dict.Any(kv => kv.Value == 3) && dict.Any(kv => kv.Value == 2))
            {
                return HandType.FullHouse;
            }
            if (dict.Any(kv => kv.Value == 3))
            {
                return HandType.ThreeOfAKind;
            }
            if (dict.Count(kv => kv.Value == 2) == 2)
            {
                return HandType.TwoPair;
            }
            if (dict.Count(kv => kv.Value == 2) == 1)
            {
                return HandType.OnePair;
            }
            return HandType.HighCard;
        }

        public int CompareTo(Hand other)
        {
            if (this.HandType < other.HandType)
            {
                return -1;
            }
            if (this.HandType > other.HandType)
            {
                return 1;
            }

            for (int i = 0; i < this.Cards.Length; ++i)
            {
                if (this.Cards[i] == other.Cards[i])
                {
                    continue;
                }

                Card a = (Card)Enum.Parse(typeof(Card), "_" + this.Cards[i].ToString());
                Card b = (Card)Enum.Parse(typeof(Card), "_" + other.Cards[i].ToString());

                return a - b;
            }

            return 0;
        }
    }

    private enum HandType
    {
        HighCard = 1,
        OnePair = 2,
        TwoPair = 3,
        ThreeOfAKind = 4,
        FullHouse = 5,
        FourOfAKind = 6,
        FiveOfAKind = 7
    }

    private enum Card
    {
        _2 = 2,
        _3 = 3,
        _4 = 4,
        _5 = 5,
        _6 = 6,
        _7 = 7,
        _8 = 8,
        _9 = 9,
        _T = 10,
        _J = 11,
        _Q = 12,
        _K = 13,
        _A = 14
    }
}
