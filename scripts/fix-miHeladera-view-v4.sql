-- Fix stored procedure: SeleccionarHeladeraByNombre
-- This procedure needs to filter out heladeras where UsuarioXHeladera.Eliminado = 1

-- Drop the existing procedure if it exists
IF OBJECT_ID('SeleccionarHeladeraByNombre', 'P') IS NOT NULL
    DROP PROCEDURE SeleccionarHeladeraByNombre;
GO

-- Create the updated stored procedure with Eliminado filter
CREATE PROCEDURE SeleccionarHeladeraByNombre
    @IdUsuario INT,
    @Nombre NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        h.ID,
        h.Color,
        h.Nombre,
        h.Eliminado
    FROM Heladera h
    INNER JOIN UsuarioXHeladera uxh ON h.ID = uxh.IdHeladera
    WHERE uxh.IdUsuario = @IdUsuario
        AND h.Nombre = @Nombre
        AND uxh.Eliminado = 0  -- Only include active user-heladera relationships
        AND h.Eliminado = 0;   -- Only include active heladeras
END;
GO
