
let movesList = document.getElementById("MovesList");
let selectTemplate = movesList.firstElementChild.cloneNode(true);

setupEventDelegation();


// Sets event listeners for adding moves to pokemon
function setupEventDelegation() {
    document.addEventListener('change', (e) => {
        if (e.target.name === 'moveIds') {
            if (e.target.parentElement.parentElement == movesList.lastElementChild) {
                if (movesList.childElementCount < 4) {
                    let newRow = selectTemplate.cloneNode(true);
                    movesList.appendChild(newRow);
                }
                let select = document.getElementById("optionSelect");
                select && select.remove();
                if (!movesList.lastElementChild.contains(movesList.lastElementChild.getElementsByClassName("deletebtn")[0])) {
                    addDeleteButton(e.target.parentElement.parentElement);
                }
            }
            disableSelectedOptions();
        }
    });
}

// Puts a delete button at the end of provided node
function addDeleteButton(node) {
    let deletebtn = document.createElement("input");
    deletebtn.type = "button";
    deletebtn.className = "btn btn-danger deletebtn";
    deletebtn.value = "X";
    deletebtn.addEventListener('click', (e) => {
        e.target.parentElement.parentElement.remove();

        if (movesList.lastElementChild.contains(movesList.lastElementChild.getElementsByClassName("deletebtn")[0])) {
            let newRow = selectTemplate.cloneNode(true);
            movesList.appendChild(newRow);
        }

        disableSelectedOptions();
    });
    node.lastElementChild.appendChild(deletebtn);
}

// Removes duplicate items in all selects, allows for a cleaner selection process
function disableSelectedOptions() {

    const values = Array.from(movesList.getElementsByTagName("select")).map((select) => select.value);
    for (const select of movesList.getElementsByTagName("select")) {
        select.querySelectorAll('option').forEach((option) => {
            const value = option.value;
            if (value && value !== select.value && values.includes(value)) {
                option.hidden = true;
            } else {
                option.hidden = false;
            }
        });
    }
}