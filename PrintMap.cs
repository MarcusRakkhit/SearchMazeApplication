using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class PrintMap
    {
        private List<Node> map;

        public PrintMap(List<Node> map, int[] size)
        {
            this.map = map;

        }

        public void writePath(State currentState)
        {
            //As left node will be first, the branches will be stored and reversed so we print from root to leaf
            List<State> list = SortPath(currentState);
            list.Reverse();
            

            //Prints all the nodes
            int index = 1;
            foreach (State node in list)
            {
                Console.WriteLine(index + ":\t" + node.GetDirection() + "\t(" + node.GetCurrentPosition().GetLocation().GetX() + ", " 
                    + node.GetCurrentPosition().GetLocation().GetY() + ")");
                index++;
            }
            writeTrail(list);
            Console.WriteLine("\nPress any key to continue");
            string menuChoice = Console.ReadLine();

        }

        protected List<State> SortPath(State currentState)
        {
            List<State> reorder = new List<State>();
            
            
            //Collects all positions until reaching the root node
            while (currentState.GetPreviousPosition() != null)
            {
                reorder.Add(currentState);
                currentState = currentState.GetPreviousPosition();
            }
            //Ensures root node is added as we will be leaving the while loop
            reorder.Add(currentState);
            //reorder.Add(currentState.GetCurrentPosition());
            return reorder;
        }


        //Prints map
        public void writeTrail(List<State> list)
        {
            //Initialises index at top most node
            int index = map[0].GetX();
            Console.WriteLine("\nRobot Naviation Maze Solution");
            Console.WriteLine("------------------------------");
            

            foreach (Node location in map)
            {
                //when printing, I want all x values prints (with same y value) on the same line
                //When y value changes, we get a new line
                if (index < location.GetY())
                {
                    Console.WriteLine();
                    index++;
                }       
                //Loops through trail
                bool skipper = false;
                for(int i = 0; i < list.Count; i++)
                {
                    if (location.GetX() == list[i].GetCurrentPosition().GetLocation().GetX() && location.GetY() == list[i].GetCurrentPosition().GetLocation().GetY())
                    {
                        //Prints start point
                        if(i == 0)
                        {
                            Console.Write("|S|");
                        }
                        //Prints end point
                        else if(i == list.Count - 1)
                        {
                            Console.Write("|F|");
                        }
                        //Prints trail path
                        else
                        {
                            Console.Write("|.|");
                        }
                        skipper = true;
                        break;
                    }

                }
                //Ensures that remaining things below are not written
                //Otherwise this will make a row longer
                if(skipper == true)
                {
                    continue;
                }
                //Prints empty node
                if (location.GetRoomType() == "EMPTY")
                {
                    Console.Write("| |");
                }
                //Prints remaining goal (if there are multiple)
                else if (location.GetRoomType() == "GOAL")
                {
                    Console.Write("|G|");
                }
                //Prints wall node
                else if (location.GetRoomType() == "WALL")
                {
                    Console.Write("|W|");
                }
            }
            Console.WriteLine("\n------------------------------");
            Console.WriteLine("S = Start");
            Console.WriteLine(". = Trail");
            Console.WriteLine("F = Finish");
            Console.WriteLine("\n");
        }



        //Prints map
        public void writeMap(Agent agent)
        {
            //Initialises index at top most node
            int index = map[0].GetX();
            Console.WriteLine("\n Robot Naviation Maze");
            Console.WriteLine("----------------------");
            foreach (Node location in map)
            {
                //when printing, I want all x values prints (with same y value) on the same line
                //When y value changes, we get a new line
                if (index < location.GetY())
                {
                    Console.WriteLine();
                    index++;
                }

                //Prints the agent on the map
                if(location.GetX() == agent.GetLocation().GetX() && location.GetY() == agent.GetLocation().GetY())
                {
                    Console.Write("|>|");
                    continue;
                }

                //Prints empty node
                if (location.GetRoomType() == "EMPTY")
                {
                    Console.Write("| |");
                }

                //Prints goal node
                else if(location.GetRoomType() == "GOAL")
                {
                    Console.Write("|G|");
                }

                //Prints wall node
                else if (location.GetRoomType() == "WALL")
                {
                    Console.Write("|W|");
                }
            }
            Console.WriteLine("\n----------------------");
            Console.WriteLine("W = Wall");
            Console.WriteLine("> = Agent");
            Console.WriteLine("G = Goal");
            Console.WriteLine("\n");
        }

    }
}
