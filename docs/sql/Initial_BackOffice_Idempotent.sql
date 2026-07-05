GO
INSERT INTO dbo.AcquisitionSourceTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Outros',     N'Origem não especificada ou não classificada.'),
    (N'Instagram',  N'Cliente ou tenant originado através do Instagram.'),
    (N'Facebook',   N'Cliente ou tenant originado através do Facebook.'),
    (N'LinkedIn',   N'Cliente ou tenant originado através do LinkedIn.'),
    (N'YouTube',    N'Cliente ou tenant originado através do YouTube.'),
    (N'WhatsApp',   N'Cliente ou tenant originado através de contacto por WhatsApp.'),
    (N'TikTok',     N'Cliente ou tenant originado através do TikTok.'),
    (N'Google',     N'Cliente ou tenant originado através de pesquisa ou anúncio no Google.'),
    (N'Amigos',     N'Cliente ou tenant originado por indicação de amigos ou conhecidos.'),
    (N'TV',         N'Cliente ou tenant originado através de publicidade ou menção em televisão.'),
    (N'Rádio',      N'Cliente ou tenant originado através de rádio.'),
    (N'Jornal',     N'Cliente ou tenant originado através de jornal.'),
    (N'Revista',    N'Cliente ou tenant originado através de revista.'),
    (N'Eventos',    N'Cliente ou tenant originado através de eventos.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.AcquisitionSourceTypes ast
    WHERE ast.Name = v.Name
);

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
INSERT INTO dbo.ConsentOriginTypes (Name, Description, CreatedBy)
SELECT v.Name, v.Description, 1
FROM (VALUES
    (N'Web',        N'Consentimento capturado através da aplicação web ou site.'),
    (N'Mobile',     N'Consentimento capturado através da aplicação mobile.'),
    (N'Paper',      N'Consentimento capturado através de documento físico.'),
    (N'API',        N'Consentimento recebido através de integração/API.'),
    (N'Backoffice', N'Consentimento registado manualmente por utilizador interno.'),
    (N'Email',      N'Consentimento capturado através de comunicação por e-mail.'),
    (N'Sms',        N'Consentimento capturado através de SMS.'),
    (N'WhatsApp',   N'Consentimento capturado através de comunicação por WhatsApp.'),
    (N'CallCenter', N'Consentimento capturado através de atendimento telefónico.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.ConsentOriginTypes cot
    WHERE cot.Name = v.Name
);

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
    Currency, MaxUsers, MaxPhotosPerVisits, CreatedBy
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
    Currency, MaxUsers, MaxPhotosPerVisits, CreatedBy
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
    (N'AcquisitionSourceTypes', N'Tipos de origem de aquisição comercial.'),
    (N'AddressTypes', N'Tipos de endereços disponíveis.'),
    (N'ConsentOriginTypes', N'Tipos de origem/canal de captura de consentimentos.'),
    (N'FileTypes', N'Tipos de arquivos.'),
    (N'ConsentTypes', N'Tipos de consentimento.'),
    (N'StatusTypes', N'Tipos de status.'),
    (N'Plans', N'Planos do sistema.'),
    (N'PlanFileRules', N'Regras de arquivos por plano.'),
    (N'Tenants', N'Tenants do sistema.'),
    (N'Status', N'Status das entidades.'),
    (N'TenantContacts', N'Contatos dos tenants.'),
    (N'TenantAddresses', N'Endereços dos tenants.'),
    (N'TenantFiscalData', N'Dados fiscais dos tenants.'),
    (N'Subscriptions', N'Assinaturas dos tenants.'),
    (N'Users', N'Usuários do sistema.'),
    (N'UserPreferences', N'Preferências dos usuários.'),
    (N'Roles', N'Papéis/perfis de acesso.'),
    (N'Resources', N'Recursos do sistema.'),
    (N'Actions', N'Ações do sistema.'),
    (N'RolePermissions', N'Permissões associadas a roles.'),
    (N'UserRoles', N'Papéis dos usuários.'),
    (N'RefreshTokens', N'Tokens de atualização.'),
    (N'JwtKeys', N'Chaves JWT.'),
    (N'JobDefinitions', N'Definições de jobs/processos.'),
    (N'Functions', N'Funções do sistema.'),
    (N'Clients', N'Clientes do sistema.'),
    (N'ClientIndividuals', N'Dados de clientes individuais.'),
    (N'ClientCompanies', N'Dados de empresas clientes.'),
    (N'ClientAddresses', N'Endereços dos clientes.'),
    (N'ClientContacts', N'Contatos dos clientes.'),
    (N'ClientFiscalData', N'Dados fiscais dos clientes.'),
    (N'ClientConsents', N'Consentimentos dos clientes.'),
    (N'Teams', N'Equipas.'),
    (N'Employees', N'Colaboradores/membros da equipa.'),
    (N'EmployeeContacts', N'Contatos dos colaboradores.'),
    (N'EmployeeAddresses', N'Endereços dos colaboradores.'),
    (N'EmployeeTeam', N'Relação entre colaboradores e equipas.'),
    (N'EquipmentTypes', N'Tipos de equipamentos.'),
    (N'Equipments', N'Equipamentos.'),
    (N'Vehicles', N'Veículos.'),
    (N'Visits', N'Visitas/intervenções realizadas.'),
    (N'VisitContacts', N'Contatos das visitas.'),
    (N'VisitAddresses', N'Endereços das visitas.'),
    (N'VisitTeam', N'Equipas associadas às visitas.'),
    (N'VisitTeamEmployee', N'Colaboradores associados às equipas das visitas.'),
    (N'VisitTeamVehicle', N'Veículos associados às equipas das visitas.'),
    (N'VisitTeamEquipment', N'Equipamentos associados às equipas das visitas.'),
    (N'AttachmentCategories', N'Categorias de anexos.'),
    (N'VisitAttachments', N'Anexos das visitas.')
) AS v(Name, Description)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Resources r WHERE r.Name = v.Name
);
GO
INSERT INTO dbo.Tenants (
    TenantType,
    AcquisitionSourceTypeId,
    Name,
    Email,
    Website,
    UrlImage,
    Note,
    CreatedBy
)
SELECT
    v.TenantType,
    ast.Id,
    v.Name,
    v.Email,
    v.Website,
    v.UrlImage,
    v.Note,
    v.CreatedBy
FROM (VALUES
    (2, N'Outros', N'VianaHub Lda', N'contact@vianahub.pt', N'https://vianahub.pt', NULL, N'Tenant principal', 1),
    (2, N'Outros', N'Gerit Demo Lda', N'demo@gerit.pt', N'https://demo.gerit.pt', NULL, N'Ambiente de demonstração', 1),
    (2, N'Outros', N'Teste Lda', N'teste@teste.pt', NULL, NULL, N'Tenant para testes internos', 1)
) AS v(
    TenantType,
    AcquisitionSourceTypeName,
    Name,
    Email,
    Website,
    UrlImage,
    Note,
    CreatedBy
)
INNER JOIN dbo.AcquisitionSourceTypes ast
    ON ast.Name = v.AcquisitionSourceTypeName
   AND ast.IsDeleted = 0
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Tenants t WHERE t.Name = v.Name
);
GO
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
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
SELECT
    t.Id,
    v.Name,
    v.Email,
    v.Phone,
    v.PhoneIsWhatsapp,
    v.CellPhone,
    v.CellPhoneIsWhatsapp,
    v.IsPrimary,
    v.CreatedBy
FROM (VALUES
    (N'VianaHub Lda', N'João Silva', N'contact@vianahub.pt', N'+351210000001', 0, N'+351910000001', 1, 1, 1),
    (N'Gerit Demo Lda', N'Maria Costa', N'demo@gerit.pt', N'+351210000002', 0, N'+351920000002', 1, 1, 1),
    (N'Teste Lda', N'Carlos Teste', N'teste@teste.pt', NULL, 0, N'+351930000003', 1, 1, 1)
) AS v(
    TenantName,
    Name,
    Email,
    Phone,
    PhoneIsWhatsapp,
    CellPhone,
    CellPhoneIsWhatsapp,
    IsPrimary,
    CreatedBy
)
INNER JOIN dbo.Tenants t
    ON t.Name = v.TenantName
   AND t.IsDeleted = 0
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.TenantContacts tc 
    WHERE tc.TenantId = t.Id
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
    Note,
    IsPrimary,
    CreatedBy
)
SELECT
    t.Id,
    atp.Id,
    v.CountryCode,
    v.Street,
    v.Neighborhood,
    v.City,
    v.District,
    v.PostalCode,
    v.StreetNumber,
    v.Complement,
    v.Latitude,
    v.Longitude,
    v.Note,
    v.IsPrimary,
    v.CreatedBy
