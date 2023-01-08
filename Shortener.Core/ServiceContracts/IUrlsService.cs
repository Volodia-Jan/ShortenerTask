using Shortener.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.ServiceContracts;

public interface IUrlsService
{
    /// <summary>
    /// Adds Url details
    /// </summary>
    /// <param name="addUrlRequest">Url details to add</param>
    /// <returns>Added url details as UrlResponse object</returns>
    Task<UrlResponse> AddNewUrl(string? detinationUrl, string requestPath);

    /// <summary>
    /// Return list of Urls
    /// </summary>
    /// <returns>List of UrlResponse objects</returns>
    Task<List<UrlResponse>> GetAllUrls();

    /// <summary>
    /// Returns url details by ID
    /// </summary>
    /// <param name="urlId">Url ID to search</param>
    /// <returns>Url details as UrlResponse object</returns>
    Task<UrlResponse> GetUrlById(Guid? urlId);

    /// <summary>
    /// Deletes url by ID
    /// </summary>
    /// <param name="urlId">Url ID to delete</param>
    /// <returns>True if Url was deleted, otherwise false</returns>
    Task<bool> DeleteUrlById(Guid? urlId);

    /// <summary>
    /// Return Url details by shortUrl
    /// </summary>
    /// <param name="shortUrl">Short url used to search</param>
    /// <returns>Url details as UrlResponse object</returns>
    Task<UrlResponse> GetBaseUrlByShortUrl(string shortUrl);
}
