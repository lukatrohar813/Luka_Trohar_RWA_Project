let loginUrl = "http://localhost:5158/api/Auth/login";

function jwtLogin(event) {
    event.preventDefault();

    $("#spinner-placeholder").addClass("spinner");
    $("#login-button").prop("disabled", true);

    let loginData = {
        "username": $("#username").val(),
        "password": $("#password").val(),
        "confirmPassword": $("#confirmPassword").val() 
    };

    console.log("Sending login data:", loginData);

    $.ajax({
        method: "POST",
        url: loginUrl,
        data: JSON.stringify(loginData),
        contentType: 'application/json'
    }).done(function (response) {
        console.log("Login successful:", response);

        if (response.token) {
            localStorage.setItem("JWT", response.token);
            console.log("Token stored in localStorage:", response.token);

            $("#spinner-placeholder").removeClass("spinner");
            $("#login-button").prop("disabled", false);

      
            window.location.href = "logs.html";
        } else {
            console.error("Login failed: No token received");
            alert("Login failed: No token received");
            $("#spinner-placeholder").removeClass("spinner");
            $("#login-button").prop("disabled", false);
        }
    }).fail(function (err) {
        console.error("Login failed:", err);
        alert("Login failed: " + err.responseText);

        localStorage.removeItem("JWT");
        $("#spinner-placeholder").removeClass("spinner");
        $("#login-button").prop("disabled", false);
    });
}