FROM (VALUES
    (N'VianaHub Lda', N'Morada comercial', 'PT', N'Avenida da Liberdade', N'Santo António', N'Lisboa', N'Lisboa', N'1250-140', N'100', N'3º Andar', 38.7223, -9.1393, N'Sede principal', 1, 1),
    (N'Gerit Demo Lda', N'Morada comercial', 'PT', N'Rua de Santa Catarina', N'Baixa', N'Porto', N'Porto', N'4000-447', N'200', NULL, 41.1496, -8.6110, N'Escritório demo', 1, 1),
    (N'Teste Lda', N'Morada comercial', 'PT', N'Avenida Central', N'Sé', N'Braga', N'Braga', N'4700-000', N'50', NULL, 41.5454, -8.4265, N'Endereço de teste', 1, 1)
) AS v(
    TenantName,
    AddressTypeName,
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
    Note,
    IsPrimary,
    CreatedBy
)
INNER JOIN dbo.Tenants t
    ON t.Name = v.TenantName
   AND t.IsDeleted = 0
INNER JOIN dbo.AddressTypes atp
    ON atp.Name = v.AddressTypeName
   AND atp.IsDeleted = 0
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.TenantAddresses ta
    WHERE ta.TenantId = t.Id
      AND ta.IsPrimary = 1
      AND ta.IsDeleted = 0
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
    p.Id,
    NULL,
    SYSDATETIME(),
    DATEADD(MONTH, 1, SYSDATETIME()),
    SYSDATETIME(),
    DATEADD(DAY, 30, SYSDATETIME()),
    0,
    NULL,
    1
FROM dbo.Tenants t
INNER JOIN dbo.Plans p
    ON p.Name = N'Free'
   AND p.IsDeleted = 0
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.Subscriptions s 
    WHERE s.TenantId = t.Id
);
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
GO

/*
    Idempotent seed de permissões para o perfil BackOffice.
    Objetivo:
      - Dener Viana no tenant VianaHub Lda
      - Admin no tenant Gerit Demo Lda
      - Ambos usando a role BackOffice
      - Ambos com as mesmas permissões: todas as combinações existentes de Resources x Actions

    Observação:
      - Não usa RoleId fixo, porque os IDs podem mudar conforme a ordem do seed.
      - Localiza a role pelo Tenant + Name = 'BackOffice'.
*/

;WITH TargetTenants AS (
    SELECT v.TenantName
    FROM (VALUES
        (N'VianaHub Lda'),
        (N'Gerit Demo Lda')
    ) v(TenantName)
), BackOfficeRoles AS (
    SELECT
        t.Id AS TenantId,
        r.Id AS RoleId
    FROM TargetTenants tt
    INNER JOIN dbo.Tenants t
        ON t.Name = tt.TenantName
       AND t.IsDeleted = 0
    INNER JOIN dbo.Roles r
        ON r.TenantId = t.Id
       AND r.Name = N'BackOffice'
       AND r.IsDeleted = 0
)
INSERT INTO dbo.RolePermissions (
    TenantId,
    RoleId,
    ResourceId,
    ActionId
)
SELECT
    bor.TenantId,
    bor.RoleId,
    res.Id AS ResourceId,
    act.Id AS ActionId
FROM BackOfficeRoles bor
CROSS JOIN dbo.Resources res
CROSS JOIN dbo.Actions act
WHERE res.IsDeleted = 0
  AND act.IsDeleted = 0
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.RolePermissions rp
      WHERE rp.TenantId = bor.TenantId
        AND rp.RoleId = bor.RoleId
        AND rp.ResourceId = res.Id
        AND rp.ActionId = act.Id
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
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
GO

/*
    Idempotent seed dos usuários principais.
    Ambos ficam com o mesmo perfil: BackOffice.
*/

MERGE dbo.Users AS target
USING (
    SELECT
        t.Id AS TenantId,
        u.Name,
        u.Email,
        u.NormalizedEmail,
        u.EmailConfirmed,
        u.PhoneNumber,
        u.PhoneNumberConfirmed,
        CAST(NULL AS DATETIME2(7)) AS LastAccessAt,
        u.PasswordHash,
        u.UrlImage
    FROM (VALUES
        (N'VianaHub Lda',   N'Dener Viana', N'viana.dener@gmail.com', N'VIANA.DENER@GMAIL.COM', 1, N'960268353', 0, N'AQAAAAIAAYagAAAAEAr88ZwhIrd69foEZO57diA8qdyfk3QkMPoCo9KxZ/CKlP1tFN7QPk6dHdMM2bQCNA==', N'www.gerit.pt/users/viana.dener.jpg'),
        (N'Gerit Demo Lda', N'Admin',       N'admin@geritapp.com',    N'ADMIN@GERITAPP.COM',    0, NULL,         0, N'AQAAAAIAAYagAAAAEAr88ZwhIrd69foEZO57diA8qdyfk3QkMPoCo9KxZ/CKlP1tFN7QPk6dHdMM2bQCNA==', NULL)
    ) AS u(TenantName, Name, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, PasswordHash, UrlImage)
    INNER JOIN dbo.Tenants t
        ON t.Name = u.TenantName
       AND t.IsDeleted = 0
) AS source
ON target.TenantId = source.TenantId
AND target.NormalizedEmail = source.NormalizedEmail
WHEN MATCHED THEN
    UPDATE SET
        target.Name = source.Name,
        target.Email = source.Email,
        target.EmailConfirmed = source.EmailConfirmed,
        target.PhoneNumber = source.PhoneNumber,
        target.PhoneNumberConfirmed = source.PhoneNumberConfirmed,
        target.PasswordHash = source.PasswordHash,
        target.UrlImage = source.UrlImage,
        target.IsActive = 1,
        target.IsDeleted = 0,
        target.ModifiedBy = 1,
        target.ModifiedAt = SYSDATETIME()
WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        TenantId,
        Name,
        Email,
        NormalizedEmail,
        EmailConfirmed,
        PhoneNumber,
        PhoneNumberConfirmed,
        LastAccessAt,
        PasswordHash,
        UrlImage,
        IsActive,
        IsDeleted,
        CreatedBy,
        CreatedAt
    )
    VALUES (
        source.TenantId,
        source.Name,
        source.Email,
        source.NormalizedEmail,
        source.EmailConfirmed,
        source.PhoneNumber,
        source.PhoneNumberConfirmed,
        source.LastAccessAt,
        source.PasswordHash,
        source.UrlImage,
        1,
        0,
        1,
        SYSDATETIME()
    );
GO

/*
    Garante que Dener Viana e Admin tenham somente o perfil BackOffice
    nos seus respectivos tenants.
*/

;WITH TargetUsers AS (
    SELECT
        t.Id AS TenantId,
        u.Id AS UserId,
        r.Id AS BackOfficeRoleId
    FROM (VALUES
        (N'VianaHub Lda',   N'VIANA.DENER@GMAIL.COM'),
        (N'Gerit Demo Lda', N'ADMIN@GERITAPP.COM')
    ) AS x(TenantName, NormalizedEmail)
    INNER JOIN dbo.Tenants t
        ON t.Name = x.TenantName
       AND t.IsDeleted = 0
    INNER JOIN dbo.Users u
        ON u.TenantId = t.Id
       AND u.NormalizedEmail = x.NormalizedEmail
       AND u.IsDeleted = 0
    INNER JOIN dbo.Roles r
        ON r.TenantId = t.Id
       AND r.Name = N'BackOffice'
       AND r.IsDeleted = 0
)
DELETE ur
FROM dbo.UserRoles ur
INNER JOIN TargetUsers tu
    ON tu.TenantId = ur.TenantId
   AND tu.UserId = ur.UserId
WHERE ur.RoleId <> tu.BackOfficeRoleId;
GO

;WITH TargetUsers AS (
    SELECT
        t.Id AS TenantId,
        u.Id AS UserId,
        r.Id AS RoleId
    FROM (VALUES
        (N'VianaHub Lda',   N'VIANA.DENER@GMAIL.COM'),
        (N'Gerit Demo Lda', N'ADMIN@GERITAPP.COM')
    ) AS x(TenantName, NormalizedEmail)
    INNER JOIN dbo.Tenants t
        ON t.Name = x.TenantName
       AND t.IsDeleted = 0
    INNER JOIN dbo.Users u
        ON u.TenantId = t.Id
       AND u.NormalizedEmail = x.NormalizedEmail
       AND u.IsDeleted = 0
    INNER JOIN dbo.Roles r
        ON r.TenantId = t.Id
       AND r.Name = N'BackOffice'
       AND r.IsDeleted = 0
)
INSERT INTO dbo.UserRoles (
    TenantId,
    UserId,
    RoleId
)
SELECT
    tu.TenantId,
    tu.UserId,
    tu.RoleId
