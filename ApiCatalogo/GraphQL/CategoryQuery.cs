using ApiCatalogo.Repository;
using GraphQL;
using GraphQL.Types;

namespace ApiCatalogo.GraphQL
{
    public class CategoryQuery : ObjectGraphType
    {
        public CategoryQuery(IUnitofWork _context)
        {
            Field<CategoryType>("categoria",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>() { Name = "id"}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return _context.CategoriaRepository.GetById(c =>c.CategoriaId == id);
                });
            Field<ListGraphType<CategoryType>>("categorias",
                resolve: context =>
                {
                    return _context.CategoriaRepository.Get();
                });
            
        }
    }
}
