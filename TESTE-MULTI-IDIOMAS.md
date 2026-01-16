# ?? Guia de Teste - Sistema Multi-idiomas Swagger

## ?? Pré-requisitos

- Aplicação rodando em ambiente de desenvolvimento
- Navegador web (Chrome, Firefox, Edge)
- Console do navegador aberto (F12)

---

## ? Lista de Verificação

### 1. Teste do Seletor de Idiomas

#### 1.1 Visualização do Seletor
- [ ] Acesse `https://localhost:{porta}/swagger`
- [ ] Verifique se o seletor de idiomas aparece no canto superior direito
- [ ] Confirme que ele contém as opções: Português, English, Español

#### 1.2 Troca de Idiomas
- [ ] Selecione "Português" no dropdown
- [ ] Verifique se a página recarrega automaticamente
- [ ] Confirme que os textos estão em português
- [ ] Selecione "English" no dropdown
- [ ] Verifique se a página recarrega automaticamente
- [ ] Confirme que os textos estão em inglês
- [ ] Selecione "Español" no dropdown
- [ ] Verifique se a página recarrega automaticamente
- [ ] Confirme que os textos estão em espanhol

#### 1.3 Persistência da Preferência
- [ ] Selecione um idioma diferente do padrão
- [ ] Feche a aba do Swagger
- [ ] Abra novamente `https://localhost:{porta}/swagger`
- [ ] Confirme que o idioma selecionado foi mantido

---

### 2. Teste de URLs com Parâmetro Lang

#### 2.1 Português (pt-BR)
- [ ] Acesse `https://localhost:{porta}/swagger?lang=pt-BR`
- [ ] Verifique se a interface está em português
- [ ] Confirme que o seletor mostra "Português" selecionado

#### 2.2 Inglês (en-US)
- [ ] Acesse `https://localhost:{porta}/swagger?lang=en-US`
- [ ] Verifique se a interface está em inglês
- [ ] Confirme que o seletor mostra "English" selecionado

#### 2.3 Espanhol (es-ES)
- [ ] Acesse `https://localhost:{porta}/swagger?lang=es-ES`
- [ ] Verifique se a interface está em espanhol
- [ ] Confirme que o seletor mostra "Español" selecionado

---

### 3. Teste de Tradução do Conteúdo

#### 3.1 Cabeçalho da API
- [ ] Verifique o título da API em português: "API Gerit VianaHub - Desenvolvimento"
- [ ] Verifique o título da API em inglês: "Gerit API VianaHub - Development"
- [ ] Verifique o título da API em espanhol: "API Gerit VianaHub - Desarrollo"

#### 3.2 Endpoints
- [ ] Expanda o endpoint `/health` em português
- [ ] Verifique o summary: "Verificação de Saúde"
- [ ] Mude para inglês e verifique: "Health Check"
- [ ] Mude para espanhol e verifique: "Verificación de Salud"

#### 3.3 Tags
- [ ] Verifique a tradução da tag "Actions" em português: "Ações"
- [ ] Verifique a tradução da tag "Health" em português: "Saúde"
- [ ] Verifique a tradução em inglês (mantém "Actions" e "Health")
- [ ] Verifique a tradução em espanhol: "Acciones" e "Salud"

#### 3.4 Respostas HTTP
- [ ] Expanda qualquer endpoint
- [ ] Verifique a descrição do código 200 em português: "Sucesso"
- [ ] Verifique em inglês: "Success"
- [ ] Verifique em espanhol: "Éxito"

---

### 4. Teste de Console e Logs

#### 4.1 Console do Navegador
- [ ] Abra o console do navegador (F12 ? Console)
- [ ] Acesse o Swagger
- [ ] Verifique a mensagem: "Swagger UI Language Switcher loaded"
- [ ] Mude o idioma
- [ ] Verifique as mensagens de log durante a troca

#### 4.2 Logs do Servidor
- [ ] Monitore os logs da aplicação
- [ ] Acesse o Swagger
- [ ] Procure por logs com prefixo `[Gerit:SwaggerLocalization]`
- [ ] Procure por logs com prefixo `[Gerit:SwaggerTranslation]`
- [ ] Confirme que não há erros ou warnings

