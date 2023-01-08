using Shortener.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.DTO;

public class AddPostRequest
{
    public Guid Id { get; set; }
    public string? Topic { get; set; }
    public string? PostDetails { get; set; }
    public string? AuthorName { get; set; }
}
