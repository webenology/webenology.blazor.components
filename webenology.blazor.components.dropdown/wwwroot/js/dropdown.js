
export function PreventEnterKey(el) {
    const input = el.querySelector("input");
    input.addEventListener("keydown", function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
}

export function ScrollToActive(el, behavior) {
    const item = el.nextElementSibling.querySelector(".item.selected");
    if (item != null) {
        var bounding = item.offsetTop;
        el.nextElementSibling.scrollTo({ top: bounding, behavior: behavior });
    }
}

export function CursorAtEnd(el) {
    var input = el.querySelector("input[type=text]");
    var len = input.value.length;
    input.setSelectionRange(len, len);
}