/* =========================
   CORE MULTI-TENANT TABLES
   ========================= */

CREATE TABLE dbo.Plans (                                                            -- Catálogo global de planos (licenciamento)
    Id                          INT IDENTITY(1,1)   NOT NULL,                       -- PK interna
    Name                        NVARCHAR(100)       NOT NULL,                       -- Nome do plano (UI)
    Description                 NVARCHAR(500)           NULL,                       -- Descrição do plano
    PricePerHour                DECIMAL(10,2)           NULL,                       -- Preço por hora
    PricePerDay                 DECIMAL(10,2)           NULL,                       -- Preço por dia
    PricePerMonth               DECIMAL(10,2)           NULL,                       -- Preço por mês
    PricePerYear                DECIMAL(10,2)           NULL,                       -- Preço por ano
    Currency                    NVARCHAR(3)         NOT NULL DEFAULT N'USD',        -- ISO currency code
    MaxUsers                    INT                 NOT NULL,                       -- Limite de usuários
    MaxPhotosPerInterventions   INT			        NOT NULL,                       -- Limite de fotos por intervenção
    IsActive	                BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted	                BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy	                INT         		NOT NULL,						-- Usuário criador
    CreatedAt	                DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	                INT         		NULL,							-- Usuário modificador
    ModifiedAt	                DATETIME2			NULL,							-- Data de modificação
    CONSTRAINT PK_Plans PRIMARY KEY CLUSTERED (Id),                                 -- PK
    CONSTRAINT CK_Plans_DeletedImpliesInactive CHECK (IsDeleted = 0 OR IsActive = 0)-- Soft delete -> não ativo
);
GO

