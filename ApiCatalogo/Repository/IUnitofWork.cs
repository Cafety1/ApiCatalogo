namespace ApiCatalogo.Repository
{
    public interface IUnitofWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task Commit();
    }
}
