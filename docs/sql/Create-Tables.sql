/* =========================
   (GLOBAL)
   ========================= */

CREATE TABLE dbo.FileTypes (                                                            -- Catálogo global de tipos de arquivo
    Id                          INT IDENTITY(1,1)   NOT NULL,                           -- Identificador do tipo de arquivo, chave primária
    MimeType                    NVARCHAR(100)       NOT NULL,                           -- Tipo MIME do arquivo (image/jpeg, application/pdf, etc.)
    Extension                   NVARCHAR(20)        NOT NULL,                           -- Extensão do arquivo (jpg, png, pdf, docx, etc.)
    IsActive                    BIT                 NOT NULL DEFAULT 1,                 -- Flag de ativo
    IsDeleted                   BIT                 NOT NULL DEFAULT 0,                 -- Soft delete
    CreatedAt                   DATETIME2(7)        NOT NULL DEFAULT SYSDATETIME(),     -- Data de criação
    CONSTRAINT PK_FileTypes PRIMARY KEY CLUSTERED (Id),                                 -- PK
    CONSTRAINT UQ_FileTypes_Mime UNIQUE (MimeType),                                     -- Garantir que cada MIME type é único
    CONSTRAINT CK_FileTypes_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)) -- Garantir que um tipo de arquivo não pode ser ativo e deletado ao mesmo tempo
);
GO

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
    ModifiedBy	                INT         		    NULL,						-- Usuário modificador
    ModifiedAt	                DATETIME2(7)		    NULL,						-- Data de modificação
    CONSTRAINT PK_Plans PRIMARY KEY CLUSTERED (Id),                                 -- PK
    CONSTRAINT CK_Plans_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)) -- Garantir que um plano não pode ser ativo e deletado ao mesmo tempo
);
GO
/* =========================
   PLAN FILE RULES
   ========================= */

CREATE TABLE dbo.PlanFileRules (                                                                -- Regras de arquivo por plano (limites de upload, etc.)
    Id                          INT IDENTITY(1,1)   NOT NULL,                                   -- PK interna
    PlanId                      INT                 NOT NULL,                                   -- FK para plano global
    FileTypeId                  INT                 NOT NULL,                                   -- FK para tipo de arquivo
    MaxFileSizeMB               INT                 NOT NULL CHECK (MaxFileSizeMB > 0),         -- Tamanho máximo do arquivo em MB
    IsActive                    BIT                 NOT NULL DEFAULT 1,                         -- Flag de ativo
    IsDeleted                   BIT                 NOT NULL DEFAULT 0,                         -- Soft delete
    CreatedBy                   INT                 NOT NULL,                                   -- Usuário criador
    CreatedAt                   DATETIME2(7)        NOT NULL DEFAULT SYSDATETIME(),             -- Data de criação
    ModifiedBy                  INT                     NULL,                                   -- Usuário modificador
    ModifiedAt                  DATETIME2(7)            NULL,                                   -- Data de modificação
    CONSTRAINT PK_PlanFileRules PRIMARY KEY CLUSTERED (Id),                                     -- PK
    CONSTRAINT UQ_PlanFileRules UNIQUE (PlanId, FileTypeId),                                    -- Garantir que só pode haver uma regra por tipo de arquivo para cada plano
    CONSTRAINT CK_PlanFileRules_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),    -- Garantir que uma regra não pode ser ativa e deletada ao mesmo tempo
    CONSTRAINT FK_PlanFileRules_Plan FOREIGN KEY (PlanId) REFERENCES dbo.Plans(Id),             -- FK para plano global
    CONSTRAINT FK_PlanFileRules_FileType FOREIGN KEY (FileTypeId) REFERENCES dbo.FileTypes(Id)  -- FK para tipo de arquivo
);
GO

/* =========================
   CORE MULTI-TENANT TABLES
   ========================= */

