-- =============================================
-- Carga de Datos de Prueba (SQL Server)
-- =============================================

-- Tabla: moneda
INSERT INTO moneda (chr_monecodigo, vch_monedescripcion) VALUES ('01', 'Soles');
INSERT INTO moneda (chr_monecodigo, vch_monedescripcion) VALUES ('02', 'Dolares');

-- Tabla: cargomantenimiento
INSERT INTO cargomantenimiento (chr_monecodigo, dec_cargMontoMaximo, dec_cargImporte) VALUES ('01', 3500.00, 7.00);
INSERT INTO cargomantenimiento (chr_monecodigo, dec_cargMontoMaximo, dec_cargImporte) VALUES ('02', 1200.00, 2.50);

-- Tabla: costomovimiento
INSERT INTO costomovimiento (chr_monecodigo, dec_costimporte) VALUES ('01', 2.00);
INSERT INTO costomovimiento (chr_monecodigo, dec_costimporte) VALUES ('02', 0.60);

-- Tabla: interesmensual
INSERT INTO interesmensual (chr_monecodigo, dec_inteimporte) VALUES ('01', 0.70);
INSERT INTO interesmensual (chr_monecodigo, dec_inteimporte) VALUES ('02', 0.60);


-- Tabla: tipomovimiento
INSERT INTO tipomovimiento (chr_tipocodigo, vch_tipodescripcion, vch_tipoaccion, vch_tipoestado)
VALUES ('001', 'Apertura de cuenta', 'INGRESO', 'ACTIVO'),
       ('002', 'Cancelar cuenta', 'SALIDA', 'ACTIVO'),
       ('003', 'Deposito', 'INGRESO', 'ACTIVO'),
       ('004', 'Retiro', 'SALIDA', 'ACTIVO'),
       ('005', 'Interes', 'INGRESO', 'ACTIVO'),
       ('006', 'Mantenimiento', 'SALIDA', 'ACTIVO'),
       ('007', 'ITF', 'SALIDA', 'ACTIVO'),
       ('008', 'Transferencia', 'INGRESO', 'ACTIVO'),
       ('009', 'Transferencia', 'SALIDA', 'ACTIVO'),
       ('010', 'Cargo por movimiento', 'SALIDA', 'ACTIVO');

-- Tabla: sucursal
INSERT INTO sucursal (chr_sucucodigo, vch_sucunombre, vch_sucuciudad, vch_sucudireccion, int_sucucontcuenta)
VALUES ('001', 'Sipan', 'Chiclayo', 'Av. Balta 1456', 2),
       ('002', 'Chan Chan', 'Trujillo', 'Jr. Independencia 456', 3),
       ('003', 'Los Olivos', 'Lima', 'Av. Central 1234', 0),
       ('004', 'Pardo', 'Lima', 'Av. Pardo 345 - Miraflores', 0),
       ('005', 'Misti', 'Arequipa', 'Bolivar 546', 0),
       ('006', 'Machupicchu', 'Cusco', 'Calle El Sol 534', 0),
       ('007', 'Grau', 'Piura', 'Av. Grau 1528', 0);


-- Tabla: empleado
INSERT INTO empleado (
    chr_emplcodigo,
    vch_emplpaterno,
    vch_emplmaterno,
    vch_emplnombre,
    vch_emplciudad,
    vch_empldireccion
)
VALUES 
('9999', 'Internet', 'Internet', 'internet', 'Internet', 'internet'),
('0001', 'Romero', 'Castillo', 'Carlos Alberto', 'Trujillo', 'Call1 1 Nro. 456'),
('0002', 'Castro', 'Vargas', 'Lidia', 'Lima', 'Federico Villarreal 456 - SMP'),
('0003', 'Reyes', 'Ortiz', 'Claudia', 'Lima', 'Av. Aviación 3456 - San Borja'),
('0004', 'Ramos', 'Garibay', 'Angelica', 'Chiclayo', 'Calle Barcelona 345'),
('0005', 'Ruiz', 'Zabaleta', 'Claudia', 'Cusco', 'Calle Cruz Verde 364'),
('0006', 'Cruz', 'Tarazona', 'Ricardo', 'Areguipa', 'Calle La Gruta 304'),
('0007', 'Diaz', 'Flores', 'Edith', 'Lima', 'Av. Pardo 546'),
('0008', 'Sarmiento', 'Bellido', 'Claudia Rocio', 'Areguipa', 'Calle Alfonso Ugarte 1567'),
('0009', 'Pachas', 'Sifuentes', 'Luis Alberto', 'Trujillo', 'Francisco Pizarro 1263'),
('0010', 'Tello', 'Alarcon', 'Hugo Valentin', 'Cusco', 'Los Angeles 865'),
('0011', 'Carrasco', 'Vargas', 'Pedro Hugo', 'Chiclayo', 'Av. Balta 1265'),
('0012', 'Mendoza', 'Jara', 'Monica Valeria', 'Lima', 'Calle Las Toronjas 450'),
('0013', 'Espinoza', 'Melgar', 'Victor Eduardo', 'Huancayo', 'Av. San Martin 6734 Dpto. 508'),
('0014', 'Hidalgo', 'Sandoval', 'Milagros Leonor', 'Chiclayo', 'Av. Luis Gonzales 1230');


