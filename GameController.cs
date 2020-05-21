using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Game2048
{
    class GameController
    {
        public string GetCommand() {
            return Console.ReadKey(true).Key switch
            {
                ConsoleKey.UpArrow => "MoveUp",
                ConsoleKey.DownArrow => "MoveDown",
                ConsoleKey.LeftArrow => "MoveLeft",
                ConsoleKey.RightArrow => "MoveRight",
                ConsoleKey.Q => "Quit",
                _ => "Nothing"
            };

        }
    }
}
