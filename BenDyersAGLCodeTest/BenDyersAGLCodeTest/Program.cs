using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        static void PrintCatsOfGender (JArray a, string gender)
        {

            var cats = from person in a
                            where (string)person["gender"] == gender
                       from pet in person["pets"]
                            where (string)pet["type"] == "Cat"
                            orderby (string)pet["name"]
                            select pet;
            Console.WriteLine();
            Console.WriteLine(gender);
            foreach (var cat in cats)
            {
                Console.WriteLine((string)cat["name"]);
            }
        }
        static async Task RunAsync()
        {
            //Get Content
            using (var client = new HttpClient())
            {
                //accept headers?
                var result = await client.GetStringAsync("http://agl-developer-test.azurewebsites.net/people.json");
                //Console.WriteLine(result);
                var a = JArray.Parse(result);
                PrintCatsOfGender(a, "Male");
                PrintCatsOfGender(a, "Female");

            }
            
            //Externalise config
        }
    }
}
