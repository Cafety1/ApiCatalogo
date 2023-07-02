using ApiCatalogo.Models;
using GraphQL.Types;

namespace ApiCatalogo.GraphQL
{
    public class CategoryType : ObjectGraphType<Categoria>
    {
        public CategoryType() 
        {
            Field(x => x.CategoriaId);
            Field(x => x.Nome);
            Field(x => x.ImagemUrl);

            Field<ListGraphType<CategoryType>>("categorias");
        }
    }
}
