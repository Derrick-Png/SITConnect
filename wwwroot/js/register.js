window.onload = () => {
    document.getElementById("profile_pfp").onchange = (e) => {
        var file = e.target.files[0];
        var reader = new FileReader();
        reader.onload = (e) => {
            document.getElementById("preview_pfp").src = e.target.result;
        }
        reader.readAsDataURL(file)
    };
}