-- Fix stored procedure: sp_GetProductosByHeladeraId
-- CORRECCIÓN: Este SP debe devolver ProductoXHeladera correctamente
-- y filtrar por UsuarioXHeladera.Eliminado solo cuando sea necesario

-- Drop the existing procedure if it exists
IF OBJECT_ID('sp_GetProductosByHeladeraId', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetProductosByHeladeraId;
GO

-- Create the updated stored procedure with proper Eliminado filter
CREATE PROCEDURE sp_GetProductosByHeladeraId
    @IdHeladera INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        pxh.Id,
        pxh.IdHeladera,
        pxh.IdProducto,
        pxh.IdUsuario,
        pxh.NombreEspecifico,
        p.Nombre AS NombreProducto,
        pxh.FechaVencimiento,
        pxh.Eliminado,
        pxh.Abierto,
        pxh.Foto,
        pxh.TipoAlmacenamiento
    FROM ProductoXHeladera pxh
    INNER JOIN Producto p ON pxh.IdProducto = p.ID
    WHERE pxh.IdHeladera = @IdHeladera
        AND pxh.Eliminado = 0  -- Solo mostrar productos no eliminados
    ORDER BY pxh.FechaVencimiento ASC;
END;
GO
