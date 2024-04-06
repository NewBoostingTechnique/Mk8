using System.Diagnostics;
using System.Text;

namespace Mk8.Web.Test.Mk8Instances;

internal sealed class Mk8Instance : IMk8Instance
{
    private readonly Uri? _baseUri = new($"https://localhost:7271/");

    private DirectoryInfo? _folder;

    private Process? _process;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    #region ISoftwareUnderTest.

    public async ValueTask<Uri> GetBaseUrlAsync()
    {
        if (_process is null || _process.HasExited)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_process is null)
                {
                    if (_folder is null || !_folder.Exists)
                        _folder = await PublishProjectAsync();

                    _process = new()
                    {
                        EnableRaisingEvents = true,
                        StartInfo = new()
                        {
                            FileName = "dotnet",
                            Arguments = $"Mk8.Web.dll --urls={_baseUri}",
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            WorkingDirectory = _folder.FullName
                        }
                    };

                    // TODO: Write errors to the test's stdout.
                    _process.Exited += OnProcessExit;

                    _process.Start();
                }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        return _baseUri!;
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        if (sender is Process process)
            _ = ThrowIfProcessErroredAsync(process);
    }

    private static async Task<DirectoryInfo> PublishProjectAsync()
    {
        DirectoryInfo outputFolder = Directory.CreateTempSubdirectory();

        try
        {
            DirectoryInfo? solutionFolder = (Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent)
                ?? throw new InvalidOperationException("Solution folder was not found at the expected relative location.");

            string sutProjectFilePath = $"{solutionFolder.FullName}/Web/Web.csproj";

            using Process process = new()
            {
                StartInfo = new()
                {
                    FileName = "dotnet",
                    Arguments = $"publish \"{sutProjectFilePath}\" -o \"{outputFolder.FullName}\"",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            await process.WaitForExitAsync();
            await ThrowIfProcessErroredAsync(process);
        }
        catch
        {
            if (outputFolder.Exists)
                outputFolder.Delete(true);

            throw;
        }

        return outputFolder;
    }

    private static async Task ThrowIfProcessErroredAsync(Process process)
    {
        if (process.ExitCode is 0)
            return;

        StringBuilder messageBuilder = new();
        messageBuilder.AppendLine("Failed to publish the Software Under Test.");
        messageBuilder.AppendLine();
        messageBuilder.AppendLine("Standard out:");
        messageBuilder.AppendLine(await process.StandardOutput.ReadToEndAsync());
        messageBuilder.AppendLine();
        messageBuilder.AppendLine("Standard error:");
        messageBuilder.AppendLine(await process.StandardError.ReadToEndAsync());

        throw new Exception(messageBuilder.ToString());
    }

    #endregion ISoftwareUnderTest.

    #region IDisposable.

    private bool _disposed;

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_process is not null)
            {
                if (!_process.HasExited)
                {
                    _process.Exited -= OnProcessExit;
                    _process.Kill(true);
                }
                _process.Dispose();
            }

            if (_folder is not null && _folder.Exists)
                _folder.Delete(true);

            _disposed = true;
        }
    }

    #endregion IDisposable.

}