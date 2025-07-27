using Dapper;
using Microsoft.Data.Sqlite;
using System;
using DataImporter.Models;

namespace DataImporter.Importers
{
    class SlingImporter
    {
        public static void Import(Sling[] slings)
        {
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"DELETE FROM Sling";
                connection.Execute(sql);
            }

            foreach (var obj in slings)
            {
                obj.Guid = Guid.NewGuid();
                Insert(obj);
            }
        }

        private static void Insert(Sling obj)
        {
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"INSERT INTO Sling (
                    Guid,
                    Brand,
                    Model,
                    StrengthInKilonewtons,
                    LengthInCentimeters,
                    WidthInMillimeters,
                    WeightInGrams
                ) VALUES (
                    @Guid,
                    @Brand,
                    @Model,
                    @StrengthInKilonewtons,
                    @LengthInCentimeters,
                    @WidthInMillimeters,
                    @WeightInGrams
                )";

                var rowsAffected = connection.Execute(sql, new
                {
                    Guid = obj.Guid.ToString(),
                    Brand = obj.Brand,
                    Model = obj.Model,
                    StrengthInKilonewtons = obj.StrengthInKilonewtons,
                    LengthInCentimeters = obj.LengthInCentimeters,
                    WidthInMillimeters = obj.WidthInMillimeters,
                    WeightInGrams = obj.WeightInGrams
                });

                if (rowsAffected != 1)
                {
                    throw new Exception("Failed to import cam to db");
                }
            }
        }
    }
}