using Shortener.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.Entities;

/// <summary>
/// Domain class that represents Urls table
/// </summary>
public class Url
{
    public Guid Id { get; set; }
    public string? BaseUrl { get; set; }
    public string? ShortUrl { get; set; }
    public Guid UserId { get; set; }
    public DateTime? CreatedDate { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
}