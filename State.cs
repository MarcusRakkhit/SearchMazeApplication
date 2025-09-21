using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class State
    {
        private Agent currentPosition = new Agent();
        
        //For tree branches, this will make a trail to the root
        //If null, then this state is the root node for the agent
        private State? previousPosition = null;

        private string direction = "START";

        //This attributes are for informed search only
        private int spacesRemaining = 0;
        private int pathCost = 0;

        public State(Agent agent){ currentPosition = agent; }

        public Agent GetCurrentPosition(){ return currentPosition; }

        public State? GetPreviousPosition(){ return previousPosition; }

        public string GetDirection(){ return direction; }
        public int GetSpacesRemaining() { return spacesRemaining; }
        public int GetPathCost() { return pathCost; }

        public void SetPreviousPosition(State lastState){ previousPosition = lastState; }

        public void SetDirection(string newDirection){ direction = newDirection; }

        public void SetSpaces(int newCount) { spacesRemaining = newCount; }

        public void SetPathCost(int existingCost) { pathCost = existingCost; }

        public int FindTrailCost()
        {
            //Initalises cost
            int trailCost = 0;
            State cloner = this;

            //Counts path cost from the root branch
            while (cloner.GetPreviousPosition() != null)
            {
                trailCost++;
                cloner = cloner.GetPreviousPosition();
            }

            return trailCost;
        }
        
        //When adding path cost we will be calculating spaces left to the goal + path cose
        public void AddPathCost() 
        {
            //Initalises cost
            int trailCost = FindTrailCost();

            //Calculates cost
            pathCost = spacesRemaining + trailCost; 
        }
    }
}
