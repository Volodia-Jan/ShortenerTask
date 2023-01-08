using Shortener.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.DTO;

public class PostResponse
{
    public Guid Id { get; set; }
    public string? Topic { get; set; }
    public string? PostDetails { get; set; }
    public string? AuthorName { get; set; }
    public DateTime? CreatedAt { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is not PostResponse)
            return false;

        var response = (PostResponse)obj;

        return Id.Equals(response.Id) &&
               Topic == response.Topic &&
               PostDetails == response.PostDetails &&
               AuthorName == response.AuthorName &&
               CreatedAt == response.CreatedAt;
    }

    public override int GetHashCode()
    {
        int result = Id.GetHashCode();
        result = 31 * result + (Topic?.GetHashCode() ?? 0);
        result = 31 * result + (PostDetails?.GetHashCode() ?? 0);
        result = 31 * result + (AuthorName?.GetHashCode() ?? 0);
        result = 31 * result + (CreatedAt?.GetHashCode() ?? 0);

        return result;
    }
}

public static class PostExtensions
{
    public static PostResponse ToResponse(this Post post) => new()
    {
        Id = post.Id,
        Topic = post.Topic,
        PostDetails = post.PostDetails,
        AuthorName = post.User?.PersonName,
        CreatedAt = post.CreatedAt,
    };
}