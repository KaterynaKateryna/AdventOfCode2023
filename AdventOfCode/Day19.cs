using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day19 : BaseDay
{
    private Dictionary<string, Workflow> _workflows;
    private Part[] _parts;

    public Day19()
    {
        string lines = File.ReadAllText(InputFilePath);
        string[] sections = lines.Split(Environment.NewLine + Environment.NewLine);

        string[] workflows = sections[0].Split(Environment.NewLine);
        _workflows = new Dictionary<string, Workflow>();
        for (int i = 0; i < workflows.Length; ++i)
        {
            string[] wfParts = workflows[i].Split("{");
            string wfName = wfParts[0];
            string[] r = wfParts[1].Replace("}", "").Split(",");

            Rule[] rules = new Rule[r.Length];
            for (int j = 0; j < rules.Length; ++j)
            {
                string[] ruleParts = r[j].Split(":");
                Condition condition = null;
                if (ruleParts.Length == 2)
                {
                    condition = new Condition(ruleParts[0][0], ruleParts[0][1], int.Parse(ruleParts[0].Substring(2)));
                }

                rules[j] = new Rule(condition, ruleParts.Length == 2 ? ruleParts[1] : ruleParts[0]);
            }

            _workflows[wfName] = new Workflow(wfName, rules);
        }

        string[] parts = sections[1].Split(Environment.NewLine);
        _parts = new Part[parts.Length];
        for (int i = 0; i < parts.Length; ++i)
        {
            Match match = Regex.Match(parts[i], "{x=(\\d+),m=(\\d+),a=(\\d+),s=(\\d+)}");
            _parts[i] = new Part(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value)
            );
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = _parts.Where(part => IsAccepted(part)).Select(part => part.Sum()).Sum();
        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long res = GetAcceptedParts();

        return new(res.ToString());
    }

    private long GetAcceptedParts(
        string workflow = "in", 
        long xMin = 1,
        long xMax = 4000,
        long mMin = 1,
        long mMax = 4000,
        long aMin = 1,
        long aMax = 4000,
        long sMin = 1,
        long sMax = 4000
    )
    {
        Workflow current = _workflows[workflow];

        long parts = 0;

        foreach (Rule rule in current.Rules)
        {
            long prevValue = 0;
            if (rule.Condition != null)
            {
                switch ((rule.Condition.Property, rule.Condition.Operation))
                {
                    case ('x', '>'):
                        prevValue = xMin;
                        xMin = rule.Condition.Value + 1;
                        break;
                    case ('x', '<'):
                        prevValue = xMax;
                        xMax = rule.Condition.Value - 1;
                        break;
                    case ('m', '>'):
                        prevValue = mMin;
                        mMin = rule.Condition.Value + 1;
                        break;
                    case ('m', '<'):
                        prevValue = mMax;
                        mMax = rule.Condition.Value - 1;
                        break;
                    case ('a', '>'):
                        prevValue = aMin;
                        aMin = rule.Condition.Value + 1;
                        break;
                    case ('a', '<'):
                        prevValue = aMax;
                        aMax = rule.Condition.Value - 1;
                        break;
                    case ('s', '>'):
                        prevValue = sMin;
                        sMin = rule.Condition.Value + 1;
                        break;
                    case ('s', '<'):
                        prevValue = sMax;
                        sMax = rule.Condition.Value - 1;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            if (rule.Result == "R")
            {
                parts += 0;
            }
            else if (rule.Result == "A")
            {
                parts += (xMax - xMin + 1) * (mMax - mMin + 1) * (aMax - aMin + 1) * (sMax - sMin + 1);
            }
            else
            {
                parts += GetAcceptedParts(rule.Result, xMin, xMax, mMin, mMax, aMin, aMax, sMin, sMax);
            }

            if (rule.Condition != null)
            {
                switch ((rule.Condition.Property, rule.Condition.Operation))
                {
                    case ('x', '>'):
                        xMin = prevValue;
                        xMax = rule.Condition.Value;
                        break;
                    case ('x', '<'):
                        xMax = prevValue;
                        xMin = rule.Condition.Value;
                        break;
                    case ('m', '>'):
                        mMin = prevValue;
                        mMax = rule.Condition.Value;
                        break;
                    case ('m', '<'):
                        mMax = prevValue; 
                        mMin = rule.Condition.Value;
                        break;
                    case ('a', '>'):
                        aMin = prevValue; 
                        aMax = rule.Condition.Value;
                        break;
                    case ('a', '<'):
                        aMax = prevValue; 
                        aMin = rule.Condition.Value;
                        break;
                    case ('s', '>'):
                        sMin = prevValue;
                        sMax = rule.Condition.Value;
                        break;
                    case ('s', '<'):
                        sMax = prevValue; 
                        sMin = rule.Condition.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        return parts;
    }

    private bool IsAccepted(Part part, string workflow = "in")
    {
        Workflow current = _workflows[workflow];

        foreach (Rule rule in current.Rules)
        {
            if (IsConditionMet(rule.Condition, part))
            {
                if (rule.Result == "R")
                {
                    return false;
                }
                else if (rule.Result == "A")
                {
                    return true;
                }
                else
                { 
                    return IsAccepted(part, rule.Result);
                }
            }
        }

        throw new NotImplementedException();
    }

    private bool IsConditionMet(Condition condition, Part part)
    {
        if (condition == null)
        {
            return true;
        }

        switch ((condition.Property, condition.Operation)) 
        {
            case ('x', '>'):
                return part.X > condition.Value;
            case ('x', '<'):
                return part.X < condition.Value;
            case ('m', '>'):
                return part.M > condition.Value;
            case ('m', '<'):
                return part.M < condition.Value;
            case ('a', '>'):
                return part.A > condition.Value;
            case ('a', '<'):
                return part.A < condition.Value;
            case ('s', '>'):
                return part.S > condition.Value;
            case ('s', '<'):
                return part.S < condition.Value;
            default:
                throw new NotImplementedException();
        }
    }

    private record Part(int X, int M, int A, int S)
    { 
        public int Sum() => X + M + A + S;  
    }

    private record Rule(Condition Condition, string Result);

    private record Condition(char Property, char Operation, int Value);

    private record Workflow(string Name, Rule[] Rules);
}
