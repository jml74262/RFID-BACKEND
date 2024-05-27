﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrinterBackEnd.Data;
using PrinterBackEnd.Models.Domain;
using PrinterBackEnd.Models.Dto.Order;

namespace PrinterBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        // Create a get method that returns 'Orden' where 'UltimoProceso' matches the 'UltimoProceso' parameter
        [HttpGet("{UltimoProceso}")]
        public async Task<ActionResult<IEnumerable<CatOrden>>> GetCatOrden(String UltimoProceso)
        {
            try
            {
                // Get the 'Orden' where 'UltimoProceso' matches the 'UltimoProceso' parameter, fill OrderNumberResponse
                var order = await _context.Cat_Ordenes
                    .Where(x => x.UltimoProceso == UltimoProceso)
                    .Select(x => new OrderNumberResponse
                    {
                        Id = x.Id,
                        Orden = x.Orden
                    })
                    .ToListAsync();
                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
