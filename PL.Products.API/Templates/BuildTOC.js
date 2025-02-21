() => {
    const tocList = document.getElementById("toc-list");
    const headers = document.querySelectorAll("h2:not(.no-toc), h3:not(.no-toc)");

    headers.forEach((header, index) => {
        const id = header.id || `section-${index}`;
        header.id = id;

        const tocItem = document.createElement("li");
        tocItem.classList.add("toc-entry");

        const link = document.createElement("a");
        link.href = `#${id}`;
        link.textContent = header.textContent;

        const pageNumber = document.createElement("span");
        pageNumber.classList.add("toc-page");
        pageNumber.dataset.sectionId = id;
        pageNumber.textContent = "Loading..."; // Placeholder

        tocItem.appendChild(link);
        tocItem.appendChild(pageNumber);
        tocList.appendChild(tocItem);
    });
}