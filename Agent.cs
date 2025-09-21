using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class Agent
    {
        private Node location = new Node(0,0);

        public Agent(){ }

        //Sets node with agent's coordinates
        public void SetLocation(Node location){ this.location = location; }

        public Node GetLocation(){ return location; }
    }
}
