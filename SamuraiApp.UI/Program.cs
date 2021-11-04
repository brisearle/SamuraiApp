using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        private static SamuraiContextNoTracking _contextNT = new SamuraiContextNoTracking();
        static void Main(string[] args)
        {
            //AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
            //GetSamurais();
            //AddVariousTypes();
            //QueryFilters();
            //QueryAggregates();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //RetrieveAndDeleteSamurai();
            //QueryAndUpdateBattles_Disconnected();
            //InsertNewSamuraiWithAQuote();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(2);
            //Simpler_AddQuoteToExistingSamuraiNotTracked(3);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();
            //ExplicitLoadQuotes();
            //FiteringWithRelatedData();
            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();
            //AddingNewSamuraiToAnExistingBattle();
            //ReturnBattleWithSamurais();
            //AddAllSamuraisToAllBattles();
            //RemoveSamuraiFromABattle();
            //RemoveSamuraiFromABattleExplicit();
            //AddNewSamuraiWithHorse();
            //AddNewHorseToSamuraiUsinbgId();
            //AddNewHorseToDisconnectedSamuraiObject();
            //GetSamuraiWithHorse();
            //GetHorsesWithSamurai();
            //QuerySamuraiBattleStats();
            //QueryUsingRawSql();
            //QueryRelatedUsingRawSql();
            //DANGERQueryUsingRawSqlWithInterpolation();
            //QueryUsingFromSqlRawStoredProc();
            //ExecuteSomeRawSql()
            Console.Write("Press any Key...");
            Console.ReadKey();
        }
        private static void AddVariousTypes(params string[] names)
        {
            _context.AddRange(new Samurai { Name = "Shamada" },
                              new Samurai { Name = "Okamoto" },
                              new Battle { Name = "Battle of Anegawa" },
                              new Battle { Name = "Battle of Nagashino" });
            /*_context.Samurais.AddRange(
                new Samurai { Name = "Shimada" },
                new Samurai { Name = "Okamoto" });
            _context.Battles.AddRange(
                new Battle { Name = "Battle of Anegawa" },
                new Battle { Name = "Battle of Nagashino" });*/
            _context.SaveChanges();
        }
        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }
        private static void GetSamurais()
        {
            var samuraies = _contextNT.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"Samurai count is {samuraies.Count}");
            foreach (var samurai in samuraies)
            {
                Console.WriteLine(samurai.Name);
            }
        }
        private static void QueryFilters()
        {
            /*var name = "Sampson";
            var samurais = _context.Samurais.Where(s => s.Name == name).ToList();*/
            var filter = "J%";
            var samurais = _contextNT.Samurais
                .Where(s => EF.Functions.Like(s.Name, filter)).ToList();
        }
        private static void QueryAggregates()
        {
            //var name = "Sampson";
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
            var samurai = _contextNT.Samurais.Find(2);
        }
        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }
        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }
        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }
        private static void RetrieveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext { })
            {
                disconnectedBattles = _context.Battles.ToList();
            } //context1 is disposed
            disconnectedBattles.ForEach(b =>
                {
                    b.StartDate = new DateTime(1570, 01, 01);
                    b.EndDate = new DateTime(1570, 12, 1);
                });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }

        }
        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I've come to save you" },
                    new Quote {Text = "I told you to watch out for the sharp sword! Oh well!" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }
        private static void EagerLoadSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            //var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            var filterInclude =
                _context.Samurais.Where(s => s.Name.Contains("Sampson"))
                    .Include(s => s.Quotes).FirstOrDefault();
        }
        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }
        private static void ProjectSamuraisWithQuotes()
        {
            /*var somePropsWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count })
                .ToList();*/
            /*var somePropsWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name,
                                   HappyQuotes = s.Quotes.Where(q=>q.Text.Contains("happy")) })
                .ToList();*/
            var samuraisAndQuotes = _context.Samurais
                .Select(s => new
                {
                    Samurai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();
            var firstsamurai = samuraisAndQuotes[0].Samurai.Name += " The Happiest";
        }
        private static void ExplicitLoadQuotes()
        {
            //make sure there's a horse in the DB, then clear the context's change tracker
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            //-----------------
            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
        private static void FiteringWithRelatedData()
        {
            var samurais = _context.Samurais
                                   .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                                   .ToList();
        }
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                           .FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.SaveChanges();
        }
        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                           .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text = "Did you hear that again?";

            using var newContext = new SamuraiContext();
            //newContext.Quotes.Update(quote); // Resulted in all quotes being updated, inefficient
            newContext.Entry(quote).State = EntityState.Modified; // Entry method results in only one update
            newContext.SaveChanges();
        }
        private static void AddingNewSamuraiToAnExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
            _context.SaveChanges();
        }
        private static void ReturnBattleWithSamurais()
        {
            //var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
            var battle = _context.Battles.Include(b => b.Samurais).ToList();
        }
        private static void AddAllSamuraisToAllBattles()
        {
            // Causes a failure if the Samurai is already in a battle, could filter it out as shown
            /*var allbattles = _context.Battles.ToList();
            var allSamurais = _context.Samurais.Where(s => s.Id != 19).ToList();*/

            // Alternatively, could do the following which will only update modified changes but can 
            // but can be inefficient since it slows down the change tracker
            var allbattles = _context.Battles.Include(b => b.Samurais).ToList();
            var allSamurais = _context.Samurais.ToList();

            foreach (var battle in allbattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }
        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 12))
                .Single(s => s.BattleId == 1);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void RemoveSamuraiFromABattleExplicit()
        {
            var b_s = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);
            if (b_s != null)
            {
                _context.Remove(b_s); //_context.Set<BattleSamurai>().Remove() works too
                _context.SaveChanges();
            }
        }
        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Silver" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiUsinbgId()
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }
        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
            samurai.Horse = new Horse { Name = "Mr. Ed" };

            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }
        private static void GetSamuraiWithHorse()
        {
            var samurais = _context.Samurais.Include(s => s.Horse).ToList();
        }
        private static void GetHorsesWithSamurai()
        {
            // OPTION 1
            var horseonly = _context.Set<Horse>().Find(3);
            // OPTION 2
            var horseWithSamurai = _context.Samurais.Include(s => s.Horse)
                                           .FirstOrDefault(s => s.Horse.Id == 3);
            // OPTION 3
            var horseSamuraiPairs = _context.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();
        }
        private static void QuerySamuraiBattleStats()
        {
            //var stats = _context.SamuraiBattleStats.ToList();
            var firststat = _context.SamuraiBattleStats.FirstOrDefault();
            var sampsonState = _context.SamuraiBattleStats
                .FirstOrDefault(b => b.Name == "SampsonSan");
            // Below Code looks fine, but will fail because Object does not have a key for find 
            //var findone = _context.SamuraiBattleStats.Find(2);
        }
        private static void QueryUsingRawSql()
        {
            var samurais = _context.Samurais.FromSqlRaw("Select * from samurais").ToList();
        }
        private static void QueryRelatedUsingRawSql()
        {
            var samurais = _context.Samurais.FromSqlRaw(
                "Select Id, Name from Samurais").Include(s => s.Quotes).ToList();
        }
        private static void DANGERQueryUsingRawSqlWithInterpolation()
        {
            /**
             * Should use FromSqlInterpolated, this code is open to injection
             **/
            string name = "Kikuchyo";
            var samurais = _context.Samurais
                .FromSqlRaw($"Select * from Samurais Where Name= {name}")
                .ToList();
        }
        private static void QueryUsingFromSqlRawStoredProc()
        {
            var text = "Happy";
            //option
            /*var samurais = _context.Samurais.FromSqlRaw(
                "EXEC dbo.SamuraisWhoSaidAWord {0}", text).ToList();*/
            //Alternative
            var samurais = _context.Samurais.FromSqlInterpolated(
                $"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
        }
        private static void ExecuteSomeRawSql()
        {
            var samuraiId = 2;

            /*var affected = _context.Database
                .ExecuteSqlRaw("EXEC DeleteQuotesForSamurai {0}", samuraiId);*/
            
            // or using Interpolation
            var affected = _context.Database
                .ExecuteSqlInterpolated($"EXEC DeleteQuotesForSamurai {samuraiId}");
        }

    }
}
