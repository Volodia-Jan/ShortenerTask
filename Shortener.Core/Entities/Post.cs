using Shortener.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string? Topic { get; set; }
    public string? PostDetails { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    public ApplicationUser? User { get; set; }
}