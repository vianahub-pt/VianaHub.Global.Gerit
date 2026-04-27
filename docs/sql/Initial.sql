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
    TenantType,
    OriginType,
    Name,
    Email,
    Website,
    UrlImage,
    Note,
    CreatedBy
)
SELECT *
FROM (VALUES
    (2, 1, N'VianaHub Lda', N'contact@vianahub.pt', N'https://vianahub.pt', NULL, N'Tenant principal', 1),
    (2, 1, N'Gerit Demo Lda', N'demo@gerit.pt', N'https://demo.gerit.pt', NULL, N'Ambiente de demonstração', 1),
    (2, 1, N'Teste Lda', N'teste@teste.pt', NULL, NULL, N'Tenant para testes internos', 1)
) AS v(
    TenantType,
    OriginType,
    Name,
    Email,
    Website,
    UrlImage,
    Note,
    CreatedBy
)
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
    Note,
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
    Note,
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
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
INSERT INTO [dbo].[RolePermissions]
           ([TenantId]
           ,[RoleId]
           ,[ResourceId]
           ,[ActionId])
VALUES
(1, 2, 1,1),
(1, 2, 1,2),
(1, 2, 1,3),
(1, 2, 1,4),
(1, 2, 1,5),
(1, 2, 1,6),
(1, 2, 1,7),
(1, 2, 1,8),
(1, 2, 1,9),
(1, 2, 2,1),
(1, 2, 2,2),
(1, 2, 2,3),
(1, 2, 2,4),
(1, 2, 2,5),
(1, 2, 2,6),
(1, 2, 2,7),
(1, 2, 2,8),
(1, 2, 2,9),
(1, 2, 3,1),
(1, 2, 3,2),
(1, 2, 3,3),
(1, 2, 3,4),
(1, 2, 3,5),
(1, 2, 3,6),
(1, 2, 3,7),
(1, 2, 3,8),
(1, 2, 3,9),
(1, 2, 4,1),
(1, 2, 4,2),
(1, 2, 4,3),
(1, 2, 4,4),
(1, 2, 4,5),
(1, 2, 4,6),
(1, 2, 4,7),
(1, 2, 4,8),
(1, 2, 4,9),
(1, 2, 5,1),
(1, 2, 5,2),
(1, 2, 5,3),
(1, 2, 5,4),
(1, 2, 5,5),
(1, 2, 5,6),
(1, 2, 5,7),
(1, 2, 5,8),
(1, 2, 5,9),
(1, 2, 6,1),
(1, 2, 6,2),
(1, 2, 6,3),
(1, 2, 6,4),
(1, 2, 6,5),
(1, 2, 6,6),
(1, 2, 6,7),
(1, 2, 6,8),
(1, 2, 6,9),
(1, 2, 7,1),
(1, 2, 7,2),
(1, 2, 7,3),
(1, 2, 7,4),
(1, 2, 7,5),
(1, 2, 7,6),
(1, 2, 7,7),
(1, 2, 7,8),
(1, 2, 7,9),
(1, 2, 8,1),
(1, 2, 8,2),
(1, 2, 8,3),
(1, 2, 8,4),
(1, 2, 8,5),
(1, 2, 8,6),
(1, 2, 8,7),
(1, 2, 8,8),
(1, 2, 8,9),
(1, 2, 9,1),
(1, 2, 9,2),
(1, 2, 9,3),
(1, 2, 9,4),
(1, 2, 9,5),
(1, 2, 9,6),
(1, 2, 9,7),
(1, 2, 9,8),
(1, 2, 9,9),
(1, 2, 10,1),
(1, 2, 10,2),
(1, 2, 10,3),
(1, 2, 10,4),
(1, 2, 10,5),
(1, 2, 10,6),
(1, 2, 10,7),
(1, 2, 10,8),
(1, 2, 10,9),
(1, 2, 11,1),
(1, 2, 11,2),
(1, 2, 11,3),
(1, 2, 11,4),
(1, 2, 11,5),
(1, 2, 11,6),
(1, 2, 11,7),
(1, 2, 11,8),
(1, 2, 11,9),
(1, 2, 12,1),
(1, 2, 12,2),
(1, 2, 12,3),
(1, 2, 12,4),
(1, 2, 12,5),
(1, 2, 12,6),
(1, 2, 12,7),
(1, 2, 12,8),
(1, 2, 12,9),
(1, 2, 13,1),
(1, 2, 13,2),
(1, 2, 13,3),
(1, 2, 13,4),
(1, 2, 13,5),
(1, 2, 13,6),
(1, 2, 13,7),
(1, 2, 13,8),
(1, 2, 13,9),
(1, 2, 14,1),
(1, 2, 14,2),
(1, 2, 14,3),
(1, 2, 14,4),
(1, 2, 14,5),
(1, 2, 14,6),
(1, 2, 14,7),
(1, 2, 14,8),
(1, 2, 14,9),
(1, 2, 15,1),
(1, 2, 15,2),
(1, 2, 15,3),
(1, 2, 15,4),
(1, 2, 15,5),
(1, 2, 15,6),
(1, 2, 15,7),
(1, 2, 15,8),
(1, 2, 15,9),
(1, 2, 16,1),
(1, 2, 16,2),
(1, 2, 16,3),
(1, 2, 16,4),
(1, 2, 16,5),
(1, 2, 16,6),
(1, 2, 16,7),
(1, 2, 16,8),
(1, 2, 16,9),
(1, 2, 17,1),
(1, 2, 17,2),
(1, 2, 17,3),
(1, 2, 17,4),
(1, 2, 17,5),
(1, 2, 17,6),
(1, 2, 17,7),
(1, 2, 17,8),
(1, 2, 17,9),
(1, 2, 18,1),
(1, 2, 18,2),
(1, 2, 18,3),
(1, 2, 18,4),
(1, 2, 18,5),
(1, 2, 18,6),
(1, 2, 18,7),
(1, 2, 18,8),
(1, 2, 18,9),
(1, 2, 19,1),
(1, 2, 19,2),
(1, 2, 19,3),
(1, 2, 19,4),
(1, 2, 19,5),
(1, 2, 19,6),
(1, 2, 19,7),
(1, 2, 19,8),
(1, 2, 19,9),
(1, 2, 20,1),
(1, 2, 20,2),
(1, 2, 20,3),
(1, 2, 20,4),
(1, 2, 20,5),
(1, 2, 20,6),
(1, 2, 20,7),
(1, 2, 20,8),
(1, 2, 20,9),
(1, 2, 21,1),
(1, 2, 21,2),
(1, 2, 21,3),
(1, 2, 21,4),
(1, 2, 21,5),
(1, 2, 21,6),
(1, 2, 21,7),
(1, 2, 21,8),
(1, 2, 21,9),
(1, 2, 22,1),
(1, 2, 22,2),
(1, 2, 22,3),
(1, 2, 22,4),
(1, 2, 22,5),
(1, 2, 22,6),
(1, 2, 22,7),
(1, 2, 22,8),
(1, 2, 22,9),
(1, 2, 23,1),
(1, 2, 23,2),
(1, 2, 23,3),
(1, 2, 23,4),
(1, 2, 23,5),
(1, 2, 23,6),
(1, 2, 23,7),
(1, 2, 23,8),
(1, 2, 23,9),
(1, 2, 24,1),
(1, 2, 24,2),
(1, 2, 24,3),
(1, 2, 24,4),
(1, 2, 24,5),
(1, 2, 24,6),
(1, 2, 24,7),
(1, 2, 24,8),
(1, 2, 24,9),
(1, 2, 25,1),
(1, 2, 25,2),
(1, 2, 25,3),
(1, 2, 25,4),
(1, 2, 25,5),
(1, 2, 25,6),
(1, 2, 25,7),
(1, 2, 25,8),
(1, 2, 25,9),
(1, 2, 26,1),
(1, 2, 26,2),
(1, 2, 26,3),
(1, 2, 26,4),
(1, 2, 26,5),
(1, 2, 26,6),
(1, 2, 26,7),
(1, 2, 26,8),
(1, 2, 26,9),
(1, 2, 27,1),
(1, 2, 27,2),
(1, 2, 27,3),
(1, 2, 27,4),
(1, 2, 27,5),
(1, 2, 27,6),
(1, 2, 27,7),
(1, 2, 27,8),
(1, 2, 27,9),
(1, 2, 28,1),
(1, 2, 28,2),
(1, 2, 28,3),
(1, 2, 28,4),
(1, 2, 28,5),
(1, 2, 28,6),
(1, 2, 28,7),
(1, 2, 28,8),
(1, 2, 28,9),
(1, 2, 29,1),
(1, 2, 29,2),
(1, 2, 29,3),
(1, 2, 29,4),
(1, 2, 29,5),
(1, 2, 29,6),
(1, 2, 29,7),
(1, 2, 29,8),
(1, 2, 29,9),
(1, 2, 30,1),
(1, 2, 30,2),
(1, 2, 30,3),
(1, 2, 30,4),
(1, 2, 30,5),
(1, 2, 30,6),
(1, 2, 30,7),
(1, 2, 30,8),
(1, 2, 30,9),
(1, 2, 31,1),
(1, 2, 31,2),
(1, 2, 31,3),
(1, 2, 31,4),
(1, 2, 31,5),
(1, 2, 31,6),
(1, 2, 31,7),
(1, 2, 31,8),
(1, 2, 31,9),
(1, 2, 32,1),
(1, 2, 32,2),
(1, 2, 32,3),
(1, 2, 32,4),
(1, 2, 32,5),
(1, 2, 32,6),
(1, 2, 32,7),
(1, 2, 32,8),
(1, 2, 32,9),
(1, 2, 33,1),
(1, 2, 33,2),
(1, 2, 33,3),
(1, 2, 33,4),
(1, 2, 33,5),
(1, 2, 33,6),
(1, 2, 33,7),
(1, 2, 33,8),
(1, 2, 33,9),
(1, 2, 34,1),
(1, 2, 34,2),
(1, 2, 34,3),
(1, 2, 34,4),
(1, 2, 34,5),
(1, 2, 34,6),
(1, 2, 34,7),
(1, 2, 34,8),
(1, 2, 34,9),
(1, 2, 35,1),
(1, 2, 35,2),
(1, 2, 35,3),
(1, 2, 35,4),
(1, 2, 35,5),
(1, 2, 35,6),
(1, 2, 35,7),
(1, 2, 35,8),
(1, 2, 35,9),
(1, 2, 36,1),
(1, 2, 36,2),
(1, 2, 36,3),
(1, 2, 36,4),
(1, 2, 36,5),
(1, 2, 36,6),
(1, 2, 36,7),
(1, 2, 36,8),
(1, 2, 36,9),
(1, 2, 37,1),
(1, 2, 37,2),
(1, 2, 37,3),
(1, 2, 37,4),
(1, 2, 37,5),
(1, 2, 37,6),
(1, 2, 37,7),
(1, 2, 37,8),
(1, 2, 37,9),
(1, 2, 38,1),
(1, 2, 38,2),
(1, 2, 38,3),
(1, 2, 38,4),
(1, 2, 38,5),
(1, 2, 38,6),
(1, 2, 38,7),
(1, 2, 38,8),
(1, 2, 38,9),
(1, 2, 39,1),
(1, 2, 39,2),
(1, 2, 39,3),
(1, 2, 39,4),
(1, 2, 39,5),
(1, 2, 39,6),
(1, 2, 39,7),
(1, 2, 39,8),
(1, 2, 39,9),
(1, 2, 40,1),
(1, 2, 40,2),
(1, 2, 40,3),
(1, 2, 40,4),
(1, 2, 40,5),
(1, 2, 40,6),
(1, 2, 40,7),
(1, 2, 40,8),
(1, 2, 40,9),
(1, 2, 41,1),
(1, 2, 41,2),
(1, 2, 41,3),
(1, 2, 41,4),
(1, 2, 41,5),
(1, 2, 41,6),
(1, 2, 41,7),
(1, 2, 41,8),
(1, 2, 41,9),
(1, 2, 42,1),
(1, 2, 42,2),
(1, 2, 42,3),
(1, 2, 42,4),
(1, 2, 42,5),
(1, 2, 42,6),
(1, 2, 42,7),
(1, 2, 42,8),
(1, 2, 42,9),
(1, 2, 43,1),
(1, 2, 43,2),
(1, 2, 43,3),
(1, 2, 43,4),
(1, 2, 43,5),
(1, 2, 43,6),
(1, 2, 43,7),
(1, 2, 43,8),
(1, 2, 43,9),
(1, 2, 44,1),
(1, 2, 44,2),
(1, 2, 44,3),
(1, 2, 44,4),
(1, 2, 44,5),
(1, 2, 44,6),
(1, 2, 44,7),
(1, 2, 44,8),
(1, 2, 44,9),
(1, 2, 45,1),
(1, 2, 45,2),
(1, 2, 45,3),
(1, 2, 45,4),
(1, 2, 45,5),
(1, 2, 45,6),
(1, 2, 45,7),
(1, 2, 45,8),
(1, 2, 45,9),
(1, 2, 46,1),
(1, 2, 46,2),
(1, 2, 46,3),
(1, 2, 46,4),
(1, 2, 46,5),
(1, 2, 46,6),
(1, 2, 46,7),
(1, 2, 46,8),
(1, 2, 46,9),
(1, 2, 47,1),
(1, 2, 47,2),
(1, 2, 47,3),
(1, 2, 47,4),
(1, 2, 47,5),
(1, 2, 47,6),
(1, 2, 47,7),
(1, 2, 47,8),
(1, 2, 47,9)
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

