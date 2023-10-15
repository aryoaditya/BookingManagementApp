$(document).ready(function () {
    $('#loginBtn').click(function () {
        login();
    });

    $('#inputPassword').keypress(function (e) {
        if (e.which == 13) {
            login();
        }
    })

    $('#inputEmail').keypress(function (e) {
        if (e.which == 13) {
            login();
        }
    })

    function login() {
        let loginAcc = {
            email: $('#inputEmail').val(),
            password: $('#inputPassword').val()
        }

        $.ajax({
            url: "https://localhost:7290/api/Account/login",
            type: "POST",
            data: JSON.stringify(loginAcc),
            cache: false,
            contentType: "application/json"
        }).done((result) => {
            console.log(result.data);
            window.location.href = '../';
        }).fail((jqXHR, textStatus, errorThrown) => {
            console.log(jqXHR.responseJSON.message);
        });
    }
});