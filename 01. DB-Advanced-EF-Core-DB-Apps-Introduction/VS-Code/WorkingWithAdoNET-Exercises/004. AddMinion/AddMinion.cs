using System;
using System.Linq;

namespace _004._AddMinion
{
    class AddMinion
    {
        static void Main(string[] args)
        {
            //Minion: Bob 14 Berlin
           Minion minionInput= InputLineReturnMinion();
            string[] secondLineInput = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string vilianName = secondLineInput[1];
            Console.WriteLine(vilianName);

            

        }

        private static Minion InputLineReturnMinion()
        {
            string[] firstLineInput = Console.ReadLine().Split(" ",StringSplitOptions.RemoveEmptyEntries);
            string minionName = firstLineInput[1];
            int minionAge = int.Parse(firstLineInput[2]);
            string minionTown = firstLineInput[3];
            Minion inputMinion = new Minion(minionName, minionAge, minionTown);
            return inputMinion;
        }

    }

    class Minion 
    {
        public Minion(string name, int age,string town)
        {
            this.Name = name;
            this.Age = age;
            this.Town = town;
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Town { get; set; }
    }
}
