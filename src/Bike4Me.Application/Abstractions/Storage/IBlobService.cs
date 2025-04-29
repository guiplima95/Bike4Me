namespace Bike4Me.Application.Abstractions.Storage;

public interface IBlobService
{
    Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default);

    Task UploadAsync(Stream stream, string contentType, string fileName, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default);
}