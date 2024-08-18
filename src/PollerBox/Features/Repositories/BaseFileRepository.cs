using System.Text.Json;

namespace PollerBox.Features.Repositories;

public abstract class BaseFileRepository(ILogger logger)
{
    protected string[] GetFiles(string directoryPath)
    {
        try
        {
            logger.LogDebug("GetFiles {directoryPath}", directoryPath);

            var files = Directory.GetFiles(directoryPath);
            return files;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred in GetFiles");
            throw;
        }
    }
    protected async Task<TElement> GetFile<TElement>(string filePath) where TElement : class
    {
        try
        {
            logger.LogDebug("GetFile {filePath}", filePath);

            var contentStream = File.OpenRead(filePath);
            var element = await JsonSerializer.DeserializeAsync<TElement>(contentStream);
            return element ?? throw new Exception("File could not be loaded");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred in GetFile");
            throw;
        }
    }

    protected async Task SaveFile<TElement>(string filePath, TElement element)
    {
        try
        {
            logger.LogDebug("SaveFile {filePath}", filePath);

            var contentStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(contentStream, element);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred in SaveFile");
            throw;
        }
    }
    protected void DeleteFile(string filePath)
    {
        try
        {
            logger.LogDebug("DeleteFile {filePath}", filePath);

            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred in DeleteFile");
            throw;
        }
    }
}
