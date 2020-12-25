declare const $: any;

export function Open(el: Element) {
    $(el).modal("show");
}

export function Close(el: Element) {
    $(el).modal("hide");
}
