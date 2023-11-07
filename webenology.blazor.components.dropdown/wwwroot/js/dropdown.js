export function Register(el, ref) {
    document.addEventListener("click", (e) => onClickReg(e, el, ref));
    ScrollToActive(el);
}

export function UnRegister(el, ref) {
    document.removeEventListener("click", (e) => onClickReg(e, el, ref));
}

export function ScrollToActive(el, behavior) {
    const item = el.querySelector(".drop-down-body .item.selected");
    const body = el.querySelector(".drop-down-body");
    if (item != null) {
        var bounding = item.offsetTop;
        body.scrollTo({ top: bounding, behavior: behavior });
    }
}

export function CursorAtEnd(el) {
    const input = el.querySelector("input[type='text']");
    var len = input.value.length;
    input.setSelectionRange(len, len);
}

function onClickReg(e, el, ref) {
    let isInside = false;
    const path = e.composedPath();
    for (let index in path) {
        if (path[index] === el) {
            isInside = true;
            break;
        }
    }
    if (isInside) {
        ref.invokeMethodAsync("OnInsideClick");
    } else {
        ref.invokeMethodAsync("OnOutsideClick");
    }
}