using BancoApi.DTOs.DTOs_Cliente;

namespace BancoApi.Models.models_cliente
{

public class model_ClienteCrear
    {
        private readonly DTO_ClienteCrear _dto;

private string? _nombreNormalizado;
        private string? _emailNormalizado;
        private string? _telefonoNormalizado;

        public model_ClienteCrear(DTO_ClienteCrear dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string prm_nombre_cliente => _nombreNormalizado ?? _dto.prm_nombre_cliente;
        public string prm_email_cliente => _emailNormalizado ?? _dto.prm_email_cliente;
        public string prm_telefono_cliente => _telefonoNormalizado ?? _dto.prm_telefono_cliente;
        public int prm_cedula_cliente => _dto.prm_cedula_cliente;

public void Validar()
        {
            if (string.IsNullOrWhiteSpace(prm_nombre_cliente))
            {
                throw new ArgumentException("El nombre del cliente es requerido");
            }

            if (string.IsNullOrWhiteSpace(prm_email_cliente))
            {
                throw new ArgumentException("El email del cliente es requerido");
            }

            if (!prm_email_cliente.Contains("@") || !prm_email_cliente.Contains("."))
            {
                throw new ArgumentException("El email debe tener un formato válido");
            }

            if (string.IsNullOrWhiteSpace(prm_telefono_cliente))
            {
                throw new ArgumentException("El teléfono del cliente es requerido");
            }

            if (prm_cedula_cliente <= 0)
            {
                throw new ArgumentException("La cédula debe ser un número válido");
            }
        }

public void Normalizar()
        {
            if (_dto.prm_nombre_cliente != null)
            {
                _nombreNormalizado = _dto.prm_nombre_cliente.Trim();
            }

            if (_dto.prm_email_cliente != null)
            {
                _emailNormalizado = _dto.prm_email_cliente.Trim().ToLower();
            }

            if (_dto.prm_telefono_cliente != null)
            {
                _telefonoNormalizado = _dto.prm_telefono_cliente.Trim();
            }
        }
    }
}
