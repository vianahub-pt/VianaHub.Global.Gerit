-- ===================================================================
-- Migration: Fix Nationality column type from CHAR(2) to NVARCHAR(50)
-- 
-- Motivo: O campo Nationality estava definido como CHAR(2), causando
--         erro 2628 (String or binary data would be truncated) quando
--         o valor ultrapassava 2 caracteres.
-- 
-- Antes: Nationality CHAR(2) NULL
-- Depois: Nationality NVARCHAR(50) NULL
-- ===================================================================

-- Verificar se a coluna ainda é CHAR(2) antes de alterar
IF EXISTS (
    SELECT 1 
    FROM sys.columns c
    JOIN sys.tables t ON c.object_id = t.object_id
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'ClientIndividuals'
      AND s.name = 'dbo'
      AND c.name = 'Nationality'
      AND (c.system_type_id = 175) -- 175 = CHAR (binário)
)
BEGIN
    PRINT 'Alterando coluna Nationality de CHAR(2) para NVARCHAR(50)...';
    
    ALTER TABLE dbo.ClientIndividuals
    ALTER COLUMN Nationality NVARCHAR(50) NULL;
    
    PRINT 'Coluna Nationality alterada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Coluna Nationality já está no formato correto (NVARCHAR) ou não existe. Nenhuma alteração necessária.';
END
GO