-- Tabla: usuario
INSERT INTO usuario (
    chr_emplcodigo,
    vch_emplusuario,
    vch_emplclave,
    vch_emplestado
)
VALUES 
('9999',  'internet',    'internet',     'ACTIVO'),
('0001',  'cromero',     'chicho',       'ACTIVO'),
('0002',  'lcastro',     'flaca',        'ACTIVO'),
('0003',  'creyes',      'linda',        'ANULADO'),
('0004',  'aramos',      'china',        'ACTIVO'),
('0005',  'cvalencia',   'angel',        'ACTIVO'),
('0006',  'rcruz',       'cerebro',      'ACTIVO'),
('0007',  'ediaz',       'princesa',     'ANULADO'),
('0008',  'csarmiento',  'chinita',      'ANULADO'),
('0009',  'lpachas',     'gato',         'ACTIVO'),
('0010',  'htello',      'machupichu',   'ACTIVO'),
('0011',  'pcarrasco',   'tinajones',    'ACTIVO'),
('0012',  'MONSTER',     'MONSTER9',     'ACTIVO');

-- Tabla: modulo
INSERT INTO modulo (
    int_moducodigo,
    vch_modunombre,
    vch_moduestado
)
VALUES
(1, 'Procesos', 'ACTIVO'),
(2, 'Tablas', 'ACTIVO'),
(3, 'Consultas', 'ACTIVO'),
(4, 'Reportes', 'ACTIVO'),
(5, 'Util', 'ACTIVO'),
(6, 'Seguridad', 'ACTIVO');


-- Tabla: permiso
INSERT INTO permiso (
    chr_emplcodigo,
    int_moducodigo,
    vch_permestado
)
VALUES
-- usuario: 0001
('0001', 1, 'ACTIVO'),
('0001', 2, 'ACTIVO'),
('0001', 3, 'ACTIVO'),
('0001', 4, 'ACTIVO'),
('0001', 5, 'ACTIVO'),
('0001', 6, 'ACTIVO'),

-- usuario: 0002
('0002', 1, 'ACTIVO'),
('0002', 2, 'ACTIVO'),
('0002', 3, 'ACTIVO'),
('0002', 4, 'ACTIVO'),
('0002', 5, 'CANCELADO'),
('0002', 6, 'CANCELADO'),

-- usuario: 0003
('0003', 1, 'ACTIVO'),
('0003', 2, 'CANCELADO'),
('0003', 3, 'ACTIVO'),
('0003', 4, 'ACTIVO'),
('0003', 5, 'ACTIVO'),
('0003', 6, 'CANCELADO'),

-- usuario: 0004
('0004', 1, 'CANCELADO'),
('0004', 2, 'ACTIVO'),
('0004', 3, 'ACTIVO'),
('0004', 4, 'CANCELADO'),
('0004', 5, 'ACTIVO'),
('0004', 6, 'CANCELADO'),

-- usuario: 0005
('0005', 1, 'ACTIVO'),
('0005', 2, 'CANCELADO'),
('0005', 3, 'ACTIVO'),
('0005', 4, 'ACTIVO'),
('0005', 5, 'ACTIVO'),
('0005', 6, 'CANCELADO'),

-- usuario: 0006
('0006', 1, 'ACTIVO'),
('0006', 2, 'ACTIVO'),
('0006', 3, 'ACTIVO'),
('0006', 4, 'ACTIVO'),
('0006', 5, 'ACTIVO'),
('0006', 6, 'ACTIVO'),

-- usuario: 0007
('0007', 1, 'CANCELADO'),
('0007', 2, 'ACTIVO'),
('0007', 3, 'ACTIVO'),
('0007', 4, 'CANCELADO'),
('0007', 5, 'ACTIVO'),
('0007', 6, 'CANCELADO');