CREATE TABLE dbo.Tenants (											                    -- Tabela principal de tenants
    Id			        INT IDENTITY(1,1)	NOT NULL,				                    -- Identificador único do tenant, chave primária
    TenantType          INT                 NOT NULL,				                    -- Tipo do tenant (residencial, comercial, etc.)
    Name	            NVARCHAR(200)		NOT NULL,				                    -- Razão social
    Email		        NVARCHAR(255)		NOT NULL,				                    -- Email do contato
    Website		        NVARCHAR(255)		    NULL,				                    -- Website do tenant
    Origin		        NVARCHAR(255)		    NULL,				                    -- Origem do tenant (para tracking de marketing)
    ConsentType         INT		            NOT NULL,				                    -- Tipo de consentimento (LGPD, GDPR, etc.)
    Consent		        BIT					NOT NULL DEFAULT 0,		                    -- Consentimento LGPD/GDPR
    ConsentDate	        DATETIME2(7)		NOT NULL,				                    -- Data do consentimento
    RevokedConsentDate  DATETIME2(7)		    NULL,				                    -- Data de revogação do consentimento
    PrivacyPolicy       BIT		            NOT NULL DEFAULT 0,                         -- Aceitou a política de privacidade
    Remarks		        NVARCHAR(1000)		    NULL,				                    -- Observações gerais sobre o tenant
    IsActive	        BIT					NOT NULL DEFAULT 1,                         -- Flag de ativo
    IsDeleted	        BIT					NOT NULL DEFAULT 0,                         -- Soft delete
    CreatedBy	        INT         		NOT NULL,						            -- Usuário criador
    CreatedAt	        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	            -- Data de criação
    ModifiedBy	        INT         		NULL,							            -- Usuário modificador
    ModifiedAt	        DATETIME2(7)		NULL,							            -- Data de modificação
	CONSTRAINT PK_Tenants PRIMARY KEY CLUSTERED (Id),                                   -- PK
    CONSTRAINT CK_Tenants_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1))   -- Garantir que um tenant não pode ser ativo e deletado ao mesmo tempo
);
GO
CREATE TABLE dbo.StatusTypes (                                                          -- Tipos de status (Intervention, Equipment, Vehicle)
    Id			INT IDENTITY(1,1)	NOT NULL,                                           -- Identificador, chave primária
    TenantId	INT					NOT NULL,                                           -- Tenant dono
    Name	    NVARCHAR(200)		NOT NULL,                                           -- Nome do status (Agendada, Em andamento, Concluída, Cancelada)
    Description NVARCHAR(500)		NOT NULL,                                           -- Descrição do status
    IsActive	BIT					NOT NULL DEFAULT 1,                                 -- Flag de ativo
    IsDeleted	BIT					NOT NULL DEFAULT 0,                                 -- Soft delete
    CreatedBy	INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt	DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	INT         		NULL,							                    -- Usuário modificador
    ModifiedAt	DATETIME2(7)		NULL,							                    -- Data de modificação
	CONSTRAINT PK_StatusTypes PRIMARY KEY CLUSTERED (Id),                               -- PK
    CONSTRAINT CK_StatusTypes_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_StatusTypes_Id_Tenant UNIQUE (Id, TenantId),                          -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_StatusTypes_Tenant_Name UNIQUE (TenantId, Name),                      -- Tipo de endereço único por tenant
    CONSTRAINT FK_StatusTypes_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)  -- FK para tenant
);
GO
CREATE TABLE dbo.Status (                                                               -- Status das intervenções
    Id			    INT IDENTITY(1,1)	NOT NULL,                                       -- Identificador, chave primária
    TenantId	    INT					NOT NULL,                                       -- Tenant dono
    StatusTypeId	INT					NOT NULL,                                       -- Tipo do status (FK para StatusTypes)
    Name	        NVARCHAR(200)		NOT NULL,                                       -- Nome do status (Agendada, Em andamento, Concluída, Cancelada)
    Description     NVARCHAR(500)		NOT NULL,                                       -- Descrição do status
    IsActive	    BIT					NOT NULL DEFAULT 1,                             -- Flag de ativo
    IsDeleted	    BIT					NOT NULL DEFAULT 0,                             -- Soft delete
    CreatedBy	    INT         		NOT NULL,						                -- Usuário criador
    CreatedAt	    DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                -- Data de criação
    ModifiedBy	    INT         		NULL,							                -- Usuário modificador
    ModifiedAt	    DATETIME2(7)		NULL,							                -- Data de modificação
	CONSTRAINT PK_Status PRIMARY KEY CLUSTERED (Id),                                    -- PK
    CONSTRAINT CK_Status_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),   -- Garantir que um status não pode ser ativo e deletado ao mesmo tempo
    CONSTRAINT UQ_Status_Id_Tenant UNIQUE (Id, TenantId),                               -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_Status_Tenant_Name UNIQUE (TenantId, StatusTypeId, Name),             -- Status único por tipo e tenant
    CONSTRAINT FK_Status_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),      -- FK para tenant
    CONSTRAINT FK_Status_StatusType FOREIGN KEY (StatusTypeId, TenantId) REFERENCES dbo.StatusTypes(Id, TenantId)   -- FK para tipo de status
);
GO
CREATE TABLE dbo.TenantContacts (									        -- Contatos do tenant
    Id			        INT IDENTITY(1,1)	NOT NULL,						-- Identificador do contato, chave primária
    TenantId	        INT					NOT NULL,						-- Tenant dono do contato
    Name		        NVARCHAR(150)		NOT NULL,						-- Nome do contato
    Email		        NVARCHAR(255)		NOT NULL,						-- Email do contato
    Phone		        NVARCHAR(30)		    NULL,				        -- Telefone
    PhoneIsWhatsapp     BIT					    NULL DEFAULT 0,		        -- O telefone é WhatsApp?
    CellPhone	        NVARCHAR(30)		    NULL,				        -- Telemóvel
    CellPhoneIsWhatsapp BIT					    NULL DEFAULT 0,		        -- O telemóvel é WhatsApp?
    IsPrimary	        BIT					NOT NULL DEFAULT 0,				-- Contato principal
    IsActive	        BIT					NOT NULL DEFAULT 1,				-- Flag de ativo
    IsDeleted	        BIT					NOT NULL DEFAULT 0,				-- Soft delete
    CreatedBy	        INT         		NOT NULL,						-- Usuário criador
    CreatedAt	        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	-- Data de criação
    ModifiedBy	        INT         		    NULL,						    -- Usuário modificador
    ModifiedAt	        DATETIME2(7)			NULL,						-- Data de modificação
	CONSTRAINT PK_TenantContacts PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_TenantContacts_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_AddressesType_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    Remarks         NVARCHAR(500)           NULL,                                           -- Observações adicionais sobre o endereço  
    IsPrimary		BIT					NOT NULL DEFAULT 0,				                    -- Endereço principal
    IsActive		BIT					NOT NULL DEFAULT 1,				                    -- Flag de ativo
    IsDeleted		BIT					NOT NULL DEFAULT 0,				                    -- Soft delete
    CreatedBy		INT         		NOT NULL,						                    -- Usuário criador
    CreatedAt		DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	                    -- Data de criação
    ModifiedBy	    INT         		    NULL,						                    -- Usuário modificador
    ModifiedAt		DATETIME2(7)			NULL,						                    -- Data de modificação
	CONSTRAINT PK_TenantAddresses PRIMARY KEY CLUSTERED (Id),		                        -- PK
    CONSTRAINT CK_TenantAddresses_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT FK_TenantAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_TenantAddresses_AddressType FOREIGN KEY (AddressTypeId, TenantId) REFERENCES dbo.AddressTypes(Id, TenantId),	-- FK para tipo de endereço
    CONSTRAINT UQ_TenantAddresses_Id_Tenant UNIQUE (Id, TenantId)                                                               -- Garantir que o Id é único dentro do tenant (para FKs compostas)
);
GO
CREATE TABLE dbo.TenantFiscalData (										                    -- Dados fiscais do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,						                    -- Identificador fiscal, chave primária
    TenantId		INT					NOT NULL,						                    -- Tenant dono
    TaxNumber   	CHAR(9)				NOT NULL,						                    -- NIF português
    VatNumber		NVARCHAR(20)		NOT NULL,						                    -- VAT calculado
    IbanNumber		NVARCHAR(34)		NULL,						                        -- IBAN para faturamento
    DocumentNumber	NVARCHAR(50)		NULL,						                        -- Número de identificação (passaporte, etc.)
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
    CONSTRAINT CK_TenantFiscalData_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_Subscriptions_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT FK_Subscriptions_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),       -- Tenant-safe
    CONSTRAINT FK_Subscriptions_Plan FOREIGN KEY (PlanId) REFERENCES dbo.Plans(Id),             -- Global plan
    CONSTRAINT UQ_Subscriptions_TenantId_Id UNIQUE (TenantId, Id)                                              -- Chave alternativa (TenantId, Id) p/ FKs compostas
        
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
    CONSTRAINT CK_Users_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_Users_Id_Tenant UNIQUE (Id, TenantId),
    CONSTRAINT UQ_Users_Tenant_Email UNIQUE (TenantId, Email),			            -- Nome único por tenant
    CONSTRAINT UQ_Users_Tenant_NormalizedEmail UNIQUE (TenantId, NormalizedEmail),	-- Email único por tenant
    CONSTRAINT FK_Users_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.UserPreferences (                                                              -- Preferências do usuário (tema, notificações, etc.)
    Id                              INT IDENTITY(1,1)   NOT NULL,                               -- Identificador, chave primária
    TenantId                        INT                 NOT NULL,                               -- Tenant dono
    UserId                          INT                 NOT NULL,                               -- Usuário dono
    Appearance                      NVARCHAR(10)        NOT NULL DEFAULT ('light'),             -- Tema (light/dark)
	CurrencyCode 					NVARCHAR(3) 		NOT NULL DEFAULT ('EUR'),               -- Código de moeda ISO (EUR, USD, etc.)
    Locale                          NVARCHAR(10)        NOT NULL DEFAULT ('pt-PT'),             -- Localização (pt-PT, en-US, es-ES, etc.)
    Timezone                        NVARCHAR(100)       NOT NULL DEFAULT ('Europe/Lisbon'),     -- Timezone IANA
    DateFormat                      NVARCHAR(20)        NOT NULL DEFAULT ('DD-MM-YYYY'),        -- Formato de data
    TimeFormat                      NVARCHAR(10)        NOT NULL DEFAULT ('24h'),               -- Formato de hora (24h/12h)
    DayStart                        TIME(0)             NOT NULL DEFAULT ('09:00'),             -- Início do dia para notificações, relatórios e trabalho (usado para calcular "hoje", "amanhã", etc.)
    DayEnd                          TIME(0)             NOT NULL DEFAULT ('18:00'),             -- Início do dia para notificações, relatórios e trabalho (usado para calcular "hoje", "amanhã", etc.)
    EmailNewsletter                 BIT                 NOT NULL DEFAULT (0),                   -- Receber newsletter por email
    EmailWeeklyReport               BIT                 NOT NULL DEFAULT (0),                   -- Receber relatório semanal por email
    EmailApproval                   BIT                 NOT NULL DEFAULT (0),                   -- Receber emails de aprovação (intervenções, equipamentos, etc.)
    EmailAlerts                     BIT                 NOT NULL DEFAULT (1),                   -- Receber alertas críticos por email (intervenções atrasadas, falhas, etc.)
    EmailReminders                  BIT                 NOT NULL DEFAULT (1),                   -- Receber lembretes por email (intervenções agendadas para o dia, etc.)
    EmailPlanner                    BIT                 NOT NULL DEFAULT (1),                   -- Receber email com planejamento diário/semanal
    IsActive                        BIT                 NOT NULL DEFAULT (1),                   -- Flag de ativo
    IsDeleted                       BIT                 NOT NULL DEFAULT (0),                   -- Soft delete
    CreatedBy                       INT                 NOT NULL,                               -- Usuário criador
    CreatedAt                       DATETIME2(7)        NOT NULL DEFAULT (SYSDATETIME()),       -- Data de criação
    ModifiedBy                      INT                     NULL,                               -- Usuário modificador
    ModifiedAt                      DATETIME2(7)            NULL,                               -- Data de modificação
    CONSTRAINT PK_UserPreferences PRIMARY KEY CLUSTERED (Id),                                   -- PK
    CONSTRAINT CK_UserPreferences_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),  -- Garantir que as preferências não podem ser ativas e deletadas ao mesmo tempo
    CONSTRAINT CK_UserPreferences_Appearance CHECK (Appearance IN ('light', 'dark')),           -- Aparência limitada a light/dark
    CONSTRAINT CK_UserPreferences_TimeFormat CHECK (TimeFormat IN ('24h', '12h')),              -- Formato de hora limitado a 24h/12h
    CONSTRAINT FK_UserPreferences_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),     -- FK para tenant
    CONSTRAINT FK_UserPreferences_User FOREIGN KEY (UserId, TenantId) REFERENCES dbo.Users(Id, TenantId)  -- FK para usuário
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
    CONSTRAINT CK_Roles_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_Roles_Id_Tenant UNIQUE (Id, TenantId),
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
    CONSTRAINT CK_Resources_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_Actions_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT FK_RolePermissions_Role FOREIGN KEY (RoleId, TenantId) REFERENCES dbo.Roles(Id, TenantId),  -- FK role
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
    CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId, TenantId) REFERENCES dbo.Users(Id, TenantId), -- FK usuário
    CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId, TenantId) REFERENCES dbo.Roles(Id, TenantId)  -- FK role
        
);
GO
CREATE TABLE dbo.RefreshTokens (
    Id 						INT IDENTITY(1,1)	NOT NULL,
    TenantId 				INT					NOT NULL,
	UserId 					INT					NOT NULL,
	Token					NVARCHAR(MAX)		NOT NULL,
	ExpiresAt				DATETIME2(7) 		NOT	NULL,
    RevokedAt 				DATETIME2(7) 			NULL,
	RevokedBy				INT 					NULL,
    CreatedBy		        INT		            NOT NULL,
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),
    ModifiedBy	            INT         		    NULL,
    ModifiedAt		        DATETIME2(7)			NULL,
    CONSTRAINT PK_RefreshTokens PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_RefreshTokens_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
	CONSTRAINT FK_RefreshTokens_User FOREIGN KEY (UserId, TenantId) REFERENCES dbo.Users(Id, TenantId)
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
    CONSTRAINT CK_JwtKeys_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_JwtKeys_KeyId UNIQUE (TenantId, KeyId),
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
    CONSTRAINT PK_Job PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Job_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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

