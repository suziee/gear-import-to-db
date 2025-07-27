using Dapper;
using Microsoft.Data.Sqlite;
using System;
using DataImporter.Models;

namespace DataImporter.Importers
{
    class CarabinerImporter
    {
        public static void Import(Carabiner[] carabiners)
        {
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"DELETE FROM Carabiner";
                connection.Execute(sql);
            }

            foreach (var obj in carabiners)
            {
                obj.Guid = Guid.NewGuid();
                Insert(obj);
            }
        }

        private static void Insert(Carabiner obj)
        {
            using (var connection = new SqliteConnection(Config.CONN_STRING))
            {
                const string sql = @"INSERT INTO Carabiner (
                    Guid,
                    Brand,
                    Model,
                    Type,
                    ClosedMajorAxisKilonewtons,
                    ClosedMinorAxisKilonewtons,
                    OpenedMajorAxisKilonewtons,
                    LengthInMillimeters,
                    WidthInMillimeters,
                    GateOpenClearanceInMillimeters,
                    WeightInGrams
                ) VALUES (
                    @Guid,
                    @Brand,
                    @Model,
                    @Type,
                    @ClosedMajorAxisKilonewtons,
                    @ClosedMinorAxisKilonewtons,
                    @OpenedMajorAxisKilonewtons,
                    @LengthInMillimeters,
                    @WidthInMillimeters,
                    @GateOpenClearanceInMillimeters,
                    @WeightInGrams
                )";

                var rowsAffected = connection.Execute(sql, new
                {
                    Guid = obj.Guid.ToString(),
                    Brand = obj.Brand,
                    Model = obj.Model,
                    Type = obj.Type,
                    ClosedMajorAxisKilonewtons = obj.ClosedMajorAxisKilonewtons,
                    ClosedMinorAxisKilonewtons = obj.ClosedMinorAxisKilonewtons,
                    OpenedMajorAxisKilonewtons = obj.OpenedMajorAxisKilonewtons,
                    LengthInMillimeters = obj.LengthInMillimeters,
                    WidthInMillimeters = obj.WidthInMillimeters,
                    GateOpenClearanceInMillimeters = obj.GateOpenClearanceInMillimeters,
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