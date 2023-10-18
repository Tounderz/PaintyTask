using Microsoft.AspNetCore.Http;
using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Domain.Models.Configurations;

namespace PaintyTask.Application.Services;

public class SaveImgService : ISaveImgService
{
    private readonly ImageConfig _imageConfig;

    public SaveImgService(ImageConfig imageConfig)
    {
        _imageConfig = imageConfig;
    }

    public async Task<string> SaveImg(IFormFile img)
    {
        var fileName = Guid.NewGuid().ToString() + img.FileName;
        var relativePath = Path.Combine(_imageConfig.RelativePath, fileName);
        var fullPath = Path.Combine(_imageConfig.ImagePath, fileName);
        if (!Directory.Exists(_imageConfig.ImagePath))
        {
            Directory.CreateDirectory(_imageConfig.ImagePath);
        }

        var stream = new FileStream(fullPath, FileMode.Create);
        await img.CopyToAsync(stream);
        await stream.FlushAsync();

        return relativePath;
    }
}