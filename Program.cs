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
            GameBoard g = new GameBoard();

            Engine e = new Engine();
            e.GameBoard = g;

            string startGame = "start";
            while (startGame == "start")
            {
                e.GameBoard.Init();

                while (true)
                {
                    var move = e.FineBestNode();
                    e.GameBoard.SetMove(move[0], move[1], Cell.MAX);
                    Draw(e.GameBoard.Squares);

                    if (Check(e.CheckWinner()))
                    {
                        break;
                    }

                    Console.WriteLine("ENTER X Y");
                    var x = Convert.ToInt32(Console.ReadLine());
                    var y = Convert.ToInt32(Console.ReadLine());

                    e.GameBoard.SetMove(x, y, Cell.MIN);
                    if (Check(e.CheckWinner()))
                    {
                        break;
                    }
                }

                Console.WriteLine("Restart GAME !!! (y/n)");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }

            Console.WriteLine("any key to exit.................................");
            Console.ReadLine();
        }

        static void Draw(Cell[][] squares)
        {
            Console.WriteLine("_______");
            string line1 = string.Format("|{0}|{1}|{2}|", drawCell(squares[0][0]), drawCell(squares[1][0]), drawCell(squares[2][0]));
            Console.WriteLine("_______");
            Console.WriteLine(line1);
            string line2 = string.Format("|{0}|{1}|{2}|", drawCell(squares[0][1]), drawCell(squares[1][1]), drawCell(squares[2][1]));
            Console.WriteLine("_______");
            Console.WriteLine(line2);
            string line3 = string.Format("|{0}|{1}|{2}|", drawCell(squares[0][2]), drawCell(squares[1][2]), drawCell(squares[2][2]));
            Console.WriteLine("_______");
            Console.WriteLine(line3);
            Console.WriteLine("_______");
        }

        static string drawCell(Cell cell)
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
    }
}
