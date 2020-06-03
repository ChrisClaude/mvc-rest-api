using MVC_REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_REST_API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CommanderContext context)
        {
            if (context.Commands.Any())
                return;

            var commands = new Command[]
            {
                new Command {HowTo="How to create migrations", Line="dotnet ef migrations add <Name of migrations>", Platform="EF Core"},
                new Command {HowTo="How to run migrations", Line="dotnet ef database update", Platform="EF Core"},
            };

            context.Commands.AddRange(commands);
            context.SaveChanges();
        }
    }
}
