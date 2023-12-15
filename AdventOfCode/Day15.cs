namespace AdventOfCode;

public class Day15 : BaseDay
{
    private string[] _steps;

    public Day15()
    {
        string input = File.ReadAllText(InputFilePath);
        _steps = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
    }

    public override ValueTask<string> Solve_1()
    {
        return new(_steps.Sum(GetHashCode).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, List<Lens>> boxes = new Dictionary<int, List<Lens>>();
        foreach (string step in _steps)
        {
            if (step.EndsWith("-"))
            {
                string label = step.Replace("-", "");
                int hashCode = GetHashCode(label);
                if (boxes.ContainsKey(hashCode))
                {
                    List<Lens> lenses = boxes[hashCode];
                    Lens existingLens = lenses.FirstOrDefault(l => l.Label == label);
                    if (existingLens != null)
                    {
                        lenses.Remove(existingLens);
                    }
                }
            }
            else
            {
                string[] lensValues = step.Split("=");
                Lens newLens = new Lens(lensValues[0], int.Parse(lensValues[1]));
                int hashCode = GetHashCode(newLens.Label);
                if (boxes.ContainsKey(hashCode))
                {
                    List<Lens> lenses = boxes[hashCode];
                    Lens existingLens = lenses.FirstOrDefault(l => l.Label == newLens.Label);
                    if (existingLens != null)
                    {
                        existingLens.FocalLength = newLens.FocalLength;
                    }
                    else
                    {
                        lenses.Add(newLens);
                    }
                }
                else
                {
                    boxes[hashCode] = [newLens];
                }
            }
        }

        return new(GetFocusingPower(boxes).ToString());
    }

    private int GetHashCode(string step)
    {
        int result = 0;
        foreach(char ch in step) 
        {
            result += ch;
            result *= 17;
            result %= 256;
        }

        return result;
    }

    private int GetFocusingPower(Dictionary<int, List<Lens>> boxes)
    {
        int result = 0;
        foreach (var kv in boxes)
        {
            for (int i = 0; i < kv.Value.Count; ++i)
            {
                result += (kv.Key + 1) * (i + 1) * kv.Value[i].FocalLength;
            }
        }
        return result;
    }

    private record Lens(string Label, int FocalLength)
    {
        public int FocalLength { get; set; } = FocalLength;
    }
}
