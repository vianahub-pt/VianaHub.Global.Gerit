
GO
INSERT INTO dbo.AddressTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Morada residencial', N'Morada para habitação (moradia ou apartamento), usada como endereço principal de pessoas.'),
    (N'Morada comercial', N'Morada de um negócio/estabelecimento (loja, escritório) para atividade comercial e atendimento.'),
    (N'Morada industrial', N'Morada associada a atividade industrial (fábrica, unidade industrial, armazém/galpão).'),
    (N'Morada rural', N'Morada em área rural (quinta, herdade, sítio/fazenda) ligada a atividade agrícola ou residência isolada.'),
    (N'Morada de serviços públicos', N'Morada de entidades públicas (Câmara Municipal, Junta de Freguesia, Conservatória, repartições).'),
    (N'Morada de educação', N'Morada de instituições de ensino (escola, universidade, creche/jardim de infância).'),
    (N'Morada de saúde', N'Morada de serviços de saúde (hospital, centro de saúde, clínica, laboratório, farmácia).'),
    (N'Morada de alojamento/turismo', N'Morada de unidades de alojamento (hotel, hostel, turismo rural, AL – Alojamento Local).'),
    (N'Morada logística/distribuição', N'Morada dedicada a logística (centro de distribuição, plataforma logística, entreposto, armazém).'),
    (N'Morada postal alternativa', N'Morada para receção de correspondência/entregas fora do domicílio (Apartado CTT, ponto CTT/Pickup, portaria).')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.AddressTypes a WHERE a.Name = v.Name
);
GO
INSERT INTO dbo.FileTypes (MimeType, Extension, CreatedBy)
SELECT v.MimeType, v.Extension, 1
FROM (VALUES
    ('image/jpeg', 'jpg'),
    ('image/png', 'png'),
    ('image/gif', 'gif'),
    ('image/webp', 'webp'),
    ('image/svg+xml', 'svg'),
    ('application/pdf', 'pdf'),
    ('application/msword', 'doc'),
    ('application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'docx'),
    ('application/vnd.ms-excel', 'xls'),
    ('application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'xlsx'),
    ('application/vnd.ms-powerpoint', 'ppt'),
    ('application/vnd.openxmlformats-officedocument.presentationml.presentation', 'pptx'),
    ('text/plain', 'txt'),
    ('text/csv', 'csv'),
    ('application/json', 'json'),
    ('application/xml', 'xml'),
    ('application/zip', 'zip'),
    ('application/x-rar-compressed', 'rar'),
    ('application/x-7z-compressed', '7z'),
    ('application/octet-stream', 'bin')
) AS v(MimeType, Extension)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.FileTypes f WHERE f.MimeType = v.MimeType
);
GO
INSERT INTO dbo.ClientTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Consumidor final', N'Pessoa física que compra para uso pessoal/familiar.'),
    (N'Pequenas e Médias Empresas (PME)', N'Negócios de menor dimensão com compras regulares ou pontuais.'),
    (N'Grandes empresas / Corporates', N'Empresas com grande volume de compras e processos formais.'),
    (N'Setor público / Administração', N'Entidades governamentais e serviços públicos.'),
    (N'Instituições e ONG', N'Organizações sem fins lucrativos e fundações.'),
    (N'Cliente internacional / expatriado', N'Compradores não nacionais ou que trazem necessidades transfronteiriças.'),
    (N'Cliente digital / online-only', N'Compra exclusivamente por canais digitais.'),
    (N'Cliente recorrente / subscritor', N'Tem relacionamento continuado (assinaturas/contratos).'),
    (N'Cliente VIP / alto valor', N'Pouco número mas muito valor por cliente; exige serviço premium.'),
    (N'Parceiro / revendedor', N'Intermediário que vende ou recomenda produtos/serviços.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.ClientTypes c WHERE c.Name = v.Name
);
GO
INSERT INTO dbo.ConsentTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'PrivacyPolicy', N'Consentimento referente à política de privacidade (LGPD/GDPR).'),
    (N'Marketing', N'Autoriza o envio de comunicações de marketing e promoções por e-mail.'),
    (N'TermsOfService', N'Aceitação dos termos de uso e condições do serviço.'),
    (N'DataProcessing', N'Consentimento para processamento de dados pessoais para fins específicos.'),
    (N'Cookies', N'Permissão para uso de cookies (persistentes e de sessão) e tracking no site.'),
    (N'EmailConsent', N'Opt-in para recebimento de comunicações transacionais e newsletters por e-mail.'),
    (N'SMSConsent', N'Autorização para envio de SMS (alertas e promoções).'),
    (N'ThirdPartySharing', N'Consentimento para compartilhamento de dados com terceiros e parceiros autorizados.'),
    (N'Analytics', N'Permissão para coleta de dados analíticos e telemetria (ex.: Google Analytics).'),
    (N'Personalization', N'Consentimento para personalização de conteúdo e recomendações com base em preferências do usuário.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.ConsentTypes c WHERE c.Name = v.Name
);
GO
INSERT INTO dbo.OriginTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Website', N'Cliente originado através do website institucional ou landing pages.'),
    (N'Google', N'Cliente proveniente de campanhas pagas no Google (Search, Display, YouTube).'),
    (N'Facebook', N'Cliente proveniente de campanhas pagas no Facebook/Instagram.'),
    (N'Instagram', N'Cliente que chegou através de conteúdo orgânico no Instagram.'),
    (N'LinkedIn', N'Cliente proveniente do LinkedIn (orgânico ou campanhas).'),
    (N'Recomendação', N'Cliente indicado por outro cliente ou parceiro.'),
    (N'Parceria', N'Cliente adquirido através de parceiros comerciais ou estratégicos.'),
    (N'Evento', N'Cliente captado em eventos, feiras ou conferências.'),
    (N'Email marketing', N'Cliente proveniente de campanhas de email marketing.'),
    (N'WhatsApp', N'Cliente que iniciou contacto via WhatsApp.'),
    (N'Contacto direto', N'Cliente que entrou em contacto direto (telefone, visita, etc.).'),
    (N'Marketplace', N'Cliente vindo de plataformas externas ou marketplaces.'),
    (N'App mobile', N'Cliente originado através da aplicação mobile.'),
    (N'Prospecção', N'Cliente adquirido através de abordagem ativa (cold calls, cold emails).')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.OriginTypes o WHERE o.Name = v.Name
);
GO
INSERT INTO dbo.StatusTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Agendada', N'Intervenção criada e agendada para uma data futura.'),
    (N'Confirmada', N'Intervenção confirmada com o cliente e pronta para execução.'),
    (N'Em deslocação', N'Equipa a caminho do local da intervenção.'),
    (N'Em andamento', N'Intervenção em execução no local.'),
    (N'Em pausa', N'Intervenção temporariamente pausada (ex: falta de material, condições externas).'),
    (N'A aguardar cliente', N'Intervenção parada à espera de ação ou resposta do cliente.'),
    (N'A aguardar material', N'Intervenção suspensa por falta de materiais ou equipamentos.'),
    (N'Reagendada', N'Intervenção reagendada para nova data.'),
    (N'Concluída', N'Intervenção finalizada com sucesso.'),
    (N'Concluída com pendências', N'Intervenção concluída, mas com itens pendentes a resolver.'),
    (N'Cancelada', N'Intervenção cancelada antes ou durante a execução.'),
    (N'Não realizada', N'Intervenção não realizada (ex: cliente ausente).'),
    (N'Em validação', N'Intervenção concluída aguardando validação interna ou do cliente.'),
    (N'Faturada', N'Intervenção já faturada ao cliente.'),
    (N'Arquivada', N'Intervenção encerrada e arquivada para histórico.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.StatusTypes s WHERE s.Name = v.Name
);
GO
INSERT INTO dbo.Plans (
    Name, Description, PricePerHour, PricePerDay, PricePerMonth, PricePerYear,
    Currency, MaxUsers, MaxPhotosPerInterventions, CreatedBy
)
SELECT *
FROM (VALUES
    (N'Free', N'Plano gratuito com funcionalidades básicas para testes e uso inicial.', NULL, NULL, 0.00, 0.00, N'EUR', 1, 10, 1),
    (N'Basic', N'Plano básico para pequenos negócios com funcionalidades essenciais.', NULL, NULL, 19.90, 199.00, N'EUR', 3, 50, 1),
    (N'Standard', N'Plano intermédio com mais capacidade e funcionalidades avançadas.', NULL, NULL, 49.90, 499.00, N'EUR', 10, 200, 1),
    (N'Professional', N'Plano avançado para equipas com maior volume de intervenções.', NULL, NULL, 99.90, 999.00, N'EUR', 25, 500, 1),
    (N'Enterprise', N'Plano completo para grandes empresas com necessidades complexas.', NULL, NULL, 199.90, 1999.00, N'EUR', 100, 2000, 1),
    (N'Pay-as-you-go Hourly', N'Plano baseado em consumo por hora.', 5.00, NULL, NULL, NULL, N'EUR', 10, 100, 1),
    (N'Pay-as-you-go Daily', N'Plano baseado em consumo por dia.', NULL, 25.00, NULL, NULL, N'EUR', 10, 100, 1)
) AS v(
    Name, Description, PricePerHour, PricePerDay, PricePerMonth, PricePerYear,
    Currency, MaxUsers, MaxPhotosPerInterventions, CreatedBy
)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Plans p WHERE p.Name = v.Name
);
GO
INSERT INTO dbo.PlanFileRules (PlanId, FileTypeId, MaxFileSizeMB, CreatedBy)
SELECT 
    p.Id,
    f.Id,
    CASE 
        WHEN p.Name = 'Tryal' THEN 2
        WHEN p.Name = 'Start' THEN 5
        WHEN p.Name = 'Basic' THEN 10
        WHEN p.Name = 'Essential' THEN 15
        WHEN p.Name = 'Standard' THEN 25
        WHEN p.Name = 'Business' THEN 50
        WHEN p.Name = 'Professional' THEN 75
        WHEN p.Name = 'Advanced' THEN 100
        WHEN p.Name = 'Premium' THEN 150
        WHEN p.Name = 'Unlimited' THEN 300
        WHEN p.Name = 'Free' THEN 2
        WHEN p.Name = 'Enterprise' THEN 500
        WHEN p.Name = 'Pay-as-you-go Hourly' THEN 50
        WHEN p.Name = 'Pay-as-you-go Daily' THEN 50
        ELSE 10
    END AS MaxFileSizeMB,
    1
