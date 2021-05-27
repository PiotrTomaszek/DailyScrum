connection.on("ToastrNotify", function (title, textContent) {

    debugger;

    toastr.options =
    {
        "debug": false,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "fadeIn": 300,
        "fadeOut": 100,
        "timeOut": 3000,
        "extendedTimeOut": 1000
    }

    toastr.success('Have fun storming the castle!', 'Miracle Max Says')

    toastr/*[$("#toastrTypeGroup input:radio:checked").val()]*/.success(textContent, title);
});