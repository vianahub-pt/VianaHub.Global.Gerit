/*
 * Script complementar para registrar a migration no histórico do EF Core
 * Execute apenas se o script principal (Fix-Users-Table-Structure.sql) foi executado com sucesso
 */

USE [GeritDb];  -- Ajuste o nome do banco se necessário
GO

-- Verifica se a tabela __EFMigrationsHistory existe
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    PRINT '? Tabela __EFMigrationsHistory encontrada';
    
    -- Verifica se a migration já foi registrada
    IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE MigrationId = '20260118061917_FixUserTableStructure')
    BEGIN
        PRINT '? Registrando migration no histórico do EF Core...';
        
        INSERT INTO [__EFMigrationsHistory] (MigrationId, ProductVersion)
        VALUES ('20260118061917_FixUserTableStructure', '8.0.0');
        
        PRINT '? Migration registrada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Migration já estava registrada no histórico';
    END
END
ELSE
BEGIN
    PRINT '? Tabela __EFMigrationsHistory não encontrada!';
    PRINT '';
    PRINT 'Possíveis causas:';
    PRINT '1. A aplicação nunca foi executada antes';
    PRINT '2. As migrations nunca foram aplicadas';
    PRINT '3. O banco de dados foi criado manualmente';
    PRINT '';
    PRINT 'Solução: Inicie a aplicação uma vez para criar a tabela de histórico';
    PRINT 'Depois execute este script novamente';
END
GO
