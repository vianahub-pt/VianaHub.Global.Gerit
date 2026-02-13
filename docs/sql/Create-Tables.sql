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
    CreatedAt	                DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	                INT         		NULL,							-- Usuário modificador
    ModifiedAt	                DATETIME2(7)		NULL,							-- Data de modificação
    CONSTRAINT PK_Plans PRIMARY KEY CLUSTERED (Id),                                 -- PK
    CONSTRAINT CK_Plans_DeletedImpliesInactive CHECK (IsDeleted = 0 OR IsActive = 0)-- Soft delete -> não ativo
);
GO

CREATE TABLE dbo.Tenants (											-- Tabela principal de tenants
    Id			INT IDENTITY(1,1)	NOT NULL,						-- Identificador único do tenant, chave primária
    Name	    NVARCHAR(200)		NOT NULL,						-- Razão social
	Consent		BIT					NOT NULL DEFAULT 1,				-- Consentimento LGPD
    IsActive	BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy	INT         		NOT NULL,						-- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		NULL,							-- Usuário modificador
    ModifiedAt	DATETIME2(7)		NULL,							-- Data de modificação
	CONSTRAINT PK_Tenants PRIMARY KEY CLUSTERED (Id)
);
GO
CREATE TABLE dbo.TenantContacts (									    -- Contatos do tenant
    Id			INT IDENTITY(1,1)	NOT NULL,						    -- Identificador do contato, chave primária
    TenantId	INT					NOT NULL,						    -- Tenant dono do contato
    Name		NVARCHAR(150)		NOT NULL,						    -- Nome do contato
    Email		NVARCHAR(255)		NOT NULL,						    -- Email do contato
    Phone		NVARCHAR(30)			NULL,						    -- Telefone
    IsPrimary	BIT					NOT NULL DEFAULT 0,				    -- Contato principal
    IsActive	BIT					NOT NULL DEFAULT 1,				    -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				    -- Soft delete
    CreatedBy	INT         		NOT NULL,						    -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	INT         		    NULL,						    -- Usuário modificador
    ModifiedAt	DATETIME2(7)			NULL,						-- Data de modificação
	CONSTRAINT PK_TenantContacts PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_TenantContacts_Email UNIQUE (TenantId, Email),			                -- Email único por client
    CONSTRAINT FK_TenantContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
        
);
GO
CREATE TABLE dbo.AddressTypes (									                            -- Tipos de endereço (residencial, comercial, etc)
    Id			INT IDENTITY(1,1)	NOT NULL,						                        -- Identificador único do tenant, chave primária
    TenantId	INT					NOT NULL,						                        -- Tenant dono do contato
    Name	    NVARCHAR(200)		NOT NULL,						                        -- Nome do tipo de endereço
    Description NVARCHAR(500)       NOT NULL,                                               -- Descrição do tipo de endereço
    IsActive	BIT					NOT NULL DEFAULT 1,                                     -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,                                     -- Soft delete
    CreatedBy	INT         		NOT NULL,						                        -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                        -- Data de criação
    ModifiedBy	INT         		NULL,							                        -- Usuário modificador
    ModifiedAt	DATETIME2(7)		NULL,							                        -- Data de modificação
	CONSTRAINT PK_AddressesType PRIMARY KEY CLUSTERED (Id),                                 -- PK
    CONSTRAINT UQ_AddressesType_Id_Tenant UNIQUE (Id, TenantId),                            -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_AddressesType_Tenant_Name UNIQUE (TenantId, Name),                        -- Tipo de endereço único por tenant
    CONSTRAINT FK_AddressesType_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.TenantAddresses (									                        -- Endereços do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						                    -- Identificador do endereço, chave primária
    TenantId		INT					NOT NULL,						                    -- Tenant dono
    AddressTypeId   INT                 NOT NULL,					                        -- Tipo de endereço (Residencial, Comercial, Billing, etc.)
    CountryCode		CHAR(2)				NOT NULL DEFAULT 'PT',			                    -- País
    Street			NVARCHAR(200)		NOT NULL,						                    -- Rua
    Neighborhood    NVARCHAR(100)       NOT NULL,						                    -- Bairro
    City			NVARCHAR(100)		NOT NULL,						                    -- Cidade
    District		NVARCHAR(100)			NULL,						                    -- Distrito
    PostalCode		NVARCHAR(20)		NOT NULL,						                    -- Código postal
    StreetNumber    NVARCHAR(20)            NULL,                                           -- Número da porta
    Complement      NVARCHAR(100)           NULL,                                           -- Apto, bloco, andar, etc.
    Latitude        DECIMAL(9,6)            NULL,                                           -- Latitude geográfica (opcional)
    Longitude       DECIMAL(9,6)            NULL,                                           -- Longitude geográfica (opcional)
    Notes           NVARCHAR(500)           NULL,                                           -- Observações adicionais sobre o endereço  
    IsPrimary		BIT					NOT NULL DEFAULT 0,				                    -- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_TenantAddresses PRIMARY KEY CLUSTERED (Id),		                        -- PK
    CONSTRAINT FK_TenantAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_TenantAddresses_AddressType FOREIGN KEY (AddressTypeId, TenantId) REFERENCES dbo.AddressTypes(Id, TenantId),	-- FK para tipo de endereço
    CONSTRAINT UQ_TenantAddresses_Id_Tenant UNIQUE (Id, TenantId)                                                               -- Garantir que o Id é único dentro do tenant (para FKs compostas)
);
GO
CREATE TABLE dbo.TenantFiscalData (										                    -- Dados fiscais do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						                    -- Identificador fiscal, chave primária
    TenantId		INT					NOT NULL,						                    -- Tenant dono
    NIF				CHAR(9)				NOT NULL,						                    -- NIF português
    VATNumber		NVARCHAR(20)		NOT NULL,						                    -- VAT calculado
    CAE				NVARCHAR(10)			NULL,						                    -- Código CAE
    FiscalCountry	CHAR(2)				NOT NULL DEFAULT 'PT',			                    -- País fiscal
    IsVATRegistered BIT					NOT NULL DEFAULT 1,				                    -- Regime IVA
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_TenantFiscalData PRIMARY KEY CLUSTERED (Id),			                    -- PK   
    CONSTRAINT FK_TenantFiscalData_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id) -- FK para tenant    
);
GO
CREATE TABLE dbo.Subscriptions (                                                                -- Assinatura do tenant (contrato de billing)
    Id                      INT IDENTITY(1,1)   NOT NULL,                                       -- Identificador da assinatura, chave primária
    TenantId                INT                 NOT NULL,                                       -- FK para tenant
    PlanId                  INT                 NOT NULL,                                       -- FK para plano global
    StripeId                NVARCHAR(100)           NULL,                                       -- Id do subscription no Stripe
    CurrentPeriodStart      DATETIME2(7)        NOT NULL,                                       -- Início do período faturado
    CurrentPeriodEnd        DATETIME2(7)        NOT NULL,                                       -- Fim do período faturado
    TrialStart              DATETIME2(7)            NULL,                                       -- Trial
    TrialEnd                DATETIME2(7)            NULL,                                       -- Trial
    CancelAtPeriodEnd       BIT                 NOT NULL DEFAULT 0,                             -- Cancelar no fim do ciclo
    CanceledAt              DATETIME2(7)            NULL,                                       -- Quando cancelou
    CancellationReason      NVARCHAR(500)           NULL,                                       -- Motivo
    StripeCustomerId        NVARCHAR(100)           NULL,                                       -- Customer id
    IsActive		        BIT					NOT NULL DEFAULT 1,				                -- Flag de ativo
    IsDeleted		        BIT					NOT NULL DEFAULT 0,				                -- Soft delete
    CreatedBy		        INT         		NOT NULL,						                -- Usuário criador
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                -- Data de criação
    ModifiedBy	            INT         		    NULL,						                -- Usuário modificador
    ModifiedAt		        DATETIME2(7)			NULL,						                -- Data de modificação
    CONSTRAINT PK_Subscriptions PRIMARY KEY CLUSTERED (Id),                                     -- PK
    CONSTRAINT FK_Subscriptions_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),       -- Tenant-safe
    CONSTRAINT FK_Subscriptions_Plan FOREIGN KEY (PlanId) REFERENCES dbo.Plans(Id),             -- Global plan
    CONSTRAINT UQ_Subscriptions_TenantId_Id UNIQUE (TenantId, Id),                              -- Chave alternativa (TenantId, Id) p/ FKs compostas
    CONSTRAINT CK_Subscriptions_DeletedImpliesInactive CHECK (IsDeleted = 0 OR IsActive = 0)    -- Se está deletado, não pode estar ativo
        
);
GO
CREATE TABLE dbo.Users (                                                            -- Usuários do sistema
    Id				        INT IDENTITY(1,1)	NOT NULL,                           -- Identificador do usuário, chave primária
    TenantId		        INT					NOT NULL,                           -- Tenant do usuário
    Name		            NVARCHAR(150)		NOT NULL,                           -- Nome completo
    Email                   NVARCHAR(256)       NOT NULL,                           -- Email original
    NormalizedEmail         NVARCHAR(256)       NOT NULL,                           -- Email normalizado (case-insensitive)
    EmailConfirmed          BIT                 NOT NULL DEFAULT 0,                 -- Confirmação de email
    PhoneNumber             NVARCHAR(50)            NULL,                           -- Telefone (opcional)
    PhoneNumberConfirmed    BIT                 NOT NULL DEFAULT 0,                 -- Confirmação de telefone
    LastAccessAt            DATETIME2(7)            NULL,                           -- Último login/acesso
    PasswordHash	        NVARCHAR(500)		NOT NULL,						    -- Hash da senha
    IsActive		        BIT					NOT NULL DEFAULT 1,				    -- Flag de ativo
    IsDeleted		        BIT					NOT NULL DEFAULT 0,				    -- Soft delete
    CreatedBy		        INT         		NOT NULL,						    -- Usuário criador
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	            INT         		    NULL,						    -- Usuário modificador
    ModifiedAt		        DATETIME2(7)		NULL,                           -- Data de modificação
	CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Users_Tenant_Email UNIQUE (TenantId, Email),			            -- Nome único por tenant
    CONSTRAINT UQ_Users_Tenant_NormalizedEmail UNIQUE (TenantId, NormalizedEmail),	-- Email único por tenant
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
    Description     NVARCHAR(500)		NOT NULL,						-- Descrição da role
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,                       -- Data de modificação
	CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (Id),						
    CONSTRAINT UQ_Roles_Tenant_Name UNIQUE (TenantId, Name),			-- Role única por tenant
    CONSTRAINT FK_Roles_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.Resources (											-- Recursos do sistema
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador do recurso, chave primária
    Name			NVARCHAR(100)		NOT NULL UNIQUE,				-- Nome único do recurso
    Description     NVARCHAR(500)		NOT NULL,						-- Descrição do recurso
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						-- Data de modificação
	CONSTRAINT PK_Resources PRIMARY KEY CLUSTERED (Id),					
	CONSTRAINT UQ_Resources_Name UNIQUE (Name)							-- Recursos únicos
);
GO
CREATE TABLE dbo.Actions (												-- Ações possíveis
    Id				INT IDENTITY(1,1)	NOT NULL,						-- Identificador da ação, chave primária
    Name			NVARCHAR(50)		NOT NULL,                       -- Nome da ação
    Description     NVARCHAR(500)		NOT NULL,                       -- Descrição da ação
    IsActive		BIT					NOT NULL DEFAULT 1,             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,             -- Soft delete
    CreatedBy		INT         		NOT NULL,						-- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,                       -- Data de modificação
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
CREATE TABLE dbo.RefreshTokens (
    Id 						INT IDENTITY(1,1)	NOT NULL,
    TenantId 				INT					NOT NULL,
	UserId 					INT					NOT NULL,
	Token					NVARCHAR(MAX)		NOT NULL,
	ExpiresAt				DATETIME2(7) 			NOT	NULL,
    RevokedAt 				DATETIME2(7) 				NULL,
	RevokedBy				INT 					NULL,
    CreatedBy		        INT		            NOT NULL,
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),
    ModifiedBy	            INT         		    NULL,
    ModifiedAt		        DATETIME2(7)			NULL,
    CONSTRAINT PK_RefreshTokens PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_RefreshTokens_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
	CONSTRAINT FK_RefreshTokens_User FOREIGN KEY (userId) REFERENCES dbo.Users(Id)
);
GO
CREATE TABLE dbo.JwtKeys (
    Id 						INT IDENTITY(1,1)	NOT NULL,
    TenantId 				INT					NOT NULL,
    KeyId 					UNIQUEIDENTIFIER	NOT NULL,
    PublicKey 				NVARCHAR(MAX) 		NOT NULL,
    PrivateKeyEncrypted 	NVARCHAR(MAX) 		NOT NULL,
    Algorithm 				NVARCHAR(50) 		NOT NULL DEFAULT 'RS256',
    KeySize 				INT 				NOT NULL DEFAULT 2048,
    KeyType 				NVARCHAR(50) 		NOT NULL DEFAULT 'RSA',
    RevokedReason 			NVARCHAR(500) 			NULL,
    UsageCount 				BIGINT 				NOT NULL DEFAULT 0,
    ActivatedAt 			DATETIME2(7) 				NULL,
    ExpiresAt 				DATETIME2(7) 			NOT NULL,
    LastUsedAt 				DATETIME2(7) 				NULL,
    NextRotationAt 			DATETIME2(7) 			NOT NULL,
    RevokedAt 				DATETIME2(7) 				NULL,
    LastValidatedAt 		DATETIME2(7) 				NULL,
    ValidationCount 		BIGINT 				NOT NULL DEFAULT 0,
    RotationPolicyDays 		INT 				NOT NULL DEFAULT 90,
    OverlapPeriodDays 		INT 				NOT NULL DEFAULT 7,
    MaxTokenLifetimeMinutes INT 				NOT NULL DEFAULT 60,
    IsActive 				BIT 				NOT NULL DEFAULT 0,
    IsDeleted 				BIT 				NOT NULL DEFAULT 0,
    CreatedBy		        INT		            NOT NULL,
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),
    ModifiedBy	            INT         		    NULL,
    ModifiedAt		        DATETIME2(7)			NULL,
    CONSTRAINT PK_JwtKeys PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_JwtKeys_KeyId UNIQUE (KeyId),
    CONSTRAINT FK_JwtKeys_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
    CONSTRAINT CK_JwtKeys_RotationPolicy CHECK (RotationPolicyDays BETWEEN 30 AND 365),
    CONSTRAINT CK_JwtKeys_OverlapPeriod CHECK (OverlapPeriodDays BETWEEN 1 AND 30),
    CONSTRAINT CK_JwtKeys_MaxTokenLifetime CHECK (MaxTokenLifetimeMinutes BETWEEN 5 AND 1440)
);
GO

