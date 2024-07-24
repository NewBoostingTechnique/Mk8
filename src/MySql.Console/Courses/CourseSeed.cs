namespace Mk8.MySql.Console.Courses;

internal static class CourseSeed
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Seeding Courses...");
        await Script.ExecuteAsync(connection, "Courses/CourseSeed.sql");
    }
}