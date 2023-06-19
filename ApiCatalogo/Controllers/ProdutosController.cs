using ApiCatalogo.Dtos;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProdutosController : ControllerBase
    {
        private readonly IUnitofWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitofWork appDbContext,IMapper mapper)
        {
            _uof = appDbContext;
            _mapper = mapper;
        }

        [HttpGet("menorPreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtoDto = _mapper.Map<List<ProdutoDto>>(produtos);

            return produtoDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produto = await _uof.ProdutoRepository.GetProdutos(produtosParameters);
            var metadata = new
            {
                produto.TotalCount,
                produto.PageSize,
                produto.CurrentPage,
                produto.TotalPages,
                produto.HasNext,
                produto.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));


            var produtosDto = _mapper.Map<List<ProdutoDto>>(produto);
            
            if (produto == null) return NotFound("Produtos não encontrados.");

            return produtosDto;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDto>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            
            if (produto == null) return NotFound("Produto não encontrado");
            var produtosDto = _mapper.Map<ProdutoDto>(produto);
            return produtosDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProdutoDto produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);
            if (produto == null) return BadRequest();

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoNew = _mapper.Map<Produto>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId },produtoNew);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (int id,ProdutoDto produtoDto) 
        {
            var produto = _mapper.Map<Produto>(produtoDto);
            if (id != produtoDto.ProdutoId) return BadRequest();

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();
            
            return Ok(produto);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDto>> Delete(int id)
        {
            
            var produtos = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produtos == null) return NotFound("Produto não encontrado");

            _uof.ProdutoRepository.Delete(produtos);
            await _uof.Commit();
            var produtosDto = _mapper.Map<ProdutoDto>(produtos);
            return Ok(produtosDto);

        }
    }
}
