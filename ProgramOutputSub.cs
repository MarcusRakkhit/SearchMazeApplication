using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class ProgramOutputSub
    {
        public ProgramOutputSub()
        {
            Console.WriteLine(@"1.   Empty file
2.   Invalid digit(map)
3.   Invalid digit(agent)
4.   Invalid digit(goal)\n
5.   Invalid digit(wall)\n
6.   Unmatching 2 digits(map)
7.   Unmatching 2 digits(agent)
8.   Unmatching 2 digits(goal)
9.   Unmatching 4 digits(wall)
10.  invalid syntax(map)
11.  invalid syntax(agent)
12.  invalid syntax(goal)
13.  invalid syntax(wall)
14.  Grid too small
15.  width / height too small(wall)
16.  Agent out of bounds
17.  Goal out of bounds
18.  Walls out of bounds
19.  No Goal Specified
20.  Wall Overlaps with Goal
21.  Wall Overlaps with Agent
22.  Goal Overlaps with Agent
23.  Map TOO BIG
24.  Grid with no walls
25.  Grid with walls
26.  Grid with All Walls Blocking Path
27.  Multiple Goals
28.  Multiple Goals, One Blocked
29.  Single line
30.  Map with 900 cells(30x30)
31.  BFS performance is better than DFS
32.  Greedy performance is better than AStar
33.  Un-optimal route for Greedy
34.  Un-optimal route for DFS
35.  Custom File");
        }
    }
}