CREATE TABLE dbo.Tenants (											-- Tabela principal de tenants
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Identificador único do tenant, chave primária
    LegalName	NVARCHAR(200)		NOT NULL,						-- Razão social
    TradeName	NVARCHAR(200)		NULL,							-- Nome comercial
	Consent		BIT					NOT NULL DEFAULT 1,				-- Consentimento LGPD
    IsActive	BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		NULL,							-- Usuário modificador
    ModifiedAt	DATETIME2			NULL,							-- Data de modificação
	CONSTRAINT PK_Tenants PRIMARY KEY CLUSTERED (Id)
);
GO
CREATE TABLE dbo.TenantContacts (									-- Contatos do tenant
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Identificador do contato, chave primária
    TenantId	INT					NOT NULL,						-- Tenant dono do contato
    Name		NVARCHAR(150)		NOT NULL,						-- Nome do contato
    Email		NVARCHAR(255)		NOT NULL,						-- Email do contato
    Phone		NVARCHAR(30)			NULL,						-- Telefone
    IsPrimary	BIT					NOT NULL DEFAULT 0,				-- Contato principal
    IsActive	BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		    NULL,						-- Usuário modificador
    ModifiedAt	DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_TenantContacts PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_TenantContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
        
);
GO
CREATE TABLE dbo.TenantAddresses (									-- Endereços do tenant
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Identificador do endereço, chave primária
    TenantId	INT					NOT NULL,						-- Tenant dono do endereço
    Street		NVARCHAR(200)		NOT NULL,						-- Rua
    City		NVARCHAR(100)		NOT NULL,						-- Cidade
    PostalCode	NVARCHAR(20)		NOT NULL,						-- Código postal
    District	NVARCHAR(100)			NULL,						-- Distrito
    CountryCode CHAR(2)				NOT NULL DEFAULT 'PT',			-- País
    IsPrimary	BIT					NOT NULL DEFAULT 0,				-- Endereço principal
    IsActive	BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		    NULL,						-- Usuário modificador
    ModifiedAt	DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_TenantAddresses PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_TenantAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
        
);
GO
CREATE TABLE dbo.TenantFiscalData (										-- Dados fiscais do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador fiscal, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono
    NIF				CHAR(9)				NOT NULL,						-- NIF português
    VATNumber		NVARCHAR(20)		NOT NULL,						-- VAT calculado
    CAE				NVARCHAR(10)			NULL,						-- Código CAE
    FiscalCountry	CHAR(2)				NOT NULL DEFAULT 'PT',			-- País fiscal
    IsVATRegistered BIT					NOT NULL DEFAULT 1,				-- Regime IVA
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_TenantFiscalData PRIMARY KEY CLUSTERED (Id),			
    CONSTRAINT UQ_TenantFiscalData_NIF UNIQUE (NIF),					-- NIF único
    CONSTRAINT FK_TenantFiscalData_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)		-- FK para tenant    
);
GO
CREATE TABLE dbo.Subscriptions (                                                -- Assinatura do tenant (contrato de billing)
    Id                      INT IDENTITY(1,1)   NOT NULL,                       -- Identificador da assinatura, chave primária
    TenantId                INT                 NOT NULL,                       -- FK para tenant
    PlanId                  INT                 NOT NULL,                       -- FK para plano global
    StripeId                NVARCHAR(100)           NULL,                       -- Id do subscription no Stripe
    CurrentPeriodStart      DATETIME2           NOT NULL,                       -- Início do período faturado
    CurrentPeriodEnd        DATETIME2           NOT NULL,                       -- Fim do período faturado
    TrialStart              DATETIME2               NULL,                       -- Trial
    TrialEnd                DATETIME2               NULL,                       -- Trial
    CancelAtPeriodEnd       BIT                 NOT NULL DEFAULT 0,             -- Cancelar no fim do ciclo
    CanceledAt              DATETIME2               NULL,                       -- Quando cancelou
    CancellationReason      NVARCHAR(500)           NULL,                       -- Motivo
    StripeCustomerId        NVARCHAR(100)           NULL,                       -- Customer id
    IsActive		        BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		        BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		        INT         		NOT NULL,						-- Usuário criador
    CreatedAt		        DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	            INT         		    NULL,						-- Usuário modificador
    ModifiedAt		        DATETIME2				NULL,						-- Data de modificação
    CONSTRAINT PK_Subscriptions PRIMARY KEY CLUSTERED (Id),                     -- PK
    CONSTRAINT FK_Subscriptions_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),       -- Tenant-safe
    CONSTRAINT FK_Subscriptions_Plan FOREIGN KEY (PlanId) REFERENCES dbo.Plans(Id),             -- Global plan
    CONSTRAINT AK_Subscriptions_TenantId_Id UNIQUE (TenantId, Id),                              -- Chave alternativa (TenantId, Id) p/ FKs compostas
    CONSTRAINT CK_Subscriptions_DeletedImpliesInactive CHECK (IsDeleted = 0 OR IsActive = 0)    -- Se está deletado, não pode estar ativo
        
);


CREATE TABLE dbo.Users (                                                -- Usuários do sistema
    Id				        INT IDENTITY(1,1)	NOT NULL,                       -- Identificador do usuário, chave primária
    TenantId		        INT					NOT NULL,                       -- Tenant do usuário
    Name		            NVARCHAR(150)		NOT NULL,                       -- Nome completo
    Email                   NVARCHAR(256)       NOT NULL,                       -- Email original
    NormalizedEmail         NVARCHAR(256)       NOT NULL,                       -- Email normalizado (case-insensitive)
    EmailConfirmed          BIT                 NOT NULL DEFAULT 0,             -- Confirmação de email
    PhoneNumber             NVARCHAR(50)            NULL,                       -- Telefone (opcional)
    PhoneNumberConfirmed    BIT                 NOT NULL DEFAULT 0,             -- Confirmação de telefone
    LastAccessAt            DATETIME2               NULL,                       -- Último login/acesso
    PasswordHash	        NVARCHAR(500)		NOT NULL,						-- Hash da senha
    IsActive		        BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		        BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		        INT         		NOT NULL,						-- Usuário criador
    CreatedAt		        DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	            INT         		    NULL,						-- Usuário modificador
    ModifiedAt		        DATETIME2				NULL,                       -- Data de modificação
	CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Users_Tenant_Email UNIQUE (TenantId, NormalizedEmail),			-- Email único por tenant
    CONSTRAINT FK_Users_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant    
);
GO
/* =========================
   RBAC STRUCTURE
   ========================= */
