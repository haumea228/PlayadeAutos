using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayaAutos.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CondicionesVehiculo",
                columns: table => new
                {
                    CondicionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicionesVehiculo", x => x.CondicionId);
                });

            migrationBuilder.CreateTable(
                name: "EstadosCita",
                columns: table => new
                {
                    EstadoCitaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCita", x => x.EstadoCitaId);
                });

            migrationBuilder.CreateTable(
                name: "EstadosVehiculo",
                columns: table => new
                {
                    EstadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermiteVenta = table.Column<bool>(type: "bit", nullable: false),
                    VisibleEnCatalogo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosVehiculo", x => x.EstadoId);
                });

            migrationBuilder.CreateTable(
                name: "FormasPago",
                columns: table => new
                {
                    FormaPagoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneraNotaCredito = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPago", x => x.FormaPagoId);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    MarcaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.MarcaId);
                });

            migrationBuilder.CreateTable(
                name: "OrigenesVehiculo",
                columns: table => new
                {
                    OrigenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrigenesVehiculo", x => x.OrigenId);
                });

            migrationBuilder.CreateTable(
                name: "Timbrados",
                columns: table => new
                {
                    TimbradoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroTimbrado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroDesde = table.Column<long>(type: "bigint", nullable: false),
                    NumeroHasta = table.Column<long>(type: "bigint", nullable: false),
                    UltimoNumeroUsado = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timbrados", x => x.TimbradoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposGasto",
                columns: table => new
                {
                    TipoGastoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposGasto", x => x.TipoGastoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposMovimiento",
                columns: table => new
                {
                    TipoMovimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AfectaCaja = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMovimiento", x => x.TipoMovimientoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposVehiculo",
                columns: table => new
                {
                    TipoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposVehiculo", x => x.TipoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposVenta",
                columns: table => new
                {
                    TipoVentaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiereFinanciacion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposVenta", x => x.TipoVentaId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivoUsuario = table.Column<bool>(type: "bit", nullable: false),
                    FechaAltaUsuario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GAMUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    ModeloId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarcaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.ModeloId);
                    table.ForeignKey(
                        name: "FK_Modelos_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "MarcaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CI_RUC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangoPrecioMin = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    RangoPrecioMax = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Consignantes",
                columns: table => new
                {
                    ConsignanteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    PorcentajeComisionDefault = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consignantes", x => x.ConsignanteId);
                    table.ForeignKey(
                        name: "FK_Consignantes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    VehiculoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoInterno = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModeloId = table.Column<int>(type: "int", nullable: false),
                    TipoId = table.Column<int>(type: "int", nullable: false),
                    CondicionId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    OrigenId = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kilometraje = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    PrecioVenta = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    CostoAdquisicion = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsignanteId = table.Column<int>(type: "int", nullable: true),
                    PorcentajeComision = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaConsigna = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAlta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.VehiculoId);
                    table.ForeignKey(
                        name: "FK_Vehiculos_CondicionesVehiculo_CondicionId",
                        column: x => x.CondicionId,
                        principalTable: "CondicionesVehiculo",
                        principalColumn: "CondicionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Consignantes_ConsignanteId",
                        column: x => x.ConsignanteId,
                        principalTable: "Consignantes",
                        principalColumn: "ConsignanteId");
                    table.ForeignKey(
                        name: "FK_Vehiculos_EstadosVehiculo_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadosVehiculo",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "ModeloId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehiculos_OrigenesVehiculo_OrigenId",
                        column: x => x.OrigenId,
                        principalTable: "OrigenesVehiculo",
                        principalColumn: "OrigenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehiculos_TiposVehiculo_TipoId",
                        column: x => x.TipoId,
                        principalTable: "TiposVehiculo",
                        principalColumn: "TipoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    CitaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: true),
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    EstadoCitaId = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoCita = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.CitaId);
                    table.ForeignKey(
                        name: "FK_Citas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_EstadosCita_EstadoCitaId",
                        column: x => x.EstadoCitaId,
                        principalTable: "EstadosCita",
                        principalColumn: "EstadoCitaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citas_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoId");
                });

            migrationBuilder.CreateTable(
                name: "ContratosConsigna",
                columns: table => new
                {
                    ContratoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    ConsignanteId = table.Column<int>(type: "int", nullable: false),
                    NumeroContrato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PorcentajeComision = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Clausulas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaPDF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratosConsigna", x => x.ContratoId);
                    table.ForeignKey(
                        name: "FK_ContratosConsigna_Consignantes_ConsignanteId",
                        column: x => x.ConsignanteId,
                        principalTable: "Consignantes",
                        principalColumn: "ConsignanteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContratosConsigna_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FotosVehiculo",
                columns: table => new
                {
                    FotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotosVehiculo", x => x.FotoId);
                    table.ForeignKey(
                        name: "FK_FotosVehiculo_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GastosVehiculo",
                columns: table => new
                {
                    GastoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    TipoGastoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioRegistro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastosVehiculo", x => x.GastoId);
                    table.ForeignKey(
                        name: "FK_GastosVehiculo_TiposGasto_TipoGastoId",
                        column: x => x.TipoGastoId,
                        principalTable: "TiposGasto",
                        principalColumn: "TipoGastoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GastosVehiculo_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    VentaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    TipoVentaId = table.Column<int>(type: "int", nullable: false),
                    FormaPagoId = table.Column<int>(type: "int", nullable: false),
                    FechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    MontoEntrada = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    SaldoFinanciado = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    TasaInteres = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CantidadCuotas = table.Column<int>(type: "int", nullable: true),
                    VehiculoPermutaId = table.Column<int>(type: "int", nullable: true),
                    ValorPermuta = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoAnulacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.VentaId);
                    table.ForeignKey(
                        name: "FK_Ventas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ventas_FormasPago_FormaPagoId",
                        column: x => x.FormaPagoId,
                        principalTable: "FormasPago",
                        principalColumn: "FormaPagoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ventas_TiposVenta_TipoVentaId",
                        column: x => x.TipoVentaId,
                        principalTable: "TiposVenta",
                        principalColumn: "TipoVentaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ventas_Usuarios_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ventas_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cuotas",
                columns: table => new
                {
                    CuotaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    NumeroCuota = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontoPagado = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    MontoRecargo = table.Column<decimal>(type: "numeric(15,0)", nullable: true),
                    FormaPagoId = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistroPago = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuotas", x => x.CuotaId);
                    table.ForeignKey(
                        name: "FK_Cuotas_FormasPago_FormaPagoId",
                        column: x => x.FormaPagoId,
                        principalTable: "FormasPago",
                        principalColumn: "FormaPagoId");
                    table.ForeignKey(
                        name: "FK_Cuotas_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "VentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    IVA = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    RutaPDF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioGeneracion = table.Column<int>(type: "int", nullable: false),
                    TimbradoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.FacturaId);
                    table.ForeignKey(
                        name: "FK_Facturas_Timbrados_TimbradoId",
                        column: x => x.TimbradoId,
                        principalTable: "Timbrados",
                        principalColumn: "TimbradoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facturas_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "VentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotasCredito",
                columns: table => new
                {
                    NotaCreditoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    NumeroNota = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    RutaPDF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioGeneracion = table.Column<int>(type: "int", nullable: false),
                    TimbradoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasCredito", x => x.NotaCreditoId);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Timbrados_TimbradoId",
                        column: x => x.TimbradoId,
                        principalTable: "Timbrados",
                        principalColumn: "TimbradoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "VentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosCaja",
                columns: table => new
                {
                    MovimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoMovimientoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(15,0)", nullable: false),
                    VentaId = table.Column<int>(type: "int", nullable: true),
                    CuotaId = table.Column<int>(type: "int", nullable: true),
                    GastoId = table.Column<int>(type: "int", nullable: true),
                    FormaPagoId = table.Column<int>(type: "int", nullable: true),
                    UsuarioRegistro = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosCaja", x => x.MovimientoId);
                    table.ForeignKey(
                        name: "FK_MovimientosCaja_Cuotas_CuotaId",
                        column: x => x.CuotaId,
                        principalTable: "Cuotas",
                        principalColumn: "CuotaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientosCaja_GastosVehiculo_GastoId",
                        column: x => x.GastoId,
                        principalTable: "GastosVehiculo",
                        principalColumn: "GastoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientosCaja_TiposMovimiento_TipoMovimientoId",
                        column: x => x.TipoMovimientoId,
                        principalTable: "TiposMovimiento",
                        principalColumn: "TipoMovimientoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosCaja_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "VentaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ClienteId",
                table: "Citas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_EstadoCitaId",
                table: "Citas",
                column: "EstadoCitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_VehiculoId",
                table: "Citas",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CI_RUC",
                table: "Clientes",
                column: "CI_RUC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId",
                table: "Clientes",
                column: "UsuarioId",
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CondicionesVehiculo_Descripcion",
                table: "CondicionesVehiculo",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consignantes_ClienteId",
                table: "Consignantes",
                column: "ClienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContratosConsigna_ConsignanteId",
                table: "ContratosConsigna",
                column: "ConsignanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratosConsigna_VehiculoId",
                table: "ContratosConsigna",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_FormaPagoId",
                table: "Cuotas",
                column: "FormaPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_VentaId",
                table: "Cuotas",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosVehiculo_Descripcion",
                table: "EstadosVehiculo",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_NumeroFactura",
                table: "Facturas",
                column: "NumeroFactura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_TimbradoId",
                table: "Facturas",
                column: "TimbradoId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_VentaId",
                table: "Facturas",
                column: "VentaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FotosVehiculo_VehiculoId",
                table: "FotosVehiculo",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosVehiculo_TipoGastoId",
                table: "GastosVehiculo",
                column: "TipoGastoId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosVehiculo_VehiculoId",
                table: "GastosVehiculo",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcas_Nombre",
                table: "Marcas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_MarcaId",
                table: "Modelos",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosCaja_CuotaId",
                table: "MovimientosCaja",
                column: "CuotaId",
                unique: true,
                filter: "[CuotaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosCaja_GastoId",
                table: "MovimientosCaja",
                column: "GastoId",
                unique: true,
                filter: "[GastoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosCaja_TipoMovimientoId",
                table: "MovimientosCaja",
                column: "TipoMovimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosCaja_VentaId",
                table: "MovimientosCaja",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_NumeroNota",
                table: "NotasCredito",
                column: "NumeroNota",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_TimbradoId",
                table: "NotasCredito",
                column: "TimbradoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_VehiculoId",
                table: "NotasCredito",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_VentaId",
                table: "NotasCredito",
                column: "VentaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposGasto_Descripcion",
                table: "TiposGasto",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposVehiculo_Descripcion",
                table: "TiposVehiculo",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UsuarioEmail",
                table: "Usuarios",
                column: "UsuarioEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_CodigoInterno",
                table: "Vehiculos",
                column: "CodigoInterno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_CondicionId",
                table: "Vehiculos",
                column: "CondicionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_ConsignanteId",
                table: "Vehiculos",
                column: "ConsignanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_EstadoId",
                table: "Vehiculos",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_ModeloId",
                table: "Vehiculos",
                column: "ModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_OrigenId",
                table: "Vehiculos",
                column: "OrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_TipoId",
                table: "Vehiculos",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_ClienteId",
                table: "Ventas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_FormaPagoId",
                table: "Ventas",
                column: "FormaPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_TipoVentaId",
                table: "Ventas",
                column: "TipoVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_VehiculoId",
                table: "Ventas",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_VendedorId",
                table: "Ventas",
                column: "VendedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "ContratosConsigna");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "FotosVehiculo");

            migrationBuilder.DropTable(
                name: "MovimientosCaja");

            migrationBuilder.DropTable(
                name: "NotasCredito");

            migrationBuilder.DropTable(
                name: "EstadosCita");

            migrationBuilder.DropTable(
                name: "Cuotas");

            migrationBuilder.DropTable(
                name: "GastosVehiculo");

            migrationBuilder.DropTable(
                name: "TiposMovimiento");

            migrationBuilder.DropTable(
                name: "Timbrados");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "TiposGasto");

            migrationBuilder.DropTable(
                name: "FormasPago");

            migrationBuilder.DropTable(
                name: "TiposVenta");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "CondicionesVehiculo");

            migrationBuilder.DropTable(
                name: "Consignantes");

            migrationBuilder.DropTable(
                name: "EstadosVehiculo");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropTable(
                name: "OrigenesVehiculo");

            migrationBuilder.DropTable(
                name: "TiposVehiculo");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
