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

document.addEventListener('DOMContentLoaded', function () {
    var dropdowns = document.querySelectorAll('.dropdown-toggle');
    dropdowns.forEach(function (dropdown) {
        if (dropdown.classList.contains('closed')) {
            dropdown.nextElementSibling.classList.remove('show');
            //dropdown.nextElementSibling.classList.remove('right');
            dropdown.nextElementSibling.style.maxHeight = '0';
        }
    });

    dropdowns.forEach(function (dropdown) {
        dropdown.addEventListener('click', function (e) {
            e.preventDefault();
            let subMenu = this.nextElementSibling;
            let currentHeight = subMenu.offsetHeight;
            const dropdownMenuAncestor = this.closest('.dropdown-menu');
            if (this.classList.contains('closed')) {
                this.classList.remove('closed');
                subMenu.classList.add('show');


                if (dropdownMenuAncestor) {
                    const flexDirection = window.getComputedStyle(dropdownMenuAncestor).getPropertyValue('flex-direction');
                    if (flexDirection === 'row') {
                        let otherDropdowns = this.parentElement.parentElement.querySelectorAll('.dropdown-toggle');
                        otherDropdowns.forEach(function (otherDropdown) {
                            if (otherDropdown !== dropdown && !otherDropdown.classList.contains('closed')) {
                                otherDropdown.classList.add('closed');
                                otherDropdown.nextElementSibling.classList.remove('show');
                                //otherDropdown.nextElementSibling.classList.remove('right');
                                otherDropdown.nextElementSibling.style.maxHeight = '0';
                            }
                        });
                    }
                }
            } else {
                this.classList.add('closed');
                subMenu.classList.remove('show');
                //subMenu.classList.remove('right');
            }

            if (subMenu.matches('.dropdown-menu ul ul')) {
                if (subMenu.classList.contains('show')) {
                    let depth = getMaxDepthOfUL(subMenu);
                    let submenuRect = subMenu.getBoundingClientRect();
                    // checl if dropdownMenuAncestor is flex and direction is row
                    const flexDirection = window.getComputedStyle(dropdownMenuAncestor).getPropertyValue('flex-direction');

                    if (flexDirection === 'column') {
                        subMenu.classList.remove('right');
                    } else {
                        if ((submenuRect.right + 200 * (depth - 1)) > window.innerWidth) {
                            subMenu.classList.add('right');
                        }
                    } 
                }
            }

             // if (dropdown.closest(`.dropdown-menu-right`) === null) {

                 let updatedHeight = recalculateHeight(subMenu);
                 subMenu.style.maxHeight = `${updatedHeight}px`;

                 let difference = updatedHeight - currentHeight;
                 let parentUl = subMenu.parentElement.closest('ul');
                 while (parentUl) {
                     const flexDirection = window.getComputedStyle(dropdownMenuAncestor).getPropertyValue('flex-direction');


                     // if parentUl contains the class dropdown-menu
                     if (parentUl.classList.contains('dropdown-menu') && window.getComputedStyle(parentUl).getPropertyValue('flex-direction') === "row") {
                         parentUl.style.maxHeight = ``;
                     } else {
                         parentUl.style.maxHeight = `${parentUl.offsetHeight + difference}px`;
                     }

                     parentUl = parentUl.parentElement.closest('ul');


                 }
             // }

            // let parentUl = subMenu.parentElement.closest('ul');
            // while (parentUl) {
            //     parentUl.style.maxHeight = `${parentUl.offsetHeight + difference}px`;
            //     parentUl = parentUl.parentElement.closest('ul');
            // }
        });
    });

    function getMaxDepthOfUL(element, currentDepth = 0) {
        if (element.tagName === 'UL') {
            currentDepth++;
        }

        let maxDepth = currentDepth;
        let children = element.children;
        for (let i = 0; i < children.length; i++) {
            let child = children[i];
            let childDepth = getMaxDepthOfUL(child, currentDepth);
            maxDepth = Math.max(maxDepth, childDepth);
        }

        return maxDepth;
    }

    function recalculateHeight(submenu) {
        let totalHeight = 0;

        if (submenu.classList.contains('show') || submenu.classList.contains('showSmall')) {
            totalHeight = Array
                .from(submenu.children)
                .reduce((sum, item) => {
                    const style = window.getComputedStyle(item);
                    const marginTop = parseFloat(style.marginTop);
                    const marginBottom = parseFloat(style.marginBottom);
                    const heightWithMargins = item.offsetHeight + marginTop + marginBottom;
                    return sum + heightWithMargins;
                }, 0);
        }

        return totalHeight;
    }
});