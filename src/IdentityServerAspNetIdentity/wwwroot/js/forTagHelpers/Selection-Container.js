function addNewItem(nameOfItem, itemToAddId, roleTemplateId, selectionContainerId) {
    const itemToAdd = document.getElementById(itemToAddId).value;
    
    // Check that the itemToAdd is not empty
    if (!itemToAdd) {
        alert(nameOfItem + ' cannot be empty.');
        return;
    }

    const template = document.getElementById(roleTemplateId).content.cloneNode(true);
    const selectionContainer = document.getElementById(selectionContainerId);

    // Set the value of the first input in the template to itemToAdd
    template.querySelector('input').setAttribute('value', itemToAdd);

    // Set the text content of the label in the template
const label = template.querySelector('label');
if (label) {
    label.textContent = itemToAdd;
}

    // Check if the item already exists
    if (selectionContainer.querySelector(`input[value="${itemToAdd}"]`)) {
        alert(nameOfItem + ' ' + itemToAdd + ' already exists.');
        return;
    }

    // Insert the cloned template before an existing button (if one exists)
    const existingButton = selectionContainer.querySelector('button');
    if (existingButton) {
        selectionContainer.insertBefore(template, existingButton);
    } else {
        selectionContainer.appendChild(template);
    }
}