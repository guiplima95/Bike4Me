using Bike4Me.Application.Abstractions.Storage;
using Bike4Me.Domain.Couriers;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Couriers.Commands;

public sealed class ImportCourierCnhCommandHandler(IBlobService blobService) : IRequestHandler<ImportCourierCnhCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(ImportCourierCnhCommand request, CancellationToken cancellationToken)
    {
        byte[] imageBytes;

        try
        {
            imageBytes = Convert.FromBase64String(request.ImagemCnh);
        }
        catch
        {
            return Result.Failure<Guid>(CnhErrors.InvalidImageBase64);
        }

        using var imageStream = new MemoryStream(imageBytes);

        await blobService.UploadAsync(imageStream, "image/jpeg", request.CourierId.ToString(), cancellationToken);

        return Result.Success(request.CourierId);
    }
}