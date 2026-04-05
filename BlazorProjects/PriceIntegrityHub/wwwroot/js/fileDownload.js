window.downloadFile = function (filename, bytesBase64) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.resetFileInput = function (inputId) {
    const input = document.getElementById(inputId);
    if (input) {
        input.value = '';
    }
}

window.triggerFileInput = function (inputId) {
    const input = document.getElementById(inputId);
    if (input) {
        input.click();
    }
}
