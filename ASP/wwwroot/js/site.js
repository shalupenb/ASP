document.addEventListener('submit', e => {
    console.log(e);
    const form = e.target;
    if (form.id == 'room-form')// перехоплюємо надсилання форми і переводимо до аякс
    {
        e.preventDefault();
        let formData = new FormData(form);
        // console.log(formData);
        fetch("/api/room", {
            method: 'POST',
            body: formData
        }).then(r => {
            console.log(r);
            if (r.status == 201) {
                window.location.reload();
            }
            else {
                r.text().then(alert);
            }
        });
    }
    // на інші форми ми не вплюваємо
});
document.addEventListener('DOMContentLoaded', function () {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);

    initAdminPage();
    serveReserveButtons();
});

function authButtonClick() {
    const authEmail = document.getElementById("auth-email");
    if (!authEmail) throw "Element '#authEmail' not found!"
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element '#authPassword' not found!"
    const authMessage = document.getElementById("auth-message");
    if (!authMessage) throw "Element '#authMessage' not found!"

    const email = authEmail.value.trim();
    if (!email) {
        authMessage.classList.remove('visually-hidden');
        authMessage.innerText = "Hеобхідно ввести E-mail";
        return;
    }
    const password = authPassword.value;

    fetch(`/api/auth?e-mail=${email}&password=${password}`)
        .then(r => {
            if (r.status != 200) {
                authMessage.classList.remove('visually-hidden');
                authMessage.innerText = "Вхід скасовано, перевірте введені дані";
            }
            else {
                // За вимогами безпеки зміна стагусу авторизації потребує перезавантаження
                window.location.reload();
            }
        });
}



///// ADMIN PAGE //////
function initAdminPage() {
    loadCategories();
}
function loadCategories() {
    const container = document.getElementById("category-container");
    if (!container) return;
    fetch("/api/category")      // запитуємо наявні категорії
        .then(r => r.json())
        .then(j => {
            let html = "";
            for (let ctg of j) {
                html += `<p data-id="${ctg["id"]}" onclick="ctgClick('${ctg["id"]}')">${ctg["name"]}</p>`;
            }
            html += `Назва: <input id="ctg-name" /><br/>
            Опис: <textarea id="ctg-description"></textarea><br/>
            Фото: <input type="file" id="ctg-photo" /><br/>
            <button onclick='addCategory()'>+</button>`;
            container.innerHTML = html;
        });

}
function ctgClick(ctgid) {
    //const ctgid = e.target.closest('[data-id]').getAttribute('data-id');
    fetch("/api/location/" + ctgid)
        .then(r => r.json())
        .then(j => {
            const container = document.getElementById("location-container");
            let html = "";
            for (let loc of j) {
                html += `<p data-id="${loc["id"]}" onclick="locClick(event)">${loc["name"]}</p>`;
            }
            html += `Назва: <input id="loc-name" /><br/>
            Опис: <textarea id="loc-description"></textarea><br/>
            Рейтинг: <input id="loc-stars" type="number"/><br/>
            Фото: <input type="file" id="loc-photo" /><br/>
            <button onclick='addLocation("${ctgid}")'>+</button>`;
            container.innerHTML = html;
        });
}

function addCategory() {
    const ctgName = document.getElementById("ctg-name").value;
    const ctgDescription = document.getElementById("ctg-description").value;
    const ctgPhoto = document.getElementById("ctg-photo");

    if (confirm(`Додаємо категорію ${ctgName} ${ctgDescription} ?`)) {
        let formData = new FormData();
        formData.append("name", ctgName);
        formData.append("description", ctgDescription);
        formData.append("photo", ctgPhoto.files[0]);

        fetch("/api/category", {
            method: 'POST',
            body: formData
        })
            .then(r => {
                if (r.status == 201) {
                    loadCategories();
                }
                else {
                    alert("error");
                }
            });
    }
}


function addLocation(ctgid) {
    const ctgName = document.getElementById("loc-name").value;
    const ctgDescription = document.getElementById("loc-description").value;
    const ctgStars = document.getElementById("loc-stars").value;
    const locPhoto = document.getElementById("loc-photo");
    if (confirm(`Додаємо локацiю ${ctgName} ${ctgDescription} ${ctgStars} ?`)) {
        let formData = new FormData();
        formData.append("categoryId", ctgid);
        formData.append("name", ctgName);
        formData.append("description", ctgDescription);
        formData.append("stars", ctgStars);
        formData.append("photo", locPhoto.files[0]);
        fetch("/api/location", {
            method: 'POST',
            body: formData
        })
            .then(r => {
                if (r.status == 201) {
                    ctgClick(ctgid);
                }
                else {
                    alert("error");
                }
            });
    }
}

function serveReserveButtons() {
    for (let btn of document.querySelectorAll('[data-type="reserve-room"]')) {
        btn.addEventListener('click', e => {
            const cont = e.target.closest('[data-type="reserve-room"]');
            const roomId = cont.getAttribute('data-room-id');
            const userId = cont.getAttribute('data-user-id');
            const roomName = cont.getAttribute('data-room-name');
            const date = cont.getAttribute('data-date');

            console.log(roomId, userId, date);
            let userChoice = confirm("Вы действительно хотите забронировать номер: " + roomName + " на дату: " + date + "?");
            if (userChoice) {
                fetch('/api/room/reserve', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        date,
                        roomId,
                        userId
                    })
                }).then(r => {
                    if (r.status === 201) {
                        window.location.reload();
                    }
                    else {
                        r.text().then(alert);
                    }
                });
            }
        });
    }
    for (let btn of document.querySelectorAll('[data-type="drop-reserve-room"]')) {
        btn.addEventListener('click', e => {
            const cont = e.target.closest('[data-type="drop-reserve-room"]');
            const reserveId = cont.getAttribute('data-reserve-id');
            const reserveName = cont.getAttribute('data-room-name');
            const reserveDate = cont.getAttribute('data-date');
            if (!confirm("Підтверджуєте скасування бронювання номеру: " + reserveName + " на дату: " + reserveDate)) {
                return;
            }
            fetch('/api/room/reserve?reserveId=' + reserveId, {
                method: 'DELETE',
            }).then(r => {
                if (r.status === 202) {
                    window.location.reload();
                }
                else {
                    r.text().then(alert);
                }
            });
        });
    }
}