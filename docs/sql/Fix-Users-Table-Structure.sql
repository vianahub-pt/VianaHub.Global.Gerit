/*
 * Script para corrigir a estrutura da tabela dbo.Users
 * Adiciona as colunas que faltam e renomeia FullName para Name
 * 
 * Execute este script no SQL Server Management Studio ou Azure Data Studio
 */

USE [GeritDb];  -- Ajuste o nome do banco se necessário
GO

BEGIN TRANSACTION;

BEGIN TRY
    PRINT '========================================';
    PRINT 'Iniciando correção da tabela dbo.Users';
    PRINT '========================================';
    PRINT '';

    -- 1. Renomeia a coluna FullName para Name
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'FullName')
    BEGIN
        PRINT '? Renomeando coluna FullName para Name...';
        EXEC sp_rename 'dbo.Users.FullName', 'Name', 'COLUMN';
        PRINT '  Coluna renomeada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna FullName não existe (já foi renomeada?)';
    END
    PRINT '';

    -- 2. Adiciona a coluna NormalizedEmail (primeiro como nullable)
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'NormalizedEmail')
    BEGIN
        PRINT '? Adicionando coluna NormalizedEmail...';
        ALTER TABLE dbo.Users ADD NormalizedEmail NVARCHAR(256) NULL;
        PRINT '  Coluna adicionada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna NormalizedEmail já existe';
    END
    PRINT '';

    -- 3. Adiciona a coluna EmailConfirmed
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'EmailConfirmed')
    BEGIN
        PRINT '? Adicionando coluna EmailConfirmed...';
        ALTER TABLE dbo.Users ADD EmailConfirmed BIT NOT NULL DEFAULT 0;
        PRINT '  Coluna adicionada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna EmailConfirmed já existe';
    END
    PRINT '';

    -- 4. Adiciona a coluna PhoneNumber
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'PhoneNumber')
    BEGIN
        PRINT '? Adicionando coluna PhoneNumber...';
        ALTER TABLE dbo.Users ADD PhoneNumber NVARCHAR(50) NULL;
        PRINT '  Coluna adicionada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna PhoneNumber já existe';
    END
    PRINT '';

    -- 5. Adiciona a coluna PhoneNumberConfirmed
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'PhoneNumberConfirmed')
    BEGIN
        PRINT '? Adicionando coluna PhoneNumberConfirmed...';
        ALTER TABLE dbo.Users ADD PhoneNumberConfirmed BIT NOT NULL DEFAULT 0;
        PRINT '  Coluna adicionada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna PhoneNumberConfirmed já existe';
    END
    PRINT '';

    -- 6. Adiciona a coluna LastAccessAt
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'LastAccessAt')
    BEGIN
        PRINT '? Adicionando coluna LastAccessAt...';
        ALTER TABLE dbo.Users ADD LastAccessAt DATETIME2 NULL;
        PRINT '  Coluna adicionada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna LastAccessAt já existe';
    END
    PRINT '';

    COMMIT TRANSACTION;
    PRINT '? Transação 1 commitada - Colunas criadas';
    PRINT '';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT '';
    PRINT '========================================';
    PRINT '? ERRO ao adicionar colunas';
    PRINT '========================================';
    PRINT '';
    PRINT 'Erro: ' + ERROR_MESSAGE();
    PRINT 'Linha: ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    PRINT '';
    
    -- Re-lança o erro
    THROW;
END CATCH;
GO

-- Segunda transação: Popula dados e ajusta constraints
BEGIN TRANSACTION;

