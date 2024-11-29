document.addEventListener("DOMContentLoaded", function () {
    const table = document.querySelector(".table-responsive");

    if (table) {
        const headings = Array.from(table.querySelectorAll("thead th")).map(th => th.textContent.trim());

        const rows = table.querySelectorAll("tbody tr");

        rows.forEach(row => {
            const cells = row.querySelectorAll("td");

            cells.forEach((cell, index) => {
                if (headings[index]) {
                    const label = document.createElement("label");
                    label.classList.add("small-screen-only");
                    label.textContent = headings[index];
                    cell.insertBefore(label, cell.firstChild);
                }
            });
        });
    }
});