SET SERVEROUTPUT ON
SET FEEDBACK ON

-- Verificar paquetes
SELECT object_name, object_type, status 
FROM user_objects 
WHERE object_type IN ('PACKAGE', 'PACKAGE BODY')
ORDER BY object_name;

-- Probar función de login
DECLARE
    v_result NUMBER;
BEGIN
    v_result := PKG_ADMINISTRADOR.function_login_admin('admin1@banco.com', 'admin123');
    DBMS_OUTPUT.PUT_LINE('Resultado del login: ' || NVL(TO_CHAR(v_result), 'NULL'));
END;
/

EXIT;
