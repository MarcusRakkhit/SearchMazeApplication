using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class Greedy
    {
        private List<State> stack = new List<State>();
        private List<Node> map;
        private Agent agent;
        private Moves moves = new Moves();
        private InformedSearchCost calculations = new InformedSearchCost();
        public Greedy(List<Node> newMap, Agent newAgent)
        {
            map = newMap;
            agent = newAgent;
            InitialiseStack();
        }

        //initalise stack with first agent state
        private void InitialiseStack()
        {
            State initial = new State(agent);
            //Initialises default spaces remaining and path cost
            calculations.CaluculateCost(initial, map);
            initial.SetPathCost(initial.GetSpacesRemaining());
            stack.Add(initial);
        }

        public void AddToStack(List<State> moves)
        {
            //All moves from last branch move are added
            foreach (State node in moves)
            {
                stack.Add(node);
            }
        }


        public State? Search()
        {
            State result = null;

            //Numbers of nodes expanded and found
            int spaceComplexity = 0;

            //How many times gone through search loop
            int timeComplextity = 0;

            //Loops through all stack elements until there's nothing left
            while (stack.Count > 0)
            {
                //Takes shortest element from stack and removes it
                State currentState = calculations.SelectedShortest(stack);
                agent = currentState.GetCurrentPosition();
                stack.Remove(currentState);

                //If you reach a goal, end the loop
                if (currentState.GetCurrentPosition().GetLocation().GetRoomType() == "GOAL")
                {
                    result = currentState;
                    Console.WriteLine("Solution Found with 'GREEDY SEARCH'-");
                    Console.WriteLine("Time complexity = " + timeComplextity + " ticks");
                    Console.WriteLine("Space complexity = " + spaceComplexity + " state nodes");
                    break;
                }

                //Gets the new list of moves
                List<State> newNodes = new List<State>();
                newNodes = moves.PickMoves(map, currentState, agent);

                //Adds each move
                foreach (State newState in newNodes)
                {
                    //Calculate shortest path to the goal
                    calculations.CaluculateCost(newState, map);

                    spaceComplexity++;
                    stack.Add(newState);
                }
                timeComplextity++;
            }

            //else statement execute if no solution found
            if (result != null)
            {
                return result;
            }
            else
            {
                Console.WriteLine("NO SOLUTION FOUND");
                Console.WriteLine("\nPress any key to continue");
                string menuChoice = Console.ReadLine();
            }

            return null;
        }
    }
}
