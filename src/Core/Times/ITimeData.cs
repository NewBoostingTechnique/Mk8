namespace Mk8.Core.Times;

public interface ITimeData
{
    Task<bool> ExistsAsync(string courseId, string playerId);

    Task InsertAsync(Time time);
}