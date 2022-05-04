using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace N_Puzzle
{
    class Main_prog
    {
        static void Main(string[] args)
        {
            //Start to read matrix from file

            string[] lineItems;

            int size;

            //FileStream file = new FileStream("8 Puzzle (1).txt", FileMode.Open, FileAccess.Read); //8 moves
            //FileStream file = new FileStream("8 Puzzle (2).txt", FileMode.Open, FileAccess.Read); //20 moves
            //FileStream file = new FileStream("8 Puzzle (3).txt", FileMode.Open, FileAccess.Read); //14 moves
            //FileStream file = new FileStream("15 Puzzle - 1.txt", FileMode.Open, FileAccess.Read); //4
            //FileStream file = new FileStream("24 Puzzle 1.txt", FileMode.Open, FileAccess.Read); //11 moves
            FileStream file = new FileStream("24 Puzzle 2.txt", FileMode.Open, FileAccess.Read); //24 moves
            //FileStream file = new FileStream("TEST.txt", FileMode.Open, FileAccess.Read); //56 moves
            //FileStream file = new FileStream("9999 Puzzle.txt", FileMode.Open, FileAccess.Read); //4 moves
            //FileStream file = new FileStream("15 Puzzle 1.txt", FileMode.Open, FileAccess.Read); //46 moves
            //FileStream file = new FileStream("15 Puzzle - Case 3.txt", FileMode.Open, FileAccess.Read); //unsolvable
            //FileStream file = new FileStream("15 Puzzle 4.txt", FileMode.Open, FileAccess.Read); //44 moves
            //FileStream file = new FileStream("15 Puzzle 5.txt", FileMode.Open, FileAccess.Read); //45 moves
            //FileStream file = new FileStream("50 Puzzle.txt", FileMode.Open, FileAccess.Read); //18 moves
            //FileStream file = new FileStream("99 Puzzle - 2.txt", FileMode.Open, FileAccess.Read); //38 moves
            //FileStream file = new FileStream("hardest.txt", FileMode.Open, FileAccess.Read); //31 moves


            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            size = int.Parse(line);

            int[,] arr = new int[size, size];
            Console.WriteLine("matrix size =" + size);

            Console.WriteLine("=============================");

            sr.ReadLine();
            Console.Write("\nThe initial matrix is : \n");
            for (int i = 0; i < size; i++)
            {
                line = sr.ReadLine();
                lineItems = line.Split(' ');
                Console.Write("\n");
                for (int j = 0; j < size; j++)
                {

                    arr[i, j] = int.Parse(lineItems[j]);
                    Console.Write("{0}\t", arr[i, j]);

                }
            }
            Console.Write("\n");
            sr.Close();

            //////////////////////////////////////////////////
            var watch = new Stopwatch();
            watch.Start();
            ///////////////////////////////////////////////////



            //string hurastic = "humming";
            string hurastic = "manhattan";


            Node puzzle = new Node(arr, size, hurastic);
            if (puzzle.Is_Solvable())
            {
                Console.WriteLine("solvable");
                Console.WriteLine();
                int num_moves = A_Star(puzzle);
                Console.WriteLine("Number of moves = " + num_moves);
            }
            else
                Console.WriteLine("not solvable");

            Console.WriteLine("Elapsed time : {0}", watch.Elapsed);
        }

        static Queue finished_list = new Queue();
        static int A_Star(Node parent)
        {
            Priority_Queue opened_nodes = new Priority_Queue(0);
            Dictionary<String, Node> opened_nodes_dict = new Dictionary<String, Node>();
            Node q, child;

            opened_nodes.push(parent, opened_nodes_dict);
            if (parent.Is_Final_State())
            {
                //Node.Print_Steps(parent);
                return 0;
            }
            else if (parent.x + 1 < Node.size)
            {
                child = parent.Move_Up();
                child.Find_Hurastic(Node.hurastic);
                opened_nodes.push(child, opened_nodes_dict);
            }
            if (parent.x - 1 >= 0)
            {
                child = parent.Move_Down();
                child.Find_Hurastic(Node.hurastic);
                opened_nodes.push(child, opened_nodes_dict);
            }
            if (parent.y + 1 < Node.size)
            {
                child = parent.Move_Left();
                child.Find_Hurastic(Node.hurastic);
                opened_nodes.push(child, opened_nodes_dict);
            }
            if (parent.y - 1 >= 0)
            {
                child = parent.Move_Right();
                child.Find_Hurastic(Node.hurastic);
                opened_nodes.push(child, opened_nodes_dict);
            }

            opened_nodes.pop(opened_nodes_dict);
            finished_list.Enqueue(parent);
            q = opened_nodes.peek();

            while (opened_nodes.Count() > 0)
            {

                if (q.Is_Final_State())
                {
                    //Node.Print_Steps(q);
                    return q.g;
                }

                if (q.x + 1 <= Node.size - 1)
                {
                    child = q.Move_Up();
                    if (!opened_nodes_dict.ContainsKey(child.key))
                    {
                        child.Find_Hurastic(Node.hurastic);
                        opened_nodes.push(child, opened_nodes_dict);
                    }
                }
                if (q.x - 1 >= 0)
                {
                    child = q.Move_Down();
                    if (!opened_nodes_dict.ContainsKey(child.key))
                    {
                        child.Find_Hurastic(Node.hurastic);
                        opened_nodes.push(child, opened_nodes_dict);
                    }
                }
                if (q.y + 1 <= Node.size - 1)
                {
                    child = q.Move_Left();
                    if (!opened_nodes_dict.ContainsKey(child.key))
                    {
                        child.Find_Hurastic(Node.hurastic);
                        opened_nodes.push(child, opened_nodes_dict);
                    }
                }
                if (q.y - 1 >= 0)
                {
                    child = q.Move_Right();
                    if (!opened_nodes_dict.ContainsKey(child.key))
                    {
                        child.Find_Hurastic(Node.hurastic);
                        opened_nodes.push(child, opened_nodes_dict);
                    }
                }

                opened_nodes.pop(opened_nodes_dict);
                finished_list.Enqueue(q);
                q = opened_nodes.peek();
            }
            return 0;
        }
    }
}