// Custom Swagger UI Language Switcher and Theme Switcher
(function () {
    'use strict';

    console.log('Swagger UI Language and Theme Switcher loaded');

    // Automatically add lang parameter to URL if not present
    (function ensureLangInUrl() {
        const urlParams = new URLSearchParams(window.location.search);
        const currentLang = urlParams.get('lang');

        // If no lang parameter exists, redirect with pt-BR
        if (!currentLang) {
            const defaultLang = getDefaultLanguage();
            console.log('No lang parameter found, redirecting to:', defaultLang);

            // Add lang parameter to current URL
            urlParams.set('lang', defaultLang);
            const newUrl = window.location.pathname + '?' + urlParams.toString();

            // Redirect to new URL with lang parameter
            window.location.replace(newUrl);
            return;
        }

        console.log('Lang parameter already present:', currentLang);
    })();

    // Initialize theme from localStorage
    (function initTheme() {
        const savedTheme = localStorage.getItem('swagger-theme') || 'light';
        if (savedTheme === 'dark') {
            document.body.classList.add('dark-theme');
        }
    })();

    // Wait for Swagger UI to be fully loaded
    window.addEventListener('load', function () {
        setTimeout(initSwitchers, 1000);
    });

    function getDefaultLanguage() {
        // Try to get from cookie first
        const cookieLang = getCookie('swagger-locale');
        if (cookieLang) {
            return cookieLang;
        }

        // Try to detect from browser
        const browserLang = navigator.language || navigator.userLanguage;
        if (browserLang.startsWith('pt')) {
            return 'pt-PT';
        }

        // Default to Portuguese
        return 'pt-PT';
    }

    function initSwitchers() {
        console.log('Initializing language and theme switchers...');

        // Get current language from URL or cookie
        const currentLang = getCurrentLanguage();
        console.log('Current language:', currentLang);

        // Create language and theme switcher UI
        const topbar = document.querySelector('.topbar');
        if (!topbar) {
            console.warn('Topbar not found, retrying...');
            setTimeout(initSwitchers, 500);
            return;
        }

        // Check if switchers already exist
        if (document.getElementById('lang-switcher')) {
            console.log('Switchers already exist');
            return;
        }

        const wrapper = document.querySelector('.topbar-wrapper');
        if (wrapper) {
            const currentTheme = localStorage.getItem('swagger-theme') || 'light';
            const themeIcon = currentTheme === 'dark' 
                ? '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="5"></circle><line x1="12" y1="1" x2="12" y2="3"></line><line x1="12" y1="21" x2="12" y2="23"></line><line x1="4.22" y1="4.22" x2="5.64" y2="5.64"></line><line x1="18.36" y1="18.36" x2="19.78" y2="19.78"></line><line x1="1" y1="12" x2="3" y2="12"></line><line x1="21" y1="12" x2="23" y2="12"></line><line x1="4.22" y1="19.78" x2="5.64" y2="18.36"></line><line x1="18.36" y1="5.64" x2="19.78" y2="4.22"></line></svg>'
                : '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"></path></svg>';

            const switchersHtml = `
                <div id="lang-switcher" class="lang-switcher">
                    <select id="lang-select" class="lang-select" aria-label="Language selector">
                        <option value="pt-PT" ${currentLang === 'pt-PT' ? 'selected' : ''}>Portugu&ecirc;s PT</option>
                        <option value="pt-BR" ${currentLang === 'pt-BR' ? 'selected' : ''}>Portugu&ecirc;s BR</option>
                        <option value="en-US" ${currentLang === 'en-US' ? 'selected' : ''}>English</option>
                        <option value="es-ES" ${currentLang === 'es-ES' ? 'selected' : ''}>Espa&ntilde;ol</option>
                    </select>
                </div>
                <div class="theme-switcher">
                    <button id="theme-toggle" class="theme-toggle" aria-label="Toggle theme" title="Alternar tema">
                        ${themeIcon}
                    </button>
                </div>
            `;

            wrapper.insertAdjacentHTML('beforeend', switchersHtml);

            // Add event listener for language selector
            const langSelect = document.getElementById('lang-select');
            if (langSelect) {
                langSelect.addEventListener('change', function (e) {
                    const newLang = e.target.value;
                    console.log('Switching to language:', newLang);
                    switchLanguage(newLang);
                });
                console.log('Language switcher initialized successfully');
            }

            // Add event listener for theme toggle
            const themeToggle = document.getElementById('theme-toggle');
            if (themeToggle) {
                themeToggle.addEventListener('click', function () {
                    toggleTheme();
                });
                console.log('Theme switcher initialized successfully');
            }
        } else {
            console.warn('Topbar wrapper not found');
        }
    }

    function toggleTheme() {
        const currentTheme = localStorage.getItem('swagger-theme') || 'light';
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        
        localStorage.setItem('swagger-theme', newTheme);
        
        const themeToggle = document.getElementById('theme-toggle');
        if (newTheme === 'dark') {
            document.body.classList.add('dark-theme');
            if (themeToggle) {
                themeToggle.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="5"></circle><line x1="12" y1="1" x2="12" y2="3"></line><line x1="12" y1="21" x2="12" y2="23"></line><line x1="4.22" y1="4.22" x2="5.64" y2="5.64"></line><line x1="18.36" y1="18.36" x2="19.78" y2="19.78"></line><line x1="1" y1="12" x2="3" y2="12"></line><line x1="21" y1="12" x2="23" y2="12"></line><line x1="4.22" y1="19.78" x2="5.64" y2="18.36"></line><line x1="18.36" y1="5.64" x2="19.78" y2="4.22"></line></svg>';
            }
        } else {
            document.body.classList.remove('dark-theme');
            if (themeToggle) {
                themeToggle.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"></path></svg>';
            }
        }
        
        console.log('Theme switched to:', newTheme);
    }

    function getCurrentLanguage() {
        // Try to get from URL first (highest priority)
        const urlParams = new URLSearchParams(window.location.search);
        const langParam = urlParams.get('lang');
        if (langParam) {
            return langParam;
        }

        // Try to get from cookie (second priority)
        const cookieLang = getCookie('swagger-locale');
        if (cookieLang) {
            return cookieLang;
        }

        // Try to detect from browser (third priority)
        const browserLang = navigator.language || navigator.userLanguage;
        if (browserLang.startsWith('pt')) {
            return 'pt-BR';
        }

        // DEFAULT CHANGED: Portuguese as default instead of English
        return 'pt-BR';
    }

    function switchLanguage(lang) {
        // Save preference in cookie
        setCookie('swagger-locale', lang, 365);

        // Update URL with new language parameter
        const urlParams = new URLSearchParams(window.location.search);
        urlParams.set('lang', lang);
        const newUrl = window.location.pathname + '?' + urlParams.toString();

        // Update browser history
        window.history.pushState({}, '', newUrl);

        console.log('URL updated:', newUrl);

        // Get current spec URL
        const ui = window.ui;
        if (!ui) {
            console.error('Swagger UI instance not found');
            return;
        }

        // Get current spec URL and update language parameter
        const currentUrl = ui.specSelectors.url();
        const url = new URL(currentUrl, window.location.origin);
        url.searchParams.set('lang', lang);

        console.log('Loading spec with new language:', url.toString());

        // Update the spec URL to reload with new language
        ui.specActions.updateUrl(url.toString());
        ui.specActions.download(url.toString());
    }

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            return parts.pop().split(';').shift();
        }
        return null;
    }

    function setCookie(name, value, days) {
        let expires = '';
        if (days) {
            const date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = `; expires=${date.toUTCString()}`;
        }
        document.cookie = `${name}=${value}${expires}; path=/; SameSite=Lax`;
        console.log('Cookie set:', name, '=', value);
    }
})();