-- Tabla: asignado
INSERT INTO asignado (
    chr_asigcodigo,
    chr_sucucodigo,
    chr_emplcodigo,
    dtt_asigfechaalta,
    dtt_asigfechabaja
)
VALUES
('000001', '001', '0004', CAST('2007-11-15' AS DATE), NULL),
('000002', '002', '0001', CAST('2007-11-20' AS DATE), NULL),
('000003', '003', '0002', CAST('2007-11-28' AS DATE), NULL),
('000004', '004', '0003', CAST('2007-12-12' AS DATE), CAST('2008-03-25' AS DATE)),
('000005', '005', '0006', CAST('2007-12-20' AS DATE), NULL),
('000006', '006', '0005', CAST('2008-01-05' AS DATE), NULL),
('000007', '004', '0007', CAST('2008-01-07' AS DATE), NULL),
('000008', '005', '0008', CAST('2008-01-07' AS DATE), NULL),
('000009', '001', '0011', CAST('2008-01-08' AS DATE), NULL),
('000010', '002', '0009', CAST('2008-01-08' AS DATE), NULL),
('000011', '006', '0010', CAST('2008-01-08' AS DATE), NULL);

-- Tabla: parametro
INSERT INTO parametro (
    chr_paracodigo,
    vch_paradescripcion,
    vch_paravalor,
    vch_paraestado
)
VALUES
('001', 'ITF - Impuesto a la Transacciones Financieras', '0.08', 'ACTIVO'),
('002', 'Número de Operaciones Sin Costo', '15', 'ACTIVO');

-- Tabla: cliente
INSERT INTO cliente (
    chr_cliecodigo,
    vch_cliepaterno,
    vch_cliematerno,
    vch_clienombre,
    chr_cliedni,
    vch_clieciudad,
    vch_cliedireccion,
    vch_clietelefono,
    vch_clieemail
)
VALUES
('00001', 'CORONEL', 'CASTILLO', 'ERIC GUSTAVO', '06914897', 'LIMA', 'LOS OLIVOS', '996-664-457', 'gcoronelc@gmail.com'),
('00002', 'VALENCIA', 'MORALES', 'PEDRO HUGO', '01576173', 'LIMA', 'MAGDALENA', '924-7834', 'pvalencia@terra.com.pe'),
('00003', 'MARCELO', 'VILLALOBOS', 'RICARDO', '10762367', 'LIMA', 'LINCE', '993-62966', 'ricardomarcelo@hotmail.com'),
('00004', 'ROMERO', 'CASTILLO', 'CARLOS ALBERTO', '06531983', 'LIMA', 'LOS OLIVOS', '865-84762', 'c.romero@hotmail.com'),
('00005', 'ARANDA', 'LUNA', 'ALAN ALBERTO', '10875611', 'LIMA', 'SAN ISIDRO', '834-67125', 'a.aranda@hotmail.com'),
('00006', 'AYALA', 'PAZ', 'JORGE LUIS', '10679245', 'LIMA', 'SAN BORJA', '963-34769', 'j.ayala@yahoo.com'),
('00007', 'CHAVEZ', 'CANALES', 'EDGAR RAFAEL', '10145693', 'LIMA', 'MIRAFLORES', '999-96673', 'e.chavez@gmail.com'),
('00008', 'FLORES', 'CHAFLOQUE', 'ROSA LIZET', '10773456', 'LIMA', 'LA MOLINA', '966-87567', 'r.florez@hotmail.com'),
('00009', 'FLORES', 'CASTILLO', 'CRISTIAN RAFAEL', '10346723', 'LIMA', 'LOS OLIVOS', '978-43768', 'c.flores@hotmail.com'),
('00010', 'GONZALES', 'GARCIA', 'GABRIEL ALEJANDRO', '10192376', 'LIMA', 'SAN MIGUEL', '945-56782', 'g.gonzales@yahoo.es'),
('00011', 'LAY', 'VALLEJOS', 'JUAN CARLOS', '10942287', 'LIMA', 'LINCE', '956-12657', 'j.lay@peru.com'),
('00012', 'MONTALVO', 'SOTO', 'DEYSI LIDIA', '10612376', 'LIMA', 'SURCO', '965-67235', 'd.montalvo@hotmail.com'),
('00013', 'RICALDE', 'RAMIREZ', 'ROSARIO ESMERALDA', '10761324', 'LIMA', 'MIRAFLORES', '991-23546', 'r.ricalde@gmail.com'),
('00014', 'RODRIGUEZ', 'FLORES', 'ENRIQUE MANUEL', '10773345', 'LIMA', 'LINCE', '976-82838', 'e.rodriguez@gmail.com'),
('00015', 'ROJAS', 'OSCANOA', 'FELIX NINO', '10238943', 'LIMA', 'LIMA', '962-32158', 'f.rojas@yahoo.com'),
('00016', 'TEJADA', 'DEL AGUILA', 'TANIA LORENA', '10446791', 'LIMA', 'PUEBLO LIBRE', '966-23854', 't.tejada@hotmail.com'),
('00017', 'VALDEVIESO', 'LEYVA', 'LIDIA ROXANA', '10452682', 'LIMA', 'SURCO', '956-78951', 'r.valdivieso@terra.com.pe'),
('00018', 'VALENTIN', 'COTRINA', 'JUAN DIEGO', '10398247', 'LIMA', 'LA MOLINA', '921-12456', 'j.valentin@terra.com.pe'),
('00019', 'YAURICASA', 'BAUTISTA', 'YESABETH', '10934584', 'LIMA', 'MAGDALENA', '977-75777', 'y.yauricasa@terra.com.pe'),
('00020', 'ZEGARRA', 'GARCIA', 'FERNANDO MOISES', '10772365', 'LIMA', 'SAN ISIDRO', '936-45876', 'f.zegarra@hotmail.com');