CREATE TABLE dbo.JobDefinitions (
    Id                      INT IDENTITY(1,1)	NOT NULL,
    JobCategory             NVARCHAR(100)		NOT NULL,                              -- Categoria (Cleanup, Maintenance, Security, Billing, Sync)
    JobName                 NVARCHAR(150)		NOT NULL,                              -- Nome único do job (usado como JobId no Hangfire)
    Description             NVARCHAR(500)		    NULL,                              -- Descrição detalhada
    JobPurpose              NVARCHAR(500)		    NULL,                              -- Propósito/objetivo do job
    JobType                 NVARCHAR(200)		NOT NULL,                              -- Namespace. Classe do job no código
    JobMethod               NVARCHAR(100)		NOT NULL DEFAULT 'Execute',            -- Nome do método a ser executado
    CronExpression          NVARCHAR(100)		    NULL,                              -- Expressão Cron para agendamento (null se fire-and-forget)
    TimeZoneId              NVARCHAR(100)		NOT NULL DEFAULT 'GMT Standard Time',  -- Timezone padrão (Portugal - UTC+0/UTC+1)
    ExecuteOnlyOnce         BIT					NOT NULL DEFAULT 0,                    -- Se deve executar apenas uma vez (fire-and-forget)
    TimeoutMinutes          INT					NOT NULL DEFAULT 5,                    -- Timeout em minutos
    Priority                INT					NOT NULL DEFAULT 5,                    -- Prioridade (1=highest, 10=lowest)
    Queue                   NVARCHAR(50)		NOT NULL DEFAULT 'default',            -- Fila do Hangfire (default, critical, low)
    MaxRetries              INT					NOT NULL DEFAULT 3,                    -- Máximo de tentativas automáticas
    JobConfiguration        NVARCHAR(MAX)		    NULL,                              -- JSON com configurações específicas do job
    IsSystemJob             BIT					NOT NULL DEFAULT 0,                    -- Job crítico do sistema (não pode ser deletado)
    HangfireJobId           NVARCHAR(100)		    NULL,                              -- ID do job recorrente no Hangfire
    LastRegisteredAt        DATETIME2(7)		    NULL,                              -- Última vez que foi registrado no Hangfire
    IsActive                BIT					NOT NULL DEFAULT 1,                    -- Indica se o job está ativo
    IsDeleted               BIT					NOT NULL DEFAULT 0,                    -- Indica se foi excluído (soft delete)
    CreatedBy               INT					NOT NULL,                              -- Quem criou o job
    CreatedAt               DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),        -- Data de criação
    UpdatedBy               INT					    NULL,                              -- Quem fez a última alteração
    UpdatedAt               DATETIME2(7)		    NULL,                              -- Data da última alteração
    CONSTRAINT UQ_Job_JobName UNIQUE (JobName),
    CONSTRAINT CK_Job_Priority CHECK (Priority BETWEEN 1 AND 10),
    CONSTRAINT CK_Job_TimeoutMinutes CHECK (TimeoutMinutes > 0),
    CONSTRAINT CK_Job_MaxRetries CHECK (MaxRetries >= 0)
);
GO

