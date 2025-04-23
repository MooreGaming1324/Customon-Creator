
let movesList = document.getElementById("MovesList");
let selectTemplate = document.getElementById("selectTemplate").cloneNode(true);
selectTemplate.id = "";
selectTemplate.hidden = false;

console.log(selectTemplate);
if (movesList.childElementCount > 4) {
    let temp = document.getElementById("selectTemplate");
    temp.remove();

} else {
    let temp = document.getElementById("selectTemplate");
    temp.hidden = false;
}


setupEventDelegation();

//edit specifics
deleteRepeatingOptions();
disableSelectedOptions();


function deleteRepeatingOptions() {

    for (const select of movesList.getElementsByTagName("select")) {
        select.querySelectorAll('option').forEach((option) => {
            const value = option.value;
            if (option.index != select.selectedIndex && value == select.value) {
                console.log("Remove duplicate: " + option.value);
                option.remove();
            }
        });
    }
}
//end edit specifics



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

    for (const btn of document.getElementsByClassName("deletebtn")) {
        btn.addEventListener('click', (e) => {
            e.target.parentElement.parentElement.remove();

            if (movesList.lastElementChild.contains(movesList.lastElementChild.getElementsByClassName("deletebtn")[0])) {
                let newRow = selectTemplate.cloneNode(true);
                movesList.appendChild(newRow);
            }

            disableSelectedOptions();
        });
    }
}


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