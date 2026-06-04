@echo off
echo ========================================
echo   SISTEMA BANCARIO - INICIO RAPIDO
echo ========================================
echo.
echo 1. Levantando contenedores...
cd /d "%~dp0"
docker-compose up -d

echo.
echo 2. Esperando a que los servicios esten listos...
timeout /t 10 /nobreak >nul

echo.
echo 3. Verificando estado...
docker ps

echo.
echo ========================================
echo   PROYECTO LISTO!
echo ========================================
echo.
echo API Swagger:  http://localhost:8102/swagger
echo Frontend:     Ejecuta "cd BancoWeb\BancoWeb && dotnet watch"
echo Base de Datos: localhost:8101
echo.
echo Credenciales de prueba:
echo   Admin: admin1@banco.com / admin123
echo   Cliente: juan.perez@email.com / 1234567890
echo.
echo Presiona cualquier tecla para abrir Swagger...
pause >nul
start http://localhost:8102/swagger