GO
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
    CONSTRAINT CK_Functions_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_Functions_Id_Tenant UNIQUE (Id, TenantId),                            -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_Functions_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)    -- FK para tenant
)
GO
CREATE TABLE dbo.Clients (                                                              -- Clientes do tenant
    Id				    INT IDENTITY(1,1)	NOT NULL,						            -- Identificador do cliente, chave primária
    TenantId		    INT					NOT NULL,						            -- Tenant dono do cliente
    ClientType          INT				    NOT NULL,						            -- Tipo do cliente (1=Individual, 2=Empresa, etc.)
    Origin              INT                 NOT NULL DEFAULT 1,						    -- Origem do cliente (Instagram, Facebook, Outros.)
    Name			    NVARCHAR(150)		NOT NULL,						            -- Nome do cliente
    Phone			    NVARCHAR(50)			NULL,						            -- Telefone do cliente
    Email			    NVARCHAR(255)			NULL,						            -- Email do cliente
    Website			    NVARCHAR(255)			NULL,						            -- Website do cliente
    UrlImage    	    NVARCHAR(255)			NULL,						            -- URL da imagem/avatar do cliente
    Score			    INT						NULL,						            -- Score de crédito ou satisfação do cliente
    Consent		        BIT				    NOT NULL DEFAULT 0,		                    -- Consentimento LGPD/GDPR
    ConsentDate	        DATETIME2(7)	    NOT NULL,				                    -- Data do consentimento
    RevokedConsentDate  DATETIME2(7)	        NULL,				                    -- Data de revogação do consentimento
    PrivacyPolicy       BIT		            NOT NULL DEFAULT 0,                         -- Aceitou a política de privacidade
    Remarks             NVARCHAR(500)           NULL,						            -- Observações adicionais sobre o cliente
    IsActive		    BIT					NOT NULL DEFAULT 1,                         -- Flag de ativo
    IsDeleted		    BIT					NOT NULL DEFAULT 0,                         -- Soft delete
    CreatedBy		    INT         		NOT NULL,						            -- Usuário criador
    CreatedAt		    DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	            -- Data de criação
    ModifiedBy	        INT         		    NULL,						            -- Usuário modificador
    ModifiedAt		    DATETIME2(7)			NULL,                                   -- Data de modificação
	CONSTRAINT PK_Clients PRIMARY KEY CLUSTERED (Id),                                   -- PK
    CONSTRAINT CK_Clients_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),  -- Garantir que um cliente não pode ser ativo e deletado ao mesmo tempo
    CONSTRAINT UQ_Clients_Id_Tenant UNIQUE (Id, TenantId),                              -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_Clients_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)      -- FK para tenant
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
    CONSTRAINT CK_ClientContacts_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_ClientAddresses_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_ClientAddresses_Id_Tenant UNIQUE (Id, TenantId),
    CONSTRAINT FK_ClientAddresses_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),	-- FK para tenant
    CONSTRAINT FK_ClientAddresses_Client FOREIGN KEY (ClientId, TenantId) REFERENCES dbo.Clients(Id, TenantId),	-- FK para client
    CONSTRAINT FK_ClientAddresses_AddressType FOREIGN KEY (AddressTypeId, TenantId) REFERENCES dbo.AddressTypes(Id, TenantId)	-- FK para tipo de endereço
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
    CONSTRAINT CK_Teams_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_TeamMembers_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_TeamMemberContacts_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_TeamMemberAddresses_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
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
    CONSTRAINT CK_TeamMembersTeams_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_TeamMembersTeams UNIQUE (TenantId, TeamId, TeamMemberId),
    CONSTRAINT FK_TeamMembersTeams_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
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
    CONSTRAINT CK_EquipmentTypes_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_EquipmentTypes_Id_Tenant UNIQUE (Id, TenantId),                           -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_EquipmentTypes_Tenant_Name UNIQUE (TenantId, Name),                       -- Tipo de endereço único por tenant
    CONSTRAINT FK_EquipmentTypes_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id)	-- FK para tenant
);
GO
CREATE TABLE dbo.Equipments (
    Id				    INT IDENTITY(1,1)	NOT NULL,						            -- Identificador, chave primária
	TenantId		    INT					NOT NULL,						            -- Tenant dono
    EquipmentTypeId	    INT					NOT NULL,						            -- Tipo do equipamento (FK para EquipmentTypes)
	StatusId		    INT					NOT NULL,						            -- Status do equipamento
    Name			    NVARCHAR(150)		NOT NULL,						            -- Nome do equipamento
    SerialNumber	    NVARCHAR(100)			NULL,						            -- Número de série do equipamento
    IsActive		    BIT					NOT NULL DEFAULT 1,				            -- Flag de ativo
    IsDeleted		    BIT					NOT NULL DEFAULT 0,				            -- Soft delete
    CreatedBy		    INT         		NOT NULL,						            -- Usuário criador
    CreatedAt		    DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),	            -- Data de criação
    ModifiedBy	        INT         		    NULL,						            -- Usuário modificador
    ModifiedAt		    DATETIME2(7)			NULL,						            -- Data de modificação
	CONSTRAINT PK_Equipments PRIMARY KEY CLUSTERED (Id),                                -- PK
    CONSTRAINT CK_Equipments_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_Equipments_Id_Tenant UNIQUE (Id, TenantId),							-- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT FK_Equipments_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),  -- FK para tenant
    CONSTRAINT FK_Equipments_Status FOREIGN KEY (StatusId, TenantId) REFERENCES dbo.Status(Id, TenantId),   -- FK para status (tenant-safe)
    CONSTRAINT FK_Equipments_EquipmentType FOREIGN KEY (EquipmentTypeId, TenantId) REFERENCES dbo.EquipmentTypes(Id, TenantId)	                                -- FK para tipo de equipamento (tenant-safe)
);
GO
CREATE TABLE dbo.Vehicles (                                                                 -- Veículos do tenant
    Id				INT IDENTITY(1,1)	NOT NULL,                                           -- Identificador, chave primária
    TenantId        INT					NOT NULL,                                           -- Tenant dono
    StatusId        INT                 NOT NULL,                                           -- Estado do veículo (Novo, Usado, Em Manutenção)
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
    CONSTRAINT CK_Vehicles_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_Vehicles_Id_Tenant UNIQUE (Id, TenantId),                                 -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_Vehicles_Tenant_Plate UNIQUE (TenantId, Plate),                           -- Placa única por tenant
	CONSTRAINT FK_Vehicles_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),       -- FK para tenant
    CONSTRAINT FK_Vehicles_Status FOREIGN KEY (StatusId, TenantId) REFERENCES dbo.Status(Id, TenantId)   -- FK para status (tenant-safe)
);
GO
CREATE TABLE dbo.Interventions (                                                                                -- Intervenções agendadas
    Id				        INT IDENTITY(1,1)	NOT NULL,                                                       -- Identificador da intervenção, chave primária
	TenantId                INT					NOT NULL,                                                       -- Tenant dono da intervenção
    ClientId		        INT					NOT NULL,                                                       -- Cliente associado à intervenção
    StatusId                INT				    NOT NULL,                                                       -- Status da intervenção
	Title			        NVARCHAR(200)		NOT NULL,                                                       -- Título da intervenção
    Description		        NVARCHAR(2000)		NOT NULL,                                                       -- Descrição detalhada da intervenção
    StartDateTime	        DATETIME2(7)		NOT NULL,                                                       -- Data e hora de início da intervenção
    EndDateTime		        DATETIME2(7)			NULL,                                                       -- Data e hora de término (pode ser atualizado após conclusão)
    EstimatedValue	        DECIMAL(10,2)		NOT NULL CHECK (EstimatedValue >= 0),                           -- Valor estimado da intervenção
	RealValue		        DECIMAL(10,2)			NULL,                                                       -- Valor real da intervenção (pode ser atualizado após conclusão)
    IsActive		        BIT					NOT NULL DEFAULT 1,								                -- Flag de ativo
    IsDeleted		        BIT					NOT NULL DEFAULT 0,								                -- Soft delete
    CreatedBy		        INT         		NOT NULL,										                -- Usuário criador
    CreatedAt		        DATETIME2(7)		NOT NULL DEFAULT SYSDATETIME(),					                -- Data de criação
    ModifiedBy	            INT         		    NULL,										                -- Usuário modificador
    ModifiedAt		        DATETIME2(7)			NULL,										                -- Data de modificação
	CONSTRAINT PK_Interventions PRIMARY KEY CLUSTERED (Id),                                                     -- PK
    CONSTRAINT CK_Interventions_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_Interventions_Id_Tenant UNIQUE (Id, TenantId),                                                -- Garantir que o Id é único dentro do tenant (para FKs compostas)
	CONSTRAINT FK_Interventions_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),                      -- FK para tenant
	CONSTRAINT FK_Interventions_Clients FOREIGN KEY (ClientId, TenantId) REFERENCES dbo.Clients(Id, TenantId),  -- FK para client
    CONSTRAINT FK_Interventions_Status FOREIGN KEY (StatusId, TenantId) REFERENCES dbo.Status(Id, TenantId),    -- FK para status da intervenção
	CONSTRAINT CK_Interventions_EndDateTime	CHECK ( EndDateTime IS NULL OR EndDateTime >= StartDateTime)        -- Garantir que a data de término seja posterior à data de início (ou nula)
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
    CONSTRAINT CK_InterventionContacts_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_InterventionContacts_Id_Tenant UNIQUE (Id, TenantId),
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
    CONSTRAINT CK_InterventionAddresses_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_InterventionAddresses_Id_Tenant UNIQUE (Id, TenantId),
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
    CONSTRAINT CK_InterventionTeams_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT UQ_InterventionTeams UNIQUE (TenantId, InterventionId, TeamId),
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
    CONSTRAINT CK_InterventionTeamVehicles_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),                 -- Garantir que um registro não pode ser ativo e deletado ao mesmo tempo
    CONSTRAINT FK_InterventionTeamVehicles_InterventionTeam FOREIGN KEY (InterventionTeamId, TenantId) REFERENCES dbo.InterventionTeams(Id, TenantId),  -- FK para equipe de intervenção
    CONSTRAINT FK_InterventionTeamVehicles_Vehicle FOREIGN KEY (VehicleId, TenantId) REFERENCES dbo.Vehicles(Id, TenantId)                              -- FK para veículo (tenant-safe)
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
    CONSTRAINT CK_InterventionTeamEquipments_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),
    CONSTRAINT FK_InterventionTeamEquipments_InterventionTeam FOREIGN KEY (InterventionTeamId, TenantId) REFERENCES dbo.InterventionTeams(Id, TenantId),    -- FK para equipe de intervenção
    CONSTRAINT FK_InterventionTeamEquipments_Equipment FOREIGN KEY (EquipmentId, TenantId) REFERENCES dbo.Equipments(Id, TenantId)      -- FK para equipamento (tenant-safe
);
GO
CREATE TABLE dbo.AttachmentCategories (                                                             -- Categorias para anexos (fotos, documentos, etc.) relacionados a clientes, intervenções, equipamentos, etc. Permite organizar os anexos em categorias definidas pelo tenant.
    Id                  INT IDENTITY(1,1)   NOT NULL,                                               -- Identificador da categoria de anexo, chave primária
    TenantId            INT                 NOT NULL,                                               -- Tenant dono da categoria de anexo
    Name                NVARCHAR(100)       NOT NULL,                                               -- Nome da categoria de anexo
    Description         NVARCHAR(300)           NULL,                                               -- Descrição da categoria de anexo
    DisplayOrder        INT                 NOT NULL DEFAULT 0,                                     -- Ordem de exibição para categorizar os anexos (pode ser usado para ordenar as categorias na UI)
    IsSystem            BIT                 NOT NULL DEFAULT 0,                                     -- Indica se a categoria é do sistema (não pode ser deletada ou desativada)
    IsActive            BIT                 NOT NULL DEFAULT 1,                                     -- Flag de ativo
    IsDeleted           BIT                 NOT NULL DEFAULT 0,                                     -- Soft delete
    CreatedBy           INT                 NOT NULL,                                               -- Usuário criador
    CreatedAt           DATETIME2(7)        NOT NULL DEFAULT SYSDATETIME(),                         -- Data de criação
    ModifiedBy          INT                     NULL,                                               -- Usuário modificador
    ModifiedAt          DATETIME2(7)            NULL,                                               -- Data de modificação
    CONSTRAINT PK_AttachmentCategories PRIMARY KEY CLUSTERED (Id),                                  -- PK
    CONSTRAINT UQ_AttachmentCategories_Id_Tenant UNIQUE (Id, TenantId),                             -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT UQ_AttachmentCategories_Name_Tenant UNIQUE (TenantId, Name),                         -- Nome único por tenant
    CONSTRAINT FK_AttachmentCategories_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),    -- FK para tenant
    CONSTRAINT CK_AttachmentCategories_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1))  -- Garantir que um registro não pode ser ativo e deletado ao mesmo tempo
);
GO
CREATE TABLE dbo.InterventionAttachments (                                                                  -- Anexos relacionados a intervenções (fotos, documentos, etc.) Permite associar arquivos diretamente a uma intervenção específica e organizá-los em categorias definidas pelo tenant.
    Id                      INT IDENTITY(1,1)   NOT NULL,                                                   -- Identificador do anexo, chave primária
    TenantId                INT                 NOT NULL,                                                   -- Tenant dono do anexo
    FileTypeId              INT                 NOT NULL,                                                   -- Tipo do arquivo (FK para FileTypes, permite classificar os anexos por tipo de arquivo, ex: "Imagem", "Documento", "PDF", etc.)                    
    InterventionId          INT                 NOT NULL,                                                   -- Intervenção associada ao anexo (FK para Interventions, permite associar fotos, documentos, etc. diretamente a uma intervenção específica)
    AttachmentCategoryId    INT                 NOT NULL,                                                   -- Categoria do anexo (FK para AttachmentCategories, permite organizar os anexos em categorias definidas pelo tenant, ex: "Fotos", "Documentos", "Relatórios", etc.)
    PublicId                UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID(),                                   -- Identificador público do anexo (pode ser usado para acessar o anexo sem expor o Id interno, ex: em URLs de download)
    S3Key                   NVARCHAR(500)       NOT NULL,                                                   -- Chave do arquivo no S3 (pode ser usado para acessar o arquivo no bucket, ex: "tenantid/interventionid/filename.jpg")
    FileName                NVARCHAR(255)       NOT NULL,                                                   -- Nome original do arquivo (pode ser usado para exibir o nome do arquivo na UI ou para download)
    FileSizeBytes           BIGINT              NOT NULL CHECK (FileSizeBytes > 0),                         -- Tamanho do arquivo em bytes (deve ser maior que 0)
    DisplayOrder            INT                 NOT NULL DEFAULT 0,                                         -- Ordem de exibição dos anexos dentro da intervenção (pode ser usado para ordenar os anexos na UI)
    IsPrimary               BIT                 NOT NULL DEFAULT 0,                                         -- Indica se é o anexo principal (ex: foto principal da intervenção)
    IsActive                BIT                 NOT NULL DEFAULT 1,                                         -- Flag de ativo
    IsDeleted               BIT                 NOT NULL DEFAULT 0,                                         -- Soft delete
    CreatedBy               INT                 NOT NULL,                                                   -- Usuário criador
    CreatedAt               DATETIME2(7)        NOT NULL DEFAULT SYSDATETIME(),                             -- Data de criação
    ModifiedBy              INT                     NULL,                                                   -- Usuário modificador
    ModifiedAt              DATETIME2(7)            NULL,                                                   -- Data de modificação
    CONSTRAINT PK_InterventionAttachments PRIMARY KEY CLUSTERED (Id),                                       -- PK
    CONSTRAINT UQ_InterventionAttachments_Id_Tenant UNIQUE (Id, TenantId),                                  -- Garantir que o Id é único dentro do tenant (para FKs compostas)
    CONSTRAINT CK_InterventionAttachments_Active_Deleted CHECK (NOT (IsActive = 1 AND IsDeleted = 1)),      -- Garantir que um registro não pode ser ativo e deletado ao mesmo tempo
    CONSTRAINT FK_InterventionAttachments_Tenant FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),         -- FK para tenant
    CONSTRAINT FK_InterventionAttachments_FileType FOREIGN KEY (FileTypeId) REFERENCES dbo.FileTypes(Id),   -- FK para tipo de arquivo (pode ser global, sem TenantId, se os tipos de arquivo forem compartilhados entre tenants)
    CONSTRAINT FK_InterventionAttachments_Intervention FOREIGN KEY (InterventionId, TenantId) REFERENCES dbo.Interventions(Id, TenantId),           -- FK para intervenção (tenant-safe)
    CONSTRAINT FK_InterventionAttachments_Category FOREIGN KEY (AttachmentCategoryId, TenantId) REFERENCES dbo.AttachmentCategories(Id, TenantId)   -- FK para categoria de anexo (tenant-safe) 
);

