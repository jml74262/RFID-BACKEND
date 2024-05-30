using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrinterBackEnd.Data;
using PrinterBackEnd.Models.Domain;
using PrinterBackEnd.Models.Dto.RFIDLabel;

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
        public async Task<ActionResult<IEnumerable<ProdEtiquetasRFID>>> GetRFIDLabels()
        {
            try
            {
                var rfidLabels = await _context.ProdEtiquetasRFID.ToListAsync();
                return Ok(rfidLabels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Create a post method that receives a 'ProdEtiquetasRFID' object and adds it to the 'ProdEtiquetasRFID' table
        [HttpPost]
        public async Task<ActionResult<ProdEtiquetasRFID>> PostRFIDLabel(PostRFIDLabeldto postRFIDLabeldto)
        {
            try
            {
                // Create a new 'ProdEtiquetasRFID' object
                var postRFIDLabel = new ProdEtiquetasRFID
                {
                    Id = postRFIDLabeldto.Id,
                    Area = postRFIDLabeldto.Area,
                    ClaveProducto = postRFIDLabeldto.ClaveProducto,
                    NombreProducto = postRFIDLabeldto.NombreProducto,
                    ClaveOperador = postRFIDLabeldto.ClaveOperador,
                    Operador = postRFIDLabeldto.Operador,
                    Turno = postRFIDLabeldto.Turno,
                    PesoTarima = postRFIDLabeldto.PesoTarima,
                    PesoBruto = postRFIDLabeldto.PesoBruto,
                    PesoNeto = postRFIDLabeldto.PesoNeto,
                    Piezas = postRFIDLabeldto.Piezas,
                    Trazabilidad = postRFIDLabeldto.Trazabilidad,
                    Orden = postRFIDLabeldto.Orden,
                    RFID = postRFIDLabeldto.RFID,
                    Status = postRFIDLabeldto.Status,
                };

                await _context.SaveChangesAsync();
                return Ok(postRFIDLabeldto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