FROM dbo.Plans p
CROSS JOIN dbo.FileTypes f
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.PlanFileRules r 
    WHERE r.PlanId = p.Id 
      AND r.FileTypeId = f.Id
);
GO
INSERT INTO dbo.Actions (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'GetAll', N'Obter todos os registros.'),
    (N'GetBy', N'Obter registro por identificador ou critério.'),
    (N'GetPaged', N'Obter lista paginada com filtros.'),
    (N'Create', N'Criar novo registro.'),
    (N'Update', N'Atualizar registro existente.'),
    (N'Activate', N'Ativar registro.'),
    (N'Deactivate', N'Desativar registro.'),
    (N'Delete', N'Excluir registro.'),
    (N'BulkUpload', N'Cadastro em massa de registros.'),
    (N'Execute', N'Executar ação específica.'),
    (N'GetActivate', N'Obter registros ativos.'),
    (N'GetExpiring', N'Obter registros expirando.'),
    (N'Cancel', N'Cancelar operação ou entidade.'),
    (N'Renew', N'Renovar contrato, assinatura ou entidade.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Actions a WHERE a.Name = v.Name
);
GO
INSERT INTO dbo.Resources (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Actions', N'Recursos relacionados às ações do sistema.'),
    (N'AddressTypes', N'Tipos de endereços disponíveis.'),
    (N'AttachmentCategories', N'Categorias de anexos.'),
    (N'ClientAddresses', N'Endereços dos clientes.'),
    (N'ClientCompanies', N'Dados de empresas clientes.'),
    (N'ClientConsents', N'Consentimentos dos clientes.'),
    (N'ClientContacts', N'Contatos dos clientes.'),
    (N'ClientFiscalData', N'Dados fiscais dos clientes.'),
    (N'ClientHierarchy', N'Hierarquia entre clientes.'),
    (N'ClientIndividuals', N'Dados de clientes individuais.'),
    (N'Clients', N'Clientes do sistema.'),
    (N'ClientTypes', N'Tipos de clientes.'),
    (N'ConsentTypes', N'Tipos de consentimento.'),
    (N'Equipments', N'Equipamentos.'),
    (N'EquipmentTypes', N'Tipos de equipamentos.'),
    (N'FileTypes', N'Tipos de arquivos.'),
    (N'Functions', N'Funções do sistema.'),
    (N'InterventionAddresses', N'Endereços das intervenções.'),
    (N'InterventionAttachments', N'Anexos das intervenções.'),
    (N'InterventionContacts', N'Contatos das intervenções.'),
    (N'Interventions', N'Intervenções realizadas.'),
    (N'InterventionTeamEquipments', N'Equipamentos das equipas nas intervenções.'),
    (N'InterventionTeams', N'Equipas das intervenções.'),
    (N'InterventionTeamVehicles', N'Veículos das equipas nas intervenções.'),
    (N'JobDefinitions', N'Definições de jobs/processos.'),
    (N'JwtKeys', N'Chaves JWT.'),
    (N'OriginTypes', N'Tipos de origem.'),
    (N'PlanFileRules', N'Regras de arquivos por plano.'),
    (N'Plans', N'Planos do sistema.'),
    (N'RefreshTokens', N'Tokens de atualização.'),
    (N'Resources', N'Recursos do sistema.'),
    (N'RolePermissions', N'Permissões associadas a roles.'),
    (N'Roles', N'Papéis/perfis de acesso.'),
    (N'Status', N'Status das entidades.'),
    (N'StatusTypes', N'Tipos de status.'),
    (N'Subscriptions', N'Assinaturas dos tenants.'),
    (N'TeamMemberAddresses', N'Endereços dos membros da equipa.'),
    (N'TeamMemberContacts', N'Contatos dos membros da equipa.'),
    (N'TeamMembers', N'Membros da equipa.'),
    (N'TeamMembersTeams', N'Relação entre membros e equipas.'),
    (N'Teams', N'Equipas.'),
    (N'TenantAddresses', N'Endereços dos tenants.'),
    (N'TenantContacts', N'Contatos dos tenants.'),
    (N'TenantFiscalData', N'Dados fiscais dos tenants.'),
    (N'Tenants', N'Tenants do sistema.'),
    (N'UserPreferences', N'Preferências dos usuários.'),
    (N'UserRoles', N'Papéis dos usuários.'),
    (N'Users', N'Usuários do sistema.'),
    (N'Vehicles', N'Veículos.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Resources r WHERE r.Name = v.Name
);
GO
INSERT INTO dbo.Tenants (
    TenantTypeId,
    OriginTypeId,
    ConsentTypeId,
    Name,
    Email,
    Website,
    UrlImage,
    Remarks,
    CreatedBy
)
SELECT *
FROM (VALUES
    (2, 1, 1, N'VianaHub Lda', N'contact@vianahub.pt', N'https://vianahub.pt', NULL, N'Tenant principal', 1),
    (2, 1, 1, N'Gerit Demo Lda', N'demo@gerit.pt', N'https://demo.gerit.pt', NULL, N'Ambiente de demonstração', 1),
    (2, 1, 1, N'Teste Lda', N'teste@teste.pt', NULL, NULL, N'Tenant para testes internos', 1)
) AS v(
    TenantTypeId,
    OriginTypeId,
    ConsentTypeId,
    Name,
    Email,
    Website,
    UrlImage,
    Remarks,
    CreatedBy
)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Tenants t WHERE t.Name = v.Name
);
GO
INSERT INTO dbo.Roles (
    TenantId,
    Name,
    Description,
    CreatedBy
)
SELECT 
    t.Id,
    r.Name,
    r.Description,
    1
