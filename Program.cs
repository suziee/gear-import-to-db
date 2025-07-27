using System;
using System.Linq;
using System.Text.Json;
using DataImporter.Importers;
using DataImporter.Models;

namespace DataImporter
{
    class Program
    {
        public static void Main(string[] args)
        {
            var cmdArgs = ArgsParser.Parse(args);

            switch (cmdArgs.GearType)
            {
                case "cam":
                    {
                        var cams = ReadJsonfile<Cam>(cmdArgs.InputFile);
                        CamImporter.Import(cams);
                        return;
                    }
                case "carabiner":
                    {
                        var carabiners = ReadJsonfile<Carabiner>(cmdArgs.InputFile);
                        CarabinerImporter.Import(carabiners);
                        return;
                    }
                case "sling":
                    {
                        var slings = ReadJsonfile<Sling>(cmdArgs.InputFile);
                        SlingImporter.Import(slings);
                        return;
                    }
                case "stopper":
                    {
                        var stoppers = ReadJsonfile<Stopper>(cmdArgs.InputFile);
                        StopperImporter.Import(stoppers);
                        return;
                    }
                default: throw new Exception($"Unknown gear type: {cmdArgs.GearType}");
            }
        }

        private static T[] ReadJsonfile<T>(string filename)
        {
            var json = System.IO.File.ReadAllText(filename);
            return JsonSerializer.Deserialize<T[]>(json);
        }

        private class CmdArgs
        {
            public string GearType { get; set; }
            public string InputFile { get; set; }
        }

        private class ArgsParser
        {
            public static CmdArgs Parse(string[] args)
            {
                return new CmdArgs
                {
                    GearType = GetValue(args, "-t"),
                    InputFile = GetValue(args, "-i"),
                };
            }

            private static string GetValue(string[] args, string flag)
            {
                if (!args.Contains(flag))
                {
                    throw new Exception($"Missing command arg {flag}");
                }

                var flagIndex = Array.IndexOf(args, flag);
                var value = args.ElementAtOrDefault(flagIndex + 1);
                return value;
            }
        }
    }
}