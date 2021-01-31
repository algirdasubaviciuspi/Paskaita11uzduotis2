using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Paskaita11uzduotis2{
    class Program
    {
        const string CFd = "..\\..\\..\\Duomenys2.txt";
        const string CFr = "..\\..\\..\\Rezultatai2.txt";
        static void Main(string[] args)
        {
            int nr = 0;            
            Queue Participants = new Queue();
            ReadFile(CFd, ref nr, ref Participants);

            if (File.Exists(CFr))
                File.Delete(CFr);
            if ((Participants.Count > 0) && (nr > 0))
            {
                PrintDataToConsole(Participants);
                TimeSpan TimeNr, TimeEnd;
                CalculiationTime(nr, out TimeNr, out TimeEnd, Participants);
                PrintDataToFile(CFr, nr, TimeNr, TimeEnd);
            }
            else
            {
                using (var fr = File.AppendText(CFr))                
                    fr.WriteLine("Sąrašas tuščias");                
            }
        }
        static void ReadFile(string file, ref int nr, ref Queue Participants)
        {           // nuskaito duomenis iš failo
            if (File.Exists(file))
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    nr = int.Parse(reader.ReadLine());
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts;
                        parts = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        string nameSurname = parts[0].Trim();
                        TimeSpan arrivalTime = TimeSpan.Parse(parts[1]);
                        Participants.Enqueue(new Participant(nameSurname, arrivalTime));
                    }
                }
            }
        }
        static void CalculiationTime(int nr, out TimeSpan TimeNr, out TimeSpan TimeEnd, Queue Participants)
        {           // skaičiuoja ...
            Console.WriteLine("Atsitiktinės perklausų trukmės");
            Participant firstParticipant = (Participant)Participants.Peek();
            TimeEnd = firstParticipant.arrivalTime;
            TimeNr = new TimeSpan();
            int i = 0;
            foreach (Participant element in Participants)
            {
                if (element.arrivalTime >= TimeEnd)
                {
                    TimeEnd = element.arrivalTime;
                    if (i+1 == nr)
                        TimeNr = new TimeSpan();    // ...kiek reikės laiko laukti 
                }
                else if (i+1 == nr)                 // ...nr-ąjam dalyviui
                    TimeNr = TimeEnd - element.arrivalTime;
                Random r = new Random();
                int rand = r.Next(2, 20);  // atsitiktinių skaičių generavimas
                Console.Write("{0} ",rand);
                TimeEnd += new TimeSpan(0, rand, 0); // ...ir perklausos pabaigą.
                i++;
            }
            Console.WriteLine();
        }
        static void PrintDataToConsole(Queue Participants)
        {           // į ekraną išveda tarpinius skaičiavimus
            foreach (Participant cust in Participants)
                Console.WriteLine(cust.ToString());
            Console.WriteLine();
        }
        static void PrintDataToFile(string file, int nr, TimeSpan TimeNr, TimeSpan TimeEnd)
        {           // į failą išveda atsakymus
            using (var fr = File.AppendText(file))
            {
                fr.WriteLine("Nr.{0} atvykęs dalyvis savo eilės perklausai turėjo laukti {1} min.",
                    nr, TimeNr.TotalMinutes);
                fr.WriteLine("Visi dalyviai buvo perklausyti {0}:{1}", TimeEnd.Hours, TimeEnd.Minutes);
            }
        }
    }
}