FROM dbo.Tenants t
CROSS JOIN (VALUES
    (N'Admin', N'Acesso administrativo completo ao tenant.'),
    (N'BackOffice', N'Acesso a operações internas e administrativas.'),
    (N'Manager', N'Gestão de equipas, clientes e operações.'),
    (N'Operator', N'Execução de tarefas operacionais.'),
    (N'User', N'Acesso básico ao sistema.'),
    (N'SuperAdmin', N'Acesso total ao sistema e configurações avançadas.')
) AS r(Name, Description)
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Roles ro
    WHERE ro.TenantId = t.Id
      AND ro.Name = r.Name
);
GO
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
INSERT INTO dbo.Status (
    TenantId,
    StatusTypeId,
    Name,
    Description,
    CreatedBy
)
SELECT 
    t.Id,
    st.Id,
    st.Name,
    st.Description,
    1
FROM dbo.Tenants t
CROSS JOIN dbo.StatusTypes st
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Status s
    WHERE s.TenantId = t.Id
      AND s.StatusTypeId = st.Id
);
GO
INSERT INTO dbo.TenantContacts (
    TenantId,
    Name,
    Email,
    Phone,
    PhoneIsWhatsapp,
    CellPhone,
    CellPhoneIsWhatsapp,
    IsPrimary,
    CreatedBy
)
SELECT *
FROM (VALUES
    (1, N'João Silva', N'contact@vianahub.pt', N'+351210000001', 0, N'+351910000001', 1, 1, 1),
    (2, N'Maria Costa', N'demo@gerit.pt', N'+351210000002', 0, N'+351920000002', 1, 1, 1),
    (3, N'Carlos Teste', N'teste@teste.pt', NULL, 0, N'+351930000003', 1, 1, 1)
) AS v(
    TenantId,
    Name,
    Email,
    Phone,
    PhoneIsWhatsapp,
    CellPhone,
    CellPhoneIsWhatsapp,
    IsPrimary,
    CreatedBy
)
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.TenantContacts tc 
    WHERE tc.TenantId = v.TenantId 
      AND tc.Email = v.Email
);
GO
INSERT INTO dbo.TenantAddresses (
    TenantId,
    AddressTypeId,
    CountryCode,
    Street,
    Neighborhood,
    City,
    District,
    PostalCode,
    StreetNumber,
    Complement,
    Latitude,
    Longitude,
    Remarks,
    IsPrimary,
    CreatedBy
)
SELECT *
FROM (VALUES
    (1, 2, 'PT', N'Avenida da Liberdade', N'Santo António', N'Lisboa', N'Lisboa', N'1250-140', N'100', N'3º Andar', 38.7223, -9.1393, N'Sede principal', 1, 1),
    (2, 2, 'PT', N'Rua de Santa Catarina', N'Baixa', N'Porto', N'Porto', N'4000-447', N'200', NULL, 41.1496, -8.6110, N'Escritório demo', 1, 1),
    (3, 2, 'PT', N'Avenida Central', N'Sé', N'Braga', N'Braga', N'4700-000', N'50', NULL, 41.5454, -8.4265, N'Endereço de teste', 1, 1)
) AS v(
    TenantId,
    AddressTypeId,
    CountryCode,
    Street,
    Neighborhood,
    City,
    District,
    PostalCode,
    StreetNumber,
    Complement,
    Latitude,
    Longitude,
    Remarks,
    IsPrimary,
    CreatedBy
)
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.TenantAddresses ta
    WHERE ta.TenantId = v.TenantId
      AND ta.IsPrimary = 1
);
GO
INSERT INTO dbo.TenantFiscalData (
    TenantId,
    TaxNumber,
    VatNumber,
    FiscalCountry,
    IsVatRegistered,
    IBAN,
    FiscalEmail,
    CreatedBy
)
SELECT *
FROM (VALUES
    (1, N'507123456', N'PT507123456', 'PT', 1, N'PT50000201231234567890154', N'finance@vianahub.pt', 1),
    (2, N'508987654', N'PT508987654', 'PT', 1, N'PT50000201239876543210987', N'billing@gerit.pt', 1),
    (3, N'509111222', NULL, 'PT', 0, NULL, N'teste@teste.pt', 1)
) AS v(
    TenantId,
    TaxNumber,
    VatNumber,
    FiscalCountry,
    IsVatRegistered,
    IBAN,
    FiscalEmail,
    CreatedBy
)
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.TenantFiscalData f 
    WHERE f.TenantId = v.TenantId
);
GO
INSERT INTO dbo.Subscriptions (
    TenantId,
    PlanId,
    StripeId,
    CurrentPeriodStart,
    CurrentPeriodEnd,
    TrialStart,
    TrialEnd,
    CancelAtPeriodEnd,
    StripeCustomerId,
    CreatedBy
)
SELECT 
    t.Id,
    1,
    NULL,
    SYSDATETIME(),
    DATEADD(MONTH, 1, SYSDATETIME()),
    SYSDATETIME(),
    DATEADD(DAY, 30, SYSDATETIME()),
    0,
    NULL,
    1
