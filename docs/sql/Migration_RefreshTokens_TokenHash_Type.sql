-- Migration: Corrigir tipo da coluna TokenHash na tabela RefreshTokens
-- Motivo: A aplicação armazena tokens como strings Base64, não como binário
-- Data: 2026-07-18

-- 1. Remover índice único que referencia a coluna (precisa recriar depois)
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_RefreshTokens_TokenHash' AND object_id = OBJECT_ID('dbo.RefreshTokens'))
    DROP INDEX UX_RefreshTokens_TokenHash ON dbo.RefreshTokens;
GO

-- 2. Alterar tipo da coluna
ALTER TABLE dbo.RefreshTokens
    ALTER COLUMN TokenHash NVARCHAR(200) NOT NULL;
GO

-- 3. Recriar índice único
CREATE UNIQUE INDEX UX_RefreshTokens_TokenHash ON dbo.RefreshTokens(TokenHash);
GO
