using ApiCatalogo.Context;
using ApiCatalogo.Controllers;
using ApiCatalogo.Dtos;
using ApiCatalogo.Dtos.Mapping;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiCatalogoTest
{
    public class CategoriasUnitTestController
    {
        private readonly IMapper _mapper;
        private readonly IUnitofWork _unitofWork;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string ConnectionString = "Server=localhost;Database=CatalogoDB;Uid=root;Pwd=123456;";
        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cgf =>
            {
                cgf.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //caso não tenha o banco para dados estaticos.
            //DbUnitTestsMockInitializer db = new DbUnitTestsMockInitializer();
            //db.Seed(context);

            _unitofWork = new UnitOfWork(context);
        }

        //================================= TESTES UNITÁRIOS ==================================
        //Testar o método GET
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(_unitofWork, _mapper);

            //Ação do Teste
            var data = controller.Get();

            //Assert
            Assert.IsType<List<CategoriaDto>>(data.Value);
        }
    }
}
