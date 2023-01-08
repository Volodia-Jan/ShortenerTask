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

public class PostsRepository : IPostsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Post> AddNewPost(Post post)
    {
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
        return post;
    }

    public async Task<List<Post>> GetAllPosts() => await _dbContext.Posts.Include("User").ToListAsync();
}
