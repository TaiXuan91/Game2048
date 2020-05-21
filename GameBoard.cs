using System;
using System.Collections.Generic;
using System.Text;

namespace Game2048
{
    class GameBoard
    {
        // game score
        private uint score = 0;

        // can't set score by other class
        public uint Score
        {
            get { return score; }
        }

        // game board size
        private readonly byte size = 4;
        public byte Size
        {
            get { return size; }
        }

        // game board
        public byte[,] board;

        public GameBoard(byte size = 4)
        {
            this.score = 0;
            this.size = size;
            this.board = new byte[size, size];
            ClearBoard();
        }

        public void ClearBoard()
        {
            // all set 0
            for (byte x = 0; x < this.size; x++)
                for (byte y = 0; y < this.size; y++)
                    this.board[x, y] = 0;
        }

        public void ClearRow(byte rowIndex) {
            // this function is useless
            for (byte y = 0; y < this.size; y++)
                this.board[rowIndex, y]=0;
        }

        public void Restart() {
            // clear board and set score to 0
            this.score = 0;
            ClearBoard();
        }

        public void CalculateScore()
        {
            uint s = 0;
            for (byte x = 0; x < Size; x++)
                for (byte y = 0; y < Size; y++)
                    if (board[x, y]>0)
                        s += (((uint)1) << board[x, y]);
            score = s;
        }

        public List<(byte, byte)> GetBlankList() {
            List < (byte, byte) > l = new List<(byte, byte)>();
            for (byte x = 0; x < Size; x++)
                for (byte y = 0; y < Size; y++)
                    if (board[x, y] <= 0)
                        l.Add((x,y));
            return l;
        }
    }
}
