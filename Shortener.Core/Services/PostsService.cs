using Microsoft.EntityFrameworkCore.Infrastructure;
using Shortener.Core.DTO;
using Shortener.Core.Entities;
using Shortener.Core.Helper;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.Services;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepository;
    private readonly IUsersService _usersService;

    public PostsService(IPostsRepository postsRepository, IUsersService usersService)
    {
        _postsRepository = postsRepository;
        _usersService = usersService;
    }

    public async Task<PostResponse> AddNewPost(AddPostRequest? addPostRequest)
    {
        if (addPostRequest is null)
            throw new ArgumentNullException(nameof(addPostRequest));

        ValidationHelper.ModelValidation(addPostRequest);

        if (string.IsNullOrEmpty(addPostRequest.AuthorName))
            throw new ArgumentException("Author name can not be empty");

        var user = await _usersService.GetUserByEmail(addPostRequest.AuthorName);

        addPostRequest.Id = Guid.NewGuid();
        Post post = new()
        {
            Id = addPostRequest.Id,
            Topic = addPostRequest.Topic,
            PostDetails = addPostRequest.PostDetails,
            AuthorId = user.Id,
            CreatedAt = DateTime.Now
        };

        Post savedPost = await _postsRepository.AddNewPost(post);

        return savedPost.ToResponse();
    }

    public async Task<List<PostResponse>> GetAllPosts() =>
        (await _postsRepository.GetAllPosts())
            .Select(post => post.ToResponse())
            .ToList();
}