document.addEventListener("DOMContentLoaded", () => {
  const filterPanel = document.querySelector("#bookFilterPanel");
  const toggles = document.querySelectorAll("[data-filter-toggle]");
  const navToggle = document.querySelector("[data-nav-toggle]");
  const navMenu = document.querySelector("[data-nav-menu]");

  toggles.forEach((toggle) => {
    toggle.addEventListener("click", () => {
      filterPanel?.classList.toggle("is-open");
    });
  });

  navToggle?.addEventListener("click", () => {
    const isOpen = navMenu?.classList.toggle("is-open") ?? false;
    navToggle.setAttribute("aria-expanded", isOpen.toString());
  });
});
