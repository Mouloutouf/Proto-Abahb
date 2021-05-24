using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Sirenix.Utilities;
using UnityEngine;

namespace Utilities.Random
{
    public class DiceMacro
    {
        public static readonly Dictionary<string, (int priority, Func<int,int,int> operation)> ToOperation = new Dictionary<string, (int priority, Func<int,int,int> operation)>() {
            {"+", (1, (a, b) => a + b)},
            {"-", (1,(a, b) => a - b)},
            {"*", (2,(a, b) => a * b)},
            {"/", (2,(a, b) => a / b)},
            {"^", (3,(a, b) => Mathf.RoundToInt(Mathf.Pow(a, b)))},
            {"max", (0,Mathf.Max)},
            {"min", (0,Mathf.Min)},
            {"=", (-5,(a, b) => a == b ? 1 : 0)},
            {"!=", (-5,(a, b) => a != b ? 1 : 0)},
            {">", (-5,(a, b) => a > b ? 1 : 0)},
            {"<", (-5,(a, b) => a < b ? 1 : 0)},
            {">=", (-5,(a, b) => a >= b ? 1 : 0)},
            {"<=", (-5,(a, b) => a <= b ? 1 : 0)},
        };
        public static readonly Dictionary<string, Func<Dice,int,int[]>> ToModifier = new Dictionary<string, Func<Dice,int,int[]>>() {
            {"clamp", (d, m) => new int[]{d.Result < m ? m : d.Result}},
            {"base", (d, m) => d.Historic.Select(n => n < m ? m : n).ToArray()},
            {"best", (d, m) => {
                List<int> best = new List<int>(d.Historic);
                best.Sort();
                best.RemoveRange(0, best.Count-m);
                return best.ToArray();
            } },
            {"worst", (d, m) => {
                List<int> worst = new List<int>(d.Historic);
                worst.Sort();
                worst.RemoveRange(m, worst.Count-m);
                return worst.ToArray();
            } },
            {"adv", (d, m) => {
                int a = d.Historic.Sum();
                int[] dd = new int[d.Amount];
                for (int i = 0; i < d.Amount; i++)
                {
                    dd[i] = UnityEngine.Random.Range(1, d.Sides+1);
                }
                int b = dd.Sum();
                return a > b ? new int[]{a} : new int[]{b};
            }},
            {"dis", (d, m) => {
                int a = d.Historic.Sum();
                int[] dd = new int[d.Amount];
                for (int i = 0; i < d.Amount; i++)
                {
                    dd[i] = UnityEngine.Random.Range(1, d.Sides+1);
                }
                int b = dd.Sum();
                return a < b ? new int[]{a} : new int[]{b};
            }},
            {"dif", (d, m) => new int[]{d.Historic.Sum() - m}}
        };

        public static string[] Operators => ToOperation.Keys.ToArray();
        public static string[] Modifiers => ToModifier.Keys.ToArray();
        
        public static int Roll(string input)
        {
            var macro = DecryptMacro(input);
            return RollMacro(macro);
        }
        
