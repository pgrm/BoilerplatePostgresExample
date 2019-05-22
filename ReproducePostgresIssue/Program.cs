using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReproducePostgresIssue.Entities;

namespace ReproducePostgresIssue
{
    class Program
    {
        static Random rnd = new Random();

        static async Task Main(string[] args)
        {
            Task[] jobs = new Task[50];

            Console.WriteLine("Hello World!");
            var optionsBuilder = new DbContextOptionsBuilder<AntFarmContext>();
            optionsBuilder.UseNpgsql("User Id=postgres;Password=mysecretpassword;Server=127.0.0.1;Port=5432;Database=postgres;Pooling=true;Max Auto Prepare=3;");

            using (var dbContext = new AntFarmContext(optionsBuilder.Options))
            {
                await dbContext.Database.MigrateAsync();
                await GenerateBaseData(dbContext);
            }

            for (int i = 0; i < jobs.Length; i++)
            {
                jobs[i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        await RunRandomJob(optionsBuilder);
                    }
                });
            }

            Console.ReadLine();
        }

        private static async Task RunRandomJob(DbContextOptionsBuilder<AntFarmContext> optionsBuilder)
        {
            try
            {
                int randomCase = rnd.Next(8);

                using (var dbContext = new AntFarmContext(optionsBuilder.Options))
                {
                    switch (randomCase)
                    {
                        case 0:
//                            Console.WriteLine(nameof(GetHivesWithInsuredQueen));
                            await GetHivesWithInsuredQueen(dbContext);
                            break;
                        case 1:
//                            Console.WriteLine(nameof(GetAllAnts));
                            await GetAllAnts(dbContext);
                            break;
                        case 2:
//                            Console.WriteLine(nameof(GetAntsInHiveThatLikeGames));
                            await GetAntsInHiveThatLikeGames(dbContext);
                            break;
                        case 3:
//                            Console.WriteLine(nameof(GetAntsInHiveWhoAreLoyalAndDoNotPlayAround));
                            await GetAntsInHiveWhoAreLoyalAndDoNotPlayAround(dbContext);
                            break;
                        case 4:
//                            Console.WriteLine(nameof(GetDriversInHiveThatAreLoyalAndDontPlayGames));
                            await GetDriversInHiveThatAreLoyalAndDontPlayGames(dbContext);
                            break;
                        case 5:
//                            Console.WriteLine(nameof(GetRiskTakersInHiveThatAreNotLoyalPlayGamesAndQueenIsInsured));
                            await GetRiskTakersInHiveThatAreNotLoyalPlayGamesAndQueenIsInsured(dbContext);
                            break;
                        case 6:
//                            Console.WriteLine(nameof(InsertMoreAntsToHive));
                            await InsertMoreAntsToHive(5, dbContext);
                            break;
                        case 7:
//                            Console.WriteLine(nameof(InsertThronelessQueens));
                            await InsertThronelessQueens(3, dbContext);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task GenerateBaseData(AntFarmContext dbContext)
        {
            Random rnd = new Random();

            for (int i = 0; i < 1; i++)
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

                for (int j = 0; j < 1; j++)
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

                dbContext.Hives.Add(hive);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task<IReadOnlyList<Hive>> GetHivesWithInsuredQueen(AntFarmContext dbContext)
            => await dbContext.Hives.Where(h => h.Queen.HasLifeInsurance).ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetAllAnts(AntFarmContext dbContext)
            => await dbContext.Ants.ToListAsync();

        private static async Task<IReadOnlyList<Ant>> GetAntsInHiveThatLikeGames(AntFarmContext dbContext)
        {
            int hiveId = (await dbContext.Hives.FirstAsync()).Id;

            return await dbContext.Ants.Where(a => a.HiveId == hiveId && a.FavouriteAntGame != null).ToListAsync();
        }

        private static async Task<IReadOnlyList<Ant>> GetAntsInHiveWhoAreLoyalAndDoNotPlayAround(AntFarmContext dbContext)
        {
            int hiveId = (await dbContext.Hives.FirstAsync()).Id;

            return await dbContext.Ants.Where(a => a.HiveId == hiveId && a.IsLoyal && a.FavouriteAntGame == null)
                .ToListAsync();
        }

        private static async Task<IReadOnlyList<Ant>> GetDriversInHiveThatAreLoyalAndDontPlayGames(AntFarmContext dbContext)
        {
            int hiveId = (await dbContext.Hives.FirstAsync()).Id;

            return await dbContext.Ants.Where(a =>
                    a.HiveId == hiveId && a.IsLoyal && a.Job.Contains("Driver") && a.FavouriteAntGame == null)
                .ToListAsync();
        }

        private static async Task<IReadOnlyList<Ant>> GetRiskTakersInHiveThatAreNotLoyalPlayGamesAndQueenIsInsured(AntFarmContext dbContext)
        {
            int hiveId = (await dbContext.Hives.FirstAsync()).Id;

            return await dbContext.Ants.Where(a =>
                a.HiveId == hiveId && !a.IsLoyal && a.Job == "Risk Taker" && a.FavouriteAntGame != null &&
                a.Hive.Queen.HasLifeInsurance).ToListAsync();
        }

        private static async Task InsertMoreAntsToHive(int quantityOfAnts, AntFarmContext dbContext)
        {
            int hiveId = (await dbContext.Hives.FirstAsync()).Id;

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

                dbContext.Ants.Add(ant);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task InsertThronelessQueens(int queensQuantity, AntFarmContext dbContext)
        {
            for (int i = 0; i < queensQuantity; i++)
            {
                dbContext.Queens.Add(new Queen
                {
                    Name = $"Queen{i}",
                    AgeInDays = i,
                    HasLifeInsurance = true // If you're a queen without a throne, who's there to say you cannot have your own backed life insurance?
                });

                await dbContext.SaveChangesAsync();
            }
        }

    }
}