using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    class Node
    {
        public int[,] grid; // stat of the game, 0-based
        public int h; // hurastic value
        public int g; // cost to reach this node from the start node
        public int f; // h + g
        public int x; // x location of the empty slot
        public int y; // y loaction if the empty slot
        public int list_location;
        public Node parent;
        public static int size;
        public static string hurastic;
        static HashSet<string> final_State;
        static Tuple<int ,int>[] loactions;
        public string key;

        public Node(Node parent)
        {
            grid = parent.grid.Clone() as int[,];
            g = parent.g + 1;
            this.parent = parent;
            x = parent.x;
            y = parent.y;
            list_location = 0;
        }
        public Node(int[,] arr, int size, string hurastic)
        {
            f = 0;
            Node.size = size;
            Node.hurastic = hurastic;
            final_State = new HashSet<string>();
            final_State.Add(Final_State());
            Set_Loactions();

            grid = new int[size, size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    grid[i, j] = arr[i, j];
                    if (arr[i, j] == 0)
                    {
                        x = i;
                        y = j;
                    }
                }
            }
            Set_key();
        }

        public Node Move_Up()
        {
            Node child = new Node(this);
            child.grid[this.x, this.y] = this.grid[this.x + 1, this.y];
            child.grid[this.x + 1, this.y] = this.grid[this.x, this.y];
            child.x = this.x + 1;
            child.y = this.y;
            Find_Hurastic(hurastic);
            child.Set_key();
            return child;
        }

        public Node Move_Down()
        {
            Node child = new Node(this);
            child.grid[this.x, this.y] = this.grid[this.x - 1, this.y];
            child.grid[this.x - 1, this.y] = this.grid[this.x, this.y];
            child.x = this.x - 1;
            child.y = this.y;
            child.Find_Hurastic(hurastic);
            child.Set_key();
            return child;
        }

        public Node Move_Right()
        {
            Node child = new Node(this);
            child.grid[this.x, this.y] = this.grid[this.x, this.y - 1];
            child.grid[this.x, this.y - 1] = this.grid[this.x, this.y];
            child.y = this.y - 1 ;
            child.x = this.x;
            child.Find_Hurastic(hurastic);
            child.Set_key();
            return child;
        }

        public Node Move_Left()
        {
            Node child = new Node(this);
            child.grid[this.x, this.y] = this.grid[this.x, this.y + 1];
            child.grid[this.x, this.y + 1] = this.grid[this.x, this.y];
            child.y= this.y + 1;
            child.x = this.x;
            child.Find_Hurastic(hurastic);
            child.Set_key();
            return child;
        }

        public void Find_Hurastic(String hurastic)
        {
            if (hurastic == "humming")
            {
                h = 0;
                int count = 1;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (grid[i, j] != 0)
                        {
                            if (grid[i, j] != count)
                            {
                                h++;
                            }
                        }
                        count++;
                    }
                }
            }
            else if (hurastic == "manhattan")
            {
                h = 0; //sum of Manhattan distance
                int count = 1;
                for (int i = 0; i < Node.size; i++)
                {
                    for (int j = 0; j < Node.size; j++)
                    {
                        if (grid[i, j] != 0)
                        {
                            if (grid[i, j] != count)
                            {
                                int x = Math.Abs(loactions[(grid[i, j])].Item1 - i);
                                int y = Math.Abs(loactions[(grid[i, j])].Item2 - j);

                                h += x + y;
                            }
                        }
                        count++;
                    }
                }
            }
            f = h + g;
        }

        public void Print_State()
        {
            Console.WriteLine("Step #" + g);
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                { 
                    Console.Write(grid[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool Is_Solvable()
        {
            //convert the 2D array to 1D for easier calculations
            int k = 0;
            int[] arr = new int[size*size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    arr[k] = grid[i, j];
                    k++;
                }
            }

            //finding number of inversions
            int num_inver = 0;

            for(int i = 0; i < size * size; i++)
            {
                for(int j = i + 1; j < size * size; j++)
                {
                    if (arr[i] != 0 && arr[j] != 0 && arr[i] > arr[j])
                        num_inver++;
                }
            }

            if(size % 2 != 0) //if odd
            {
                if (num_inver % 2 == 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else //if even
            {
                if ((size - x) % 2 == 0 && num_inver % 2 != 0)
                    return true;
                else if ((size - x) % 2 != 0 && num_inver % 2 == 0)
                    return true;
                else 
                    return false;
            }
        }

        public string Final_State()
        {
            int count = 1;
            string str = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == size -1 && j == size - 1)
                    {
                        str = str + 0;
                        count++;
                    }
                    else
                    {
                        str = str + count;
                        count++;
                    }
                }
            }
            str = str.Remove(str.Length - 1, 1) + 0;
            return str;
        }

        public bool Is_Final_State()
        {
            return final_State.Contains(key);
        }

        void Set_Loactions()
        {
            loactions = new Tuple<int, int>[size * size + 1];
            int counter = 1;
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    loactions[counter] = new Tuple<int, int>(i, j);
                    counter++;
                }
            }
        }

        static public void Print_Steps(Node node)
        {
            Node temp = node;
            Stack<Node> steps = new Stack<Node>();
            while(temp.parent != null)
            {
                steps.Push(temp);
                temp = temp.parent;
            }
            while(steps.Count != 0)
            {
                steps.Peek().Print_State();
                steps.Pop();
            }
        }

        private void Set_key()
        {
            for (int i = 0; i < size; ++i)
            {
                for(int j = 0; j < size; ++j)
                {
                    key  = key + grid[i,j];
                }
            }
        }
    }
}
