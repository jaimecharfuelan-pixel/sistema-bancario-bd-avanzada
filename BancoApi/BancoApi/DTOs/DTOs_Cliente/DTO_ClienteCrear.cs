using System.ComponentModel.DataAnnotations;

namespace BancoApi.DTOs.DTOs_Cliente
{

public class DTO_ClienteCrear
    {
        [Required(ErrorMessage = "El nombre del cliente es requerido")]
        public string prm_nombre_cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email del cliente es requerido")]
        [EmailAddress(ErrorMessage = "El email debe tener un formato válido")]
        public string prm_email_cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono del cliente es requerido")]
        public string prm_telefono_cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula del cliente es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cédula debe ser un número válido mayor a cero")]
        public int prm_cedula_cliente { get; set; }
    }
}

