using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class Moves
    {
        public Moves(){ }

        //Check that the space doesn't exist in the trail (otherwise we could have an endless loop)
        
        protected bool NotInTrail(State currentState, int x, int y)
        {
            //saves a copy of state to ensure it doesn't get overriden
            State copyState = new State(currentState.GetCurrentPosition());
            copyState.SetPreviousPosition(currentState.GetPreviousPosition());

            //Loops through branch trail until you reach the root
            while(copyState.GetPreviousPosition() != null)
            {
                //If existing node is already visited, don't visit this node
                if(x == copyState.GetPreviousPosition().GetCurrentPosition().GetLocation().GetX() && y 
                    == copyState.GetPreviousPosition().GetCurrentPosition().GetLocation().GetY())
                {
                    return true;
                }

                copyState = copyState.GetPreviousPosition();
            }
            //If the new path hasn't been visited, give authorisation to enter this node
            return false;
        }

        //Check that the space is free
        protected bool SpaceFree(List<Node> map, Agent agent, State currentState, string direction)
        {
            int x = agent.GetLocation().GetX();
            int y = agent.GetLocation().GetY();

            //Determines the direction the agent is going and changes the coordinates
            switch (direction)
            {
                case "north": y -= 1; break;
                case "south": y += 1; break;
                case "west": x -= 1; break;
                case "east": x += 1; break;
                default: return false;
            }

            //If the new path hasn't been visited, give authorisation to enter this node
            if (NotInTrail(currentState, x, y))
            {
                return false;
            }

            //Looks through the map for these coordinates
            foreach (Node location in map)
            {
                if (location.GetX() == x && location.GetY() == y)
                {
                    //Ensures that the location is not a wall
                    if(location.GetRoomType() == "WALL")
                    {
                        return false;
                    }
                    else
                    {
                        //Space is free if the coordinates are found on the map and not a wall
                        return true;
                    }
                }
            }
            //if the location is not on the map, return false
            return false;
        }
        
        public List<State> PickMoves(List<Node> map, State currentState, Agent agent)
        {
            //stores all possible moves (null values mean that moves is impossible)
            List<State> tempList = new List<State>();

            //Checks if all directions are free
            if (SpaceFree(map, agent, currentState, "north"))
            {
                //Adds new direction with state
                tempList.Add(MoveDirection(map, currentState, agent, "UP"));
            }
            
            if (SpaceFree(map, agent, currentState, "west"))
            {
                tempList.Add(MoveDirection(map, currentState, agent, "LEFT"));
            }
            
            if (SpaceFree(map, agent, currentState, "south"))
            {
                tempList.Add(MoveDirection(map, currentState, agent, "DOWN"));
            }

            if (SpaceFree(map, agent, currentState, "east"))
            {
                tempList.Add(MoveDirection(map, currentState, agent, "RIGHT"));
            }

            return tempList;
        }

        protected State MoveDirection(List<Node> map, State currentState, Agent agent, string direction)
        {
            State newState;
            int x = agent.GetLocation().GetX();
            int y = agent.GetLocation().GetY();

            //Determines the direction the agent is going and changes the coordinates
            switch (direction)
            {
                case "UP": y -= 1; break;
                case "DOWN": y += 1; break;
                case "LEFT": x -= 1; break;
                case "RIGHT": x += 1; break;
                default: return null;
            }

            //Finds new node on the map
            foreach (Node location in map)
            {
                if (location.GetX() == x && location.GetY() == y)
                {
                    //Reassigns agent location
                    Agent newAgent = new Agent();
                    newAgent.SetLocation(location);
                    
                    //Creates a new states and makes currentstate a previous state
                    newState = new State(newAgent);
                    newState.SetPathCost(currentState.GetSpacesRemaining());
                    newState.SetPreviousPosition(currentState);
                    newState.SetDirection(direction);

                    return newState;
                }
            }

            return null;
        }
    }
}
