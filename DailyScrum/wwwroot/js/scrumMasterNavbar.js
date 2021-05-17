﻿connection.on("GenScrumMasterProblems", function () {
    var smProblemsNavbar = document.createElement('li');
    smProblemsNavbar.classList.add('nav-item');
    smProblemsNavbar.classList.add('ml-2');

    var innerLink = document.createElement('a');
    innerLink.classList.add('nav-link');
    innerLink.classList.add('text');
    innerLink.classList.add('text-white');
    innerLink.classList.add('link');
    innerLink.classList.add('link--mneme');

    innerLink.href = '/Problems/Index';
    innerLink.innerText = 'Problemy';

    smProblemsNavbar.appendChild(innerLink);

    document.getElementById('navbarOptionsId').appendChild(smProblemsNavbar);
});