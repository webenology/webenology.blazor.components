
export function PreventEnterKey(el) {
    const input = el.querySelector("input");
    input.addEventListener("keydown", function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
}

export function ScrollToActive(el, index, behavior) {
    var item = el.nextElementSibling.querySelector(".item");
    var bounding = 29.67 * index;
    if (item != null) {
        var height = item.getBoundingClientRect().height;
        bounding = height * index;
        console.log(item);
    }
    console.log(index, bounding);
    el.nextElementSibling.scrollTo({ top: bounding, behavior: behavior });
}

export function CursorAtEnd(el) {
    var input = el.querySelector("input[type=text]");
    var len = input.value.length;
    input.setSelectionRange(len, len);
}