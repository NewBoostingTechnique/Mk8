namespace Mk8.Core.Times;

public interface ITimeService
{
    Task<bool> ExistsAsync(string courseName, string playerName);

    Task<Time> InsertAsync(Time time);
}