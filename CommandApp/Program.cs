using CommandApp.Enum;
using CommandApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.IO;

namespace CommandApp
{
    public class MethodCollections
    {
        public static BigInteger Add(List<BigInteger> a)
        {
            return a.Aggregate(BigInteger.Add);
        }

        public static BigInteger Mult(List<BigInteger> a)
        {
            return a.Aggregate(BigInteger.Multiply);
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            string Folderpath = (Directory.GetParent(Directory.GetCurrentDirectory()).Parent).Parent.FullName;
            string path = Path.Combine(Folderpath, "input_big.txt");
            //string path = @"C:\input.txt";
            var obj = ReadInput(path);
            ExecuteCommand(obj);
            Console.ReadLine();
        }

        public static List<Command> ReadInput(string path)
        {
            string line;
            List<Command> commands = new List<Command>();

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(' ');
                var comm = new Command();
                comm.Inputs = new List<int>();
                for (int i = 0; i < words.Length; i++)
                {
                    if (i == 0)
                    {
                        comm.Step = Convert.ToInt32(words[0].Replace(":", ""));
                    }
                    else if (i == 1)
                    {
                        comm.CommandType = (CommandType)System.Enum.Parse(typeof(CommandType), words[1].Trim());
                    }
                    else
                    {
                        comm.Inputs.Add(Convert.ToInt32(words[i]));
                    }

                }

                commands.Add(comm);
            }

            file.Close();

            return commands;

        }

        public static void ExecuteCommand(List<Command> commands)
        {

            var firstItem = commands.First();

            Console.Write($"{firstItem.CommandType.ToString()} ");
            firstItem.Inputs.ForEach(item => Console.Write($"{item} "));

            Console.WriteLine("");
            Console.WriteLine(DateTime.Now);
            Console.WriteLine($"{GetExpressionString(commands, firstItem)}");
            Console.WriteLine(DateTime.Now);
        }

        public static string GetExpressionString(List<Command> all, Command current)
        {
            //StringBuilder path = new StringBuilder();
            
            Func<List<BigInteger>, BigInteger> add = new Func<List<BigInteger> , BigInteger>(MethodCollections.Add);
            Func<List<BigInteger>, BigInteger> mult = new Func<List<BigInteger>, BigInteger>(MethodCollections.Mult);
            Dictionary<int, BigInteger> stepValue = new Dictionary<int, BigInteger>();

            Func<List<Command>, Command, BigInteger> GetExpression = null;
            GetExpression = (ps, p) =>
            {
                if(stepValue.ContainsKey(p.Step))
                {
                    return stepValue[p.Step];
                }

                if (p.CommandType == CommandType.Value)
                {
                    //path.Append($" ({p.Inputs.First()})");
                    return p.Inputs.First();
                }
                else
                {
                    //path.Append($" {p.CommandType.ToString()}");
                    List<BigInteger> subResult = new List<BigInteger>();
                    
                    foreach (var item in p.Inputs)
                    {
                        //path.Append($" {item}");
                        var level = all.Where(x => x.Step == item).First();
                        subResult.Add(GetExpression(ps, level));
                    }

                    if(p.CommandType == CommandType.Add)
                    {
                        var addResult = add(subResult);
                        stepValue.Add(p.Step, addResult);
                        return addResult;
                    }
                    else
                    {
                        var multResult = mult(subResult);
                        stepValue.Add(p.Step, multResult);
                        return multResult;
                    }

                }

            };
            
            Console.WriteLine($"Result = {GetExpression(all, current)}");
            //return path.ToString();
            return "Completed";
           
        }
   
    }
}


