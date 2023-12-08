namespace AdventOfCode;

public class Day07 : BaseDay
{
    public override ValueTask<string> Solve_1() => Solve(Hand.Parse);

    public override ValueTask<string> Solve_2() => Solve(HandV2.ParseV2);

    private ValueTask<string> Solve(Func<string, Hand> Parse)
    {
        Hand[] hands = File.ReadAllLines(InputFilePath).Select(Parse).ToArray();
        Array.Sort(hands);

        long result = 0;
        for (int i = 0; i < hands.Length; ++i)
        {
            result += hands[i].Bid * (i + 1);
        }

        return new(result.ToString());
    }

    private record Hand(string Cards, int Bid) : IComparable<Hand>
    {
        public HandType HandType { get; private set; }

        public static Hand Parse(string line)
        {
            string[] parts = line.Split(" ");
            Hand hand = new Hand(parts[0], int.Parse(parts[1]));
            hand.InitHandType();
            return hand;
        }

        public void InitHandType()
        {
            Dictionary<char, int> dict = GetCardsDictionary(Cards);

            if (dict.Any(kv => kv.Value == 5))
            {
                HandType = HandType.FiveOfAKind;
            }
            else if (dict.Any(kv => kv.Value == 4))
            {
                HandType = HandType.FourOfAKind;
            }
            else if (dict.Any(kv => kv.Value == 3) && dict.Any(kv => kv.Value == 2))
            {
                HandType = HandType.FullHouse;
            }
            else if (dict.Any(kv => kv.Value == 3))
            {
                HandType = HandType.ThreeOfAKind;
            }
            else if (dict.Count(kv => kv.Value == 2) == 2)
            {
                HandType = HandType.TwoPair;
            }
            else if (dict.Count(kv => kv.Value == 2) == 1)
            {
                HandType = HandType.OnePair;
            }
            else
            {
                HandType = HandType.HighCard;
            }
        }

        protected virtual Type CardType => typeof(Card);

        protected virtual Dictionary<char, int> GetCardsDictionary(string cards)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            foreach (char ch in cards)
            {
                dict[ch] = dict.ContainsKey(ch) ? dict[ch] + 1 : 1;
            }
            return dict;
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

                int a = (int)Enum.Parse(CardType, "_" + this.Cards[i].ToString());
                int b = (int)Enum.Parse(CardType, "_" + other.Cards[i].ToString());

                return a - b;
            }

            return 0;
        }
    }

    private record HandV2(string Cards, int Bid) : Hand(Cards, Bid)
    {
        public static Hand ParseV2(string line)
        {
            string[] parts = line.Split(" ");
            HandV2 hand = new HandV2(parts[0], int.Parse(parts[1]));
            hand.InitHandType();
            return hand;
        }

        protected override Type CardType => typeof(CardV2);

        protected override Dictionary<char, int> GetCardsDictionary(string cards)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            int jCount = 0;
            foreach (char ch in cards)
            {
                if (ch == 'J')
                {
                    jCount++;
                    continue;
                }
                dict[ch] = dict.ContainsKey(ch) ? dict[ch] + 1 : 1;
            }

            if (!dict.Any())
            {
                dict['J'] = jCount;
                return dict;
            }

            char key = dict.First().Key; 
            int value = dict.First().Value;
            foreach (var kv in dict)
            {
                if (kv.Value > value)
                { 
                    key = kv.Key;
                    value = kv.Value;
                }
            }

            dict[key] += jCount;

            return dict;
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

    private enum CardV2
    {
        _J = 1,
        _2 = 2,
        _3 = 3,
        _4 = 4,
        _5 = 5,
        _6 = 6,
        _7 = 7,
        _8 = 8,
        _9 = 9,
        _T = 10,
        _Q = 11,
        _K = 12,
        _A = 13
    }
}
