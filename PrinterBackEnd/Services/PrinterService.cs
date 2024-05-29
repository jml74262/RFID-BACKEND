using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrinterBackEnd.Models;
using System.Text;
using static PrinterBackEnd.Models.Printer;

namespace PrinterBackEnd.Services
{
    public class PrinterService
    {
        private readonly Printer _printer;
        private readonly ILogger<PrinterService> _logger;

        public PrinterService(IOptions<PrinterSettings> settings, ILogger<PrinterService> logger)
        {
            var printerSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _printer = new Printer
            {
                Interface = Printer.InterfaceType.TCPIP,
                TCPIPAddress = printerSettings.IPAddress,
                TCPIPPort = printerSettings.Port,
                Timeout = printerSettings.Timeout * 2 // Increase timeout
            };
            _logger = logger;
        }

        public Status GetPrinterStatus()
        {
            try
            {
                _logger.LogInformation("Attempting to get printer status...");
                var status = _printer.GetPrinterStatus();
                _logger.LogInformation("Printer status retrieved successfully.");
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving printer status.");
                throw; // Rethrow to allow the controller to catch and log it
            }
        }

        public bool SendCommand(string command)
        {
            try
            {
                _logger.LogInformation("Attempting to send command...");
                byte[] data = Encoding.ASCII.GetBytes(command);
                _printer.Send(data);
                _logger.LogInformation("Command sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending command.");
                return false;
            }
        }
    }
}
