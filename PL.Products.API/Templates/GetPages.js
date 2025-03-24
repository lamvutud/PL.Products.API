() => {
    const elements = document.querySelectorAll('h2, h3'); // Targeting <h2> and <h3> tags
    const pages = {}; // To store page numbers for each section
    const pageHeight = 1122; // A4 height in pixels at 96 DPI (standard for Puppeteer/PDF rendering)

    // Get all page breaks in the document
    const pageBreaks = document.querySelectorAll('.page-break');
    let pageIndex = 1; // Start with page 1

    // Loop through all elements (h2, h3) and calculate their page numbers
    elements.forEach((el, index) => {
        // Calculate the position of the current element in the viewport
        const rect = el.getBoundingClientRect();

        // The position (top) of the element, adjusted by the current page offset
        const elementPosition = rect.top + window.scrollY;

        // Start by assuming this element is on the first page
        let pageNumber = Math.ceil(elementPosition / pageHeight);

        // Adjust the page number based on the page breaks
        pageBreaks.forEach((pageBreak, breakIndex) => {
            const breakPosition = pageBreak.getBoundingClientRect().top + window.scrollY;

            // If the section (el) is located after a page break, increase the page number
            if (elementPosition >= breakPosition) {
                pageNumber++; // This section will appear on the next page
            }
        });

        // Store the page number for the section, using its ID or a fallback to a unique index
        pages[el.id || `section-${index}`] = pageNumber;
    });

    return pages;


}
