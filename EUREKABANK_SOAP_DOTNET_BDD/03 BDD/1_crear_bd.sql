-- =============================================
-- Creacion de los Objetos de la Base de Datos para SQL Server
-- =============================================

CREATE TABLE tipomovimiento (
	chr_tipocodigo       CHAR(3) NOT NULL,
	vch_tipodescripcion  VARCHAR(40) NOT NULL,
	vch_tipoaccion       VARCHAR(10) NOT NULL,
	vch_tipoestado       VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT PK_TipoMovimiento PRIMARY KEY (chr_tipocodigo),
	CONSTRAINT chk_tipomovimiento_vch_tipoaccion CHECK (vch_tipoaccion IN ('INGRESO', 'SALIDA')),
	CONSTRAINT chk_tipomovimiento_vch_tipoestado CHECK (vch_tipoestado IN ('ACTIVO', 'ANULADO', 'CANCELADO'))
);

CREATE TABLE sucursal (
	chr_sucucodigo       CHAR(3) NOT NULL,
	vch_sucunombre       VARCHAR(50) NOT NULL,
	vch_sucuciudad       VARCHAR(30) NOT NULL,
	vch_sucudireccion    VARCHAR(50),
	int_sucucontcuenta   INT NOT NULL,
	CONSTRAINT PK_Sucursal PRIMARY KEY (chr_sucucodigo)
);

CREATE TABLE empleado (
	chr_emplcodigo       CHAR(4) NOT NULL,
	vch_emplpaterno      VARCHAR(25) NOT NULL,
	vch_emplmaterno      VARCHAR(25) NOT NULL,
	vch_emplnombre       VARCHAR(30) NOT NULL,
	vch_emplciudad       VARCHAR(30) NOT NULL,
	vch_empldireccion    VARCHAR(50),
	CONSTRAINT PK_Empleado PRIMARY KEY (chr_emplcodigo)
);

