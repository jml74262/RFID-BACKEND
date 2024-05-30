namespace PrinterBackEnd.Models.Domain
{
    public class CatArea
    {
        public int Id { get; set; }
        public string Area { get; set; }
    }

    public class CatFolioConsec
    {
        public int Id { get; set; }
        public int? Area { get; set; }
        public int? Maquina { get; set; }
        public int? OT { get; set; }
        public int? Consec { get; set; }
    }

    public class CatMaquina
    {
        public int Id { get; set; }
        public int? Area { get; set; }
        public string No { get; set; }
        public string Maquina { get; set; }
        public string Nombre { get; set; }
        public int? Status { get; set; }
    }

    public class CatOperador
    {
        public int Id { get; set; }
        public string NumNomina { get; set; }
        public string Clave { get; set; }
        public string NombreCompleto { get; set; }
        public int? TipoUsuario { get; set; }
        public int? Id_Area { get; set; }
        public int? Id_Turno { get; set; }
        public string CodeQR { get; set; }
        public int? Status { get; set; }
    }

    public class CatOrden
    {
        public int Id { get; set; }
        public string Orden { get; set; }
        public string ClaveProducto { get; set; }
        public string Producto { get; set; }
        public string UltimoProceso { get; set; }
    }

    public class CatProducto
    {
        public int Id { get; set; }
        public string ClaveProducto { get; set; }
        public string NombreProducto { get; set; }
        public string PrintCard { get; set; }
    }

    public class CatTurno
    {
        public int Id { get; set; }
        public string Turno { get; set; }
    }

    public class ProdEtiquetasRFID
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string ClaveProducto { get; set; }
        public string NombreProducto { get; set; }
        public string ClaveOperador { get; set; }
        public string Operador { get; set; }
        public string Turno { get; set; }
        public float PesoTarima { get; set; }
        public float PesoBruto { get; set; }
        public float PesoNeto { get; set; }
        public float Piezas { get; set; }
        public string Trazabilidad { get; set; }
        public string Orden { get; set; }
        public string RFID { get; set; }
        public int Status { get; set; }
    }

}
