using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReproducePostgresIssue.Entities;

namespace ReproducePostgresIssue
{
    class Program
    {
        private static AntFarmContext DbContext;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var optionsBuilder = new DbContextOptionsBuilder<AntFarmContext>();
            optionsBuilder.UseNpgsql("YourConnectionStringPlease");

            DbContext = new AntFarmContext(optionsBuilder.Options);

            GenerateBaseData().Wait();

            GetHivesWithInsuredQueen().Wait();

            GetAntsInHiveThatLikeGames(5).Wait();

            InsertThronelessQueens(10);

            GetRiskTakersInHiveThatAreNotLoyalPlayGamesAndQueenIsInsured(4);

        }

        private static async Task GenerateBaseData()
        {
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var hive = new Hive
                {
                    Name = $"Hive{i}",
                    Latitude = (decimal) rnd.NextDouble(),
                    Longitude = (decimal) rnd.NextDouble(),
                    Queen = new Queen
                    {
                        Name = $"Queen{i}",
                        AgeInDays = i,
                        HasLifeInsurance = i % 3 == 0
                    }
                };

                var ants = new List<Ant>();

                for (int j = 0; j < 100; j++)
                {
                    var ant = new Ant
                    {
                        Name = $"Ant{j}",
                        AgeInDays = i,
                        FavouriteAntGame = j % 2 == 0 ? $"Game{j}" : null,
                        IsLoyal = j % 5 == 0
                    };

                    int randomJob = rnd.Next(0, 3);

                    switch (randomJob)
                    {
                        case 0:
                            ant.Job = "Risk Taker";
                            break;
                        case 1:
                            ant.Job = "Diver";
                            break;
                        case 2:
                            ant.Job = "F1 Driver";
                            break;
                    }
                    ants.Add(ant);
                }

                hive.Ants = ants;

                DbContext.Hives.Add(hive);
            }

            await DbContext.SaveChangesAsync();
        }

        private static async Task<IReadOnlyList<Hive>> GetHivesWithInsuredQueen()
            => await DbContext.Hives.Where(h => h.Queen.HasLifeInsurance).ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetAntsThatLikeGames()
            => await DbContext.Ants.Where(x => x.FavouriteAntGame != null).ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetAntsInHiveThatLikeGames(int hiveId)
            => await DbContext.Ants.Where(a => a.HiveId == hiveId && a.FavouriteAntGame != null).ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetAntsInHiveWhoAreLoyalAndDoNotPlayAround(int hiveId)
            => await DbContext.Ants.Where(a => a.HiveId == hiveId && a.IsLoyal && a.FavouriteAntGame == null).ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetDriversInHiveThatAreLoyalAndDontPlayGames(int hiveId)
            => await DbContext.Ants.Where(a =>
                a.HiveId == hiveId && a.IsLoyal && a.Job.Contains("Driver") && a.FavouriteAntGame == null).ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetRiskTakersInHiveThatAreNotLoyalPlayGamesAndQueenIsInsured(int hiveId)
            => await DbContext.Ants.Where(a =>
                a.HiveId == hiveId && !a.IsLoyal && a.Job == "Risk Taker" && a.FavouriteAntGame != null &&
                a.Hive.Queen.HasLifeInsurance).ToListAsync();

        private static async Task InsertMoreAntsToHive(int quantityOfAnts, int hiveId)
        {
            var ants = new List<Ant>();

            var rnd = new Random();
            for (int i = 0; i < quantityOfAnts; i++)
            {
                var ant = new Ant
                {
                    Name = $"Ant{i}",
                    AgeInDays = i,
                    FavouriteAntGame = i % 2 == 0 ? $"Game{i}" : null,
                    IsLoyal = i % 5 == 0,
                    HiveId = hiveId
                };

                int randomJob = rnd.Next(0, 3);

                switch (randomJob)
                {
                    case 0:
                        ant.Job = "Risk Taker";
                        break;
                    case 1:
                        ant.Job = "Diver";
                        break;
                    case 2:
                        ant.Job = "F1 Driver";
                        break;
                }
                ants.Add(ant);
            }

            await DbContext.Ants.AddRangeAsync(ants);

            await DbContext.SaveChangesAsync();
        }

        private static async Task InsertThronelessQueens(int queensQuantity)
        {
            for (int i = 0; i < queensQuantity; i++)
            {
                DbContext.Queens.Add(new Queen
                {
                    Name = $"Queen{i}",
                    AgeInDays = i,
                    HasLifeInsurance = true // If you're a queen without a throne, who's there to say you cannot have your own backed life insurance?
                });
            }

            await DbContext.SaveChangesAsync();
        }

    }
}