CREATE TABLE dbo.Roles (												-- Roles por tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador da role, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono da role
    Name			NVARCHAR(100)		NOT NULL,						-- Nome da role
    Description     NVARCHAR(255)		NOT NULL,						-- Descrição da role
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,                       -- Data de modificação
	CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (Id),						
    CONSTRAINT UQ_Roles_Tenant_Name UNIQUE (TenantId, Name),			-- Role única por tenant
    CONSTRAINT FK_Roles_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.Resources (											-- Recursos do sistema
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador do recurso, chave primária
    Name			NVARCHAR(100)		NOT NULL UNIQUE,				-- Nome único do recurso
    Description     NVARCHAR(255)		NOT NULL,						-- Descrição do recurso
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_Resources PRIMARY KEY CLUSTERED (Id),					
	CONSTRAINT UQ_Resources_Name UNIQUE (Name)							-- Recursos únicos
);
GO
CREATE TABLE dbo.Actions (												-- Ações possíveis
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador da ação, chave primária
    Name			NVARCHAR(50)		NOT NULL,                       -- Nome da ação
    Description     NVARCHAR(255)		NOT NULL,                       -- Descrição da ação
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,                       -- Data de modificação
	CONSTRAINT PK_Actions PRIMARY KEY CLUSTERED (Id),					
	CONSTRAINT UQ_Actions_Name UNIQUE (Name)							-- Açoes única
);
GO
CREATE TABLE dbo.RolePermissions (																	-- Permissões por role
    Id				INT IDENTITY(1,1)	NOT NULL,													-- Identificador da permissão, chave primária
    TenantId		INT					NOT NULL,													-- Tenant dono
    RoleId			INT					NOT NULL,													-- Role associada
    ResourceId		INT					NOT NULL,													-- Recurso
    ActionId		INT					NOT NULL,													-- Ação
	CONSTRAINT PK_RolePermissions PRIMARY KEY CLUSTERED (Id),					
    CONSTRAINT UQ_RolePermissions UNIQUE (TenantId, RoleId, ResourceId, ActionId),					-- Permissão única
    CONSTRAINT FK_RolePermissions_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),			-- FK tenant
    CONSTRAINT FK_RolePermissions_Role FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id),				-- FK role
    CONSTRAINT FK_RolePermissions_Resource FOREIGN KEY (ResourceId) REFERENCES dbo.Resources(Id),	-- FK resource
    CONSTRAINT FK_RolePermissions_Action FOREIGN KEY (ActionId) REFERENCES dbo.Actions(Id)			-- FK action        
);
GO
CREATE TABLE dbo.UserRoles (															-- Relação usuário x role
    Id				INT IDENTITY(1,1)	NOT NULL,										-- Identificador, 
    TenantId		INT					NOT NULL,										-- Tenant dono
    UserId			INT					NOT NULL,										-- Usuário
    RoleId			INT					NOT NULL,										-- Role
	CONSTRAINT PK_UserRoles PRIMARY KEY CLUSTERED (Id),					
    CONSTRAINT UQ_UserRoles UNIQUE (TenantId, UserId, RoleId),							-- Relação única
    CONSTRAINT FK_UserRoles_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK tenant
    CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId) REFERENCES dbo.Users(Id),			-- FK usuário
    CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id)			-- FK role
        
);
GO

