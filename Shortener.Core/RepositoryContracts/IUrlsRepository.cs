using Shortener.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.RepositoryContracts;

/// <summary>
/// Class that represents access the database
/// </summary>
public interface IUrlsRepository
{
    /// <summary>
    /// Saves URLs details into database
    /// </summary>
    /// <param name="url">URLs object to save</param>
    /// <returns>Saved URLs object</returns>
    Task<Url> Save(Url url);

    /// <summary>
    /// Featches all data from Database
    /// </summary>
    /// <returns>List of URLs</returns>
    Task<List<Url>> FindAll();

    /// <summary>
    /// Returns Url detail by its ID
    /// </summary>
    /// <param name="id">URLs id to search</param>
    /// <returns>Returns matching URLs object or null if it's not found</returns>
    Task<Url?> FindById(Guid id);

    /// <summary>
    /// Returns Url details by full url
    /// </summary>
    /// <param name="url">Url to searhc</param>
    /// <returns>Returns matching URLs object or null if it's not found</returns>
    Task<Url?> FindByBaseUrl(string url);

    /// <summary>
    /// Deletes Url by its ID
    /// </summary>
    /// <param name="id">Url id to delete</param>
    /// <returns>True if object was deleted, otherwise false</returns>
    Task<bool> DeleteById(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    Task<Url?> FindByShortUrl(string shortUrl);
}
