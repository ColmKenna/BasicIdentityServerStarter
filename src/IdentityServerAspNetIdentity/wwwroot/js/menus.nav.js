function UpdateHeightOfRootNav(nav) {
    let totalHeight = Array.from(nav.children).reduce((sum, item) => {
        const style = window.getComputedStyle(item);
        const marginTop = parseFloat(style.marginTop);
        const marginBottom = parseFloat(style.marginBottom);
        const heightWithMargins = item.scrollHeight + marginTop + marginBottom;
        return sum + heightWithMargins;
    }, 0);

    nav.style.maxHeight = `${totalHeight + 5}px`;
}

document.getElementById('burgerMenu')?.addEventListener('click', function () {
    let nav = document.getElementById('nav');
    if (nav) {
        nav.classList.toggle('hidden-small');

        if (!nav.classList.contains('hidden-small')) {
            UpdateHeightOfRootNav(nav);
        } else {
            nav.style.maxHeight = '0';
        }
    }
});
