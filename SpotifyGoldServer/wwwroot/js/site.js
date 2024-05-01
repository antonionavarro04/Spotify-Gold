﻿window.onload = function () {

}

function download() {
    inpElement = document.querySelector('input[type="text"]');

    const url = "https://spotifygold.azurewebsites.net/api/yt/";
    // const url = "http://localhost:5244/api/id/";

    const quality = document.querySelector("select").value
    let id;

    if (inpElement.value === "") {
        alert("Please enter a valid URL");
        return;
    } else if (inpElement.value.includes("youtu.be")) {
        id = inpElement.value.split("/")[3];
        id = id.split("?")[0];
    } else if (inpElement.value.includes("youtube.com")) {
        id = inpElement.value.split("v=")[1];
        id = id.split("&")[0];
    } else {
        alert("Please enter a valid URL");
        return;
    }

    // Redirect the client to the url + id
    window.location.href = `${url}${id}/download?q=${quality}&appendMetadata=false`;
}