        public static int[] RollMany(int recursion, string input)
        {
            var macro = DecryptMacro(input);
            int[] results = new int[recursion];
            for (int i = 0; i < recursion; i++)
            {
                results[i] = RollMacro(macro);
            }
            return results;
        }
        public static List<List<Resultable>> DecryptMacro(string input)
        {
            input = input.ToLower().Replace(" ", "");
            string[] blocks;
            if (input.Contains("(") && input.Contains(")"))
            {
                blocks = input.SplitAt(true, ')', '(');
            }
            else blocks = new string[]{input};
            
            List<List<Resultable>> macro = new List<List<Resultable>>();
            for (int i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                List<(Type type, string source)> elements = new List<(Type type, string source)>();

                Mode mode = Mode.Chain;
                //string msg = "";
                string current = "";
                void ShipCurrent()
                {
                    if (!current.IsNullOrWhitespace())
                    {
                        switch (mode)
                        {
                            case Mode.Chain:
                                if(current.Contains('d')) elements.Add((Type.Dice, current));
                                else elements.Add((Type.Number, current));
                                break;
                            case Mode.Operator:
                                elements.Add((Type.Operator, current));
                                break;
                            case Mode.Mod:
                                elements.Add((Type.Mod, current));
                                break;
                        }
                    }
                    current = "";
                }
                for (int j = 0; j < block.Length; j++)
                {
                    var c = block[j];
                    CharType type = c == 'd' || Char.IsDigit(c) ? CharType.Chain :
                        c == '[' ? CharType.StartMod :
                        c == ']' ? CharType.EndMod :
                        CharType.Operator;
                    switch (mode)
                    {
                        case Mode.Chain:
                            switch (type)
                            {
                                case CharType.Chain:
                                    current += c;
                                    break;
                                case CharType.StartMod:
                                    ShipCurrent();
                                    mode = Mode.Mod;
                                    break;
                                case CharType.EndMod:
                                    new Exception("Incorrect syntax. Mods should start with [ and end with ].");
                                    break;
                                case CharType.Operator:
                                    ShipCurrent();
                                    mode = Mode.Operator;
                                    current += c;
                                    break;
                            }

                            break;
                        case Mode.Operator:
                            switch (type)
                            {
                                case CharType.Chain:
                                    ShipCurrent();
                                    mode = Mode.Chain;
                                    current += c;
                                    break;
                                case CharType.StartMod:
                                    ShipCurrent();
                                    mode = Mode.Mod;
                                    break;
                                case CharType.EndMod:
                                    new Exception("Incorrect syntax. Mods should start with [ and end with ].");
                                    break;
                                case CharType.Operator:
                                    current += c;
                                    break;
                            }

                            break;
                        case Mode.Mod:
                            switch (type)
                            {
                                case CharType.Chain:
                                    current += c;
                                    break;
                                case CharType.StartMod:
                                    new Exception("Incorrect syntax. Mods should start with [ and end with ].");
                                    break;
                                case CharType.EndMod:
                                    ShipCurrent();
                                    mode = Mode.Chain;
                                    break;
                                case CharType.Operator:
                                    current += c;
                                    break;
                            }

                            break;
                    }
                    //msg += $"{c} : type({type}) mode({mode})\n";
                }
                ShipCurrent();
                //Debug.Log(msg);
                /*
                msg = "";
                for (int j = 0; j < elements.Count; j++)
                {
                    msg += $"{elements[j].source}({elements[j].type.ToString()}) ";
                }
                Debug.Log(msg);*/

                List<Resultable> chain = new List<Resultable>();
                for (int j = 0; j < elements.Count; j++)
                {
                    var element = elements[j];
                    Resultable resultable = null;
                    switch (element.type)
                    {
                        case Type.Number:
                            var value = 0;
                            if (!int.TryParse(element.source, out value)) 
                                new Exception($"Incorrect Syntax => {element.source}");
                            resultable = Number.New(element.source, value);
                            break;
                        case Type.Dice:
                            var dice = element.source.SplitAt(true, 'd');
                            int amount = 1;
                            int sides = 0;
                            if (dice.Length > 1)
                            {
                                if(!dice[0].IsNullOrWhitespace() && !int.TryParse(dice[0], out amount)) 
                                    new Exception($"Amount value ({dice[0]}) of dice could not be parsed");
                                if(!int.TryParse(dice[1], out sides))
                                    new Exception($"Sides value ({dice[1]}) of dice could not be parsed");
                            } 
                            else new Exception($"Incorrect Syntax => {element.source}");
                            resultable = Dice.New(element.source, amount, sides);
                            break;
                        case Type.Mod:
                            var mod = element.source.SplitAt(true, ':');
                            if (!ToModifier.ContainsKey(mod[0])) new Exception($"Modifier ({mod[0]}) is not recognized.");
                            int modifier = 0;
                            if (mod.Length > 1 && !int.TryParse(mod[1], out modifier)) new Exception($"Modifier value ({mod[1]}) could not be parsed");
                            resultable = Mod.New(element.source, ToModifier[mod[0]], modifier);
                            break;
                        case Type.Operator:
                            if (!ToOperation.ContainsKey(element.source)) new Exception($"Operator ({element.source}) is not recognized.");
                            var op = ToOperation[element.source];
                            resultable = Operation.New(element.source, op.priority, op.operation);
                            break;
                    }
                    chain.Add(resultable);
                }

                macro.Add(chain);
            }
            return macro;
        }
        public static int RollMacro(List<List<Resultable>> macro)
        {
            var blocks = new List<List<Resultable>>();
            for (int i = 0; i < macro.Count; i++)
            {
                blocks.Add(new List<Resultable>(macro[i]));
                blocks.Last().ForEach(r => r.Clear());
            }
            var chain = new List<Resultable>();
            //var msg = $"Macro :\n-Block Length {blocks.Count}\n";
            for (int i = 0; i < blocks.Count; i++)
            {
                chain.AddRange(Resolve(blocks[i]));
            }
            if (chain.Count > 1)
            {
                //msg += $"Final Pass :\n-Block Length {blocks.Count}\n";
                Resolve(chain);
            }
            //Debug.Log(msg);
            if (chain.Count > 1) new Exception("Chain cannot be executed");
            return chain.First().Result;
            List<Resultable> Resolve(List<Resultable> block)
            {
                List<Resultable> order = new List<Resultable>();
                var query = 
                    from result in block
                    group result by result.Type into newGroup
                    orderby (int)newGroup.Key
                    select newGroup;
                foreach (var typeGroup in query)
                {
                     order.AddRange(typeGroup.OrderByDescending(r => r.Priority).ToList());
                }

                //msg += $"    |-Block :\n-Element Length {order.Length}\n";
                for (int j = 0; j < order.Count; j++)
                {
                    var element = order[j];
                    var index = block.IndexOf(element);
                    bool end = false;
                    //msg += $"        |-{element.Type} : {element.Source}";
                    if (element.Historic.Length > 0) continue;
                    switch (element.Type)
                    {
                        case Type.Number:
                        case Type.Dice:
                            element.Calculate(block, index);
                            //msg += $" | Result : {element.Result}\n";
                            break;
                        case Type.Mod:
                            if (j > 0 && block[index - 1].Type == Type.Dice)
                            {
                                element.Calculate(block, index);
                                //msg += $" | Result : {element.Result}\n";
                            }
                            break;
                        case Type.Operator:
                            if (index - 1 >= 0 && index + 1 < block.Count)
                            {
                                element.Calculate(block, index);
                                //msg += $" | Result : {element.Result}\n";
                            }
                            else end = true;
                            break;
                    }
                    if(end) break;
                }
                return block;
            }
        }
        