---

### 5. Teste de Cookies

#### 5.1 Criação do Cookie
- [ ] Limpe todos os cookies do site
- [ ] Acesse o Swagger
- [ ] Selecione um idioma
- [ ] Abra as DevTools ? Application ? Cookies
- [ ] Confirme que existe um cookie `swagger-locale` com o valor do idioma

#### 5.2 Leitura do Cookie
- [ ] Feche o navegador
- [ ] Abra novamente
- [ ] Acesse o Swagger sem parâmetro lang
- [ ] Confirme que o idioma salvo no cookie foi aplicado

---

### 6. Teste de Fallback

#### 6.1 Idioma Não Suportado
- [ ] Acesse `https://localhost:{porta}/swagger?lang=fr-FR`
- [ ] Verifique se o sistema faz fallback para pt-BR
- [ ] Confirme que não há erros no console

#### 6.2 Sem Parâmetro Lang
- [ ] Limpe todos os cookies
- [ ] Configure o navegador para preferir inglês
- [ ] Acesse `https://localhost:{porta}/swagger`
- [ ] Verifique se o sistema detectou a preferência do navegador
- [ ] Se não houver preferência configurada, deve usar pt-BR como padrão

---

### 7. Teste de Arquivos Estáticos

#### 7.1 CSS Customizado
- [ ] Acesse o Swagger
- [ ] Abra DevTools ? Elements
- [ ] Procure por `<link href="/swagger-ui/custom.css">`
- [ ] Verifique se o arquivo foi carregado (Status 200)
- [ ] Inspecione o seletor de idiomas e confirme os estilos aplicados

#### 7.2 JavaScript Customizado
- [ ] Acesse o Swagger
- [ ] Abra DevTools ? Network
- [ ] Filtre por JS
- [ ] Confirme que `custom.js` foi carregado (Status 200)
- [ ] Verifique se não há erros de JavaScript no console

---

### 8. Teste de Endpoints da API

#### 8.1 Endpoint Health
- [ ] Acesse o Swagger em português
- [ ] Execute o endpoint `/health`
- [ ] Verifique se a resposta está correta
- [ ] Confirme que o idioma da interface não afeta a resposta da API

#### 8.2 Endpoints Actions
- [ ] Expanda o grupo "Ações" (ou "Actions" em inglês)
- [ ] Verifique se todos os endpoints estão traduzidos
- [ ] Execute um endpoint (ex: GET /v1/actions/paged)
- [ ] Confirme que funciona independentemente do idioma

---

## ?? Problemas Conhecidos e Soluções

### Seletor não aparece
**Problema:** O seletor de idiomas não aparece no Swagger UI  
**Solução:** 
1. Verifique se os arquivos `custom.css` e `custom.js` estão na pasta `wwwroot/swagger-ui/`
2. Confirme que o `UseStaticFiles()` está configurado no `Program.cs`
3. Verifique o console do navegador para erros de carregamento

### Traduções não aplicadas
**Problema:** As traduções não estão sendo aplicadas ao documento Swagger  
**Solução:**
1. Verifique se os arquivos JSON estão na pasta `Localization/`
2. Confirme que o `SwaggerTranslationFilter` está registrado
3. Verifique os logs do servidor para erros de leitura dos arquivos

### Cookie não persiste
**Problema:** A preferência de idioma não é salva entre sessões  
**Solução:**
1. Verifique se o cookie `swagger-locale` está sendo criado
2. Confirme que o SameSite está configurado como `Lax`
3. Verifique se o navegador não está bloqueando cookies

---

## ?? Resultado Esperado

? **Tudo funcionando corretamente se:**
- O seletor de idiomas aparece e funciona
- As traduções são aplicadas corretamente
- A preferência é salva em cookie
- Não há erros no console ou nos logs
- Os arquivos estáticos são servidos corretamente
- Os endpoints funcionam independentemente do idioma

---

## ?? Suporte

Em caso de problemas, verifique:
1. Logs da aplicação (logs/gerit-dev-.log)
2. Console do navegador (F12)
3. Network tab do DevTools
4. README.md da pasta wwwroot/swagger-ui/
