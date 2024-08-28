using PollerBox.Models;
using System.Reflection;
using System.Text.Json;

namespace PollerBox.Features.Repositories;

public abstract class BaseFileRepository(ILogger logger)
{
	protected string[] GetFiles(string directoryPath)
	{
		try
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath);
			EnsurePathExistence(path);
			logger.LogDebug("GetFiles {directoryPath}", path);
			var dir = Directory.GetFiles(path);
			var files = Directory.GetFiles(path)
				.Where(f => f.EndsWith(".json"))
				.OrderBy(f => f)
				.ToArray();
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
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
			EnsurePathExistence(path);
			logger.LogDebug("GetFile {filePath}", path);

			using var contentStream = File.OpenRead(path);
			var element = await JsonSerializer.DeserializeAsync<TElement>(contentStream);
			return element ?? throw new Exception("File could not be loaded");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred in GetFile");
			throw;
		}
	}

	protected bool FileExists(string filePath)
	{
		try
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

			return File.Exists(path);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error checking file existence");
			throw;
		}
	}

	protected async Task<bool> SaveFile<TElement>(string filePath, TElement element, Stream? dataStream = null)
		where TElement : IIdObject
	{
		try
		{
			EnsurePathExistence(filePath);
			logger.LogDebug("SaveFile {filePath}", filePath);

			if (dataStream is not null && element is IFileContainer fileContainer)
			{
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath, fileContainer.Filename);
				using var fileStream = File.Create(path);
				await dataStream.CopyToAsync(fileStream);
			}

			var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath, $"{element.Id}.json");
			using var contentStream = File.Create(file);
			await JsonSerializer.SerializeAsync(contentStream, element);
			return true;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred in SaveFile");
			return false;
		}
	}
	protected bool DeleteFile<TElement>(string filePath, TElement element)
		where TElement : IIdObject
	{
		try
		{
			EnsurePathExistence(filePath);
			logger.LogDebug("SaveFile {filePath}", filePath);

			if (element is IFileContainer fileContainer)
			{
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath, fileContainer.Filename);
				File.Delete(path);

			}

			var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath, $"{element.Id}.json");
			File.Delete(file);
			logger.LogDebug("DeleteFile {filePath}", filePath);
			return true;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred in DeleteFile");
			return false;
		}
	}

	private static void EnsurePathExistence(string path)
	{
		path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
		try
		{
			if ((File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory)
			{
				return;
			}
		}
		catch
		{
			Directory.CreateDirectory(path);
		}
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}
}
