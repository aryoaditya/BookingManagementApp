$(document).ready(function () {
    $('#registerBtn').click(function () {
        console.log("Hello");
        let registAcc = {
            firstName: $("#firstName").val(),
            lastName: $("#lastName").val(),
            gender: parseInt($("#gender").val()),
            email: $("#email").val(),
            birthDate: $("#birthDate").val(),
            hiringDate: $("#hiringDate").val(),
            phoneNumber: $("#phone-number").val(),
            major: $("#major").val(),
            degree: $("#degree").val(),
            gpa: $("#gpa").val(),
            universityCode: $("#universityCode").val(),
            universityName: $("#universityName").val(),
            password: $("#password").val(),
            confirmPassword: $("#passwordConfirm").val(),
        }
        let jsonString = JSON.stringify(registAcc);

        $.ajax({
            url: "https://localhost:7290/api/account/register",
            data: jsonString,
            type: "POST",
            contentType: "application/json",
            cache: false
        }).done((result) => {
            console.log(result);
        }).fail((jqXHR, textStatus, errorThrown) => {
            console.log(jqXHR.responseJSON.message);
        });
    })
});