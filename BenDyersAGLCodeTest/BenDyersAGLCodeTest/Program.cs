using System;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace BenDyersAGLCodeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ben Dyer's AGL Code Test");
            RunAsync().Wait();
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }


        //*******I would normally put tests around this.**********

        /// <summary>
        /// Takes a Json Array of People/Pets filters by gender of person and type of pet
        /// Orders by Name of Cat, and writes the output to a textWriter (funtion is not dependant 
        /// on Console for easier testing)
        /// </summary>
        /// <param name="jsonArray">The Newtonsoft.Json representation of the response of the 
        /// service call</param>
        /// <param name="gender">the gender to filter the results by</param>
        /// <param name="output">TextWriter to push output to</param>
        static void PrintCatsOfGender (JArray jsonArray, string gender, TextWriter output)
        {
            output.WriteLine();
            output.WriteLine(gender);

            (from person in jsonArray
             where (string)person["gender"] == gender
                       from pet in person["pets"]
                       where (string)pet["type"] == "Cat"
                       orderby (string)pet["name"]
                       select (string)pet["name"]).ToList<string>()
                       .ForEach(x => output.WriteLine(x));
           
        }

        /// <summary>
        /// Makes async Get Request to configured Service Url
        /// </summary>
        /// <returns>String representation of JSON resuts of service call </returns>
        static async Task<string> GetPeoplePetsStringFromAglService()
        {
            using (var client = new HttpClient())
            {
                var peoplePetsUrl = ConfigurationManager.AppSettings["agl.people-pets.url"];
                return await client.GetStringAsync(peoplePetsUrl);
            }
        }

        /// <summary>
        /// Top level coordinator function. Makes the service request, converts result from string to Json,
        /// filters and prints male and female ownes pets names alphabetically.
        /// </summary>
        /// <returns></returns>
        static async Task RunAsync()
        {
            var result = await GetPeoplePetsStringFromAglService();
            var a = JArray.Parse(result);
            PrintCatsOfGender(a, "Male", Console.Out);
            PrintCatsOfGender(a, "Female", Console.Out);
        }
    }
}
