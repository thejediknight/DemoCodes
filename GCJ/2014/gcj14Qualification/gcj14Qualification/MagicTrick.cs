using System;
using System.Linq;
using System.Collections.Generic;

namespace gcj14Qualification
{
    public class MagicTrick
    {
        public MagicTrick()
        {
        }

        public string[] solve(string[] lines)
        {
            var cases = int.Parse(lines[0]);
            string[] res = new string[cases];
            for (int i = 0; i < cases; i++)
            {
                var firstAnswer = int.Parse(lines[1 + i * 10]);
                var firstRow = lines[1 + i * 10 + firstAnswer].Split(' ').Select(x => int.Parse(x));
                var secondAnswer = int.Parse(lines[6 + i * 10]);
                var secondRow = lines[6 + i * 10 + secondAnswer].Split(' ').Select(x => int.Parse(x));
                var intersection = firstRow.Intersect(secondRow).ToList();

                if (intersection.Count == 0)
                {
                    res[i] = string.Format("Case #{0}: Volunteer cheated!", i + 1);
                }
                else if (intersection.Count == 1)
                {
                    res[i] = string.Format("Case #{0}: {1}", i + 1, intersection[0]);
                }
                else
                {
                    res[i] = string.Format("Case #{0}: Bad Magician!", i + 1);
                }
            }
            return res;
        }
    }
}