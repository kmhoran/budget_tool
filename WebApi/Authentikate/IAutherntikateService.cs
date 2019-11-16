using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApi.Authentikate
{
    public interface IAuthentikateService
    {
        Task<string> GetUser(HttpContext httpContext);
    }
}