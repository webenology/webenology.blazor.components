export function selectText(e: HTMLInputElement) {
    e.setSelectionRange(0, e.value.length);
}