CREATE TABLE dbo.JwtKeys (
    Id 						INT IDENTITY(1,1)	NOT NULL,
    TenantId 				INT					NOT NULL,
    ApplicationId 			INT					NOT NULL,
    KeyId 					UNIQUEIDENTIFIER	NOT NULL,
    Code 					NVARCHAR(100) 		NOT NULL,
    PublicKey 				NVARCHAR(MAX) 		NOT NULL,
    PrivateKeyEncrypted 	NVARCHAR(MAX) 		NOT NULL,
    Algorithm 				NVARCHAR(50) 		NOT NULL DEFAULT 'RS256',
    KeySize 				INT 				NOT NULL DEFAULT 2048,
    KeyType 				NVARCHAR(50) 		NOT NULL DEFAULT 'RSA',
    RevokedReason 			NVARCHAR(500) 			NULL,
    UsageCount 				BIGINT 				NOT NULL DEFAULT 0,
    ActivatedAt 			DATETIME2 				NULL,
    ExpiresAt 				DATETIME2 			NOT NULL,
    LastUsedAt 				DATETIME2 				NULL,
    NextRotationAt 			DATETIME2 			NOT NULL,
    RevokedAt 				DATETIME2 				NULL,
    LastValidatedAt 		DATETIME2 				NULL,
    ValidationCount 		BIGINT 				NOT NULL DEFAULT 0,
    RotationPolicyDays 		INT 				NOT NULL DEFAULT 90,
    OverlapPeriodDays 		INT 				NOT NULL DEFAULT 7,
    MaxTokenLifetimeMinutes INT 				NOT NULL DEFAULT 60,
    Status 					INT 				NOT NULL DEFAULT 1,
    IsActive 				BIT 				NOT NULL DEFAULT 0,
    IsDeleted 				BIT 				NOT NULL DEFAULT 0,
    CreatedBy		        INT		            NOT NULL,
    CreatedAt		        DATETIME2			NOT NULL DEFAULT SYSDATETIME(),
    ModifiedBy	            INT         		    NULL,
    ModifiedAt		        DATETIME2				NULL,
    CONSTRAINT PK_JwtKeys PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_JwtKeys_Code UNIQUE (Code),
    CONSTRAINT UQ_JwtKeys_KeyId UNIQUE (KeyId),
    CONSTRAINT FK_JwtKeys_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
    CONSTRAINT CK_JwtKeys_RotationPolicy CHECK (RotationPolicyDays BETWEEN 30 AND 365),
    CONSTRAINT CK_JwtKeys_OverlapPeriod CHECK (OverlapPeriodDays BETWEEN 1 AND 30),
    CONSTRAINT CK_JwtKeys_MaxTokenLifetime CHECK (MaxTokenLifetimeMinutes BETWEEN 5 AND 1440)
);
GO

/* =========================
   DOMAIN TABLE EXAMPLE
   (All domain tables MUST have TenantId)
   ========================= */

