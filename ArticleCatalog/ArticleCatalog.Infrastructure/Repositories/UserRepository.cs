﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;

internal class UserRepository : DataRepository<ArticleCatalogDbContext, User>,
    IUserDomainRepository
{
    private readonly IMapper mapper;

    public UserRepository(ArticleCatalogDbContext db, IMapper mapper)
        : base(db)
        => this.mapper = mapper;

    public async Task<User?> Find(Guid id, CancellationToken cancellationToken = default)
        => await All()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<UserResponse> GetById(Guid id, CancellationToken cancellationToken = default)
        => await mapper
            .ProjectTo<UserResponse>(AllAsNoTracking())
                .FirstAsync(b => b.Id == id, cancellationToken);

    public async Task<List<ArticleResponse>> GetAll(CancellationToken cancellationToken = default)
        => (await mapper
            .ProjectTo<ArticleResponse>(AllAsNoTracking())
                .ToListAsync(cancellationToken));

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await Data.Articles.FindAsync(id);

        if (article == null)
            throw new ArgumentException("Article does not exist");

        Data.Articles.Remove(article);

        await Data.SaveChangesAsync(cancellationToken);
    }
}