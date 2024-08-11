namespace Mk8.MySql.Console.Courses;

internal static class CourseDeploy
{
    internal static async Task ExecuteAsync(MySqlConnection connection)
    {
        System.Console.WriteLine("Deploying Courses...");
        await Script.ExecuteAsync(connection, "Courses/CourseTable.sql");
        await Script.ExecuteAsync(connection, "Courses/CourseExists.sql");
        await Script.ExecuteAsync(connection, "Courses/CourseIdentify.sql");
        await Script.ExecuteAsync(connection, "Courses/CourseInsert.sql");
        await Script.ExecuteAsync(connection, "Courses/CourseList.sql");
    }
}
