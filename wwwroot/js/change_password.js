
var check = [true, true]
var button = document.getElementById("Change_Password_Button")

function setButton(index, value)
{
    check[index] = value
    var check2 = true
    for (var i = 0; i < check.length; i++) {
        if (check[i])
        {
            check2 = false
        }
    }
    if (check2) {
        button.disabled = false
    }
    else {

        button.disabled = true
    }
}

function validateForm() {


    var Password = document.getElementById("New_Password")
    var Password_Error_1_Button = document.getElementById("Password_Error_1_Button")
    var Password_Error_1_Icon = document.getElementById("Password_Error_1_Icon")
    var Password_Error_2_Button = document.getElementById("Password_Error_2_Button")
    var Password_Error_2_Icon = document.getElementById("Password_Error_2_Icon")
    var Password_Error_3_Button = document.getElementById("Password_Error_3_Button")
    var Password_Error_3_Icon = document.getElementById("Password_Error_3_Icon")
    var Password_Error_4_Button = document.getElementById("Password_Error_4_Button")
    var Password_Error_4_Icon = document.getElementById("Password_Error_4_Icon")

    var Confirm_Password = document.getElementById("Confirm_New_Password")



    Password.onchange = (e) => {
        var inner_check = true

        if (Password.value.length < 12) {
            Password_Error_1_Button.className = "input button is-danger"
            Password_Error_1_Icon.style = "color:white"
            setButton(0, true)
            inner_check = false
        }
        else {
            Password_Error_1_Button.className = "input button is-success"
            Password_Error_1_Icon.style = "color:white"
        }

        if (/([A-Z].*[a-z]|[a-z].*[A-Z])/.test(Password.value)) {
            Password_Error_2_Button.className = "input button is-success"
            Password_Error_2_Icon.style = "color:white"
        }
        else {
            Password_Error_2_Button.className = "input button is-danger"
            Password_Error_2_Icon.style = "color:white"
            setButton(0, true)
            inner_check = false
        }

        if (/\d/.test(Password.value)) {
            Password_Error_3_Button.className = "input button is-success"
            Password_Error_3_Icon.style = "color:white"
        }
        else {
            Password_Error_3_Button.className = "input button is-danger"
            Password_Error_3_Icon.style = "color:white"
            setButton(0, true)
            inner_check = false
        }

        if (/[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(Password.value)) {
            Password_Error_4_Button.className = "input button is-success"
            Password_Error_4_Icon.style = "color:white"
        }
        else {
            Password_Error_4_Button.className = "input button is-danger"
            Password_Error_4_Icon.style = "color:white"
            setButton(0, true)
            inner_check = false
        }

        if (inner_check) {
            setButton(0, false)
        }
    }

    Confirm_Password.onchange = () => {
        if (Confirm_Password.value != Password.value)
        {
            setButton(1, true)
        }
        else
        {
            setButton(1, false)
        }
    }


    button.disabled = true

    if (Password.value != "")
        Password.onchange()
}
window.onload = () => {
    validateForm()
}