CREATE TABLE dbo.Clients (
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Clientes do tenant
    TenantId		INT					NOT NULL,						-- Tenant dono
    Name			NVARCHAR(150)		NOT NULL,						-- Nome do cliente
    Email			NVARCHAR(255)			NULL,						-- Email do cliente
    Phone			NVARCHAR(50)		NOT NULL,						-- Phone do cliente
    Consent			BIT					NOT NULL DEFAULT 1,				-- Consentimento LGPD
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,                       -- Data de modificação
	CONSTRAINT PK_Clients PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Clients_Tenant_Name UNIQUE (TenantId, Name),			-- Nome único por tenant
    CONSTRAINT FK_Clients_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant    
);
GO
CREATE TABLE dbo.ClientContacts (															-- Contatos do client
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Identificador do contato, chave primária
    TenantId	INT					NOT NULL,						-- Tenant dono do contato
	ClientId	INT					NOT NULL,						-- Client dono do contato
    Name		NVARCHAR(150)		NOT NULL,						-- Nome do contato
    Email		NVARCHAR(255)		NOT NULL,						-- Email do contato
    Phone		NVARCHAR(30)			NULL,						-- Telefone
    IsPrimary	BIT					NOT NULL DEFAULT 0,				-- Contato principal
    IsActive	BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		    NULL,						-- Usuário modificador
    ModifiedAt	DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_ClientContacts PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_ClientContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_ClientContacts_Client FOREIGN KEY (ClientId) REFERENCES dbo.Clients(Id)	-- FK para client
);
GO
CREATE TABLE dbo.ClientAddresses (															-- Endereços do client
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Identificador do endereço, chave primária
    TenantId	INT					NOT NULL,						-- Tenant dono do endereço
	ClientId	INT					NOT NULL,						-- Client dono do endereço
    Street		NVARCHAR(200)		NOT NULL,						-- Rua
    City		NVARCHAR(100)		NOT NULL,						-- Cidade
    PostalCode	NVARCHAR(20)		NOT NULL,						-- Código postal
    District	NVARCHAR(100)			NULL,						-- Distrito
    CountryCode CHAR(2)				NOT NULL DEFAULT 'PT',			-- País
    IsPrimary	BIT					NOT NULL DEFAULT 0,				-- Endereço principal
    IsActive	BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		    NULL,						-- Usuário modificador
    ModifiedAt	DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_ClientAddresses PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_ClientAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_ClientAddresses_Client FOREIGN KEY (ClientId) REFERENCES dbo.Clients(Id)	-- FK para client     
);
GO
CREATE TABLE dbo.ClientFiscalData (										-- Dados fiscais do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador fiscal, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono
	ClientId		INT					NOT NULL,						-- Tenant dono
    NIF				CHAR(9)				NOT NULL,						-- NIF português
    VATNumber		NVARCHAR(20)		NOT NULL,						-- VAT calculado
    CAE				NVARCHAR(10)			NULL,						-- Código CAE
    FiscalCountry	CHAR(2)				NOT NULL DEFAULT 'PT',			-- País fiscal
    IsVATRegistered BIT					NOT NULL DEFAULT 1,				-- Regime IVA
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_ClientFiscalData PRIMARY KEY CLUSTERED (Id),			
    CONSTRAINT UQ_ClientFiscalData_NIF UNIQUE (NIF),					-- NIF único
    CONSTRAINT FK_ClientFiscalData_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),		-- FK para tenant    
	CONSTRAINT FK_ClientFiscalData_Client FOREIGN KEY (ClientId) REFERENCES dbo.Clients(Id)
);
GO
CREATE TABLE dbo.TeamMembers (
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Membros do time
    TenantId	INT					NOT NULL,						-- Tenant dono do endereço
    Name		NVARCHAR(150)		NOT NULL,						-- Nome do membro do time
	TaxNumber	NVARCHAR(20)			NULL,						-- Numero fiscal do membro do time
    IsActive	BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		    NULL,						-- Usuário modificador
    ModifiedAt	DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_TeamMembers PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_TeamMembers_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)
);
GO
CREATE TABLE dbo.TeamMemberContacts (									-- Contatos do TeamMember
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador do contato, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono do contato
	TeamMemberId	INT					NOT NULL,						-- Client dono do contato
    Name			NVARCHAR(150)		NOT NULL,						-- Nome do contato
    Email			NVARCHAR(255)		NOT NULL,						-- Email do contato
    Phone			NVARCHAR(30)			NULL,						-- Telefone
    IsPrimary		BIT					NOT NULL DEFAULT 0,				-- Contato principal
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_TeamMemberContacts PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_TeamMemberContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),				-- FK para tenant
    CONSTRAINT FK_TeamMemberContacts_TeamMember FOREIGN KEY (TeamMemberId) REFERENCES dbo.TeamMembers(Id)	-- FK para client
);
GO
CREATE TABLE dbo.TeamMemberAddresses (									-- Endereços do TeamMember
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador do endereço, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono
	TeamMemberId	INT					NOT NULL,						-- Endereço do membro do time
    Street			NVARCHAR(200)		NOT NULL,						-- Rua
    City			NVARCHAR(100)		NOT NULL,						-- Cidade
    PostalCode		NVARCHAR(20)		NOT NULL,						-- Código postal
    District		NVARCHAR(100)			NULL,						-- Distrito
    CountryCode		CHAR(2)				NOT NULL DEFAULT 'PT',			-- País
    IsPrimary		BIT					NOT NULL DEFAULT 0,				-- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_TeamMemberAddresses PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_TeamMemberAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),				-- FK para tenant
    CONSTRAINT FK_TeamMemberAddresses_TeamMember FOREIGN KEY (TeamMemberId) REFERENCES dbo.TeamMembers(Id)	-- FK para client     
);
GO
CREATE TABLE dbo.Equipments (
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador, chave primária
	TenantId		INT					NOT NULL,						-- Tenant dono
    Name			NVARCHAR(150)		NOT NULL,						-- Nome do equipamento
    SerialNumber	NVARCHAR(100)			NULL,						-- Número de série do equipamento
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_Equipments PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Equipments_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.Vehicles (
    Id				INT IDENTITY(1,1)	NOT NULL,
    TenantId        INT					NOT NULL,
    Plate           NVARCHAR(20)		NOT NULL,
    Model           NVARCHAR(100)			NULL,
    IsActive		BIT					NOT NULL DEFAULT 1,					-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,					-- Soft delete
    CreatedBy		INT         		NOT NULL,							-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),		-- Data de criação
    ModifiedBy	    INT         		    NULL,							-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,							-- Data de modificação
	CONSTRAINT PK_Vehicles PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Vehicles_Tenant_Plate UNIQUE (TenantId, Plate),			-- Role única por tenant
	CONSTRAINT FK_Vehicles_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)
);
GO
CREATE TABLE dbo.Interventions (
    Id				INT IDENTITY(1,1)	NOT NULL,
	TenantId        INT					NOT NULL,
    ClientId		INT					NOT NULL,
	TeamMemberId	INT					NOT NULL,															-- Endereço do membro do time
    VehicleId		INT					NOT NULL,
	Title			NVARCHAR(200)		NOT NULL,
    Description		NVARCHAR(2000)		NOT NULL,
    StartDateTime	DATETIME2			NOT NULL,
    EndDateTime		DATETIME2				NULL,
    EstimatedValue	DECIMAL(10,2)		NOT NULL CHECK (EstimatedValue >= 0),
	RealValue		DECIMAL(10,2)			NULL,
    Status			TINYINT				NOT NULL,
    IsActive		BIT					NOT NULL DEFAULT 1,								-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,								-- Soft delete
    CreatedBy		INT         		NOT NULL,										-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),					-- Data de criação
    ModifiedBy	    INT         		    NULL,										-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,										-- Data de modificação
	CONSTRAINT PK_Interventions PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK_Interventions_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
	CONSTRAINT FK_Interventions_Clients FOREIGN KEY (ClientId) REFERENCES dbo.Clients(Id),
	CONSTRAINT FK_Interventions_TeamMembers FOREIGN KEY (TeamMemberId) REFERENCES dbo.TeamMembers(Id),
	CONSTRAINT FK_Interventions_Vehicles FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles(Id),
	CONSTRAINT CK_Interventions_EndDateTime	CHECK ( EndDateTime IS NULL OR EndDateTime >= StartDateTime)

);
GO

