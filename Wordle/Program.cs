using System;
using System.IO;

namespace Wordle
{
    class Program
    {
        public class Node<T>
        {
            public static int mone = 0;
            private T Value;
            private Node<T> Next;
            public static void Add()
            {
                mone++;
            }
            public Node(T x)
            {
                this.Value = x;
                this.Next = null;
                mone++;
            }
            public Node(T Value, Node<T> Next)
            {
                this.Value = Value;
                this.Next = Next;
            }
            public T GetValue()
            {
                return Value;
            }
            public Node<T> GetNext()
            {
                return Next;
            }
            public void SetNext(Node<T> Next)
            {
                this.Next = Next;
            }
            public void SetValue(T Value)
            {
                this.Value = Value;
            }
            public void Ret()
            {
                Console.WriteLine(Value);
            }
            public bool HasNext()
            {
                return this.Next != null;
            }
        }
        public static bool found(Node<string> p, string x)
        {
            Node<string> p1 = p;
            while (p1 != null)
            {
                if (p1.GetValue() == x)
                    return true;
                p1 = p1.GetNext();
            }
            return false;
        }
        public static int len(Node<string> p)
        {
            int count = 0;
            Node<string> p1 = p;
            while (p1 != null)
            {
                count++;
                p1 = p1.GetNext();
            }
            return count;
        }
        public static Node<string> txtToList(StreamReader sr)
        {
            Node<string> first = new Node<string>("");
            Node<string> p = first;
            string line = sr.ReadLine();
            while(line!=null)
            {
                p.SetNext(new Node<string>(line));
                p = p.GetNext();
                line = sr.ReadLine();
            }
            return first.GetNext();
        }
        public static string getWord(Node<string> p)
        {
            Random r = new Random();
            int x = r.Next(len(p));
            for (int i = 0; i < x; i++)
                p = p.GetNext();
            string word = p.GetValue();
            return word;
        }
        public static int found(char[] ch, char c)
        {
            for(int i = 0;i< ch.Length;i++)
            {
                if (c == ch[i])
                    return i;
            }
            return -1;
        }
        public static ConsoleColor [] checkWord(string word, string guess)
        {
            char[] wordArray = word.ToCharArray();
            char[] guessArray = guess.ToCharArray();
            ConsoleColor[] colorArray = new ConsoleColor[5];
            for (int i = 0; i < guessArray.Length; i++)
            {
                if (guessArray[i] == wordArray[i])
                {
                    colorArray[i] = ConsoleColor.Green;
                    wordArray[i] = ' ';
                    guessArray[i] = '-';
                }
            }
            for (int i = 0; i< guessArray.Length;i++)
            {
                int charIndex = found(wordArray, guessArray[i]);
                if (charIndex != -1)
                {
                    colorArray[i] = ConsoleColor.Yellow;
                    wordArray[charIndex] = ' ';
                }
                if (colorArray[i] == ConsoleColor.Black)
                    colorArray[i] = ConsoleColor.Gray;
            }
            return colorArray;
        }
        public static void printTable(string[] stringArray, ConsoleColor[][] colorArraysArray)
        {
            for(int i = 0; i<stringArray.Length; i++)
            {
                Console.WriteLine("-----------");
                for (int j = 0; j < colorArraysArray[i].Length; j++)
                {
                    Console.Write("|");
                    if (colorArraysArray[i] != null)
                        Console.BackgroundColor = colorArraysArray[i][j]; 
                    if (stringArray[i]!=null)
                        Console.Write(stringArray[i][j]);
                    else
                        Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("-----------");
        }
        public static void wordleGame(Node<string> words)
        {
            Console.ForegroundColor = ConsoleColor.White;
            int turns = 0;
            string[] guesses = new string[6];
            ConsoleColor[][] colors = new ConsoleColor[6][];
            for (int i =0;i<colors.Length;i++)
            {
                ConsoleColor[] color = new ConsoleColor[5];
                for (int j = 0; j < color.Length; j++)
                    color[j] = ConsoleColor.Black;
                colors[i] = color;
            }
            printTable(guesses, colors);
            string theWord = getWord(words);
            Console.WriteLine("Guess");
            string guess = Console.ReadLine();
            while(turns!=5 && guess!= theWord)
            {
                while(!found(words, guess))
                {
                    Console.WriteLine("Try a real word");
                    guess = Console.ReadLine();
                }
                guesses[turns] = guess;
                colors[turns] = checkWord(theWord, guess);
                printTable(guesses, colors);
                turns++;
                Console.WriteLine("Guess");
                guess = Console.ReadLine();
            }
            guesses[turns] = guess;
            colors[turns] = checkWord(theWord, guess);
            printTable(guesses, colors);
            if (guess == theWord)
                Console.WriteLine("you won!");
            Console.WriteLine($"The Word Was {theWord}");
        }
        static void Main(string[] args) 
        {
            StreamReader sr = new StreamReader("C:\\Users\\itama\\Downloads\\sgb-words.txt");
            Node<string> words = txtToList(sr);
            wordleGame(words);
        }
    }
}
