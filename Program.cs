using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Cw2
{
    class Program
    {
        //biblioteka do json newtonsoft.JSON
        // JSonConver.SerializableObject();  
     
        static void Main(string[] args)
        {

            var InputPath = @"..\..\..\data\dane.csv";
            var OutputPathLog = @"..\..\..\output\log.txt";
            var OutputPathXML = @"..\..\..\output\daneXML.xml";
            var OutputPathJSON = @"..\..\..\output\daneJSON.json";
            var FileExtension = "xml";
            if (args.Length > 0)
            {
                if (args[0] != null)
                {
                    InputPath = @"dane.csv";
                }
                else
                {
                    InputPath = args[0];
                }

                if (args[2] != null)
                {
                    FileExtension = "xml";

                }
                else
                {
                    FileExtension = args[2];
                }

                if (args[1] != null)
                {
                    OutputPathXML = @"result.xml";
                }
                else
                {
                    if (args[2] == "json")
                        OutputPathJSON = args[1];
                }
            }
           



            //Sprawdzenie czy plik istnieje i czy sciezka poprawna
            try
            {
                if (!File.Exists(InputPath))
                {
                    throw new FileNotFoundException();
                }

                Path.GetPathRoot(InputPath);
            
            } catch (FileNotFoundException e1)
            {
                Console.WriteLine(e1.Message);
            } catch(ArgumentException e2)
            {
                Console.WriteLine(e2.Message);
            }

            long NewMediaCounter = 0;
            long ComputerScienceCounter = 0;

            var lines = File.ReadLines(InputPath);
            var AllStudents = new HashSet<Student>(new OwnComparer());
            List<Student> LogStudents = new List<Student>();

            foreach (var line in lines)
            {
                string[] data = line.Split(',', 9, StringSplitOptions.None);

                Boolean allFilled = IsAllFieldFilled(data);

                Student NewStudent = new Student
                {
                    Index = Convert.ToInt64(data[4]),
                    FirstName = data[0],
                    LastName = RemoveNumbersFromString(data[1]),
                    StudiesName = RemoveRedundantInformationFromString(data[2]),
                    StudiesType = data[3],
                    Birthdate = data[5],
                    Email = data[6],
                    MothersName = data[7],
                    FathersName = data[8]

                };

                if (allFilled)
                {

                    if (!AllStudents.Add(NewStudent))
                    {                       
                        LogStudents.Add(NewStudent);
                    }
                    else
                    {

                        if (NewStudent.StudiesName.Contains("Informatyka"))
                        {
                            ComputerScienceCounter++;
                        }
                        else
                        {
                            NewMediaCounter++;
                        }

                        AllStudents.Add(NewStudent);

                    }
                } else
                {
                    LogStudents.Add(NewStudent);
                }
            }

            Console.WriteLine("Liczba elementów: " + AllStudents.Count);
            Console.WriteLine("Liczba Sztuki nowych Mediów: " + NewMediaCounter);
            Console.WriteLine("Liczba Informatyki: " + ComputerScienceCounter);


            //Przygotowanie plików
            PrepareLog(OutputPathLog, LogStudents);
            
            if (FileExtension == "xml")
            {
                PrepareXMLFile(OutputPathXML, AllStudents, NewMediaCounter, ComputerScienceCounter);
            } else if(FileExtension == "json")
            {
                PrepareJSONFile(OutputPathJSON, AllStudents, NewMediaCounter, ComputerScienceCounter);
            } else
            {
                Console.WriteLine("Wprowadzone rozszerzenie jest nieprawidlowe, dostepne rozszerzenia wyjsciowe to xml i json");
            }
        }

        public static void PrepareLog(String path, List<Student> list )
        {
            StreamWriter OutputLogFile = new StreamWriter(path);
            foreach (var item in list)
            {
                OutputLogFile.Write(item.ToString());
            }

            OutputLogFile.Close();
        }
        public static void PrepareXMLFile(String path, HashSet<Student> list, long mediaStud, long itStud)
        {

            XElement[] arr = new XElement[list.Count];
            long i = 0;
            foreach (var stud in list)
            {
                arr[i] = new XElement("student", new XAttribute("indexNumber", stud.Index),
                    new XElement("fname", stud.FirstName),
                    new XElement("lname", stud.LastName),
                    new XElement("birthdate", stud.Birthdate),
                    new XElement("email", stud.Email),
                    new XElement("mothersName", stud.MothersName),
                    new XElement("fathersName", stud.FathersName),
                    new XElement("studies",
                        new XElement("name", stud.StudiesName),
                        new XElement("mode", stud.StudiesType)
                        )
                    );
                    i++;
            }

            XDocument OutputXMLFile = new XDocument(
                new XElement("uczelnia", new XAttribute("createdAt", DateTime.Today.ToShortDateString()), new XAttribute("author", "Rafał Sadowski"),
                    new XElement("studenci", arr),
                        new XElement("activeStudies",
                            new XElement("studies", new XAttribute("name", "Computer Science"), new XAttribute("numberOfStudents", itStud),
                            new XElement("studies", new XAttribute("name", "New Media Art"), new XAttribute("numberOfStudents", mediaStud)
                            )
                        )
                    )
                )
           );

         OutputXMLFile.Save(path);

        }
        public static void PrepareJSONFile(String path, HashSet<Student> list, long mediaStud, long itStud)
        {
            //TODO 
            XElement[] arr = new XElement[list.Count];
            long i = 0;
            foreach (var stud in list)
            {
                arr[i] = new XElement("student", new XAttribute("indexNumber", stud.Index),
                    new XElement("fname", stud.FirstName),
                    new XElement("lname", stud.LastName),
                    new XElement("birthdate", stud.Birthdate),
                    new XElement("email", stud.Email),
                    new XElement("mothersName", stud.MothersName),
                    new XElement("fathersName", stud.FathersName),
                    new XElement("studies",
                        new XElement("name", stud.StudiesName),
                        new XElement("mode", stud.StudiesType)
                        )
                    );
                i++;
            }

            XDocument OutputXMLFile = new XDocument(
                new XElement("uczelnia", new XAttribute("createdAt", DateTime.Today.ToShortDateString()), new XAttribute("author", "Rafał Sadowski"),
                    new XElement("studenci", arr),
                        new XElement("activeStudies",
                            new XElement("studies", new XAttribute("name", "Computer Science"), new XAttribute("numberOfStudents", itStud),
                            new XElement("studies", new XAttribute("name", "New Media Art"), new XAttribute("numberOfStudents", mediaStud)
                            )
                        )
                    )
                )
           );

            var xmlDocument = new XmlDocument();
            using (var xmlReader = OutputXMLFile.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            


            string json = JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented);            
            File.WriteAllText(path, json);

        }

        public static Boolean IsAllFieldFilled(string[] data)
        {
            foreach (var str in data)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }                
            }

            return true;
        }


        public static String RemoveNumbersFromString(string val)
        {
            return Regex.Replace(val, @"\d", "");
        }

        public static String RemoveRedundantInformationFromString(string val)
        {
            if (val.Contains("Sztuka Nowych Mediów"))
            {
                return val = "Sztuka Nowych Mediów";

            } else if (val.Contains("Informatyka"))
            {
                return val = "Informatyka";
            } else
            {
                return val;
            }
        }

        
    }
}
