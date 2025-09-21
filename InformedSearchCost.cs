using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class InformedSearchCost
    {
        public InformedSearchCost() { }

        public State CaluculateCost(State agent, List<Node> map)
        {

            //Stores goals
            List<Node> goalNodes = new List<Node>();

            //Stores count from current space to goal
            List<int> goalSpaces = new List<int>();

            //Finds all goals
            foreach (Node location in map)
            {
                if (location.GetRoomType() == "GOAL")
                {
                    goalNodes.Add(location);
                }
            }

            //Gets distance to each goal
            foreach (Node goals in goalNodes)
            {
                int distance = Math.Abs(agent.GetCurrentPosition().GetLocation().GetX() - goals.GetX()) 
                    + Math.Abs(agent.GetCurrentPosition().GetLocation().GetY() - goals.GetY());
                goalSpaces.Add(distance);
            }

            //Selects shortest distance to a goal
            int smallest = goalSpaces.Min();
            agent.SetSpaces(smallest);

            //In A* search, add a path cost
            agent.AddPathCost();
            
            return agent;
        }

        public State SelectedShortest(List<State> stack) 
        {
            var directionOrder = new Dictionary<string, int>
            {
                { "START", 0 },
                { "UP", 1 },
                { "LEFT", 2 },
                { "DOWN", 3 },
                { "RIGHT", 4 },
            };

            //As we'll be reordering the stack, we don't want to alter the original by accident
            List<State> clonedStack = new List<State>(stack);

            //Reorders stack by spaces remaining and selects first element
            State smallestPath = clonedStack
                .OrderBy(direction => directionOrder[direction.GetDirection()])
                .OrderBy(state => state.GetSpacesRemaining())
                .First();

            return smallestPath;
        }

        //Same as Greedy but also orders by '.OrderBy(state => state.GetPathCost())'
        public State SelectedShortestAstar(List<State> stack)
        {
            var directionOrder = new Dictionary<string, int>
            {
                { "START", 0 },
                { "UP", 1 },
                { "LEFT", 2 },
                { "DOWN", 3 },
                { "RIGHT", 4 },
            };

            //As we'll be reordering the stack, we don't want to alter the original by accident
            List<State> clonedStack = new List<State>(stack);

            //Reorders stack by spaces remaining and selects first element
            State smallestPath = clonedStack
                .OrderBy(direction => directionOrder[direction.GetDirection()])
                .OrderBy(state => state.GetSpacesRemaining())
                .OrderBy(state => state.GetPathCost())
                .First();

            return smallestPath;
        }
    }
}