-- Índices para performance
CREATE INDEX IX_Services_Category_Active 
    ON dbo.JobDefinitions(JobCategory, IsActive, IsDeleted);

CREATE INDEX IX_Services_Active_System 
    ON dbo.JobDefinitions(IsActive, IsSystemJob) 
    WHERE IsDeleted = 0;

CREATE INDEX IX_Services_HangfireJobId 
    ON dbo.JobDefinitions(HangfireJobId) 
    WHERE HangfireJobId IS NOT NULL;
GO

/* =========================
   DOMAIN TABLE EXAMPLE
   (All domain tables MUST have TenantId)
   ========================= */

CREATE TABLE dbo.Functions (
	Id              INT IDENTITY(1,1)   NOT NULL,                                       -- Identificador da função, chave primária
	TenantId        INT                 NOT NULL,                                       -- Tenant dono da função
	Name            NVARCHAR(150)       NOT NULL,                                       -- Nome da função
	Description     NVARCHAR(500)       NOT NULL,                                       -- Descrição da função
    IsActive		BIT					NOT NULL DEFAULT 1,                             -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,                             -- Soft delete
    CreatedBy		INT         		NOT NULL,						                -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                -- Data de criação
    ModifiedBy	    INT         		    NULL,						                -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,                                       -- Data de modificação
    CONSTRAINT PK_Functions PRIMARY KEY CLUSTERED (Id),                                 -- PK
    CONSTRAINT UQ_Functions_Id_Tenant UNIQUE (Id, TenantId),                            -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_Functions_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)    -- FK para tenant
)
GO
CREATE TABLE dbo.Clients (
    Id				INT IDENTITY(1,1)	NOT NULL,						            -- Clientes do tenant
    TenantId		INT					NOT NULL,						            -- Tenant dono
    Name			NVARCHAR(150)		NOT NULL,						            -- Nome do cliente
    Email			NVARCHAR(255)			NULL,						            -- Email do cliente
    Phone			NVARCHAR(50)		NOT NULL,						            -- Phone do cliente
    Consent			BIT					NOT NULL DEFAULT 1,				            -- Consentimento LGPD
    IsActive		BIT					NOT NULL DEFAULT 1,                         -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,                         -- Soft delete
    CreatedBy		INT         		NOT NULL,						            -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	            -- Data de criação
    ModifiedBy	    INT         		    NULL,						            -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,                                   -- Data de modificação
	CONSTRAINT PK_Clients PRIMARY KEY CLUSTERED (Id),                               -- PK
    CONSTRAINT UQ_Clients_Email UNIQUE (TenantId, Email),			                -- Email único por tenant
    CONSTRAINT UQ_Clients_Tenant_Name UNIQUE (TenantId, Name),                      -- Nome único por tenant
    CONSTRAINT UQ_Clients_Id_Tenant UNIQUE (Id, TenantId),                            -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_Clients_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)  -- FK para tenant
);
GO
CREATE TABLE dbo.ClientContacts (															-- Contatos do client
    Id			INT IDENTITY(1,1)	NOT NULL,						                        -- Identificador do contato, chave primária
    TenantId	INT					NOT NULL,						                        -- Tenant dono do contato
	ClientId	INT					NOT NULL,						                        -- Client dono do contato
    Name		NVARCHAR(150)		NOT NULL,						                        -- Nome do contato
    Email		NVARCHAR(255)		NOT NULL,						                        -- Email do contato
    Phone		NVARCHAR(30)			NULL,						                        -- Telefone
    IsPrimary	BIT					NOT NULL DEFAULT 0,				                        -- Contato principal
    IsActive	BIT					NOT NULL DEFAULT 1,				                        -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				                        -- Soft delete
    CreatedBy	INT         		NOT NULL,						                        -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                        -- Data de criação
    ModifiedBy	INT         		    NULL,						                        -- Usuário modificador
    ModifiedAt	DATETIME2(7)			NULL,						                        -- Data de modificação
	CONSTRAINT PK_ClientContacts PRIMARY KEY CLUSTERED (Id),                                -- PK
    CONSTRAINT UQ_ClientContacts_Client_Email UNIQUE (TenantId, ClientId, Email),			-- Email único por client
    CONSTRAINT FK_ClientContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_ClientContacts_Client FOREIGN KEY (ClientId, TenantId) REFERENCES dbo.Clients(Id, TenantId)	-- FK para client
);
GO
CREATE TABLE dbo.ClientAddresses (															-- Endereços do client
    Id			    INT IDENTITY(1,1)	NOT NULL,						                    -- Identificador do endereço, chave primária
    TenantId	    INT					NOT NULL,						                    -- Tenant dono do endereço
	ClientId	    INT					NOT NULL,						                    -- Client dono do endereço
    AddressTypeId   INT                 NOT NULL,					                        -- Tipo de endereço (Residencial, Comercial, Billing, etc.)
    CountryCode		CHAR(2)				NOT NULL DEFAULT 'PT',			                    -- País
    Street			NVARCHAR(200)		NOT NULL,						                    -- Rua
    Neighborhood    NVARCHAR(100)       NOT NULL,						                    -- Bairro
    City			NVARCHAR(100)		NOT NULL,						                    -- Cidade
    District		NVARCHAR(100)			NULL,						                    -- Distrito
    PostalCode		NVARCHAR(20)		NOT NULL,						                    -- Código postal
    StreetNumber    NVARCHAR(20)            NULL,                                           -- Número da porta
    Complement      NVARCHAR(100)           NULL,                                           -- Apto, bloco, andar, etc.
    Latitude        DECIMAL(9,6)            NULL,                                           -- Latitude geográfica (opcional)
    Longitude       DECIMAL(9,6)            NULL,                                           -- Longitude geográfica (opcional)
    Notes           NVARCHAR(500)           NULL,                                           -- Observações adicionais sobre o endereço  
    IsPrimary		BIT					NOT NULL DEFAULT 0,				                    -- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_ClientAddresses PRIMARY KEY CLUSTERED (Id),		                        -- PK
    CONSTRAINT FK_ClientAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_ClientAddresses_Client FOREIGN KEY (ClientId, TenantId) REFERENCES dbo.Clients(Id, TenantId),	-- FK para client
    CONSTRAINT FK_ClientAddresses_AddressType FOREIGN KEY (AddressTypeId, TenantId) REFERENCES dbo.AddressTypes(Id, TenantId)	-- FK para tipo de endereço
);
GO
CREATE TABLE dbo.ClientFiscalData (										                    -- Dados fiscais do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						                    -- Identificador fiscal, chave primária
    TenantId		INT					NOT NULL,						                    -- Tenant dono
	ClientId		INT					NOT NULL,						                    -- Tenant dono
    TaxNumber		CHAR(9)				NOT NULL,						                    -- NIF português
    VATNumber		NVARCHAR(20)		NOT NULL,						                    -- VAT calculado
    CAE				NVARCHAR(10)			NULL,						                    -- Código CAE
    FiscalCountry	CHAR(2)				NOT NULL DEFAULT 'PT',			                    -- País fiscal
    IsVATRegistered BIT					NOT NULL DEFAULT 1,				                    -- Regime IVA
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_ClientFiscalData PRIMARY KEY CLUSTERED (Id),                              -- PK			
    CONSTRAINT FK_ClientFiscalData_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),-- FK para tenant    
	CONSTRAINT FK_ClientFiscalData_Client FOREIGN KEY (ClientId, TenantId) REFERENCES dbo.Clients(Id, TenantId) -- FK para client
);
GO
CREATE TABLE dbo.Teams (                                                                    -- Times de trabalho, projetos, squads, etc.
    Id              INT IDENTITY(1,1)   NOT NULL,                                           -- Identificador do time, chave primária
    TenantId        INT                 NOT NULL,                                           -- Tenant dono do time
    Name            NVARCHAR(150)       NOT NULL,                                           -- Nome do time
    Description     NVARCHAR(500)       NOT NULL,                                           -- Descrição do time
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
    CONSTRAINT PK_Teams PRIMARY KEY CLUSTERED (Id),                                         -- PK			
    CONSTRAINT UQ_Teams_Id_Tenant UNIQUE (Id, TenantId),                                    -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_Teams_Name_Tenant UNIQUE (Name, TenantId),                                -- Nome único por tenant
    CONSTRAINT FK_Teams_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)            -- FK para tenant
);
GO
CREATE TABLE dbo.TeamMembers (
    Id			INT IDENTITY(1,1)	NOT NULL,						                                                -- Membros do time
    TenantId	INT					NOT NULL,						                                                -- Tenant dono do endereço
    FunctionId	INT					NOT NULL,						                                                -- Função do membro do time
    Name		NVARCHAR(150)		NOT NULL,						                                                -- Nome do membro do time
	TaxNumber	NVARCHAR(20)			NULL,						                                                -- Numero fiscal do membro do time
    IsActive	BIT					NOT NULL DEFAULT 1,				                                                -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,				                                                -- Soft delete
    CreatedBy	INT         		NOT NULL,						                                                -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                                                -- Data de criação
    ModifiedBy	INT         		    NULL,						                                                -- Usuário modificador
    ModifiedAt	DATETIME2(7)			NULL,						                                                -- Data de modificação
	CONSTRAINT PK_TeamMembers PRIMARY KEY CLUSTERED (Id),                                                           -- PK
    CONSTRAINT UQ_TeamMembers_Id_Tenant UNIQUE (Id, TenantId),                                                      -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_TeamMembers_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),                             -- FK para tenant
    CONSTRAINT FK_TeamMembers_Function FOREIGN KEY (FunctionId, TenantId) REFERENCES dbo.Functions(Id, TenantId)    -- FK para função (tenant-safe
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
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						-- Data de modificação
	CONSTRAINT PK_TeamMemberContacts PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_TeamMemberContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),				-- FK para tenant
    CONSTRAINT FK_TeamMemberContacts_TeamMember FOREIGN KEY (TeamMemberId, TenantId) REFERENCES dbo.TeamMembers(Id, TenantId)	-- FK para client
);
GO
CREATE TABLE dbo.TeamMemberAddresses (									                    -- Endereços do TeamMember
    Id				INT IDENTITY(1,1)	NOT NULL,						                    -- Identificador do endereço, chave primária
    TenantId		INT					NOT NULL,						                    -- Tenant dono
	TeamMemberId	INT					NOT NULL,						                    -- Endereço do membro do time
    AddressTypeId   INT                 NOT NULL,					                        -- Tipo de endereço (Residencial, Comercial, Billing, etc.)
    CountryCode		CHAR(2)				NOT NULL DEFAULT 'PT',			                    -- País
    Street			NVARCHAR(200)		NOT NULL,						                    -- Rua
    Neighborhood    NVARCHAR(100)       NOT NULL,						                    -- Bairro
    City			NVARCHAR(100)		NOT NULL,						                    -- Cidade
    District		NVARCHAR(100)			NULL,						                    -- Distrito
    PostalCode		NVARCHAR(20)		NOT NULL,						                    -- Código postal
    StreetNumber    NVARCHAR(20)            NULL,                                           -- Número da porta
    Complement      NVARCHAR(100)           NULL,                                           -- Apto, bloco, andar, etc.
    Latitude        DECIMAL(9,6)            NULL,                                           -- Latitude geográfica (opcional)
    Longitude       DECIMAL(9,6)            NULL,                                           -- Longitude geográfica (opcional)
    Notes           NVARCHAR(500)           NULL,                                           -- Observações adicionais sobre o endereço  
    IsPrimary		BIT					NOT NULL DEFAULT 0,				                    -- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_TeamMemberAddresses PRIMARY KEY CLUSTERED (Id),                           -- PK
    CONSTRAINT FK_TeamMemberAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),				-- FK para tenant
    CONSTRAINT FK_TeamMemberAddresses_TeamMember FOREIGN KEY (TeamMemberId, TenantId) REFERENCES dbo.TeamMembers(Id, TenantId),	-- FK para client     
    CONSTRAINT FK_TeamMemberAddresses_AddressType FOREIGN KEY (AddressTypeId, TenantId) REFERENCES dbo.AddressTypes(Id, TenantId)	-- FK para tipo de endereço
);
GO
CREATE TABLE dbo.TeamMembersTeams (                                                         -- Associação N:N entre membros e times (um membro pode estar em vários times e um time pode ter vários membros)
    Id              INT IDENTITY(1,1)   NOT NULL,                                           -- Identificador, chave primária
    TenantId        INT                 NOT NULL,                                           -- Tenant dono
    TeamId          INT                 NOT NULL,                                           -- Time ao qual o membro pertence
    TeamMemberId    INT                 NOT NULL,                                           -- Membro do time
    IsLeader        BIT                 NOT NULL DEFAULT 0,                                 -- Indica se o membro é líder do time
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
    CONSTRAINT PK_TeamMembersTeams PRIMARY KEY CLUSTERED (Id),                              -- PK
    CONSTRAINT FK_TeamMembersTeams_Team FOREIGN KEY (TeamId, TenantId) REFERENCES dbo.Teams(Id, TenantId),              -- FK para time
    CONSTRAINT FK_TeamMembersTeams_Member FOREIGN KEY (TeamMemberId, TenantId) REFERENCES dbo.TeamMembers(Id, TenantId) -- FK para membro do time (tenant-safe
);
GO
CREATE TABLE dbo.EquipmentTypes (									                        -- Tipos de equipamentos do tenant
    Id			INT IDENTITY(1,1)	NOT NULL,						                        -- Identificador único do tenant, chave primária
    TenantId	INT					NOT NULL,						                        -- Tenant dono do contato
    Name	    NVARCHAR(200)		NOT NULL,						                        -- Nome do tipo de endereço
    Description NVARCHAR(500)		NOT NULL,						                        -- Descrição do tipo de equipamento
    IsActive	BIT					NOT NULL DEFAULT 1,                                     -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,                                     -- Soft delete
    CreatedBy	INT         		NOT NULL,						                        -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                        -- Data de criação
    ModifiedBy	INT         		NULL,							                        -- Usuário modificador
    ModifiedAt	DATETIME2(7)		NULL,							                        -- Data de modificação
	CONSTRAINT PK_EquipmentTypes PRIMARY KEY CLUSTERED (Id),                                -- PK
    CONSTRAINT UQ_EquipmentTypes_Id_Tenant UNIQUE (Id, TenantId),                           -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_EquipmentTypes_Tenant_Name UNIQUE (TenantId, Name),                       -- Tipo de endereço único por tenant
    CONSTRAINT FK_EquipmentTypes_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.Equipments (
    Id				    INT IDENTITY(1,1)	NOT NULL,						            -- Identificador, chave primária
	TenantId		    INT					NOT NULL,						            -- Tenant dono
    EquipmentTypeId	    INT					NOT NULL,						            -- Tipo do equipamento (FK para EquipmentTypes)
    Name			    NVARCHAR(150)		NOT NULL,						            -- Nome do equipamento
    SerialNumber	    NVARCHAR(100)			NULL,						            -- Número de série do equipamento
	Status			    INT					NOT NULL,						            -- Estado (0=Disponível, 1=Em uso, 2=Em manutenção)						
    IsActive		    BIT					NOT NULL DEFAULT 1,				            -- Flag de ativo
    IsDeleted		    BIT					NOT NULL DEFAULT 0,				            -- Soft delete
    CreatedBy		    INT         		NOT NULL,						            -- Usuário criador
    CreatedAt		    DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	            -- Data de criação
    ModifiedBy	        INT         		    NULL,						            -- Usuário modificador
    ModifiedAt		    DATETIME2(7)			NULL,						            -- Data de modificação
	CONSTRAINT PK_Equipments PRIMARY KEY CLUSTERED (Id),                                -- PK
    CONSTRAINT UQ_Equipments_Id_Tenant UNIQUE (Id, TenantId),							-- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_Equipments_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),  -- FK para tenant
    CONSTRAINT FK_Equipments_EquipamentType FOREIGN KEY (EquipmentTypeId, TenantId) REFERENCES dbo.EquipmentTypes(Id, TenantId)	                                -- FK para tipo de equipamento (tenant-safe)
);
GO
CREATE TABLE dbo.Vehicles (                                                                 -- Veículos do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,                                           -- Identificador, chave primária
    TenantId        INT					NOT NULL,                                           -- Tenant dono
    Plate           NVARCHAR(20)		NOT NULL,                                           -- Placa do veículo
    Brand           NVARCHAR(100)		NOT NULL,                                           -- Marca do veículo
    Model           NVARCHAR(100)		NOT NULL,                                           -- Modelo do veículo
    Year            INT					NOT NULL,                                           -- Ano de fabricação
    Color           NVARCHAR(50)            NULL,                                           -- Cor do veículo
    FuelType        NVARCHAR(50)            NULL,                                           -- Tipo de combustível (Gasolina, Diesel, Elétrico, Híbrido)
    IsActive		BIT					NOT NULL DEFAULT 1,					                -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,					                -- Soft delete
    CreatedBy		INT         		NOT NULL,							                -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),		                -- Data de criação
    ModifiedBy	    INT         		    NULL,							                -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_Vehicles PRIMARY KEY CLUSTERED (Id),                                      -- PK
    CONSTRAINT UQ_Vehicles_Id_Tenant UNIQUE (Id, TenantId),                                 -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_Vehicles_Tenant_Plate UNIQUE (TenantId, Plate),                           -- Placa única por tenant
    CONSTRAINT CK_Vehicles_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),     -- Garantir que um veículo não pode ser ativo e deletado ao mesmo tempo
	CONSTRAINT FK_Vehicles_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)        -- FK para tenant
);
GO
CREATE TABLE dbo.InterventionStatus (                                                           -- Status das intervenções
    Id			INT IDENTITY(1,1)	NOT NULL,                                                   -- Identificador, chave primária
    TenantId	INT					NOT NULL,                                                   -- Tenant dono
    Name	    NVARCHAR(200)		NOT NULL,                                                   -- Nome do status (Agendada, Em andamento, Concluída, Cancelada)
    Description NVARCHAR(500)		NOT NULL,                                                   -- Descrição do status
    IsActive	BIT					NOT NULL DEFAULT 1,                                         -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,                                         -- Soft delete
    CreatedBy	INT         		NOT NULL,						                            -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                            -- Data de criação
    ModifiedBy	INT         		NULL,							                            -- Usuário modificador
    ModifiedAt	DATETIME2(7)		NULL,							                            -- Data de modificação
	CONSTRAINT PK_InterventionStatus PRIMARY KEY CLUSTERED (Id),                                -- PK
    CONSTRAINT UQ_InterventionStatus_Id_Tenant UNIQUE (Id, TenantId),                           -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_InterventionStatus_Tenant_Name UNIQUE (TenantId, Name),                       -- Tipo de endereço único por tenant
    CONSTRAINT FK_InterventionStatus_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)   -- FK para tenant
);
GO
CREATE TABLE dbo.Interventions (                                                                                                    -- Intervenções agendadas
    Id				        INT IDENTITY(1,1)	NOT NULL,                                                                           -- Identificador da intervenção, chave primária
	TenantId                INT					NOT NULL,                                                                           -- Tenant dono da intervenção
    ClientId		        INT					NOT NULL,                                                                           -- Cliente associado à intervenção
	TeamMemberId	        INT					NOT NULL,                                                                           -- Endereço do membro do time
    VehicleId		        INT					NOT NULL,                                                                           -- Veículo associado à intervenção
    InterventionStatusId    INT				    NOT NULL,                                                                           -- Status da intervenção
	Title			        NVARCHAR(200)		NOT NULL,                                                                           -- Título da intervenção
    Description		        NVARCHAR(2000)		NOT NULL,                                                                           -- Descrição detalhada da intervenção
    StartDateTime	        DATETIME2(7)		NOT NULL,                                                                           -- Data e hora de início da intervenção
    EndDateTime		        DATETIME2(7)			NULL,                                                                           -- Data e hora de término (pode ser atualizado após conclusão)
    EstimatedValue	        DECIMAL(10,2)		NOT NULL CHECK (EstimatedValue >= 0),                                               -- Valor estimado da intervenção
	RealValue		        DECIMAL(10,2)			NULL,                                                                           -- Valor real da intervenção (pode ser atualizado após conclusão)
    IsActive		        BIT					NOT NULL DEFAULT 1,								                                    -- Flag de ativo
    IsDeleted		        BIT					NOT NULL DEFAULT 0,								                                    -- Soft delete
    CreatedBy		        INT         		NOT NULL,										                                    -- Usuário criador
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),					                                    -- Data de criação
    ModifiedBy	            INT         		    NULL,										                                    -- Usuário modificador
    ModifiedAt		        DATETIME2(7)			NULL,										                                    -- Data de modificação
	CONSTRAINT PK_Interventions PRIMARY KEY CLUSTERED (Id),                                                                         -- PK
    CONSTRAINT UQ_Interventions_Id_Tenant UNIQUE (Id, TenantId),                                                                    -- Garantir que o Id é único dentro do tenant (para FKs compostas)
	CONSTRAINT FK_Interventions_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),                                          -- FK para tenant
	CONSTRAINT FK_Interventions_Clients FOREIGN KEY (ClientId, TenantId) REFERENCES dbo.Clients(Id, TenantId),                      -- FK para client
	CONSTRAINT FK_Interventions_TeamMembers FOREIGN KEY (TeamMemberId, TenantId) REFERENCES dbo.TeamMembers(Id, TenantId),          -- FK para membro do time
	CONSTRAINT FK_Interventions_Vehicles FOREIGN KEY (VehicleId, TenantId) REFERENCES dbo.Vehicles(Id, TenantId),                   -- FK para veículo
    CONSTRAINT FK_Interventions_Status FOREIGN KEY (InterventionStatusId, TenantId) REFERENCES dbo.InterventionStatus(Id, TenantId),-- FK para status da intervenção
	CONSTRAINT CK_Interventions_EndDateTime	CHECK ( EndDateTime IS NULL OR EndDateTime >= StartDateTime)                            -- Garantir que a data de término seja posterior à data de início (ou nula)
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
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	    INT         		    NULL,						-- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						-- Data de modificação
	CONSTRAINT PK_InterventionContacts PRIMARY KEY CLUSTERED (Id),		
    CONSTRAINT FK_InterventionContacts_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),				-- FK para tenant
    CONSTRAINT FK_InterventionContacts_Intervention FOREIGN KEY (InterventionId, TenantId) REFERENCES dbo.Interventions(Id, TenantId)	-- FK para client
);
GO
CREATE TABLE dbo.InterventionAddresses (								                                            -- Endereços do Intervention
    Id				INT IDENTITY(1,1)	NOT NULL,						                                            -- Identificador do endereço, chave primária
    TenantId		INT					NOT NULL,						                                            -- Tenant dono
	InterventionId	INT					NOT NULL,						                                            -- Intervenção associada
    AddressTypeId   INT                 NOT NULL,					                                                -- Tipo de endereço (Residencial, Comercial, Billing, etc.)
    CountryCode		CHAR(2)				NOT NULL DEFAULT 'PT',			                                            -- País
    Street			NVARCHAR(200)		NOT NULL,						                                            -- Rua
    Neighborhood    NVARCHAR(100)       NOT NULL,						                                            -- Bairro
    City			NVARCHAR(100)		NOT NULL,						                                            -- Cidade
    District		NVARCHAR(100)			NULL,						                                            -- Distrito
    PostalCode		NVARCHAR(20)		NOT NULL,						                                            -- Código postal
    StreetNumber    NVARCHAR(20)            NULL,                                                                   -- Número da porta
    Complement      NVARCHAR(100)           NULL,                                                                   -- Apto, bloco, andar, etc.
    Latitude        DECIMAL(9,6)            NULL,                                                                   -- Latitude geográfica (opcional)
    Longitude       DECIMAL(9,6)            NULL,                                                                   -- Longitude geográfica (opcional)
    Notes           NVARCHAR(500)           NULL,                                                                   -- Observações adicionais sobre o endereço  
    IsPrimary		BIT					NOT NULL DEFAULT 0,				                                            -- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				                                            -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                                            -- Soft delete
    CreatedBy		INT         		NOT NULL,						                                            -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                                            -- Data de criação
    ModifiedBy	    INT         		    NULL,						                                            -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                                            -- Data de modificação
	CONSTRAINT PK_InterventionAddresses PRIMARY KEY CLUSTERED (Id),		                                            -- PK
    CONSTRAINT FK_InterventionAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),					-- FK para tenant
    CONSTRAINT FK_InterventionAddresses_Intervention FOREIGN KEY (InterventionId, TenantId) REFERENCES dbo.Interventions(Id, TenantId),	-- FK para client
    CONSTRAINT FK_InterventionAddresses_AddressType FOREIGN KEY (AddressTypeId, TenantId) REFERENCES dbo.AddressTypes(Id, TenantId)	-- FK para tipo de endereço
);
GO
CREATE TABLE dbo.InterventionTeams (
    Id              INT IDENTITY(1,1)   NOT NULL,                                                                   -- Identificador, chave primária
    TenantId        INT                 NOT NULL,                                                                   -- Tenant dono
    InterventionId  INT                 NOT NULL,                                                                   -- Intervenção associada
    TeamId          INT                 NOT NULL,                                                                   -- Time associado à intervenção
    IsActive		BIT					NOT NULL DEFAULT 1,				                                            -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                                            -- Soft delete
    CreatedBy		INT         		NOT NULL,						                                            -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                                            -- Data de criação
    ModifiedBy	    INT         		    NULL,						                                            -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                                            -- Data de modificação
    CONSTRAINT PK_InterventionTeams PRIMARY KEY CLUSTERED (Id),		                                                -- PK
    CONSTRAINT UQ_InterventionTeams_Id_Tenant UNIQUE (Id, TenantId),                                                -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_InterventionTeams_Intervention FOREIGN KEY (InterventionId, TenantId) REFERENCES dbo.Interventions(Id, TenantId),
    CONSTRAINT FK_InterventionTeams_Team FOREIGN KEY (TeamId, TenantId) REFERENCES dbo.Teams(Id, TenantId)
);
GO
CREATE TABLE dbo.InterventionTeamVehicles (                                                                             -- Associação entre equipes de intervenção e veículos utilizados (uma equipe pode usar vários veículos e um veículo pode ser usado por várias equipes em intervenções diferentes)
    Id                  INT IDENTITY(1,1)   NOT NULL,                                                                   -- Identificador, chave primária
    TenantId            INT                 NOT NULL,                                                                   -- Tenant dono
    InterventionTeamId  INT                 NOT NULL,                                                                   -- Equipe associada à intervenção
    VehicleId           INT                 NOT NULL,                                                                   -- Veículo associado à intervenção
    IsActive		    BIT					NOT NULL DEFAULT 1,				                                            -- Flag de ativo
    IsDeleted		    BIT					NOT NULL DEFAULT 0,				                                            -- Soft delete
    CreatedBy		    INT         		NOT NULL,						                                            -- Usuário criador
    CreatedAt		    DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                                            -- Data de criação
    ModifiedBy	        INT         		    NULL,						                                            -- Usuário modificador
    ModifiedAt		    DATETIME2(7)			NULL,						                                            -- Data de modificação
    CONSTRAINT PK_InterventionTeamVehicles PRIMARY KEY CLUSTERED (Id),                                                  -- PK
    CONSTRAINT FK_InterventionTeamVehicles_InterventionTeam FOREIGN KEY (InterventionTeamId) REFERENCES dbo.InterventionTeams(Id),  -- FK para equipe de intervenção
    CONSTRAINT FK_InterventionTeamVehicles_Vehicle FOREIGN KEY (VehicleId, TenantId) REFERENCES dbo.Vehicles(Id, TenantId)          -- FK para veículo (tenant-safe
);
GO
CREATE TABLE dbo.InterventionTeamEquipments (                                                                           -- Associação entre equipes de intervenção e equipamentos utilizados (uma equipe pode usar vários equipamentos e um equipamento pode ser usado por várias equipes em intervenções diferentes)
    Id                  INT IDENTITY(1,1)   NOT NULL,                                                                   -- Identificador, chave primária
    TenantId            INT                 NOT NULL,                                                                   -- Tenant dono
    InterventionTeamId  INT                 NOT NULL,                                                                   -- Equipe associada à intervenção
    EquipmentId         INT                 NOT NULL,                                                                   -- Equipamento associado à intervenção
    IsActive		    BIT					NOT NULL DEFAULT 1,				                                            -- Flag de ativo
    IsDeleted		    BIT					NOT NULL DEFAULT 0,				                                            -- Soft delete
    CreatedBy		    INT         		NOT NULL,						                                            -- Usuário criador
    CreatedAt		    DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                                            -- Data de criação
    ModifiedBy	        INT         		    NULL,						                                            -- Usuário modificador
    ModifiedAt		    DATETIME2(7)			NULL,						                                            -- Data de modificação
    CONSTRAINT PK_InterventionTeamEquipments PRIMARY KEY CLUSTERED (Id),                                                -- PK
    CONSTRAINT FK_InterventionTeamEquipments_InterventionTeam FOREIGN KEY (InterventionTeamId) REFERENCES dbo.InterventionTeams(Id),    -- FK para equipe de intervenção
    CONSTRAINT FK_InterventionTeamEquipments_Equipment FOREIGN KEY (EquipmentId, TenantId) REFERENCES dbo.Equipments(Id, TenantId)      -- FK para equipamento (tenant-safe
);


CREATE UNIQUE INDEX UX_Clients_Tenant_Name_Active					ON dbo.Clients (TenantId, Name) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_ClientAddresses_Primary						ON dbo.ClientAddresses (ClientId) WHERE IsPrimary = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_ClientFiscalData_Client_Active               ON dbo.ClientFiscalData (ClientId) WHERE IsActive = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_TeamMembers_Tenant_TaxNumber                 ON dbo.TeamMembers (TenantId, TaxNumber) WHERE TaxNumber IS NOT NULL AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_TenantFiscalData_NIF_Active                  ON dbo.TenantFiscalData (NIF) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_ClientFiscalData_TaxNumber_Active            ON dbo.ClientFiscalData (TaxNumber) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_TenantFiscalData_Active                      ON dbo.TenantFiscalData(TenantId) WHERE IsActive = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_Subscriptions_Active                         ON dbo.Subscriptions(TenantId) WHERE IsActive = 1 AND IsDeleted = 0;


CREATE NONCLUSTERED INDEX IX_TenantContacts_TenantId				ON dbo.TenantContacts (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Clients_Tenant_Active					ON dbo.Clients (TenantId, Name) INCLUDE (Email, Phone) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientContacts_ClientId				ON dbo.ClientContacts (ClientId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientAddresses_ClientId				ON dbo.ClientAddresses (ClientId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientFiscalData_ClientId				ON dbo.ClientFiscalData (ClientId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMembers_FunctionId                 ON dbo.TeamMembers(FunctionId);
CREATE NONCLUSTERED INDEX IX_TeamMembers_Tenant_TaxNumber           ON dbo.TeamMembers (TenantId, TaxNumber) WHERE TaxNumber IS NOT NULL AND IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMemberContacts_TeamMemberId		ON dbo.TeamMemberContacts (TeamMemberId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMemberAddresses_TeamMemberId		ON dbo.TeamMemberAddresses (TeamMemberId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Interventions_Tenant_Date              ON dbo.Interventions (TenantId, StartDateTime, InterventionStatusId) INCLUDE (ClientId, TeamMemberId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionContacts_InterventionId	ON dbo.InterventionContacts (InterventionId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionAddresses_InterventionId	ON dbo.InterventionAddresses (InterventionId) INCLUDE (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionStatus_Tenant              ON dbo.InterventionStatus(TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_UserRoles_UserId						ON dbo.UserRoles (UserId) INCLUDE (TenantId, RoleId); 
CREATE NONCLUSTERED INDEX IX_UserRoles_RoleId						ON dbo.UserRoles (RoleId) INCLUDE (TenantId, UserId); 
CREATE NONCLUSTERED INDEX IX_RolePermissions_RoleId					ON dbo.RolePermissions (RoleId) INCLUDE (TenantId, ResourceId, ActionId); 
CREATE NONCLUSTERED INDEX IX_RolePermissions_ResourceId				ON dbo.RolePermissions (ResourceId) INCLUDE (TenantId, RoleId, ActionId);
CREATE NONCLUSTERED INDEX IX_RolePermissions_ActionId				ON dbo.RolePermissions (ActionId) INCLUDE (TenantId, RoleId, ResourceId);
CREATE NONCLUSTERED INDEX IX_Users_Login				            ON dbo.Users (TenantId, NormalizedEmail) INCLUDE (Id, IsActive) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Subscriptions_PlanId                   ON dbo.Subscriptions(PlanId);

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
        ISNULL(CAST(SESSION_CONTEXT(N'IsSuperAdmin') AS INT), 0) = 1
        OR
        -- Tenant ID deve corresponder
        -- SESSION_CONTEXT retorna VARBINARY, então convertemos de volta para INT
        (
            SESSION_CONTEXT(N'TenantId') IS NOT NULL
            AND @TenantId = CONVERT(INT, SESSION_CONTEXT(N'TenantId'))
        );
GO

CREATE SECURITY POLICY dbo.TenantSecurityPolicy												                -- Criação da policy de RLS
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users,					                -- Filtro por TenantId, aplica RLS em Users
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles,					                -- Aplica RLS em Roles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles,				                -- Aplica RLS em UserRoles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RolePermissions,			                -- Aplica RLS em RolePermissions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.JwtKeys,					                -- Aplica RLS em JwtKeys
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Subscriptions,			                -- Aplica RLS em Subscriptions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantContacts,			                -- Aplica RLS em TenantContacts
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.AddressTypes,			                -- Aplica RLS em AddressTypes
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantAddresses,			                -- Aplica RLS em TenantAddresses
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantFiscalData,		                -- Aplica RLS em TenantFiscalData
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Functions,				                -- Aplica RLS em Functions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Clients,					                -- Aplica RLS em Clients
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientContacts,			                -- Aplica RLS em ClientContacts
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientAddresses,			                -- Aplica RLS em ClientAddresses
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembers,				                -- Aplica RLS em TeamMembers
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMemberContacts,		                -- Aplica RLS em TeamMemberContacts
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMemberAddresses,                     -- Aplica RLS em TeamMemberAddresses
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionContacts,                    -- Aplica RLS em InterventionContacts
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionAddresses,                   -- Aplica RLS em InterventionAddresses
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionStatus,                      -- Aplica RLS em InterventionStatus
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Interventions,                           -- Aplica RLS em Interventions
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Vehicles,                                -- Aplica RLS em Vehicles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.EquipmentTypes,                          -- Aplica RLS em EquipmentTypes
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Equipments,                              -- Aplica RLS em Equipments
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RefreshTokens,                           -- Aplica RLS em RefreshTokens
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Teams,                                   -- Aplica RLS em Teams
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembersTeams,                        -- Aplica RLS em TeamMembersTeams
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionTeams,                       -- Aplica RLS em InterventionTeams
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionTeamVehicles,                -- Aplica RLS em InterventionTeamVehicles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionTeamEquipments,              -- Aplica RLS em InterventionTeamEquipments

ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users AFTER INSERT,	                    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users AFTER UPDATE,	                    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users BEFORE DELETE,                     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Roles AFTER INSERT,                      -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Roles AFTER UPDATE,                      -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Roles BEFORE DELETE,                     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserRoles AFTER INSERT,                  -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserRoles AFTER UPDATE,                  -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserRoles BEFORE DELETE,                 -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.RolePermissions AFTER INSERT,            -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.RolePermissions AFTER UPDATE,            -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.RolePermissions BEFORE DELETE,           -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.JwtKeys AFTER INSERT,                    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.JwtKeys AFTER UPDATE,                    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.JwtKeys BEFORE DELETE,                   -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Subscriptions AFTER INSERT,              -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Subscriptions AFTER UPDATE,              -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Subscriptions BEFORE DELETE,             -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantContacts AFTER INSERT,             -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantContacts AFTER UPDATE,             -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantContacts BEFORE DELETE,            -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.AddressTypes AFTER INSERT,               -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.AddressTypes AFTER UPDATE,               -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.AddressTypes BEFORE DELETE,              -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantAddresses AFTER INSERT,            -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantAddresses AFTER UPDATE,            -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantAddresses BEFORE DELETE,           -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantFiscalData AFTER INSERT,           -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantFiscalData AFTER UPDATE,           -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TenantFiscalData BEFORE DELETE,          -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Functions AFTER INSERT,                  -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Functions AFTER UPDATE,                  -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Functions BEFORE DELETE,                 -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Clients AFTER INSERT,                    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Clients AFTER UPDATE,                    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Clients BEFORE DELETE,                   -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientContacts AFTER INSERT,             -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientContacts AFTER UPDATE,             -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientContacts BEFORE DELETE,            -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientAddresses AFTER INSERT,            -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientAddresses AFTER UPDATE,            -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.ClientAddresses BEFORE DELETE,           -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembers AFTER INSERT,                -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembers AFTER UPDATE,                -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembers BEFORE DELETE,               -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberContacts AFTER INSERT,         -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberContacts AFTER UPDATE,         -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberContacts BEFORE DELETE,        -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberAddresses AFTER INSERT,        -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberAddresses AFTER UPDATE,        -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMemberAddresses BEFORE DELETE,       -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionContacts AFTER INSERT,       -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionContacts AFTER UPDATE,       -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionContacts BEFORE DELETE,      -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionAddresses AFTER INSERT,      -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionAddresses AFTER UPDATE,      -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionAddresses BEFORE DELETE,     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionStatus AFTER INSERT,         -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionStatus AFTER UPDATE,         -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionStatus BEFORE DELETE,        -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Interventions AFTER INSERT,              -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Interventions AFTER UPDATE,              -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Interventions BEFORE DELETE,             -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Vehicles AFTER INSERT,                   -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Vehicles AFTER UPDATE,                   -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Vehicles BEFORE DELETE,                  -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.EquipmentTypes AFTER INSERT,             -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.EquipmentTypes AFTER UPDATE,             -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.EquipmentTypes BEFORE DELETE,            -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Equipments AFTER INSERT,                 -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Equipments AFTER UPDATE,                 -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Equipments BEFORE DELETE,                -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Teams AFTER INSERT,                      -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Teams AFTER UPDATE,                      -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Teams BEFORE DELETE,                     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembersTeams AFTER INSERT,           -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembersTeams AFTER UPDATE,           -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.TeamMembersTeams BEFORE DELETE,          -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeams AFTER INSERT,          -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeams AFTER UPDATE,          -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeams BEFORE DELETE,         -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeamVehicles AFTER INSERT,   -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeamVehicles AFTER UPDATE,   -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeamVehicles BEFORE DELETE,  -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeamEquipments AFTER INSERT, -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeamEquipments AFTER UPDATE, -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.InterventionTeamEquipments BEFORE DELETE,-- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RefreshTokens AFTER INSERT,               -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RefreshTokens AFTER UPDATE,               -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RefreshTokens BEFORE DELETE               -- Bloqueia DELETE fora do Tenant

WITH (STATE = ON);																		                -- Ativa a policy
GO
