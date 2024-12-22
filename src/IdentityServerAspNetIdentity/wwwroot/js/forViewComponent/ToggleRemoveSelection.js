function toggleRemoveSelection(id, buttonId, deletedItemsName) {
    var element = document.getElementById(id);
    var button = document.getElementById(buttonId);
    var deletedItems = document.getElementsByName(deletedItemsName)[0];


    if (element.checked) {
        element.checked = false;
        button.classList.remove("delete-icon-button-selected");
    } else {
        element.checked = true;
        button.classList.add("delete-icon-button-selected");
    }
}