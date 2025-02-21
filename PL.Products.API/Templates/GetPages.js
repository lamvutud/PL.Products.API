() => {
    const elements = document.querySelectorAll('h2, h3');
    const pages = {};
    elements.forEach(el => {
        const rect = el.getBoundingClientRect();
        pages[el.id] = Math.ceil(rect.top / 800);
    });
    return pages;
}