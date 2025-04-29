
console.log("working");

const editTeamModalDOM = document.getElementById('editTeamModal');
const editTeamModal = new bootstrap.Modal(editTeamModalDOM);



setupEventDelegation();

deleteRepeatingOptions();
disableSelectedOptions();


function deleteRepeatingOptions() {

    for (const select of editTeamModalDOM.getElementsByTagName("select")) {
        select.querySelectorAll('option').forEach((option) => {
            const value = option.value;
            if (option.index != select.selectedIndex && value == select.value) {
                option.remove();
            }
        });
    }
}



function setupEventDelegation() {


    let editTeamBtn = document.getElementById("editTeamBtn");
    editTeamBtn.addEventListener('click', async (e) => {
        editTeamModal.show();
    });

    let selects = document.querySelectorAll(".form-select");
    selects.forEach((select) => {
        select.addEventListener('change', (e) => {
            disableSelectedOptions();
        });
    });

    let submitBtn = document.getElementById("teamModalSubmit");
    submitBtn.addEventListener('click', async (e) => {
        e.preventDefault();
        const pokemonForm = document.querySelector("#formTeam");
        const pokemon = new FormData(pokemonForm);
        let response = await submitTeamAsync(pokemon);
        if (response.ok == false) {
            alert("Oops! Something went wrong!");
        }
        location.reload();
    });

    let copyBtn = document.getElementById("copyTeamBtn");
    let copyBtnTooltip = new bootstrap.Tooltip(copyBtn);
    copyBtn.addEventListener('click', (e) => {
        let copy = document.querySelector("#dropdown-username").textContent.trim() + "'s Team:\n\n";
        let cards = document.querySelectorAll(".card");
        for (let i = 0; i < 6; i++) {
            copy += `${i+1}: `
            let card = cards[i];
            let name = card.querySelector(".name").textContent.trim();
            if (name !== "Empty...") {
                let type1 = card.querySelector(".type1").textContent.trim();
                let type2 = card.querySelector(".type2").textContent.trim();
                let moves = card.querySelectorAll(".move");

                copy += name + "\n  Type: " + type1 + " " + type2;
                moves.forEach((move) => {
                    copy += "\n    - " + move.textContent.trim();
                });
            } else {
                copy += "Empty...";
            }
            copy += "\n\n"
        }
        navigator.clipboard.writeText(copy.trim());
        //alert("Team Copied!");
    })

}


function disableSelectedOptions() {

    let values = Array.from(editTeamModalDOM.getElementsByTagName("select")).map((select) => select.value);
    for (const select of editTeamModalDOM.getElementsByTagName("select")) {
        select.querySelectorAll('option').forEach((option) => {
            const value = option.value;
            if (value && value !== select.value && values.includes(value) && value !== "0") {
                option.hidden = true;
            } else {
                option.hidden = false;
            }
        });
    }
}



async function submitTeamAsync(pokemon) {
    const address = `/home/updateteam`;
    const response = await fetch(address, {
        method: "post",
        body: pokemon
    });
    return response;
}

