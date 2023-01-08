using Shortener.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.DTO;

public class UrlResponse
{
    public Guid? UrlId { get; set; }
    public string? Url { get; set; }
    public string? ShortUrl { get; set; }
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public DateTime? CreatedDate { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is not UrlResponse)
            return false;

        var response = (UrlResponse)obj;

        return UrlId.Equals(response.UrlId) &&
               Url == response.Url &&
               ShortUrl == response.ShortUrl &&
               UserId.Equals(response.UserId) &&
               Username == response.Username &&
               CreatedDate == response.CreatedDate;
    }

    public override int GetHashCode()
    {
        int result = UrlId.GetHashCode();
        result = 31 * result + (Url?.GetHashCode() ?? 0);
        result = 31 * result + (ShortUrl?.GetHashCode() ?? 0);
        result = 31 * result + (UserId?.GetHashCode() ?? 0);
        result = 31 * result + (Username?.GetHashCode() ?? 0);
        result = 31 * result + (CreatedDate?.GetHashCode() ?? 0);

        return result;
    }
}

public static class UrlExtension
{
    public static UrlResponse ToResponse(this Url url)
        => new()
        {
            UrlId = url.Id,
            Url = url.BaseUrl,
            ShortUrl = url.ShortUrl,
            UserId = url.UserId,
            Username = url?.User?.UserName,
            CreatedDate = url?.CreatedDate,
        };
}