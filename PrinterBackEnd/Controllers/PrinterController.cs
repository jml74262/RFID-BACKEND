using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PrinterBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : Controller
    {
        private readonly IConfiguration _configuration;

        public PrinterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("PrintLabel")]
        public IActionResult PrintLabel([FromBody] PrintRequest request)
        {
            // Validate and send print command
            var printResult = SendToPrinter(request);
            if (printResult)
            {
                return Ok("Print job successful.");
            }
            else
            {
                return StatusCode(500, "Print job failed.");
            }
        }

        private bool SendToPrinter(PrintRequest request)
        {
            string ipAddress = _configuration["PrinterSettings:IPAddress"];
            int port = int.Parse(_configuration["PrinterSettings:Port"]);

            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (NetworkStream stream = client.GetStream())
                {
                    string printCommand = CreatePrintCommand(request);
                    byte[] data = Encoding.ASCII.GetBytes(printCommand);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    Console.WriteLine($"Sent to printer: {printCommand.Replace("\x1B", "\\x1B").Replace("\x0A", "\\x0A")}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        private string CreatePrintCommand(PrintRequest request)
        {
            // Construct SBPL commands based on the provided documentation
            StringBuilder sbplCommand = new StringBuilder();
            sbplCommand.Append("\x1B" + "A\x0A"); // Start command
            sbplCommand.Append("\x1B" + "CS\x0A"); // Clear buffer

            // Print each field at specified positions
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0100\x1B" + "L0101\x1B" + "K\"" + request.Area + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0200\x1B" + "L0101\x1B" + "K\"" + request.Fecha + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0300\x1B" + "L0101\x1B" + "K\"" + request.Producto + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0400\x1B" + "L0101\x1B" + "K\"" + request.Turno + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0500\x1B" + "L0101\x1B" + "K\"" + request.Operador + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0600\x1B" + "L0101\x1B" + "K\"" + request.PesoBruto + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0700\x1B" + "L0101\x1B" + "K\"" + request.PesoNeto + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0800\x1B" + "L0101\x1B" + "K\"" + request.PesoTarima + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V0900\x1B" + "L0101\x1B" + "K\"" + request.Cantidad + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V1000\x1B" + "L0101\x1B" + "K\"" + request.CodigoTrazabilidad + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V1100\x1B" + "L0101\x1B" + "K\"" + request.Lote + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V1200\x1B" + "L0101\x1B" + "K\"" + request.Qr + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V1300\x1B" + "L0101\x1B" + "K\"" + request.FechaRevision + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V1400\x1B" + "L0101\x1B" + "K\"" + request.IdRevision + "\"\x0A");
            sbplCommand.Append("\x1B" + "H0100\x1B" + "V1500\x1B" + "L0101\x1B" + "K\"EPC: " + request.EPC + "\"\x0A");

            sbplCommand.Append("\x1B" + "Q1\x0A"); // Print quantity
            sbplCommand.Append("\x1B" + "Z\x0A"); // End command
            return sbplCommand.ToString();
        }

        //Endpoint to clear buffer and printer queue
        [HttpGet("ClearBufferAndQueue")]
        public IActionResult ClearBufferAndQueue()
        {
            string ipAddress = _configuration["PrinterSettings:IPAddress"];
            int port = int.Parse(_configuration["PrinterSettings:Port"]);

            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (NetworkStream stream = client.GetStream())
                {
                    string clearCommand = ClearBufferAndQueue2();
                    byte[] data = Encoding.ASCII.GetBytes(clearCommand);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    Console.WriteLine($"Sent to printer: {clearCommand.Replace("\x1B", "\\x1B").Replace("\x0A", "\\x0A")}");
                }
                return Ok("Buffer and queue cleared.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Failed to clear buffer and queue.");
            }
        }

        //Create method to clear buffer and printer queue
        private string ClearBufferAndQueue2()
        {
            return "\x1B" + "CS\x0A";
        }

        private string CreateSimplePrintCommand()
        {
            return "\x1B" + "A\x0A\x1B" + "H100\x1B" + "V100\x1B" + "L0101\x1B" + "K\"TEST PRINT\"\x0A\x1B" + "Q1\x0A\x1B" + "Z\x0A";
        }

        public class PrintRequest
        {
            public string IdEtiqueta { get; set; }
            public string Area { get; set; }
            public string Fecha { get; set; }
            public string Producto { get; set; }
            public string Turno { get; set; }
            public string Operador { get; set; }
            public string PesoBruto { get; set; }
            public string PesoNeto { get; set; }
            public string PesoTarima { get; set; }
            public string Cantidad { get; set; }
            public string CodigoTrazabilidad { get; set; }
            public string Lote { get; set; }
            public string Qr { get; set; }
            public string FechaRevision { get; set; }
            public string IdRevision { get; set; }
            public string EPC { get; set; }
        }

        // Endpoint to detect printer status
        [HttpGet("PrinterStatus")]
        public IActionResult PrinterStatus()
        {
            string ipAddress = _configuration["PrinterSettings:IPAddress"];
            int port = int.Parse(_configuration["PrinterSettings:Port"]);

            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (NetworkStream stream = client.GetStream())
                {
                    string statusCommand = GetPrinterStatus();
                    byte[] data = Encoding.ASCII.GetBytes(statusCommand);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    Console.WriteLine($"Sent to printer: {statusCommand.Replace("\x1B", "\\x1B").Replace("\x0A", "\\x0A")}");
                }
                return Ok("Printer is online.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Printer is offline.");
            }
        }

        private string GetPrinterStatus()
        {
            return "\x1B" + "CS\x0A";
        }

        // Endpoint to detect all usb printers using libusbnet
       

    }
}
