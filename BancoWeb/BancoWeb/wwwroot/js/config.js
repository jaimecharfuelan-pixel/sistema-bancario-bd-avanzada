const API_CONFIG = {
    BASE_URL: "http://localhost:8102/api",
    
    AUTH: {
        LOGIN: "/auth/login"
    },
    
    CLIENTE: {
        SOLICITAR_CUENTA: "/controller_Cliente/service_solicitarCuenta",
        ACTUALIZAR_DATOS: "/controller_Cliente/service_actualizarDatosCliente",
        ACTUALIZAR_CORREO: "/controller_Cliente/service_actualizarCorreo",
        ACTUALIZAR_NOMBRE: "/controller_Cliente/service_actualizarNombre",
        ACTUALIZAR_TELEFONO: "/controller_Cliente/service_actualizarTelefono",
        ACTUALIZAR_CEDULA: "/controller_Cliente/service_actualizarCedula"
    },
    
    CUENTA: {
        CONSULTAR_POR_CLIENTE: "/controller_Cuenta/service_consultarPorCliente",
        CONSULTAR_POR_ID: "/controller_Cuenta/service_consultarPorId",
        ACTUALIZAR: "/controller_Cuenta/service_actualizarCuenta",
        CAMBIAR_SALDO: "/controller_Cuenta/service_cambiarSaldo",
        CAMBIAR_CONTRASENA: "/controller_Cuenta/service_cambiarContrasena",
        ELIMINAR: "/controller_Cuenta/service_eliminarCuenta"
    },
    
    SUCURSAL: {
        LISTAR: "/controller_Sucursal/service_listar",
        CREAR: "/controller_Sucursal/service_crear",
        EDITAR_ESTADO: "/controller_Sucursal/service_editarEstado",
        ELIMINAR: "/controller_Sucursal/service_eliminar"
    },
    
    CAJERO: {
        CREAR: "/controller_Cajero/service_crear",
        CAMBIAR_ESTADO: "/controller_Cajero/service_cambiarEstado",
        RECARGAR: "/controller_Cajero/service_recargar",
        DESCONTAR: "/controller_Cajero/service_descontar",
        LISTAR: "/controller_Cajero/service_listar",
        LISTAR_ACTIVOS: "/controller_Cajero/service_listarActivos",
        OBTENER_POR_IDS: "/controller_Cajero/service_obtenerPorIds"
    },
    
    ADMIN: {
        SOLICITUDES: "/controller_Admin/service_solicitudes",
        CREAR_CUENTA: "/controller_Admin/service_crearCuenta"
    },
    
    CLIENTE_ADMIN: {
        LISTAR_ACTIVOS: "/controller_Cliente/service_listarActivos",
        OBTENER_POR_IDS: "/controller_Cliente/service_obtenerPorIds",
        ELIMINAR: "/controller_Cliente/service_eliminarCliente",
        ACTUALIZAR_DATOS: "/controller_Cliente/service_actualizarDatosCliente"
    },
    
    CUENTA_ADMIN: {
        LISTAR_ACTIVAS: "/controller_Cuenta/service_listarActivas",
        OBTENER_POR_IDS: "/controller_Cuenta/service_obtenerPorIds",
        CONSULTAR_POR_ID: "/controller_Cuenta/service_consultarPorId",
        ACTUALIZAR: "/controller_Cuenta/service_actualizarCuenta",
        ELIMINAR: "/controller_Cuenta/service_eliminarCuenta"
    },
    
    PRESTAMO: {
        LISTAR_SOLICITUDES: "/controller_Prestamo/service_listarSolicitudes",
        ACEPTAR: "/controller_Prestamo/service_aceptarPrestamo",
        RECHAZAR: "/controller_Prestamo/service_rechazarPrestamo",
        LISTAR_ACTIVOS: "/controller_Prestamo/service_listarActivos",
        OBTENER_POR_IDS: "/controller_Prestamo/service_obtenerPorIds"
    },
    
    TARJETA: {
        LISTAR: "/controller_Tarjeta/service_listarTarjetas",
        CREAR_DEBITO: "/controller_Tarjeta/service_crearTarjetaDebito",
        CREAR_CREDITO: "/controller_Tarjeta/service_crearTarjetaCredito",
        CAMBIAR_ESTADO: "/controller_Tarjeta/service_cambiarEstadoTarjeta"
    },
    
    TRANSACCION: {
        TRANSFERENCIA: "/controller_Transaccion/service_transferencia",
        RETIRO_DEBITO: "/controller_Transaccion/service_retiroDebito",
        RETIRO_CREDITO: "/controller_Transaccion/service_retiroCredito",
        HISTORIAL_CUENTA: "/controller_Transaccion/service_historialCuenta",
        HISTORIAL_CAJERO: "/controller_Transaccion/service_historialCajero"
    },
    
    PRESTAMO_CLIENTE: {
        SOLICITAR: "/controller_Prestamo/service_solicitarPrestamo"
    }
};

async function apiCall(endpoint, options = {}) {
    const defaultOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        timeout: 10000
    };
    
    const finalOptions = { ...defaultOptions, ...options };
    
    if (finalOptions.body && typeof finalOptions.body === 'object') {
        finalOptions.body = JSON.stringify(finalOptions.body);
    }
    
    try {
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), finalOptions.timeout);
        
        const response = await fetch(API_CONFIG.BASE_URL + endpoint, {
            ...finalOptions,
            signal: controller.signal
        });
        
        clearTimeout(timeoutId);
        
        if (!response.ok) {
            let errorMessage = "Error en la petición";
            
            try {
                const errorData = await response.json();
                errorMessage = errorData.error || errorMessage;
            } catch {
                errorMessage = `Error ${response.status}: ${response.statusText}`;
            }
            
            throw new Error(errorMessage);
        }
        
        try {
            return await response.json();
        } catch {
            return { message: "Operación exitosa" };
        }
        
    } catch (error) {
        if (error.name === 'AbortError') {
            throw new Error("La petición tardó demasiado. Verifica tu conexión.");
        }
        
        if (error.message.includes('Failed to fetch') || error.message.includes('NetworkError')) {
            throw new Error("No se pudo conectar con el servidor. Verifica que la API esté ejecutándose.");
        }
        
        throw error;
    }
}

