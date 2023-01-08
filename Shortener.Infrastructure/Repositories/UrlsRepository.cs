using Microsoft.EntityFrameworkCore;
using Shortener.Core.Entities;
using Shortener.Core.RepositoryContracts;
using Shortener.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Infrastructure.Repositories;

public class UrlsRepository : IUrlsRepository
{
    private readonly ApplicationDbContext _context;

    public UrlsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteById(Guid id)
    {
        var url = await FindById(id);
        if (url is null) return false;
        _context.URLs.Remove(url);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Url>> FindAll() => await _context.URLs.Include("User").ToListAsync();

    public async Task<Url?> FindByBaseUrl(string url)
    {
        var urls = await FindAll();
        return urls.FirstOrDefault(urlData => urlData.BaseUrl == url);
    }

    public async Task<Url?> FindById(Guid id)
    {
        var urls = await FindAll();
        return urls.FirstOrDefault(url => url.Id == id);
    }

    public async Task<Url?> FindByShortUrl(string shortUrl)
    {
        var urls = await FindAll();
        return urls.FirstOrDefault(url => url.ShortUrl == shortUrl);
    }

    public async Task<Url> Save(Url url)
    {
        await _context.AddAsync(url);
        await _context.SaveChangesAsync();

        return url;
    }
}
