namespace Mk8.Core.Times;

internal interface ITimeData
{
    Task<bool> ExistsAsync(string courseId, string playerId);

    Task InsertAsync(Time time);
}