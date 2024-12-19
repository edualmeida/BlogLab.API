public interface IArticleQueryRepository : IQueryRepository<Article>
{
    Task<ArticleResponse> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<List<ArticleResponse>> GetAll(QueryOrderBy orderBy, CancellationToken cancellationToken = default);
}