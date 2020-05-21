using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace Game2048
{
    class Game
    {
        // should be private
        private readonly GameRenderer gameRenderer;
        private readonly GameController gameController;
        private readonly GameBoard gameBoard;
        private readonly Random random;
        // game status
        private bool isGameEnd = false;
        public bool IsGameEnd {
            get { return isGameEnd; }
        }
        public bool quitGame = false;
        // no merge or move no random block
        private bool hasBlocksMerged;
        private bool hasBlocksMoved;

        public Game(GameRenderer gameRenderer, GameController gameController)
        {
            this.gameRenderer = gameRenderer;
            this.gameController = gameController;
            this.gameBoard = new GameBoard(4);
            this.random = new Random();
            RandomBlock();
            RandomBlock();
            // Show board 
            gameRenderer.DrawBoard(gameBoard);
            hasBlocksMerged = false;
            hasBlocksMoved = false;
        }

        public void GameEndInfo() {
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Final score is {gameBoard.Score}");
        }

        public void GameQuitInfo() {
            Console.WriteLine("You quited game!");
            Console.WriteLine($"Final score is {gameBoard.Score}");
        }

        public void Step() {
            switch (gameController.GetCommand()) {
                case "MoveLeft":
                    MoveLeft();
                    break;
                case "MoveUp":
                    MoveUp();
                    break;
                case "MoveRight":
                    MoveRight();
                    break;
                case "MoveDown":
                    MoveDown();
                    break;
                case "Quit":
                    quitGame = true;
                    break;
                default:
                    return;
            }
            // add random block
            if (hasBlocksMerged)
            {
                RandomBlock();
                hasBlocksMerged = false;
            }
            else if (hasBlocksMoved) {
                RandomBlock();
                hasBlocksMoved = false;
            }
            // check end game
            isGameEnd = EndGameCheck();
            // calculate score
            gameBoard.CalculateScore();
            // Show board 
            gameRenderer.DrawBoard(gameBoard);
        }

        private void RandomBlock() {
            List<(byte, byte)> l = gameBoard.GetBlankList();
            if (l.Count <= 0)
                return;
            (byte x,byte y) =l[random.Next(l.Count)];
            gameBoard.board[x, y] = (byte)(random.Next(2) + 1);
        }

        private void SqueezeRow(byte rowIndex)
        {
            // backup
            byte[] bkp = new byte[gameBoard.Size];
            for (int i = 0; i < gameBoard.Size; i++)
                bkp[i] = gameBoard.board[rowIndex, i];
            // save nonzero value
            List<byte> values = new List<byte>();
            for (int i = 0; i < gameBoard.Size; i++)
            {
                if (gameBoard.board[rowIndex, i] > 0)
                {
                    values.Add(gameBoard.board[rowIndex, i]);
                }
            }
            // move values
            byte colIndex = 0;
            foreach (var v in values)
            {
                gameBoard.board[rowIndex, colIndex] = v;
                colIndex++;
            }
            // set 0s
            while (colIndex < gameBoard.Size)
            {
                gameBoard.board[rowIndex, colIndex] = 0;
                colIndex++;
            }
            // detect move
            for (int i = 0; i < gameBoard.Size; i++)
                if (bkp[i] != gameBoard.board[rowIndex, i])
                {
                    hasBlocksMoved = true;
                    break;
                }
        }

        private void MergePairs(byte rowIndex)
        {
            byte colIndex = 0;
            // squeeze 0s
            SqueezeRow(rowIndex);
            while (colIndex + 1 < gameBoard.Size)
            {
                if (gameBoard.board[rowIndex, colIndex] != 0 &&
                    gameBoard.board[rowIndex, colIndex] == gameBoard.board[rowIndex, colIndex + 1])
                {
                    gameBoard.board[rowIndex, colIndex] += 1;
                    gameBoard.board[rowIndex, colIndex + 1] *= 0;
                    hasBlocksMerged = true;
                    colIndex += 2;
                }
                else { colIndex += 1; }
            }
            // squeeze new 0s
            SqueezeRow(rowIndex);
        }

        private void Rotate90Clockwise() {
            GameBoard tempBoard = new GameBoard(gameBoard.Size);
            for (byte x = 0; x < gameBoard.Size; x++)
                for (byte y = 0; y < gameBoard.Size; y++) {
                    tempBoard.board[y,gameBoard.Size-x-1] = gameBoard.board[x, y];
                }
            gameBoard.board = tempBoard.board;
        }

        private void MoveLeft() {
            for (byte i = 0; i < gameBoard.Size; i++) {
                MergePairs(i);
            }
        }

        private void MoveDown()
        {
            Rotate90Clockwise();
            for (byte i = 0; i < gameBoard.Size; i++)
            {
                MergePairs(i);
            }
            Rotate90Clockwise();
            Rotate90Clockwise();
            Rotate90Clockwise();
        }

        private void MoveRight()
        {
            Rotate90Clockwise();
            Rotate90Clockwise();
            for (byte i = 0; i < gameBoard.Size; i++)
            {
                MergePairs(i);
            }
            Rotate90Clockwise();
            Rotate90Clockwise();
        }

        private void MoveUp()
        {
            Rotate90Clockwise();
            Rotate90Clockwise();
            Rotate90Clockwise();
            for (byte i = 0; i < gameBoard.Size; i++)
            {
                MergePairs(i);
            }
            Rotate90Clockwise();
        }

        private bool RowNoPairs(byte rowIndex) {
            for (byte i = 0; i + 1 < gameBoard.Size; i++)
                if(gameBoard.board[rowIndex, i]>0)
                    if (gameBoard.board[rowIndex, i] == gameBoard.board[rowIndex, i + 1])
                        return false;
            return true;
        }

        private bool NoPairsCheck() {
            // check row
            for (byte i = 0; i + 1 < gameBoard.Size; i++)
                if (!RowNoPairs(i))
                    return false;
            Rotate90Clockwise();
            // check column
            for (byte i = 0; i + 1 < gameBoard.Size; i++)
                if (!RowNoPairs(i))
                {
                    Rotate90Clockwise();
                    Rotate90Clockwise();
                    Rotate90Clockwise();
                    return false;
                }
            Rotate90Clockwise();
            Rotate90Clockwise();
            Rotate90Clockwise();
            return true;
        }

        private bool EndGameCheck() {
            if (gameBoard.GetBlankList().Count <= 0 )
                if(NoPairsCheck())
                    return true;
            return false;
        }
    }
}
