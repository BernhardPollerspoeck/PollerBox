namespace PollerBox.Features.Repositories;

public interface IBaseRepository
{
    bool FileExists(string path);
}