FROM TargetUsers tu
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.UserRoles ur
    WHERE ur.TenantId = tu.TenantId
      AND ur.UserId = tu.UserId
      AND ur.RoleId = tu.RoleId
);

GO
DECLARE @TenantId INT;
DECLARE @CreatedBy INT = 1;

SELECT @TenantId = t.Id
FROM dbo.Tenants t
WHERE t.Name = N'VianaHub Lda'
  AND t.IsDeleted = 0;

IF @TenantId IS NULL
BEGIN
    THROW 51000, 'Tenant VianaHub Lda não encontrado. Execute primeiro o seed de Tenants.', 1;
END;

DECLARE @ResidentialAddressTypeId INT = COALESCE(
    (SELECT TOP 1 Id FROM dbo.AddressTypes WHERE Name = N'Morada residencial' AND IsDeleted = 0),
    1
);

DECLARE @CommercialAddressTypeId INT = COALESCE(
    (SELECT TOP 1 Id FROM dbo.AddressTypes WHERE Name = N'Morada comercial' AND IsDeleted = 0),
    @ResidentialAddressTypeId
);

DECLARE @SeedClients TABLE
(
    SeedKey NVARCHAR(50) NOT NULL PRIMARY KEY,
    ClientId INT NULL,
    ClientType INT NOT NULL,
    AcquisitionSourceTypeName NVARCHAR(50) NOT NULL,
    Note NVARCHAR(500) NOT NULL,

    FullName NVARCHAR(500) NULL,
	FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    LegalName NVARCHAR(200) NULL,
    TradeName NVARCHAR(200) NULL,

    PhoneNumber NVARCHAR(50) NULL,
    CellPhoneNumber NVARCHAR(50) NULL,
    IsWhatsapp BIT NOT NULL,
    Email NVARCHAR(500) NULL,

    BirthDate DATE NULL,
    Gender NVARCHAR(20) NULL,
    DocumentType NVARCHAR(50) NULL,
    DocumentNumber NVARCHAR(50) NULL,
    Nationality NVARCHAR(100) NULL,

    Site NVARCHAR(500) NULL,
    CompanyRegistration NVARCHAR(50) NULL,
    CAE NVARCHAR(10) NULL,
    NumberOfEmployee INT NULL,
    LegalRepresentative NVARCHAR(150) NULL,

    TaxNumber NVARCHAR(20) NOT NULL,
    VatNumber NVARCHAR(20) NULL,
    FiscalEmail NVARCHAR(255) NULL,
    IsVatRegistered BIT NOT NULL,

    AddressTypeId INT NOT NULL,
    Street NVARCHAR(200) NOT NULL,
    Neighborhood NVARCHAR(100) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    District NVARCHAR(100) NULL,
    PostalCode NVARCHAR(20) NOT NULL,
    StreetNumber NVARCHAR(20) NULL,
    Complement NVARCHAR(100) NULL,
    ContactName NVARCHAR(150) NOT NULL
);

INSERT INTO @SeedClients
(
    SeedKey, ClientType, AcquisitionSourceTypeName, Note,
    FullName, FirstName, LastName, LegalName, TradeName,
    PhoneNumber, CellPhoneNumber, IsWhatsapp, Email,
    BirthDate, Gender, DocumentType, DocumentNumber, Nationality,
    Site, CompanyRegistration, CAE, NumberOfEmployee, LegalRepresentative,
    TaxNumber, VatNumber, FiscalEmail, IsVatRegistered,
    AddressTypeId, Street, Neighborhood, City, District, PostalCode, StreetNumber, Complement, ContactName
)
VALUES
-- 4 clientes pessoa física
(N'Client-001', 1, N'Instagram', N'[InitialSeed:Client-001] Pessoa Singular - Mariana Costa',
 N'Mariana Costa', N'Mariana', N'Costa', NULL, NULL,
 N'289123401', N'934123401', 1, N'mariana.costa@example.pt',
 '1992-04-18', N'Feminino', N'Cartão de Cidadão', N'12345678', N'Portugal',
 NULL, NULL, NULL, NULL, NULL,
 N'245789321', NULL, N'mariana.costa@example.pt', 0,
 @ResidentialAddressTypeId, N'Rua de São Luís', N'Centro', N'Faro', N'Faro', N'8000-285', N'14', N'2.º Esq.', N'Mariana Costa'),

(N'Client-002', 1, N'Google', N'[InitialSeed:Client-002] Pessoa Singular - João Martins',
 N'João Martins', N'João', N'Martins', NULL, NULL,
 N'282123402', N'935123402', 1, N'joao.martins@example.pt',
 '1987-09-03', N'Masculino', N'Cartão de Cidadão', N'23456789', N'Portugal',
 NULL, NULL, NULL, NULL, NULL,
 N'246789322', NULL, N'joao.martins@example.pt', 0,
 @ResidentialAddressTypeId, N'Avenida da República', N'Centro', N'Portimão', N'Faro', N'8500-300', N'52', N'1.º Dt.', N'João Martins'),

(N'Client-003', 2, N'WhatsApp', N'[InitialSeed:Client-003] Recibos Verdes - Sofia Almeida',
 N'Sofia Almeida', N'Sofia', N'Almeida', NULL, NULL,
 N'213123403', N'936123403', 1, N'sofia.almeida@example.pt',
 '1990-01-26', N'Feminino', N'Cartão de Cidadão', N'34567890', N'Portugal',
 NULL, NULL, NULL, NULL, NULL,
 N'247789323', N'PT247789323', N'sofia.almeida@example.pt', 1,
 @ResidentialAddressTypeId, N'Rua do Alecrim', N'Misericórdia', N'Lisboa', N'Lisboa', N'1200-014', N'21', N'3.º', N'Sofia Almeida'),

(N'Client-004', 3, N'LinkedIn', N'[InitialSeed:Client-004] Freelancer - Tiago Ferreira',
 N'Tiago Ferreira', N'Tiago', N'Ferreira', NULL, NULL,
 N'222123404', N'937123404', 1, N'tiago.ferreira@example.pt',
 '1985-11-12', N'Masculino', N'Cartão de Cidadão', N'45678901', N'Portugal',
 NULL, NULL, NULL, NULL, NULL,
 N'248789324', N'PT248789324', N'tiago.ferreira@example.pt', 1,
 @ResidentialAddressTypeId, N'Rua de Cedofeita', N'Cedofeita', N'Porto', N'Porto', N'4050-174', N'88', N'4.º Esq.', N'Tiago Ferreira'),

-- 8 clientes empresa
(N'Client-005', 4, N'Facebook', N'[InitialSeed:Client-005] Pessoa Jurídica - Algarve Tech Solutions Lda',
 NULL, NULL, NULL, N'Algarve Tech Solutions Lda', N'Algarve Tech',
 N'289123405', N'938123405', 1, N'geral@algarvetech.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://algarvetech.example.pt', N'514789325', N'62010', 18, N'Ricardo Neves',
 N'514789325', N'PT514789325', N'fiscal@algarvetech.example.pt', 1,
 @CommercialAddressTypeId, N'Rua do Comércio', N'Baixa', N'Loulé', N'Faro', N'8100-536', N'7', N'Loja A', N'Ricardo Neves'),

(N'Client-006', 4, N'YouTube', N'[InitialSeed:Client-006] Pessoa Jurídica - Lisboa Digital Services Lda',
 NULL, NULL, NULL, N'Lisboa Digital Services Lda', N'Lisboa Digital',
 N'211123406', N'939123406', 1, N'contacto@lisboadigital.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://lisboadigital.example.pt', N'514789326', N'62020', 32, N'Patrícia Ramos',
 N'514789326', N'PT514789326', N'fiscal@lisboadigital.example.pt', 1,
 @CommercialAddressTypeId, N'Avenida da Liberdade', N'Santo António', N'Lisboa', N'Lisboa', N'1250-096', N'110', N'5.º', N'Patrícia Ramos'),

(N'Client-007', 4, N'TikTok', N'[InitialSeed:Client-007] Pessoa Jurídica - Porto Creative Agency Lda',
 NULL, NULL, NULL, N'Porto Creative Agency Lda', N'Porto Creative',
 N'222123407', N'930123407', 1, N'hello@portocreative.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://portocreative.example.pt', N'514789327', N'73110', 14, N'Helena Pinto',
 N'514789327', N'PT514789327', N'fiscal@portocreative.example.pt', 1,
 @CommercialAddressTypeId, N'Rua de Santa Catarina', N'Santo Ildefonso', N'Porto', N'Porto', N'4000-447', N'312', N'Sala 2', N'Helena Pinto'),

