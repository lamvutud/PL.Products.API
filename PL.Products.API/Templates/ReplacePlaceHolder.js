(id, content) => 
{
    const placeholder = document.querySelector(id);
    if (placeholder) {
        placeholder.innerHTML = content;
    }
} 