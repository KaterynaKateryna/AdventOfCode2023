namespace AdventOfCode;

public class Day20 : BaseDay
{
    private Dictionary<string, Module> _modules;

    public Day20()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        _modules = new Dictionary<string, Module>();
        for (int i = 0; i < lines.Length; ++i)
        {
            ModuleType type;
            string name;
            if (lines[i][0] == '%')
            {
                type = ModuleType.FlipFlop;
                name = lines[i].Substring(1, 2);
            }
            else if (lines[i][0] == '&')
            {
                type = ModuleType.Conjunction;
                name = lines[i].Substring(1, 2);
            }
            else
            {
                type = ModuleType.Broadcast;
                name = lines[i].Substring(0, 11);
            }
            Module module = new Module(name, type, new List<Module>());
            _modules[name] = module;
        }

        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(" -> ");
            string name = parts[0].Replace(" ", "").Replace("%", "").Replace("&", "");
            Module sourceModule = _modules[name];

            string[] destinationModules = parts[1].Replace(" ", "").Split(",");
            foreach(string destinationModule in destinationModules) 
            {
                if (_modules.ContainsKey(destinationModule))
                {
                    Module module = _modules[destinationModule];
                    sourceModule.DestinationModules.Add(module);

                    if (module.Type == ModuleType.Conjunction)
                    {
                        module.InputStates[sourceModule.Name] = false;
                    }
                }
                else
                {
                    sourceModule.DestinationModules.Add(new Module(destinationModule, ModuleType.Empty, new List<Module>()));
                }
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        long lowPulses = 0;
        long highPulses = 0;

        for (int i = 0; i < 1000; ++i)
        {
            (long l, long h) = GetPulses();
            lowPulses += l;
            highPulses += h;
        }

        return new((lowPulses * highPulses).ToString());
    }

    private (long, long) GetPulses()
    {
        long lowPulses = 1; // button emits low pulse
        long highPulses = 0;

        Queue<(Module Module, bool Pulse, string Source)> queue = new Queue<(Module Module, bool Pulse, string Source)>();
        Module current = _modules["broadcaster"];
        foreach (Module module in current.DestinationModules)
        {
            queue.Enqueue((module, false, "button"));
        }

        while (queue.Any())
        {
            (current, bool pulse, string source) = queue.Dequeue();
            if (pulse)
            {
                highPulses++;
            }
            else
            {
                lowPulses++;
            }

            switch (current.Type)
            {
                case ModuleType.Broadcast:
                    foreach (Module module in current.DestinationModules)
                    {
                        queue.Enqueue((module, pulse, current.Name));
                    }
                    break;
                case ModuleType.FlipFlop:
                    if (!pulse)
                    {
                        current.State = !current.State;
                        foreach (Module module in current.DestinationModules)
                        {
                            queue.Enqueue((module, current.State, current.Name));
                        }
                    }
                    break;
                case ModuleType.Conjunction:
                    current.InputStates[source] = pulse;
                    foreach (Module module in current.DestinationModules)
                    {
                        queue.Enqueue((module, !current.InputStates.Values.All(x => x == true), current.Name));
                    }
                    break;
            }
        }

        return (lowPulses, highPulses);
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private enum ModuleType
    { 
        FlipFlop, // %
        Conjunction, // & 
        Broadcast, // broadcast
        Empty
    }

    private record Module(string Name, ModuleType Type, List<Module> DestinationModules)
    { 
        public bool State { get; set; }

        public Dictionary<string, bool> InputStates { get; set; } = new Dictionary<string, bool>();
    }
}
