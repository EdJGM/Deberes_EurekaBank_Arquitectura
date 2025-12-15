-- =============================================
-- Actualizar tabla sucursal para Google Maps
-- =============================================

-- Agregar columnas para coordenadas geográficas y datos adicionales
ALTER TABLE sucursal 
ADD COLUMN dec_sucuclatitud DECIMAL(10,8) DEFAULT 0.00000000,
ADD COLUMN dec_sucuclongitud DECIMAL(11,8) DEFAULT 0.00000000,
ADD COLUMN vch_sucutelefono VARCHAR(20) DEFAULT '',
ADD COLUMN vch_sucuemail VARCHAR(100) DEFAULT '',
ADD COLUMN vch_sucuestado VARCHAR(15) DEFAULT 'ACTIVO';

-- Agregar constraint para el estado
ALTER TABLE sucursal 
ADD CONSTRAINT chk_sucursal_vch_sucuestado 
CHECK (vch_sucuestado IN ('ACTIVO', 'INACTIVO', 'MANTENIMIENTO'));

-- Actualizar datos existentes con coordenadas reales de Ecuador
-- (Estas son coordenadas aproximadas de ciudades principales de Ecuador)

UPDATE sucursal SET 
    dec_sucuclatitud = -2.1894, 
    dec_sucuclongitud = -79.8890, 
    vch_sucutelefono = '04-2123456',
    vch_sucuemail = 'sipan@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '001'; -- Sipan, Chiclayo -> Guayaquil

UPDATE sucursal SET 
    dec_sucuclatitud = -0.2298, 
    dec_sucuclongitud = -78.5249, 
    vch_sucutelefono = '02-2234567',
    vch_sucuemail = 'chachan@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '002'; -- Chan Chan, Trujillo -> Quito

UPDATE sucursal SET 
    dec_sucuclatitud = -0.1807, 
    dec_sucuclongitud = -78.4678, 
    vch_sucutelefono = '02-3345678',
    vch_sucuemail = 'olivosquito@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '003'; -- Los Olivos, Lima -> Norte de Quito

UPDATE sucursal SET 
    dec_sucuclatitud = -0.2635, 
    dec_sucuclongitud = -78.5224, 
    vch_sucutelefono = '02-4456789',
    vch_sucuemail = 'pardoquito@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '004'; -- Pardo, Lima -> Sur de Quito

UPDATE sucursal SET 
    dec_sucuclatitud = -2.9001, 
    dec_sucuclongitud = -79.0059, 
    vch_sucutelefono = '07-5567890',
    vch_sucuemail = 'misticuenca@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '005'; -- Misti, Arequipa -> Cuenca

UPDATE sucursal SET 
    dec_sucuclatitud = -1.2544, 
    dec_sucuclongitud = -78.6181, 
    vch_sucutelefono = '03-6678901',
    vch_sucuemail = 'machupicchu@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '006'; -- Machupicchu, Cusco -> Ambato

UPDATE sucursal SET 
    dec_sucuclatitud = -0.9537, 
    dec_sucuclongitud = -80.7286, 
    vch_sucutelefono = '05-7789012',
    vch_sucuemail = 'graumanta@banquito.com',
    vch_sucuestado = 'ACTIVO'
WHERE chr_sucucodigo = '007'; -- Grau, Piura -> Manta

-- Insertar algunas sucursales adicionales para pruebas
INSERT INTO sucursal VALUES(
    '008', 
    'Centro Histórico', 
    'Quito', 
    'García Moreno N4-49 y Sucre', 
    0,
    -0.2201, 
    -78.5123, 
    '02-8890123',
    'centrohistorico@banquito.com',
    'ACTIVO'
);

INSERT INTO sucursal VALUES(
    '009', 
    'La Mariscal', 
    'Quito', 
    'Av. Amazonas N24-03 y Colón', 
    0,
    -0.2033, 
    -78.4936, 
    '02-9901234',
    'mariscal@banquito.com',
    'ACTIVO'
);

INSERT INTO sucursal VALUES(
    '010', 
    'Samborondón', 
    'Guayaquil', 
    'Av. Juan Tanca Marengo Km 4.5', 
    0,
    -2.1372, 
    -79.8900, 
    '04-1012345',
    'samborondon@banquito.com',
    'ACTIVO'
);

-- Crear índices para mejorar rendimiento en consultas geográficas
CREATE INDEX idx_sucursal_coordenadas ON sucursal(dec_sucuclatitud, dec_sucuclongitud);
CREATE INDEX idx_sucursal_estado ON sucursal(vch_sucuestado);

-- Procedimiento almacenado para calcular distancia (opcional)
DELIMITER //
CREATE PROCEDURE CalcularDistanciaHaversine(
    IN lat1 DECIMAL(10,8),
    IN lon1 DECIMAL(11,8),
    IN lat2 DECIMAL(10,8),
    IN lon2 DECIMAL(11,8),
    OUT distancia DECIMAL(8,2)
)
BEGIN
    DECLARE dlat DECIMAL(12,10);
    DECLARE dlon DECIMAL(12,10);
    DECLARE a DECIMAL(12,10);
    DECLARE c DECIMAL(12,10);
    DECLARE R INT DEFAULT 6371; -- Radio de la Tierra en km
    
    SET dlat = RADIANS(lat2 - lat1);
    SET dlon = RADIANS(lon2 - lon1);
    
    SET a = SIN(dlat/2) * SIN(dlat/2) + COS(RADIANS(lat1)) * COS(RADIANS(lat2)) * SIN(dlon/2) * SIN(dlon/2);
    SET c = 2 * ATAN2(SQRT(a), SQRT(1-a));
    SET distancia = R * c;
END //
DELIMITER ;