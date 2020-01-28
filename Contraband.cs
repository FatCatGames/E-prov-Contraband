using System;
using System.Collections.Generic;

namespace Contraband
{
    class Program
    {
        abstract class Car
        {
            public int passengers;
            public int contrabandAmount;
            public bool alreadyChecked = false;
            public Random generator = new Random();
            public bool Examine()
            {
                alreadyChecked = true;

                if (contrabandAmount == 0) return false;
                int missChance = 20 - (contrabandAmount * 5); //1 = 15%, 2 = 10%, 3 = 5%, 4 = 0%
                int randomNumber = generator.Next(0, 101); //slumpar 100 tal
                return missChance <= randomNumber; //returnerar false om talet hamnar under missChance
            }
        }

        class CleanCar : Car
        {
            public CleanCar()
            {
                passengers = generator.Next(1, 4); //den andra parametern i en Random är exkluderad - Alltså går 1,4 egentligen endast från 1-3.
                contrabandAmount = 0;
            }
        }

        class ContrabandCar : Car
        {
            public ContrabandCar() //slumpar passagerare och stöldgods när bilen skapas
            {
                passengers = generator.Next(1, 5);
                contrabandAmount = generator.Next(1, 5);
            }
        }

        static void Main(string[] args)
        {
            int totalContrabandFound = 0;
            int totalContrabandMissed = 0;


            //Frågar hur många bilar som ska skapas
            int answer = 0;
            while (answer <= 0)
            {
                Console.Clear();
                Console.WriteLine("Hur många bilar ska skapas?");
                bool success = int.TryParse(Console.ReadLine(), out answer);
                if (!success) Console.WriteLine("Ogiltig inmatningsformat, snälla försök igen.");
            }

            //Skapar nya bilar
            List<Car> cars = new List<Car>();
            Random random = new Random();
            int cleanChance = 70;
            for (int i = 0; i < answer; i++)
            {
                bool isClean = (cleanChance >= random.Next(0, 101)); //om slumpade numret hamnar inom cleanChance blir isClean true.                
                //cars.Add(isClean ? new CleanCar(): new ContrabandCar()); det här hade varit snyggt men tyvärr funkar det inte
                Car newCar;
                if (isClean) newCar = new CleanCar(); else newCar = new ContrabandCar();
                cars.Add(newCar);

            }

            bool allCarsChecked = false;
            while (!allCarsChecked)
            {
                //Välj bil att kolla på
                Console.Clear();
                Console.WriteLine("Vilken bil vill du kolla på?");
                for (int i = 0; i < cars.Count; i++)
                {
                    Console.WriteLine(i + ": " + (!cars[i].alreadyChecked ? "ej " : "") + "undersökt");
                }

                //Kollar om du svarade rätt
                int carToCheck = -1;
                bool success = false;
                while (!success)
                {
                    success = int.TryParse(Console.ReadLine(), out carToCheck); //kollar att du skrev en siffra
                    if (carToCheck < 0 || carToCheck >= cars.Count) success = false; //kollar så att du inte överskred mängden bilar
                    if (!success) Console.WriteLine("Snälla svara med bilens siffra.");
                }

                //Kollar bilen
                if (cars[carToCheck].alreadyChecked) Console.WriteLine("Den där bilen har du redan tittat på!");
                else
                {
                    bool containsContraband = cars[carToCheck].Examine();
                    int contrabandAmount = cars[carToCheck].contrabandAmount;
                    if (containsContraband) totalContrabandFound += contrabandAmount; else { totalContrabandMissed += contrabandAmount; } //Hittade du eller missade stöldgods?
                    Console.WriteLine(containsContraband ? "Du hittade " + contrabandAmount + " stöldgods." : "Den här bilen verkar vara ren.");
                   
                }
                Console.ReadKey(true);
                //kollar om alla bilar har undersökts
                allCarsChecked = true;
                for (int i = 0; i < cars.Count; i++)
                {
                    if (cars[i].alreadyChecked == false) { allCarsChecked = false; break; } //bryter sig ur loopen om en bil inte har undersökts ännu                   
                }

            }
            Console.Clear();
            Console.WriteLine("Du har kollat på alla bilar! Du hittade " + totalContrabandFound + " stöldgods och missade " + totalContrabandMissed + ".");
            Console.ReadLine();
        }

    }
}
