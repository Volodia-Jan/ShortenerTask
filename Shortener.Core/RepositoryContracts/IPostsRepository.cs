using Shortener.Core.DTO;
using Shortener.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.RepositoryContracts;

public interface IPostsRepository
{
    Task<Post> AddNewPost(Post post);

    Task<List<Post>> GetAllPosts();
}
