﻿using AutoMapper;
using MediatR;

public class BookmarkGetAllQuery : EntityCommand, IRequest<List<BookmarkArticleResponse>>
{
    public class BookmarkGetAllQueryHandler : IRequestHandler<BookmarkGetAllQuery, List<BookmarkArticleResponse>>
    {
        private readonly IBookmarkQueryRepository bookmarkRepository;
        private readonly IArticleCatalogHttpService articleCatalogHttpService;
        private readonly IMapper mapper;

        public BookmarkGetAllQueryHandler(
            IBookmarkQueryRepository bookmarkRepository,
            IArticleCatalogHttpService articleCatalogHttpService,
            IMapper mapper)
        {
            this.bookmarkRepository = bookmarkRepository;
            this.articleCatalogHttpService = articleCatalogHttpService;
            this.mapper = mapper;
        }

        public async Task<List<BookmarkArticleResponse>> Handle(
            BookmarkGetAllQuery request, 
            CancellationToken cancellationToken)
        {
            var bookmarks = await bookmarkRepository.GetAll(cancellationToken);
            if(bookmarks.Count == 0)
            {
                return [];
            }

            var articles = await articleCatalogHttpService.GetArticlesByIds(bookmarks.Select(x => x.ArticleId.ToString()));

            return mapper.Map<List<BookmarkArticleResponse>>(articles.Where(x=> bookmarks.Exists(f=>f.ArticleId == x.Id)));
        }
    }
}