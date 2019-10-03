$(document).ready(function () {
});

// Method client side to verify if the user exist in our database
function validateUser() {
    let user = document.getElementById("txtUser").value;
    let userPassword = document.getElementById("txtPassword").value;

    let dataLogin = {
        userName: user,
        password: userPassword
    }

    $.ajax({
        url: '/Zitro/verifyUSer',
        type: 'POST',
        async: false,
        data: dataLogin,
        dataType: 'json',
        success: function (data) {
            if (data.StatusCode === 200) {
                localStorage.setItem('user', user);
                window.location.href = window.location.protocol + '//' + window.location.host + '/Orders/Index'
            }
            else {
                $("#lblError").text(data.StatusDescription);
                $("#lblError").show();
                //$("#lblError").show();
            }
        },
        complete: function () {
            console.log('complete')
        },
        error: function () {
            console.log(JSON.stringify(request));
        }
    });
}