FROM dbo.Tenants t
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.Subscriptions s 
    WHERE s.TenantId = t.Id
);
GO
INSERT INTO dbo.Clients (
    TenantId,
    ClientTypeId,
    OriginTypeId,
    Remarks,
    CreatedBy
)
SELECT 
    t.Id,
    v.ClientTypeId,
    1,
    v.Remarks,
    1
FROM dbo.Tenants t
CROSS JOIN (VALUES
    -- 👤 Humanos (4)
    (1, N'Cliente individual 1'),
    (1, N'Cliente individual 2'),
    (1, N'Cliente individual 3'),
    (1, N'Cliente individual 4'),

    -- 🏢 Empresas (12)
    (2, N'Empresa 1'),
    (2, N'Empresa 2'),
    (2, N'Empresa 3'),
    (2, N'Empresa 4'),
    (2, N'Empresa 5'),
    (2, N'Empresa 6'),
    (2, N'Empresa 7'),
    (2, N'Empresa 8'),
    (2, N'Empresa 9'),
    (2, N'Empresa 10'),
    (2, N'Empresa 11'),
    (2, N'Empresa 12')
) v(ClientTypeId, Remarks);
GO
INSERT INTO dbo.ClientIndividuals (
    ClientId,
    TenantId,
    FirstName,
    LastName,
    BirthDate,
    Gender,
    DocumentType,
    DocumentNumber,
    Nationality,
    CreatedBy
)
SELECT 
    c.Id,
    c.TenantId,

    -- Nome
    CASE rn
        WHEN 1 THEN N'João'
        WHEN 2 THEN N'Maria'
        WHEN 3 THEN N'Pedro'
        WHEN 4 THEN N'Ana'
    END,

    -- Apelido
    CASE rn
        WHEN 1 THEN N'Silva'
        WHEN 2 THEN N'Costa'
        WHEN 3 THEN N'Santos'
        WHEN 4 THEN N'Ferreira'
    END,

    -- Data nascimento
    DATEADD(YEAR, - (25 + rn), CAST('1990-01-01' AS DATE)),

    -- Gênero
    CASE rn
        WHEN 1 THEN N'Masculino'
        WHEN 2 THEN N'Feminino'
        WHEN 3 THEN N'Masculino'
        WHEN 4 THEN N'Feminino'
    END,

    -- Documento
    N'CC',

    -- Número documento (único)
    CONCAT('PT', c.TenantId, RIGHT('000000' + CAST(rn AS VARCHAR), 6)),

    -- Nacionalidade
    'PT',

    1
