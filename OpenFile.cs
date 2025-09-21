using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace TheRobotNavigationProblem
{
    internal class OpenFile
    {
        //Stores extracted values from txt file
        private int[] gridSize;
        private int[] agent;
        private List<int[]> goals = new List<int[]>();
        private List<int[]> walls = new List<int[]>();

        //Stores map
        private List<Node> map = new List<Node>();


        public OpenFile(string filePath)
        {
            //Line index for extracting each line to the application
            int lineIndex = 0;

            try
            {
                //Temporary storage for file lines
                string[] rawFile = File.ReadAllLines(filePath);

                //Removes blanks from file
                List<string> getLines = new List<string>();
                foreach(string fileLine in rawFile)
                {
                    if(!string.IsNullOrWhiteSpace(fileLine))
                    {
                        getLines.Add(fileLine);
                    }    
                    
                }

                //Prints lines to output
                int readIndex = 0;
                for(int i = 0; i < getLines.Count; i++)
                {
                    Console.WriteLine("Line " + readIndex + ": " + getLines[i]);
                    readIndex++;    
                }
                Console.WriteLine();

                // Parse grid size
                string line = getLines[lineIndex].Trim();

                gridSize = line
                    .Trim('[', ']')               // remove brackets
                    .Split(',')                   // split by comma
                    .Select(int.Parse)            // parses to int
                    .ToArray();                   // convert to array


                //IF '1': Checks that map is not too big
                //IF '2': Contains 2 map values
                //IF '3': Ensures the map is not empty, and has at least 2 cells
                if (gridSize[0] > 30 || gridSize[1] > 30)
                {
                    throw new InvalidOperationException("Map TOO BIG, the maximum size is x=30 y=30");
                }
                if (gridSize.Length != 2)
                {
                    throw new InvalidOperationException("You can only have 2 numbers");
                }
                if (!(gridSize[0] >= 1 && gridSize[1] >= 1 && (gridSize[0] > 1 || gridSize[1] > 1)))
                {
                    throw new InvalidOperationException("You need at least 2 cells");
                }

                lineIndex++;

                // Parse initial agent coordinates
                line = getLines[lineIndex].Trim();
                agent = line
                    .Trim('(', ')')               // remove brackets
                    .Split(',')                   // split by comma
                    .Select(int.Parse)            // parses to int
                    .ToArray();                   // convert to array


                //IF '1': Contains 2 agent values
                //IF '2': Ensures the agent is not out of bounds
                if (agent.Length != 2)
                {
                    throw new InvalidOperationException("You can only have 2 numbers");
                }
                if (agent[0] < 0 || agent[1] < 0 || agent[0] >= gridSize[0] || agent[1] >= gridSize[1])
                {
                    throw new InvalidOperationException("Agent out of bounds");
                }
                lineIndex++;

                // Parse goals; splits each goal individually
                var tempGoalParts = getLines[lineIndex].Trim()
                    .Split('|')
                    .Select(part => part.Trim())
                    .ToArray();

                foreach (var part in tempGoalParts)
                {
                    //Parse each goal
                    int[] tempGoal = part
                    .Trim('(', ')')               // remove brackets
                    .Split(',')                   // split by comma
                    .Select(int.Parse)            // parses to int
                    .ToArray();                   // convert to array

                    //IF '1': Contains 2 values per goal
                    //IF '2': Ensures the goal is not out of bounds
                    if (tempGoal.Length != 2)
                    {
                        throw new InvalidOperationException("For '" + part + "' You can only have 2 numbers");
                    }
                    if (tempGoal[0] < 0 || tempGoal[1] < 0 || tempGoal[0] >= gridSize[0] || tempGoal[1] >= gridSize[1])
                    {
                        throw new InvalidOperationException("'" + part + "' goal out of bounds");
                    }

                    //Adds goal to the list
                    goals.Add(tempGoal);
                }
                lineIndex++;
                //Loops through all other lines
                for (int i = lineIndex; i < getLines.Count; i++)
                {
                    //Parse each wall
                    line = getLines[i].Trim();

                    //Some lines might have blanks
                    if(line.Length == 0)
                    {
                        continue;
                    }

                    int[] tempWall = line
                    .Trim('(', ')')               // remove brackets
                    .Split(',')                   // split by comma
                    .Select(int.Parse)            // parses to int
                    .ToArray();                   // convert to array


                    //IF '1': Contains 4 wall values
                    //IF '2': Ensures the walls are not out of bounds
                    //IF '3': Ensures that width/height is not 0 or under
                    if (tempWall.Length != 4)
                    {
                        throw new InvalidOperationException("For '" + line + "' You can only have 4 numbers");
                    }
                    if (tempWall[0] < 0 || tempWall[1] < 0 || tempWall[0] + tempWall[2] > gridSize[0] || tempWall[1] + tempWall[3] > gridSize[1])
                    {
                        Console.WriteLine("Error 'Line " + i + "': wall out of bounds");
                        Console.WriteLine("\nPress any key to continue");
                        string menuChoice = Console.ReadLine();

                        //restarts application
                        Console.WriteLine("\n\n\t");
                        Process.Start("SearchMazeApplication.exe");
                        Thread.Sleep(100);
                        Environment.Exit(0);
                    }
                    if (tempWall[2] < 1 || tempWall[3] < 1)
                    {
                        Console.WriteLine("Error 'Line " + i + "': the width/height must be at least 1");
                        Console.WriteLine("\nPress any key to continue");
                        string menuChoice = Console.ReadLine();

                        //restarts application
                        Console.WriteLine("\n\n\t");
                        Process.Start("SearchMazeApplication.exe");
                        Thread.Sleep(100);
                        Environment.Exit(0);
                    }

                    //Adds walls
                    walls.Add(tempWall);

                    //Loops through each wall section (as there are multiple)
                    lineIndex++;
                }
            }
            catch (Exception ex)
            {
                //If parsing phases goes wrong, an exception message will be written
                Console.WriteLine($"Error 'Line "+ lineIndex + $"': {ex.Message}");
                Console.WriteLine("\nPress any key to continue");
                string menuChoice = Console.ReadLine();

                //restarts application
                Console.WriteLine("\n\n\t");
                Process.Start("SearchMazeApplication.exe");
                Thread.Sleep(100);
                Environment.Exit(0);
            }
            
        }

        public void CreateMap()
        {
            //Loops through all x and y coordinates
            for (int y = 0; y < gridSize[1]; y++)
            {
                for (int x = 0; x < gridSize[0]; x++)
                {
                    //Assigns and adds a node with corresponding x and y values
                    Node newNode = new Node(x, y);
                    map.Add(newNode);
                }
            }
        }

        public void CreateGoals()
        {
            foreach (Node location in map)
            {
                //Goes through each goal
                foreach(int[] goalLocation in goals)
                {
                    //Finds and assigns goals in the map
                    if (goalLocation[0] == location.GetX() && goalLocation[1] == location.GetY())
                    {
                        //Ensures goal and agent don't overlap
                        if(location.GetX() == agent[0] && location.GetY() == agent[1])
                        {
                            Console.WriteLine("Error Line 3: Goal '(" + goalLocation[0] +","+ goalLocation[1] + ")' cannot overlap agent");
                            Console.WriteLine("\nPress any key to continue");
                            string menuChoice = Console.ReadLine();

                            //restarts application
                            Console.WriteLine("\n\n\t");
                            Process.Start("SearchMazeApplication.exe");
                            Thread.Sleep(100);
                            Environment.Exit(0);
                        }
                        
                        location.AssignFeature("GOAL");
                        break;
                    }
                }
            }
        }

        public void CreateWall()
        {
            List<int[]> tempNodes = new List<int[]>();

            //Goes through each wall
            foreach (int[] wallLocation in walls)
            {
                //x starts at left most point of the wall section and loops to the end of its width
                for (int x = wallLocation[0]; x < wallLocation[0] + wallLocation[2]; x++)
                {
                    //y starts at left-top most point of the wall section and loops to the end of its height
                    for (int y = wallLocation[1]; y < wallLocation[1] + wallLocation[3]; y++)
                    {
                        //ensures that the values are within the map boundaries
                        if (x < gridSize[0] && y < gridSize[1])
                        {
                            //Add temporary wall coordinates
                            int[] newWall = { x, y };
                            tempNodes.Add(newWall);
                        }
                    }
                }
            }

            foreach (Node location in map)
            {
                foreach (int[] wall in tempNodes)
                {
                    //Adds each wall to the map
                    if (wall[0] == location.GetX() && wall[1] == location.GetY())
                    {
                        //Ensures walls don't overlap with wall and agent
                        if (wall[0] == agent[0] && wall[1] == agent[1])
                        {
                            Console.WriteLine("Error Line 3: Wall '(" + wall[0] + "," + wall[1] + ")' cannot overlap agent");
                            Console.WriteLine("\nPress any key to continue");
                            string menuChoice = Console.ReadLine();

                            //restarts application
                            Console.WriteLine("\n\n\t");
                            Process.Start("SearchMazeApplication.exe");
                            Thread.Sleep(100);
                            Environment.Exit(0);
                        }

                        //Ensures walls don't overlap with wall and goal
                        if (location.GetRoomType() == "GOAL" && (wall[0] == location.GetX() && wall[1] == location.GetY()))
                        {
                            Console.WriteLine("Error Line 3: Wall '(" + wall[0] + "," + wall[1] + ")' cannot overlap Goal");
                            Console.WriteLine("\nPress any key to continue");
                            string menuChoice = Console.ReadLine();

                            //restarts application
                            Console.WriteLine("\n\n\t");
                            Process.Start("SearchMazeApplication.exe");
                            Thread.Sleep(100);
                            Environment.Exit(0);
                        }

                        location.AssignFeature("WALL");
                        break;
                    }
                }
            }
        }

        public Agent CreateAgent()
        {
            Agent newAgent = new Agent();

            foreach (Node location in map)
            {
                //Checks agent coordinates on the map
                if (agent[0] == location.GetX() && agent[1] == location.GetY())
                {
                    //Assigns the node to the agent's coordinates
                    newAgent.SetLocation(location);
                    break;
                }
            }

            return newAgent;
        }

        public List<Node> GetNodes() { return map; }

        public int[] GetGridSize() { return gridSize; }
    }
}
