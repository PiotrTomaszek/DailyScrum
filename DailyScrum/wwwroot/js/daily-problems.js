connection.on("SendProblem", function (fullname, userId, problemText, date, problemId, photopath) {
    var problemPlace = document.getElementById('problemsTable');

    var newRow = document.createElement('tr');

    var newData1 = document.createElement('td');

    var img = document.createElement('img');
    img.id = `userProblem-${userId}`;
    img.alt = `${fullname}`
    img.classList.add('rounded-circle')
    img.style.width = '50px';
    img.style.height = '50px';
    img.src = '/avatars/' + photopath;

    newData1.appendChild(img);
    /*newData1.appendChild(fullname);*/

    var newData2 = document.createElement('td');
    newData2.innerText = date

    var newData3 = document.createElement('td');
    newData3.innerHTML = `<p><strong>${problemText}</strong></p>`

    var newData4 = document.createElement('td');
    newData4.classList.add('text-right');
    newData4.classList.add('h-100');

    newData4.innerHTML =
        `<form method="get" asp-action="Index" asp-controller="Problems">
            <input type="hidden" name="${problemId}" value="${problemId}" />
            <button class="btn btn-outline-danger align-self-center" type="submit">
                Zatwierdź i usuń.
            </button>
        </form>`;

    newRow.appendChild(newData1);
    newRow.appendChild(newData2);
    newRow.appendChild(newData3);
    newRow.appendChild(newData4);

    problemPlace.appendChild(newRow);
});