INSERT INTO dbo.Users (TenantId, Name, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, LastAccessAt, PasswordHash, UrlImage, IsActive, IsDeleted, CreatedBy, CreatedAt
)
SELECT t.Id, f.Name, f.Email, f.NormalizedEmail, f.EmailConfirmed, f.PhoneNumber, f.PhoneNumberConfirmed, f.LastAccessAt, f.PasswordHash, f.UrlImage, 1, 0, 1, GETDATE()
FROM dbo.Tenants t
CROSS JOIN (
              SELECT 'Dener Viana', 'viana.dener@gmail.com', 'VIANA.DENER@GMAIL.COM', 1, '960268353', 0, NULL, 'AQAAAAIAAYagAAAAEOEcNoyHhCnBMw84OU5SZC8S2SZn27KM0TFVOE1r+Y3KhYfBMENuzh1FcBzgSONunQ==', 'www.gerit.pt/users/viana.dener.jpg'
    UNION ALL SELECT 'Admin'      , 'admin@geritapp.com'   , 'ADMIN@GERITAPP.COM'   , 0, NULL       , 0, NULL, 'AQAAAAIAAYagAAAAEPa5FtGlF2OXscP5GHvrRXGkAHMIdGGOa5rJZn/+Z/QlN2AttEYvNvt2seo7IcKvMw==', NULL
    UNION ALL SELECT 'Utilizador' , 'user@geritapp.com'    , 'USER@GERITAPP.COM'    , 0, NULL       , 0, NULL, 'AQAAAAIAAYagAAAAEB/CQwDlmO0A43s7vDArU1L1bHHH9gIYAx6U7fHg7lA1tq1wg0B6vbckMY90+SDU/Q==', NULL
) f (
    Name, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, LastAccessAt, PasswordHash, UrlImage
)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Users fx
    WHERE fx.TenantId = t.Id AND fx.NormalizedEmail = f.NormalizedEmail
);
GO
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
INSERT INTO dbo.UserRoles (TenantId, UserId, RoleId) VALUES 
(1, 1, 2)
GO