CREATE TABLE modulo (
	int_moducodigo       INT NOT NULL,
	vch_modunombre       VARCHAR(50),
	vch_moduestado       VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT chk_modulo_vch_moduestado CHECK (vch_moduestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Modulo PRIMARY KEY (int_moducodigo)
);

CREATE TABLE usuario (
	chr_emplcodigo       CHAR(4) NOT NULL,
	vch_emplusuario      VARCHAR(20) NOT NULL UNIQUE,
	vch_emplclave        VARCHAR(50) NOT NULL,
	vch_emplestado       VARCHAR(15) DEFAULT 'ACTIVO',
	CONSTRAINT chk_usuario_vch_emplestado CHECK (vch_emplestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Usuario PRIMARY KEY (chr_emplcodigo),
	FOREIGN KEY (chr_emplcodigo) REFERENCES empleado (chr_emplcodigo)
);

CREATE TABLE permiso (
	chr_emplcodigo       CHAR(4) NOT NULL,
	int_moducodigo       INT NOT NULL,
	vch_permestado       VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT chk_permiso_vch_permestado CHECK (vch_permestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Permiso PRIMARY KEY (chr_emplcodigo, int_moducodigo),
	FOREIGN KEY (int_moducodigo) REFERENCES modulo (int_moducodigo),
	FOREIGN KEY (chr_emplcodigo) REFERENCES usuario (chr_emplcodigo)
);

CREATE TABLE LOGSESSION (
	ID                 INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	chr_emplcodigo     CHAR(4) NOT NULL,
	fec_ingreso        DATETIME NOT NULL,
	fec_salida         DATETIME,
	ip                 VARCHAR(20) NOT NULL DEFAULT 'NONE',
	hostname           VARCHAR(100) NOT NULL DEFAULT 'NONE',
	FOREIGN KEY (chr_emplcodigo) REFERENCES empleado (chr_emplcodigo)
);

CREATE TABLE asignado (
	chr_asigcodigo       CHAR(6) NOT NULL,
	chr_sucucodigo       CHAR(3) NOT NULL,
	chr_emplcodigo       CHAR(4) NOT NULL,
	dtt_asigfechaalta    DATE NOT NULL,
	dtt_asigfechabaja    DATE,
	CONSTRAINT PK_Asignado PRIMARY KEY (chr_asigcodigo),
	FOREIGN KEY (chr_emplcodigo) REFERENCES empleado (chr_emplcodigo),
	FOREIGN KEY (chr_sucucodigo) REFERENCES sucursal (chr_sucucodigo)
);

CREATE TABLE cliente (
	chr_cliecodigo       CHAR(5) NOT NULL,
	vch_cliepaterno      VARCHAR(25) NOT NULL,
	vch_cliematerno      VARCHAR(25) NOT NULL,
	vch_clienombre       VARCHAR(30) NOT NULL,
	chr_cliedni          CHAR(8) NOT NULL,
	vch_clieciudad       VARCHAR(30) NOT NULL,
	vch_cliedireccion    VARCHAR(50) NOT NULL,
	vch_clietelefono     VARCHAR(20),
	vch_clieemail        VARCHAR(50),
	CONSTRAINT PK_Cliente PRIMARY KEY (chr_cliecodigo)
);

CREATE TABLE moneda (
	chr_monecodigo       CHAR(2) NOT NULL,
	vch_monedescripcion  VARCHAR(20) NOT NULL,
	CONSTRAINT PK_Moneda PRIMARY KEY (chr_monecodigo)
);

CREATE TABLE cuenta (
	chr_cuencodigo       CHAR(8) NOT NULL,
	chr_monecodigo       CHAR(2) NOT NULL,
	chr_sucucodigo       CHAR(3) NOT NULL,
	chr_emplcreacuenta   CHAR(4) NOT NULL,
	chr_cliecodigo       CHAR(5) NOT NULL,
	dec_cuensaldo        DECIMAL(12,2) NOT NULL,
	dtt_cuenfechacreacion DATE NOT NULL,
	vch_cuenestado       VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	int_cuencontmov      INT NOT NULL,
	chr_cuenclave        CHAR(6) NOT NULL,
	CONSTRAINT chk_cuenta_vch_cuenestado CHECK (vch_cuenestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Cuenta PRIMARY KEY (chr_cuencodigo),
	FOREIGN KEY (chr_cliecodigo) REFERENCES cliente (chr_cliecodigo),
	FOREIGN KEY (chr_emplcreacuenta) REFERENCES empleado (chr_emplcodigo),
	FOREIGN KEY (chr_sucucodigo) REFERENCES sucursal (chr_sucucodigo),
	FOREIGN KEY (chr_monecodigo) REFERENCES moneda (chr_monecodigo)
);

CREATE TABLE movimiento (
	chr_cuencodigo       CHAR(8) NOT NULL,
	int_movinumero       INT NOT NULL,
	dtt_movifecha        DATETIME NOT NULL,
	chr_emplcodigo       CHAR(4) NOT NULL,
	chr_tipocodigo       CHAR(3) NOT NULL,
	dec_moviimporte      DECIMAL(12,2) NOT NULL,
	chr_cuenreferencia   CHAR(8),
	CONSTRAINT chk_movimiento_dec_moviimporte CHECK (dec_moviimporte >= 0.0),
	CONSTRAINT PK_Movimiento PRIMARY KEY (chr_cuencodigo, int_movinumero),
	FOREIGN KEY (chr_tipocodigo) REFERENCES tipomovimiento (chr_tipocodigo),
	FOREIGN KEY (chr_emplcodigo) REFERENCES empleado (chr_emplcodigo),
	FOREIGN KEY (chr_cuencodigo) REFERENCES cuenta (chr_cuencodigo)
);

CREATE TABLE parametro (
	chr_paracodigo       CHAR(3) NOT NULL,
	vch_paradescripcion  VARCHAR(50) NOT NULL,
	vch_paravalor        VARCHAR(70) NOT NULL,
	vch_paraestado       VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT chk_parametro_vch_paraestado CHECK (vch_paraestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Parametro PRIMARY KEY (chr_paracodigo)
);

CREATE TABLE interesmensual (
	chr_monecodigo       CHAR(2) NOT NULL,
	dec_inteimporte      DECIMAL(12,2) NOT NULL,
	CONSTRAINT PK_InteresMensual PRIMARY KEY (chr_monecodigo),
	FOREIGN KEY (chr_monecodigo) REFERENCES moneda (chr_monecodigo)
);

CREATE TABLE costomovimiento (
	chr_monecodigo       CHAR(2) NOT NULL,
	dec_costimporte      DECIMAL(12,2) NOT NULL,
	CONSTRAINT PK_CostoMovimiento PRIMARY KEY (chr_monecodigo),
	FOREIGN KEY (chr_monecodigo) REFERENCES moneda (chr_monecodigo)
);

CREATE TABLE cargomantenimiento (
	chr_monecodigo       CHAR(2) NOT NULL,
	dec_cargMontoMaximo  DECIMAL(12,2) NOT NULL,
	dec_cargImporte      DECIMAL(12,2) NOT NULL,
	CONSTRAINT PK_CargoMantenimiento PRIMARY KEY (chr_monecodigo),
	FOREIGN KEY (chr_monecodigo) REFERENCES moneda (chr_monecodigo)
);

CREATE TABLE contador (
	vch_conttabla        VARCHAR(30) NOT NULL,
	int_contitem         INT NOT NULL,
	int_contlongitud     INT NOT NULL,
	CONSTRAINT PK_Contador PRIMARY KEY (vch_conttabla)
);
