using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            GetSamurais("Before Add:");
            AddSamurai();
            GetSamurais("After Add:");
            Console.WriteLine("Press any Key...");
            Console.ReadKey();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Sampson" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        
        private static void GetSamurais(string text)
        {
            var samuraies = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samuraies.Count}");
            foreach (var samurai in samuraies)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
