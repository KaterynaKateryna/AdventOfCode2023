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
        return new("TBD");
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