FROM (
    SELECT 
        c.*,
        ROW_NUMBER() OVER (
            PARTITION BY c.TenantId 
            ORDER BY c.Id
        ) AS rn
    FROM dbo.Clients c
    WHERE c.ClientTypeId = 1
) c
WHERE c.rn <= 4
AND NOT EXISTS (
    SELECT 1
    FROM dbo.ClientIndividuals ci
    WHERE ci.ClientId = c.Id
);
GO
INSERT INTO dbo.ClientCompanies (
    ClientId,
    TenantId,
    LegalName,
    TradeName,
    CompanyRegistration,
    CAE,
    NumberOfEmployees,
    LegalRepresentative,
    CreatedBy
)
SELECT 
    c.Id,
    c.TenantId,

    -- Nome da empresa (real PT)
    CASE rn
        WHEN 1 THEN N'EDP - Energias de Portugal, S.A.'
        WHEN 2 THEN N'Galp Energia, SGPS, S.A.'
        WHEN 3 THEN N'Sonae SGPS, S.A.'
        WHEN 4 THEN N'Jerónimo Martins, SGPS, S.A.'
        WHEN 5 THEN N'Mota-Engil, SGPS, S.A.'
        WHEN 6 THEN N'CTT - Correios de Portugal, S.A.'
        WHEN 7 THEN N'NOS, SGPS, S.A.'
        WHEN 8 THEN N'Altice Portugal, S.A.'
        WHEN 9 THEN N'Vodafone Portugal, S.A.'
        WHEN 10 THEN N'Siemens, S.A.'
        WHEN 11 THEN N'Deloitte, S.A.'
        WHEN 12 THEN N'Bosch Termotecnologia, S.A.'
    END,

    -- TradeName
    CASE rn
        WHEN 1 THEN N'EDP'
        WHEN 2 THEN N'Galp'
        WHEN 3 THEN N'Sonae'
        WHEN 4 THEN N'Jerónimo Martins'
        WHEN 5 THEN N'Mota-Engil'
        WHEN 6 THEN N'CTT'
        WHEN 7 THEN N'NOS'
        WHEN 8 THEN N'MEO'
        WHEN 9 THEN N'Vodafone'
        WHEN 10 THEN N'Siemens'
        WHEN 11 THEN N'Deloitte'
        WHEN 12 THEN N'Bosch'
    END,

    -- Registo comercial (fake consistente)
    CONCAT('REG-', c.TenantId, '-', RIGHT('000' + CAST(rn AS VARCHAR), 3)),

    -- CAE (exemplo genérico IT/serviços)
    '62010',

    -- Nº funcionários
    (10 * rn),

    -- Representante
    CASE rn % 4
        WHEN 1 THEN N'João Silva'
        WHEN 2 THEN N'Maria Costa'
        WHEN 3 THEN N'Pedro Santos'
        WHEN 0 THEN N'Ana Ferreira'
    END,

    1
