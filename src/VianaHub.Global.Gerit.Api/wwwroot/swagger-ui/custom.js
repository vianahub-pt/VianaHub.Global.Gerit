// Custom Swagger UI Language Switcher
(function () {
    'use strict';

    console.log('Swagger UI Language Switcher loaded');

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

    // Wait for Swagger UI to be fully loaded
    window.addEventListener('load', function () {
        setTimeout(initLanguageSwitcher, 1000);
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
            return 'pt-BR';
        }

        // Default to Portuguese
        return 'pt-BR';
    }

    function initLanguageSwitcher() {
        console.log('Initializing language switcher...');

        // Get current language from URL or cookie
        const currentLang = getCurrentLanguage();
        console.log('Current language:', currentLang);

        // Create language switcher UI
        const topbar = document.querySelector('.topbar');
        if (!topbar) {
            console.warn('Topbar not found, retrying...');
            setTimeout(initLanguageSwitcher, 500);
            return;
        }

        // Check if switcher already exists
        if (document.getElementById('lang-switcher')) {
            console.log('Language switcher already exists');
            return;
        }

        const wrapper = document.querySelector('.topbar-wrapper');
        if (wrapper) {
            const switcherHtml = `
                <div id="lang-switcher" class="lang-switcher">
                    <select id="lang-select" class="lang-select" aria-label="Language selector">
                        <option value="pt-BR" ${currentLang === 'pt-BR' ? 'selected' : ''}>Portugu&ecirc;s</option>
                        <option value="en-US" ${currentLang === 'en-US' ? 'selected' : ''}>English</option>
                        <option value="es-ES" ${currentLang === 'es-ES' ? 'selected' : ''}>Espa&ntilde;ol</option>
                    </select>
                </div>
            `;

            wrapper.insertAdjacentHTML('beforeend', switcherHtml);

            // Add event listener
            const langSelect = document.getElementById('lang-select');
            if (langSelect) {
                langSelect.addEventListener('change', function (e) {
                    const newLang = e.target.value;
                    console.log('Switching to language:', newLang);
                    switchLanguage(newLang);
                });
                console.log('Language switcher initialized successfully');
            }
        } else {
            console.warn('Topbar wrapper not found');
        }
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
