function toggleRemoveForStringContainer(button, deletedItemsName) {
    var row = button.parentElement;
    var valueToDelete = row.getElementsByTagName('input')[0].value;
    var hiddenInput = document.getElementsByName(deletedItemsName)[0];
    var deletedItems = hiddenInput.value ? hiddenInput.value.split(',') : [];

    // create a random id for the row
    let ariaSpanId;// = "aria-describedby-" + Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 5);

    // if the row does not have an aria-describedby attribute, add it
    if (!row.getAttribute('aria-describedby')) {
        ariaSpanId = "aria-describedby-" + Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 5);
        row.setAttribute('aria-describedby', ariaSpanId);
        // create a span element with the id
        let span = document.createElement('span');
        span.id = ariaSpanId;
        span.textContent = "Deleted " + valueToDelete;
        span.style.display = "none";
        let rowParent= row.parentElement;
        // append after the row
        if(row.nextSibling === null){
            rowParent.appendChild(span);}
        else {
            rowParent.insertBefore(span, row.nextSibling);
        }
    }else{
        ariaSpanId = row.getAttribute('aria-describedby');
    }
    let ariaSpan = document.getElementById(ariaSpanId);

    let index = deletedItems.indexOf(valueToDelete);
    if (index === -1) {
        deletedItems.push(valueToDelete);
        row.classList.add('bg-warning');
        button.classList.remove('delete-icon-button');
        button.classList.add('add-icon-button');
        ariaSpan.innerText = valueToDelete + " marked for deletion";
    } else {
        deletedItems.splice(index, 1);
        row.classList.remove('bg-warning');
        button.classList.remove('add-icon-button');
        button.classList.add('delete-icon-button');
        ariaSpan.innerText = valueToDelete + " unmarked for deletion";
    }
    hiddenInput.value = deletedItems.join(',');
}