(N'Client-008', 4, N'Eventos', N'[InitialSeed:Client-008] Pessoa Jurídica - Coimbra Business Consulting Lda',
 NULL, NULL, NULL, N'Coimbra Business Consulting Lda', N'Coimbra Consulting',
 N'239123408', N'931123408', 1, N'geral@coimbraconsulting.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://coimbraconsulting.example.pt', N'514789328', N'70220', 22, N'Miguel Santos',
 N'514789328', N'PT514789328', N'fiscal@coimbraconsulting.example.pt', 1,
 @CommercialAddressTypeId, N'Rua Ferreira Borges', N'Baixa', N'Coimbra', N'Coimbra', N'3000-179', N'45', N'2.º', N'Miguel Santos'),

(N'Client-009', 5, N'Instagram', N'[InitialSeed:Client-009] Sociedade Unipessoal por Quotas - Braga Web Studio Unipessoal Lda',
 NULL, NULL, NULL, N'Braga Web Studio Unipessoal Lda', N'Braga Web Studio',
 N'253123409', N'932123409', 1, N'geral@bragawebstudio.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://bragawebstudio.example.pt', N'516789329', N'62010', 5, N'Inês Carvalho',
 N'516789329', N'PT516789329', N'fiscal@bragawebstudio.example.pt', 1,
 @CommercialAddressTypeId, N'Avenida Central', N'Centro', N'Braga', N'Braga', N'4710-229', N'91', N'Escritório 3', N'Inês Carvalho'),

(N'Client-010', 5, N'LinkedIn', N'[InitialSeed:Client-010] Sociedade Unipessoal por Quotas - Aveiro Automation Unipessoal Lda',
 NULL, NULL, NULL, N'Aveiro Automation Unipessoal Lda', N'Aveiro Automation',
 N'234123410', N'933123410', 1, N'geral@aveiroautomation.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://aveiroautomation.example.pt', N'516789330', N'71120', 7, N'Pedro Lima',
 N'516789330', N'PT516789330', N'fiscal@aveiroautomation.example.pt', 1,
 @CommercialAddressTypeId, N'Rua do Clube dos Galitos', N'Centro', N'Aveiro', N'Aveiro', N'3810-164', N'18', N'Sala 4', N'Pedro Lima'),

(N'Client-011', 5, N'Google', N'[InitialSeed:Client-011] Sociedade Unipessoal por Quotas - Évora Design Lab Unipessoal Lda',
 NULL, NULL, NULL, N'Évora Design Lab Unipessoal Lda', N'Évora Design Lab',
 N'266123411', N'934123411', 1, N'geral@evoradesignlab.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://evoradesignlab.example.pt', N'516789331', N'74100', 4, N'Carla Mendes',
 N'516789331', N'PT516789331', N'fiscal@evoradesignlab.example.pt', 1,
 @CommercialAddressTypeId, N'Rua da República', N'Centro Histórico', N'Évora', N'Évora', N'7000-656', N'27', N'Loja 1', N'Carla Mendes'),

(N'Client-012', 5, N'Amigos', N'[InitialSeed:Client-012] Sociedade Unipessoal por Quotas - Viseu Data Services Unipessoal Lda',
 NULL, NULL, NULL, N'Viseu Data Services Unipessoal Lda', N'Viseu Data',
 N'232123412', N'935123412', 1, N'geral@viseudata.example.pt',
 NULL, NULL, NULL, NULL, NULL,
 N'https://viseudata.example.pt', N'516789332', N'63110', 6, N'André Rocha',
 N'516789332', N'PT516789332', N'fiscal@viseudata.example.pt', 1,
 @CommercialAddressTypeId, N'Rua Formosa', N'Centro', N'Viseu', N'Viseu', N'3500-135', N'60', N'1.º', N'André Rocha');

INSERT INTO dbo.Clients (TenantId, AcquisitionSourceTypeId, ClientType, UrlImage, Note, IsActive, IsDeleted, CreatedBy, CreatedAt)
SELECT
    @TenantId,
    ast.Id,
    s.ClientType,
    NULL,
    s.Note,
    1,
    0,
    @CreatedBy,
    SYSDATETIME()
FROM @SeedClients s
INNER JOIN dbo.AcquisitionSourceTypes ast
    ON ast.Name = s.AcquisitionSourceTypeName
   AND ast.IsDeleted = 0
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Clients c
    WHERE c.TenantId = @TenantId
      AND c.Note = s.Note
);

UPDATE s
SET ClientId = c.Id
FROM @SeedClients s
INNER JOIN dbo.Clients c
    ON c.TenantId = @TenantId
   AND c.Note = s.Note;

INSERT INTO dbo.ClientIndividuals
(
    TenantId, ClientId, FullName, FirstName, LastName, PhoneNumber, CellPhoneNumber, IsWhatsapp, Email,
    BirthDate, Gender, DocumentType, DocumentNumber, Nationality,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.FullName, s.FirstName, s.LastName, s.PhoneNumber, s.CellPhoneNumber, s.IsWhatsapp, s.Email,
    s.BirthDate, s.Gender, s.DocumentType, s.DocumentNumber, s.Nationality,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientType IN (1, 2, 3)
  AND s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientIndividuals ci
      WHERE ci.TenantId = @TenantId
        AND ci.ClientId = s.ClientId
  );

INSERT INTO dbo.ClientCompanies
(
    TenantId, ClientId, LegalName, TradeName, PhoneNumber, CellPhoneNumber, IsWhatsapp, Email,
    Site, CompanyRegistration, CAE, NumberOfEmployee, LegalRepresentative,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.LegalName, s.TradeName, s.PhoneNumber, s.CellPhoneNumber, s.IsWhatsapp, s.Email,
    s.Site, s.CompanyRegistration, s.CAE, s.NumberOfEmployee, s.LegalRepresentative,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientType IN (4, 5)
  AND s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientCompanies cc
      WHERE cc.TenantId = @TenantId
        AND cc.ClientId = s.ClientId
  );

INSERT INTO dbo.ClientFiscalData
(
    TenantId, ClientId, TaxNumber, VatNumber, FiscalCountry, IsVatRegistered, IBAN, FiscalEmail,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.TaxNumber, s.VatNumber, 'PT', s.IsVatRegistered, NULL, s.FiscalEmail,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientFiscalData fd
      WHERE fd.TenantId = @TenantId
        AND fd.ClientId = s.ClientId
  );

INSERT INTO dbo.ClientAddresses
(
    TenantId, ClientId, AddressTypeId, CountryCode, Street, Neighborhood, City, District, PostalCode,
    StreetNumber, Complement, Latitude, Longitude, Note, IsPrimary,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.AddressTypeId, 'PT', s.Street, s.Neighborhood, s.City, s.District, s.PostalCode,
    s.StreetNumber, s.Complement, NULL, NULL, N'Endereço principal criado pelo seed inicial.', 1,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientAddresses a
      WHERE a.TenantId = @TenantId
        AND a.ClientId = s.ClientId
        AND a.IsPrimary = 1
        AND a.IsDeleted = 0
  );

INSERT INTO dbo.ClientContacts
(
    TenantId, ClientId, Name, PhoneNumber, CellPhoneNumber, IsWhatsapp, Email, IsPrimary,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.ContactName, s.PhoneNumber, s.CellPhoneNumber, s.IsWhatsapp, CONVERT(NVARCHAR(255), s.Email), 1,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientId IS NOT NULL
  AND s.Email IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientContacts ct
      WHERE ct.TenantId = @TenantId
        AND ct.ClientId = s.ClientId
        AND ct.Email = CONVERT(NVARCHAR(255), s.Email)
  );

