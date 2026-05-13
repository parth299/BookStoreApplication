document.addEventListener("DOMContentLoaded", () => {
    const path = window.location.pathname.toLowerCase();

    document.querySelectorAll(".side-nav a").forEach((link) => {
        const href = link.getAttribute("href")?.toLowerCase() || "";

        if (href !== "/" && path.startsWith(href)) {
            link.classList.add("active");
        }

        if (href === "/" && path === "/") {
            link.classList.add("active");
        }
    });

    const filterPanel = document.querySelector("#bookFilterPanel");
    const filterToggles = document.querySelectorAll("[data-filter-toggle]");

    filterToggles.forEach((toggle) => {
        toggle.addEventListener("click", () => {
            filterPanel?.classList.toggle("is-open");
        });
    });

    const navToggle = document.querySelector("[data-nav-toggle]");
    const navMenu = document.querySelector("[data-nav-menu]");

    navToggle?.addEventListener("click", () => {
        const isOpen = navMenu?.classList.toggle("is-open") ?? false;
        navToggle.setAttribute("aria-expanded", isOpen.toString());
    });
});