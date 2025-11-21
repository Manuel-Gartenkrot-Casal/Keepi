-- Fix stored procedure: traerNombresHeladerasById
-- This procedure needs to filter out heladeras where UsuarioXHeladera.Eliminado = 1

-- Drop the existing procedure if it exists
IF OBJECT_ID('traerNombresHeladerasById', 'P') IS NOT NULL
    DROP PROCEDURE traerNombresHeladerasById;
GO

-- Create the updated stored procedure with Eliminado filter
CREATE PROCEDURE traerNombresHeladerasById
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT h.Nombre
    FROM Heladera h
    INNER JOIN UsuarioXHeladera uxh ON h.ID = uxh.IdHeladera
    WHERE uxh.IdUsuario = @IdUsuario
        AND uxh.Eliminado = 0  -- Only include active user-heladera relationships
        AND h.Eliminado = 0    -- Only include active heladeras
    ORDER BY h.Nombre;
END;
GO
