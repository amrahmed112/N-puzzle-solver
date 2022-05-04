using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    struct Priority_Queue
    {
        List<Node> pqueue;

        public Priority_Queue(byte b)
        {
            pqueue = new List<Node>();
        }

        public void push(Node node, Dictionary<String, Node> dictionary)
        {
            if (pqueue.Count() == 0)
            {
                pqueue.Add(node);
                dictionary.Add(node.key, node);
            }
                
            else
            {
                if (dictionary.ContainsKey(node.key))
                {
                    if (dictionary[node.key].f > node.f)
                    {
                        Remove_Node_At(dictionary[node.key].list_location, dictionary);
                    }
                    else
                        return;
                }
                else
                {
                    pqueue.Add(node);
                    dictionary.Add(node.key, node);

                    Bubble_up(pqueue.Count - 1, dictionary);
                }

               
            }
        }

        public void pop(Dictionary<String, Node> dictionary)
        {
            Remove_Node_At(0, dictionary);
        }

        public void Remove_Node_At(int index, Dictionary<String, Node> dictionary)
        {

            Node temp = pqueue[index];
            pqueue[index] = pqueue[pqueue.Count - 1];
            pqueue[pqueue.Count - 1] = temp;
            dictionary.Remove(pqueue[pqueue.Count - 1].key);
            pqueue.RemoveAt(pqueue.Count - 1);
            if(index > pqueue.Count - 1)
            {
                return;
            }
            if (pqueue.Count > 0)
            {
                if (pqueue[index].f > pqueue[(index - 1) / 2].f)
                    Bubble_up(index, dictionary);
                else
                    Heaphy(index, dictionary); 
            }
        }

        public Node peek()
        {
            return pqueue[0];
        }

        public void Heaphy(int index, Dictionary<String, Node> dictionary)
        {
            Node temp;
            int i = index;
            while (i < pqueue.Count)
            {
                if ((2 * i) + 1 < pqueue.Count) //Left child exists
                {
                    if (pqueue[(2 * i) + 1].f < pqueue[i].f) //If left child is smaller than the parent(more priority)
                    {
                        if ((2 * i) + 2 < pqueue.Count) //Right child exists
                        {
                            if (pqueue[(2 * i) + 1].f < pqueue[(i * 2) + 2].f) //If left child is smaller than the right
                            {
                                //swap parent and left child
                                temp = pqueue[i];
                                pqueue[i] = pqueue[(2 * i) + 1];
                                pqueue[(2 * i) + 1] = temp;
                                i = (2 * i) + 1;
                                continue;
                            }
                            else // Otherwise
                            {
                                //swap parent and right child
                                temp = pqueue[i];
                                pqueue[i] = pqueue[(2 * i) + 2];
                                pqueue[(2 * i) + 2] = temp;
                                i = (2 * i) + 2;
                                continue;
                            }
                        }
                        else // Right child doesn't exist
                        {
                            //swap parent and left child
                            temp = pqueue[i];
                            pqueue[i] = pqueue[(2 * i) + 1];
                            pqueue[(2 * i) + 1] = temp;
                            i = (2 * i) + 1;
                            continue;
                        }
                    }
                    else if ((2 * i) + 2 < pqueue.Count) //Right child exists
                    {
                        if (pqueue[(2 * i) + 2].f < pqueue[i].f) //If right child is smaller than the parent
                        {
                            //swap parent and right child
                            temp = pqueue[i];
                            pqueue[i] = pqueue[(2 * i) + 2];
                            pqueue[(2 * i) + 2] = temp;
                            i = (2 * i) + 2;
                            continue;
                        }
                        else
                        {
                            dictionary[pqueue[i].key].list_location = i;
                            pqueue[i].list_location = i;
                            break;
                        }
                    }
                    else
                    {
                        dictionary[pqueue[i].key].list_location = i;
                        pqueue[i].list_location = i;
                        break;
                    }
                }
                else
                {
                    dictionary[pqueue[i].key].list_location = i;
                    pqueue[i].list_location = i;
                    break;
                }
            }
        }

        public void Bubble_up(int index, Dictionary<String, Node> dictionary)
        {

            int i = index;
            while (i > 0)
            {
                if (pqueue[i].f >= pqueue[(i - 1) / 2].f)
                {
                    dictionary[pqueue[i].key].list_location = i;
                    pqueue[i].list_location = i;
                    break;
                }
                else
                {
                    Node temp = pqueue[i];
                    pqueue[i] = pqueue[(i - 1) / 2];
                    pqueue[(i - 1) / 2] = temp;

                    i = (i - 1) / 2;
                }
            }

        }
        public int Count()
        {
            return pqueue.Count();
        }
    }
}
