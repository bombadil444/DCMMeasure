using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DCMMeasure
{
    public class DatabaseHandler
    {
        // ENHANCEMENT: if this codebase was to get larger, would read variables like this from a .env file
        private readonly string DBConnection = "DevDB";

        private readonly IConfiguration appsettings;


        public DatabaseHandler(IConfiguration appsettings)
        {
            this.appsettings = appsettings;
        }


        public void InsertMeasurement(
            string instanceUID,
            string measurementType,
            string anatomicalFeature,
            float measurementValue
        )
        {
            try
            {
                using (
                    var context = new ApplicationDbContext(
                        new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseSqlServer(appsettings.GetConnectionString(DBConnection))
                            .Options
                    )
                )
                {
                    // Insert the values
                    context.Measurement.Add(new Measurement
                    {
                        InstanceUID = instanceUID,
                        MeasurementType = measurementType,
                        AnatomicalFeature = anatomicalFeature,
                        Value = measurementValue
                    });

                    // Commit the changes to the DB
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Database insert successful!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured during database insertion: {ex.Message}");
            }
        }
    }
}
