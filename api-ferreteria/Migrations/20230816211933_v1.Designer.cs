﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api_ferreteria;

namespace api_ferreteria.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230816211933_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api_ferreteria.Entitys.Categoria", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Cliente", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("numdocumento")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Compra", b =>
                {
                    b.Property<int>("numero")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProveedorId")
                        .HasColumnType("int");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("fechaCompra")
                        .HasColumnType("date");

                    b.Property<decimal>("igv")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("subtotal")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(20,2)");

                    b.HasKey("numero");

                    b.HasIndex("ProveedorId");

                    b.ToTable("Compra");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Comprobante", b =>
                {
                    b.Property<int>("numero")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<string>("Documento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FormaPago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<string>("comentario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("date");

                    b.Property<decimal>("igv")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("subtotal")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(20,2)");

                    b.HasKey("numero");

                    b.HasIndex("ClienteId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Comprobante");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Detalle", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComprobanteNumero")
                        .HasColumnType("int");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<decimal>("importe")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("precio")
                        .HasColumnType("decimal(20,2)");

                    b.HasKey("id");

                    b.HasIndex("ComprobanteNumero");

                    b.HasIndex("ProductoId");

                    b.ToTable("Detalle");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.DetalleCompra", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompraNumero")
                        .HasColumnType("int");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<decimal>("importe")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("precioCompra")
                        .HasColumnType("decimal(20,2)");

                    b.HasKey("id");

                    b.HasIndex("CompraNumero");

                    b.HasIndex("ProductoId");

                    b.ToTable("DetalleCompra");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Empleado", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<string>("apellido")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("id");

                    b.HasIndex("RolId");

                    b.ToTable("Empleado");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Marca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Marca");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Producto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int>("MarcaId")
                        .HasColumnType("int");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("precioVenta")
                        .HasColumnType("decimal(20,2)");

                    b.Property<int>("stock")
                        .HasColumnType("int");

                    b.Property<string>("url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("MarcaId");

                    b.ToTable("Producto");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.ProductoProveedor", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int>("ProveedorId")
                        .HasColumnType("int");

                    b.Property<decimal>("precioCompra")
                        .HasColumnType("decimal(20,2)");

                    b.HasKey("id");

                    b.HasIndex("ProductoId");

                    b.HasIndex("ProveedorId");

                    b.ToTable("ProductoProveedor");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Proveedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreContacto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Razon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ruc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonoContacto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Proveedor");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Rol", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("id");

                    b.ToTable("Rol");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Usuario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmpleadoId")
                        .HasColumnType("int");

                    b.Property<byte[]>("StoredSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("contraseña")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("id");

                    b.HasIndex("EmpleadoId")
                        .IsUnique();

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Compra", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Proveedor", null)
                        .WithMany("compra")
                        .HasForeignKey("ProveedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Comprobante", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Cliente", null)
                        .WithMany("comprobante")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_ferreteria.Entitys.Usuario", null)
                        .WithMany("comprobante")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Detalle", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Comprobante", null)
                        .WithMany("detalle")
                        .HasForeignKey("ComprobanteNumero")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_ferreteria.Entitys.Producto", null)
                        .WithMany("detalle")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.DetalleCompra", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Compra", "Compra")
                        .WithMany("DetalleCompra")
                        .HasForeignKey("CompraNumero")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_ferreteria.Entitys.Producto", null)
                        .WithMany("detalleCompra")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Compra");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Empleado", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Rol", null)
                        .WithMany("empleado")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Producto", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Categoria", null)
                        .WithMany("producto")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_ferreteria.Entitys.Marca", null)
                        .WithMany("producto")
                        .HasForeignKey("MarcaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.ProductoProveedor", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Producto", null)
                        .WithMany("productoProveedor")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_ferreteria.Entitys.Proveedor", null)
                        .WithMany("productoProveedor")
                        .HasForeignKey("ProveedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Usuario", b =>
                {
                    b.HasOne("api_ferreteria.Entitys.Empleado", null)
                        .WithOne("usuario")
                        .HasForeignKey("api_ferreteria.Entitys.Usuario", "EmpleadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Categoria", b =>
                {
                    b.Navigation("producto");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Cliente", b =>
                {
                    b.Navigation("comprobante");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Compra", b =>
                {
                    b.Navigation("DetalleCompra");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Comprobante", b =>
                {
                    b.Navigation("detalle");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Empleado", b =>
                {
                    b.Navigation("usuario");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Marca", b =>
                {
                    b.Navigation("producto");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Producto", b =>
                {
                    b.Navigation("detalle");

                    b.Navigation("detalleCompra");

                    b.Navigation("productoProveedor");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Proveedor", b =>
                {
                    b.Navigation("compra");

                    b.Navigation("productoProveedor");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Rol", b =>
                {
                    b.Navigation("empleado");
                });

            modelBuilder.Entity("api_ferreteria.Entitys.Usuario", b =>
                {
                    b.Navigation("comprobante");
                });
#pragma warning restore 612, 618
        }
    }
}
