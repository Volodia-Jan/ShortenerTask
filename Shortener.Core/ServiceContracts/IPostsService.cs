using Shortener.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.ServiceContracts;

public interface IPostsService
{

    Task<PostResponse> AddNewPost(AddPostRequest? addPostRequest);

    Task<List<PostResponse>> GetAllPosts();
}