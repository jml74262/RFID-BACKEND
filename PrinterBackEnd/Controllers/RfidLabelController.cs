using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrinterBackEnd.Data;
using PrinterBackEnd.Models.Domain;

namespace PrinterBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RfidLabelController : ControllerBase
    {
        private readonly DataContext _context;

        public RfidLabelController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdEtiquetasRFID>>> GetCatArea()
        {
            try
            {
                return await _context.ProdEtiquetasRFID.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
