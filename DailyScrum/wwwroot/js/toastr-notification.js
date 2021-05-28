connection.on("ToastrNotify", function (title, textContent) {

    toastr.options =
    {
        "debug": false,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "fadeIn": 300,
        "fadeOut": 100,
        "timeOut": 3000,
        "extendedTimeOut": 1000,
        "progressBar": true
    }

    toastr.success(textContent, title);
});

connection.on("ToastrInfoNotify", function (title, textContent) {

    toastr.options =
    {
        "debug": false,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "fadeIn": 900,
        "fadeOut": 900,
        "timeOut": 3000,
        "extendedTimeOut": 1000,
        "progressBar": true,
        "closeEasing ": 'linear'
    }

    toastr.info(title, textContent);
});