CREATE UNIQUE INDEX UX_Clients_Tenant_Name_Active					ON dbo.Clients (TenantId, Name) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_ClientAddresses_Primary						ON dbo.ClientAddresses (ClientId, TenantId) WHERE IsPrimary = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_TeamMembers_Tenant_TaxNumber                 ON dbo.TeamMembers (TenantId, TaxNumber) WHERE TaxNumber IS NOT NULL AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_TenantFiscalData_NIF_Active                  ON dbo.TenantFiscalData (TenantId, TaxNumber) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_TenantFiscalData_Active                      ON dbo.TenantFiscalData(TenantId) WHERE IsActive = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_Subscriptions_Active                         ON dbo.Subscriptions(TenantId) WHERE IsActive = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_Clients_Email_Active                         ON dbo.Clients (TenantId, Email) WHERE Email IS NOT NULL AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_TenantContacts_Primary                       ON dbo.TenantContacts (TenantId) WHERE IsPrimary = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_InterventionAttachments_Primary              ON dbo.InterventionAttachments (TenantId, InterventionId) WHERE IsPrimary = 1 AND IsDeleted = 0 AND IsActive = 1;
CREATE UNIQUE INDEX UX_InterventionAttachments_PublicId             ON dbo.InterventionAttachments (TenantId, PublicId);
CREATE UNIQUE INDEX UX_InterventionAddresses_Primary                ON dbo.InterventionAddresses (TenantId, InterventionId) WHERE IsPrimary = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_InterventionTeamEquipments_Unique            ON dbo.InterventionTeamEquipments (TenantId, InterventionTeamId, EquipmentId) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_InterventionTeamVehicles_Unique              ON dbo.InterventionTeamVehicles (TenantId, InterventionTeamId, VehicleId) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_InterventionAttachments_S3Key                ON dbo.InterventionAttachments (TenantId, S3Key) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_JwtKeys_Active                               ON dbo.JwtKeys (TenantId) WHERE IsActive = 1 AND IsDeleted = 0;
CREATE UNIQUE INDEX UX_UserPreferences_Tenant_User_Active           ON dbo.UserPreferences (TenantId, UserId) WHERE IsDeleted = 0;

