using System;


namespace Game2048
{
    class Program
    {
        static void Main(string[] args)
        {
            // create new game
            GameController c = new GameController();
            GameRenderer g = new GameRenderer();
            Game ga = new Game(g, c);
            // main loop
            while (true)
            {
                ga.Step();
                if (ga.IsGameEnd)
                {
                    ga.GameEndInfo(); break;
                }
                if (ga.quitGame) { 
                    ga.GameQuitInfo(); break; 
                }
            }
            Console.WriteLine("Press Enter Back To System...");
            Console.ReadLine();
        }
    }
}
