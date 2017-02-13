using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAR_MNGR
{
    class State
    {
        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                int maxVal = Folder.GetFileSystemInfos().Length;
                if (value >= 0 && value < maxVal)
                {
                    index = value;
                }
            }
        }
        public DirectoryInfo Folder { get; set; }
    }

    class Program
    {
        //private static object f;

        static void ShowFolderContent(State state)
        {
            Console.Clear();
            FileSystemInfo[] list = state.Folder.GetFileSystemInfos();
          

            for (int i = 0; i < list.Length; ++i)
            {
                if (state.Index == i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                if (list[i] is DirectoryInfo)
                {

                    Console.Write("[+] " + list[i]);
                }
                else {
                    Console.Write(list[i]);

                }
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }

        }

        static void Main(string[] args)
        {
            //Setting default color for manager
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            bool alive = true;
            State state = new State { Folder = new DirectoryInfo(@"C:\"), Index = 0 };
            Stack<State> layers = new Stack<State>();
            layers.Push(state);
            while (alive)
            {
                
                ShowFolderContent(layers.Peek());
                ConsoleKeyInfo pressedKey = Console.ReadKey();
               
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        layers.Peek().Index--;
                        break;
                    case ConsoleKey.DownArrow:
                        layers.Peek().Index++;
                        break;
                    case ConsoleKey.Escape:
                        if (layers.Count != 1)
                            layers.Pop();
                        break;
                    case ConsoleKey.Enter:
                        FileSystemInfo[] list = layers.Peek().Folder.GetFileSystemInfos();
                        if (list[layers.Peek().Index] is DirectoryInfo) {
                            State newState = new State
                            {
                                Folder = new DirectoryInfo(list[layers.Peek().Index].FullName),
                                Index = 0
                            };
                            layers.Push(newState);
                            break;
                        }

                        string extension = layers.Peek().Folder.GetFileSystemInfos()[layers.Peek().Index].Extension;
                        switch (extension)
                        {
                            case ".txt":
                                opentxt(list[layers.Peek().Index].FullName);
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Can't open :(");
                               
                                break;
                              
                        }
                        ConsoleKeyInfo pressedpauseKey;
                        do
                        {
                            //pause key, press Escape key to return
                            pressedpauseKey = Console.ReadKey();


                        }
                        while (pressedpauseKey.Key != ConsoleKey.Escape);
                        

                        
                        break;
                }

            }
        }

        private static void opentxt(string fullName)
        {

            StreamReader tmp = new StreamReader(new FileStream(fullName, FileMode.Open, FileAccess.Read));
            string s = tmp.ReadToEnd();
            Console.Clear();
            Console.Write(s);

        }
    }
}