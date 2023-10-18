using Microsoft.AspNetCore.Http;

namespace PaintyTask.Application.Services.Interfaces;

public interface ISaveImgService
{
    Task<string> SaveImg(IFormFile img);
}