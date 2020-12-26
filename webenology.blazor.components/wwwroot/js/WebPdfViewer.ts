export function getBlobFromPdf(base64Pdf: string): string {

    const file = new Blob([base64Pdf], { type: "application/pdf" });

    const url = URL.createObjectURL(file);

    return url;
}