export function getBlobFromPdf(base64Pdf) {
    var file = new Blob([base64Pdf], { type: "application/pdf" });
    var url = URL.createObjectURL(file);
    return url;
}
//# sourceMappingURL=WebPdfViewer.js.map