/*
    ClientConsents - VianaHub Lda
    - Idempotente por Tenant + Cliente + Tipo de consentimento + Origem.
    - Usa Name nos catálogos ConsentTypes e ConsentOriginTypes.
*/
INSERT INTO dbo.ClientConsents
(
    TenantId, ClientId, ConsentTypeId, ConsentOriginTypeId, Granted, GrantedDate,
    RevokedDate, IpAddress, UserAgent,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId,
    s.ClientId,
    ct.Id,
    cot.Id,
    v.Granted,
    DATEADD(DAY, v.DaysOffset, SYSDATETIME()),
    NULL,
    v.IpAddress,
    v.UserAgent,
    1,
    0,
    @CreatedBy,
    SYSDATETIME()
FROM @SeedClients s
CROSS APPLY (VALUES
    (N'PrivacyPolicy',   N'Backoffice', CONVERT(BIT, 1), -30, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed - política de privacidade aceite no backoffice.'),
    (N'TermsOfService',  N'Backoffice', CONVERT(BIT, 1), -30, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed - termos de serviço aceites no backoffice.'),
    (N'DataProcessing',  N'Backoffice', CONVERT(BIT, 1), -29, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed - tratamento de dados aceite no backoffice.'),
    (N'Marketing',       N'Email',      CONVERT(BIT, 1), -20, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed - opt-in de marketing por email.'),
    (N'EmailConsent',    N'Email',      CONVERT(BIT, 1), -20, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed - consentimento de comunicação por email.'),
    (N'Cookies',         N'Web',        CONVERT(BIT, 1), -15, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed - cookies aceites na web.')
) AS v(ConsentTypeName, ConsentOriginTypeName, Granted, DaysOffset, IpAddress, UserAgent)
INNER JOIN dbo.ConsentTypes ct
    ON ct.Name = v.ConsentTypeName
   AND ct.IsDeleted = 0
INNER JOIN dbo.ConsentOriginTypes cot
    ON cot.Name = v.ConsentOriginTypeName
   AND cot.IsDeleted = 0
WHERE s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientConsents cc
      WHERE cc.TenantId = @TenantId
        AND cc.ClientId = s.ClientId
        AND cc.ConsentTypeId = ct.Id
        AND cc.ConsentOriginTypeId = cot.Id
        AND cc.IsDeleted = 0
  );

GO
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
GO

DECLARE @TenantId INT;
DECLARE @CreatedBy INT = 1;

SELECT @TenantId = t.Id
FROM dbo.Tenants t
WHERE t.Name = N'Gerit Demo Lda'
  AND t.IsDeleted = 0;

IF @TenantId IS NULL
BEGIN
    THROW 51000, 'Tenant Gerit Demo Lda não encontrado. Execute primeiro o seed de Tenants.', 1;
END;

DECLARE @ResidentialAddressTypeId INT = COALESCE(
    (SELECT TOP 1 Id FROM dbo.AddressTypes WHERE Name = N'Morada residencial' AND IsDeleted = 0),
    1
);

DECLARE @CommercialAddressTypeId INT = COALESCE(
    (SELECT TOP 1 Id FROM dbo.AddressTypes WHERE Name = N'Morada comercial' AND IsDeleted = 0),
    @ResidentialAddressTypeId
);

DECLARE @SeedClients TABLE
(
    SeedKey NVARCHAR(50) NOT NULL PRIMARY KEY,
    ClientId INT NULL,
    ClientType INT NOT NULL,
    AcquisitionSourceTypeName NVARCHAR(50) NOT NULL,
    Note NVARCHAR(500) NOT NULL,

	FullName NVARCHAR(500) NULL,
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    LegalName NVARCHAR(200) NULL,
    TradeName NVARCHAR(200) NULL,

    PhoneNumber NVARCHAR(50) NULL,
    CellPhoneNumber NVARCHAR(50) NULL,
    IsWhatsapp BIT NOT NULL,
    Email NVARCHAR(500) NULL,

    BirthDate DATE NULL,
    Gender NVARCHAR(20) NULL,
    DocumentType NVARCHAR(50) NULL,
    DocumentNumber NVARCHAR(50) NULL,
    Nationality NVARCHAR(100) NULL,

    Site NVARCHAR(500) NULL,
    CompanyRegistration NVARCHAR(50) NULL,
    CAE NVARCHAR(10) NULL,
    NumberOfEmployee INT NULL,
    LegalRepresentative NVARCHAR(150) NULL,

    TaxNumber NVARCHAR(20) NOT NULL,
    VatNumber NVARCHAR(20) NULL,
    FiscalEmail NVARCHAR(255) NULL,
    IsVatRegistered BIT NOT NULL,

    AddressTypeId INT NOT NULL,
    Street NVARCHAR(200) NOT NULL,
    Neighborhood NVARCHAR(100) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    District NVARCHAR(100) NULL,
    PostalCode NVARCHAR(20) NOT NULL,
    StreetNumber NVARCHAR(20) NULL,
    Complement NVARCHAR(100) NULL,
    ContactName NVARCHAR(150) NOT NULL
);

INSERT INTO @SeedClients
(
    SeedKey, ClientType, AcquisitionSourceTypeName, Note,
    FullName, FirstName, LastName, LegalName, TradeName,
    PhoneNumber, CellPhoneNumber, IsWhatsapp, Email,
    BirthDate, Gender, DocumentType, DocumentNumber, Nationality,
    Site, CompanyRegistration, CAE, NumberOfEmployee, LegalRepresentative,
    TaxNumber, VatNumber, FiscalEmail, IsVatRegistered,
    AddressTypeId, Street, Neighborhood, City, District, PostalCode, StreetNumber, Complement, ContactName
)
VALUES
(N'GD-001', 1, N'Instagram', N'[GeritDemoSeed:GD-001] Pessoa Singular - Ana Ribeiro', N'Ana Ribeiro', N'Ana', N'Ribeiro', NULL, NULL, N'218100001', N'910100001', 1, N'ana.ribeiro.demo@gerit.pt', '1991-03-14', N'Feminino', N'Cartão de Cidadão', N'GD100001', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900001', NULL, N'ana.ribeiro.demo@gerit.pt', 0, @ResidentialAddressTypeId, N'Rua das Flores', N'Centro', N'Lisboa', N'Lisboa', N'1200-195', N'10', N'2.º Esq.', N'Ana Ribeiro'),
(N'GD-002', 1, N'Google', N'[GeritDemoSeed:GD-002] Pessoa Singular - Miguel Correia', N'Miguel Correia', N'Miguel', N'Correia', NULL, NULL, N'222100002', N'910100002', 1, N'miguel.correia.demo@gerit.pt', '1986-07-22', N'Masculino', N'Cartão de Cidadão', N'GD100002', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900002', NULL, N'miguel.correia.demo@gerit.pt', 0, @ResidentialAddressTypeId, N'Rua de Cedofeita', N'Cedofeita', N'Porto', N'Porto', N'4050-174', N'84', N'3.º Dt.', N'Miguel Correia'),
(N'GD-003', 2, N'WhatsApp', N'[GeritDemoSeed:GD-003] Recibos Verdes - Catarina Lopes', N'Catarina Lopes', N'Catarina', N'Lopes', NULL, NULL, N'239100003', N'910100003', 1, N'catarina.lopes.demo@gerit.pt', '1990-11-05', N'Feminino', N'Cartão de Cidadão', N'GD100003', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900003', N'PT245900003', N'catarina.lopes.demo@gerit.pt', 1, @ResidentialAddressTypeId, N'Rua Ferreira Borges', N'Baixa', N'Coimbra', N'Coimbra', N'3000-179', N'36', N'1.º', N'Catarina Lopes'),
(N'GD-004', 3, N'LinkedIn', N'[GeritDemoSeed:GD-004] Freelancer - Rui Fernandes', N'Rui Fernandes', N'Rui', N'Fernandes', NULL, NULL, N'253100004', N'910100004', 1, N'rui.fernandes.demo@gerit.pt', '1988-01-19', N'Masculino', N'Cartão de Cidadão', N'GD100004', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900004', N'PT245900004', N'rui.fernandes.demo@gerit.pt', 1, @ResidentialAddressTypeId, N'Avenida Central', N'Centro', N'Braga', N'Braga', N'4710-229', N'75', N'Sala 2', N'Rui Fernandes'),
(N'GD-005', 1, N'Amigos', N'[GeritDemoSeed:GD-005] Pessoa Singular - Beatriz Sousa', N'Beatriz Sousa', N'Beatriz', N'Sousa', NULL, NULL, N'289100005', N'910100005', 1, N'beatriz.sousa.demo@gerit.pt', '1995-05-30', N'Feminino', N'Cartão de Cidadão', N'GD100005', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900005', NULL, N'beatriz.sousa.demo@gerit.pt', 0, @ResidentialAddressTypeId, N'Rua de Santo António', N'Centro', N'Faro', N'Faro', N'8000-283', N'18', N'R/C', N'Beatriz Sousa'),
(N'GD-006', 2, N'Facebook', N'[GeritDemoSeed:GD-006] Recibos Verdes - Diogo Matos', N'Diogo Matos', N'Diogo', N'Matos', NULL, NULL, N'234100006', N'910100006', 1, N'diogo.matos.demo@gerit.pt', '1984-10-11', N'Masculino', N'Cartão de Cidadão', N'GD100006', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900006', N'PT245900006', N'diogo.matos.demo@gerit.pt', 1, @ResidentialAddressTypeId, N'Rua Direita', N'Centro', N'Aveiro', N'Aveiro', N'3810-005', N'25', N'2.º', N'Diogo Matos'),
(N'GD-007', 3, N'TikTok', N'[GeritDemoSeed:GD-007] Freelancer - Inês Carvalho', N'Inês Carvalho', N'Inês', N'Carvalho', NULL, NULL, N'266100007', N'910100007', 1, N'ines.carvalho.demo@gerit.pt', '1993-12-08', N'Feminino', N'Cartão de Cidadão', N'GD100007', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900007', N'PT245900007', N'ines.carvalho.demo@gerit.pt', 1, @ResidentialAddressTypeId, N'Rua da República', N'Centro Histórico', N'Évora', N'Évora', N'7000-656', N'42', N'1.º Esq.', N'Inês Carvalho'),
(N'GD-008', 1, N'YouTube', N'[GeritDemoSeed:GD-008] Pessoa Singular - Pedro Nunes', N'Pedro Nunes', N'Pedro', N'Nunes', NULL, NULL, N'232100008', N'910100008', 1, N'pedro.nunes.demo@gerit.pt', '1982-06-27', N'Masculino', N'Cartão de Cidadão', N'GD100008', N'Portugal', NULL, NULL, NULL, NULL, NULL, N'245900008', NULL, N'pedro.nunes.demo@gerit.pt', 0, @ResidentialAddressTypeId, N'Rua Formosa', N'Centro', N'Viseu', N'Viseu', N'3500-135', N'61', N'4.º', N'Pedro Nunes'),
(N'GD-009', 4, N'LinkedIn', N'[GeritDemoSeed:GD-009] Empresa real em Portugal - EDP - Energias de Portugal, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'EDP - Energias de Portugal, S.A.', N'EDP', N'351210009', N'911100009', 1, N'demo+edp@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.edp.com', N'500900009', N'35110', 12000, N'Responsável Demo EDP', N'507900009', N'PT507900009', N'fiscal+edp@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida 24 de Julho', N'Santos', N'Lisboa', N'Lisboa', N'1200-868', N'12', N'Edifício Demo', N'Responsável Demo EDP'),
(N'GD-010', 4, N'Google', N'[GeritDemoSeed:GD-010] Empresa real em Portugal - Galp Energia, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Galp Energia, SGPS, S.A.', N'Galp', N'351210010', N'911100010', 1, N'demo+galp@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.galp.com', N'500900010', N'46711', 6000, N'Responsável Demo Galp', N'507900010', N'PT507900010', N'fiscal+galp@gerit.pt', 1, @CommercialAddressTypeId, N'Rua Tomás da Fonseca', N'São Domingos de Benfica', N'Lisboa', N'Lisboa', N'1600-209', N'15', N'Torre Demo', N'Responsável Demo Galp'),
(N'GD-011', 4, N'Eventos', N'[GeritDemoSeed:GD-011] Empresa real em Portugal - Jerónimo Martins, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Jerónimo Martins, SGPS, S.A.', N'Jerónimo Martins', N'351210011', N'911100011', 1, N'demo+jeronimo.martins@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.jeronimomartins.com', N'500900011', N'70100', 30000, N'Responsável Demo JM', N'507900011', N'PT507900011', N'fiscal+jeronimo.martins@gerit.pt', 1, @CommercialAddressTypeId, N'Rua Actor António Silva', N'Alta de Lisboa', N'Lisboa', N'Lisboa', N'1600-404', N'7', N'Piso Demo', N'Responsável Demo JM'),
(N'GD-012', 4, N'LinkedIn', N'[GeritDemoSeed:GD-012] Empresa real em Portugal - Sonae, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Sonae, SGPS, S.A.', N'Sonae', N'351220012', N'911100012', 1, N'demo+sonae@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.sonae.pt', N'500900012', N'70100', 45000, N'Responsável Demo Sonae', N'507900012', N'PT507900012', N'fiscal+sonae@gerit.pt', 1, @CommercialAddressTypeId, N'Lugar do Espido', N'Via Norte', N'Maia', N'Porto', N'4470-177', N'0', N'Bloco Demo', N'Responsável Demo Sonae'),
(N'GD-013', 4, N'Google', N'[GeritDemoSeed:GD-013] Empresa real em Portugal - NOS, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'NOS, SGPS, S.A.', N'NOS', N'351210013', N'911100013', 1, N'demo+nos@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.nos.pt', N'500900013', N'64202', 2000, N'Responsável Demo NOS', N'507900013', N'PT507900013', N'fiscal+nos@gerit.pt', 1, @CommercialAddressTypeId, N'Rua Actor António Silva', N'Lumiar', N'Lisboa', N'Lisboa', N'1600-404', N'9', N'Edifício Demo', N'Responsável Demo NOS'),
(N'GD-014', 4, N'Eventos', N'[GeritDemoSeed:GD-014] Empresa real em Portugal - Mota-Engil, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Mota-Engil, SGPS, S.A.', N'Mota-Engil', N'351220014', N'911100014', 1, N'demo+mota.engil@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.mota-engil.com', N'500900014', N'42990', 10000, N'Responsável Demo Mota-Engil', N'507900014', N'PT507900014', N'fiscal+mota.engil@gerit.pt', 1, @CommercialAddressTypeId, N'Rua do Rego Lameiro', N'Campanhã', N'Porto', N'Porto', N'4300-454', N'38', N'Piso Demo', N'Responsável Demo Mota-Engil'),
(N'GD-015', 4, N'Google', N'[GeritDemoSeed:GD-015] Empresa real em Portugal - REN - Redes Energéticas Nacionais, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'REN - Redes Energéticas Nacionais, SGPS, S.A.', N'REN', N'351210015', N'911100015', 1, N'demo+ren@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.ren.pt', N'500900015', N'35120', 700, N'Responsável Demo REN', N'507900015', N'PT507900015', N'fiscal+ren@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida dos Estados Unidos da América', N'Alvalade', N'Lisboa', N'Lisboa', N'1749-061', N'55', N'Piso Demo', N'Responsável Demo REN'),
(N'GD-016', 4, N'WhatsApp', N'[GeritDemoSeed:GD-016] Empresa real em Portugal - CTT - Correios de Portugal, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'CTT - Correios de Portugal, S.A.', N'CTT', N'351210016', N'911100016', 1, N'demo+ctt@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.ctt.pt', N'500900016', N'53100', 12000, N'Responsável Demo CTT', N'507900016', N'PT507900016', N'fiscal+ctt@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida D. João II', N'Parque das Nações', N'Lisboa', N'Lisboa', N'1999-001', N'13', N'Torre Demo', N'Responsável Demo CTT'),
(N'GD-017', 4, N'Facebook', N'[GeritDemoSeed:GD-017] Empresa real em Portugal - Banco Comercial Português, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Banco Comercial Português, S.A.', N'Millennium bcp', N'351210017', N'911100017', 1, N'demo+bcp@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.millenniumbcp.pt', N'500900017', N'64190', 7000, N'Responsável Demo BCP', N'507900017', N'PT507900017', N'fiscal+bcp@gerit.pt', 1, @CommercialAddressTypeId, N'Praça D. João I', N'Baixa', N'Porto', N'Porto', N'4000-295', N'28', N'Agência Demo', N'Responsável Demo BCP'),
(N'GD-018', 4, N'LinkedIn', N'[GeritDemoSeed:GD-018] Empresa real em Portugal - Corticeira Amorim, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Corticeira Amorim, SGPS, S.A.', N'Corticeira Amorim', N'351220018', N'911100018', 1, N'demo+amorim@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.amorim.com', N'500900018', N'16294', 4000, N'Responsável Demo Amorim', N'507900018', N'PT507900018', N'fiscal+amorim@gerit.pt', 1, @CommercialAddressTypeId, N'Rua de Meladas', N'Mozelos', N'Santa Maria da Feira', N'Aveiro', N'4535-186', N'260', N'Unidade Demo', N'Responsável Demo Amorim'),
(N'GD-019', 4, N'Google', N'[GeritDemoSeed:GD-019] Empresa real em Portugal - The Navigator Company, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'The Navigator Company, S.A.', N'The Navigator Company', N'351210019', N'911100019', 1, N'demo+navigator@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.thenavigatorcompany.com', N'500900019', N'17120', 3000, N'Responsável Demo Navigator', N'507900019', N'PT507900019', N'fiscal+navigator@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida Fontes Pereira de Melo', N'Avenidas Novas', N'Lisboa', N'Lisboa', N'1050-120', N'27', N'Escritório Demo', N'Responsável Demo Navigator'),
(N'GD-020', 4, N'Eventos', N'[GeritDemoSeed:GD-020] Empresa real em Portugal - Semapa - Sociedade de Investimento e Gestão, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Semapa - Sociedade de Investimento e Gestão, SGPS, S.A.', N'Semapa', N'351210020', N'911100020', 1, N'demo+semapa@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.semapa.pt', N'500900020', N'64202', 200, N'Responsável Demo Semapa', N'507900020', N'PT507900020', N'fiscal+semapa@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida Fontes Pereira de Melo', N'Avenidas Novas', N'Lisboa', N'Lisboa', N'1050-121', N'14', N'Piso Demo', N'Responsável Demo Semapa'),
(N'GD-021', 4, N'LinkedIn', N'[GeritDemoSeed:GD-021] Empresa real em Portugal - Altri, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Altri, SGPS, S.A.', N'Altri', N'351220021', N'911100021', 1, N'demo+altri@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.altri.pt', N'500900021', N'17110', 800, N'Responsável Demo Altri', N'507900021', N'PT507900021', N'fiscal+altri@gerit.pt', 1, @CommercialAddressTypeId, N'Rua Manuel Pinto de Azevedo', N'Ramalde', N'Porto', N'Porto', N'4100-320', N'818', N'Escritório Demo', N'Responsável Demo Altri'),
(N'GD-022', 4, N'Google', N'[GeritDemoSeed:GD-022] Empresa real em Portugal - Ibersol, SGPS, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Ibersol, SGPS, S.A.', N'Ibersol', N'351220022', N'911100022', 1, N'demo+ibersol@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.ibersol.pt', N'500900022', N'56101', 6000, N'Responsável Demo Ibersol', N'507900022', N'PT507900022', N'fiscal+ibersol@gerit.pt', 1, @CommercialAddressTypeId, N'Praça do Bom Sucesso', N'Boavista', N'Porto', N'Porto', N'4150-146', N'105', N'Loja Demo', N'Responsável Demo Ibersol'),
(N'GD-023', 4, N'WhatsApp', N'[GeritDemoSeed:GD-023] Empresa real em Portugal - EDP Renováveis, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'EDP Renováveis, S.A.', N'EDP Renováveis', N'351210023', N'911100023', 1, N'demo+edpr@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.edpr.com', N'500900023', N'35113', 2500, N'Responsável Demo EDPR', N'507900023', N'PT507900023', N'fiscal+edpr@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida da Boavista', N'Boavista', N'Porto', N'Porto', N'4100-130', N'3433', N'Piso Demo', N'Responsável Demo EDPR'),
(N'GD-024', 4, N'Eventos', N'[GeritDemoSeed:GD-024] Empresa real em Portugal - TAP - Transportes Aéreos Portugueses, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'TAP - Transportes Aéreos Portugueses, S.A.', N'TAP Air Portugal', N'351210024', N'911100024', 1, N'demo+tap@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.flytap.com', N'500900024', N'51100', 8000, N'Responsável Demo TAP', N'507900024', N'PT507900024', N'fiscal+tap@gerit.pt', 1, @CommercialAddressTypeId, N'Aeroporto Humberto Delgado', N'Olivais', N'Lisboa', N'Lisboa', N'1700-008', N'0', N'Terminal Demo', N'Responsável Demo TAP'),
(N'GD-025', 4, N'Google', N'[GeritDemoSeed:GD-025] Empresa real em Portugal - Caixa Geral de Depósitos, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Caixa Geral de Depósitos, S.A.', N'CGD', N'351210025', N'911100025', 1, N'demo+cgd@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.cgd.pt', N'500900025', N'64190', 6000, N'Responsável Demo CGD', N'507900025', N'PT507900025', N'fiscal+cgd@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida João XXI', N'Areeiro', N'Lisboa', N'Lisboa', N'1000-300', N'63', N'Sede Demo', N'Responsável Demo CGD'),
(N'GD-026', 4, N'LinkedIn', N'[GeritDemoSeed:GD-026] Empresa real em Portugal - Vodafone Portugal - Comunicações Pessoais, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Vodafone Portugal - Comunicações Pessoais, S.A.', N'Vodafone Portugal', N'351210026', N'911100026', 1, N'demo+vodafone@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.vodafone.pt', N'500900026', N'61200', 1500, N'Responsável Demo Vodafone', N'507900026', N'PT507900026', N'fiscal+vodafone@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida D. João II', N'Parque das Nações', N'Lisboa', N'Lisboa', N'1998-017', N'36', N'Piso Demo', N'Responsável Demo Vodafone'),
(N'GD-027', 4, N'Google', N'[GeritDemoSeed:GD-027] Empresa real em Portugal - Altice Portugal, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Altice Portugal, S.A.', N'Altice Portugal', N'351210027', N'911100027', 1, N'demo+altice@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.telecom.pt', N'500900027', N'61100', 8000, N'Responsável Demo Altice', N'507900027', N'PT507900027', N'fiscal+altice@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida Fontes Pereira de Melo', N'Avenidas Novas', N'Lisboa', N'Lisboa', N'1069-300', N'40', N'Escritório Demo', N'Responsável Demo Altice'),
(N'GD-028', 4, N'Facebook', N'[GeritDemoSeed:GD-028] Empresa real em Portugal - Super Bock Bebidas, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Super Bock Bebidas, S.A.', N'Super Bock Group', N'351220028', N'911100028', 1, N'demo+superbock@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.superbockgroup.com', N'500900028', N'11050', 1200, N'Responsável Demo Super Bock', N'507900028', N'PT507900028', N'fiscal+superbock@gerit.pt', 1, @CommercialAddressTypeId, N'Via Norte', N'Leça do Balio', N'Matosinhos', N'Porto', N'4465-764', N'0', N'Unidade Demo', N'Responsável Demo Super Bock'),
(N'GD-029', 4, N'Eventos', N'[GeritDemoSeed:GD-029] Empresa real em Portugal - Delta Cafés, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Delta Cafés, S.A.', N'Delta Cafés', N'351245029', N'911100029', 1, N'demo+delta@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.deltacafes.pt', N'500900029', N'10830', 3000, N'Responsável Demo Delta', N'507900029', N'PT507900029', N'fiscal+delta@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida Calouste Gulbenkian', N'Campo Maior', N'Campo Maior', N'Portalegre', N'7370-025', N'0', N'Unidade Demo', N'Responsável Demo Delta'),
(N'GD-030', 4, N'Amigos', N'[GeritDemoSeed:GD-030] Empresa real em Portugal - Sumol+Compal Marcas, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Sumol+Compal Marcas, S.A.', N'Sumol Compal', N'351210030', N'911100030', 1, N'demo+sumolcompal@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.sumolcompal.pt', N'500900030', N'11070', 1200, N'Responsável Demo Sumol Compal', N'507900030', N'PT507900030', N'fiscal+sumolcompal@gerit.pt', 1, @CommercialAddressTypeId, N'Estrada da Portela', N'Carnaxide', N'Oeiras', N'Lisboa', N'2790-124', N'9', N'Escritório Demo', N'Responsável Demo Sumol Compal'),
(N'GD-031', 4, N'LinkedIn', N'[GeritDemoSeed:GD-031] Empresa real em Portugal - Vista Alegre Atlantis, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Vista Alegre Atlantis, S.A.', N'Vista Alegre', N'351234031', N'911100031', 1, N'demo+vistaalegre@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://vistaalegre.com', N'500900031', N'23410', 900, N'Responsável Demo Vista Alegre', N'507900031', N'PT507900031', N'fiscal+vistaalegre@gerit.pt', 1, @CommercialAddressTypeId, N'Lugar da Vista Alegre', N'Ílhavo', N'Ílhavo', N'Aveiro', N'3830-292', N'0', N'Fábrica Demo', N'Responsável Demo Vista Alegre'),
(N'GD-032', 4, N'Google', N'[GeritDemoSeed:GD-032] Empresa real em Portugal - Critical Software, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Critical Software, S.A.', N'Critical Software', N'351239032', N'911100032', 1, N'demo+criticalsoftware@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.criticalsoftware.com', N'500900032', N'62010', 1200, N'Responsável Demo Critical', N'507900032', N'PT507900032', N'fiscal+criticalsoftware@gerit.pt', 1, @CommercialAddressTypeId, N'Parque Industrial de Taveiro', N'Taveiro', N'Coimbra', N'Coimbra', N'3045-504', N'0', N'Edifício Demo', N'Responsável Demo Critical'),
(N'GD-033', 4, N'Eventos', N'[GeritDemoSeed:GD-033] Empresa real em Portugal - Farfetch Portugal, Unipessoal Lda (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Farfetch Portugal, Unipessoal Lda', N'Farfetch Portugal', N'351220033', N'911100033', 1, N'demo+farfetch@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.farfetch.com', N'500900033', N'62010', 2500, N'Responsável Demo Farfetch', N'507900033', N'PT507900033', N'fiscal+farfetch@gerit.pt', 1, @CommercialAddressTypeId, N'Rua da Lionesa', N'Leça do Balio', N'Matosinhos', N'Porto', N'4465-671', N'446', N'Hub Demo', N'Responsável Demo Farfetch'),
(N'GD-034', 4, N'LinkedIn', N'[GeritDemoSeed:GD-034] Empresa real em Portugal - OutSystems - Software em Rede, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'OutSystems - Software em Rede, S.A.', N'OutSystems', N'351210034', N'911100034', 1, N'demo+outsystems@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://www.outsystems.com', N'500900034', N'62010', 2000, N'Responsável Demo OutSystems', N'507900034', N'PT507900034', N'fiscal+outsystems@gerit.pt', 1, @CommercialAddressTypeId, N'Rua Central Park', N'Linda-a-Velha', N'Oeiras', N'Lisboa', N'2795-242', N'2', N'Escritório Demo', N'Responsável Demo OutSystems'),
(N'GD-035', 4, N'Google', N'[GeritDemoSeed:GD-035] Empresa real em Portugal - Feedzai - Consultadoria e Inovação Tecnológica, S.A. (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Feedzai - Consultadoria e Inovação Tecnológica, S.A.', N'Feedzai', N'351239035', N'911100035', 1, N'demo+feedzai@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://feedzai.com', N'500900035', N'62010', 600, N'Responsável Demo Feedzai', N'507900035', N'PT507900035', N'fiscal+feedzai@gerit.pt', 1, @CommercialAddressTypeId, N'Rua Pedro Nunes', N'Coimbra', N'Coimbra', N'Coimbra', N'3030-199', N'0', N'Escritório Demo', N'Responsável Demo Feedzai'),
(N'GD-036', 4, N'WhatsApp', N'[GeritDemoSeed:GD-036] Empresa real em Portugal - Unbabel, Unipessoal Lda (dados de contacto/fiscais sintéticos para demo)', NULL, NULL, NULL, N'Unbabel, Unipessoal Lda', N'Unbabel', N'351210036', N'911100036', 1, N'demo+unbabel@gerit.pt', NULL, NULL, NULL, NULL, NULL, N'https://unbabel.com', N'500900036', N'62020', 400, N'Responsável Demo Unbabel', N'507900036', N'PT507900036', N'fiscal+unbabel@gerit.pt', 1, @CommercialAddressTypeId, N'Avenida da República', N'Avenidas Novas', N'Lisboa', N'Lisboa', N'1050-191', N'45', N'Escritório Demo', N'Responsável Demo Unbabel');


INSERT INTO dbo.Clients (TenantId, AcquisitionSourceTypeId, ClientType, UrlImage, Note, IsActive, IsDeleted, CreatedBy, CreatedAt)
SELECT
    @TenantId,
    ast.Id,
    s.ClientType,
    NULL,
    s.Note,
    1,
    0,
    @CreatedBy,
    SYSDATETIME()
FROM @SeedClients s
INNER JOIN dbo.AcquisitionSourceTypes ast
    ON ast.Name = s.AcquisitionSourceTypeName
   AND ast.IsDeleted = 0
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Clients c
    WHERE c.TenantId = @TenantId
      AND c.Note = s.Note
);

UPDATE s
SET ClientId = c.Id
FROM @SeedClients s
INNER JOIN dbo.Clients c
    ON c.TenantId = @TenantId
   AND c.Note = s.Note
   AND c.IsDeleted = 0;

INSERT INTO dbo.ClientIndividuals
(
    TenantId, ClientId, FullName, FirstName, LastName, PhoneNumber, CellPhoneNumber, IsWhatsapp, Email,
    BirthDate, Gender, DocumentType, DocumentNumber, Nationality,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.FullName, s.FirstName, s.LastName, s.PhoneNumber, s.CellPhoneNumber, s.IsWhatsapp, s.Email,
    s.BirthDate, s.Gender, s.DocumentType, s.DocumentNumber, s.Nationality,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientType IN (1, 2, 3)
  AND s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientIndividuals ci
      WHERE ci.TenantId = @TenantId
        AND ci.ClientId = s.ClientId
        AND ci.IsDeleted = 0
  );

INSERT INTO dbo.ClientCompanies
(
    TenantId, ClientId, LegalName, TradeName, PhoneNumber, CellPhoneNumber, IsWhatsapp, Email,
    Site, CompanyRegistration, CAE, NumberOfEmployee, LegalRepresentative,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.LegalName, s.TradeName, s.PhoneNumber, s.CellPhoneNumber, s.IsWhatsapp, s.Email,
    s.Site, s.CompanyRegistration, s.CAE, s.NumberOfEmployee, s.LegalRepresentative,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientType IN (4, 5)
  AND s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientCompanies cc
      WHERE cc.TenantId = @TenantId
        AND cc.ClientId = s.ClientId
        AND cc.IsDeleted = 0
  );

INSERT INTO dbo.ClientFiscalData
(
    TenantId, ClientId, TaxNumber, VatNumber, FiscalCountry, IsVatRegistered, IBAN, FiscalEmail,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.TaxNumber, s.VatNumber, 'PT', s.IsVatRegistered, NULL, s.FiscalEmail,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientFiscalData fd
      WHERE fd.TenantId = @TenantId
        AND fd.ClientId = s.ClientId
        AND fd.IsDeleted = 0
  );

INSERT INTO dbo.ClientAddresses
(
    TenantId, ClientId, AddressTypeId, CountryCode, Street, Neighborhood, City, District, PostalCode,
    StreetNumber, Complement, Latitude, Longitude, Note, IsPrimary,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.AddressTypeId, 'PT', s.Street, s.Neighborhood, s.City, s.District, s.PostalCode,
    s.StreetNumber, s.Complement, NULL, NULL, N'Endereço principal criado pelo seed Gerit Demo. Dados sintéticos para demonstração.', 1,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientAddresses a
      WHERE a.TenantId = @TenantId
        AND a.ClientId = s.ClientId
        AND a.IsPrimary = 1
        AND a.IsDeleted = 0
  );

INSERT INTO dbo.ClientContacts
(
    TenantId, ClientId, Name, PhoneNumber, CellPhoneNumber, IsWhatsapp, Email, IsPrimary,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId, s.ClientId, s.ContactName, s.PhoneNumber, s.CellPhoneNumber, s.IsWhatsapp, CONVERT(NVARCHAR(255), s.Email), 1,
    1, 0, @CreatedBy, SYSDATETIME()
FROM @SeedClients s
WHERE s.ClientId IS NOT NULL
  AND s.Email IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientContacts ct
      WHERE ct.TenantId = @TenantId
        AND ct.ClientId = s.ClientId
        AND ct.Email = CONVERT(NVARCHAR(255), s.Email)
        AND ct.IsDeleted = 0
  );



/*
    ClientConsents - Gerit Demo Lda
    - Idempotente por Tenant + Cliente + Tipo de consentimento + Origem.
    - Usa Name nos catálogos ConsentTypes e ConsentOriginTypes.
*/
INSERT INTO dbo.ClientConsents
(
    TenantId, ClientId, ConsentTypeId, ConsentOriginTypeId, Granted, GrantedDate,
    RevokedDate, IpAddress, UserAgent,
    IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT
    @TenantId,
    s.ClientId,
    ct.Id,
    cot.Id,
    v.Granted,
    DATEADD(DAY, v.DaysOffset, SYSDATETIME()),
    NULL,
    v.IpAddress,
    v.UserAgent,
    1,
    0,
    @CreatedBy,
    SYSDATETIME()
FROM @SeedClients s
CROSS APPLY (VALUES
    (N'PrivacyPolicy',   N'Backoffice', CONVERT(BIT, 1), -30, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed Gerit Demo - política de privacidade aceite no backoffice.'),
    (N'TermsOfService',  N'Backoffice', CONVERT(BIT, 1), -30, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed Gerit Demo - termos de serviço aceites no backoffice.'),
    (N'DataProcessing',  N'Backoffice', CONVERT(BIT, 1), -29, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed Gerit Demo - tratamento de dados aceite no backoffice.'),
    (N'Marketing',       N'Email',      CONVERT(BIT, 1), -20, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed Gerit Demo - opt-in de marketing por email.'),
    (N'EmailConsent',    N'Email',      CONVERT(BIT, 1), -20, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed Gerit Demo - consentimento de comunicação por email.'),
    (N'Cookies',         N'Web',        CONVERT(BIT, 1), -15, CONVERT(VARCHAR(45), '127.0.0.1'), N'Initial seed Gerit Demo - cookies aceites na web.')
) AS v(ConsentTypeName, ConsentOriginTypeName, Granted, DaysOffset, IpAddress, UserAgent)
INNER JOIN dbo.ConsentTypes ct
    ON ct.Name = v.ConsentTypeName
   AND ct.IsDeleted = 0
INNER JOIN dbo.ConsentOriginTypes cot
    ON cot.Name = v.ConsentOriginTypeName
   AND cot.IsDeleted = 0
WHERE s.ClientId IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.ClientConsents cc
      WHERE cc.TenantId = @TenantId
        AND cc.ClientId = s.ClientId
        AND cc.ConsentTypeId = ct.Id
        AND cc.ConsentOriginTypeId = cot.Id
        AND cc.IsDeleted = 0
  );

GO
