/*
 * Migration: Add Description column to AddressTypes table
 * Date: 2025-01-11
 * Author: AI Assistant
 * Description: Adds the missing Description column to the AddressTypes table
 *              to match the entity definition in the codebase.
 */

-- Verify if the column already exists before adding it
IF NOT EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' 
    AND TABLE_NAME = 'AddressTypes' 
    AND COLUMN_NAME = 'Description'
)
BEGIN
    PRINT 'Adding Description column to AddressTypes table...';
    
    ALTER TABLE dbo.AddressTypes
    ADD Description NVARCHAR(255) NOT NULL DEFAULT N'';
    
    PRINT 'Description column added successfully.';
END
ELSE
BEGIN
    PRINT 'Description column already exists in AddressTypes table. Skipping...';
END
GO

-- Update the default constraint to allow NULL values for existing records
-- Then set a proper description for existing records
IF EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' 
    AND TABLE_NAME = 'AddressTypes' 
    AND COLUMN_NAME = 'Description'
)
BEGIN
    -- Update existing records with a default description based on their Name
    UPDATE dbo.AddressTypes
    SET Description = CONCAT(N'Tipo de endereço: ', Name)
    WHERE Description = N'' OR Description IS NULL;
    
    PRINT 'Existing AddressTypes records updated with default descriptions.';
END
GO
