using Dapper;
using Microsoft.Data.Sqlite;
using System;
using DataImporter.Models;

namespace DataImporter.Importers
{
    class CamImporter
    {
        public static void Import(Cam[] cams)
        {
            // need to truncate table to avoid unique constraint error
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"DELETE FROM Cam";
                connection.Execute(sql);
            }

            foreach (var obj in cams)
            {
                obj.Guid = Guid.NewGuid();
                Insert(obj);
            }
        }

        private static void Insert(Cam obj)
        {
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"INSERT INTO Cam (
                    Guid,
                    Brand,
                    Model,
                    Color,
                    Size,
                    StrengthInKilonewtons,
                    UsableMinInMillimeters,
                    UsableMaxInMillimeters,
                    UsableMinInInches,
                    UsableMaxInInches,
                    WeightInGrams
                ) VALUES (
                    @Guid,
                    @Brand,
                    @Model,
                    @Color,
                    @Size,
                    @StrengthInKilonewtons,
                    @UsableMinInMillimeters,
                    @UsableMaxInMillimeters,
                    @UsableMinInInches,
                    @UsableMaxInInches,
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
                    UsableMinInMillimeters = obj.UsableMinInMillimeters,
                    UsableMaxInMillimeters = obj.UsableMaxInMillimeters,
                    UsableMinInInches = obj.UsableMinInInches,
                    UsableMaxInInches = obj.UsableMaxInInches,
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