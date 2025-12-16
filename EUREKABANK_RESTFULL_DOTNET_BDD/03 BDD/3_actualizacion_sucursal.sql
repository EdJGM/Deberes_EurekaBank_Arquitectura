-- =============================================
-- Actualizar tabla sucursal para Google Maps (.NET/SQL Server)
-- =============================================

-- Verificar si las columnas ya existen antes de agregarlas
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'sucursal' AND COLUMN_NAME = 'dec_sucuclatitud')
BEGIN
    ALTER TABLE sucursal ADD dec_sucuclatitud DECIMAL(10,8) DEFAULT 0.00000000;
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'sucursal' AND COLUMN_NAME = 'dec_sucuclongitud')
BEGIN
    ALTER TABLE sucursal ADD dec_sucuclongitud DECIMAL(11,8) DEFAULT 0.00000000;
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'sucursal' AND COLUMN_NAME = 'vch_sucutelefono')
BEGIN
    ALTER TABLE sucursal ADD vch_sucutelefono VARCHAR(20) DEFAULT '';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'sucursal' AND COLUMN_NAME = 'vch_sucuemail')
BEGIN
    ALTER TABLE sucursal ADD vch_sucuemail VARCHAR(100) DEFAULT '';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'sucursal' AND COLUMN_NAME = 'vch_sucuestado')
BEGIN
    ALTER TABLE sucursal ADD vch_sucuestado VARCHAR(15) DEFAULT 'ACTIVO';
END

-- Agregar constraint para el estado (solo si no existe)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS WHERE CONSTRAINT_NAME = 'chk_sucursal_vch_sucuestado')
BEGIN
    ALTER TABLE sucursal 
    ADD CONSTRAINT chk_sucursal_vch_sucuestado 
    CHECK (vch_sucuestado IN ('ACTIVO', 'INACTIVO', 'MANTENIMIENTO'));
END

-- Actualizar datos existentes con coordenadas reales de Ecuador
-- (Estas son coordenadas aproximadas de ciudades principales de Ecuador)

UPDATE sucursal SET 
    dec_sucuclatitud = -2.1894, 
    dec_sucuclongitud = -79.8890, 
    vch_sucutelefono = '04-2123456',
    vch_sucuemail = 'sipan@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '001' AND dec_sucuclatitud IS NULL; -- Sipan, Chiclayo -> Guayaquil

UPDATE sucursal SET 
    dec_sucuclatitud = -0.2298, 
    dec_sucuclongitud = -78.5249, 
    vch_sucutelefono = '02-2234567',
    vch_sucuemail = 'chachan@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '002' AND dec_sucuclatitud IS NULL; -- Chan Chan, Trujillo -> Quito

UPDATE sucursal SET 
    dec_sucuclatitud = -0.1807, 
    dec_sucuclongitud = -78.4678, 
    vch_sucutelefono = '02-3345678',
    vch_sucuemail = 'olivosquito@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '003' AND dec_sucuclatitud IS NULL; -- Los Olivos, Lima -> Norte de Quito

UPDATE sucursal SET 
    dec_sucuclatitud = -0.2635, 
    dec_sucuclongitud = -78.5224, 
    vch_sucutelefono = '02-4456789',
    vch_sucuemail = 'pardoquito@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '004' AND dec_sucuclatitud IS NULL; -- Pardo, Lima -> Sur de Quito

UPDATE sucursal SET 
    dec_sucuclatitud = -2.9001, 
    dec_sucuclongitud = -79.0059, 
    vch_sucutelefono = '07-5567890',
    vch_sucuemail = 'misticuenca@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '005' AND dec_sucuclatitud IS NULL; -- Misti, Arequipa -> Cuenca

UPDATE sucursal SET 
    dec_sucuclatitud = -1.2544, 
    dec_sucuclongitud = -78.6181, 
    vch_sucutelefono = '03-6678901',
    vch_sucuemail = 'machupicchu@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '006' AND dec_sucuclatitud IS NULL; -- Machupicchu, Cusco -> Ambato

UPDATE sucursal SET 
    dec_sucuclatitud = -0.9537, 
    dec_sucuclongitud = -80.7286, 
    vch_sucutelefono = '05-7789012',
    vch_sucuemail = 'graumanta@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '007' AND dec_sucuclatitud IS NULL; -- Grau, Piura -> Manta