-- Tabla: cuenta
INSERT INTO cuenta (
    chr_cuencodigo,
    chr_monecodigo,
    chr_sucucodigo,
    chr_emplcreacuenta,
    chr_cliecodigo,
    dec_cuensaldo,
    dtt_cuenfechacreacion,
    vch_cuenestado,
    int_cuencontmov,
    chr_cuenclave
)
VALUES
('00200001', '01', '002', '0001', '00008', 7000, CAST('2008-01-05' AS DATE), 'ACTIVO', 15, '123456'),
('00200002', '01', '002', '0001', '00001', 6800, CAST('2008-01-09' AS DATE), 'ACTIVO', 3, '123456'),
('00200003', '02', '002', '0001', '00007', 6000, CAST('2008-01-11' AS DATE), 'ACTIVO', 6, '123456'),
('00100001', '01', '001', '0004', '00005', 6900, CAST('2008-01-06' AS DATE), 'ACTIVO', 7, '123456'),
('00100002', '02', '001', '0004', '00005', 4500, CAST('2008-01-08' AS DATE), 'ACTIVO', 4, '123456'),
('00300001', '01', '003', '0002', '00010', 0,    CAST('2008-01-07' AS DATE), 'CANCELADO', 3, '123456');


-- Tabla: movimiento
INSERT INTO movimiento (
    chr_cuencodigo,
    int_movinumero,
    dtt_movifecha,
    chr_emplcodigo,
    chr_tipocodigo,
    dec_moviimporte,
    chr_cuenreferencia
)
VALUES
('00100002', 1, CAST('2008-01-08 00:00:00' AS DATETIME), '0004', '001', 1800, NULL),
('00100002', 2, CAST('2008-01-25 00:00:00' AS DATETIME), '0004', '004', 1000, NULL),
('00100002', 3, CAST('2008-02-13 00:00:00' AS DATETIME), '0004', '003', 2200, NULL),
('00100002', 4, CAST('2008-03-08 00:00:00' AS DATETIME), '0004', '003', 1500, NULL),

('00100001', 1, CAST('2008-01-06 00:00:00' AS DATETIME), '0004', '001', 2800, NULL),
('00100001', 2, CAST('2008-01-15 00:00:00' AS DATETIME), '0004', '003', 3200, NULL),
('00100001', 3, CAST('2008-01-20 00:00:00' AS DATETIME), '0004', '004', 800, NULL),
('00100001', 4, CAST('2008-02-14 00:00:00' AS DATETIME), '0004', '003', 2000, NULL),
('00100001', 5, CAST('2008-02-25 00:00:00' AS DATETIME), '0004', '004', 500, NULL),
('00100001', 6, CAST('2008-03-03 00:00:00' AS DATETIME), '0004', '004', 800, NULL),
('00100001', 7, CAST('2008-03-15 00:00:00' AS DATETIME), '0004', '003', 1000, NULL),