CREATE NONCLUSTERED INDEX IX_TenantContacts_TenantId				        ON dbo.TenantContacts (TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Clients_Tenant_Active					        ON dbo.Clients (TenantId, Name) INCLUDE (Email, Phone) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientContacts_ClientId				        ON dbo.ClientContacts (TenantId, ClientId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_ClientAddresses_ClientId				        ON dbo.ClientAddresses (TenantId, ClientId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMembers_FunctionId                         ON dbo.TeamMembers(TenantId, FunctionId)  WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMembers_Tenant_TaxNumber                   ON dbo.TeamMembers (TenantId, TaxNumber) WHERE TaxNumber IS NOT NULL AND IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMemberContacts_TeamMemberId		        ON dbo.TeamMemberContacts (TenantId, TeamMemberId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_TeamMemberAddresses_TeamMemberId		        ON dbo.TeamMemberAddresses (TenantId, TeamMemberId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionContacts_InterventionId	        ON dbo.InterventionContacts (TenantId, InterventionId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Interventions_Tenant_Date                      ON dbo.Interventions (TenantId, StartDateTime) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionAddresses_InterventionId	        ON dbo.InterventionAddresses (TenantId, InterventionId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Interventions_ClientId                         ON dbo.Interventions (TenantId, ClientId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_StatusTypes_Tenant						        ON dbo.StatusTypes(TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Status_Tenant                                  ON dbo.Status(TenantId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_UserRoles_UserId						        ON dbo.UserRoles (TenantId, UserId) INCLUDE (RoleId);
CREATE NONCLUSTERED INDEX IX_UserRoles_RoleId						        ON dbo.UserRoles (TenantId, RoleId) INCLUDE (UserId);
CREATE NONCLUSTERED INDEX IX_RolePermissions_RoleId					        ON dbo.RolePermissions (TenantId, RoleId) INCLUDE (ResourceId, ActionId); 
CREATE NONCLUSTERED INDEX IX_RolePermissions_ResourceId				        ON dbo.RolePermissions (TenantId, ResourceId) INCLUDE (RoleId, ActionId);
CREATE NONCLUSTERED INDEX IX_RolePermissions_ActionId				        ON dbo.RolePermissions (TenantId, ActionId) INCLUDE (RoleId, ResourceId);
CREATE NONCLUSTERED INDEX IX_Users_Login				                    ON dbo.Users (TenantId, NormalizedEmail) INCLUDE (Id, IsActive) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_Subscriptions_PlanId                           ON dbo.Subscriptions(TenantId, PlanId)  WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_RefreshTokens_User_Active                      ON dbo.RefreshTokens (TenantId, UserId) WHERE RevokedAt IS NULL;
CREATE NONCLUSTERED INDEX IX_Interventions_Dashboard                        ON dbo.Interventions (TenantId, StatusId, StartDateTime) INCLUDE (ClientId, EstimatedValue) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_RefreshTokens_ExpiresAt                        ON dbo.RefreshTokens (TenantId, ExpiresAt) WHERE RevokedAt IS NULL;
CREATE NONCLUSTERED INDEX IX_InterventionTeams_InterventionId               ON dbo.InterventionTeams (TenantId, InterventionId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionTeamVehicles_InterventionTeamId    ON dbo.InterventionTeamVehicles (TenantId, InterventionTeamId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionTeamEquipments_InterventionTeamId  ON dbo.InterventionTeamEquipments (TenantId, InterventionTeamId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_AttachmentCategories_Tenant_Display            ON dbo.AttachmentCategories (TenantId, DisplayOrder) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_AttachmentCategories_Tenant_Active             ON dbo.AttachmentCategories (TenantId) WHERE IsDeleted = 0 AND IsActive = 1;
CREATE NONCLUSTERED INDEX IX_InterventionAttachments_Tenant_Intervention    ON dbo.InterventionAttachments (TenantId, InterventionId) INCLUDE (AttachmentCategoryId, DisplayOrder, IsPrimary, FileTypeId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_InterventionAttachments_FileTypeId             ON dbo.InterventionAttachments (TenantId, FileTypeId) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_PlanFileRules_Plan                             ON dbo.PlanFileRules (PlanId) INCLUDE (FileTypeId, MaxFileSizeMB) WHERE IsDeleted = 0;
CREATE NONCLUSTERED INDEX IX_PlanFileRules_FileType                         ON dbo.PlanFileRules (FileTypeId) INCLUDE (PlanId, MaxFileSizeMB) WHERE IsDeleted = 0;

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
            AND @TenantId = CAST(SESSION_CONTEXT(N'TenantId') AS INT)
        );
GO

CREATE SECURITY POLICY dbo.TenantSecurityPolicy												                -- Criação da policy de RLS
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users,					                -- Filtro por TenantId, aplica RLS em Users
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles,					                -- Aplica RLS em Roles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles,				                -- Aplica RLS em UserRoles
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserPreferences,			                -- Aplica RLS em UserPreferences
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
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.StatusTypes,                             -- Aplica RLS em StatusTypes
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Status,                                  -- Aplica RLS em Status
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
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.AttachmentCategories,                    -- Aplica RLS em AttachmentCategories
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.InterventionAttachments,                 -- Aplica RLS em InterventionAttachments


ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users AFTER INSERT,	                    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users AFTER UPDATE,	                    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Users BEFORE DELETE,                     -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserPreferences AFTER INSERT,	        -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserPreferences AFTER UPDATE,	        -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.UserPreferences BEFORE DELETE,           -- Bloqueia DELETE fora do Tenant
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
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.StatusTypes AFTER INSERT,                -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.StatusTypes AFTER UPDATE,                -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.StatusTypes BEFORE DELETE,               -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Status AFTER INSERT,                     -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Status AFTER UPDATE,                     -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)	ON dbo.Status BEFORE DELETE,                    -- Bloqueia DELETE fora do Tenant
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
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.RefreshTokens AFTER INSERT,              -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.RefreshTokens AFTER UPDATE,              -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.RefreshTokens BEFORE DELETE,             -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.AttachmentCategories AFTER INSERT,       -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.AttachmentCategories AFTER UPDATE,       -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.AttachmentCategories BEFORE DELETE,      -- Bloqueia DELETE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.InterventionAttachments AFTER INSERT,    -- Bloqueia INSERT fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.InterventionAttachments AFTER UPDATE,    -- Bloqueia UPDATE fora do Tenant
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId)  ON dbo.InterventionAttachments BEFORE DELETE    -- Bloqueia DELETE fora do Tenant

WITH (STATE = ON);																		                -- Ativa a policy
GO
