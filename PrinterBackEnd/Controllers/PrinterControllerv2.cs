using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrinterBackEnd.Services;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class PrinterController : ControllerBase
{
    private readonly PrinterService _printerService;
    private readonly ILogger<PrinterController> _logger;

    public PrinterController(PrinterService printerService, ILogger<PrinterController> logger)
    {
        _printerService = printerService;
        _logger = logger;
    }

    [HttpGet("status")]
    public IActionResult GetPrinterStatus()
    {
        try
        {
            var status = _printerService.GetPrinterStatus();
            if (status == null)
            {
                return NotFound("Printer status not available.");
            }
            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the printer status.");
            return StatusCode(500, $"An error occurred while retrieving the printer status: {ex.Message}");
        }
    }

    [HttpPost("send-command")]
    public async Task<IActionResult> SendCommandFromFile([FromQuery] string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return BadRequest("File path is required.");
        }

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound($"The file at path '{filePath}' was not found.");
        }

        try
        {
            string command = await System.IO.File.ReadAllTextAsync(filePath);
            bool result = _printerService.SendCommand(command);

            if (!result)
            {
                return StatusCode(500, "Failed to send the command to the printer.");
            }

            return Ok("Command sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the command from file.");
            return StatusCode(500, $"An error occurred while sending the command from file: {ex.Message}");
        }
    }
}
