using ApiCatalogo.Dtos;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers
{
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitofWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitofWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasProdutos()
        {
            var categoria = await _uof.CategoriaRepository.Get().ToListAsync();
            var categoriaDto = _mapper.Map<List<CategoriaDto>>(categoria);
            return categoriaDto;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDto>> Get()
        {
            var categoria = _uof.CategoriaRepository.Get();
            
            var categoriaDto = _mapper.Map<List<CategoriaDto>>(categoria);
            if (categoriaDto == null) return BadRequest();
            return categoriaDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> Get([FromQuery] CategoriaParameters categoriaParameters) 
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasPaginas(categoriaParameters);
            if(categorias == null) return NotFound("Categorias não encontradas.");

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDto = _mapper.Map<List<CategoriaDto>>(categorias);

            return categoriaDto;
        }
        /// <summary>
        /// Pega Cateogria Pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<CategoriaDto>> Get(int id)
        {
            var categorias = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categorias == null) return NotFound("Não encontrado");
            var categoriaDto = _mapper.Map<CategoriaDto>(categorias);

            return categoriaDto;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (categoriaDto == null) return BadRequest();
            _uof.CategoriaRepository.Add(categoria);
            await _uof.Commit();
            var categoriaNew = _mapper.Map<CategoriaDto>(categoria);
            return new CreatedAtRouteResult("ObterCategoria", new { categoria.CategoriaId }, categoriaNew);
        }
        /// <summary>
        /// Atualiza a Categoria de um item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoriaDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (id != categoria.CategoriaId) return BadRequest();

            _uof.CategoriaRepository.Update(categoria);
            await _uof.Commit();

            return Ok(categoriaDto);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDto>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria == null) return NotFound("Categoria não encontrado");

            _uof.CategoriaRepository.Delete(categoria);
            await  _uof.Commit();
            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return Ok(categoriaDto);
        }

    }
}