FROM (
    SELECT 
        c.*,
        ROW_NUMBER() OVER (
            PARTITION BY c.TenantId 
            ORDER BY c.Id
        ) AS rn
    FROM dbo.Clients c
    WHERE c.ClientTypeId = 2
) c
WHERE c.rn <= 12
AND NOT EXISTS (
    SELECT 1
    FROM dbo.ClientCompanies cc
    WHERE cc.ClientId = c.Id
);
GO
DECLARE @Now DATETIME2 = SYSDATETIME();

INSERT INTO dbo.ClientAddresses (
    TenantId,
    ClientId,
    AddressTypeId,
    CountryCode,
    Street,
    Neighborhood,
    City,
    District,
    PostalCode,
    StreetNumber,
    Complement,
    Latitude,
    Longitude,
    Notes,
    IsPrimary,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedAt
)
SELECT 
    c.TenantId,
    c.Id,
    CASE 
        WHEN c.ClientTypeId = 1 THEN 1 -- Residencial
        ELSE 2                         -- Comercial
    END,
    'PT',
    CONCAT('Rua ', 
        CASE (c.Id % 5)
            WHEN 0 THEN 'das Flores'
            WHEN 1 THEN 'da Liberdade'
            WHEN 2 THEN 'do Comércio'
            WHEN 3 THEN '25 de Abril'
            ELSE 'das Oliveiras'
        END
    ),
    CONCAT('Zona ', (c.Id % 10) + 1),
    CASE (c.TenantId)
        WHEN 1 THEN 'Lisboa'
        WHEN 2 THEN 'Porto'
        ELSE 'Braga'
    END,
    CASE (c.TenantId)
        WHEN 1 THEN 'Lisboa'
        WHEN 2 THEN 'Porto'
        ELSE 'Braga'
    END,
    CONCAT(
        FORMAT((1000 + c.Id), '0000'),
        '-',
        FORMAT((100 + c.Id), '000')
    ),
    CAST((c.Id % 200 + 1) AS NVARCHAR),
    NULL,
    NULL,
    NULL,
    CONCAT('Endereço gerado automaticamente para cliente ', c.Id),
    1, -- IsPrimary
    1, -- IsActive
    0, -- IsDeleted
    1,
    @Now
