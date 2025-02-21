(pages) => {
    document.querySelectorAll('.toc-page').forEach(el => {
        const sectionId = el.dataset.sectionId;
        if (pages[sectionId]) {
            el.textContent = pages[sectionId];
        }
    });
}