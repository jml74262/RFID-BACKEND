using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrinterBackEnd.Data;

namespace PrinterBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        // Create get method that receives an 'Id' parameter from 'Cat_Maquina' and returns 'Cat_Productos' where 'Id' matches 'Maquina'
    }
}