BEGIN TRY
    PRINT '========================================';
    PRINT 'Continuando - Populando dados e ajustando constraints';
    PRINT '========================================';
    PRINT '';

    -- 7. Popula NormalizedEmail com base no Email existente
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'NormalizedEmail')
    BEGIN
        PRINT '? Populando NormalizedEmail com base no Email...';
        UPDATE dbo.Users 
        SET NormalizedEmail = UPPER(Email)
        WHERE NormalizedEmail IS NULL OR NormalizedEmail = '';
        
        DECLARE @rowsUpdated INT = @@ROWCOUNT;
        PRINT '  ' + CAST(@rowsUpdated AS NVARCHAR(10)) + ' registro(s) atualizado(s)';
    END
    PRINT '';

    -- 8. Torna a coluna NormalizedEmail NOT NULL (agora que está populada)
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'NormalizedEmail' AND is_nullable = 1)
    BEGIN
        PRINT '? Tornando coluna NormalizedEmail obrigatória...';
        ALTER TABLE dbo.Users ALTER COLUMN NormalizedEmail NVARCHAR(256) NOT NULL;
        PRINT '  Coluna alterada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Coluna NormalizedEmail já é obrigatória';
    END
    PRINT '';

    -- 9. Remove o índice antigo IX_Users_Login
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'IX_Users_Login')
    BEGIN
        PRINT '? Removendo índice antigo IX_Users_Login...';
        DROP INDEX IX_Users_Login ON dbo.Users;
        PRINT '  Índice removido com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Índice IX_Users_Login não existe';
    END
    PRINT '';

    -- 10. Remove a constraint antiga UQ_Users_Tenant_Email
    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE object_id = OBJECT_ID('dbo.UQ_Users_Tenant_Email'))
       OR EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'UQ_Users_Tenant_Email')
    BEGIN
        PRINT '? Removendo constraint antiga UQ_Users_Tenant_Email...';
        
        -- Tenta remover como constraint
        IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE object_id = OBJECT_ID('dbo.UQ_Users_Tenant_Email'))
        BEGIN
            ALTER TABLE dbo.Users DROP CONSTRAINT UQ_Users_Tenant_Email;
        END
        
        -- Tenta remover como índice único
        IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'UQ_Users_Tenant_Email')
        BEGIN
            DROP INDEX UQ_Users_Tenant_Email ON dbo.Users;
        END
        
        PRINT '  Constraint removida com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Constraint UQ_Users_Tenant_Email não existe';
    END
    PRINT '';

    -- 11. Recria o índice IX_Users_Login com as colunas corretas
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'IX_Users_Login')
    BEGIN
        PRINT '? Recriando índice IX_Users_Login...';
        CREATE NONCLUSTERED INDEX IX_Users_Login 
        ON dbo.Users (TenantId, Email)
        INCLUDE (Id, IsActive)
        WHERE IsDeleted = 0;
        PRINT '  Índice recriado com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Índice IX_Users_Login já existe';
    END
    PRINT '';

    -- 12. Recria a constraint UQ_Users_Tenant_Email
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE object_id = OBJECT_ID('dbo.UQ_Users_Tenant_Email'))
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'UQ_Users_Tenant_Email')
    BEGIN
        PRINT '? Recriando constraint UQ_Users_Tenant_Email...';
        ALTER TABLE dbo.Users 
        ADD CONSTRAINT UQ_Users_Tenant_Email UNIQUE (TenantId, Email);
        PRINT '  Constraint recriada com sucesso!';
    END
    ELSE
    BEGIN
        PRINT '? Constraint UQ_Users_Tenant_Email já existe';
    END
    PRINT '';

    -- 13. Atualiza a tabela de migrations do EF Core
    DECLARE @migrationTableExists BIT = 0;
    
    -- Verifica se a tabela __EFMigrationsHistory existe
    IF EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
    BEGIN
        SET @migrationTableExists = 1;
    END
    
    IF @migrationTableExists = 1
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE MigrationId = '20260118061917_FixUserTableStructure')
        BEGIN
            PRINT '? Registrando migration no histórico do EF Core...';
            INSERT INTO [__EFMigrationsHistory] (MigrationId, ProductVersion)
            VALUES ('20260118061917_FixUserTableStructure', '8.0.0');
            PRINT '  Migration registrada com sucesso!';
        END
        ELSE
        BEGIN
            PRINT '? Migration já registrada no histórico';
        END
    END
    ELSE
    BEGIN
        PRINT '? Tabela __EFMigrationsHistory não encontrada';
        PRINT '  A migration será aplicada automaticamente quando a aplicação iniciar';
    END
    PRINT '';

    COMMIT TRANSACTION;

    PRINT '';
    PRINT '========================================';
    PRINT '? SUCESSO! Tabela dbo.Users corrigida!';
    PRINT '========================================';
    PRINT '';
    PRINT 'Estrutura final da tabela:';
    PRINT '  - Id (INT, PK, IDENTITY)';
    PRINT '  - TenantId (INT, FK)';
    PRINT '  - Name (NVARCHAR(150))  ? Renomeada';
    PRINT '  - Email (NVARCHAR(256))';
    PRINT '  - NormalizedEmail (NVARCHAR(256))  ? Nova';
    PRINT '  - EmailConfirmed (BIT)  ? Nova';
    PRINT '  - PhoneNumber (NVARCHAR(50))  ? Nova';
    PRINT '  - PhoneNumberConfirmed (BIT)  ? Nova';
    PRINT '  - LastAccessAt (DATETIME2)  ? Nova';
    PRINT '  - PasswordHash (VARBINARY(64))';
    PRINT '  - IsActive (BIT)';
    PRINT '  - IsDeleted (BIT)';
    PRINT '  - CreatedBy (INT)';
    PRINT '  - CreatedAt (DATETIME2)';
    PRINT '  - ModifiedBy (INT)';
    PRINT '  - ModifiedAt (DATETIME2)';
    PRINT '';
    PRINT 'Você já pode reiniciar a aplicação!'

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT '';
    PRINT '========================================';
    PRINT '? ERRO ao ajustar constraints';
    PRINT '========================================';
    PRINT '';
    PRINT 'Erro: ' + ERROR_MESSAGE();
    PRINT 'Linha: ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    PRINT '';
    PRINT 'A transação foi revertida.';
    
    -- Re-lança o erro
    THROW;
END CATCH;
GO