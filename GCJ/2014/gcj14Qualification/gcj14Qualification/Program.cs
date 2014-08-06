using System;
using System.IO;

namespace gcj14Qualification
{
    public class Program
    {
        public const string PATH_IN = @"E:\GitHub\GCJ\2014\A-small-practice.in";
        public const string PATH_OUT = @"E:\GitHub\GCJ\2014\A.OUT";

        static void Main(string[] args)
        {
            var obj = new MagicTrick();
            var lines = File.ReadAllLines(PATH_IN);
            var res = obj.solve(lines);
            File.WriteAllLines(PATH_OUT, res);
        }
    }
}
