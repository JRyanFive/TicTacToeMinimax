using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Engine
    {
        public GameBoard GameBoard { get; set; }

        public int[] FineBestNode()
        {
            //var bestNode = Minimax(3, true);
            var bestNode = MinimaxPrunning(10, true, -1, 1);

            return new int[] { bestNode.X, bestNode.Y };
        }

        //1 max, -1 min
        public Node Minimax(int depth, bool maxPlayer)
        {
            Node bestNode = null;

            if (depth == 0 || !GameBoard.GetOpenCells().Any())
            {
                return new Node() { Rank = Heuristic(GameBoard.Squares) };
            }
            else
            {
                var openCells = GameBoard.GetOpenCells();
                foreach (var openCell in openCells)
                {
                    GameBoard.SetMove(openCell[0], openCell[1], maxPlayer ? Cell.MAX : Cell.MIN);
                    var nextNode = Minimax(depth - 1, !maxPlayer);

                    if (bestNode == null ||
                        (maxPlayer && nextNode.Rank > bestNode.Rank) ||
                        (!maxPlayer && nextNode.Rank < bestNode.Rank))
                    {
                        bestNode = nextNode;
                        bestNode.X = openCell[0];
                        bestNode.Y = openCell[1];
                    }

                    GameBoard.SetMove(openCell[0], openCell[1], Cell.OPEN);
                }
            }

            return bestNode;
        }

        public Node MinimaxPrunning(int depth, bool maxPlayer, int alpha, int beta)
        {
            Node bestNode = null;

            if (depth == 0 || !GameBoard.GetOpenCells().Any())
            {
                return new Node() { Rank = Heuristic(GameBoard.Squares) };
            }
            else
            {
                var openCells = GameBoard.GetOpenCells();
                foreach (var openCell in openCells)
                {
                    GameBoard.SetMove(openCell[0], openCell[1], maxPlayer ? Cell.MAX : Cell.MIN);
                    var nextNode = Minimax(depth - 1, !maxPlayer);

                    if (bestNode == null ||
                        (maxPlayer && nextNode.Rank > bestNode.Rank) ||
                        (!maxPlayer && nextNode.Rank < bestNode.Rank))
                    {
                        bestNode = nextNode;
                        bestNode.X = openCell[0];
                        bestNode.Y = openCell[1];
                    }

                    if (maxPlayer && nextNode.Rank > alpha)
                    {
                        alpha = nextNode.Rank;
                    }
                    if (!maxPlayer && nextNode.Rank < beta)
                    {
                        beta = nextNode.Rank;
                    }

                    GameBoard.SetMove(openCell[0], openCell[1], Cell.OPEN);

                    if (alpha >= beta) break;
                }
            }

            return bestNode;
        }

        public int Heuristic(Cell[][] squares)
        {
            int score = 0;
            int[,] lines = { { 0, 0, 1 , 0, 2, 0},
                           { 0, 1, 1, 1, 2, 1},
                           { 0, 2, 1, 2, 2, 2},
                           { 0, 0, 0, 1, 0, 2},
                           { 1, 0, 1, 1, 1, 2},
                           { 2, 0, 2, 1, 2, 2},
                           { 0, 0, 1, 1, 2, 2},
                           { 0, 2, 1, 1, 2, 0}
                           };

            for (int i = lines.GetLowerBound(0); i <= lines.GetUpperBound(0); i++)
            {
                score += GetScoreOneLine(new[] { squares[lines[i, 0]][lines[i, 1]], squares[lines[i, 2]][lines[i, 3]], squares[lines[i, 4]][lines[i, 5]] });
            }
            return score;
        }

        // X=1 MAX, O=-1 MIN
        public int GetScoreOneLine(Cell[] line)
        {
            int countX = 0, countO = 0;
            foreach (var value in line)
            {
                if (value == Cell.MAX)
                {
                    countX++;
                }
                if (value == Cell.MIN)
                {
                    countO++;
                }
            }

            if (countX == 0 && countO == 0)
            {
                return 0;
            }

            if (countX == 0)
            {
                return (-1) * (int)Math.Pow(10, countO);
            }
            if (countO == 0)
            {
                return (int)Math.Pow(10, countX);
            }
            return 0;
        }

        public Status CheckWinner()
        {
            int score = 0;
            int[,] lines = { { 0, 0, 1 , 0, 2, 0},
                           { 0, 1, 1, 1, 2, 1},
                           { 0, 2, 1, 2, 2, 2},
                           { 0, 0, 0, 1, 0, 2},
                           { 1, 0, 1, 1, 1, 2},
                           { 2, 0, 2, 1, 2, 2},
                           { 0, 0, 1, 1, 2, 2},
                           { 0, 2, 1, 1, 2, 0}
                           };

            for (int i = lines.GetLowerBound(0); i <= lines.GetUpperBound(0); i++)
            {
                score = GetScoreOneLine(new[] { GameBoard.Squares[lines[i, 0]][lines[i, 1]], GameBoard.Squares[lines[i, 2]][lines[i, 3]], GameBoard.Squares[lines[i, 4]][lines[i, 5]] });
                score /= 1000;
                if (score == 1)
                {
                    return Status.MAX;
                }
                if (score == -1)
                {
                    return Status.MIN;
                }
            }

            if (true)
            {
                if (GameBoard.GetOpenCells().Any())
                {
                    return Status.OPEN;
                }
            }
            return Status.UNKNOW;
        }
    }

    public enum Cell
    {
        OPEN = 0,
        MAX = 1,
        MIN = -1
    }

    public enum Status
    {
        UNKNOW = -10,
        OPEN = 0,
        MAX = 1,
        MIN = -1
    }

    public class GameBoard
    {
        public Cell[][] Squares { get; set; }
        public void Init()
        {
            Squares = new Cell[3][];
            //Squares = new int[][] {
            //    new int[] { 0, 0, (int)Cell.OPEN },
            //    new int[] { 0, 1, (int)Cell.OPEN },
            //    new int[] { 0, 2, (int)Cell.OPEN },
            //    new int[] { 1, 0, (int)Cell.OPEN },
            //    new int[] { 1, 1, (int)Cell.OPEN },
            //    new int[] { 1, 2, (int)Cell.OPEN },
            //    new int[] { 2, 0, (int)Cell.OPEN },
            //    new int[] { 2, 1, (int)Cell.OPEN },
            //    new int[] { 2, 2, (int)Cell.OPEN }
            //};

            for (int x = 0; x < 3; x++)
            {
                Squares[x] = new Cell[3];
            }
        }

        public List<int[]> GetOpenCells()
        {
            List<int[]> result = new List<int[]>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (Squares[x][y] == Cell.OPEN)
                    {
                        result.Add(new int[] { x, y });
                    }
                }
            }

            return result;
        }

        public void SetMove(int x, int y, Cell value)
        {
            Squares[x][y] = value;
        }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Rank { get; set; }

        public Node()
        {
            X = -1;
            Y = -1;
        }
    }
}
