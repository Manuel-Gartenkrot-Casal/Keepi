USE [master]
GO
/****** Object:  Database [Keepi_DataBase]    Script Date: 14/11/2025 09:02:43 ******/
CREATE DATABASE [Keepi_DataBase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Keepi_DataBase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Keepi_DataBase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Keepi_DataBase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Keepi_DataBase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Keepi_DataBase] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Keepi_DataBase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Keepi_DataBase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET ARITHABORT OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Keepi_DataBase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Keepi_DataBase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Keepi_DataBase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Keepi_DataBase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET RECOVERY FULL 
GO
ALTER DATABASE [Keepi_DataBase] SET  MULTI_USER 
GO
ALTER DATABASE [Keepi_DataBase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Keepi_DataBase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Keepi_DataBase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Keepi_DataBase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Keepi_DataBase] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Keepi_DataBase', N'ON'
GO
ALTER DATABASE [Keepi_DataBase] SET QUERY_STORE = OFF
GO
USE [Keepi_DataBase]
GO
/****** Object:  User [alumno]    Script Date: 14/11/2025 09:02:43 ******/
CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Categoria]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categoria](
	[IdCategoria] [int] IDENTITY(1,1) NOT NULL,
	[nombreCategoria] [varchar](50) NOT NULL,
	[porcentajeDeCambio] [decimal](6, 3) NOT NULL,
 CONSTRAINT [PK_Categoria] PRIMARY KEY CLUSTERED 
(
	[IdCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Heladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Heladera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Eliminado] [bit] NOT NULL,
 CONSTRAINT [PK__Heladera__3214EC075F9005F4] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificacion]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificacion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mensaje] [varchar](100) NOT NULL,
	[FechaNotificacion] [date] NOT NULL,
	[Leida] [bit] NOT NULL,
	[IdProductoxHeladera] [int] NOT NULL,
 CONSTRAINT [PK_Notificacion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Duracion] [int] NOT NULL,
	[Favorito] [bit] NOT NULL,
	[IdCategoria] [int] NOT NULL,
	[PorcAcidez] [decimal](6, 3) NOT NULL,
	[PorcAgua] [decimal](6, 3) NOT NULL,
	[PorcAzucar] [decimal](6, 3) NOT NULL,
	[PorcConservantes] [decimal](6, 3) NOT NULL,
	[PorcAlcohol] [decimal](6, 3) NOT NULL,
 CONSTRAINT [PK__Producto__3214EC076B2DFAD0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoXHeladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoXHeladera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdHeladera] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[NombreEspecifico] [varchar](50) NULL,
	[FechaVencimiento] [date] NULL,
	[Eliminado] [bit] NULL,
	[Abierto] [bit] NULL,
	[Foto] [varchar](50) NULL,
	[Estado] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoXReceta]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoXReceta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProducto] [int] NOT NULL,
	[IdReceta] [int] NOT NULL,
	[CantidadNecesaria] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receta]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Favorito] [bit] NULL,
	[Duracion] [int] NULL,
	[Popularidad] [int] NULL,
	[Raciones] [int] NULL,
	[Instrucciones] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
 CONSTRAINT [PK__Usuario__3214EC078C41C33D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioXHeladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioXHeladera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdHeladera] [int] NOT NULL,
	[EsDueño] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categoria] ON 

INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (1, N'Lácteos', CAST(1.050 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (2, N'Bebidas', CAST(1.020 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (3, N'Carnes', CAST(1.070 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (4, N'Verduras', CAST(1.030 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (5, N'Frutas', CAST(1.040 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (6, N'Congelados', CAST(1.060 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (7, N'Panificados', CAST(1.045 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (8, N'Snacks', CAST(1.080 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (9, N'Salsas', CAST(1.035 AS Decimal(6, 3)))
INSERT [dbo].[Categoria] ([IdCategoria], [nombreCategoria], [porcentajeDeCambio]) VALUES (10, N'Dulces', CAST(1.055 AS Decimal(6, 3)))
SET IDENTITY_INSERT [dbo].[Categoria] OFF
GO
SET IDENTITY_INSERT [dbo].[Heladera] ON 

INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (1, N'Blanca', N'Mi Primera Heladera', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (2, N'Blanca', N'Heladera Country', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (3, N'Negra', N'Heladera Cocina Principal', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (4, N'Plateada', N'Heladera de Bebidas', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (5, N'Gris', N'Heladera del Quincho', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (6, N'Roja', N'MiniBar', 1)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (7, N'Azul', N'Heladera Oficina', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (8, N'Blanca', N'Heladera Familiar', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (9, N'Verde', N'Heladera Eco', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (10, N'Negra', N'Heladera Secundaria', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (11, N'Gris Oscuro', N'Heladera del Garage', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (12, N'Blanca', N'Heladera Invierno', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (16, N'rojo', N'tomas', 1)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (17, N'rojo', N'pepe', 1)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (18, N'rojo', N'lolo', 0)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (19, N'rojo', N'pepe', 1)
INSERT [dbo].[Heladera] ([Id], [Color], [Nombre], [Eliminado]) VALUES (20, N'rojo', N'hola', 0)
SET IDENTITY_INSERT [dbo].[Heladera] OFF
GO
SET IDENTITY_INSERT [dbo].[Producto] ON 

INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (1, N'Leche entera', 10, 0, 1, CAST(0.020 AS Decimal(6, 3)), CAST(89.000 AS Decimal(6, 3)), CAST(4.800 AS Decimal(6, 3)), CAST(0.200 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (2, N'Yogur natural', 15, 1, 1, CAST(0.040 AS Decimal(6, 3)), CAST(80.000 AS Decimal(6, 3)), CAST(5.000 AS Decimal(6, 3)), CAST(0.100 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (3, N'Carne vacuna', 5, 0, 3, CAST(0.010 AS Decimal(6, 3)), CAST(60.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (4, N'Pollo fresco', 6, 0, 3, CAST(0.015 AS Decimal(6, 3)), CAST(65.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (5, N'Manzana', 20, 1, 5, CAST(0.030 AS Decimal(6, 3)), CAST(85.000 AS Decimal(6, 3)), CAST(10.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (6, N'Lechuga', 5, 0, 4, CAST(0.015 AS Decimal(6, 3)), CAST(93.000 AS Decimal(6, 3)), CAST(0.800 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (7, N'Coca-Cola', 120, 1, 2, CAST(0.100 AS Decimal(6, 3)), CAST(88.000 AS Decimal(6, 3)), CAST(10.000 AS Decimal(6, 3)), CAST(0.100 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (8, N'Cerveza', 180, 1, 2, CAST(0.050 AS Decimal(6, 3)), CAST(90.000 AS Decimal(6, 3)), CAST(3.000 AS Decimal(6, 3)), CAST(0.200 AS Decimal(6, 3)), CAST(4.500 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (9, N'Pan lactal', 7, 0, 7, CAST(0.010 AS Decimal(6, 3)), CAST(35.000 AS Decimal(6, 3)), CAST(5.500 AS Decimal(6, 3)), CAST(0.200 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
INSERT [dbo].[Producto] ([Id], [Nombre], [Duracion], [Favorito], [IdCategoria], [PorcAcidez], [PorcAgua], [PorcAzucar], [PorcConservantes], [PorcAlcohol]) VALUES (10, N'Queso cremoso', 25, 1, 1, CAST(0.020 AS Decimal(6, 3)), CAST(50.000 AS Decimal(6, 3)), CAST(1.000 AS Decimal(6, 3)), CAST(0.500 AS Decimal(6, 3)), CAST(0.000 AS Decimal(6, 3)))
SET IDENTITY_INSERT [dbo].[Producto] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductoXHeladera] ON 

INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (1, 1, 1, N'Leche La Serenísima', CAST(N'2025-10-25' AS Date), 0, 0, N'leche.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (2, 1, 2, N'Yogur Ser Natural', CAST(N'2025-10-30' AS Date), 0, 0, N'yogur.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (3, 2, 3, N'Carne para milanesa', CAST(N'2025-10-20' AS Date), 0, 0, N'carne.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (4, 3, 4, N'Pechuga de pollo', CAST(N'2025-10-21' AS Date), 0, 0, N'pollo.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (5, 4, 5, N'Manzana roja', CAST(N'2025-10-28' AS Date), 0, 0, N'manzana.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (6, 5, 6, N'Lechuga criolla', CAST(N'2025-10-18' AS Date), 0, 1, N'lechuga.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (7, 6, 7, N'Coca-Cola 1.5L', CAST(N'2026-01-01' AS Date), 0, 0, N'cocacola.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (8, 7, 8, N'Cerveza Quilmes', CAST(N'2026-06-01' AS Date), 0, 1, N'cerveza.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (9, 8, 9, N'Pan Fargo', CAST(N'2025-10-19' AS Date), 0, 0, N'pan.jpg', NULL)
INSERT [dbo].[ProductoXHeladera] ([Id], [IdHeladera], [IdProducto], [NombreEspecifico], [FechaVencimiento], [Eliminado], [Abierto], [Foto], [Estado]) VALUES (10, 9, 10, N'Queso La Paulina', CAST(N'2025-11-05' AS Date), 0, 0, N'queso.jpg', NULL)
SET IDENTITY_INSERT [dbo].[ProductoXHeladera] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductoXReceta] ON 

INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (1, 1, 2, CAST(0.50 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (2, 3, 2, CAST(0.30 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (3, 4, 5, CAST(0.40 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (4, 6, 1, CAST(0.20 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (5, 5, 9, CAST(0.25 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (6, 7, 3, CAST(0.30 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (7, 10, 4, CAST(0.50 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (8, 9, 6, CAST(0.60 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (9, 3, 7, CAST(0.35 AS Decimal(10, 2)))
INSERT [dbo].[ProductoXReceta] ([Id], [IdProducto], [IdReceta], [CantidadNecesaria]) VALUES (10, 2, 8, CAST(0.20 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[ProductoXReceta] OFF
GO
SET IDENTITY_INSERT [dbo].[Receta] ON 

INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (1, N'Ensalada César', 0, 15, NULL, NULL, N'Paso 1: Hervir dos huevos en una olla pequeña durante 10 minutos.
Paso 2: Mientras los huevos se cocinan, lavar y cortar la lechuga, el tomate y la cebolla.
Paso 3: Abrir la lata de atún y escurrir el aceite o el agua.
Paso 4: Una vez cocidos los huevos, enfriarlos, pelarlos y cortarlos en cuartos.
Paso 5: Colocar todos los ingredientes en un bol grande, mezclar y condimentar con sal, aceite y vinagre a gusto.')
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (2, N'Milanesas con puré', 1, 40, NULL, NULL, N'Paso 1: Precalentar el horno a 180°C.
Paso 2: Estirar la masa de tarta (pascualina) en una fuente para horno previamente aceitada.
Paso 3: En una sartén, dorar la cebolla y el morrón picados. Agregar el pollo (previamente hervido y desmenuzado) y cocinar por 5 minutos.
Paso 4: En un bol, mezclar el sofrito de pollo con 2 huevos batidos y 3 cucharadas de queso crema. Condimentar con sal, pimienta y orégano.
Paso 5: Volcar el relleno sobre la masa en la fuente.
Paso 6: (Opcional) Tapar con la otra masa de tarta y hacer un repulgue.
Paso 7: Hornear durante 30-40 minutos o hasta que la masa esté dorada.')
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (3, N'Smoothie de frutas', 0, 10, NULL, NULL, N'Paso 1: Pelar las papas, cortarlas en cubos y ponerlas a hervir en una olla con agua y sal.
Paso 2: Mientras hierven las papas, preparar las milanesas. Batir 2 huevos con ajo picado, perejil, sal y pimienta.
Paso 3: Pasar cada bife de carne primero por el huevo batido y luego por el pan rallado, presionando bien para que se adhiera.
Paso 4: Freír las milanesas en aceite caliente o cocinarlas en una placa para horno hasta que estén doradas de ambos lados.
Paso 5: Cuando las papas estén tiernas (pincharlas con un tenedor), colarlas. Pisarlas con un pisapapas, agregar una cucharada de manteca, un chorrito de leche caliente y sal. Batir hasta que quede cremoso.
Paso 6: Servir una milanesa con una porción de puré. ¡Ideal con un chorrito de limón sobre la milanesa!')
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (4, N'Tarta de jamón y queso', 0, 35, NULL, NULL, N'Paso 1: Poner a hervir abundante agua con sal en una olla grande para los fideos.
Paso 2: Para la salsa, picar y rehogar la cebolla y el ajo en una sartén grande con un chorrito de aceite de oliva.
Paso 3: Agregar la carne picada a la sartén y cocinar, revolviendo para separarla, hasta que cambie de color y se dore.
Paso 4: Incorporar la lata de tomate triturado. Condimentar con sal, pimienta y orégano. Bajar el fuego y dejar cocinar la salsa tapada durante al menos 20 minutos.
Paso 5: Cuando el agua hierva, echar los fideos y cocinar según el tiempo indicado en el paquete (generalmente 8-10 minutos).
Paso 6: Colar los fideos, servirlos en un plato y cubrir generosamente con la salsa bolognesa. Opcional: agregar queso rallado por encima.')
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (5, N'Pollo al horno', 0, 60, NULL, NULL, NULL)
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (6, N'Pizza casera', 1, 45, NULL, NULL, NULL)
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (7, N'Empanadas de carne', 0, 50, NULL, NULL, NULL)
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (8, N'Flan con dulce de leche', 0, 70, NULL, NULL, NULL)
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (9, N'Ensalada de frutas', 1, 10, NULL, NULL, NULL)
INSERT [dbo].[Receta] ([Id], [Nombre], [Favorito], [Duracion], [Popularidad], [Raciones], [Instrucciones]) VALUES (10, N'Sándwich de pollo', 0, 20, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Receta] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (1, N'juanperez', N'Juan1234', N'Juan', N'Pérez', N'juan.perez@gmail.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (2, N'maria.gomez', N'Maria2025', N'María', N'Gómez', N'maria.gomez@hotmail.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (3, N'carlos_rojas', N'Carlos!987', N'Carlos', N'Rojas', N'carlos.rojas@yahoo.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (4, N'lucia_flores', N'Lucia@25', N'Lucía', N'Flores', N'lucia.flores@gmail.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (5, N'martin87', N'Mart1n#87', N'Martín', N'Fernández', N'martin.fernandez@outlook.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (6, N'sofia.mendez', N'SofiMdz2025', N'Sofía', N'Méndez', N'sofia.mendez@gmail.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (7, N'agustin_ort', N'AgusOrt!', N'Agustín', N'Ramírez', N'agustin.ramirez@escuelaort.edu.ar')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (8, N'valentina', N'Valen#123', N'Valentina', N'López', N'valentina.lopez@gmail.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (9, N'admin', N'AdminKeepi', N'Administrador', N'Keepi', N'admin@keepi.com')
INSERT [dbo].[Usuario] ([Id], [Username], [Password], [Nombre], [Apellido], [Email]) VALUES (10, N'tommy', N'Keepi2025', N'Tomás', N'Goldentul', N'tomas.goldentul@ort.edu.ar')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
SET IDENTITY_INSERT [dbo].[UsuarioXHeladera] ON 

INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (1, 1, 1, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (2, 1, 2, 0)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (3, 2, 2, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (4, 3, 3, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (5, 4, 4, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (6, 5, 5, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (7, 6, 6, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (8, 7, 7, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (9, 8, 8, 1)
INSERT [dbo].[UsuarioXHeladera] ([Id], [IdUsuario], [IdHeladera], [EsDueño]) VALUES (10, 9, 9, 1)
SET IDENTITY_INSERT [dbo].[UsuarioXHeladera] OFF
GO
ALTER TABLE [dbo].[Heladera] ADD  CONSTRAINT [DF__Heladera__Elimin__6477ECF3]  DEFAULT ((0)) FOR [Eliminado]
GO
ALTER TABLE [dbo].[Producto] ADD  CONSTRAINT [DF__Producto__Favori__3F466844]  DEFAULT ((0)) FOR [Favorito]
GO
ALTER TABLE [dbo].[ProductoXHeladera] ADD  DEFAULT ((0)) FOR [Eliminado]
GO
ALTER TABLE [dbo].[ProductoXHeladera] ADD  DEFAULT ((0)) FOR [Abierto]
GO
ALTER TABLE [dbo].[Receta] ADD  DEFAULT ((0)) FOR [Favorito]
GO
ALTER TABLE [dbo].[UsuarioXHeladera] ADD  DEFAULT ((0)) FOR [EsDueño]
GO
ALTER TABLE [dbo].[Notificacion]  WITH CHECK ADD  CONSTRAINT [FK_Notificacion_ProductoXHeladera] FOREIGN KEY([IdProductoxHeladera])
REFERENCES [dbo].[ProductoXHeladera] ([Id])
GO
ALTER TABLE [dbo].[Notificacion] CHECK CONSTRAINT [FK_Notificacion_ProductoXHeladera]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Categoria] FOREIGN KEY([IdCategoria])
REFERENCES [dbo].[Categoria] ([IdCategoria])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Categoria]
GO
ALTER TABLE [dbo].[ProductoXHeladera]  WITH CHECK ADD  CONSTRAINT [FK__ProductoX__IdHel__49C3F6B7] FOREIGN KEY([IdHeladera])
REFERENCES [dbo].[Heladera] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductoXHeladera] CHECK CONSTRAINT [FK__ProductoX__IdHel__49C3F6B7]
GO
ALTER TABLE [dbo].[ProductoXHeladera]  WITH CHECK ADD  CONSTRAINT [FK__ProductoX__IdPro__44FF419A] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductoXHeladera] CHECK CONSTRAINT [FK__ProductoX__IdPro__44FF419A]
GO
ALTER TABLE [dbo].[ProductoXReceta]  WITH CHECK ADD  CONSTRAINT [FK__ProductoX__IdPro__4AB81AF0] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductoXReceta] CHECK CONSTRAINT [FK__ProductoX__IdPro__4AB81AF0]
GO
ALTER TABLE [dbo].[ProductoXReceta]  WITH CHECK ADD  CONSTRAINT [FK__ProductoX__IdRec__4CA06362] FOREIGN KEY([IdReceta])
REFERENCES [dbo].[Receta] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductoXReceta] CHECK CONSTRAINT [FK__ProductoX__IdRec__4CA06362]
GO
ALTER TABLE [dbo].[UsuarioXHeladera]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioXH__IdHel__4D94879B] FOREIGN KEY([IdHeladera])
REFERENCES [dbo].[Heladera] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuarioXHeladera] CHECK CONSTRAINT [FK__UsuarioXH__IdHel__4D94879B]
GO
ALTER TABLE [dbo].[UsuarioXHeladera]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioXH__IdUsu__4E88ABD4] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuarioXHeladera] CHECK CONSTRAINT [FK__UsuarioXH__IdUsu__4E88ABD4]
GO
ALTER TABLE [dbo].[ProductoXHeladera]  WITH CHECK ADD  CONSTRAINT [CK_ProductoXHeladera_Estado] CHECK  (([Estado]='Congelado' OR [Estado]='Refrigerado' OR [Estado]='Alacena'))
GO
ALTER TABLE [dbo].[ProductoXHeladera] CHECK CONSTRAINT [CK_ProductoXHeladera_Estado]
GO
/****** Object:  StoredProcedure [dbo].[buscarProducto]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[buscarProducto]
@Nombre VARCHAR(50)
AS
BEGIN
IF EXISTS (SELECT 1 FROM Producto WHERE Nombre = @Nombre)
    BEGIN
        SELECT * FROM Producto WHERE Nombre = @Nombre;
    END
    ELSE
    BEGIN
        SELECT
            CAST(NULL AS INT) AS Id,
            CAST(NULL AS VARCHAR(50)) AS Nombre,
            CAST(NULL AS INT) AS Duracion,
            CAST(NULL AS BIT) AS Favorito,
            CAST(NULL AS INT) AS IdCategoria,
            CAST(NULL AS DECIMAL(6,3)) AS PorcAcidez,
            CAST(NULL AS DECIMAL(6,3)) AS PorcAgua,
            CAST(NULL AS DECIMAL(6,3)) AS PorcAzucar,
            CAST(NULL AS DECIMAL(6,3)) AS PorcConservantes,
            CAST(NULL AS DECIMAL(6,3)) AS PorcAlcohol;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[CalcularFPromedioBase]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalcularFPromedioBase]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @f_promedio_base DECIMAL(10,5);

    SELECT @f_promedio_base = AVG(
        CASE 
            WHEN (0.05*productos.porcentaje_acidez 
                  + 0.05*productos.porcentaje_agua 
                  + 0.02*productos.porcentaje_azucar 
                  - 0.03*productos.porcentaje_conservantes 
                  - 0.01*productos.porcentaje_alcohol) < 0
            THEN 0
            ELSE (0.05*productos.porcentaje_acidez 
                  + 0.05*productos.porcentaje_agua 
                  + 0.02*productos.porcentaje_azucar 
                  - 0.03*productos.porcentaje_conservantes 
                  - 0.01*productos.porcentaje_alcohol)
        END
    )
    FROM productosXHeladera
    JOIN productos ON productosXHeladera.IdProducto = productos.Id
    WHERE productosXHeladera.Abierto = 1 AND productosXHeladera.Eliminado = 0;

    -- Devolver resultado
    SELECT @f_promedio_base AS f_promedio_base;
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoProducto]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CambiarEstadoProducto]
    @NombreEspecifico VARCHAR(50),
    @NuevoEstado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        --Fijarse que el estado sea uno de los permitidos
        IF @NuevoEstado NOT IN ('Alacena', 'Refrigerado', 'Congelado')
        BEGIN
            RAISERROR('Che, no podes hacer eso, solo para que sepas. El estado solo puede ser: Alacena, Refrigerado o Congelado.', 16, 1);
            RETURN;
        END;

        --El producto tiene que existir
        IF NOT EXISTS (SELECT 1 FROM ProductoXHeladera WHERE NombreEspecifico = @NombreEspecifico)
        BEGIN
            RAISERROR('El producto especificado no existe en éste universo por lo menos.', 16, 1);
            RETURN;
        END;

        -- Actualizar estado
        UPDATE ProductoXHeladera
        SET Estado = @NuevoEstado
        WHERE NombreEspecifico = @NombreEspecifico;

        PRINT CONCAT('El estado del producto "', @NombreEspecifico, '" fue actualizado a "', @NuevoEstado, '" de forma gloriosa.');
    END TRY

    BEGIN CATCH
        DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Msg, 16, 1);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[crearHeladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[crearHeladera]
@color varchar(50),
@Nombre varchar(50)
as 
begin
declare @resultado int
BEGIN TRY
BEGIN TRANSACTION
if not exists (select 1 from Heladera where Nombre = @Nombre and Eliminado = 'false')
	begin
	insert into Heladera (Color, Nombre, Eliminado)
	values (@color, @Nombre, 'false')
	set @resultado = 1
	end
else
	begin
		set @resultado = -1
	end
COMMIT TRANSACTION;
    SELECT @Resultado AS Resultado;
 
END TRY
BEGIN CATCH
if @@ROWCOUNT > 0
ROLLBACK TRANSACTION
        PRINT  @resultado;

END CATCH
end
GO
/****** Object:  StoredProcedure [dbo].[crearUsuario]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[crearUsuario]
@Username varchar(100),
@Password varchar (100)
as
begin
if exists (select 1 from Usuario where Username = @Username and Password = @Password)
begin
select '-1'
end
else
begin
insert into Usuario (Username, Password)
values (@Username, @Password)
select '1'
end
end
GO
/****** Object:  StoredProcedure [dbo].[eliminarHeladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[eliminarHeladera]
@nombre varchar(50)
as
begin
declare @resultado int
BEGIN TRY
begin transaction
if exists (select 1 from Heladera where Nombre = @nombre and Eliminado = 'false')
	begin
	update Heladera set Eliminado = 'true' where Nombre = @nombre
	set @resultado = 1
	end
else
	begin
		set @resultado = -1
	end
COMMIT TRANSACTION;
    SELECT @Resultado AS Resultado;

END TRY
BEGIN CATCH
if @@ROWCOUNT >0
rollback transaction;
print @resultado
END CATCH
end
GO
/****** Object:  StoredProcedure [dbo].[eliminarProductoHeladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[eliminarProductoHeladera]
@idHeladera int,
@idProducto int
as
begin
declare @resultado int
begin try
begin transaction
if exists (select 1 from ProductoXHeladera where IdProducto = @idProducto and IdHeladera = @idHeladera)
begin
update ProductoXHeladera set Eliminado = 1 where IdProducto = @idProducto and IdHeladera = @idHeladera
set @resultado = 1
end
else
begin
set @resultado = -1
end
commit transaction
select @resultado
end try
begin catch
if @@ROWCOUNT >0
rollback transaction;
print @resultado
end catch
end
GO
/****** Object:  StoredProcedure [dbo].[getProductosByNombreHeladeraAndIdUsuario]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getProductosByNombreHeladeraAndIdUsuario]
    @nombre VARCHAR(50),
    @IdUsuario INT
AS
BEGIN
    SELECT 
        PH.Id,
        PH.IdHeladera,
        PH.IdProducto,
        PH.NombreEspecifico,
        PH.FechaVencimiento,
        PH.Eliminado,
        PH.Abierto,
        PH.Foto,
        P.Nombre AS NombreProducto,
        UH.IdUsuario
    FROM ProductoXHeladera PH 
    INNER JOIN Heladera H ON H.Id = PH.IdHeladera 
    INNER JOIN UsuarioXHeladera UH ON H.Id = UH.IdHeladera 
    LEFT JOIN Producto P ON P.Id = PH.IdProducto
    WHERE UH.IdUsuario = @IdUsuario 
        AND H.Nombre = @nombre
        AND (PH.Eliminado = 0 OR PH.Eliminado IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[SeleccionarHeladeraByNombre]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SeleccionarHeladeraByNombre]
@IdUsuario int,
@Nombre varchar(50)
as
begin
select H.* from Heladera H
inner join UsuarioXHeladera UxH on UxH.IdHeladera = H.Id
where UxH.IdUsuario = @IdUsuario and H.Nombre = @Nombre
end
GO
/****** Object:  StoredProcedure [dbo].[sp_AgregarProductoXHeladera]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AgregarProductoXHeladera]
    -- Parámetros que coinciden con el método de C#
    @IdHeladera INT,
    @IdProducto INT,
    @NombreEsp VARCHAR(255), 
    @FechaVencimiento DATETIME,
    @Eliminado BIT,
    @Abierto BIT,
    @Foto Varchar(255) 

AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[ProductoXHeladera]
           (
            IdHeladera,
            IdProducto,
            NombreEspecifico	,
            FechaVencimiento,
            Eliminado,
            Abierto,
            Foto
           )
    VALUES
           (
            @IdHeladera,
            @IdProducto,
            @NombreEsp,
            @FechaVencimiento,
            @Eliminado,  
            @Abierto,    
            @Foto
           );
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllProductos]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllProductos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ID,
        Nombre,
        Duracion,
        IdCategoria,
        PorcAcidez,
       PorcAgua,
        PorcAzucar,
        PorcConservantes,
        PorcAlcohol,
		Favorito
    FROM Producto
     
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetHeladeraByUsuarioId]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- 4. Obtiene la heladera principal de un usuario
-- Usado en: RecetasController -> SugerirRecetas()
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetHeladeraByUsuarioId]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Asumo que un usuario puede tener varias heladeras, pero la lógica
    -- de SugerirRecetas() espera solo una. Usamos TOP 1.
    SELECT TOP 1
        H.ID,
        H.Nombre,
        H.Color
    FROM 
        Heladera H
    INNER JOIN 
        UsuarioXHeladera UXH ON H.ID = UXH.IdHeladera
    WHERE 
        UXH.IdUsuario = @IdUsuario;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProductosByHeladeraId]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetProductosByHeladeraId]
    @IdHeladera INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        PXH.*
    FROM 
        Producto P
    INNER JOIN 
        ProductoXHeladera PXH ON P.Id = PXH.IdProducto
    WHERE 
        PXH.IdHeladera = @IdHeladera
        AND PXH.Eliminado = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProductosByRecetaId]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- 3. Obtiene los productos (ingredientes) de UNA receta
-- Usado en: RecetasController -> Detalles(id)
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductosByRecetaId]
    @IdReceta INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Hacemos un JOIN entre Productos y la tabla intermedia ProductosXReceta
    SELECT 
        P.ID,
        P.Nombre -- Asumo que tu tabla Producto tiene una columna "Nombre"
    FROM 
        Producto P
    INNER JOIN 
        ProductoXReceta PXR ON P.ID = PXR.IdProducto
    WHERE 
        PXR.IdReceta = @IdReceta;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRecetaById]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Modifica el Stored Procedure existente
CREATE PROCEDURE [dbo].[sp_GetRecetaById]
    @IdReceta INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ID, 
        nombre, 
        favorito, 
        duracion, 
        Popularidad, 
        Raciones,
        Instrucciones
    FROM 
        Receta
    WHERE 
        ID = @IdReceta;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRecetas]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- 1. Obtiene TODAS las recetas
-- Usado en: HomeController -> Recetas()
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRecetas]
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Selecciona los campos que coinciden con el modelo Receta.cs
    SELECT 
        ID, 
        nombre, 
        favorito, 
        duracion, 
        Popularidad, 
        Raciones
    FROM 
        Receta;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_IncrementarPopularidadReceta]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- 7. Incrementa la popularidad de una receta
-- Usado en: RecetasController -> FinalizarReceta(id)
-- =============================================
CREATE PROCEDURE [dbo].[sp_IncrementarPopularidadReceta]
    @IdReceta INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Receta
    SET 
        Popularidad = Popularidad + 1
    WHERE 
        ID = @IdReceta;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ToggleFavoritoReceta]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ToggleFavoritoReceta]
    @IdReceta INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Receta
    -- Esta lógica (1 - favorito) cambia un 1 a 0, y un 0 a 1.
    SET favorito = 1 - favorito 
    WHERE ID = @IdReceta;
END

GO
/****** Object:  StoredProcedure [dbo].[TeoremaHevia]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TeoremaHevia]
    @Producto VARCHAR(50),
    @D0 DECIMAL(10,2),
    @Acidez DECIMAL(6,3),
    @Agua DECIMAL(6,3),
    @Azucar DECIMAL(6,3),
    @Conservantes DECIMAL(6,3),
    @Alcohol DECIMAL(6,3),
    @PorcentajeCambio DECIMAL(6,3),
    @DiasAbierto INT,
    @f_promedio_base DECIMAL(10,5)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @f_base DECIMAL(10,5);
        DECLARE @FactorCategoria DECIMAL(10,5);
        DECLARE @f_total_raw DECIMAL(10,5);
        DECLARE @f_total DECIMAL(10,5);
        DECLARE @DuracionRestante DECIMAL(10,2);
		
		--Abajo agregamos los de estado para incluir esto en la cuenta que al parecer fue pasado de largo

		Declare @EstadoProducto Varchar(50);
		Declare @FactorAmbiente Decimal(10,2);

		Set @EstadoProducto = (Select Estado From ProductoXHeladera Where NombreEspecifico = @Producto);

		If @EstadoProducto IS NULL Set @EstadoProducto = 'Alacena'; --Si no existe se pone en valor base, Alacena, ya que deja el valor exactamente igual

		SET @FactorAmbiente = 
		CASE 
			WHEN @EstadoProducto = 'Refrigerado' THEN 0.5
			WHEN @EstadoProducto = 'Congelado' THEN 0.2
			ELSE 1.0
		END;

		--La Cuenta:

        -- Paso 1: f_base
        SET @f_base = 0.05*@Acidez + 0.05*@Agua + 0.02*@Azucar - 0.03*@Conservantes - 0.01*@Alcohol;
        IF @f_base < 0 SET @f_base = 0;

        -- Paso 2: FactorCategoria
        SET @FactorCategoria = @PorcentajeCambio / @f_promedio_base;

		

        -- Paso 3: f_total
        SET @f_total_raw = @f_base * @FactorCategoria * @EstadoProducto; --Agregar Estado al coso este (Ojala no se desmorone toda la cuenta por ésta adición)
        IF @f_total_raw > 0.5
            SET @f_total = 0.5;
        ELSE
            SET @f_total = @f_total_raw;

        -- Paso 4: Duración restante
        SET @DuracionRestante = @D0 * POWER(1 - @f_total, @DiasAbierto);

        -- Resultado
        SELECT @DuracionRestante AS DuracionRestante;



    END TRY
BEGIN CATCH
    DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@Msg, 16, 1);
END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[traerNombresHeladerasById]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[traerNombresHeladerasById]
@IdUsuario int
as
begin
select H.Nombre from Heladera H 
inner join UsuarioXHeladera UxH on H.Id = UxH.IdHeladera
where UxH.IdUsuario = @IdUsuario
end
GO
/****** Object:  StoredProcedure [dbo].[traerNotificacionesNoLeidas]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[traerNotificacionesNoLeidas]
AS
BEGIN
    SELECT  N.Id, N.Mensaje, N.FechaNotificacion, N.Leida
	FROM Notificacion N
	inner join ProductoXHeladera as PXH on N.IdProductoXHeladera = PXH.Id
	inner join Producto as P on PXH.IdProducto = P.Id
	inner join Heladera as H on PXH.IdHeladera = H.Id
    WHERE N.Leida = 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[verificarUsuario]    Script Date: 14/11/2025 09:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[verificarUsuario]
@Username varchar(100),
@Password varchar(100)
as
begin

if not exists (select 1 from Usuario where Username = @Username and Password = @Password)
begin
 select null as Username;
end
else
begin
select * from Usuario where Username = @Username and Password = @Password
end
end
GO
USE [master]
GO
ALTER DATABASE [Keepi_DataBase] SET  READ_WRITE 
GO