        public enum Type
        {
            Number = 0,
            Dice = 1,
            Mod = 2,
            Operator = 3
        }
        public enum CharType
        {
            Chain,
            StartMod,
            EndMod,
            Operator,
        }
        public enum Mode
        {
            Chain,
            Operator,
            Mod
        }
        
        public abstract class Resultable
        {
            public int Result => Historic.Sum();
            public int[] Historic;
            public string Source;
            public int Priority;
            public abstract Type Type { get; }

            public abstract void Calculate(List<Resultable> chain, int index);

            public void Clear()
            {
                Historic = new int[0];
            }
        }
        public class Number : Resultable
        {
            public static Number New(string source, int value)
            {
                var num = new Number();
                num.Source = source;
                num.Value = value;
                return num;
            }

            public int Value;
            public override Type Type => Type.Number;
            public override void Calculate(List<Resultable> chain, int index)
            {
                if(Historic.Length > 0) return;
                Historic = new[] {Value};
            }
        }
        public class Dice : Resultable
        {
            public static Dice New(string source, int amount, int sides) 
            {
                var dice = new Dice();
                dice.Source = source;
                dice.Amount = amount;
                dice.Sides = sides;
                return dice;
            }

            public int Amount;
            public int Sides;
            public override Type Type => Type.Dice;
            public override void Calculate(List<Resultable> chain, int index)
            {
                if(Historic.Length > 0) return;
                Historic = new int[Amount];
                for (int i = 0; i < Amount; i++)
                {
                    Historic[i] = UnityEngine.Random.Range(1, Sides+1);
                }
            }
        }
        public class Mod : Resultable
        {
            public static Mod New(string source, Func<Dice, int, int[]> func, int modifier)
            {
                var mod = new Mod();
                mod.Source = source;
                mod.Modifier = modifier;
                mod.Func = func;
                return mod;
            }

            public int Modifier;
            public Func<Dice, int, int[]> Func;
            public override Type Type => Type.Mod;
            public override void Calculate(List<Resultable> chain, int index)
            {
                if(Historic.Length > 0) return;
                if (index == 0) new Exception("A modifier has no target.");
                if (chain[index-1].GetType() != typeof(Dice)) new Exception("Modifiers can only be added to dices.");
                Historic = Func.Invoke((Dice)chain[index-1], Modifier);
                chain.RemoveAt(index - 1);
            }
        }
        public class Operation : Resultable
        {
            public static Operation New(string source, int priority, Func<int, int, int> func)
            {
                var operation = new Operation();
                operation.Source = source;
                operation.Priority = priority;
                operation.Func = func;
                return operation;
            }
            
            public Func<int, int, int> Func;
            public override Type Type => Type.Operator;
            public override void Calculate(List<Resultable> chain, int index)
            {
                if(Historic.Length > 0) return;
                Historic = new int[]{Func.Invoke(chain[index-1].Result, chain[index+1].Result)};
                chain.RemoveAt(index+1);
                chain.RemoveAt(index-1);
            }
        }
    }
}