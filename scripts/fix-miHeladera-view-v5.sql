-- Fix stored procedure: BuscarHeladeras
-- This procedure needs to filter out heladeras where UsuarioXHeladera.Eliminado = 1

-- Drop the existing procedure if it exists
IF OBJECT_ID('BuscarHeladeras', 'P') IS NOT NULL
    DROP PROCEDURE BuscarHeladeras;
GO

-- Create the updated stored procedure with Eliminado filter
CREATE PROCEDURE BuscarHeladeras
    @Nombre NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- @Nombre is actually the username, not the heladera name
    -- This procedure fetches all heladeras for a specific user
    SELECT 
        h.ID,
        h.Color,
        h.Nombre,
        h.Eliminado
    FROM Heladera h
    INNER JOIN UsuarioXHeladera uxh ON h.ID = uxh.IdHeladera
    INNER JOIN Usuario u ON uxh.IdUsuario = u.ID
    WHERE u.Username = @Nombre
        AND uxh.Eliminado = 0  -- Only include active user-heladera relationships
        AND h.Eliminado = 0    -- Only include active heladeras
    ORDER BY h.Nombre;
END;
GO
