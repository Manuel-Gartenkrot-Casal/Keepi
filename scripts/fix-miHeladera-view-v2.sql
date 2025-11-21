-- Fix stored procedure: getProductosByNombreHeladeraAndIdUsuario
-- CORRECCIÓN: Asegurar que solo se muestren productos de relaciones activas

-- Drop the existing procedure if it exists
IF OBJECT_ID('getProductosByNombreHeladeraAndIdUsuario', 'P') IS NOT NULL
    DROP PROCEDURE getProductosByNombreHeladeraAndIdUsuario;
GO

-- Create the updated stored procedure with proper Eliminado filter
CREATE PROCEDURE getProductosByNombreHeladeraAndIdUsuario
    @nombre NVARCHAR(255),
    @IdUsuario INT
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
    INNER JOIN Heladera h ON pxh.IdHeladera = h.ID
    INNER JOIN Producto p ON pxh.IdProducto = p.ID
    INNER JOIN UsuarioXHeladera uxh ON h.ID = uxh.IdHeladera 
        AND uxh.IdUsuario = @IdUsuario
    WHERE h.Nombre = @nombre
        AND pxh.Eliminado = 0      -- Solo productos no eliminados
        AND uxh.Eliminado = 0      -- Solo si la relación usuario-heladera está activa
        AND h.Eliminado = 0        -- Solo heladeras no eliminadas
    ORDER BY pxh.FechaVencimiento ASC;
END;
GO
