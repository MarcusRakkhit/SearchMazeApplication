
using System.Diagnostics;
using TheRobotNavigationProblem;

OpenFile readFile = null;

//Prints opening text
string ReadfileChoice = "";
ProgramOutputSub printMenu = new ProgramOutputSub();
Console.WriteLine("--------------------------------");
Console.WriteLine("SELECT TEST CASE NUMBER TO OPEN ('q' quit):");


while (true)
{
    ReadfileChoice = Console.ReadLine();
    
    if( ReadfileChoice == "q" ||  ReadfileChoice == "Q")
    {
        Environment.Exit(0);
    }

    //Checks if file number is valid
    if (int.TryParse(ReadfileChoice, out int result))
    {
        if(result >= 1 && result < 36)
        {
            string fileName = "RobotNav-test" + result + ".txt";
            Console.WriteLine("\nOPENING: " + fileName);

            //Gets file contents
            readFile = new OpenFile(fileName);
            break;
        }
        else
        {
            Console.WriteLine("\nRobotNav-test" + ReadfileChoice + ": Pick a number between 1 - 35:");
        }
        
    }
    else
    {
        Console.WriteLine("\nRobotNav-test" + ReadfileChoice + ": INVALID FILE, PLEASE TRY AGAIN:");

    }
}


//Defines map, gridsize and agent
List<Node> map = new List<Node>();
int[] gridSize;
Agent agent = new Agent();

//Create map components
readFile.CreateMap();
readFile.CreateGoals();
readFile.CreateWall();

//Assigns map, gridsize and agent
map = readFile.GetNodes();
gridSize = readFile.GetGridSize();
agent = readFile.CreateAgent();

while (true)
{
    
    //Prints the map
    PrintMap printer = new PrintMap(map, gridSize);
    printer.writeMap(agent);

    Console.WriteLine("\nSELECT AN ALGORITHM TO IMPLEMENT: \n1. BFS\n2. DFS\n3. GREEDY\n4. ASTAR\n5. Select Different Test Case\nQ. Quit\n");
    Console.Write("INPUT: ");
    string menuChoice = Console.ReadLine();
    
    //Picks algorithm
    switch (menuChoice.ToLower())
    {
        case "1":
            BFS bfs = new BFS(map, agent);
            //Starts search
            State solution = bfs.Search();
            //Writes solution
            if(solution != null) 
            {
                printer.writePath(solution);
            }
            break;
        case "2":
            DFS dfs = new DFS(map, agent);
            State solution2 = dfs.Search();
            if (solution2 != null)
            {
                printer.writePath(solution2);
            }
            break;
        case "3":
            Greedy greed = new Greedy(map, agent);
            State solution3 = greed.Search();
            if (solution3 != null)
            {
                printer.writePath(solution3);
            }
            break;
        case "4":
            AStar astar = new AStar(map, agent);
            State solution4 = astar.Search();
            if (solution4 != null)
            {
                printer.writePath(solution4);
            }
            break;
        case "5":
            //restarts application
            Console.WriteLine("\n\n\t");
            Process.Start("SearchMazeApplication.exe");
            Thread.Sleep(100);
            Environment.Exit(0);
            break;
        case "q":
            Environment.Exit(0);
            break;
        default: Console.WriteLine("INVALID RESPONSE"); break;
    }
}

