function addNewItem(nameOfItem, itemToAddId, roleTemplateId, selectionContainerId, rowClass) {
    const itemToAddElement = document.getElementById(itemToAddId);

    let itemToAdd;
    let textToAdd;

    if (itemToAddElement.tagName === 'SELECT') {
        const selectedOption = itemToAddElement.options[itemToAddElement.selectedIndex];
        itemToAdd = selectedOption.value;
        textToAdd = selectedOption.textContent;
    } else {
        itemToAdd = itemToAddElement.value;
        textToAdd = itemToAdd;
    }

    
    
    // Check that the itemToAdd is not empty
    if (!itemToAdd) {
        alert(nameOfItem + ' cannot be empty.');
        return;
    }

    const selectionContainer = document.getElementById(selectionContainerId);
    let count = selectionContainer.querySelectorAll('.' + rowClass).length;
    var r = document.getElementById(roleTemplateId).innerHTML;

    const templateString = document.getElementById(roleTemplateId).innerHTML .replace(/_replace-with-index_/g,count.toString());
    var tempDiv = document.createElement('div');
    tempDiv.innerHTML = templateString;

// Extract the first element from the container
    var template = tempDiv.firstElementChild;
    

    // Set the value of the first input in the template to itemToAdd
    template.querySelector('input').setAttribute('value', itemToAdd);

    // Set the text content of the label in the template
    const label = template.querySelector('label');
    if (label) {
        label.textContent = textToAdd;
    }
    
  
     
        
        

    // Check if the item already exists
    if (selectionContainer.querySelector(`input[value="${itemToAdd}"]`)) {
        alert(nameOfItem + ' ' + itemToAdd + ' already exists.');
        return;
    }

    // Insert the cloned template before an existing button (if one exists)
    const existingButton = selectionContainer.querySelector(':scope > button');
    if (existingButton) {
        selectionContainer.insertBefore(template, existingButton);
    } else {
        selectionContainer.appendChild(template);
    }

    const labelForEmptyList = selectionContainer.querySelector('.empty-list-label');
    if (labelForEmptyList) {
        labelForEmptyList.style.display = 'none';
    }
}


function addNewTextInputFromTemplate(templateId, elementToAddToId, newInputId, currentCountId) {
    const template = document.getElementById(templateId);
    const elementToAddTo = document.getElementById(elementToAddToId);
    const newElement = template.content.cloneNode(true);
    const value = document.getElementById(newInputId).value;

    const currentCount = document.getElementById(currentCountId);
    const count = parseInt(currentCount.value);

    
for (let element of newElement.querySelectorAll('[name], [id]')) {
    if (element.hasAttribute('name')) {
        element.setAttribute('name', element.getAttribute('name').replace('_replace-with-index_', count));
    }
    if (element.hasAttribute('id')) {
        element.setAttribute('id', element.getAttribute('id').replace('_replace-with-index_', count));
    }    
}

    

    
    newElement.querySelector('input').value =  value; 
    elementToAddTo.appendChild(newElement);

    document.getElementById(newInputId).value = '';
    
    currentCount.value = count + 1;     
}

