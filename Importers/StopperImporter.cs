using Dapper;
using Microsoft.Data.Sqlite;
using System;
using DataImporter.Models;

namespace DataImporter.Importers
{
    class StopperImporter
    {
        public static void Import(Stopper[] stoppers)
        {
            foreach (var obj in stoppers)
            {
                obj.Guid = Guid.NewGuid();
                Insert(obj);
            }
        }

        private static void Insert(Stopper obj)
        {
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"INSERT INTO Stopper (
                    Guid,
                    Brand,
                    Model,
                    Color,
                    Size,
                    StrengthInKilonewtons,
                    LengthInMillimeters,
                    WidthInMillimeters,
                    SmallerWidthInMillimeters,
                    WeightInGrams
                ) VALUES (
                    @Guid,
                    @Brand,
                    @Model,
                    @Color,
                    @Size,
                    @StrengthInKilonewtons,
                    @LengthInMillimeters,
                    @WidthInMillimeters,
                    @SmallerWidthInMillimeters,
                    @WeightInGrams
                )";

                var rowsAffected = connection.Execute(sql, new
                {
                    Guid = obj.Guid.ToString(),
                    Brand = obj.Brand,
                    Model = obj.Model,
                    Color = obj.Color,
                    Size = obj.Size,
                    StrengthInKilonewtons = obj.StrengthInKilonewtons,
                    LengthInMillimeters = obj.LengthInMillimeters,
                    WidthInMillimeters = obj.WidthInMillimeters,
                    SmallerWidthInMillimeters = obj.SmallerWidthInMillimeters,
                    WeightInGrams = obj.WeightInGrams
                });

                if (rowsAffected != 1)
                {
                    throw new Exception("Failed to import stopper to db");
                }
            }
        }
    }
}