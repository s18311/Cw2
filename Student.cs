using System;
using System.Collections.Generic;
using System.Text;

namespace Cw2
{
    public class Student
    {
        public long Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthdate { get; set; }
        public string Email { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }
        public string StudiesName { get; set; }
        public string StudiesType { get; set; }
        
        override
        public  string ToString()
        {
            return ($"STUDENT: {Environment.NewLine}" +
                $"Index: {Index.ToString()}, {Environment.NewLine}" +
                $"Imie:  { FirstName.ToString() }, Naziwsko: {LastName.ToString()},{Environment.NewLine}" +
                $"Data Urodzenia: {Birthdate.ToString()},{Environment.NewLine}" +
                $"Email: {Email.ToString()},{Environment.NewLine}" +
                $"Imie Matki: {MothersName.ToString()}, Imie Ojca: {FathersName.ToString()}, {Environment.NewLine}" +
                $"Kierunek Studiów: {StudiesName.ToString()}, Tryb: {StudiesType.ToString()}{Environment.NewLine}" +
                $"##########################################################################  {Environment.NewLine }  {Environment.NewLine }"
                ); 
        }
    }
}
