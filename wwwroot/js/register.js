var check = [true, true, true, true, true, true]
var button = document.getElementById("Register_Button")

function setButton(index, value) {
    check[index] = value

    var check2 = true

    for (var i = 0; i < check.length; i++) {
        if (check[i]) {
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

function validateForm()
{
    



    var fname = document.getElementById("fname")
    var fname_Error = document.getElementById("fname_Error")

    var lname = document.getElementById("lname")
    var lname_Error = document.getElementById("lname_Error")

    var Email = document.getElementById("Email")
    var Email_Error = document.getElementById("Email_Error")

    var Password = document.getElementById("Password")
    var Password_Error_1_Button = document.getElementById("Password_Error_1_Button")
    var Password_Error_1_Icon = document.getElementById("Password_Error_1_Icon")
    var Password_Error_2_Button = document.getElementById("Password_Error_2_Button")
    var Password_Error_2_Icon = document.getElementById("Password_Error_2_Icon")
    var Password_Error_3_Button = document.getElementById("Password_Error_3_Button")
    var Password_Error_3_Icon = document.getElementById("Password_Error_3_Icon")
    var Password_Error_4_Button = document.getElementById("Password_Error_4_Button")
    var Password_Error_4_Icon = document.getElementById("Password_Error_4_Icon")

    var dob = document.getElementById("dob")
    var dob_Error = document.getElementById("dob_Error")

    var cc = document.getElementById("cc")
    var cc_Error = document.getElementById("cc_Error")

    // Validate Name as Alphabets or '
    fname.onchange = (e) => {
        if (fname.value == "") {
            fname.className = "input is-danger"
            fname_Error.innerText = "Empty Field"
            setButton(0, true)
            return
        }
        if (/^[a-zA-Z]+$/.test(fname.value))
        {
            fname.className = "input is-link"
            fname_Error.innerText = ""
            setButton(0, false)
        }
        else
        {
            fname.className = "input is-danger"
            fname_Error.innerText = "Invalid Name"
            setButton(0, true)
        }
    }

    lname.onchange = (e) => {
        if (lname.value == "") {
            lname.className = "input is-danger"
            lname_Error.innerText = "Empty Field"
            setButton(1, true)
            return
        }
        if (/^[a-zA-Z]+$/.test(lname.value)) {
            lname.className = "input is-link"
            lname_Error.innerText = ""
            setButton(1, false)

        }
        else {
            lname.className = "input is-danger"
            lname_Error.innerText = "Invalid Name"
            setButton(1, true)
        }
    }

    Email.onchange = (e) => {
        if (Email.value == "") {
            Email.className = "input is-danger"
            Email_Error.innerText = "Empty Field"
            setButton(2, true)
            return
        }
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(Email.value)) {
            Email.className = "input is-link"
            Email_Error.innerText = ""
            setButton(2, false)
        }
        else {
            Email.className = "input is-danger"
            Email_Error.innerText = "Invalid Email"
            setButton(2, true)
        }
    }

    Password.onchange = (e) => {
        var inner_check = true

        if (Password.value.length < 12) {
            Password_Error_1_Button.className = "input button is-danger"
            Password_Error_1_Icon.style = "color:white"
            setButton(3, true)
            inner_check = false
        }
        else
        {
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
            setButton(3, true)
            inner_check = false
        }

        if (/\d/.test(Password.value)) {
            Password_Error_3_Button.className = "input button is-success"
            Password_Error_3_Icon.style = "color:white"
        }
        else {
            Password_Error_3_Button.className = "input button is-danger"
            Password_Error_3_Icon.style = "color:white"
            setButton(3, true)
            inner_check = false
        }

        if (/[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(Password.value)) {
            Password_Error_4_Button.className = "input button is-success"
            Password_Error_4_Icon.style = "color:white"
        }
        else {
            Password_Error_4_Button.className = "input button is-danger"
            Password_Error_4_Icon.style = "color:white"
            setButton(3, true)
            inner_check = false
        }

        if (inner_check)
        {
            setButton(3, false)
        }
    }

    dob.onchange = (e) => {
        console.log(dob.value)
        if (dob.value == "") {
            dob.className = "input is-danger"
            dob_Error.innerText = "Invalid Date"
            setButton(4, true)
        }
        if (/\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*/g.test(dob.value))
        {
            dob.className = "input is-info"
            dob_Error.innerText = ""
            setButton(4, false)
        }
        else {
            dob.className = "input is-danger"
            dob_Error.innerText = "Invalid Date"
            setButton(4, true)
        }
    }

    cc.onchange = (e) => {

        // Different Credit Card Formats
        var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        var mastercardRegEx = /^(?:5[1-5][0-9]{14})$/;
        var amexpRegEx = /^(?:3[47][0-9]{13})$/;
        var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;

        if (cc.value == "")
        {
            cc.className = "input is-danger"
            cc_Error.innerText = "Empty Field"
            setButton(5, true)
            return
        }
        if (
            visaRegEx.test(cc.value) ||
            mastercardRegEx.test(cc.value) ||
            amexpRegEx.test(cc.value) ||
            discovRegEx.test(cc.value)
        )
        {
            cc.className = "input is-warning"
            cc_Error.innerText = ""
            setButton(5, false)
        }
        else
        {
            cc.className = "input is-danger"
            cc_Error.innerText = "Invalid Credit Card"
            setButton(5, true)
        }
    }

    button.disabled = true

    if (fname.value != "")
        fname.onchange()
    if (lname.value != "")
        lname.onchange()
    if (Email.value != "")
        Email.onchange()
    if (Password.value != "")
        Password.onchange()
    if (dob.value != "")
        dob.onchange()
    if (cc.value != "")
        cc.onchange()
}

window.onload = () => {
    document.getElementById("profile_pfp").onchange = (e) => {
        var file = e.target.files[0];
        var reader = new FileReader();
        reader.onload = (e) => {
            document.getElementById("preview_pfp").src = e.target.result;
        }
        reader.readAsDataURL(file)
    };
    validateForm()
}