CREATE TABLE dbo.InterventionContacts (									-- Contatos do TeamMember
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador do contato, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono do contato
	InterventionId	INT					NOT NULL,						-- Client dono do contato
    Name			NVARCHAR(150)		NOT NULL,						-- Nome do contato
    Email			NVARCHAR(255)		NOT NULL,						-- Email do contato
    Phone			NVARCHAR(30)			NULL,						-- Telefone
    IsPrimary		BIT					NOT NULL DEFAULT 0,				-- Contato principal
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_InterventionContacts PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_InterventionContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),				-- FK para tenant
    CONSTRAINT FK_InterventionContacts_Intervention FOREIGN KEY (InterventionId) REFERENCES dbo.Interventions(Id)	-- FK para client
);
GO
CREATE TABLE dbo.InterventionAddresses (								-- Endereços do TeamMember
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador do endereço, chave primária
    TenantId		INT					NOT NULL,						-- Tenant dono
	InterventionId	INT					NOT NULL,						-- Endereço do membro do time
    Street			NVARCHAR(200)		NOT NULL,						-- Rua
    City			NVARCHAR(100)		NOT NULL,						-- Cidade
    PostalCode		NVARCHAR(20)		NOT NULL,						-- Código postal
    District		NVARCHAR(100)			NULL,						-- Distrito
    CountryCode		CHAR(2)				NOT NULL DEFAULT 'PT',			-- País
    IsPrimary		BIT					NOT NULL DEFAULT 0,				-- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2			NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2				NULL,						-- Data de modificação
	CONSTRAINT PK_InterventionAddresses PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_InterventionAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),					-- FK para tenant
    CONSTRAINT FK_InterventionAddresses_Intervention FOREIGN KEY (InterventionId) REFERENCES dbo.Interventions(Id)	-- FK para client     
);
GO

