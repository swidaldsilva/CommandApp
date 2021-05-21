using CommandApp.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandApp.Model
{
    public class Command
    {
        public int Step { get; set; }
        public CommandType CommandType { get; set; }
        public List<int> Inputs { get; set; }

        public Command()
        {

        }
        public Command(int step, CommandType commandType, List<int> inputs)
        {
            Step = step;
            CommandType = commandType;
            Inputs = inputs;
        }

    }
}
