var userObj = localStorage.getItem('user');

$(document).ready(function () {
    watchSession();
})

// Method to verify if the Session still alive.
function watchSession() {
    $.ajax({
        url: '/Home/verifySession',
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.StatusCode !== 200) {
                if (userObj) {
                    localStorage.removeItem('user', '');
                    window.location.href = window.location.protocol + '//' + window.location.host + '/Zitro/Index';
                }
            }

        },
        complete: function () {
            console.log('complete');
        },
        error: function () {
            console.log(JSON.stringify(request));
        }
    });
}