using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepositorio;
        private readonly INumeroVillaRepositorio _numeroVillaRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepositorio, IMapper mapper, INumeroVillaRepositorio numeroVillaRepositorio)
        {
            _logger = logger;
            _villaRepositorio = villaRepositorio;
            _mapper = mapper;
            _response = new();
            _numeroVillaRepositorio = numeroVillaRepositorio;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas() 
        {
            try
            {
                _logger.LogInformation("Obtener Numero villas");
                IEnumerable<NumeroVilla> numeroVillaList = await _numeroVillaRepositorio.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);

            } catch (Exception ex) 
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id:int", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Error al traer Numero Villa con id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var numeroVilla = await _numeroVillaRepositorio.ObtenerRegistro(v => v.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            } catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var existeNombre = await _numeroVillaRepositorio.ObtenerRegistro(v => v.VillaNo == createDto.VillaNo);
                if (existeNombre != null)
                {
                    ModelState.AddModelError("NombreExiste", "El Numero de Villa ya existe");
                    return BadRequest(ModelState);
                }
                var existeVilla = await _villaRepositorio.ObtenerRegistro(v => v.Id == createDto.VillaId);
                if (existeVilla == null)
                {
                    ModelState.AddModelError("Clave Foranea", "El Id de Villa no existe");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroVillaRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var numeroVilla = await _numeroVillaRepositorio.ObtenerRegistro(v => v.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _numeroVillaRepositorio.Remover(numeroVilla);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            if(updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var existeVilla = await _villaRepositorio.ObtenerRegistro(v => v.Id == updateDto.VillaId);
            if (existeVilla == null)
            {
                ModelState.AddModelError("Clave Foranea", "El Id de Villa no existe");
                return BadRequest(ModelState);
            }
            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);
            await _numeroVillaRepositorio.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

    }
}