-- Insertar algunas sucursales adicionales para pruebas (solo si no existen)
IF NOT EXISTS (SELECT 1 FROM sucursal WHERE chr_sucucodigo = '008')
BEGIN
    INSERT INTO sucursal (
        chr_sucucodigo, vch_sucunombre, vch_sucuciudad, vch_sucudireccion, 
        int_sucucontcuenta, dec_sucuclatitud, dec_sucuclongitud, 
        vch_sucutelefono, vch_sucuemail, vch_sucuestado
    ) VALUES (
        '008', 'Centro Histórico', 'Quito', 'García Moreno N4-49 y Sucre', 
        0, -0.2201, -78.5123, 
        '02-8890123', 'centrohistorico@banquito.com', 'ACTIVO'
    );
END

IF NOT EXISTS (SELECT 1 FROM sucursal WHERE chr_sucucodigo = '009')
BEGIN
    INSERT INTO sucursal (
        chr_sucucodigo, vch_sucunombre, vch_sucuciudad, vch_sucudireccion, 
        int_sucucontcuenta, dec_sucuclatitud, dec_sucuclongitud, 
        vch_sucutelefono, vch_sucuemail, vch_sucuestado
    ) VALUES (
        '009', 'La Mariscal', 'Quito', 'Av. Amazonas N24-03 y Colón', 
        0, -0.2033, -78.4936, 
        '02-9901234', 'mariscal@banquito.com', 'ACTIVO'
    );
END

IF NOT EXISTS (SELECT 1 FROM sucursal WHERE chr_sucucodigo = '010')
BEGIN
    INSERT INTO sucursal (
        chr_sucucodigo, vch_sucunombre, vch_sucuciudad, vch_sucudireccion, 
        int_sucucontcuenta, dec_sucuclatitud, dec_sucuclongitud, 
        vch_sucutelefono, vch_sucuemail, vch_sucuestado
    ) VALUES (
        '010', 'Samborondón', 'Guayaquil', 'Av. Juan Tanca Marengo Km 4.5', 
        0, -2.1372, -79.8900, 
        '04-1012345', 'samborondon@banquito.com', 'ACTIVO'
    );
END

-- Crear índices para mejorar rendimiento en consultas geográficas (solo si no existen)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_sucursal_coordenadas')
BEGIN
    CREATE INDEX idx_sucursal_coordenadas ON sucursal(dec_sucuclatitud, dec_sucuclongitud);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_sucursal_estado')
BEGIN
    CREATE INDEX idx_sucursal_estado ON sucursal(vch_sucuestado);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_sucursal_ciudad')
BEGIN
    CREATE INDEX idx_sucursal_ciudad ON sucursal(vch_sucuciudad);
END

-- Función para calcular distancia Haversine (SQL Server 2016+)
-- Esta función se puede usar en consultas SQL para ordenar por distancia
IF OBJECT_ID('dbo.fn_CalcularDistanciaHaversine') IS NOT NULL
    DROP FUNCTION dbo.fn_CalcularDistanciaHaversine;
GO

CREATE FUNCTION dbo.fn_CalcularDistanciaHaversine(
    @lat1 FLOAT,
    @lon1 FLOAT,
    @lat2 FLOAT,
    @lon2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @R FLOAT = 6371; -- Radio de la Tierra en km
    DECLARE @dLat FLOAT = RADIANS(@lat2 - @lat1);
    DECLARE @dLon FLOAT = RADIANS(@lon2 - @lon1);
    DECLARE @a FLOAT = SIN(@dLat/2) * SIN(@dLat/2) + COS(RADIANS(@lat1)) * COS(RADIANS(@lat2)) * SIN(@dLon/2) * SIN(@dLon/2);
    DECLARE @c FLOAT = 2 * ATN2(SQRT(@a), SQRT(1-@a));
    DECLARE @distancia FLOAT = @R * @c;
    
    RETURN @distancia;
END
GO

-- Ejemplo de consulta usando la función de distancia:
-- SELECT *, dbo.fn_CalcularDistanciaHaversine(-0.2298, -78.5249, dec_sucuclatitud, dec_sucuclongitud) as distancia_km
-- FROM sucursal 
-- WHERE vch_sucuestado = 'ACTIVO'
-- ORDER BY distancia_km;

PRINT 'Actualización de tabla sucursal completada exitosamente.';
PRINT 'Se han agregado columnas para coordenadas, teléfono, email y estado.';
PRINT 'Se han actualizado los datos existentes con coordenadas de Ecuador.';
PRINT 'Se han creado índices para mejorar el rendimiento de consultas geográficas.';