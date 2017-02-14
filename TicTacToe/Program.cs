using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            var ddd = Convert.ToInt32("001001001", 2);
            var ddd2 = Convert.ToInt32("010010010", 2);

            Console.WriteLine("_______");
            Console.WriteLine("|1|2|3|");
            Console.WriteLine("_______");
            Console.WriteLine("|4|5|6|");
            Console.WriteLine("_______");
            Console.WriteLine("|7|8|9|");
            Console.WriteLine("_______");
            Console.WriteLine("START GAME!!" + Environment.NewLine + Environment.NewLine);
            GameBoard g = new GameBoard();

            Engine e = new Engine();
            e.GameBoard = g;

            bool comFirst = true;
            string startGame = "start";
            while (startGame == "start")
            {
                e.GameBoard.Init();

                //For testing
                //e.GameBoard.SetMove(2, 1, Cell.MIN);
                //e.GameBoard.SetMove(2, 2, Cell.MIN);
                //e.GameBoard.SetMove(0, 2, Cell.MIN);
                ////--e.GameBoard.SetMove(0, 1, Cell.MIN);

                //e.GameBoard.SetMove(0, 0, Cell.MAX);
                //e.GameBoard.SetMove(2, 0, Cell.MAX);
                //e.GameBoard.SetMove(1, 1, Cell.MAX);
                ////--e.GameBoard.SetMove(1, 0, Cell.MAX);
                ////--e.GameBoard.SetMove(1, 2, Cell.MAX);
                ////var test = e.Heuristic(e.GameBoard.Squares);

                if (!comFirst)
                {
                    Draw(e.GameBoard.Squares);
                    Console.WriteLine("ENTER cell num");
                    var num = Convert.ToInt32(Console.ReadLine());
                    var xy = GetXy(num);
                    e.GameBoard.SetMove(xy[0], xy[1], Cell.MIN);
                }

                while (true)
                {
                    var move = e.FineBestNode();
                    e.GameBoard.SetMove(move[0], move[1], Cell.MAX);

                    Draw(e.GameBoard.Squares);

                    if (Check(e.CheckWinner()))
                    {
                        break;
                    }

                    Console.WriteLine("ENTER cell num");
                    var num = Convert.ToInt32(Console.ReadLine());
                    var xy = GetXy(num);

                    e.GameBoard.SetMove(xy[0], xy[1], Cell.MIN);
                    if (Check(e.CheckWinner()))
                    {
                        break;
                    }
                }

                comFirst = !comFirst;
                Console.WriteLine("Restart GAME !!! (y/n)");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }

            Console.WriteLine("Press any key to exit.................................");
            Console.ReadLine();
        }

        static void Draw(Cell[][] squares)
        {
            Console.WriteLine("_______");
            string line1 = string.Format("|{0}|{1}|{2}|", DrawCell(squares[0][0]), DrawCell(squares[1][0]), DrawCell(squares[2][0]));
            Console.WriteLine("_______");
            Console.WriteLine(line1);
            string line2 = string.Format("|{0}|{1}|{2}|", DrawCell(squares[0][1]), DrawCell(squares[1][1]), DrawCell(squares[2][1]));
            Console.WriteLine("_______");
            Console.WriteLine(line2);
            string line3 = string.Format("|{0}|{1}|{2}|", DrawCell(squares[0][2]), DrawCell(squares[1][2]), DrawCell(squares[2][2]));
            Console.WriteLine("_______");
            Console.WriteLine(line3);
            Console.WriteLine("_______");
        }

        static string DrawCell(Cell cell)
        {
            if (cell == Cell.MAX)
            {
                return "X";
            }
            if (cell == Cell.MIN)
            {
                return "O";
            }
            return " ";
        }

        static bool Check(Status cellCheck)
        {
            if (cellCheck == Status.OPEN)
            {
                return false;
            }

            if (cellCheck == Status.MAX)
            {
                Console.WriteLine("Computer WIN!");
            }
            if (cellCheck == Status.MIN)
            {
                Console.WriteLine("Player WIN!");
            }
            if (cellCheck == Status.UNKNOW)
            {
                Console.WriteLine("Cat game ... we have a tie.");
            }

            return true;
        }

        static int[] GetXy(int num)
        {
            switch (num)
            {
                case 1:
                    return new int[] { 0, 0 };
                case 2:
                    return new int[] { 1, 0 };
                case 3:
                    return new int[] { 2, 0 };
                case 4:
                    return new int[] { 0, 1 };
                case 5:
                    return new int[] { 1, 1 };
                case 6:
                    return new int[] { 2, 1 };
                case 7:
                    return new int[] { 0, 2 };
                case 8:
                    return new int[] { 1, 2 };
                case 9:
                    return new int[] { 2, 2 };
            }
            return null;
        }
    }
}