FROM dbo.Clients c
WHERE c.IsDeleted = 0
AND NOT EXISTS (
    SELECT 1
    FROM dbo.ClientAddresses ca
    WHERE ca.TenantId = c.TenantId
      AND ca.ClientId = c.Id
      AND ca.IsPrimary = 1
      AND ca.IsDeleted = 0
);
GO
DECLARE @Now DATETIME2 = SYSDATETIME();
INSERT INTO dbo.ClientContacts (
    TenantId,
    ClientId,
    Name,
    PhoneNumber,
	CellPhoneNumber,
	IsWhatsapp,
	Email,
    IsPrimary,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedAt
)
SELECT 
    c.TenantId,
    c.Id,
    CASE 
        WHEN c.ClientTypeId = 1 
            THEN CONCAT(ci.FirstName, ' ', ci.LastName)
        ELSE cc.LegalName
    END,
    CONCAT('2', RIGHT('00000000' + CAST(c.Id AS VARCHAR), 8)),
	CONCAT('9', RIGHT('00000000' + CAST(c.Id AS VARCHAR), 8)),
	0,
    LOWER(
        CONCAT(
            'client', c.Id,
            '@tenant', c.TenantId,
            '.pt'
        )
    ),
    1, -- IsPrimary
    1,
    0,
    1,
    @Now
FROM dbo.Clients c
LEFT JOIN dbo.ClientIndividuals ci
    ON ci.ClientId = c.Id AND ci.TenantId = c.TenantId
LEFT JOIN dbo.ClientCompanies cc
    ON cc.ClientId = c.Id AND cc.TenantId = c.TenantId
WHERE c.IsDeleted = 0
AND NOT EXISTS (
    SELECT 1
    FROM dbo.ClientContacts existing
    WHERE existing.TenantId = c.TenantId
      AND existing.ClientId = c.Id
      AND existing.IsPrimary = 1
      AND existing.IsDeleted = 0
);
GO
DECLARE @Now DATETIME2 = SYSDATETIME();
INSERT INTO dbo.ClientIndividualFiscalData (
    TenantId,
    ClientIndividualId,
    TaxNumber,
    VatNumber,
    FiscalCountry,
    IsVatRegistered,
    IBAN,
    FiscalEmail,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedAt
)
SELECT
    ci.TenantId,
    ci.Id,
    CONCAT('1', RIGHT('00000000' + CAST(ci.Id + (ci.TenantId * 1000) AS VARCHAR), 8)),
    NULL, -- VatNumber
    'PT',
    0,
    CONCAT('PT50', RIGHT('000000000000000000000000' + CAST(ci.Id AS VARCHAR), 21)),
    LOWER(
        CONCAT(
            ci.FirstName, '.', ci.LastName,
            '@fiscal.pt'
        )
    ),
    1,
    0,
    1,
    @Now
FROM dbo.ClientIndividuals ci
WHERE ci.IsDeleted = 0
AND NOT EXISTS (
    SELECT 1
    FROM dbo.ClientIndividualFiscalData f
    WHERE f.ClientIndividualId = ci.Id
      AND f.TenantId = ci.TenantId
);
GO
DECLARE @Now DATETIME2 = SYSDATETIME();
INSERT INTO dbo.ClientCompanyFiscalData (
    TenantId,
    ClientCompanyId,
    TaxNumber,
    VatNumber,
    FiscalCountry,
    IsVatRegistered,
    IBAN,
    FiscalEmail,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedAt
)
SELECT
    cc.TenantId,
    cc.Id,
    CONCAT('5', RIGHT('00000000' + CAST(cc.Id + (cc.TenantId * 1000) AS VARCHAR), 8)),
    CONCAT('PT', CONCAT('5', RIGHT('00000000' + CAST(cc.Id + (cc.TenantId * 1000) AS VARCHAR), 8))),
    'PT',
    1, -- empresas normalmente com IVA
    CONCAT('PT50', RIGHT('000000000000000000000000' + CAST(cc.Id AS VARCHAR), 21)),
    LOWER(
        CONCAT(
            REPLACE(REPLACE(cc.TradeName, ' ', ''), '.', ''),
            '@fiscal.pt'
        )
    ),
    1,
    0,
    1,
    @Now
