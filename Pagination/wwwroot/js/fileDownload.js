function saveAsFile(fileName, content) {
    var link = document.createElement('a');
    link.href = 'data:application/octet-stream;base64,' + content;
    link.download = fileName;
    link.click();
}
