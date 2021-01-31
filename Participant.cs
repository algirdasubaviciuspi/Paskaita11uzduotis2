using System;
using System.Collections;
using System.Text;

namespace Paskaita11uzduotis2
{
    class Participant 
    {
        public string name { get; set; }
        public TimeSpan arrivalTime { get; set; }
        public Participant() { }
        public Participant(string name, TimeSpan arrivalTime)
        {
            this.name = name;
            this.arrivalTime = arrivalTime;
        }
        public string ToString()
        {
            return String.Format("{0,-25} {1,20}", name, arrivalTime);
        }
    }
}