('00200003', 1, CAST('2008-01-11 00:00:00' AS DATETIME), '0001', '001', 2500, NULL),
('00200003', 2, CAST('2008-01-17 00:00:00' AS DATETIME), '0001', '003', 1500, NULL),
('00200003', 3, CAST('2008-01-20 00:00:00' AS DATETIME), '0001', '004', 500, NULL),
('00200003', 4, CAST('2008-02-09 00:00:00' AS DATETIME), '0001', '004', 500, NULL),
('00200003', 5, CAST('2008-02-25 00:00:00' AS DATETIME), '0001', '003', 3500, NULL),
('00200003', 6, CAST('2008-03-11 00:00:00' AS DATETIME), '0001', '004', 500, NULL),

('00200002', 1, CAST('2008-01-09 00:00:00' AS DATETIME), '0001', '001', 3800, NULL),
('00200002', 2, CAST('2008-01-20 00:00:00' AS DATETIME), '0001', '003', 4200, NULL),
('00200002', 3, CAST('2008-03-06 00:00:00' AS DATETIME), '0001', '004', 1200, NULL),

('00200001', 1, CAST('2008-01-05 00:00:00' AS DATETIME), '0001', '001', 5000, NULL),
('00200001', 2, CAST('2008-01-07 00:00:00' AS DATETIME), '0001', '003', 4000, NULL),
('00200001', 3, CAST('2008-01-09 00:00:00' AS DATETIME), '0001', '004', 2000, NULL),
('00200001', 4, CAST('2008-01-11 00:00:00' AS DATETIME), '0001', '003', 1000, NULL),
('00200001', 5, CAST('2008-01-13 00:00:00' AS DATETIME), '0001', '003', 2000, NULL),
('00200001', 6, CAST('2008-01-15 00:00:00' AS DATETIME), '0001', '004', 4000, NULL),
('00200001', 7, CAST('2008-01-19 00:00:00' AS DATETIME), '0001', '003', 2000, NULL),
('00200001', 8, CAST('2008-01-21 00:00:00' AS DATETIME), '0001', '004', 3000, NULL),
('00200001', 9, CAST('2008-01-23 00:00:00' AS DATETIME), '0001', '003', 7000, NULL),
('00200001',10, CAST('2008-01-27 00:00:00' AS DATETIME), '0001', '004', 1000, NULL),
('00200001',11, CAST('2008-01-30 00:00:00' AS DATETIME), '0001', '004', 3000, NULL),
('00200001',12, CAST('2008-02-04 00:00:00' AS DATETIME), '0001', '003', 2000, NULL),
('00200001',13, CAST('2008-02-08 00:00:00' AS DATETIME), '0001', '004', 4000, NULL),
('00200001',14, CAST('2008-02-13 00:00:00' AS DATETIME), '0001', '003', 2000, NULL),
('00200001',15, CAST('2008-02-19 00:00:00' AS DATETIME), '0001', '004', 1000, NULL),

('00300001', 1, CAST('2008-01-07 00:00:00' AS DATETIME), '0002', '001', 5600, NULL),
('00300001', 2, CAST('2008-01-18 00:00:00' AS DATETIME), '0002', '003', 1400, NULL),
('00300001', 3, CAST('2008-01-25 00:00:00' AS DATETIME), '0002', '002', 7000, NULL);

-- Tabla: contador
INSERT INTO contador (
    vch_conttabla,
    int_contitem,
    int_contlongitud
)
VALUES
('moneda', 2, 2),
('tipomovimiento', 10, 3),
('sucursal', 7, 3),
('empleado', 14, 4),
('asignado', 11, 6),
('parametro', 2, 3),
('cliente', 20, 5);

UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'internet'), 2) WHERE chr_emplcodigo = '9999';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'chicho'), 2) WHERE chr_emplcodigo = '0001';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'flaca'), 2) WHERE chr_emplcodigo = '0002';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'linda'), 2) WHERE chr_emplcodigo = '0003';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'china'), 2) WHERE chr_emplcodigo = '0004';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'angel'), 2) WHERE chr_emplcodigo = '0005';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'cerebro'), 2) WHERE chr_emplcodigo = '0006';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'princesa'), 2) WHERE chr_emplcodigo = '0007';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'chinita'), 2) WHERE chr_emplcodigo = '0008';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'gato'), 2) WHERE chr_emplcodigo = '0009';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'machupichu'), 2) WHERE chr_emplcodigo = '0010';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'tinajones'), 2) WHERE chr_emplcodigo = '0011';
UPDATE usuario SET vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', 'MONSTER9'), 2) WHERE chr_emplcodigo = '0012';