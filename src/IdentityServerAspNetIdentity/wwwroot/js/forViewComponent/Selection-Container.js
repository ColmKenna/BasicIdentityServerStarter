﻿function addNewItem(nameOfItem, itemToAddId, roleTemplateId, selectionContainerId, rowClass) {
    const itemToAdd = document.getElementById(itemToAddId).value;
    
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
        label.textContent = itemToAdd;
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