CREATE UNIQUE INDEX UX_Clients_Tenant_Name_Active					ON dbo.Clients (TenantId, Name) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_ClientAddresses_Primary						ON dbo.ClientAddresses (ClientId) WHERE IsPrimary = 1 AND IsDeleted = 0;

CREATE NONCLUSTERED INDEX IX_TenantContacts_TenantId				ON dbo.TenantContacts (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Clients_Tenant_Active					ON dbo.Clients (TenantId, Name) INCLUDE (Email, Phone) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientContacts_ClientId				ON dbo.ClientContacts (ClientId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientAddresses_ClientId				ON dbo.ClientAddresses (ClientId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientFiscalData_ClientId				ON dbo.ClientFiscalData (ClientId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMemberContacts_TeamMemberId		ON dbo.TeamMemberContacts (TeamMemberId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMemberAddresses_TeamMemberId		ON dbo.TeamMemberAddresses (TeamMemberId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Interventions_Tenant_Date				ON dbo.Interventions (TenantId, StartDateTime) INCLUDE (ClientId, TeamMemberId, Status) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionContacts_InterventionId	ON dbo.InterventionContacts (InterventionId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionAddresses_InterventionId	ON dbo.InterventionAddresses (InterventionId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_UserRoles_UserId						ON dbo.UserRoles (UserId) INCLUDE (TenantId, RoleId); 
CREATE NONCLUSTERED INDEX IX_UserRoles_RoleId						ON dbo.UserRoles (RoleId) INCLUDE (TenantId, UserId); 
CREATE NONCLUSTERED INDEX IX_RolePermissions_RoleId					ON dbo.RolePermissions (RoleId) INCLUDE (TenantId, ResourceId, ActionId); 
CREATE NONCLUSTERED INDEX IX_RolePermissions_ResourceId				ON dbo.RolePermissions (ResourceId) INCLUDE (TenantId, RoleId, ActionId);
CREATE NONCLUSTERED INDEX IX_RolePermissions_ActionId				ON dbo.RolePermissions (ActionId) INCLUDE (TenantId, RoleId, ResourceId);
CREATE NONCLUSTERED INDEX IX_Users_Login				ON dbo.Users (TenantId, Email) INCLUDE (Id, IsActive) WHERE IsDeleted = 0;

GO

/* =========================
   ROW LEVEL SECURITY
   ========================= */

-- Função de isolamento multi-tenant com suporte a SuperAdmin
CREATE FUNCTION dbo.fn_TenantAccessPredicate (
    @TenantId INT
)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS fn_access
    WHERE 
        -- SuperAdmin tem acesso a tudo (bypass RLS)
        CAST(SESSION_CONTEXT(N'IsSuperAdmin') AS INT) = 1
        OR
        -- Tenant ID deve corresponder
        -- SESSION_CONTEXT retorna VARBINARY, então convertemos de volta para INT
        (
            SESSION_CONTEXT(N'TenantId') IS NOT NULL
            AND @TenantId = CONVERT(INT, SESSION_CONTEXT(N'TenantId'))
        );
GO

CREATE SECURITY POLICY dbo.TenantSecurityPolicy												-- Criação da policy de RLS
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users,					-- Filtro por TenantId, aplica RLS em Users
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles,					-- Aplica RLS em Roles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles,				-- Aplica RLS em UserRoles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RolePermissions,			-- Aplica RLS em RolePermissions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.JwtKeys,					-- Aplica RLS em JwtKeys
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Subscriptions,			-- Aplica RLS em Subscriptions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantContacts,			-- Aplica RLS em TenantContacts
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantAddresses,			-- Aplica RLS em TenantAddresses
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantFiscalData,		-- Aplica RLS em TenantFiscalData
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Clients,					-- Aplica RLS em Clients
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientContacts,			-- Aplica RLS em ClientContacts
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientAddresses,			-- Aplica RLS em ClientAddresses
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembers,				-- Aplica RLS em TeamMembers
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMemberContacts,		
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMemberAddresses,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionContacts,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionAddresses,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Interventions,   -- Aplica RLS em Interventions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Vehicles,        -- Aplica RLS em Vehicles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Equipments,      -- Aplica RLS em Equipments

ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users AFTER INSERT,	            -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users AFTER UPDATE,	            -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users BEFORE DELETE,             -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Roles AFTER INSERT,              -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Roles AFTER UPDATE,              -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Roles BEFORE DELETE,             -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserRoles AFTER INSERT,          -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserRoles AFTER UPDATE,          -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserRoles BEFORE DELETE,         -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.RolePermissions AFTER INSERT,    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.RolePermissions AFTER UPDATE,    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.RolePermissions BEFORE DELETE,   -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.JwtKeys AFTER INSERT,            -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.JwtKeys AFTER UPDATE,            -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.JwtKeys BEFORE DELETE,           -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Subscriptions AFTER INSERT,      -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Subscriptions AFTER UPDATE,      -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Subscriptions BEFORE DELETE,     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantContacts AFTER INSERT,     -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantContacts AFTER UPDATE,     -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantContacts BEFORE DELETE,    -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantAddresses AFTER INSERT,    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantAddresses AFTER UPDATE,    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantAddresses BEFORE DELETE,   -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantFiscalData AFTER INSERT,   -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantFiscalData AFTER UPDATE,   -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantFiscalData BEFORE DELETE,  -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Clients AFTER INSERT,            -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Clients AFTER UPDATE,            -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Clients BEFORE DELETE,           -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientContacts AFTER INSERT,     -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientContacts AFTER UPDATE,     -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientContacts BEFORE DELETE,    -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientAddresses AFTER INSERT,    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientAddresses AFTER UPDATE,    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientAddresses BEFORE DELETE,   -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembers AFTER INSERT,        -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembers AFTER UPDATE,        -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembers BEFORE DELETE,       -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberContacts AFTER INSERT,     -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberContacts AFTER UPDATE,     -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberContacts BEFORE DELETE,    -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberAddresses AFTER INSERT,    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberAddresses AFTER UPDATE,    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberAddresses BEFORE DELETE,   -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionContacts AFTER INSERT,   -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionContacts AFTER UPDATE,   -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionContacts BEFORE DELETE,  -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionAddresses AFTER INSERT,  -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionAddresses AFTER UPDATE,  -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionAddresses BEFORE DELETE, -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Interventions AFTER INSERT,      -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Interventions AFTER UPDATE,      -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Interventions BEFORE DELETE,     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Vehicles AFTER INSERT,           -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Vehicles AFTER UPDATE,           -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Vehicles BEFORE DELETE,          -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Equipments AFTER INSERT,         -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Equipments AFTER UPDATE,         -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Equipments BEFORE DELETE         -- Bloqueia DELETE fora do Tenant
WITH (STATE = ON);																		            -- Ativa a policy
GO
