using BancoApi.DTOs.DTOs_Cliente;
using System;
using System.Linq;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteProcesarMasivos
    {
        private readonly DTO_ClienteProcesarMasivos _dto;
        private List<DTO_ClienteProcesarMasivos.ClienteInfo>? _clientesNormalizados;

        public model_ClienteProcesarMasivos(DTO_ClienteProcesarMasivos dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public List<DTO_ClienteProcesarMasivos.ClienteInfo> clientes => _clientesNormalizados ?? (_dto.clientes ?? new List<DTO_ClienteProcesarMasivos.ClienteInfo>());

public void Validar()
        {
            var clientesParaValidar = _clientesNormalizados ?? _dto.clientes;
            if (clientesParaValidar == null || clientesParaValidar.Count == 0)
            {
                throw new ArgumentException("Debe proporcionar al menos un cliente");
            }

            foreach (var cliente in clientesParaValidar)
            {
                if (string.IsNullOrWhiteSpace(cliente.idCliente))
                {
                    throw new ArgumentException("Todos los clientes deben tener un ID válido");
                }
                if (string.IsNullOrWhiteSpace(cliente.nombreCliente))
                {
                    throw new ArgumentException("Todos los clientes deben tener un nombre válido");
                }
                if (string.IsNullOrWhiteSpace(cliente.emailCliente))
                {
                    throw new ArgumentException("Todos los clientes deben tener un email válido");
                }
            }
        }

public void Normalizar()
        {
            if (_dto.clientes != null)
            {
                _clientesNormalizados = new List<DTO_ClienteProcesarMasivos.ClienteInfo>();
                foreach (var cliente in _dto.clientes)
                {
                    var clienteNormalizado = new DTO_ClienteProcesarMasivos.ClienteInfo
                    {
                        idCliente = cliente.idCliente?.Trim().ToUpper() ?? "",
                        nombreCliente = cliente.nombreCliente?.Trim() ?? "",
                        emailCliente = cliente.emailCliente?.Trim().ToLower() ?? "",
                        telefonoCliente = cliente.telefonoCliente?.Trim() ?? ""
                    };
                    _clientesNormalizados.Add(clienteNormalizado);
                }
            }
            else
            {
                _clientesNormalizados = new List<DTO_ClienteProcesarMasivos.ClienteInfo>();
            }
        }
    }
}

