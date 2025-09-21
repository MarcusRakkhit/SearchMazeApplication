using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobotNavigationProblem
{
    internal class Node
    {
        //Map coordinates
        private int x;
        private int y;

        //Whether the node is empty, a wall or a goal
        private string roomType = "EMPTY";

        public Node(int addX, int addY)
        {
            x = addX;
            y = addY;
        }

        //Sets a node to be a GOAL or a WALL
        public void AssignFeature(string feature)
        {
            roomType = feature;
        }

        public int GetX() { return x; }
        public int GetY() { return y; }
        public string GetRoomType() { return roomType; }
    }
}