FROM dbo.ClientCompanies cc
WHERE cc.IsDeleted = 0
AND NOT EXISTS (
    SELECT 1
    FROM dbo.ClientCompanyFiscalData f
    WHERE f.ClientCompanyId = cc.Id
      AND f.TenantId = cc.TenantId
);
GO
DECLARE @Now DATETIME2 = SYSDATETIME();
INSERT INTO dbo.ClientHierarchy (
    TenantId,
    ParentClientId,
    ChildClientId,
    RelationshipType,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedAt
)
SELECT
    cParent.TenantId,
    cParent.Id AS ParentClientId,
    cChild.Id AS ChildClientId,
    CASE 
        WHEN cChild.Id % 3 = 0 THEN 2  -- algumas subsidiárias
        ELSE 1                         -- maioria filiais
    END,
    1,
    0,
    1,
    @Now
FROM dbo.Clients cParent
JOIN dbo.Clients cChild 
    ON cParent.TenantId = cChild.TenantId
WHERE 
    cParent.ClientTypeId = 2 -- empresa
    AND cChild.ClientTypeId = 2
    AND cParent.Id = (
        SELECT MIN(Id)
        FROM dbo.Clients
        WHERE TenantId = cParent.TenantId
          AND ClientTypeId = 2
    )
    AND cChild.Id <> cParent.Id
    AND NOT EXISTS (
        SELECT 1
        FROM dbo.ClientHierarchy h
        WHERE h.TenantId = cParent.TenantId
          AND h.ParentClientId = cParent.Id
          AND h.ChildClientId = cChild.Id
    );
GO
DECLARE @Now DATETIME2 = SYSDATETIME();
INSERT INTO dbo.ClientConsents (
    TenantId,
    ClientId,
    ConsentTypeId,
    Granted,
    GrantedDate,
    Origin,
    IpAddress,
    UserAgent,
    CreatedBy,
    CreatedAt
)
SELECT 
    c.TenantId,
    c.Id AS ClientId,
    ct.Id AS ConsentTypeId,
    CASE 
        WHEN ct.Id = 2 THEN  -- Marketing
            CASE WHEN c.Id % 2 = 0 THEN 1 ELSE 0 END
        ELSE 1
    END AS Granted,
    @Now,
    'Web',
    '192.168.1.' + CAST(c.Id AS VARCHAR),
    'Mozilla/5.0 (Windows NT 10.0; Win64; x64)',
    1,
    @Now
FROM dbo.Clients c
CROSS JOIN dbo.ConsentTypes ct
WHERE 
    c.IsDeleted = 0
    AND NOT EXISTS (
        SELECT 1
        FROM dbo.ClientConsents cc
        WHERE cc.TenantId = c.TenantId
          AND cc.ClientId = c.Id
          AND cc.ConsentTypeId = ct.Id
    );
GO
DECLARE @Now DATETIME2 = SYSDATETIME();
WITH Seasons AS (
    SELECT 1 AS Id, 'Primavera' AS Name, 'Equipa responsável pelo período da Primavera' AS Description
    UNION ALL
    SELECT 2, 'Verão', 'Equipa responsável pelo período do Verão'
    UNION ALL
    SELECT 3, 'Outono', 'Equipa responsável pelo período do Outono'
    UNION ALL
    SELECT 4, 'Inverno', 'Equipa responsável pelo período do Inverno'
)
INSERT INTO dbo.Teams (
    TenantId,
    Name,
    Description,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedAt
)
SELECT 
    t.Id AS TenantId,
    s.Name,
    s.Description,
    1,
    0,
    1,
    @Now
FROM dbo.Tenants t
CROSS JOIN Seasons s
WHERE 
    t.IsDeleted = 0
    AND NOT EXISTS (
        SELECT 1
        FROM dbo.Teams tm
        WHERE tm.TenantId = t.Id
          AND tm.Name = s.Name
    );
GO
INSERT INTO dbo.Functions (TenantId, Name, Description, CreatedBy)
SELECT t.Id, f.Name, f.Description, 1
FROM dbo.Tenants t
CROSS JOIN (
    SELECT 'Gerente','Responsável pela gestão'
    UNION ALL SELECT 'Especialista','Especialista técnico'
    UNION ALL SELECT 'Analista','Analista funcional'
    UNION ALL SELECT 'Senior','Profissional experiente'
    UNION ALL SELECT 'Pleno','Profissional intermédio'
    UNION ALL SELECT 'Junior','Profissional iniciante'
    UNION ALL SELECT 'Estagiário','Em formação'
) f(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Functions fx
    WHERE fx.TenantId = t.Id AND fx.Name = f.Name
);
GO
