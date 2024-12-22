function toggleRemoveForStringContainer(button, deletedItemsName) {
    var row = button.parentElement;
    var valueToDelete = row.getElementsByTagName('input')[0].value;
    var hiddenInput = document.getElementsByName(deletedItemsName)[0];
    var deletedItems = hiddenInput.value ? hiddenInput.value.split(',') : [];


    let index = deletedItems.indexOf(valueToDelete);
    if (index === -1) {
        deletedItems.push(valueToDelete);
        row.classList.add('bg-warning');
        button.classList.remove('delete-icon-button');
        button.classList.add('add-icon-button');

    } else {
        deletedItems.splice(index, 1);
        row.classList.remove('bg-warning');
        button.classList.remove('add-icon-button');
        button.classList.add('delete-icon-button');




    }
    hiddentInput.value = deletedItems.join(',');
}
