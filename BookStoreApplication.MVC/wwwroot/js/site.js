(function () {
    const path = window.location.pathname.toLowerCase();
    document.querySelectorAll('.side-nav a').forEach(a => {
        const href = a.getAttribute('href')?.toLowerCase() || '';
        if (href !== '/' && path.startsWith(href)) a.classList.add('active');
        if (href === '/' && path === '/') a.classList.add('active');
    });
})();
