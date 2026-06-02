namespace BancoApi.DTOs.DTOs_Sucursal
{
    public class DTO_SucursalCrear
    {
        public string nombreSucursal { get; set; } = string.Empty;
        public string direccionSucursal { get; set; } = string.Empty;
        public string telefonoSucursal { get; set; } = string.Empty;
        public int? idAdministrador { get; set; }
    }
}

