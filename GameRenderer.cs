using System;
using System.Collections.Generic;
using System.Text;

namespace Game2048
{
    class GameRenderer
    {
        private void SetConsoleColor(byte value)
        {
            ConsoleColor[] foregroundColorList = { ConsoleColor.White, ConsoleColor.White, ConsoleColor.White, ConsoleColor.White,
                ConsoleColor.White, ConsoleColor.White, ConsoleColor.White, ConsoleColor.White,
                ConsoleColor.Black,ConsoleColor.Black,ConsoleColor.Black,ConsoleColor.Black,
                ConsoleColor.Black,ConsoleColor.Black,ConsoleColor.Black,ConsoleColor.Black};
            ConsoleColor[] backgroundColorList = { ConsoleColor.Black, ConsoleColor.DarkGray,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen,
                ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta,ConsoleColor.DarkYellow,ConsoleColor.DarkRed,
                ConsoleColor.Green,ConsoleColor.Yellow,ConsoleColor.Gray,ConsoleColor.Blue,
                ConsoleColor.Red,ConsoleColor.Cyan,ConsoleColor.Magenta,ConsoleColor.White};
            Console.ForegroundColor = foregroundColorList[value];
            Console.BackgroundColor = backgroundColorList[value];

        }

        public void DrawBoard(GameBoard board)
        {
            byte x, y;
            // set cursor position to (0,0)
            Console.SetCursorPosition(0,0);
            // then draw
            Console.Write($"Game2048 {board.Score} points\n\n");

            for (x = 0; x < board.Size; x++)
            {
                for (y = 0; y < board.Size; y++)
                {
                    SetConsoleColor(board.board[x,y]);
                    Console.Write("       ");
                    Console.ResetColor();
                }
                Console.Write("\n");
                for (y = 0; y < board.Size; y++)
                {
                    SetConsoleColor(board.board[x,y]);
                    if (board.board[x,y] != 0)
                    {
                        // board save exp, but we show power
                        string s = (1<<board.board[x,y]).ToString();
                        int t = 7 - s.Length;
                        s = s.PadLeft(7 - t / 2);
                        s = s.PadRight(7);
                        Console.Write(s);
                    }
                    else
                    {
                        Console.Write("   .   ");
                    }
                    Console.ResetColor();
                }
                Console.Write("\n");
                for (y = 0; y < board.Size; y++)
                {
                    SetConsoleColor(board.board[x,y]);
                    Console.Write("       ");
                    Console.ResetColor();
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            Console.Write("        ←,↑,→,↓ or q        \n");
            //Console.Write("\033[A"); // one line up
        }
    }
}
